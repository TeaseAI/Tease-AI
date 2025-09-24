Imports AForge.Video
Imports AForge.Video.DirectShow
Imports System.Xml
Imports System.IO
Imports System.Drawing.Imaging

Public Class FormMarkusWebcam

    Dim webcamMode As String
    Dim myVideoSourceInt As Int32
    Dim myVideoSourceString As String

    Dim videoSource As VideoCaptureDevice

    Private Sub FormMarkusWebcam_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        placeWindow()
        Me.Text = ""
        PictureBox1.Dock = DockStyle.Fill
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.BackColor = Color.Black

        Try
            myVideoSourceString = loadVarString_Videosource("videosource")
        Catch
            myVideoSourceString = "0"
            'Label1.Text = "Error: Cannot load from file 'videosource.xml'"
        End Try

        myVideoSourceInt = Convert.ToInt32(myVideoSourceString)

        webCamStart()

    End Sub

    Private Sub placeWindow()
        Dim countScreens As Int32
        Dim countScreensString As String

        countScreens = Screen.AllScreens.Length
        countScreensString = Convert.ToString(countScreens)
        'Label1.Text = countScreensString + vbCrLf ' "\n"   ->    vbCRLF

        If countScreens > 1 Then
            Dim screen1width = Screen.AllScreens(0).Bounds.Width
            Me.Left = screen1width + 50
        End If

    End Sub

    Public Function loadVarString_Videosource(ByVal Variabel As String) As String
        Dim Filename_Settings As String = ("videosource.xml")
        Dim doc As XmlDocument = New XmlDocument
        doc.Load(Filename_Settings)
        Dim node As XmlNode = doc.SelectSingleNode(("//" + Variabel))
        Dim result As String = node.InnerText
        Return result
    End Function

    Private Sub videoSource_NewFrame(ByVal sender As Object, ByVal eventArgs As AForge.Video.NewFrameEventArgs)
        'Cast each incoming object as a bitmap and assign it to the picturebox
        '(Remember the ".Clone()" to avoid access violations.)
        PictureBox1.Image = CType(eventArgs.Frame.Clone, Bitmap)
    End Sub

    Public Sub webCamStart()
        'Create a list of all video sources. (Here, in addition to the desired webcam, TV cards, etc. also appear)
        Dim videosources As FilterInfoCollection = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        'Check if at least one recording source is available
        If (Not (videosources) Is Nothing) Then
            'Bind the first recording source to our webcam object
            '(If you have several sources, the first source does not always have to be the desired webcam!)

            Try
                videoSource = New VideoCaptureDevice(videosources(myVideoSourceInt).MonikerString)
            Catch ex As Exception
                MsgBox("No webcam detected." & vbCrLf & ex.Message)
                Me.Close()
                Return
            End Try

            Try
                'Check whether the recording source provides a list of possible recording resolutions.
                If (videoSource.VideoCapabilities.Length > 0) Then
                    Dim highestSolution As String = "0;0"
                    'Find the profile with the highest resolution
                    Dim i As Integer = 0
                    Do While (i < videoSource.VideoCapabilities.Length)
                        If (videoSource.VideoCapabilities(i).FrameSize.Width > Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(0))) Then
                            highestSolution = (videoSource.VideoCapabilities(i).FrameSize.Width.ToString + (";" + i.ToString))
                        End If

                        i = (i + 1)
                    Loop

                    'Pass the determined resolution to the webcam object
                    videoSource.DesiredFrameSize = videoSource.VideoCapabilities(Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(1))).FrameSize
                End If

            Catch ex As Exception

            End Try

            'Create and assign a NewFrame event handler.
            '(This registers every new frame of the webcam)
            AddHandler videoSource.NewFrame, AddressOf Me.videoSource_NewFrame
            'Activate the recording device
            videoSource.Start()
        End If

    End Sub

    Public Sub webCamStop()
        If ((Not (videoSource) Is Nothing) _
                    AndAlso videoSource.IsRunning) Then
            videoSource.SignalToStop()
            videoSource = Nothing
        End If

    End Sub

    Private Sub FormMarkusWebcam_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        webCamStop()
    End Sub
End Class

Class MyWebcam

    Private videoSource As VideoCaptureDevice
    Public picBoxToUse As PictureBox
    Public snapshotShouldBeTaken As Boolean
    Private webcamStartTimer As System.Windows.Forms.Timer = New Timer
    Private snapshot As Bitmap
    Public pathToUse As String
    Public pathFileMode As Boolean
    Private myVideoSourceString As String
    Private myVideoSourceInt As Integer

    Private Sub videoSource_NewFrame(ByVal sender As Object, ByVal eventArgs As AForge.Video.NewFrameEventArgs)
        'Cast each incoming object as a bitmap and assign it to the picturebox
        '(Remember the ".Clone()" to avoid access violations.)
        If (Me.snapshotShouldBeTaken = False) Then
            Try
                Me.picBoxToUse.Image = CType(eventArgs.Frame.Clone, Bitmap)
            Catch
            End Try

        Else
            Me.snapshot = CType(eventArgs.Frame.Clone, Bitmap)
        End If

    End Sub

    Private Sub webcamStartTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Me.webcamStartTimer.Stop()
        ' @ShowImage[av_imagens\Games\Numbers\1.jpg]
        ' @ShowImage[av_imagens\02\*.jpg]
        ' @WebcamSnapshotFolder[snapshot test folder\test01] ... there would then be the name according to date and time
        ' @WebcamSnapshotFile[snapshot test folder\test01\01.jpg]
        ' mw.pathToUse = "snapshot test folder\test01\01.jpg";
        Dim pathPart1 As String = Application.StartupPath
        Dim pathPart2 As String = "\Images\"
        Dim folders As String = (pathPart1 _
                    + (pathPart2 + Me.pathToUse))

        If (Me.pathFileMode = False) Then
            If (Directory.Exists(folders) = False) Then
                Directory.CreateDirectory(folders)
            End If

            Dim newDate As DateTime = DateTime.Now

            Dim myDate As String = newDate.ToString("yyyy_MM_dd___H_mm_ss")

            Try
                Dim g As Graphics = Graphics.FromImage(Me.snapshot)
                'g.CopyFromScreen(0, 0, 0, 0, GlobalVar.snapshot.Size);
                'g.Dispose();
                Dim jgpEncoder As ImageCodecInfo = Me.GetEncoder(ImageFormat.Jpeg)
                Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality
                Dim myEncoderParameters As EncoderParameters = New EncoderParameters(1)
                Dim myEncoderParameter As EncoderParameter = New EncoderParameter(myEncoder, 95L)
                ' 80L is the quality, 80%
                myEncoderParameters.Param(0) = myEncoderParameter
                Dim SSFileNameFromDate As String = folders + "\" + "Webcamshot_" + myDate + ".jpg"

                Me.snapshot.Save(SSFileNameFromDate, jgpEncoder, myEncoderParameters)
            Catch
                MessageBox.Show("Error while saving snapshot while pathFileMode is false.")

            End Try

        Else
            Dim justThePath As String = Path.GetDirectoryName(folders)

            Dim andTheFileName As String = Path.GetFileName(folders)

            If (Directory.Exists(justThePath) = False) Then
                Directory.CreateDirectory(justThePath)
            End If

            Try
                Dim g As Graphics = Graphics.FromImage(Me.snapshot)
                'g.CopyFromScreen(0, 0, 0, 0, GlobalVar.snapshot.Size);
                'g.Dispose();
                Dim jgpEncoder As ImageCodecInfo = Me.GetEncoder(ImageFormat.Jpeg)
                Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality
                Dim myEncoderParameters As EncoderParameters = New EncoderParameters(1)
                Dim myEncoderParameter As EncoderParameter = New EncoderParameter(myEncoder, 95L)
                ' 80L is the quality, 80%
                myEncoderParameters.Param(0) = myEncoderParameter
                Dim SSFileNameFromDate As String = (justThePath + ("\" + andTheFileName))
                Me.snapshot.Save(SSFileNameFromDate, jgpEncoder, myEncoderParameters)
            Catch
                MessageBox.Show("Error while saving snapshot while pathFileMode is true.")
            End Try

        End If

        Me.webCamStop()
    End Sub

    Private Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo
        Dim codecs() As ImageCodecInfo = ImageCodecInfo.GetImageDecoders
        For Each codec As ImageCodecInfo In codecs
            If (codec.FormatID = format.Guid) Then
                Return codec
            End If

        Next
        Return Nothing
    End Function

    Public Function loadVarString_Videosource(ByVal Variabel As String) As String
        Dim Filename_Settings As String = "videosource.xml"
        Dim doc As XmlDocument = New XmlDocument
        doc.Load(Filename_Settings)

        Dim node As XmlNode = doc.SelectSingleNode(("//" + Variabel))
        Dim result As String = node.InnerText
        Return result
    End Function

    Public Sub webCamStart()

        Try
            Me.myVideoSourceString = Me.loadVarString_Videosource("videosource")
        Catch
            Me.myVideoSourceString = "0"
        End Try

        Me.myVideoSourceInt = Convert.ToInt32(Me.myVideoSourceString)
        'Create a list of all video sources. (Here, in addition to the desired webcam, TV cards, etc. also appear)
        Dim videosources As FilterInfoCollection = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        'Check if at least one recording source is available
        If (Not (videosources) Is Nothing) Then
            'Bind the first recording source to our webcam object
            '(If you have several sources, the first source does not always have to be the desired webcam!)
            Try
                Me.videoSource = New VideoCaptureDevice(videosources(Me.myVideoSourceInt).MonikerString)
            Catch
                MessageBox.Show("No webcam detected." + vbCrLf + vbCrLf + "If you have a working webcam connected to your computer" + vbCrLf + "and still get this error message then you could try to" + vbCrLf + "The reason for this could be that you have more than one" + vbCrLf + "videocaputre devices connected to your computer.")
            End Try

            Try
                'Check whether the recording source provides a list of possible recording resolutions.
                If (Me.videoSource.VideoCapabilities.Length > 0) Then
                    Dim highestSolution As String = "0;0"
                    'Find the profile with the highest resolution
                    Dim i As Integer = 0
                    Do While (i < Me.videoSource.VideoCapabilities.Length)
                        If (Me.videoSource.VideoCapabilities(i).FrameSize.Width > Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(0))) Then
                            highestSolution = (Me.videoSource.VideoCapabilities(i).FrameSize.Width.ToString + (";" + i.ToString))
                        End If

                        i = (i + 1)
                    Loop

                    'Pass the determined resolution to the webcam object
                    Me.videoSource.DesiredFrameSize = Me.videoSource.VideoCapabilities(Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(1))).FrameSize
                End If

            Catch

            End Try

            'Create and assign a NewFrame event handler.
            '(This registers every new frame of the webcam)
            Try
                AddHandler Me.videoSource.NewFrame, AddressOf Me.videoSource_NewFrame
                'Activate the recording device
                Me.videoSource.Start()
            Catch

                Return
            End Try

        End If

        If (Me.snapshotShouldBeTaken = True) Then

            Me.webcamStartTimer.Interval = 10000
            AddHandler Me.webcamStartTimer.Tick, AddressOf Me.webcamStartTimer_Tick
            Me.webcamStartTimer.Start()
        End If

    End Sub

    Public Sub webCamStop()
        If ((Not (Me.videoSource) Is Nothing) _
                    AndAlso Me.videoSource.IsRunning) Then
            Me.videoSource.SignalToStop()
            Me.videoSource = Nothing
        End If

    End Sub
End Class

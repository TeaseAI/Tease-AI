Imports System.Drawing.Imaging
Imports System.IO
Imports AForge.Video.DirectShow

Public Class MarWebcam
	Private WindowMode As Boolean

	Public videoSource As VideoCaptureDevice
	Public picBoxToUse As PictureBox
	Public snapshotShouldBeTaken As Boolean
	Public webcamStartTimer As Timer = New Timer
	Private snapshot As Bitmap
	Public pathToUse As String
	Public pathFileMode As Boolean
	Public Running As Boolean

	Public Sub New(windowMode As Boolean)
		windowMode = windowMode
	End Sub

	Private Sub videoSource_NewFrame(sender As Object, eventArgs As AForge.Video.NewFrameEventArgs)
		'Cast each incoming object as a bitmap and assign it to the picturebox
		'(Remember the ".Clone()" to avoid access violations.)
		Try
			snapshot = CType(eventArgs.Frame.Clone, Bitmap)
		Catch
		End Try
	End Sub

	Private Sub webcamStartTimer_Tick(sender As Object, e As EventArgs)
		webcamStartTimer.Stop()
		' @ShowImage[av_imagens\Games\Numbers\1.jpg]
		' @ShowImage[av_imagens\02\*.jpg]
		' @WebcamSnapshotFolder[snapshot test folder\test01] ... there would then be the name according to date and time
		' @WebcamSnapshotFile[snapshot test folder\test01\01.jpg]
		' mw.pathToUse = "snapshot test folder\test01\01.jpg";
		Dim folders As String = Application.StartupPath + "\Images\" + pathToUse

		Dim dir = Path.GetDirectoryName(folders)
		Directory.CreateDirectory(dir)

		If (pathFileMode = False) Then
			Try
				SnapImage(folders + "\" + "Webcamshot_" + DateTime.Now.ToString("yyyy_MM_dd___H_mm_ss") + ".jpg")
			Catch
				MessageBox.Show("Error while saving snapshot while pathFileMode is false.")
			End Try

		Else
			Try
				SnapImage(folders)
			Catch
				MessageBox.Show("Error while saving snapshot while pathFileMode is true.")
			End Try

		End If

		WebcamStop()
	End Sub

	Private Sub SnapImage(fileName As String)
		Dim g As Graphics = Graphics.FromImage(snapshot)
		Dim jgpEncoder As ImageCodecInfo = GetEncoder(ImageFormat.Jpeg)
		Dim myEncoder As Encoder = Encoder.Quality
		Dim myEncoderParameters As EncoderParameters = New EncoderParameters(1)
		Dim myEncoderParameter As EncoderParameter = New EncoderParameter(myEncoder, 95L)
		myEncoderParameters.Param(0) = myEncoderParameter

		snapshot.Save(fileName, jgpEncoder, myEncoderParameters)
	End Sub

	Public Sub WebcamStart()
		'Create a list of all video sources. (Here, in addition to the desired webcam, TV cards, etc. also appear)
		Dim videosources As FilterInfoCollection = New FilterInfoCollection(FilterCategory.VideoInputDevice)
		'Check if at least one recording source is available
		If videosources IsNot Nothing Then
			Dim currentDevice As FilterInfo = Nothing

			For Each source As FilterInfo In videosources
				If source.MonikerString = My.Settings.WebcamDevice Then
					currentDevice = source
					Exit For
				End If
			Next

			If currentDevice Is Nothing Then
				currentDevice = videosources(0)
			End If

			Try
				videoSource = New VideoCaptureDevice(currentDevice.MonikerString)
			Catch
				MessageBox.Show("No webcam detected." + vbCrLf + vbCrLf + "If you have a working webcam connected to your computer" + vbCrLf + "and still get this error message then you could try to" + vbCrLf + "The reason for this could be that you have more than one" + vbCrLf + "videocaputre devices connected to your computer.")
			End Try

			Try
				'Check whether the recording source provides a list of possible recording resolutions.
				If videoSource.VideoCapabilities.Length > 0 Then
					Dim highestSolution As String = "0;0"
					'Find the profile with the highest resolution
					For i As Integer = 0 To videoSource.VideoCapabilities.Length
						If (videoSource.VideoCapabilities(i).FrameSize.Width > Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(0))) Then
							highestSolution = (videoSource.VideoCapabilities(i).FrameSize.Width.ToString + (";" + i.ToString))
						End If
					Next

					'Pass the determined resolution to the webcam object
					videoSource.DesiredFrameSize = videoSource.VideoCapabilities(Convert.ToInt32(highestSolution.Split(Microsoft.VisualBasic.ChrW(59))(1))).FrameSize
				End If
			Catch
			End Try

			Running = True

			If Not WindowMode Then
				'Create and assign a NewFrame event handler.
				'(This registers every new frame of the webcam)
				Try
					AddHandler videoSource.NewFrame, AddressOf videoSource_NewFrame
					'Activate the recording device
					videoSource.Start()
				Catch
					Return
				End Try

				If snapshotShouldBeTaken = True Then
					webcamStartTimer.Interval = 10000
					AddHandler webcamStartTimer.Tick, AddressOf webcamStartTimer_Tick
					webcamStartTimer.Start()
				End If
			End If
		End If
	End Sub

	Public Sub WebcamStop()
		Running = False
		If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
			If Not WindowMode Then RemoveHandler videoSource.NewFrame, AddressOf videoSource_NewFrame
			videoSource.SignalToStop()
			videoSource = Nothing
		End If
	End Sub

	Private Function GetEncoder(format As ImageFormat) As ImageCodecInfo
		Dim codecs() As ImageCodecInfo = ImageCodecInfo.GetImageDecoders
		For Each codec As ImageCodecInfo In codecs
			If (codec.FormatID = format.Guid) Then
				Return codec
			End If
		Next
		Return Nothing
	End Function
End Class

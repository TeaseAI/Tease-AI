Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class FrmWebcam

	Dim Webcam As MarWebcam = New MarWebcam(True)

	Private Sub FormMarkusWebcam_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Dim videosources As FilterInfoCollection = New FilterInfoCollection(FilterCategory.VideoInputDevice)

		If videosources IsNot Nothing Then
			For Each source As FilterInfo In videosources
				videoDevices.Items.Add(source)
				If source.MonikerString = My.Settings.WebcamDevice Then videoDevices.SelectedItem = source
			Next

			If videoDevices.SelectedItem Is Nothing Then videoDevices.SelectedIndex = 0

			StartWebcam()
		End If
	End Sub

	Private Sub videoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
		'Cast each incoming object as a bitmap and assign it to the picturebox
		'(Remember the ".Clone()" to avoid access violations.)
		Try
			PictureBox1.Image = CType(eventArgs.Frame.Clone, Bitmap)
		Catch
		End Try
	End Sub

	Private Sub FormMarkusWebcam_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
		StopWebcam()
	End Sub

	Private Sub StartWebcam()
		Webcam.WebcamStart()
		AddHandler Webcam.videoSource.NewFrame, AddressOf videoSource_NewFrame
		Webcam.videoSource.Start()
	End Sub


	Private Sub StopWebcam()
		If Not Webcam.Running Then Return

		RemoveHandler Webcam.videoSource.NewFrame, AddressOf videoSource_NewFrame
		Webcam.WebcamStop()
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		If Not Webcam.Running OrElse videoDevices.SelectedItem Is Nothing Then Return

		StopWebcam()
		PictureBox1.Image = Nothing
		Dim item As FilterInfo = videoDevices.SelectedItem
		My.Settings.WebcamDevice = item.MonikerString
		My.Settings.Save()
		StartWebcam()
	End Sub
End Class
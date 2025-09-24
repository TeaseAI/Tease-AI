Imports System.ComponentModel
Imports System.IO
Imports System.Media
Imports System.Threading

Public Class MarMetronome
	'credit to markus, originally metro.dll in Fury
	Private ExamplePlayer As SoundPlayer = New SoundPlayer()

	Private backgroundWorker1 As BackgroundWorker = New BackgroundWorker()

	Private runMetro As Boolean

	Private metroInterval As Integer = 1000

	Private thereShouldBeAlimit As Boolean

	Private metroBeatCounter As Integer

	Private metroBeatLimit As Integer

	Private pausePossible As Boolean

	Private _currentBPM As Integer

	Private Property currentBPM As Integer
		Get
			Return _currentBPM
		End Get
		Set(value As Integer)
			If value > 360 Then
				_currentBPM = 360
			Else
				If value < 1 Then
					_currentBPM = 1
				Else
					_currentBPM = value
				End If
			End If
		End Set
	End Property

	Public Sub New()
		Dim wavFilepath As String = Application.StartupPath & "\Audio\System\metronome.wav"
		Dim wavStream As MemoryStream

		AddHandler backgroundWorker1.DoWork, AddressOf backgroundWorker1_DoWork
		backgroundWorker1.WorkerSupportsCancellation = True
		If Directory.Exists(Path.GetDirectoryName(wavFilepath)) AndAlso File.Exists(wavFilepath) Then
			wavStream = New MemoryStream(File.ReadAllBytes(wavFilepath))
			ExamplePlayer.Stream = wavStream
			ExamplePlayer.Load()
		End If
		_currentBPM = 60
	End Sub

	Private Sub backgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs)
		pausePossible = True
		While True
			If thereShouldBeAlimit Then
				metroBeatCounter += 1
				Console.WriteLine("metroBeatCounter: " & metroBeatCounter & " / " & metroBeatLimit)
				If metroBeatCounter >= metroBeatLimit Then
					MetroOff()
					Exit While
				End If
			End If
			ExamplePlayer.Play()
			Thread.Sleep(metroInterval)
			If Not runMetro Then
				Return
			End If
		End While

	End Sub

	Public Sub TogglePauseMetro()
		If pausePossible Then
			runMetro = False
			backgroundWorker1.CancelAsync()
		Else
			If runMetro AndAlso Not backgroundWorker1.IsBusy Then
				runMetro = True
				backgroundWorker1.RunWorkerAsync()
			End If
		End If
	End Sub

	Public Sub LimitMetro(countBeats As Integer)
		thereShouldBeAlimit = True
		metroBeatLimit = countBeats
	End Sub

	Private Function BPMtoMilliseconds(bpm As Integer) As Integer
		Return Convert.ToInt32(Math.Round(60000D / bpm))
	End Function

	Public Sub MetroOn(bpm As Integer)
		metroBeatCounter = 0
		UpdateBPM(bpm)
	End Sub

	Public Sub MetroOff()
		pausePossible = False
		metroBeatCounter = 0
		thereShouldBeAlimit = False
		metroBeatLimit = 0
		runMetro = False
		backgroundWorker1.CancelAsync()
		OutputTest()
	End Sub

	Public Sub MetroUp(addBpm As Integer)
		UpdateBPM(currentBPM + addBpm)
	End Sub

	Public Sub MetroDown(subBpm As Integer)
		UpdateBPM(currentBPM - subBpm)
	End Sub

	Private Sub OutputTest()
		Console.WriteLine("metroInterval: " + metroInterval.ToString())
		Console.WriteLine("BPM: " + currentBPM.ToString() + vbLf)
	End Sub

	Private Sub UpdateBPM(bpm As Integer)
		currentBPM = bpm
		metroInterval = BPMtoMilliseconds(currentBPM)
		If Not runMetro Then
			runMetro = True
			metroBeatCounter = 0
			backgroundWorker1.RunWorkerAsync()
		End If
		OutputTest()
	End Sub
End Class

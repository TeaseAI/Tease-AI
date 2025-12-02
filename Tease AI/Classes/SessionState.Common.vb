Partial Public Class SessionState
	''' <summary>
	''' Sets the stroke taunt tick value to a new one based on the user's settings
	''' </summary>
	''' <param name="initial">if this is the start of a taunt cycle, will limit the first taunt from showing too fast and bottlenecking</param>
	Public Sub UpdateStrokeTaunts(initial As Boolean)
		If My.Settings.TimerSTF = 1 Then StrokeTauntTick = randomizer.Next(120, 241)
		If My.Settings.TimerSTF = 2 Then StrokeTauntTick = randomizer.Next(75, 121)
		If My.Settings.TimerSTF = 3 Then StrokeTauntTick = randomizer.Next(45, 76)
		If My.Settings.TimerSTF = 4 Then StrokeTauntTick = randomizer.Next(25, 46)
		If My.Settings.TimerSTF = 5 Then StrokeTauntTick = randomizer.Next(15, 26)

		If initial And StrokeTauntTick < 20 Then StrokeTauntTick = 20
	End Sub

	''' <summary>
	''' Sets the edge taunt tick value to a new one based on the user's settings
	''' </summary>
	''' <param name="initial">if this is the start of a taunt cycle, will limit the first taunt from showing too fast and bottlenecking</param>
	Public Sub UpdateEdgeTaunts(initial As Boolean)
		If My.Settings.TimerETF = 1 Then EdgeTauntInt = randomizer.Next(50, 76)
		If My.Settings.TimerETF = 2 Then EdgeTauntInt = randomizer.Next(40, 51)
		If My.Settings.TimerETF = 3 Then EdgeTauntInt = randomizer.Next(30, 41)
		If My.Settings.TimerETF = 4 Then EdgeTauntInt = randomizer.Next(20, 31)
		If My.Settings.TimerETF = 5 Then EdgeTauntInt = randomizer.Next(5, 21)

		If initial And EdgeTauntInt < 15 Then EdgeTauntInt = 15
	End Sub

	Public Function GetGeneralTime() As String
		If Now.Hour > 3 And Now.Hour < 12 Then
			Return "Morning"
		ElseIf Now.Hour > 11 And Now.Hour < 18 Then
			Return "Afternoon"
		Else
			Return "Night"
		End If
	End Function

End Class

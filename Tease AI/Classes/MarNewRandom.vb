<Serializable()>
Public Class MarNewRandom
	'credit to markus
	Private myRandom As Random

	Public Sub New(Optional Seed As Integer = -1)
		If Seed = -1 Then
			myRandom = New Random(Guid.NewGuid().GetHashCode())
		End If
	End Sub

	Public Function [Next](lowerLimitInclusive As Integer, upperLimitExclusive As Integer) As Integer
		Dim myValue As Integer
		If lowerLimitInclusive <= upperLimitExclusive Then
			myValue = myRandom.[Next](lowerLimitInclusive, upperLimitExclusive)
		Else
			myValue = 5
			MsgBox("Error occurred while calling the random function:" & vbCrLf & "Min value was bigger than max value, to prevent a crash, the return value is 5.", MsgBoxStyle.OkOnly, Nothing)
		End If
		Return myValue
	End Function
End Class

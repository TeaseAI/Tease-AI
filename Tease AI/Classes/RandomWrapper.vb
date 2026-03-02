' Wraps a Random object to simplify init with a random seed, and attempts to fix reversed numbers in a range to prevent an exception due to a script typo.
' Originally MarNewRandom from markus/Fury decomp but enough has changed to make this separate.
<Serializable()>
Public Class RandomWrapper
	Private rnd As Random

	Public Sub New(Optional Seed As Integer = -1)
		If Seed = -1 Then
			rnd = New Random(Guid.NewGuid().GetHashCode() And &H7FFFFFFF)
		Else
			rnd = New Random(Seed)
		End If
	End Sub

	Public Function [Next](lowerLimitInclusive As Integer, upperLimitExclusive As Integer) As Integer
		Dim lo = Math.Min(lowerLimitInclusive, upperLimitExclusive)
		Dim hi = Math.Max(lowerLimitInclusive, upperLimitExclusive)
		Return rnd.Next(lo, hi)
	End Function

	Public Function [Next](upperLimitExclusive As Integer) As Integer
		Return rnd.Next(upperLimitExclusive)
	End Function

	Public Function [Next]() As Integer
		Return rnd.Next()
	End Function
End Class

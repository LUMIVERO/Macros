Sub CitaviWordCount()
Dim Ctl As ContentControl, d As Long, t As Long
With ActiveDocument
  For Each Ctl In .ContentControls
    With Ctl
      t = t + .Range.ComputeStatistics(wdStatisticWords)
    End With
  Next
  d = .Range.ComputeStatistics(wdStatisticWords)
End With
MsgBox "There are:" & vbCr & _
  d & " words in the document body, including" & vbCr & _
  t & " words in content controls (Citavi fields)." & vbCr & vbCr & "There are" & vbCr & _
  d - t & " words in the document body, excluding content controls (Citavi fields)."
End Sub


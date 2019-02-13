Sub CitaviReferenceHyperlinkYear()
    ActiveDocument.UpdateStylesOnOpen = False
    
    Dim ctl As ContentControl

    For Each ctl In ActiveDocument.ContentControls
        If InStr(1, ctl.Tag, "CitaviPlaceholder") = 1 Then
              For i = 1 To ctl.Range.Words.Count
                If ctl.Range.Words(i) Like "####" Then
                    ctl.Range.Words(i).Style = ActiveDocument.Styles(wdStyleHyperlink)
                End If
             Next
        End If
    Next
End Sub

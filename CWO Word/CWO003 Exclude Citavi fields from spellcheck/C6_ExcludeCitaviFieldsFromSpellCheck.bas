Sub CitaviFieldsNoProofing()
    Dim oStyle As Style
    ActiveDocument.UpdateStylesOnOpen = False

    Dim ctl As ContentControl

    For Each ctl In ActiveDocument.ContentControls
        If InStr(1, ctl.Tag, "CitaviPlaceholder") = 1 Then
            ctl.Range.NoProofing = True
        End If
    Next
End Sub

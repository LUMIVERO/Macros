Sub CitaviReferenceHyperlink()
    Dim oStyle As Style
    ActiveDocument.UpdateStylesOnOpen = False

    Dim ctl As ContentControl

    For Each ctl In ActiveDocument.ContentControls
        If InStr(1, ctl.Tag, "CitaviPlaceholder") = 1 Then
            ctl.DefaultTextStyle = ActiveDocument.Styles(wdStyleHyperlink)
        End If
    Next
End Sub

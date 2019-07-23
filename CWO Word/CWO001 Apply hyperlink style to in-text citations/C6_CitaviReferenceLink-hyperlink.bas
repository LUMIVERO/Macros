Sub CitaviReferenceHyperlink()
    Dim oStyle As Style
    ActiveDocument.UpdateStylesOnOpen = False

    Dim ctl As ContentControl
    Dim ctl2 As ContentControl
    Dim fn As Footnote
    
    
    For Each ctl In ActiveDocument.ContentControls
        If InStr(1, ctl.Tag, "CitaviPlaceholder") = 1 Then
            ctl.DefaultTextStyle = ActiveDocument.Styles(wdStyleHyperlink)
        End If
    Next
    
    For Each fn In ActiveDocument.Footnotes
       For Each ctl2 In fn.Range.ContentControls
            If InStr(1, ctl2.Tag, "CitaviPlaceholder") = 1 Then
                 ctl2.DefaultTextStyle = ActiveDocument.Styles(wdStyleHyperlink)
            End If
       Next
    Next
    
End Sub

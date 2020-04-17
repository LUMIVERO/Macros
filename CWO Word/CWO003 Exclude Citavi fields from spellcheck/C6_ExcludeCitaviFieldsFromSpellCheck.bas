Sub CitaviFieldsNoProofingFullDoc()

    ' source: https://help.citavi.com/topic/deaktivieren-der-word-korrektur-in-quellennachweisen#comment-224602

    Dim oStyle As Style
    ActiveDocument.UpdateStylesOnOpen = False

    Dim doc As Document
    Set doc = ActiveDocument
    Dim ctl As ContentControl
    Dim rge As Range
    For Each rge In doc.StoryRanges
        For Each ctl In rge.ContentControls
            If InStr(1, ctl.Tag, "CitaviPlaceholder") = 1 Then
            ctl.Range.NoProofing = True
            End If
        Next
    Next
EndSub

# Ein Citavi 5-Projekt in das Citavi 4-Format rückkonvertieren

[[> English Version](readme.md)]

Sie haben ein Projekt in Citavi 5 erstellt. Dieses Projekt möchten Sie an jemanden weitergeben, der noch mit Citavi 4 arbeitet.

## Lösung
Das hier erhältliche Makro exportiert die Daten von Citavi 5 in eine XML-Datei, die Sie mit dem ebenfalls hier erhältlichen Makro in ein neues Citavi 4-Projekt importieren


## Download
[01 - für den Export aus Citavi 5](01_Export_XML_from_C5_Project.cs)

[02 - für den Import in Citavi 4](02_Import_XML_into_newly_created_C4_Project.cs)

## Anwendung

1. Starten Sie Citavi 5 und öffnen Sie das Projekt, dessen Daten Sie in ein Citavi 4-Projekt umwandeln möchten.
1. Drücken Sie `Alt+F11`, um den Makro-Editor zu starten oder wählen Sie *Extras > Makro-Editor*.
1. Öffnen Sie im Makro-Editor die Skript-Datei `01_Export_XML_from_C5_Project.cs` und führen Sie sie mit `F5` aus. Sie werden vom Skript aufgefordert, einen Namen für eine XML-Datei anzugeben (XML ist das Austauschformat zwischen C5 und C4). Wählen Sie z.B. *Dissertation.xml*. Nach erfolgten Export erscheint die Meldung "Finished".
1. Schließen Sie Citavi 5.
1. Installieren Sie Citavi 4, falls es noch nicht vorhanden ist. Sie erhalten Citavi 4 [hier](http://setup1.citavi.com/release/default/Citavi4Setup.exe).
1. Starten Sie Citavi 4 und öffnen Sie ein beliebiges Projekt.
1. Drücken Sie `Alt+F11`, um den Makro-Editor zu starten.
1. Öffnen Sie im Makro-Editor die Skript-Datei `02_Import_XML_into_newly_created_C4_project.cs` und führen Sie sie mit `F5` aus. Sie werden zunächst vom Skript aufgefordert, die zuvor gespeicherte XML-Datei auszuwählen und anschließend den Ort und den Namen einer neu zu erstellenden Citavi 4-Projektdatei. Nach erfolgtem Import erscheint die Meldung "Finished".
1. Schließen Sie den Makro-Editor und öffnen Sie nun das neu erstellte Citavi 4-Projekt mit den aus Citavi 5 übernommenen Daten.
1. Kopieren Sie den Inhalt des Ordners `Citavi 5\Projects\[Projektname]\Citavi Attachments` nach `Citavi 4\Projects[Projektname]\CitaviFiles`.

**WICHTIG:** Sollte es beim Import der XML-Daten in eine Citavi 4-Projektdatei zu einer Fehlermeldung kommen, starten Sie bitte Citavi 4 mit dem Parameter /noScratch (vgl. [Handbuch](https://www1.citavi.com/sub/manual4/de/program_start_options.html)).
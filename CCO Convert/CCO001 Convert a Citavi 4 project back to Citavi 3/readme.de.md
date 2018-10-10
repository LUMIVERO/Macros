# Ein Citavi 4-Projekt in das Citavi 3-Format rückkonvertieren

[[> English Version](readme.md)]

Sie haben ein Projekt in Citavi 4 erstellt. Dieses Projekt möchten Sie an jemanden weitergeben, der noch mit Citavi 3 arbeitet.

## Lösung
Die hier erhältlichen Makros exportieren die Daten von Citavi 4 in eine XML-Datei, die Sie mit dem ebenfalls hier erhältlichen Makro in ein neues Citavi 3-Projekt importieren.


## Download
[für den Export aus Citavi 4](01_Export_XML_from_C4_Project.cs)
[für den Import in Citavi 3](02_Import_XML_into_newly_created_C3_Project.cs)

## Anwendung

**Wichitg:** Um das Makro in Citavi 3 anzuwenden, benötigen Sie Citavi 3.4.1.2, das Sie [hier](http://www.citavi.com/sub/setup/citavi3beta/CitaviSetup.exe) zum Download erhalten.

Folgen Sie danach [dieser Anleitung](instructions_de.pdf).

Falls beim Ausführen des Makros der Fehler »Der angeforderte Wert "Omit" konnte nicht gefunden werden.« auftritt, öffnen Sie die aus Citavi 4 exportierte XML-Datei mit einem Texteditor. Führen Sie einen Suchen-Ersetzen-Durchlauf durch: 

Suche nach: `Omit`

Ersetzen durch: `Arabic`
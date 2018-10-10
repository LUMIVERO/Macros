# DOI-Adressen aus verknüpften PDF-Dateien importieren 

Ihnen liegt ein Projekt vor, in dem mehrere Titel keinen Eintrag im Feld DOI-Adresse haben. Die DOI-Adresse findet sich aber in verknüpften PDF-Dateien. Sie möchten aus den PDF-Dateien die jeweiligen DOI-Adressen beim zugehörigen Titel in Ihrem Citavi-Projekt ergänzen.

## Lösung
Wir stellen Ihnen ein Makro zur Verfügung, das alle verknüpften PDF-Dateien durchsucht. Ist am Titel, mit dem das PDF verknüpft ist, noch keine DOI vorhanden, wird innerhalb des PDFs danach gesucht. Wurde eine DOI gefunden, so wird diese beim Titel eingetragen.

## Download
[für Citavi 5](C5_Extract_DOIs_from_linked_PDFs.cs)

[für Citavi 4](C4_Extract_DOIs_from_linked_PDFs.cs)

## Anwendung
Folgen Sie der Anleitung in [diesem Artikel](\readme.de.md), um das Makro einzusetzen.

Bitte beachten Sie: Es gibt keine Fortschrittsanzeige. Auch wenn es so aussieht, als ob keine Aktion stattfindet, findet im Hintergrund die Recherche statt. Sobald alle Titel geprüft wurden, erscheint eine Meldung.

Anhand der DOI-Adresse oder einer PubMed-ID können Sie Titeldaten von Citavi ergänzen lassen. Standardmäßig geht das immer nur einen einzelnen Titel. Sie klicken dazu in das DOI- oder PubMed-Feld und drücken die Taste F9. 

Wenn Sie viele Titel ergänzen möchten, ist dieses Makro für Sie interessant: Titeldaten anhand DOI oder PubMed-ID automatisch ergänzen

## Autor
Jörg Pasch @joepasch
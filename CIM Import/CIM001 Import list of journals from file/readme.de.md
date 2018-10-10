# Zeitschriftenliste aus Datei inportieren

Ihnen liegt eine Textdatei mit Zeitschriftennamen und deren Abkürzungen vor. Diese Liste möchten Sie in Citavi importieren. Wenn Sie Zeitschriftennamen von PubMed importieren möchten, verwenden Sie stattdessen dieses Makro.

## Lösung
Wir stellen Ihnen hier ein Makro zur Verfügung, das strukturierte Textdateien importiert.

## Download
[für Citavi 6](C6_Import_Journals.cs)

[für Citavi 5](C5_Import_Journals.cs)

[für Citavi 4](C4_Import_Journals.cs)

## Anwendung
Folgen Sie der Anleitung in [diesem Artikel](\readme.de.md), um das Makro einzusetzen.

Die zu importierende Zeitschriftenliste muss als reine Textdatei vorliegen und sollte folgende Struktur haben: 

* Je Zeile ist nur eine Zeitschrift aufgeführt.
* Jede Zeile enthält den vollständigen Namen, sowie bis zu 3 Abkürzungen und die ISSN mit Trennzeichenl
* Trennzeichen können sein: Tabulator, Semikolon, Gleichheitszeichen, Pipe-Zeichen.

Folgende Zeilen sind gültige Importdaten in diesem Sinne: 
* Langer Name; Abk.1
* Langer Name; Abk.1; Abk.2
* Langer Name; Abk.1; Abk.2; Abk.3
* Langer Name; Abk.1; Abk. 2; Abk.3; ISSN
* Langer Name;;;;ISSN


## Autor
Jörg Pasch @joepasch
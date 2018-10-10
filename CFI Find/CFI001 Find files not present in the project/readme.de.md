# Noch nicht erfasste Dateien finden

Sie haben in Ihrem Citavi-Projekt bereits viele Titel bibliographisch erfasst und die zugehörigen Volltexte in Form von PDF-Dateien mit den jeweiligen Titeln verknüpft. Nun finden Sie weitere PDF-Dateien, von denen Sie nicht wissen, ob Sie zu diesen schon die Titeldaten in Citavi erfasst haben.

## Lösung
Wir stellen Ihnen ein Makro zur Verfügung, das eine Textdatei mit den Namen aller PDF-Dateien erstellt, die noch nicht in Ihrem Citavi-Projekt erfasst wurden.

Sie werden zunächst aufgefordert, einen Ordner mit Dateien anzugeben, die mit den aus dem aktuell geöffneten Projekt verlinkten Dateien verglichen werden. Das sind sowohl elektronische Standorte als auch Datei-Wissenselemente.

Anschließend erzeugt das Makro vier Textdateien im Ordner des aktuellen Projektes, wobei die letztgenannte Datei für Sie die entscheidende ist:

1. ALL_Files_LINKED_From_Project.txt
 
    Das ist eine alphabetisch sortierte, eindeutige Liste aller Dateien, die mit dem aktuellen Projekt in irgendeiner Weise verknüpft sind (CitaviFiles oder andere, alle Pfadangaben absolut).

1. Particular_Files_Checked_For_Link_From_Project.txt
 
    Das ist eine alphabetisch sortierte Liste aller Dateien aus dem von Ihnen angegebenen Vergleichsordner.

1. Particular_Files_LINKED_From_Project.txt

    Das ist eine alphabetisch sortierte Liste aller Dateien aus Particular_Files_Checked_For_Link_From_Project.txt, die auch in ALL_Files_LINKED_From_Project.txt vorkommen (Schnittmenge).

1. Particular_Files_NOT_LINKED_From_Project.txt

    Das ist eine alphabetisch sortierte Liste aller Dateien aus Particular_Files_Checked_For_Link_From_Project.txt, die NICHT in ALL_Files_LINKED_From_Project.txt vorkommen (Differenzmenge).

## Download
[für Citavi 5](C5_List_Linked_And_Not_Linked_Files.cs)

[für Citavi 4](C4_List_Linked_And_Not_Linked_Files.cs)


## Anwendung
Folgen Sie der Anleitung in [diesem Artikel](/readme.de.md), um das Makro einzusetzen.

## Autor
Jörg Pasch @joepasch
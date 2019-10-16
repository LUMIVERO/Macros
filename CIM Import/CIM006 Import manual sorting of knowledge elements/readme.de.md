# Manuelle Sortierung von Wissenselementen importieren

Sie haben über die Funktion **Titel in ein anderes Projekt verschieben/kopieren** Titel mitsamt ihren Zitaten und Kategorien aus einem Quellprojekt in ein Zielprojekt **kopiert**. Sie möchten im Zielprojekt die manuelle Sortierung der Wissenselemente unterhalb der Kategorien aus dem Quellprojekt übernehmen. 

## Lösung
Wir stellen Ihnen ein Makro zur Verfügung, das die Sortierinformationen aus dem Quellprojekt ausliest und die Zitate im Zielprojekt in dieselbe Reihenfolge bringt. Die aus dem Quellprojekt übernommenen Zitate werden dabei als Block unterhalb der bereits vorhandenen Zitate des Zielprojektes angeordnet.

## Download
[für Citavi 6](C6_Import_sorting.cs)

## Anwendung
Weil Sie die Aktion nicht rückgängig machen können, erstellen Sie zuerst eine Sicherungskopie Ihres Projekts:
- Für Cloud-Projekte:  **Datei** > **Dieses Projekt** > **Archivkopie lokal speichern**
- Für lokale Projekte: **Datei** > **Dieses Projekt** > **Sicherungskopie**
1. Kopieren Sie die gewünschten Titel mit der Funktion  **Titel in ein anderes Projekt verschieben/kopieren**, falls noch nicht geschehen. **WICHTIG**: Verwenden Sie **nicht** die Option **verschieben**, da dann die Zitate für die Ermittlung der ursprünglichen Reihenfolge im Quellprojekt nicht mehr zur Verfügung stehen.    
1. Wechseln Sie zum Zielprojekt, das nach dem Kopieren die neuen Titel, Kategorien und Zitate enthält.
1. Öffnen Sie im Makro-Editor [s. Handbuch](https://www1.citavi.com/sub/manual6/de/index.html?executing_macros.html) das hier bereitgestellte Makro. 
1. Klicken Sie auf **Ausführen**, um das Quellprojekt auszuwählen, aus dem Sie die Sortierung der Wissenselemente übernehmen möchten.

## Autor
Jörg Pasch @joepasch

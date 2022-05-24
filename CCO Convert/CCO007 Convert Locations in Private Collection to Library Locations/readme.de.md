# Standortnachweise vom Typ "Private Sammlung" in "Bibliothek" konvertieren

Sie haben für Ihre Literatur als Standortnachweis Ihre `Private Sammlung` hinterlegt. In der Liste der Bibliotheken (im Menü `Listen` > `Bibliotheken`) werden allerdings nur eben diese angezeigt, also keine Einträge, die Sie unter `Private Sammlung` vorgenommen haben.

Sie möchten nun daher stattdessen die Standorte lieber als `Bibliothek` erfassen.

## Lösung
Wir stellen Ihnen ein Makro zur Verfügung, das alle Standorte vom Typ `Private Sammlung` in einen Standortnachweis vom Typ `Bibliothek` konvertiert.

## Download
[für Citavi 6](CCO007_Convert_Locations_in_Private_Collection_to_Library_Locations.cs)

## Anwendung
In Zeile 24 können Sie mit einem Schalter festlegen, ob nach der Konvertierung der ursprüngliche Standortnachweis unter `Private Sammlung` gelöscht werden (`true`) oder erhalten bleiben soll (`false`).

Um nach allen Titeln zu suchen, die Teil Ihrer privaten Sammlung sind, nutzen Sie bitte die **Erweiterte Suche** und geben nach Auswahl des Feldes `Standorte (Private Sammlung)` den Wert `*` ein.
Drücken Sie dann die Tastenkombination <kbd>STRG</kbd>+<kbd>A</kbd>, um diejenigen Titel auszuwählen, welche das Makro berücksichtigen soll.
 
Folgen Sie der Anleitung in [unserem Handbuch](https://www1.citavi.com/sub/manual6/de/index.html?executing_macros.html), um das Makro einzusetzen.

## Autor

* **Jörg Pasch** [joepasch](https://github.com/joepasch)

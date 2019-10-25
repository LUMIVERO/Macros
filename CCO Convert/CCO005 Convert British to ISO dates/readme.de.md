# Britische Datumseingaben korrigieren
Sie haben in den [Optionen von Citavi](https://www1.citavi.com/sub/manual6/de/index.html?formatting_tab.html) ein selbst definiertes Datumsformat gewählt. Dabei haben Sie das britische Datumsformat genutzt. Dieses führt jedoch dazu, dass in den erstellen Publikationen unter Umständen Datumsangaben falsch dargestellt sind. 

## Lösung
Das hier erhältliche Makro wandelt die Daten in das ISO Format um:
- ALT: 03/11/2019 (Citavi erkennt das Datum im US Format als 11. März 2019)
- NEU: 2019-11-03 (Citavi erkennt das Datum im ISO Format als 3. November 2019)

## Download
[Convert British to ISO dates](ConvertBritishToISODate.cs)

## Anwendung
1. Öffnen Sie das betreffende Citavi-Projekt.
1. Öffnen Sie im Makro-Editor ([s. Handbuch](https://www1.citavi.com/sub/manual6/de/index.html?executing_macros.html)) das hier bereitgestellte Makro. 
1. Klicken Sie auf **Ausführen**.

## Autor
Jörg Pasch

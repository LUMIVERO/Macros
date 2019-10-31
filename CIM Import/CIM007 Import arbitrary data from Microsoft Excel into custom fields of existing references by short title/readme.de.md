# Import von Excel-Daten in ein Projekt mit bestehenden Titeln

**WICHTIG**: Da dieses Makro die Daten in Ihrem Projekt verändert, ist es zwingend erforderlich, dass Sie zuvor ein Backup Ihres Projektes erstellen.

**Vorbemerkung**: Um Daten aus Excel importieren zu können, benötigen Sie einen sogenannten OLE-DB-Provider von Microsoft Access. 
Falls Sie bei der Ausführung des Makros folgende Fehlermeldung erhalten ...

**"Microsoft.ACE.OLEDB.16.0 provider is not registered on the local machine".**

... dann installieren Sie bitte das Microsoft Access Database Engine 2016 Redistributable Kit, welches Sie unter nachfolgender Adresse herunterladen können: https://www.microsoft.com/en-us/download/details.aspx?id=54920


![alt text](https://github.com/Citavi/Macros/blob/master/CIM%20Import/CIM007%20Import%20arbitrary%20data%20from%20Microsoft%20Excel%20into%20custom%20fields%20of%20existing%20references%20by%20short%20title/Parameters.jpg?raw=true)

## Aufbau des Makros und des Excel Worksheets

Das Makro ist zunächst dafür ausgelegt, anhand einer Spalte "Kurztitel" im Excel Worksheet die vorhandenen Titel im geöffneten Citavi Projekt zu identifizieren, um ihnen dann in das Feld "Freitext 1" den Inhalt der Spalte "Data1" aus dem Excel Worksheet hinzuzufügen. Die erste Zeile des Worksheets enthält also keine Importdaten sondern Feldnamen. 

Im Folgenden werden kurz einige Parameter zur Steuerung des Makros sowie seine Code-Abschnitte beschrieben, die Sie bearbeiten müssen, wenn Sie die Bezeichner der Spalten in Excel oder das Zielfeld in Citavi ändern möchten oder gar weitere Import-Spalten im Excel Worksheet hinzufügen möchten.

Beachten Sie bitte, dass Sie jedoch als Zielfelder in Citavi lediglich einfache Textfelder verwenden können. Der Import von Personen, Zeitschriften, Aufgaben, Wissenselementen u.a. ist komplexer und setzt umfangreichere Änderungen am Makro voraus.

## Parameter

(1) Zeile 28: **createMissingReference** Wenn "= true" dann wird der Titel merzeugt, falls kein Titel mit dem Kurztitel aus Excel im aktuellen Projekt gefunden werden kann. Wenn "= false", dann wird die Daten-Zeile in Excel nicht importiert.

(2) Zeile 29: **targetField1** Hier können Sie das Zielfeld der Spalte "Data1" aus Excel bestimmen. Verwenden Sie die Eigenschaften der ReferencePropertyId Auflistung (z.B. CustomField1, CustomField2, Abstract, Notes usw.)


## Autor
Jörg Pasch @joepasch

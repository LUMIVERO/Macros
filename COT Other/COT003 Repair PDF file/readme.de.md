# PDF-Dateien reparieren

Sie haben im Internet eine PDF-Datei gefunden, die sich in Citavi nicht annotieren lässt. Die PDF-Datei ist nicht schreibgeschützt.

## Lösung
Das hier bereitgestellte Werkzeug repariert PDF-Dateien.

## Download

[PDF Repair Tool.zip](pdf_repair_tool.zip)

## Anwendung

**Achtung:** Wenn die PDF-Datei Kommentare in Form von "sticky notes" (Acrobat) enthielt, werden diese durch die Reparatur unbrauchbar.

1. Installieren Sie Ghostscript ([Download](https://www.ghostscript.com/download/gsdnld.html)) --  aus Lizenzgründen dürfen wir das nicht mit ausliefern.
1. Laden Sie die ZIP-Datei [PDF Repair Tool.zip](pdf_repair_tool.zip) auf Ihren PC.
1. Entpacken Sie die Datei: Rechtsklick auf Datei > Alle Extrahieren > Extrahieren.
1. Doppelklicken Sie auf die Datei PDF Repair Tool.exe.
1. Eine leere Textdatei wird geöffnet. Tragen Sie in die Textdatei den Pfad zur Datei gswin32c.exe oder gswin64c ein (ohne Anführungszeichen), z.B.: `C:\Program Files\gs\gs9.16\bin\gswin64c.exe`.
1. Klicken Sie mit der rechten Maustaste auf `PDF Repair Tool.exe`. Wählen Sie aus dem Kontextmenü den Befehl **Senden an > Desktop (Verknüpfung erstellen)**.
1. Ziehen Sie die defekte PDF-Datei auf die erstellte Verknüpfung von `PDF Repair Tool.exe`.
1. Das Tool erzeugt eine Kopie der PDF-Datei und repariert diese. An den Dateinamen hängt das Tool die Erweiterung `Repaired`.

## Autor
Daniel Lutz

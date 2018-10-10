# Converting a Citavi 5 project back to Citavi 4 format

[[> Deutsche Version](readme.de.md)]

You need to pass on your Citavi 5 project to someone who still uses Citavi 4.

## Solution
The macros available here  help you to convert a Citavi 5 project back to Citavi 4 format.

## Download
[01 - for exporting from Citavi 5](01_Export_XML_from_C5_Project.cs)

[02 - for importing into Citavi 4](02_Import_XML_into_newly_created_C4_Project.cs)

## Usage

1. Open the Citavi project. Press `Alt+F11` to open the Macro editor. On the **File** menu, click **Open** and then open the script "01 Export XML from C5 Project.cs" that you downloaded from the link above.
1. Click **Run** to start the export.
1. Enter a name for the XML file.
1. Wait for the "Finished" message. Press **OK**.
1. Install Citavi 4 if not already done. The setup file can be downloaded [here](http://setup1.citavi.com/release/default/Citavi4Setup.exe).
1. Open Citavi 4, press `Alt+F11`, and click **File > Open**. Open the script "02 Import XML into newly created C4 Project.cs".
1. Select the XML file you created.
1. Enter a name for the new Citavi 4 project.
1. If your Citavi 5 project contained attachments, copy these files from the folder `Documents\Citavi 5\Projects\[project name]\Citavi Attachments` to `Documents\Citavi 4\Projects\[project name]\CitaviFiles`.

**Please note:**
 If an error occurs during importing the XML data into a Citavi 4 project, please start Citavi 4 with the command line parameter /noScratch (see [manual](http://www.citavi.com/sub/manual4/en/index.html?program_start_options.html)).

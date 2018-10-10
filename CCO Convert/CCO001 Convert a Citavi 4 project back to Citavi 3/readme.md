# Converting a Citavi 4 project back to Citavi 3 format

[[> Deutsche Version](readme.de.md)]

You need to pass on your Citavi 4 project to someone who still uses Citavi 3.

## Solution
The macros available here can convert the project. 

## Download
[for exporting from Citavi 4](01_Export_XML_from_C4_Project.cs)
[for importing into Citavi 3](02_Import_XML_into_newly_created_C3_Project.cs)

## Usage

**Important:** To use the macro in Citavi 3 you need Citavi 3.4.1.2, which can be downloaded [here](http://www.citavi.com/sub/setup/citavi3beta/CitaviSetup.exe) zum Download erhalten.

First export the Citavi 4 project into an XML file using the first macro. Once this is complete, you can open Citavi 3, load the second macro there and import the XML file into a new project.

Detailed instructions can be found in German in [this manual](instructions_de.pdf).

If you see the error message "The requested value "Omit" was not found", open the XML file you exported from Citavi 4 in a text editor such as Notepad. Use search and replace to search for the word `Omit` and replace it with `Arabic`.

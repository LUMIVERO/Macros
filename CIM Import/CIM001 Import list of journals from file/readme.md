#  Importing a list of journals

Some citation styles require specific abbreviations to be used for the names of periodicals. If you have the list as a text file, you can use this macro to import the list into the list of periodicals in your Citavi project.

## Solution
We provide a macro here that can import structured text data. 

## Download
[für Citavi 6](C6_Import_Journals.cs)

[für Citavi 5](C5_Import_Journals.cs)

[für Citavi 4](C4_Import_Journals.cs)

## Anwendung
Follow the instructions in  [this Artikel](\readme.md) to run the macro.

The list of periodicals to be imported needs to be saved as a .txt file and needs to fulfill the following requirements:

* Contain one periodical per line.
* Contain a full name, up to 3 abbreviations, and an ISSN on each line.
* Use either a tab, semicolon, equals sign, or vertical bar as the delimiter.

The following are some examples of valid formats:

* Full name; Abbr. 1
* Full name; Abbr. 1; Abbr. 2
* Full name; Abbr. 1; Abbr. 2; Abbr. 3; ISSN
* Full name = Abbr. 1; Abbr. 2;
* Full name = Abbr. 1; Abbr. 2; Abbr. 3; ISSN
* Full name;;;; ISSN

## Author
Jörg Pasch @joepasch
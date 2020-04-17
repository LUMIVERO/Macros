# Exclude Citavi fields from Word spellchecker

[[> Deutsche Version](readme.de.md)]

You'd like to exclude the Citavi fields with your references from the Word spell checker.

## Solution
The macro for Microsoft Word provided here helps you to achieve this goal.

## Download
[Word Macro: Citavi 6](C6_ExcludeCitaviFieldsFromSpellCheck.bas)

## Usage

1. Download the Macro provided above.
1. Start Word.
1. In Word press `ALT+F11` to open the VBA macro editor.
1. In the Macro Editor on the **File**  menu, select **Import file**.
1. Select the `C6_ExcludeCitaviFieldsFromSpellCheck.bas` file you downloaded.
1. Close the Macro Editor.
1. In Word, right-click the Quick Access Toolbar (i.e. the toolbar found at the very top of Word.)
1. Click *Customize Quick Access Toolbar*. Under *Choose commmands from*, select *Macros*.
1. Select the `CitaviFieldsNoProofing` macro you just added.
1. Click **Add**.
1. Click **OK**.

You can now continue inserting references in your document with Citavi's Add-In for Word. To exclude all of your citations from the spellchecker, click the macro symbol in the Quick Access Toolbar. 


## Author
Sebastian Pabel @sebastianpabel

Updated version from: https://help.citavi.com/topic/deaktivieren-der-word-korrektur-in-quellennachweisen#comment-224602

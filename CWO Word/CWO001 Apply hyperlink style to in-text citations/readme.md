# Apply the hyperlink style to hyperlinks in in-text citations

[[> Deutsche Version](readme.de.md)]

You linked citations in your text to the bibliography as described in our [manual](https://www1.citavi.com/sub/manual6/de/index.html?link_in_text_citations_to_references.html). Now you'd like the links to appear in another color (blue, for example). For this to occur, each citation should have the Word Hyperlink style applied to it.

## Solution
The macro for Microsoft Word provided here helps you to achieve this goal.

## Download
[Word Macro: Citavi 4 und 5](C4+_CitaviReferenceLink-hyperlink.bas)

[Word Macro: Citavi 6](C6_CitaviReferenceLink-hyperlink.bas)

[Word Macro: Citavi 6 (only year as hyperlink)](C6_CitaviReferenceLink-hyperlink-yearonly.bas)

## Usage

1. Click on the Macro provided above, e.g. **Word Macro: Citavi 6 (only year as hyperlink)**.
2. Click on **Raw**.
3. Right-click on the new page in the browser and select **Save Page As**.
4. Start Word.
5. In Word press `ALT+F11` to open the VBA macro editor.
6. In the Macro Editor on the **File**  menu, select **Import file**.
7. Select the `CitaviReferenceLink-hyperlink.bas` file you downloaded.
8. Close the Macro Editor.
9. In Word, right-click the Quick Access Toolbar (i.e. the toolbar found at the very top of Word).
10. Click *Customize Quick Access Toolbar*. Under *Choose commmands from*, select *Macros*.
11. Select the `Project.Modul1.CitaviReferenceHyperLink` or `Project.Modul1.CitaviReferenceHyperlinkYear` macro you just added.
12. Click **Add**.
13. Click **OK**.

You can now continue inserting references in your document with Citavi's Add-In for Word. To apply the hyperlink style to all of your citations, click the macro symbol in the Quick Access Toolbar. 


## Author
Sebastian Pabel @sebastianpabel

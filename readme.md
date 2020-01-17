# Macros for Citavi

## How to run a macro in Citavi
[[> Deutsche Version](readme.de.md)]

Macros are mini-programs that run within Citavi. They let you do things with your project that are not possible with Citavi's standard features. For example, you could batch move contents from one field to another, or sort categories alphabetically.

Citavi macros are written in the C# ("C sharp") programming language and interact with the Citavi object model, which is specific to a version of Citavi. This means that macros from earlier versions of Citavi usually will not work in a later version of Citavi.

If you receive a macro as a file from Citavi support staff, or from a forum post, here's how to use it:

1. Macro files have the .cs file extension. If you received the macro in a ZIP archive, be sure to extract it from the ZIP first.
1. Start Citavi and open the project you want to work on.
1. **Important: Back up the project before running a macro on it.**  Creating a backup is very important because the changes made by a macro cannot be undone!
1. Many macros apply to the current selection only, so if you want them to apply only to some references, use the filter or search features to create a selection first. (You can identify a macro that applies to the current selection because the macro's program code will contain `.GetFilteredReferences()` somewhere.)
1. Press Alt+F11 (or click **Tools > Options**) to open the Macro Editor. It can take several seconds before it opens.
1. In the Macro Editor, on the **File** menu, click **Open** and choose the macro file (.cs) you prepared in step 1.
1. Click **Compile**. No errors should appear in the lower pane of the window. 
1. Click Run to run the macro. You will be asked to confirm that you created a backup. If you haven't, cancel, create the backup, and then continue.

## Support

If you have any questions, comments or requests, please contact the [support](https://www.citavi.com/en/support/overview) directly

## License

This project is licensed under the [MIT](LICENSE) License

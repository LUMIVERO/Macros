using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;


using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
    public static void Main()
    {
        //****************************************************************************************************************
        // Clear Citation key
        // 2017-12-06
        //
        // This macro will overwrite the content of the field Citation key for all references in the current selection
        //
        // EDIT HERE
        // Variables to be changed by user


        // DO NOT EDIT BELOW THIS LINE
        // ****************************************************************************************************************

        if (Program.ProjectShells.Count == 0) return;       //no project open
        if (IsBackupAvailable() == false) return;           //user wants to backup his/her project first

        //iterate over all references in the current filter (or over all, if there is no filter)
        List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

        //reference to active Project
        Project activeProject = Program.ActiveProjectShell.Project;

        int counter = 0;


        foreach (Reference reference in references)
        {
            counter++;
            reference.CitationKey = String.Empty;
            if (string.IsNullOrEmpty(reference.CitationKey)) reference.CitationKeyUpdateType = UpdateType.Automatic;
        }

        // Message upon completion
        string message = "{0} reference(s) were changed.";
        message = string.Format(message, counter.ToString());
        MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    // Ask whether backup is available
    private static bool IsBackupAvailable()
    {
        string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
            "\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
            "\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
            "\r\n\r\n", "Do you want to continue?"
        );

        return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);
    }
    //end IsBackupAvailable()
}
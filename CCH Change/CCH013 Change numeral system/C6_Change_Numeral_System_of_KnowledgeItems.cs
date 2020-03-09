using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;

using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Resources;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Citavi.DataExchange;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//****************************************************************************************************************
		// CHANGE NUMERAL SYSTEM FOR ALL KNOWLEDGE ITEMS OF THE SELECTED REFERENCES
		// 1.1 -- 2018-05-29 added ".ToList()" to foreach loop on reference.Quotations to avoid "enumeration changed" errors 
		// 1.0 -- 2017-12-12
		//
		// This macro sets the numeral system to "arabic" for all knowledge items that currently have no numeral system
		// specified.

		//
		// EDIT HERE
		// Variables to be changed by user



		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first

		
		int foundCounter = 0;
		int changeCounter = 0;
		int errorCounter = 0;
		

		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;


		foreach (Reference reference in references)
		{
			foreach (KnowledgeItem knowledgeItem in reference.Quotations.ToList())
            {
                if (knowledgeItem.Reference == null) continue; // ignore Ideas
				if (knowledgeItem.QuotationType == QuotationType.Highlight) continue; // ignore highlights

				
				
				
				foundCounter++;
                var knowledgeItemPageRange = knowledgeItem.PageRange;				
                try
                {
                    if (knowledgeItem.PageRange.NumeralSystem == NumeralSystem.Omit)
                    {
                        PageRange newPageRange = knowledgeItemPageRange.Update(NumeralSystem.Arabic);    
                 		knowledgeItem.PageRange = newPageRange;
						changeCounter++;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("An error occurred: " + e.Message, "Citavi Macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DebugMacro.WriteLine(e.ToString());
                    errorCounter++;
                    return;
                }
            }
		}

		//activeProject.Save();

		// Message upon completion
		string message = " {0} knowledge item(s) found\n {1} knowledge item(s) changed\n {2} knowledge item(s) with errors";
		message = string.Format(message, foundCounter.ToString(), changeCounter.ToString(), errorCounter.ToString());
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

}

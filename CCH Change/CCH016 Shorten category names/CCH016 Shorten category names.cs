using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
		// Shorten category names that contain more than 100 characters
		// 1.0 -- 2021-06-09
		//
		// This macro checks all category names. Those that contain more than 100 characters will be shortened.
		//
		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		int counter = 0;

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first

		//Get the active project
		Project activeProject = Program.ActiveProjectShell.Project;
		
		List<Category> longCategories = activeProject.AllCategories.Where(x => x.Name.Length > 100).ToList();
		
		foreach (Category category in longCategories)
		{
			counter++;
			string shortName = category.Name.Substring(0, 100);
			category.SetValue(CategoryPropertyId.Name, shortName);
		}

		// Message upon completion
		string message = "{0} categories have been normalised.";
		message = string.Format(message, counter.ToString());
		MessageBox.Show(message, "Citavi Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

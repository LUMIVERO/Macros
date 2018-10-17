using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
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
		// ADD PARENT CATEGORIES TO REFERENCE
		// 1.0 - 2016-06-01
		//
	    // DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first

			
		int foundCounter = 0;
		int changeCounter = 0;
		
		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;

		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
		List<Category> allCategories = Program.ActiveProjectShell.Project.AllCategories.ToList();
	
		//regex for space replacement
		var space = new Regex(@"\s");
		
		foundCounter = references.Count;			

		foreach (Reference reference in references)
		{
			if (reference.Categories == null) continue;
			List<Category> refCategories = reference.Categories.ToList();			
			List<Category> categoriesToAdd = new List<Category>();
			
            foreach (Category cat in refCategories)
			{				
				string catPath = cat.GetPath(true);
				if (String.IsNullOrEmpty(catPath)) continue; // go on if no categories path is present
			
				//split path and compare
				List<string> parentCats = catPath.Split(new String[]{" > "}, StringSplitOptions.RemoveEmptyEntries).ToList();
				parentCats.RemoveAll(string.IsNullOrWhiteSpace);
				if (parentCats.Count <= 1) continue;
				foreach (string pc in parentCats)	
				{			
					string pcCorrect = space.Replace(pc, "\u00a0", 1); // Category.FullName uses &nbsp; after number, Category.Path doesn't
					var foundCategory = allCategories.Where(item => item.FullName.Equals(pcCorrect, StringComparison.InvariantCulture)).FirstOrDefault();
					if (foundCategory != null && !reference.Categories.Contains(foundCategory)) categoriesToAdd.Add(foundCategory);								
				}
				
			}
			
			if (categoriesToAdd.Count > 0)
			{
				categoriesToAdd = categoriesToAdd.Distinct().ToList();
				reference.Categories.AddRange(categoriesToAdd);	
				changeCounter++;
			}					
			
			
   
		}

		// Message upon completion
		 string message = " {0} reference(s) in selection\n {1} reference(s) changed\n";
		 message = string.Format(message, foundCounter.ToString(), changeCounter.ToString());
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

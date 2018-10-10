using System;
using System.ComponentModel;
using System.Collections.Generic;
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
		if (Program.ProjectShells.Count == 0) return;		//no project open	
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first
		
		int counter = 0;
		
		try 
		{

			//if this macro should ALWAYS affect all titles in active project, choose first option
			//if this macro should affect just filtered rows if there is a filter applied and ALL if not, choose second option
			
			//ProjectReferenceCollection references = Program.ActiveProjectShell.Project.References;		
			List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
			
			//if we need a ref to the active project
			//SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
			
			//we collect references to be changed in a list
			List<Reference> referencesToBeChanged = new List<Reference>();

			foreach (Reference reference in references)
			{
				if (reference.ShortTitleUpdateType == UpdateType.Manual) referencesToBeChanged.Add(reference);				
			}
		
			//now apply changes
			foreach (Reference reference in referencesToBeChanged)
			{
				counter++;
				reference.ShortTitleUpdateType = UpdateType.Automatic;
			}
		
		} //end try
		
		finally 
		{
			MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} changes were made.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} //end finally
		
	} //end main()
	
	
	
	
	private static bool IsBackupAvailable() 
	{
		string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
			"\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
			"\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
			"\r\n\r\n", "Do you want to continue?"
		);
				
		
		return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);


	} //end IsBackupAvailable()
	
}
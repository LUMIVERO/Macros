using System;
using System.Linq;
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
		char splitter = ',';	//if you have any other separating character, like '|' or '/' you can adjust this here
		
		
		
		if (!Program.ProjectShells.Any()) return;		//no project open	
		if (IsBackupAvailable() == false) return;		//user wants to backup his/her project first
		
		SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
		activeProject.Save();
			
		//collect all keywords to be separated
		List<Keyword> keywordsToBeSplit = new List<Keyword>();
		foreach (Keyword keyword in activeProject.Keywords)
		{
			if (keyword.Name.Contains(splitter)) keywordsToBeSplit.Add(keyword);			
		}
			
		if (keywordsToBeSplit.Count == 0)
		{
			MessageBox.Show("No keywords to be split.");
			return;
		}
			
			
		foreach (Keyword keyword in keywordsToBeSplit)
		{
			//get references and knowledge items that need to be "tagged" with the "fractions"
			List<Reference> referencesToBeTagged = new List<Reference>();
			foreach(Reference reference in keyword.References)
			{
				referencesToBeTagged.Add(reference);
			}
			
			List<KnowledgeItem> knowledgeItemsToBeTagged = new List<KnowledgeItem>();
			foreach(KnowledgeItem knowledgeItem in keyword.KnowledgeItems)
			{
				knowledgeItemsToBeTagged.Add(knowledgeItem);
			}
			
			
			List<string> keywordStringFragments = keyword.Name.Split(new char[] { splitter }, StringSplitOptions.RemoveEmptyEntries).ToList();
			List<Keyword> fragmentKeywords = new List<Keyword>(); 
			//make sure the fragment strings exist as keywords
			foreach(string keywordStringFragment in keywordStringFragments)
			{
				fragmentKeywords.Add(activeProject.Keywords.Add(keywordStringFragment.Trim()));
			}
			
			
			//assign keyword fragment to reference-to-be-tagged
			foreach(Reference reference in referencesToBeTagged)
			{
				reference.Keywords.AddRange(fragmentKeywords);
			}
				
				
			//assign keyword fragment to knowledge-item-to-be-tagged
			foreach(KnowledgeItem knowledgeItem in knowledgeItemsToBeTagged)
			{
				knowledgeItem.Keywords.AddRange(fragmentKeywords);
			}
		}
					
		//finally we could delete the old "mega-keywords"
		activeProject.Keywords.RemoveRange(keywordsToBeSplit);
		activeProject.Save();
	
		MessageBox.Show("Done");
	}
	
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
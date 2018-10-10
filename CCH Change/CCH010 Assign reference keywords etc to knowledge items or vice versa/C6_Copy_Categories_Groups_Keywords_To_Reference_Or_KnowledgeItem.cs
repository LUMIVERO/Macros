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
		// ADD REFERENCE CATEGORIES, KEYWORDS AND GROUPS TO KNOWLEDGE ITEMS AND VICE VERSA
		// 2.0 -- 2017-03-16
		// 
		//
		// EDIT HERE
		// Variables to be changed by user

        // choose direction
        int direction = 1; // 1 for reference -> knowledge item, 2 for knowledge item -> reference

        // set one or several of the following to true
		bool setCategories = false;  
		bool setKeywords = false; 
        bool setGroups = false;

		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first


        // wrong direction
        if (direction != 1 && direction != 2)
        {
            DebugMacro.WriteLine("Direction not set correctly, please change line 29 in the code!");
            return;
        }

		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;


		//counters

		int categoryCounter = 0;
        int keywordCounter = 0;
        int groupCounter = 0;
        int errorCounter = 0;

		foreach (Reference reference in references)
		{
			foreach (KnowledgeItem knowledgeItem in reference.Quotations)
			{
				if (knowledgeItem.Reference == null) continue; // ignore Ideas
				if (knowledgeItem.QuotationType == QuotationType.Highlight) continue; // ignore highlights

				if (setCategories)
				{
					try
                    {
                        List<Category> kiCategories = knowledgeItem.Categories.ToList();
                        List<Category> refCategories = knowledgeItem.Reference.Categories.ToList();
                        
                        List<Category> mergedCategories = kiCategories.Union(refCategories).ToList();
                        mergedCategories.Sort();

                        switch (direction)
                        {
                            case 1:
                                knowledgeItem.Categories.Clear();
                                knowledgeItem.Categories.AddRange(mergedCategories);
                                categoryCounter++;
                                break;

                            case 2:
                                reference.Categories.Clear();
                                reference.Categories.AddRange(mergedCategories);
                                categoryCounter++;
                                break;
                        }
                                
                       
                    }
                    catch (Exception e)
                    {
                        string errorString = String.Format("An error occurred with knowledge item '{0}' in reference {1}:\n  {2}", knowledgeItem.CoreStatement, reference.ShortTitle, e.Message);
                        DebugMacro.WriteLine(errorString);
                        errorCounter++;
                    }
                   
				}

                if (setKeywords)
                {
                    try
                    {
                        List<Keyword> kiKeywords = knowledgeItem.Keywords.ToList();
                        List<Keyword> refKeywords = reference.Keywords.ToList();
                        
                        List<Keyword> mergedKeywords = kiKeywords.Union(refKeywords).ToList();
                        mergedKeywords.Sort();

                        switch (direction)
                        {
                            case 1:
                                knowledgeItem.Keywords.Clear();
                                knowledgeItem.Keywords.AddRange(mergedKeywords);
                                keywordCounter++;
                                break;

                            case 2:
                                reference.Keywords.Clear();
                                reference.Keywords.AddRange(mergedKeywords);
                                keywordCounter++;
                                break;
                        }

                    }              

                    catch (Exception e)
                    {
                        string errorString = String.Format("An error occurred with knowledge item '{0}' in reference {1}:\n  {2}", knowledgeItem.CoreStatement, reference.ShortTitle, e.Message);
                        DebugMacro.WriteLine(errorString);
                        errorCounter++;
                    }
                }			
				
                if (setGroups)
                {
                    try
                    {
                        List<Group> kiGroups = knowledgeItem.Groups.ToList();
                        List<Group> refGroups = reference.Groups.ToList();

                        List<Group> mergedGroups = kiGroups.Union(refGroups).ToList();
                        mergedGroups.Sort();
                        
                        switch (direction)
                        {
                            case 1:
                                knowledgeItem.Groups.Clear();
                                knowledgeItem.Groups.AddRange(mergedGroups);
                                groupCounter++;
                                break;

                            case 2:
                                reference.Groups.Clear();
                                reference.Groups.AddRange(mergedGroups);
                                groupCounter++;
                                break;
                        }



                    }
                    catch (Exception e)
                    {
                        string errorString = String.Format("An error occurred with knowledge item '{0}' in reference {1}:\n  {2}", knowledgeItem.CoreStatement, reference.ShortTitle, e.Message);
                        DebugMacro.WriteLine(errorString);
                        errorCounter++;
                    }
                }				
			}		
		}		

		// Message upon completion
		string message = String.Empty;
        
        switch (direction)
        {
            case 1:
                message = "On {0} knowledge items categories have been changed,\n on {1} knowledge items keywords have been changed,\n on {2} knowledge items groups have been changed.\n {3} errors occurred.";
		        break;
            case 2:
                message = "On {0} references categories have been changed,\n on {1} references keywords have been changed,\n on {2} references groups have been changed.\n {3} errors occurred.";
                break;
        }
              
        message = string.Format(message, categoryCounter.ToString(), keywordCounter.ToString(), groupCounter.ToString(), errorCounter.ToString());       
         
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
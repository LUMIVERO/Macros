using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
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
		// List Keywords for Each Category in a Text File
		// 1.0 -- 2015-07-30
		//
				
		// This macro writes a text file containing a list of all Keywords used in References assigned to a certain
		// category. 
		//
		//
		// EDIT HERE
		// Variables to be changed by user


		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
				
		if (Program.ProjectShells.Count == 0) return;		//no project open
		
		string file = string.Empty;
		
		
		string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		using (SaveFileDialog saveFileDialog = new SaveFileDialog())
		{
			saveFileDialog.Filter =  "Plain Text (*.txt)|*.txt|All files (*.*)|*.*";
			saveFileDialog.InitialDirectory = initialDirectory;
			saveFileDialog.Title = "Enter a file name for the output file.";	

			if (saveFileDialog.ShowDialog(primaryMainForm) != DialogResult.OK) return;
			file = saveFileDialog.FileName;
		}
			

		StreamWriter sw = null;
	
		try 
		{
			sw = File.AppendText(file);
		}
		catch (Exception e)
		{
			DebugMacro.WriteLine("Error creating file: " + e.Message.ToString());
			return;
		}
		
		
		// write line with categories to file
		string headline = activeProject.Name + " -- Categories and Keywords ";
		headline = headline + "\n";
		sw.WriteLine(headline);
		
		
		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

				
		//reference to all Categories in the project
		Category[] allCategories = activeProject.AllCategories.ToArray();
		Array.Sort(allCategories);
		
		//reference to all Keywords in the project
		Keyword[] allKeywords = activeProject.Keywords.ToArray();
		Array.Sort(allKeywords);

		// abort if no categories/references present
		if (!allCategories.Any() || !references.Any() || !allKeywords.Any())
		{
			DebugMacro.WriteLine("No categories/keywords or references in current project.");
			return; 
		}
		
		

		// check each category in turn and gather keywords
		
		foreach (Category category in allCategories) 
		{
			List<Keyword> categoryKeywords = new List<Keyword>();
			
			foreach (Reference reference in references)
			{
				foreach (Category referenceCategory in reference.Categories)
				{
					if (referenceCategory.Equals(category))
					{
						foreach (Keyword kw in reference.Keywords)
						{
							categoryKeywords.Add(kw);
						}
					}
				}
			}
			
			
			// Clean and Sort list of Keywords
			List<Keyword> categoryKeywordsUnique = categoryKeywords.Distinct().ToList();
			categoryKeywordsUnique.Sort();
			
			
			// write output to file			

			sw.WriteLine(category.ToString() + "\n");
			
			if (categoryKeywordsUnique.Count > 0)
			{			
				foreach (Keyword key in categoryKeywordsUnique)
				{
					sw.WriteLine("\t" + key.ToString());
				}
			}
			else 
			{
				sw.WriteLine("\t" + "---");
			}
			
			// clear Keywords for new Category
			categoryKeywords.Clear();
			categoryKeywordsUnique.Clear();
			sw.WriteLine("\n");
		}
	
	// close file
	sw.Close();	
		
	// Message upon completion
	string message = "File written to " + file;
	MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);	

	}
	
}
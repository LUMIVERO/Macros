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
		// CSV-Table Showing the Categories for Each Title
		// 1.1 -- 2015-08-07
		//
				
		// This macro writes a tab delimited CSV file showing for each reference which categories are assigned to that 
		// reference. Import the file into Excel using the "From text" function in the DATA tab.
		// In the variable "categoryPresent" you can set the text that occurs in a cell if a category is present
		// for a certain reference.
		//
		//
		// EDIT HERE
		// Variables to be changed by user

		string categoryPresent = "Yes."; // string to put in table cell if category is present

		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
				
		if (Program.ProjectShells.Count == 0) return;		//no project open
		
		
		
		
		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

		
		
		//reference to all Categories in the project
		Category[] allCategories = activeProject.AllCategories.ToArray();
		Array.Sort(allCategories);

		// abort if no categories/references present
		if (!allCategories.Any() || !references.Any())
		{
			DebugMacro.WriteLine("No categories or references in current project.");
			return; 
		}
		
		string file = string.Empty;
		
		
		string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		using (SaveFileDialog saveFileDialog = new SaveFileDialog())
		{
			saveFileDialog.Filter =  "CSV files (*.csv, *.txt)|*.csv;*.txt|All files (*.*)|*.*";
			saveFileDialog.InitialDirectory = initialDirectory;
			saveFileDialog.Title = "Enter a file name for the CSV file.";	

			if (saveFileDialog.ShowDialog(primaryMainForm) != DialogResult.OK) return;
			file = saveFileDialog.FileName;
		}
			

		// create file, delete if already present
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
		string headline = "\t \t \t";
		foreach (Category category in allCategories)
		{
			string cString = category.ToString() + "\t";
			headline = headline + cString;					
		}
		headline = headline + "\n";
		sw.WriteLine(headline);
		
		
		// check categories for each reference
		foreach (Reference reference in references)
		{
			// write author, year and title to file
			string author = reference.AuthorsOrEditorsOrOrganizations.ToString();
			string year = reference.YearResolved.ToString();
			string title = reference.Title.ToString();
			string line = author + "\t" + year + "\t" + title + "\t";
			
			// iterate over all Categories and Categories of each reference and write
			// categoryPresent string to file if category is present
			Category[] referenceCategories = reference.Categories.ToArray();
			Array.Sort(referenceCategories);
			foreach (Category category in allCategories)
			{
				foreach (Category referenceCategory in referenceCategories)
				{
					if (referenceCategory == category)
					{
						line = line + categoryPresent;
					}
				}
				line = line + "\t";
			}
			line = line + "\n";
			sw.WriteLine(line);
		}	
				
		// close file
		sw.Close();
		
		// Message upon completion
		string message = "CSV file written to " + file;
		MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
	}
}
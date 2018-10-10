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
		// List Inbound and Outbound Links for each Reference
		// 1.0 -- 2016-04-05
		//
				
		// This macro creates a csv file containing the numbers of reference each reference links to or is linked from.
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
		string headline = activeProject.Name + " -- Links to / links from ";
		headline = headline + "\n";
		sw.WriteLine(headline);
        string headers = "ShortTitle \t Outbound \t Inbound \n";
        sw.WriteLine(headers); 
		
		
		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

		if (references.Count == 0) return;        

        
        foreach (Reference reference in references)
        {
            var entityList = reference.EntityLinks.ToList();                                  
            int linksFromCount = entityList.Where(x => (x.Source.ToString() == reference.ShortTitle) && (x.Target.ToString() != reference.ShortTitle)).Count();
            int linksToCount = entityList.Where(x => (x.Target.ToString() == reference.ShortTitle) && (x.Source.ToString() != reference.ShortTitle)).Count();  
            sw.WriteLine(reference.ShortTitle + "\t" + linksFromCount.ToString() + "\t" + linksToCount.ToString() + "\n");                            
                   
        }  		
		
	
	    // close file
    	sw.Close();	
		
	    // Message upon completion
	    string message = "File written to " + file;
    	MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);	

	}
    

    
	
}
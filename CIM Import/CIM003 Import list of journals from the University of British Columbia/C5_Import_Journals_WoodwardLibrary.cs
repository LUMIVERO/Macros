
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;


using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Persistence;


#region MacroInfo
// Import PubMed journal list into project and update information already present in project
// Version 1.3
// 2016-03-09
#endregion


public static class CitaviMacro
{
	
	public static void Main()
	{

        #region UserVariable
        string journalUrl = @"http://journal-abbreviations.library.ubc.ca/dump.php"; // URL for journal list text file
      
      
        #endregion   
           
           
     
		if (Program.ProjectShells.Count == 0) return;		//no project open	
		
		Project activeProject = Program.ActiveProjectShell.Project;

		List<Periodical> journalCollection = new List<Periodical>();   
        string completeList;
        try
         {
            // Get list of journals from website
            Cursor.Current = Cursors.WaitCursor;
            DebugMacro.WriteLine("Reading list of journals from " + journalUrl);
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(journalUrl);
            StreamReader streamReader = new StreamReader(stream);
            completeList = streamReader.ReadToEnd();
         }
         catch  (Exception e)
         {
             Cursor.Current = Cursors.Default;
             DebugMacro.WriteLine("Could not read file from " + journalUrl +": " + e.Message);
             return;
         }
         
        
        // split into individual Journals
        int counter = 0;
        int refCounter = 0;
               
        
        try
        {
            Cursor.Current = Cursors.WaitCursor;
         
            // prepare data
            completeList = Regex.Match(completeList, @"(<tbody>.*?<\\/tbody)").ToString();
            completeList = Regex.Replace(completeList, @"\\/", @"/");
           
            
            MatchCollection journals = Regex.Matches(completeList, "(?<=<tr>).*?(?=</tr>)");   
            DebugMacro.WriteLine("{0} journal entries found.", journals.Count.ToString());
            DebugMacro.WriteLine("Parsing entries ...");
        
            foreach (Match journalAndAbbrev in journals)
            {
                string journalTitle = String.Empty;
                string abbreviation1 = String.Empty; // this one should have full stops
                string abbreviation2 = String.Empty; // this one shouldn't
                string abbreviation3; // this one should be any all uppercase acronym after a colon in JournalTitl
                
                MatchCollection journalData = Regex.Matches(journalAndAbbrev.ToString(), "(?<=<td>).*?(?=</td>)");
                if (journalData.Count < 2) continue;							
				
				abbreviation1 = journalData[0].Value;
				journalTitle = Regex.Replace(journalData[1].Value, @"\bfur\b", "für");			                               
	           
				// generate abbreviation2 by removing full stops from abbreviation1
	            string[] abbreviation1Words = abbreviation1.Split(' ');
	            for (int i = 0; i < abbreviation1Words.Length; i++)
	                {
	                    abbreviation1Words[i] = abbreviation1Words[i].TrimEnd('.');
	                }
	            abbreviation2 = String.Join(" ", abbreviation1Words);
                
				
                // try to establish Abbreviation3
	            abbreviation3 = Regex.Match(journalTitle, @"(?:: )[A-Z]{2,6}$").ToString();
                        
            
            
            Periodical journal = new Periodical(activeProject, journalTitle);			
            if (!String.IsNullOrEmpty(abbreviation1)) journal.StandardAbbreviation = abbreviation1;
            if (!String.IsNullOrEmpty(abbreviation2)) journal.UserAbbreviation1 = abbreviation2;
            if (!String.IsNullOrEmpty(abbreviation3)) journal.UserAbbreviation2 = abbreviation3;
            
            journalCollection.Add(journal);	
                
          }
            
            DialogResult updateReferences = MessageBox.Show(
                  @"Would you like to update journal information for references in current selection (all references if no selection is active)?",
                  "Citavi Macro", MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
             MessageBoxDefaultButton.Button2);
            
            
        // update references that have identical titles
        if (updateReferences == DialogResult.Yes)
        {
            
            List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
            foreach (Reference reference in references)
            {
                if (reference.Periodical == null) continue;
				if (journalCollection.Any(item => item.Name == reference.Periodical.Name) && !String.IsNullOrEmpty(reference.Periodical.Name))
                {
                    reference.Periodical = journalCollection.Where(item => item.Name == reference.Periodical.Name).FirstOrDefault();
                    refCounter++;                       
                }                   
            }
            activeProject.Periodicals.AddRange(journalCollection);                  
        }
        else
        {
            activeProject.Periodicals.AddRange(journalCollection);   
        }
            
                
        
    }
        catch (Exception e)
        {
               Cursor.Current = Cursors.Default;
               DebugMacro.WriteLine("An error occurred importing the journal data: " + e.ToString());
               return;
          
        }        		
		
		finally
		{
			Cursor.Current = Cursors.Default;
            var saveOptions = new DesktopDatabaseSaveOptions();
			saveOptions.ForceMultiUserUpdate = true;
			activeProject.Save(saveOptions);
			
			Cursor.Current = Cursors.Default;
			
			if (journalCollection != null) 
			{
				MessageBox.Show(journalCollection.Count.ToString() + " journal(s) were imported (not counting duplicates)\n " + refCounter.ToString() + " reference(s) were changed.", "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
				journalCollection = null;	
			}
			

		}
        
        
		
	} //end Main()
	
	
		
	
	
	public static bool IsValidISSN (string issn) 
	{ 
		//example of a valid ISSN
		//string issn = "0317-8471";

		issn = System.Text.RegularExpressions.Regex.Replace(issn, @"[^\dxX]", "");
		if (issn.Length != 8) return false;

		char[] issnchars = issn.ToCharArray();
		string[] issnsubstrings = new string[issnchars.Length];
		for (int i = 0; i < issnchars.Length; i++)
		{
			issnsubstrings[i] = issnchars[i].ToString();							
		}

			if (issnsubstrings[7].ToUpper() == "X")
			{
				issnsubstrings[7] = "10";
			} 
		int sum = 0; 
		for (int i = 0; i < issnsubstrings.Length; i++) 
		{
			sum += ((8 - i) * Int32.Parse(issnsubstrings[i])); 
		}; 
		return ((sum % 11) == 0); 
		
		
	} //end isValidISSN
	
	
}	//end class CitaviMacro
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


// Diese Implementation eines Citavi-Makroeditors ist vorläufig und experimentell.
// Das Citavi Objektmodell kann sich in zukünftigen Versionen ändern.

#region Beispiele

/*	Springen zum ersten Titel:
	--------------------------
	GuiManager.ActiveProjectSet.PrimaryMainForm.ActiveReference = GuiManager.ActiveProjectSet.References[0];


	Iterieren durch alle Titel des aktiven Projekts:
	------------------------------------------------

	Project activeProject = GuiManager.ActiveProjectSet.Project;
	
	foreach (Reference reference in activeProject.References)
	{
		// Do something
	}
*/

#endregion




public static class CitaviMacro
{
	
	public static void Main()
	{

        #region UserVariable
        string journalUrl = @"ftp://ftp.ncbi.nih.gov/pubmed/J_Entrez.txt"; // URL for journal list text file
      
      
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
        
        
        try
        {
            Cursor.Current = Cursors.WaitCursor;
         
            string[] entrySplitters = {@"--------------------------------------------------------"};
            List<string> individualJournalEntries = new List<string>(completeList.Split(entrySplitters, StringSplitOptions.RemoveEmptyEntries));
            
            DebugMacro.WriteLine("{0} journal entries found.", individualJournalEntries.Count.ToString());
            DebugMacro.WriteLine("Parsing entries ...");
            
            
            int counter = 0;
            
            Regex splitEntry = new Regex(@"^(?:JrId: )(?<JournalId>\d+?)(?:\nJournalTitle: )(?<JournalTitle>.*?)(?:\nMedAbbr: )(?<Abbreviation2>.*?)(?:\nISSN \(Print\): )(?<IssnPrint>.*?)(?:\nISSN \(Online\): )(?<IssnOnline>.*?)(?:\nIsoAbbr: )(?<Abbreviation1>.*?)(?:\nNlmId: )(?<NlmId>.*?)$",
            RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.Multiline); 
            
            foreach (string journalEntry in individualJournalEntries)
            {
            
                counter++;
                
                string journalTitle;
                string abbreviation1; // this one should have full stops
                string abbreviation2; // this one shouldn't
                string abbreviation3; // this one should be any all uppercase acronym after a colon in JournalTitle
                string issnPrint;
                string issnOnline;
                string nlmId;
                
                // split into fields
                Match m = splitEntry.Match(journalEntry);
            
                //if (String.IsNullOrEmpty(m.Groups["JournalId"].Value)) continue; // nothing here
                
                journalTitle = m.Groups["JournalTitle"].Value;
                abbreviation1 = m.Groups["Abbreviation1"].Value;
                abbreviation2 = m.Groups["Abbreviation2"].Value;
                issnPrint = m.Groups["IssnPrint"].Value; // to be validated
                issnOnline = m.Groups["IssnOnline"].Value;
                nlmId = m.Groups["NlmId"].Value;
                
                // check format of abbreviation1
                if (String.IsNullOrEmpty(abbreviation1)) abbreviation1 = abbreviation2;                
                if (!abbreviation1.Contains(".") && !String.IsNullOrEmpty(abbreviation1))
                {
                    string[] journalTitleWords = journalTitle.ToLowerInvariant().Split(new char[]{' ', '.', ';', ',', ':', '&', '-'}, StringSplitOptions.RemoveEmptyEntries);
                    string[] abbreviation1Words = abbreviation1.Split(' ');
                    List<string> abbreviation1WithFullStops = new List<string>();
                    
                    
                    foreach (string word in abbreviation1Words)
                    {
                        
                        if (word.StartsWith("(") || word.EndsWith(")"))
                        {
                            abbreviation1WithFullStops.Add(word);
                        }
                        else if (!Array.Exists(journalTitleWords, x => x == word.ToLowerInvariant()))
                        {
                            abbreviation1WithFullStops.Add(word + ".");
                        }
                        else
                        {
                            abbreviation1WithFullStops.Add(word);
                        }
                    }
                    
                    abbreviation1 = String.Join(" ", abbreviation1WithFullStops); 
                }            
            
                // try to establish Abbreviation3
                abbreviation3 = Regex.Match(journalTitle, @"(?:: )[A-Z]{2,6}$").ToString();
                
            
           
            
                Periodical journal = new Periodical(activeProject, journalTitle);
                if (!String.IsNullOrEmpty(abbreviation1)) journal.StandardAbbreviation = abbreviation1;
                if (!String.IsNullOrEmpty(abbreviation2)) journal.UserAbbreviation1 = abbreviation2;
                if (!String.IsNullOrEmpty(abbreviation3)) journal.UserAbbreviation2 = abbreviation3;
                
                if (!string.IsNullOrEmpty(issnPrint) && IsValidISSN(issnPrint)) journal.Issn = issnPrint;
                else if (!string.IsNullOrEmpty(issnOnline) && IsValidISSN(issnOnline)) journal.Issn = issnOnline;
                
                if (!string.IsNullOrEmpty(issnPrint) && IsValidISSN(issnPrint) && !string.IsNullOrEmpty(issnOnline) && IsValidISSN(issnOnline))
                {
                    journal.Notes = "ISSN (Online): " + issnOnline;
                }
                
                if (!String.IsNullOrEmpty(nlmId)) journal.Notes = journal.Notes + "\nNlmID: " + nlmId;
                
                journalCollection.Add(journal);	
                
            }
        activeProject.Periodicals.AddRange(journalCollection);    
        
        }
        catch (Exception e)
        {
               Cursor.Current = Cursors.Default;
               DebugMacro.WriteLine("An error occurred importing the journal data: " + e.Message);
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
				MessageBox.Show(journalCollection.Count.ToString() + " journal(s) were imported (not counting duplicates)", "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
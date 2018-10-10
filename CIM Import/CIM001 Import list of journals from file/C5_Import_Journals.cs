using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;


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
		if (Program.ProjectShells.Count == 0) return;		//no project open	
		
		Project activeProject = Program.ActiveProjectShell.Project;
	
		string fileName = null;

		List<Periodical> journalCollection = new List<Periodical>();
			
		try
		{
			// Dialog zum Auswählen des gewünschten Ordners einblenden
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text Dateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
			openFileDialog.Title = "Wählen Sie die zu importierende Zeitschriften-Datei aus (Text-Format)";			
						
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				fileName = openFileDialog.FileName;
			} 
			else
			{
				return;
			}
			

			//Hourglass or other wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			
			Encoding enc = GetFileEncoding(fileName);		
			StreamReader streamReader = new StreamReader(fileName, enc);
			string journalList = streamReader.ReadToEnd();
			streamReader.Close();
			
			//We check for certain non-printable chars to ensure we are dealing with a "text file"
			Regex testRegex = new Regex("[\x00-\x1f-[\t\n\r]]", RegexOptions.CultureInvariant | RegexOptions.Compiled);	
			if (testRegex.IsMatch(journalList))
			{	//this is most likely not a textfile
				Cursor.Current = Cursors.Default;
				MessageBox.Show("Diese Datei enthält ungültige Zeichen und kann daher nicht importiert werden.", "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
					
			
			Regex matchRegex = new Regex(
				@"^(?<FullName>[^#=;|\t\n]+?)(?: *[#=;|\t] *(?<Abbreviation1>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation2>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation3>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<ISSN>[^#=;|\t\n]*))??$",
				RegexOptions.CultureInvariant
    			| RegexOptions.Compiled
				| RegexOptions.Multiline	//IMPORTANT!
    		);
			
		
			
			
			//// Capture all matches in the journalList
			MatchCollection ms = matchRegex.Matches(journalList);
			//MessageBox.Show("Insgesamt sind " + ms.Count.ToString() + " Treffer gefunden worden.");
			
			foreach (Match m in ms)
			{
				
				if (string.IsNullOrEmpty(m.Groups["FullName"].Value)) continue;
				
				Periodical journal = new Periodical(activeProject, m.Groups["FullName"].Value);
				if (!string.IsNullOrEmpty(m.Groups["Abbreviation1"].Value)) journal.StandardAbbreviation = m.Groups["Abbreviation1"].Value;
				if (!string.IsNullOrEmpty(m.Groups["Abbreviation2"].Value)) journal.UserAbbreviation1 = m.Groups["Abbreviation2"].Value;
				if (!string.IsNullOrEmpty(m.Groups["Abbreviation3"].Value)) journal.UserAbbreviation2 = m.Groups["Abbreviation3"].Value;
				
				string sISSN = "";
				if (!string.IsNullOrEmpty(m.Groups["ISSN"].Value)) 
				{
					sISSN = m.Groups["ISSN"].Value;
					if (IsValidISSN(sISSN)) journal.Issn = sISSN;
				}
				
				

				journalCollection.Add(journal);	
			}
			
			activeProject.Periodicals.AddRange(journalCollection);
			//activeProject.Journals.AddRangeBound(journalCollection);
		}
			
		catch (Exception exception)
		{
			Cursor.Current = Cursors.Default;
			
			MessageBox.Show(exception.ToString(), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		
		finally
		{
			activeProject.Save(TrackPriority.Immediate);
			
			Cursor.Current = Cursors.Default;
			
			if (journalCollection != null) 
			{
				MessageBox.Show("Es wurde(n) " + journalCollection.Count.ToString() + " Zeitschrift(en) eingelesen. \n(Dubletten wurden unterdrückt.)", "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
				journalCollection = null;	
			}
			

		}
		
	} //end Main()
	
	
	public static Encoding GetFileEncoding(string srcFile)
	{
		//http://www.west-wind.com/Weblog/posts/197245.aspx
		//http://de.wikipedia.org/wiki/Byte_Order_Mark
		
    	// *** Use Default of Encoding.Default (Ansi CodePage)
	    Encoding enc = Encoding.Default;

	    // *** Detect byte order mark if any - otherwise assume default
	    byte[] buffer = new byte[5];
	    FileStream file = new FileStream(srcFile, FileMode.Open);
	    file.Read(buffer, 0, 5);
    	file.Close();

   		if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
	        enc = Encoding.UTF8;

    	else if (buffer[0] == 0xfe && buffer[1] == 0xff)
	        enc = Encoding.Unicode;

    	else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
	        enc = Encoding.UTF32;

    	else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
	        enc = Encoding.UTF7;

    	return enc;

	}
	
	
	
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
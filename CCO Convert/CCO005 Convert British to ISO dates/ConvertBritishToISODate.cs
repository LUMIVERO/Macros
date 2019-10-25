using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	//This macro replaces all occurences of a British date "dd/mm/yyyy" by its ISO date equivalent "yyyy-MM-dd"
	//inside the reference fields "Year", "Date", "Date2" and "AccessDate" 
	public static void Main()
	{
		string targetDateFormat = "{2}-{1}-{0}";  	//{0}=day, {1}=month, {2}=year
													//US American: 	"{1}/{0}/{2}"
													//German: 		"{0}.{1}.{2}"
													//ISO: 			"{2}-{1}-{0}
		
		if (Program.ProjectShells.Count == 0) return;	//no project open	
		if (IsBackupAvailable() == false) return;		//user wants to backup his/her project first
		
		SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		List<Reference> references = Program.ActiveProjectShell.Project.References.ToList();		
		//List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
			
		if (references == null || !references.Any()) return;
		
		List<ReferencePropertyDescriptor> dateProperties = new List<ReferencePropertyDescriptor>() {
			ReferencePropertyDescriptor.Date,
			ReferencePropertyDescriptor.Date2,
			ReferencePropertyDescriptor.AccessDate,
			ReferencePropertyDescriptor.Year
		};
		
		Regex britishDateRegex = new Regex(@"(?<fulldate>(?<day>\d\d{1,2})/(?<month>\d\d{1,2})/(?<year>\d\d\d\d))");
		int hitCounter = 0;
		
		foreach (Reference reference in references)
		{
			foreach (ReferencePropertyDescriptor dateProperty in dateProperties)
			{
				string input = reference.GetValue(dateProperty.PropertyId) as string;
				if (string.IsNullOrEmpty(input)) continue;
				
				MatchCollection matches = britishDateRegex.Matches(input);
				if (matches == null || matches.Count == 0) continue;
				
				
				foreach(Match match in matches)
				{
					hitCounter++;
					string britishDate = match.Groups["fulldate"].Value;
					string year = match.Groups["year"].Value;
					string month = match.Groups["month"].Value;
					string day = match.Groups["day"].Value;
					
					string targetDate = string.Format(targetDateFormat, month, day, year);
					string isoDate = 
					input = Regex.Replace(input, britishDate, targetDate);

					//DebugMacro.WriteLine("Input: {0}\tRecognized Year: {1}\tRecognized Month: {2}\tRecognized Day: {3}\tAmerican Date: {4}", input, year, month, day, targetDate);
				}
				
				reference.SetValue(dateProperty.PropertyId, input);
			}
		}
		
		MessageBox.Show(string.Format("Finished.\r\n\r\n{0} dates were converted from British to US date format.", hitCounter));
	}
	
	
		private static bool IsBackupAvailable()
	{
		string warning = String.Concat("Important: This macro will make changes to your project.",
			"\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
			"\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
			"\r\n\r\n", "Do you want to continue?"
		);


		return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);


	} //end IsBackupAvailable()
}

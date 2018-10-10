using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

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
		
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
		if (references == null || references.Count == 0) return;
		
		int counter = 0;
		
		//NOTE: Change the order or listing of cultures in the following array, to prioritize certain cultures
		CultureInfo[] cultures = new CultureInfo[] {
			CultureInfo.GetCultureInfo("en-US"),
			CultureInfo.GetCultureInfo("de-DE"),
			CultureInfo.GetCultureInfo("es-ES"),
			CultureInfo.GetCultureInfo("pt-BR"),
			CultureInfo.GetCultureInfo("fr-FR"),
			CultureInfo.GetCultureInfo("pl-PL")
		};
		
		//NOTE: Change the target culture and format to your needs
		CultureInfo targetCulture = CultureInfo.GetCultureInfo("de-DE");
		string targetFormat = "d"; //Standard date ... differs from culture to culture, e.g. 12/24/2015 in en-US and 24.12.2015 in de-DE

		//NOTE: Change the reference fields where this macros looks for valid dates to convert
		ReferencePropertyDescriptor[] properties = new ReferencePropertyDescriptor[] {
			ReferencePropertyDescriptor.AccessDate,
			ReferencePropertyDescriptor.Date,
			ReferencePropertyDescriptor.Date2
		};
		
		//NOTE: Change the sequence and listing of format strings to look for
		string[] formatsToSearchFor = new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd-MMM-yy", "dd-MMM-yyyy" };
		
		
		foreach (Reference reference in references)
		{
			foreach (ReferencePropertyDescriptor property in properties)
			{
				var value = reference.GetValue(property.PropertyId) as string;
				if (value == null || string.IsNullOrWhiteSpace(value)) continue;
							
				DateTime dateFound = DateTime.MinValue;

				foreach (CultureInfo culture in cultures)
				{
					if (DateTime.TryParseExact(value, formatsToSearchFor, culture, DateTimeStyles.None, out dateFound))
					{
						reference.SetValue(property.PropertyId, dateFound.ToString(targetFormat, targetCulture)); 
						counter++;
						break;
					}
				}	
			}
		}
		
		string message = "{0} dates were changed to standard format"; 
		
		message = string.Format(message, counter);
		MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information); 
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
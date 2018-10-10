using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//This macro iterates through all linked PDF files and checks for a DOI inside the file.
		//If the reference that the file is linked to does not have a DOI already, it is added to the record.
		
		if (!Program.ProjectShells.Any()) return;		//no project open	
		if (IsBackupAvailable() == false) return;		//user wants to backup his/her project first
		
		int counter = 0;
		
		try 
		{
		
			SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
			string citaviFilesPath = activeProject.GetFolderPath(CitaviFolder.CitaviFiles);
			

			var fileLocations = activeProject.AllLocations.Where(location => 
				location.LocationType == LocationType.ElectronicAddress &&
				(
					location.AddressUri.AddressInfo == ElectronicAddressInfo.CitaviFiles || 
					location.AddressUri.AddressInfo == ElectronicAddressInfo.AbsoluteFileUri || 
					location.AddressUri.AddressInfo == ElectronicAddressInfo.RelativeFileUri
				)
			).ToList();

			
			var supporter = new ReferenceIdentifierSupport();
			
			
			foreach(Location location in fileLocations)
			{
				if (location.Reference == null) continue;
				if (!string.IsNullOrEmpty(location.Reference.Doi)) continue; //if DOI is already available, we do not scan the PDF
				
				string path = string.Empty;
				switch(location.AddressUri.AddressInfo)
				{
					case ElectronicAddressInfo.CitaviFiles:
						path = Path.Combine(citaviFilesPath, location.Address);
						//DebugMacro.WriteLine("CitaviFiles: " + path);
						break;
						
					case ElectronicAddressInfo.RelativeFileUri:
						path = location.AddressUri.AbsoluteUri.LocalPath;
						//DebugMacro.WriteLine("RelativeFileUri: " + path);
						break;
					
					case ElectronicAddressInfo.AbsoluteFileUri:
						path = location.Address;
						//DebugMacro.WriteLine("AbsoluteFileUri: " + path);
						break;
						
					default:
						continue;	
				}
				
				if (string.IsNullOrEmpty(path)) continue;
				if (!File.Exists(path)) continue;
				if (!Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase)) continue;
				
				var matches = supporter.FindIdentifierInFile(path, ReferenceIdentifierType.Doi, false);
				if (matches.Count == 0) continue;
				var match = matches.ElementAt(0);
				
				if (string.IsNullOrEmpty(location.Reference.Doi))
				{
					//DebugMacro.WriteLine(match.ToString());
					location.Reference.Doi = match.ToString();
					counter++;
				}
			}
	
		
		} //end try
		
		finally 
		{
			MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} changes were made.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} //end finally
		
	} //end main()
	
	
	
	
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
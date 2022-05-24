//CCO007
//Description: Convert Locations in Private Collection to Library Locations

using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

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
		//Private Sammlung > Bibliothek
		bool removePrivateCollectionLocation = true; //will remove the location information in a private collection after adding it as a library location
		
		if (IsBackupAvailable() == false) return;
		if (Program.ActiveProjectShell == null) return;
		if (Program.ActiveProjectShell.PrimaryMainForm == null) return;
		
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetSelectedReferences();
		if (references == null || !references.Any())
		{
			MessageBox.Show("Please filter for references whose locations in private collection should be transformed into library locations");
			return;
		}
		
		SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;

		int referenceCounter = 0;
		int locationCounter = 0;
		
		//List<Reference> selectedReferences = Program.ActiveProjectShell.Refer
		foreach(Reference reference in references)
		{
			List<Location> referenceLocations = reference.Locations.ToList();
			if (referenceLocations == null || !referenceLocations.Any()) continue;
			
			List<Location> referencePrivateLocations = referenceLocations.Where(item => item.LocationType == LocationType.PrivateCollection).ToList();
			if (referencePrivateLocations == null || !referencePrivateLocations.Any()) continue;
			referenceCounter++; //count references that DO have private locations
			
			foreach(Location l in referencePrivateLocations)
			{	
				locationCounter++; //count private locations converted into library locations
				string address = l.Address.ToString();
				string signature = l.CallNumber;
				string note = l.Notes;
				
				
				Library library = activeProject.Libraries.FindKey(address);
				if (library == null)
				{
					library = activeProject.Libraries.Add(address);
				}
				
				Location newLocation = reference.Locations.Add(library, signature);
				newLocation.Notes = note;
				
				if (removePrivateCollectionLocation)
				{
					reference.Locations.Remove(l);
				}
				
			}
		}
		
		string message = string.Format("Macro finished execution.\r\n\r\nWith {0} reference(s) an overall number of {1} locations in a private collection were converted into library locations.", 
			referenceCounter.ToString(), 
			locationCounter.ToString());
		
		MessageBox.Show(message, "Citavi", MessageBoxButtons.OK);
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

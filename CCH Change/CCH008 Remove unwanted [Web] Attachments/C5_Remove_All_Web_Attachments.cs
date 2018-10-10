using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Citavi.DataExchange;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//****************************************************************************************************************
		// REMOVE ALL WEB LOCATIONS
		// 1.1 -- 20156-05-17
        //          -- 1.1: keeps OnlineAddress location by default
        //
        // Removes all [Web] locations for a reference (DOI link can be kept optionally)
		//
		//
		// EDIT HERE
		// Variables to be changed by user

        bool keepDoi = true; // if false, also DOI links will be removed
        bool keepOnlineAddress = true; // if true, Location will be kept if the URL is stored in the references OnlineAddress field

		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first

			
		int foundCounter = 0;
		int changeCounter = 0;
		int errorCounter = 0;
		

		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();

		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;


		foreach (Reference reference in references)
		{
            if (reference.Locations == null || reference.Locations.Count == 0) continue;
            
            List<Location> locationsToBeRemoved = new List<Location>();
            foreach (Location location in reference.Locations)
            {
                if (location.LocationType == LocationType.ElectronicAddress)
                {
                    if (location.AddressUri.AddressInfo == ElectronicAddressInfo.RemoteUri)
                    {
                        if (keepOnlineAddress && location.Address == reference.OnlineAddress) continue;
                        else if (keepDoi && location.AddressUri.AbsoluteUri.Host == "dx.doi.org") continue;
                        else
                        {
                            locationsToBeRemoved.Add(location);
                            foundCounter++;  
                        }                        
                    }           
                                        
                }
             }
  
             try
             {
                reference.Locations.RemoveRange(locationsToBeRemoved);
                changeCounter += locationsToBeRemoved.Count;    
             }
             catch (Exception e)
             {
               errorCounter += locationsToBeRemoved.Count;
               DebugMacro.WriteLine(reference.ShortTitle + " - An error occurred: " + e.Message);
             }	
		}

		activeProject.Save();

		// Message upon completion
		string message = " {0} locations(s) found\n {1} locations(s) removed\n {2} locations(s) with errors";
		message = string.Format(message, foundCounter.ToString(), changeCounter.ToString(), errorCounter.ToString());
		MessageBox.Show(message, "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
	}

	// Ask whether backup is available
	private static bool IsBackupAvailable()
	{
		string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
			"\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
			"\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
			"\r\n\r\n", "Do you want to continue?"
		);

		return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);
	}


}
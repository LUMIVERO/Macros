//Version 2.1

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

// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//****************************************************************************************************************
		// UPDATE BIBLIOGRAPHIC DATA FROM PMID OR DOI-SEARCH
		// Version 2.2 -- 2019-04-09
        //    -- updated to work with Citavi 6
		//
		// This macro iterates through the references in a selection ("filter").
		// If they have a DOI or PMID, it downloads bibliographical data and owverwrites the reference's data.
		// PMID is given priority over DOI, i.e. if both are present, data will be loaded from PubMed.
		//
		//
		// EDIT HERE
		// Variables to be changed by user
		
		bool overwriteAbstract = false;				// if true, existing Abstract will be replaced
		bool overwriteTableOfContents = false;		// if true, existing TOC will be replaced
		bool overwriteKeywords = false;				// if true, existing Keywords will be replaced
		bool clearNotes = true;					// if true, Notes field will be emptied
	
		
		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (!Program.ProjectShells.Any()) return;		//no project open	
		if (IsBackupAvailable() == false) return;		//user wants to backup his/her project first
		
		int counter = 0;
		
		try 
		{
	
			
			List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
			List<Reference> referencesWithDoi = references.Where(reference => !string.IsNullOrEmpty(reference.PubMedId) || !string.IsNullOrEmpty(reference.Doi)).ToList();
			
			SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
			
			var identifierSupport = new ReferenceIdentifierSupport();
			string message = "Updating reference {0} of {1} - '{2}' ... {3}";
			int currentRef = 0;
			int overallRefs = referencesWithDoi.Count();
			
			foreach (Reference reference in referencesWithDoi)
			{
				currentRef++;
                
				
				// Look up PMID. If that fails, look up DOI. If no data is found, move on.
				DebugMacro.WriteLine(string.Format(message, currentRef.ToString(), overallRefs.ToString(), reference.ShortTitle, ""));
				Reference lookedUpReference = identifierSupport.FindReference(activeProject.Engine, activeProject, new ReferenceIdentifier() { Type = ReferenceIdentifierType.PubMedId, Value = reference.PubMedId });
				if (lookedUpReference == null)
				{				
					lookedUpReference = identifierSupport.FindReference(activeProject.Engine, activeProject, new ReferenceIdentifier() { Type = ReferenceIdentifierType.Doi, Value = reference.Doi });
					if (lookedUpReference == null) 
					{
						DebugMacro.WriteLine(string.Format(message, currentRef.ToString(), overallRefs.ToString(), reference.ShortTitle, "No data found!"));
						continue;
					}
				}
				
				//merge reference & lookedUpReference, overwriting bibliographic data of the former
				List<ReferencePropertyId> omitData = new List<ReferencePropertyId>();
				omitData.Add(ReferencePropertyId.CoverPath);
				omitData.Add(ReferencePropertyId.Locations);
				
				if (!overwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
				if (!overwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
				if (!overwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);
				
				reference.MergeReference(lookedUpReference, true, omitData);
								
				counter++;
	
				if (!string.IsNullOrEmpty(reference.Notes) && clearNotes) reference.Notes = string.Empty;	//empty notes field
				if (activeProject.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = activeProject.BibTeXKeyAssistant.GenerateKey(reference);
				if (activeProject.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = activeProject.CitationKeyAssistant.GenerateKey(reference);
			}
		
		
		
		
		} //end try
		
		finally 
		{
			MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} references were updated.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} //end finally
		
	} //end main()

	
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
	//end IsBackupAvailable()
}
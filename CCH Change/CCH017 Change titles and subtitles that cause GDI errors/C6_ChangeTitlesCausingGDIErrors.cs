using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;


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
		// Shorten Titles Causing GDI+ Errors
		// 1.0 - 2022-05-24
		//
		// Shortens Titles and Subtitles causing GDI+ errors (red X over list of references)
		//
		// EDIT HERE
		// Variables to be changed by user
			
				
		// DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
				
		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;
		
		List<Reference> references = activeProject.References.ToList();
		
		foreach (Reference reference in references)
		{
			if (reference.Title.Length > 9999)
			{
				string shorterTitle = reference.Title.Substring(0,100);
				reference.Title = shorterTitle;
			}
			
			if (reference.Subtitle.Length > 9999)
			{
				string shorterSubTitle = reference.Subtitle.Substring(0,100);
				reference.Subtitle = shorterSubTitle;
			}			
		}




		List<KnowledgeItem> allKis = activeProject.AllKnowledgeItems.ToList();
		
		List<KnowledgeItem> noEntityLink = new List<KnowledgeItem>();
		
		try
		{
			foreach (KnowledgeItem ki in allKis)
			{
				if (ki.QuotationType == QuotationType.None || ki.QuotationType == QuotationType.Highlight) continue;
				if (ki.EntityLinks == null || ki.EntityLinks.Count() == 0) noEntityLink.Add(ki);			

			}
			

			if (noEntityLink.Count > 0)	
			{
				noEntityLink = noEntityLink.Distinct().ToList();
				KnowledgeItemFilter kiFilter = new KnowledgeItemFilter(noEntityLink, "Knowledge items without entity links", false);
				List<KnowledgeItemFilter> kiFilters = new List<KnowledgeItemFilter>();
				kiFilters.Add(kiFilter);
				Program.ActiveProjectShell.PrimaryMainForm.KnowledgeOrganizerFilterSet.Filters.ReplaceBy(kiFilters);
			}
		}
		catch (Exception e)
		{
			DebugMacro.WriteLine("An error occurred: " + e.Message);
		}		
		finally
		{
			if (noEntityLink.Count == 0)
			{
				MessageBox.Show("No such KI found.", "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show(string.Format("{0} reference(s) in new selection", noEntityLink.Count), "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
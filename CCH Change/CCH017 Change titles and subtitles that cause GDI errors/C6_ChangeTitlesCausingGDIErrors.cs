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
		// 1.1 - 2022-11-11 - added Categories
		// 1.2 - 2024-07-26 - added Journal Names
		//
		// Shortens entity names causing GDI+ errors (red X over list of references):
		// - Titles and Subtitles
		// - Categories
		// - Journal Names
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
		
		List<Category> categories = activeProject.AllCategories.ToList();
		
		foreach (Category category in categories)
		{
			if (category.FullName.Length > 500)
			{
				string shortName = category.Name.Substring(0,100);
				category.Name = shortName;
			}
		}
		
		List<Periodical> periodicals = activeProject.Periodicals.ToList();
		
		foreach (Periodical periodical in periodicals)
		{
			if (periodical.FullName.Length > 500)
			{
				string shortName = periodical.Name.Substring(0,100);
				periodical.Name = shortName;
			}
		}
	}
}
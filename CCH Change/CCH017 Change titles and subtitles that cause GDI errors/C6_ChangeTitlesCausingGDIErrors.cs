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
	}
}

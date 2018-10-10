using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Persistence;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//Purpose: 	If running under C4: 	EXPORT of project data in XML
		//			If running under C3: 	(RE)IMPORT of the same data 
		
		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
		
		string xmlFile = string.Empty;
		string initialDirectory = Program.Engine.EngineInfo.GetFolderPath(CitaviFolder.Projects, activeProject);

		
		string citaviVersion = SwissAcademic.Environment.InformationalVersion.ToString(4);
		if (citaviVersion.StartsWith("4"))
		{
			//EXPORT			
			
			//ask for file name & path
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter =  "XML files (*.xml)|*.xml|All files (*.*)|*.*";
				saveFileDialog.InitialDirectory = initialDirectory;
				saveFileDialog.Title = "Enter an XML file name for export of Citavi project data.";	

				if (saveFileDialog.ShowDialog(primaryMainForm) != DialogResult.OK) return;
				xmlFile = saveFileDialog.FileName;
			}
			
			//ProjectSaverXml.Save(xmlFile, activeProject, null, null);		//C3
			ProjectSaverXml.Save(xmlFile, Program.ActiveProjectShell.Project, new CancellationToken(false), null, null);	//C4
			
			MessageBox.Show("Finished");
		
		}
		

		

	
		
	}
}
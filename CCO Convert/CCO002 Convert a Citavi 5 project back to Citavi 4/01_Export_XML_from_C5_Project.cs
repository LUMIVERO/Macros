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
using SwissAcademic.Citavi.Persistence;
using System.Threading;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//EXPORT from C5
		string citaviVersion = SwissAcademic.Environment.InformationalVersion.ToString(4);
		DebugMacro.WriteLine(citaviVersion);
		if (!(citaviVersion.StartsWith("5") || citaviVersion.StartsWith("4.9."))) return;
		
		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
		
		string xmlFile = string.Empty;
		string ctv3File = string.Empty;
		
		string initialDirectory = Program.Engine.DesktopEngineConfiguration.GetFolderPath(CitaviFolder.Projects, activeProject);

		//ask for file name & path
		using (SaveFileDialog saveFileDialog = new SaveFileDialog())
		{
			saveFileDialog.Filter =  "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			saveFileDialog.InitialDirectory = initialDirectory;
			saveFileDialog.Title = "Enter an XML file name for export of Citavi project data.";	

			if (saveFileDialog.ShowDialog(primaryMainForm) != DialogResult.OK) return;
			xmlFile = saveFileDialog.FileName;
		}
		
	
		var compatibility = ProjectXmlExportCompatibility.Citavi4;
		ProjectToXml.Write(xmlFile, activeProject, CancellationToken.None, null, null, compatibility);
		MessageBox.Show("Finished");	
	}
}
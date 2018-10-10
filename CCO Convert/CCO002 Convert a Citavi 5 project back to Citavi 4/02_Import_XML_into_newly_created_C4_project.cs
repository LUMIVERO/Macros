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
		
		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
		
		string xmlFile = string.Empty;
		string ctv4File = string.Empty;
		string initialDirectory = Program.Engine.EngineInfo.GetFolderPath(CitaviFolder.Projects, activeProject);

		//(RE)IMPORT
		using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog.Title = "Select an XML file with Citavi project data for import";	
				
			if (openFileDialog.ShowDialog() != DialogResult.OK) return;
			xmlFile = openFileDialog.FileName;
			if (string.IsNullOrEmpty(xmlFile)) return;
			if (!File.Exists(xmlFile)) return;
		}

		bool goOn = true;
		while (goOn)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter =  "CTV4 files (*.ctv4)|*.ctv4";
				saveFileDialog.InitialDirectory = initialDirectory;
				saveFileDialog.Title = "Enter a CTV4 file name to create a new C4 project with the imported data";

				if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
				ctv4File = saveFileDialog.FileName;
			}
				
			if (string.IsNullOrEmpty(ctv4File)) return;
				
			if (File.Exists(ctv4File))
			{
				if (MessageBox.Show(string.Format("Do you want to overwrite the file {0}?", ctv4File), "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) goOn = false;			
			}
			else
			{
				goOn = false;
			}
		}
			
		ProjectCreationInfo pc = new ProjectCreationInfo();

		pc.FilePath = ctv4File;
		Program.Engine.Projects.RestoreFromXml(xmlFile, pc, null);
			
		MessageBox.Show("Finished");
	}
}
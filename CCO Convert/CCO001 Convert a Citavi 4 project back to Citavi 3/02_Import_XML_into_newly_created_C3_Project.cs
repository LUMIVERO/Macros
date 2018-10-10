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
		string ctv3File = string.Empty;
		string initialDirectory = Program.Engine.EngineInfo.GetFolderPath(CitaviFolder.Projects, activeProject);

		
		string citaviVersion = SwissAcademic.Environment.InformationalVersion.ToString(4);
		if (citaviVersion.StartsWith("3"))
		{
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

			initialDirectory = Path.GetFullPath(xmlFile);
			bool goOn = true;
			while (goOn)
			{
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.Filter =  "CTV3 files (*.ctv3)|*.ctv3";
					saveFileDialog.InitialDirectory = initialDirectory;
					saveFileDialog.Title = "Enter a CTV3 file name to create a new C3 project with the imported data";

					if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
					ctv3File = saveFileDialog.FileName;
				}
				
				if (string.IsNullOrEmpty(ctv3File)) return;
				
				if (File.Exists(ctv3File))
				{
					if (MessageBox.Show(string.Format("Do you want to overwrite the file {0}?", ctv3File), "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) goOn = false;			
				}
				else
				{
					goOn = false;
				}
			}
			
			ProjectCreationInfo pc = new ProjectCreationInfo();

			pc.FilePath = ctv3File;
			Program.Engine.Projects.RestoreFromXml(xmlFile, pc, null);
			
			MessageBox.Show("Finished");
		}
	}
}
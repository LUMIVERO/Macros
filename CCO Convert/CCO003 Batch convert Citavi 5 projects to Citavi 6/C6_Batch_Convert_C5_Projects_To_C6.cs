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

using System.Threading;
using System.Threading.Tasks;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		Convert();
	}
	
	public static async Task Convert()
	{
		DirectoryInfo sourceFolder;
		DirectoryInfo targetRootFolder;
		using (FolderBrowserDialog fbd = new FolderBrowserDialog())
		{
			fbd.Description = "Quellordner der Citavi 5-Projekte";
			if (fbd.ShowDialog() != DialogResult.OK) return;
			sourceFolder = new DirectoryInfo(fbd.SelectedPath);
			
			fbd.Description = "Zielordner der Citavi 6-Projekte";
			if (fbd.ShowDialog() != DialogResult.OK) return;
			targetRootFolder = new DirectoryInfo(fbd.SelectedPath);
		}
		
		FileInfo[] projectFiles = sourceFolder.GetFiles("*.ctv5", SearchOption.AllDirectories);
		
		if (!projectFiles.Any())
		{
			MessageBox.Show("Keine Citavi 5-Projekte gefunden.");
			return;
		}
		
		CancellationTokenSource cts = new CancellationTokenSource();
		try
		{
			foreach (FileInfo projectFile in projectFiles)
			{			
				List<DirectoryInfo> parentFolders = new List<DirectoryInfo>();
				DirectoryInfo parentFolder = projectFile.Directory.Parent;
				while (parentFolder.FullName != sourceFolder.FullName)
				{
					parentFolders.Insert(0, parentFolder);
					parentFolder = parentFolder.Parent;
				}			
				
				DirectoryInfo targetFolder = targetRootFolder;
				foreach (DirectoryInfo di in parentFolders)
				{
					targetFolder = new DirectoryInfo(Path.Combine(targetFolder.FullName, di.Name));
					if (!targetFolder.Exists) targetFolder.Create();
				}
				
				DesktopProjectConfiguration config = await DesktopProjectConfiguration.OpenAsync(Program.Engine, ProjectType.DesktopSQLite, projectFile.FullName, ignoreLicense: true, cancellationToken: CancellationToken.None);
				
	            var projectFilePath = Program.Engine.Projects.CreateProjectFolders(targetFolder, Path.GetFileNameWithoutExtension(projectFile.Name));
				GenericProgressDialog.RunTask(Program.ActiveProjectShell.PrimaryMainForm, config.SQLiteProjectInfo.ConvertToCurrentVersionAsync, Program.Engine, projectFilePath, string.Format("Konvertiere {0}", projectFile.Name), null, cts);
				cts.Token.ThrowIfCancellationRequested();
			}
			
			MessageBox.Show(Program.ActiveProjectShell.PrimaryMainForm, "Alle Projekte wurden konvertiert.", "Citavi");
		}
		catch (OperationCanceledException x)
		{
			MessageBox.Show(Program.ActiveProjectShell.PrimaryMainForm, "Die Konvertierung wurde abgebrochen.", "Citavi");
		}
		catch (Exception x)
		{
			MessageBox.Show(Program.ActiveProjectShell.PrimaryMainForm, string.Format("Ein Fehler ist aufgetreten:\r\n\r\n{0}", x.Message), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
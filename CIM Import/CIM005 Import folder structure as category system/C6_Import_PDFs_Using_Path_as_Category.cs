using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		if (Program.ProjectShells.Count == 0) return;		//no project open	
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first
		
		int counter = 0;
		
		try 
		{	
			string sourcePath;
					
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.Description = "Select a root folder for the PDF files to be imported ...";
			DialogResult folderResult = folderDialog.ShowDialog();
			if (folderResult == DialogResult.OK)
			{
				sourcePath = folderDialog.SelectedPath;
			}
			else return;
			
			AttachmentAction action = AttachmentAction.Copy;
						
			DirectoryInfo dir = new DirectoryInfo(sourcePath);
			List<FileInfo> fileInfos = Path2.GetFilesSafe(dir, "*.pdf", SearchOption.AllDirectories).ToList();		
			
			List<Reference> newReferences = new List<Reference>();
			SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
			
			List<string> filePaths = fileInfos.Select<FileInfo, string>(info => info.FullName).ToList();
			foreach(string filePath in filePaths)
			{
				DebugMacro.WriteLine("START IMPORT: " + filePath);
				List<Reference> referencesFromFile = new FileImportSupport().ImportFiles(activeProject, activeProject.Engine.TransformerManager, new List<string>() { filePath }, action);
				
				if (referencesFromFile != null && referencesFromFile.Any())
				{
					var referencesFromFileAdded = activeProject.References.AddRange(referencesFromFile);
					
					var fileName = Path.GetFileName(filePath);
					AddCategories(referencesFromFileAdded, filePath.Substring(sourcePath.Length, filePath.Length - sourcePath.Length - fileName.Length));
					DebugMacro.WriteLine("SUCCESS");
					counter++;
				}
				else
				{
					DebugMacro.WriteLine("ERROR importing file");
				}
			}
			
			
		} //end try
		
		finally 
		{
			MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} changes were made.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} //end finally
		
	} //end main()

	private static void AddCategories(IEnumerable<Reference> references, string categoryHierarchy)
	{
		DebugMacro.WriteLine(categoryHierarchy);
		
		List<string> categoryStrings = categoryHierarchy.Split(new Char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
		if (categoryStrings == null || !categoryStrings.Any()) return;
		if (references == null || !references.Any()) return;
		

		#region Add Categories to Project if required
		
		Project project = references.First().Project;
		string currentCategoryString = categoryStrings.FirstOrDefault();
		Category currentCategory = project.Categories.FirstOrDefault(item => item.Name.Equals(currentCategoryString, StringComparison.Ordinal));
		if (currentCategory == null) currentCategory = project.Categories.Add(currentCategoryString);
			
		for(int i = 1; i < categoryStrings.Count; i++)
		{
			currentCategoryString = categoryStrings.ElementAt(i); 
			Category existingCategory = currentCategory.Categories.FirstOrDefault(item => item.Name.Equals(currentCategoryString, StringComparison.Ordinal));
			if (existingCategory == null)
			{
				currentCategory = currentCategory.Categories.Add(currentCategoryString);	
			}
			else
			{
				currentCategory = existingCategory;
			}
		}
		
		#endregion
		
		#region Add Category to Reference
		
		foreach(Reference reference in references)
		{
			reference.Categories.Add(currentCategory);
		}
		
		#endregion
	}

	private static bool IsBackupAvailable() 
	{
		string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
			"\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
			"\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
			"\r\n\r\n", "Do you want to continue?"
		);
				
		
		return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);


	} //end IsBackupAvailable()
	
}
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
			//reference to the active project and the main window
			Form primaryMainForm = Program.ActiveProjectShell.PrimaryMainForm;
			if(primaryMainForm == null) return;
			
			SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
			if (activeProject == null) return;
			
			if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first
			
			//if no reference filters applied, macro cannot operate
			if (Program.ActiveProjectShell.PrimaryMainForm.ReferenceEditorFilterSet.Filters.Count == 0)
			{
				string message = "This macro requires a selection of references to operate on.\r\n";
				message += "Please select some references with PDF files attached.";
				MessageBox.Show(message, "Citavi");
				return;
			}
			
			//path to the current project's CitaviFiles folder
			string citaviFilesPath = activeProject.GetFolderPath(CitaviFolder.CitaviFiles);			
			
			List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
			//List<Reference> references = CollectionUtility.ToList(activeProject.References);	
			
			#region Generate list of file locations to move			

			//first we create a list of files attached to the currently selected references, 
			//having a PDF extension,
			//and existing inside the CitaviFiles folder
			List<FileMoveOperation> fileMoveOperations = new List<FileMoveOperation>();
			
			foreach (Reference reference in references)
			{
				foreach (Location location in reference.Locations)
				{
					if (location.LocationType != LocationType.ElectronicAddress) continue;
					
					if (location.AddressUri.AddressInfo == ElectronicAddressInfo.CitaviFiles)
					{
						if (Path.GetExtension(location.Address).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
						{
							FileMoveOperation fileMoveOperation = new FileMoveOperation(location);
							fileMoveOperations.Add(fileMoveOperation);
							//MessageBox.Show(fileMoveOperation.SourcePath);
							//MessageBox.Show(fileMoveOperation.OriginalFileName);
						}
					}
				}
			}
			
			if (fileMoveOperations.Count == 0) 
			{
				string message = string.Format("The {0} selected reference(s) do not have any PDF files attached, that are stored inside the CitaviFiles folder.", references.Count);
				MessageBox.Show(message, "Citavi");
				return;
			}
			
			#endregion Generate list of file locations to move
			
			#region Prompt user for target folder to move files to
			
			string targetFolderPath = string.Empty;
			
			using (var folderPicker = new CommonOpenFileDialog())
			{
				folderPicker.IsFolderPicker = true;
				folderPicker.Title = string.Format("Select the folder where you want to move the {0} PDF files to.", fileMoveOperations.Count);
				folderPicker.InitialDirectory = Program.Settings.InitialDirectories.GetInitialDirectory(SwissAcademic.Citavi.Settings.InitialDirectoryContext.LocalFile, null);
				if (folderPicker.ShowDialog() == CommonFileDialogResult.Ok)
				{
					targetFolderPath = folderPicker.FileName;
				}
				else
				{
					MessageBox.Show("Macro execution cancelled upon user's request.", "Citavi");
					return;
				}
			}
			
			
			
			#endregion Prompt user for target folder to move files to
			
			#region Copy the files to the new folder
			
			DirectoryInfo targetDirectory = new DirectoryInfo(targetFolderPath); 
			
			foreach(FileMoveOperation fileMoveOperation in fileMoveOperations)
			{
				//avoid overwriting a possible existing file
				fileMoveOperation.TargetPath = Path2.GetUniqueFilePath(targetDirectory, fileMoveOperation.OriginalFileName);
				try
				{
					File.Copy(fileMoveOperation.SourcePath, fileMoveOperation.TargetPath, false);
					fileMoveOperation.CopySuccess = true;
				}
				catch(Exception exception)
				{
					fileMoveOperation.Errors.Add(exception);
					fileMoveOperation.CopySuccess = false;
				}
			}
			
			#endregion Copy the files to the new region

			#region Relink each reference to the new files
			
			foreach(FileMoveOperation fileMoveOperation in fileMoveOperations)
			{
				if (fileMoveOperation.CopySuccess)
				{
					try
					{
						fileMoveOperation.TargetLocation = fileMoveOperation.SourceLocation.Reference.Locations.Add(LocationType.ElectronicAddress, fileMoveOperation.TargetPath, string.Empty, false);
						fileMoveOperation.TargetLocation.Notes = fileMoveOperation.SourceLocation.Notes;
						fileMoveOperation.RelinkSuccess = true;
					}
					catch(Exception exception)
					{
						fileMoveOperation.Errors.Add(exception);
						fileMoveOperation.RelinkSuccess = false;
					}
				}
			}
			
			#endregion Relink each reference to the new files
			
			#region Delete the original locations and move the files in the CitaviFiles folder to CitaviFiles\RecycleBin
			
			foreach(FileMoveOperation fileMoveOperation in fileMoveOperations)
			{
				if (fileMoveOperation.RelinkSuccess)
				{
					try
					{
						Location locationToDelete = fileMoveOperation.SourceLocation;
						Project locationProject = locationToDelete.Project;
											
						if (locationToDelete == null) continue;
						
						ElectronicAddressUri addressUriToDelete = locationToDelete.AddressUri;
						locationToDelete.Reference.Locations.Remove(locationToDelete);
						Program.ActiveProjectShell.Save(primaryMainForm);
						
						
						DirectoryInfo recycleBinDirectory = new DirectoryInfo(locationProject.GetFolderPath(CitaviFolder.RecycleBin));
						string deletedFilePath = Path2.GetUniqueFilePath(recycleBinDirectory, fileMoveOperation.OriginalFileName);
						File.Move(fileMoveOperation.SourcePath, deletedFilePath);
						
						fileMoveOperation.DeleteSuccess = true;
					}
					catch(Exception exception)
					{
						fileMoveOperation.Errors.Add(exception);
						fileMoveOperation.DeleteSuccess = false;
					}
					
				}
			}
			
			#endregion Delete the original locations and move the files in the CitaviFiles folder to CitaviFiles\RecycleBin
			
			
			
			MessageBox.Show("Macro has finished execution.", "Citavi");
			
	}
	
	class FileMoveOperation
	{
		#region Internal Members
		
		//Path denotes a fully qualified file path including its name and extension (which distringuihes it from Folder or Directory)
		//FileName denotes just the filename and extension
		Location _sourceLocation;
		Location _targetLocation;
		
		string _sourcePath;
		string _targetPath;
		
		bool? _success = null;
		bool? _copySuccess = null;
		bool? _relinkSuccess = null;
		bool? _deleteSuccess = null;
		
		List<Exception> _errors;
		
		#endregion Internal Members
		
		#region Constructor
		
		//constructor
		public FileMoveOperation(Location sourceLocation)
		{
			_sourceLocation = sourceLocation;
		}
		
		#endregion Constructor
		
		#region SourceLocation
		
		public Location SourceLocation
		{
			get
			{
				return _sourceLocation;
			}
		}
		
		#endregion SourceLocation
		
		#region SourcePath
		
		public string SourcePath
		{
			get
			{
				if (SourceLocation == null) return null;
				return Path.Combine(SourceLocation.Project.GetFolderPath(CitaviFolder.CitaviFiles), SourceLocation.Address);
			}
		}
		
		#endregion SourcePath
		
		#region OriginalFileName
		
		public string OriginalFileName
		{
			get
			{
				if (SourceLocation == null) return null;
				return Path.GetFileName(SourcePath);
			}
		}
		
		#endregion OriginalFileName
		
		#region TargetPath
		
		public string TargetPath
		{
			get
			{
				return _targetPath;
			}
			
			set
			{
				_targetPath = value;
			}
		}
		
		#endregion TargetPath
		
		#region TargetLocation
		
		public Location TargetLocation
		{
			get
			{
				return _targetLocation;
			}
			set
			{
				_targetLocation = value;
			}
		}
		
		#endregion TargetLocation
		
		#region Errors
		
		public List<Exception> Errors
		{
			get
			{
				if (_errors == null) return new List<Exception>();
				return _errors;
			}
		}
		
		#endregion Errors
		
		#region Success
		
		public bool Success
		{
			get
			{
				bool copySuccess = false;
				if (_copySuccess.HasValue && _copySuccess.Value) copySuccess = true;
				
				bool relinkSuccess = false;
				if (_relinkSuccess.HasValue && _relinkSuccess.Value) relinkSuccess = true;
				
				bool deleteSuccess = false;
				if (_deleteSuccess.HasValue && _deleteSuccess.Value) deleteSuccess = true;
				
				return copySuccess && relinkSuccess && deleteSuccess;
			}
		}
		
		#endregion Success
		
		#region CopySuccess
		
		public bool CopySuccess
		{
			get
			{
				bool copySuccess = false;
				if (_copySuccess.HasValue && _copySuccess.Value) copySuccess = true;
				return copySuccess;
			}
			set
			{
				_copySuccess = value;
			}
		}
		
		#endregion CopySuccess
		
		#region RelinkSuccess
		
		public bool RelinkSuccess
		{
			get
			{
				bool relinkSuccess = false;
				if (_relinkSuccess.HasValue && _relinkSuccess.Value) relinkSuccess = true;
				return relinkSuccess;
			}
			set
			{
				_relinkSuccess = value;
			}
		}
		
		#endregion RelinkSuccess
		
		#region DeleteSuccess
		
		public bool DeleteSuccess
		{
			get
			{
				bool deleteSuccess = false;
				if (_deleteSuccess.HasValue && _deleteSuccess.Value) deleteSuccess = true;
				return deleteSuccess;
			}
			set
			{
				_deleteSuccess = value;
			}
		}
		
		#endregion DeleteSuccess
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
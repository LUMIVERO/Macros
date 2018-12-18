using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;

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
        //REQUIRES Citavi 5

        //reference to the active project and the main window
        System.Windows.Forms.Form primaryMainForm = Program.ActiveProjectShell.PrimaryMainForm;
        if (primaryMainForm == null) return;

        SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
        if (activeProject == null) return;

        string projectCitaviFilesFolderPath = activeProject.Addresses.AttachmentsFolderPath;

        //MessageBox.Show(projectCitaviFilesFolderPath);

        string compareFolderPath = string.Empty;
        compareFolderPath = projectCitaviFilesFolderPath;
        //if you want to check files and their links to the current Citavi project from a different folder, use something like:
        //compareFolderPath = @"D:\MyFiles";


        var references = activeProject.References.ToArray();
        var knowledgeItems = activeProject.AllKnowledgeItems.ToArray();

        List<string> pathsToFilesToCompare = new List<string>();
        List<string> pathsToLinkedFiles = new List<string>();
        List<string> pathsToLinkedFilesWithDeadLinks = new List<string>();

        string[] filePaths = Directory.GetFiles(compareFolderPath);
        pathsToFilesToCompare.AddRange(filePaths);
        if (pathsToFilesToCompare.Count == 0) return;


        //C5 save to desktop unless SQLite project
        string activeProjectFolderPath = String.Empty;

        if (activeProject.DesktopProjectConfiguration.ProjectType == ProjectType.DesktopSQLite)
        {
            string activeProjectFilePath = activeProject.DesktopProjectConfiguration.SQLiteProjectInfo.FilePath;
            activeProjectFolderPath = Path.GetDirectoryName(activeProjectFilePath);
        }
        else
        {
            activeProjectFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + activeProject.Name;
            if (!Directory.Exists(activeProjectFolderPath))
            {
                Directory.CreateDirectory(activeProjectFolderPath);
            }
        }


        //MessageBox.Show(activeProjectFolderPath);

        //C5: Attachments path			
        string citaviFilesPath = activeProject.Addresses.AttachmentsFolderPath;

        //MessageBox.Show(citaviFilesPath);

        //iterate through all electronic location files
        foreach (Reference reference in references)
        {
            foreach (Location location in reference.Locations)
            {
                if (location.LocationType != LocationType.ElectronicAddress) continue;

                if (location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile)
                {
                    string path = Path.Combine(citaviFilesPath, location.Address);
                    pathsToLinkedFiles.Add(path);
                    //MessageBox.Show("CitaviFiles:\r\n" + path);

                    if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                    continue;
                }
                else if (location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri)
                {
                    string path = location.Address.Resolve().LocalPath;
                    pathsToLinkedFiles.Add(path);
                    //MessageBox.Show("RelativeFileUri:\r\n" + path);

                    if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                    continue;
                }
                else if (location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri)
                {
                    string path = location.Address;
                    pathsToLinkedFiles.Add(path);
                    //MessageBox.Show("AbsoluteFileUri:\r\n" + path);

                    if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                    continue;
                }

                //all others: not applicable

            }
        }



        //iterate through all knowledge item files
        foreach (KnowledgeItem knowledgeItem in knowledgeItems)
        {
            if (knowledgeItem.KnowledgeItemType != KnowledgeItemType.File) continue;

            if (knowledgeItem.Address.LinkedResourceType == LinkedResourceType.AttachmentFile)
            {
                string path = Path.Combine(citaviFilesPath, knowledgeItem.Address);
                pathsToLinkedFiles.Add(path);
                //MessageBox.Show("CitaviFiles:\r\n" + path);

                if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                continue;
            }
            else if (knowledgeItem.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri)
            {
                string path = knowledgeItem.Address.Resolve().LocalPath;
                pathsToLinkedFiles.Add(path);
                //MessageBox.Show("RelativeFileUri:\r\n" + path);

                if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                continue;
            }
            else if (knowledgeItem.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri)
            {
                string path = knowledgeItem.Address;
                pathsToLinkedFiles.Add(path);
                //MessageBox.Show("AbsoluteFileUri:\r\n" + path);

                if (!File.Exists(path)) pathsToLinkedFilesWithDeadLinks.Add(path);

                continue;
            }
        }



        //sort the lists
        pathsToLinkedFiles.Sort();
        pathsToFilesToCompare.Sort();

        //make them unique
        pathsToLinkedFiles = pathsToLinkedFiles.Distinct().ToList();
        //pathsToFilesToCompare = pathsToFilesToCompare.Distinct().ToList(); unnecessary

        //generate list of differences between the two lists
        List<string> pathsToFilesNOTLinked = new List<string>();
        List<string> pathsToFilesLinked = new List<string>();
        bool found;
        foreach (string path in pathsToFilesToCompare)
        {
            found = false;

            foreach (string pathToFileLinked in pathsToLinkedFiles)
            {
                if (path.Equals(pathToFileLinked, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }

            }

            if (!found)
            {
                pathsToFilesNOTLinked.Add(path);
            }
            else
            {
                pathsToFilesLinked.Add(path);
            }
        }

        string logPathsToLinkedFiles = Path.Combine(activeProjectFolderPath, "ALL_Files_LINKED_From_Project.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPathsToLinkedFiles))
        {
            foreach (string path in pathsToLinkedFiles)
            {
                file.WriteLine(path);
            }
        }


        string logPathsToLinkedFilesWithDeadLinks = Path.Combine(activeProjectFolderPath, "ALL_Files_LINKED_From_Project_with_DEAD_Links.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPathsToLinkedFilesWithDeadLinks))
        {
            foreach (string path in pathsToLinkedFilesWithDeadLinks)
            {
                file.WriteLine(path);
            }
        }



        string logPathsToFilesToCompare = Path.Combine(activeProjectFolderPath, "Particular_Files_Checked_For_Link_From_Project.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPathsToFilesToCompare))
        {
            foreach (string path in pathsToFilesToCompare)
            {
                file.WriteLine(path);
            }
        }


        string logPathsToFilesLinked = Path.Combine(activeProjectFolderPath, "Particular_Files_LINKED_From_Project.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPathsToFilesLinked))
        {
            foreach (string path in pathsToFilesLinked)
            {
                file.WriteLine(path);
            }
        }


        string logPathsToFilesNOTLinked = Path.Combine(activeProjectFolderPath, "Particular_Files_NOT_LINKED_From_Project.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPathsToFilesNOTLinked))
        {
            foreach (string path in pathsToFilesNOTLinked)
            {
                file.WriteLine(path);
            }
        }



        var message2 = "Macro has finished execution.\r\nThe following files were created in your project's folder:\r\n\r\n";
        message2 += "ALL_Files_LINKED_From_Project.txt\r\n";
        message2 += "ALL_Files_LINKED_From_Project_with_DEAD_Links.txt\r\n";
        message2 += "Particular_Files_Checked_For_Link_From_Project.txt\r\n";
        message2 += "Particular_Files_LINKED_From_Project.txt\r\n";
        message2 += "Particular_Files_NOT_LINKED_From_Project.txt\r\n";
        message2 += "\r\nDo you want to open the project folder to inspect these files?";

        if (MessageBox.Show(message2, "Citavi", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        {
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", logPathsToFilesNOTLinked));
        }

    }
}
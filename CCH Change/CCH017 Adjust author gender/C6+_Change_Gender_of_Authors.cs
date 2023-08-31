using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro {
	public static void Main() {
	
		if (Program.ProjectShells.Count == 0) return;		// no project open
		if (IsBackupAvailable() == false) return;			// user wants to backup his/her project first
		
		//Get the active project
		Project activeProject = Program.ActiveProjectShell.Project;
		
		//get names and exit if none are present
		Person[] authors = activeProject.Persons.ToArray();
		if (!authors.Any()) return;
		
		// TODO: maybe there is method that directly gets the folder and/or file?
		//       If so, can the default list and this list be merged?
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		path += @"\Swiss Academic Software\Citavi 6\Settings\Firstnames.txt";
		
		if (!File.Exists(path)) {
			MessageBox.Show(
				"'" + path + "' doesn't exist. The script will be aborted.", 
				"Citavi Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
		}
		
		// TODO: check if entries are unique?
		Dictionary<string, string> namesAndSex = new Dictionary<string, string> ();
		System.IO.File.ReadAllLines(path)
				.ForEach(entry => {
					var nS = entry.Split(new[] {','});
					string name = nS[0];
					string sex = nS[1];
					namesAndSex.Add(name, sex);
				});
		
		int counter = 0;
		
		foreach(Person author in authors) {
			if (author.Sex == SwissAcademic.Sex.Unknown) {
				string firstName = author.FirstName;
				if (namesAndSex.ContainsKey(firstName)) {
					var sex = namesAndSex[firstName];
					string message = "Set gender of '" + author.FullName + "' to: ";
					
					try {
						switch (sex) {
							case "M":
								author.Sex = SwissAcademic.Sex.Male;
								message += sex;
								break;
							case "F":
								author.Sex = SwissAcademic.Sex.Female;
								message += sex;
								break;
							case "N":
								author.Sex = SwissAcademic.Sex.Neutral;
								message +=  sex;
								break;
							default:
								message = "! gender '" + sex + "' is not available in Citavi.";
								break;
						}
					} catch (Exception e) {
						DebugMacro.WriteLine("An error occurred with '" + author.FullName + "': " + e.Message);
					}
					counter++;
				}
			}
		}
		
		// Message upon completion
		string boxMessage = "";
		if (counter == 1) {
			boxMessage = "{0} gender of authors has been set.";
		} else {
        	boxMessage = "{0} genders of authors have been set.";
		}
        boxMessage = string.Format(boxMessage, counter.ToString());
        MessageBox.Show(boxMessage, "Citavi Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
	}
	
	// Ask whether backup is available
	private static bool IsBackupAvailable() {
		string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
			"\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
			"\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
			"\r\n\r\n", "Do you want to continue?"
		);
		
		return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);
	}
}

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
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
        //****************************************************************************************************************
        // Normalize Capitalization of All Upper Case Author Names
        // 1.2 -- 2018-01-22
        //
		// This macro checks all author names if they are stored in capital letters (SMITH, JOHN A.) and normalises
		// the name to mixed case (Smith, John A.).
        //
        // v1.1 - names with captial last names only are also normalized if option is set
		// v1.2 - better error handling
        
        // EDIT HERE
        // Variables to be changed by user

		bool prefixSuffixFirstCapitalLetter = false; // capitalize the first letter of name prefixes/suffixes
        bool normalizeCapitalLastname = true; // if true macro will work if only last name is in capital letters, e.g. "HUBER, David"

        // DO NOT EDIT BELOW THIS LINE
        // ****************************************************************************************************************

        
        int counter = 0;

        if (Program.ProjectShells.Count == 0) return;		//no project open
        if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first

        //reference to active Project
        Project activeProject = Program.ActiveProjectShell.Project;

        //get names and exit if none are present
        Person[] authors = activeProject.Persons.ToArray();
		if (!authors.Any()) return;
				
        // RegEx search and replace in every category name
        foreach (Person author in authors)
        {

			// get full name and check if it is all upper case
			string originalAuthorFullName = author.FullName.ToString();
            string originalAuthorLastName = author.LastName.ToString();
						
			if (originalAuthorFullName == author.FullName.ToUpper() ||
                (originalAuthorLastName == author.LastName.ToUpper() && normalizeCapitalLastname)) {
				counter++;
				
				// get name parts as strings
				string authorPrefix = author.Prefix.ToString();
				string authorFirstName = author.FirstName.ToString();
				string authorMiddleName = author.MiddleName.ToString();
				string authorLastName = author.LastName.ToString();
				string authorSuffix = author.Suffix.ToString();
					
				string authorFirstNameNormal = String.Empty;
				string authorMiddleNameNormal = String.Empty;
				string authorLastNameNormal = String.Empty;
				
				try
				{
					// normalise the strings to initial upper case
					if (!String.IsNullOrEmpty(authorFirstName)) authorFirstNameNormal = authorFirstName.ToLower().ToInitialUpper();
					if (!String.IsNullOrEmpty(authorMiddleName)) authorMiddleNameNormal = authorMiddleName.ToLower().ToInitialUpper();
					if (!String.IsNullOrEmpty(authorLastName)) authorLastNameNormal = authorLastName.ToLower().ToInitialUpper();
				}
				catch (Exception e)
				{
					DebugMacro.WriteLine("An error occurred with " + author.FullName + ": " + e.Message);	
					counter--;
					continue;
				}
					
					
				// Prefix/Suffix get initial lower case unless prefixSuffixFirstCapitalLetter is true
				string authorPrefixNormal = String.Empty;
				string authorSuffixNormal = String.Empty;
				
				if (prefixSuffixFirstCapitalLetter == true) {
					try
					{
						if (!String.IsNullOrEmpty(authorPrefix)) authorPrefixNormal = authorPrefix.ToLower().ToInitialUpper();
						if (!String.IsNullOrEmpty(authorSuffix)) authorSuffixNormal = authorSuffix.ToLower().ToInitialUpper();						
					}
					catch (Exception e)
					{
						DebugMacro.WriteLine("An error occurred with " + author.FullName + ": " + e.Message);	
						counter--;
						continue;
					}
				}
				else
				{
					authorPrefixNormal = authorPrefix.ToLower();
					authorSuffixNormal = authorSuffix.ToLower();
				}
				
				// change author
				if (!String.IsNullOrEmpty(authorPrefixNormal)) author.Prefix = authorPrefixNormal;
				if (!String.IsNullOrEmpty(authorFirstNameNormal)) author.FirstName = authorFirstNameNormal;
				if (!String.IsNullOrEmpty(authorMiddleNameNormal)) author.MiddleName = authorMiddleNameNormal;
				if (!String.IsNullOrEmpty(authorLastNameNormal)) author.LastName = authorLastNameNormal;
				if (!String.IsNullOrEmpty(authorSuffixNormal)) author.Suffix = authorSuffixNormal;				
			}            
        }
   
        // Message upon completion
        string message = "{0} names have been normalised.";
        message = string.Format(message, counter.ToString());
        MessageBox.Show(message, "Citavi Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    // Ask whether backup is available
    private static bool IsBackupAvailable()
    {
        string warning = String.Concat("Important: This macro will make irreversible changes to your project.",
            "\r\n\r\n", "Make sure you have a current backup of your project before you run this macro.",
            "\r\n", "If you aren't sure, click Cancel and then, in the main Citavi window, on the File menu, click Create backup.",
            "\r\n\r\n", "Do you want to continue?"
        );

        return (MessageBox.Show(warning, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK);
    }
    //end IsBackupAvailable()
}
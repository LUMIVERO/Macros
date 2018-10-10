using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		//****************************************************************************************************************
		// This macro checks URLs for validity and sets AccessDate to today's date or the error message of the URL check
		// Version 1.5 -- 2016-01-19
		//
		//
		// EDIT HERE
        
		int timeOut = 3000; // time in milliseconds until URL check is aborted
		string setToDate = null; // if not null, string is used for AccessDate, e.g. setToDate = "05.02.2013", otherwise today's date;
		bool setDateAlways = false; // if true, AccessDate is set regardless of outcome of URL check
		
        // DO NOT EDIT BELOW THIS LINE
		// ****************************************************************************************************************

		if (Program.ProjectShells.Count == 0) return;		//no project open
		if (IsBackupAvailable() == false) return;			//user wants to backup his/her project first
			
        string dateTimeFormat = Program.Engine.Settings.General.DateTimeFormat;
		string newAccessDate = setToDate;
        if (setToDate == null) newAccessDate = DateTime.Today.ToString(dateTimeFormat);

		//iterate over all references in the current filter (or over all, if there is no filter)
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
        List<Reference> referencesWithUrl = references.Where(reference => !String.IsNullOrEmpty(reference.OnlineAddress)).ToList();
        List<Reference> referencesWithInvalidUrls = new List<Reference>();
        
		//reference to active Project
		Project activeProject = Program.ActiveProjectShell.Project;

        int loopCounter = 0;
		int changeCounter = 0;
		int invalidCounter = 0;
		string urlResult = null;
        
        //Welcome message
        DebugMacro.WriteLine(String.Format("Checking links for {0} reference(s) with URLs out of {1} reference(s) in total.", referencesWithUrl.Count.ToString(), references.Count.ToString()));        
        
		foreach (Reference reference in referencesWithUrl)
		{		
                                    
            loopCounter++;
            DebugMacro.WriteLine(String.Format("Checking {0} of {1}: {2}", loopCounter.ToString(), referencesWithUrl.Count.ToString(), reference.ShortTitle));
            
            // get URL to check
			string url = reference.OnlineAddress;
			DebugMacro.WriteLine(string.Format("Processing URL '{0}'", url));

			string oldAccessDate = reference.AccessDate; // get previous last access date
			
			// Check URL and set fields
			
            bool urlExists = RemoteFileExists(url, timeOut, out urlResult);
			
			if (urlExists)
			{
				DebugMacro.WriteLine("URL is valid.");
				reference.Notes += String.Format(" *** Link {0} checked on {1}: {2}; original access date: {3} ***", reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                reference.AccessDate = newAccessDate;
				changeCounter++;
			}
		    else        
			{
				DebugMacro.WriteLine("URL is NOT valid: " + urlResult);
				reference.Notes += String.Format(" *** Link {0} checked on {1}: {2}; original access date: {3} ***", reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
				if (setDateAlways) reference.AccessDate = newAccessDate;
				invalidCounter++;
                referencesWithInvalidUrls.Add(reference);
			}
		}
		
        string  message = "{0} links checked:\n{1} URLs valid\n{2} URLs not reachable\n Would you like to show a selection with references having an unreachable Online Address?";
		message = string.Format(message, referencesWithUrl.Count.ToString(), changeCounter.ToString(), invalidCounter.ToString());
		DialogResult showSelection = MessageBox.Show(message, "Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (showSelection == DialogResult.Yes && referencesWithInvalidUrls.Count > 0)
        {
            ReferenceFilter filter = new ReferenceFilter(referencesWithInvalidUrls, "References with invalid URLs", false);
			List<ReferenceFilter> referenceFilters = new List<ReferenceFilter>();
			referenceFilters.Add(filter);
			Program.ActiveProjectShell.PrimaryMainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(referenceFilters);
        }
        else return;
        
	}

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

	
	// Link Checker, returns true or false and the error message
	private static bool RemoteFileExists(string url, int timeOut, out string urlResult)
	{
		try
		{
			//Creating the HttpWebRequest
	        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			//Setting the Request method HEAD, you can also use GET too.
	        request.Method = "HEAD";
			request.Timeout = timeOut; 
			//Getting the Web Response.
	        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			
			urlResult = null;
                                                 
            if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 400)
            {
                if (response.Headers["Location"] != null)
                {
                    urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription + " - Redirect: " + response.Headers["Location"].ToStringSafe();
                }
                else
                {
                    urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription;
                }
            }    
            else
            {
                urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription;
            }
            
                     
            //Returns TRUE if the Status code == 200			
            return response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception e)
		{
			//Any exception will returns false.
			urlResult = e.Message.ToString();
			return false;
		}
	}
}
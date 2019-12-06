using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Citavi.DataExchange;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
    public static async void Main()
    {
        //****************************************************************************************************************
        // UPDATE BIBLIOGRAPHIC DATA FROM PMID OR DOI-SEARCH
        // Version 2.4 -- 2019-12-06
        //			-- updated to work with Citavi 6 (only version 6.3.15 or newer!)
        //
        // This macro iterates through the references in a selection ("filter").
        // If they have a PMID or DOI or ISBN, it downloads bibliographical data and owverwrites the reference's data.
        // 
        // PMID is given priority over DOI , i.e. if both are present, data will be loaded from PubMed, then
        // Crossref. then the selected catalogues.
        //
        //
        // EDIT HERE
        // Variables to be changed by user

        bool overwriteAbstract = false;             // if true, existing Abstract will be replaced
        bool overwriteTableOfContents = false;      // if true, existing TOC will be replaced
        bool overwriteKeywords = false;             // if true, existing Keywords will be replaced
        bool clearNotes = true;                     // if true, Notes field will be emptied

        bool useIsbn = true;                        // if true, the ISBN field will also be used to query data

        int wait = 4;                              // timeout in seconds

        // DO NOT EDIT BELOW THIS LINE
        // ****************************************************************************************************************

        if (!Program.ProjectShells.Any()) return;       //no project open	
        if (IsBackupAvailable() == false) return;       //user wants to backup his/her project first

        int counter = 0;
		
	    try
        {

            List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
            SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;

            foreach (Reference reference in references)
            {                
				if (String.IsNullOrEmpty(reference.PubMedId) &&
                    String.IsNullOrEmpty(reference.Doi) && 
                    String.IsNullOrEmpty(reference.Isbn.ToString())) continue;

				DebugMacro.WriteLine("Checking reference " + reference.ShortTitle);
                                
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(wait));
				
				Reference lookedUpReference = null;
               
                if (!String.IsNullOrEmpty(reference.PubMedId))
                {
                    try
                    {
                    ReferenceIdentifier refIdentPmid = new ReferenceIdentifier() { Value = reference.PubMedId, Type = ReferenceIdentifierType.PubMedId };
                    var identifierSupport = new ReferenceIdentifierSupport();
                    lookedUpReference = await identifierSupport.FindReferenceAsync(activeProject, refIdentPmid, cts.Token);  
                    }
                    catch (System.OperationCanceledException e)
                    {
						DebugMacro.WriteLine("Timeout.");
						continue;
                    }
                    catch (Exception e)
					{
                        DebugMacro.WriteLine("An error occured: " + e.ToString());
                        continue;
                    }
                }
				else if (!String.IsNullOrEmpty(reference.Doi))
                {
                    try
                    {
                    ReferenceIdentifier refIdentDoi = new ReferenceIdentifier() { Value = reference.Doi, Type = ReferenceIdentifierType.Doi };
                    var identifierSupport = new ReferenceIdentifierSupport();
                    lookedUpReference = await identifierSupport.FindReferenceAsync(activeProject, refIdentDoi, cts.Token);  
                    }
                    catch (System.OperationCanceledException e)
                    {
						DebugMacro.WriteLine("Timeout.");
						continue;
                    }
                    catch (Exception e)
					{
                        DebugMacro.WriteLine("An error occured: " + e.ToString());
                        continue;
                    }                      
                }
                else if (!String.IsNullOrEmpty(reference.Isbn.ToString()) && useIsbn)
                {
                    try
                    {
                    ReferenceIdentifier refIdentIsbn = new ReferenceIdentifier() { Value = reference.Isbn, Type = ReferenceIdentifierType.Isbn };
                    var identifierSupport = new ReferenceIdentifierSupport();
                    lookedUpReference = await identifierSupport.FindReferenceAsync(activeProject, refIdentIsbn, cts.Token);    
		            }
                    catch (System.OperationCanceledException e)
                    {
						DebugMacro.WriteLine("Timeout.");
						continue;
                    }
                    catch (Exception e)
					{
                        DebugMacro.WriteLine("An error occured: " + e.ToString());
                        continue;
                    }
				}
	
                // if nothing is found, move on
				if (lookedUpReference == null) continue;


                //merge reference & lookedUpReference, overwriting bibliographic data of the former
                List<ReferencePropertyId> omitData = new List<ReferencePropertyId>();
                omitData.Add(ReferencePropertyId.CoverPath);
                omitData.Add(ReferencePropertyId.Locations);
                

                if (!overwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
                if (!overwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
                if (!overwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);

                reference.MergeReference(lookedUpReference, true, omitData.ToArray());

                counter++;

                if (!string.IsNullOrEmpty(reference.Notes) && clearNotes) reference.Notes = string.Empty;   //empty notes field
                if (activeProject.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = activeProject.BibTeXKeyAssistant.GenerateKey(reference);
                if (activeProject.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = activeProject.CitationKeyAssistant.GenerateKey(reference);
            }

        } //end try

        finally
        {
            MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} references were updated.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } //end finally

    } //end main()


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

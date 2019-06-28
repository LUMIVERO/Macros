using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Controls.WordProcessor;

// IMPORTANT: This macro might lead to a crash of Citavi AFTER it has finished execution
// (affects Citavi versions up and including 3.0.18 and will be fixed for later versions).
// All changes will nevertheless be saved by this macro.
// If Citavi crashes, use the task manager (CTRL+ALT+DELETE) to end a hanging Citavi.exe process.
// Otherwise you will not be able to restart Citavi.

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
    public static void Main()
    {
        if (Program.ProjectShells.Count == 0) return;       //no project open	
        if (IsBackupAvailable() == false) return;           //user wants to backup his/her project first

        int counter = 0;

        try
        {
            CommonParagraphAttributes paragraphFormat = new CommonParagraphAttributes();
            //*******************************************************************************************************************//
            //NOTE: YOU CAN ADAPT THE FOLLOWING ACCORDING TO YOUR NEEDS:


            //------------- SCOPE ----------------------------------------------------------------------------------------------//
            // Would you like to include Abstract, Table of Content and Evaluation fields?
            bool includeReferencePropertyFields = true; // set to false, if only "real" Knowledge Items should be affected			

            //------------------ FONT -------------------------------------------------------------------------------------------//
            string fontName = "Segoe UI";               //NOTE: Name der Schriftart
            int fontSize = 9;                                   //NOTE: Groesse in Punkt
                                                                //-------------------------------------------------------------------------------------------------------------------//

            //------------------ PARAGRAPH ALIGNMENT ----------------------------------------------------------------------------//
            paragraphFormat.Alignment = Alignment.Left;         //NOTE: Linksbuending		

            //NOTE: OR:
            //paragraphFormat.Alignment = Alignment.Justify;		//NOTE: Blocksatz

            //NOTE: OR:
            //paragraphFormat.Alignment = Alignment.Center;		//NOTE: Zentriert

            //NOTE: OR:
            //paragraphFormat.Alignment = Alignment.Right;		//NOTE: Rechtsbuendig
            //-------------------------------------------------------------------------------------------------------------------//


            //------------------ LINE SPACING WITHIN PARAGRAPH ------------------------------------------------------------------//
            paragraphFormat.Spacing.LineSpacingType = LineSpacingType.One;  //NOTE: Einzeilig

            //NOTE: OR: 
            //paragraphFormat.Spacing.LineSpacingType = LineSpacingType.OneAndHalf;		//NOTE: Anderthalbzeilig

            //NOTE: OR:
            //paragraphFormat.Spacing.LineSpacingType = LineSpacingType.Double;		//NOTE: Zweizeilig

            //NOTE: OR:
            //paragraphFormat.Spacing.LineSpacingType = LineSpacingType.Multiple;	//NOTE: Mehrfach
            //float n = 3F; 		//NOTE: for threefold, always with F at the end !
            //paragraphFormat.Spacing.Line = Convert.ToInt32((n * 100F) - 100F);

            //NOTE: OR:
            //paragraphFormat.Spacing.LineSpacingType = LineSpacingType.Precise;	//NOTE: Exakt (in Punkt)
            //float pt = 12.5F; 	//NOTE: in points, always with F at the end, do NOT change following line
            //paragraphFormat.Spacing.Line = Convert.ToInt32(MeasurementUnit.Points.ConvertValue(pt, MeasurementUnitType.Twips));

            //NOTE: OR:
            //paragraphFormat.Spacing.LineSpacingType = LineSpacingType.Minimum;	//NOTE: Mindestens (in Punkt)
            //float pt = 12.5F; 	//NOTE: in points, always with F at the end, do NOT change the following line
            //paragraphFormat.Spacing.Line = Convert.ToInt32(MeasurementUnit.Points.ConvertValue(pt, MeasurementUnitType.Twips));
            //-------------------------------------------------------------------------------------------------------------------//			


            //------------------ SPACING AFTER/BEFORE PARAGRAPH -----------------------------------------------------------------//			
            float ptAfter = 6F; //NOTE: in points, always with F at the end, do NOT change the following line
            paragraphFormat.Spacing.After = Convert.ToInt32(MeasurementUnit.Points.ConvertValue(ptAfter, MeasurementUnitType.Twips));

            //NOTE: AND/OR:
            //float ptBefore = 12F; //NOTE: in points, always with F at the end, do NOT change the following line
            //paragraphFormat.Spacing.Before = Convert.ToInt32(MeasurementUnit.Points.ConvertValue(ptBefore, MeasurementUnitType.Twips));
            //-------------------------------------------------------------------------------------------------------------------//


            //------------------ INDENTATION OF PARAGRAPH -----------------------------------------------------------------------//
            //NOTE: RESET - DO NOT REMOVE THE FOLLOWING LINES - THEY MUST ALWAYS BE APPLIED OTHERWISE YOU END UP WITH A WILD MIX
            paragraphFormat.Indentation.Left = 0;
            paragraphFormat.Indentation.Right = 0;
            paragraphFormat.Indentation.FirstLine = 0;

            //NOTE: LEFT
            //float cmLeft = 0F; 			//NOTE: in cm, always with F at the end, do NOT edit the following lines, just remove comments
            //paragraphFormat.Indentation.Left = Convert.ToInt32(MeasurementUnit.Centimeters.ConvertValue(cmLeft, MeasurementUnitType.Twips));

            //NOTE: RIGHT
            //float cmRight = 0F; 		//NOTE: in cm, always with F at the end, do NOT edit the following lines, just remove comments
            //paragraphFormat.Indentation.Right = Convert.ToInt32(MeasurementUnit.Centimeters.ConvertValue(cmRight, MeasurementUnitType.Twips));

            //NOTE: FIRSTLINE
            //float cmFirstLine = 1F; 	//NOTE: in cm, always with F at the end, do NOT edit the following lines, just remove comments
            //paragraphFormat.Indentation.IndentationType = IndentationType.FirstLine;
            //paragraphFormat.Indentation.FirstLine = Convert.ToInt32(MeasurementUnit.Centimeters.ConvertValue(cmFirstLine, MeasurementUnitType.Twips));

            //NOTE: OR: HANGING
            //float cmHanging = 1F; 	//NOTE: in cm, always with F at the end, do NOT edit the following lines, just remove comments
            //paragraphFormat.Indentation.IndentationType = IndentationType.Hanging;
            //paragraphFormat.Indentation.FirstLine = Convert.ToInt32(MeasurementUnit.Centimeters.ConvertValue(cmHanging, MeasurementUnitType.Twips));			
            //-------------------------------------------------------------------------------------------------------------------//

            //*******************************************************************************************************************//
            //NOTE: DO NOT CHANGE ANYTHING BELOW THIS LINE


            //KnowledgeItem[] knowledgeItems = Program.ActiveProjectShell.Project.AllKnowledgeItems.ToArray();


            //reference to active project shell
            SwissAcademic.Citavi.Shell.ProjectShell activeShell = Program.ActiveProjectShell;

            //reference to active project
            SwissAcademic.Citavi.Project activeProject = activeShell.Project;

            //reference to primary main form
            SwissAcademic.Citavi.Shell.MainForm mainForm = activeShell.PrimaryMainForm;

            var references = mainForm.GetFilteredReferences();
            List<KnowledgeItem> knowledgeItemList = new List<KnowledgeItem>();
            foreach (Reference reference in references)
            {
                knowledgeItemList.AddRange(reference.Quotations);
                if (includeReferencePropertyFields)
                {
                    knowledgeItemList.Add(reference.Abstract);
                    knowledgeItemList.Add(reference.TableOfContents);
                    knowledgeItemList.Add(reference.Evaluation);
                }

            }
            KnowledgeItem[] knowledgeItems = knowledgeItemList.ToArray();

            object tag = null;
            foreach (KnowledgeItem knowledgeItem in knowledgeItems)
            {
                if (string.IsNullOrEmpty(knowledgeItem.Text)) continue;
                if (knowledgeItem.KnowledgeItemType != KnowledgeItemType.Text &&
                    knowledgeItem.KnowledgeItemType != KnowledgeItemType.ReferenceProperty) continue;

                counter++;

                //first we instantiate a new RTF Form ...
                //SwissAcademic.Citavi.Shell.RtfForm rtfForm = activeShell.ShowAbstractForm(reference);


                SwissAcademic.Citavi.Shell.ProjectShellForm rtfForm = null;

                switch (knowledgeItem.MirrorsReferencePropertyId)
                {
                    case ReferencePropertyId.Abstract:
                        rtfForm = activeShell.ShowAbstractForm(knowledgeItem.Reference);
                        break;
                    case ReferencePropertyId.TableOfContents:
                        rtfForm = activeShell.ShowTableOfContentsForm(knowledgeItem.Reference);
                        break;
                    case ReferencePropertyId.Evaluation:
                        rtfForm = activeShell.ShowEvaluationForm(knowledgeItem.Reference);
                        break;
                    default:
                        rtfForm = activeShell.ShowKnowledgeItemFormForExistingItem(mainForm, knowledgeItem);
                        break;
                }



                //rtfForm.PerformCommand("FormatRemoveReturnsAndTabs", tag);


                //... then via reflection we get a reference to the WordProcessorControl therein ...
                Type t = rtfForm.GetType();

                FieldInfo fieldInfo = t.GetField("wordProcessor", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo == null) continue;

                WordProcessorControlEx wordProcessorControl = (WordProcessorControlEx)fieldInfo.GetValue(rtfForm);
                if (wordProcessorControl == null) continue;

                WordProcessorControl wordProcessor = wordProcessorControl.Editor;
                if (wordProcessor == null) continue;

                //... and do the formatting:
                wordProcessor.SelectAll();
                if (!string.IsNullOrEmpty(fontName)) wordProcessor.Selection.FontName = fontName;
                if (fontSize != 0) wordProcessor.Selection.FontSize = Convert.ToInt32(SwissAcademic.MeasurementUnit.Points.ConvertValue(fontSize, SwissAcademic.MeasurementUnitType.Twips));
                wordProcessor.Selection.SetCommonParagraphAttributes(paragraphFormat);

                rtfForm.PerformCommand("Save", tag);
                rtfForm.Close();
            }
        } //end try

        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString());

        }

        finally
        {
            MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} changes were made.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } //end finally

    } //end main()




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
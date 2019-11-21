using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	const double mb = 1; // Size to search for (mb) - adjust if necessary
	const int bytes = mb * 1024 * 1024;
	
	public static void Main()
	{
		ProjectReferenceCollection references = Program.ActiveProjectShell.Project.References;
		ProjectAllKnowledgeItemsCollection knowledgeItems = Program.ActiveProjectShell.Project.AllKnowledgeItems;
		
		foreach(Reference reference in references)
		{
			CheckText("Titel", reference.Abstract.TextRtf, reference.ShortTitle, reference.Id, "Abstract"); 
			CheckText("Titel", reference.TableOfContents.TextRtf, reference.ShortTitle, reference.Id, "TableOfContents"); 
			CheckText("Titel", reference.Evaluation.TextRtf, reference.ShortTitle, reference.Id, "Evaluation"); 
		}
		foreach(KnowledgeItem knowledgeItem in knowledgeItems)
		{
			CheckText("Wissenselement", knowledgeItem.TextRtf, knowledgeItem.CoreStatement, knowledgeItem.Id, "Text");
		}
	}
	static void CheckText(string entity, string text, string name, Guid id, string property)
	{
		if(text.Length > bytes)
		{
			string output = string.Format("{0}: {1}({2}) - {3} > {4}MB. Größe: {5}MB", entity, name, id, property, mb, text.Length / 1024 / 1024); 
			
			DebugMacro.WriteLine(output);
		}
	}
	
}

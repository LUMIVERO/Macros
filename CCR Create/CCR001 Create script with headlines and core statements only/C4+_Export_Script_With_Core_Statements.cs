using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

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
		if (Program.ActiveProjectShell == null) return;
			
		var activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		
		//consider all KnowledgeItems inside the category system
		foreach (Category category in activeProject.Categories)
		{
			AddCategory(category, stringBuilder);
		}
		
		
		//consider KnowledgeItems WITHOUT category
		if (activeProject.KnowledgeItemsWithNoCategory != null && activeProject.KnowledgeItemsWithNoCategory.Count > 0)
		{
			stringBuilder.Append("Ohne Kategorie");
			stringBuilder.Append("\r\n");
			
			foreach (KnowledgeItem knowledgeItem in activeProject.KnowledgeItemsWithNoCategory)
			{
				stringBuilder.Append(knowledgeItem.ToString());
				stringBuilder.Append("\r\n");
			}		
		}

		
		var result = stringBuilder.ToString();
		if (!string.IsNullOrEmpty(result))
		{
			Clipboard.SetText(stringBuilder.ToString());
			MessageBox.Show("Die Kernaussagen aller Wissenselemente wurden mitsamt Gliederungssystem in die Zwischenablage kopiert.");
		}
		else
		{
			MessageBox.Show("Das Projekt enthält keine Wissenselemente.");
		}
		
	}
	
	static void AddCategory(Category category, System.Text.StringBuilder stringBuilder)
	{
		stringBuilder.Append(category.FullName);
		stringBuilder.Append("\r\n");
	
		foreach (KnowledgeItem knowledgeItem in category.KnowledgeItems)
		{
			stringBuilder.Append(knowledgeItem.ToString());
			stringBuilder.Append("\r\n");
		}
		
		if (category.HasChildren)
		{
			foreach (Category childCategory in category.Categories)
			{
				AddCategory(childCategory, stringBuilder);
			}
		}	
	
	}
}
//CIM006
//Import manual sorting of knowledge elements
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;


public static class CitaviMacro
{
	public static void Main()
	{
		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project targetProject = Program.ActiveProjectShell.Project;
		if (targetProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
		
		string sourceCTV6File = string.Empty;
		string initialDirectory = Program.Engine.DesktopEngineConfiguration.GetFolderPath(CitaviFolder.Projects);
		
		DebugMacro.WriteLine(initialDirectory);
		
		//System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop
		var openFileDialog = new OpenFileDialog() { 
			Filter = "Citavi Project Files|*.ctv6", 
			CheckFileExists = true, 
			CheckPathExists = true, 
			Multiselect = false, 
			InitialDirectory = initialDirectory 
		};

		if (openFileDialog.ShowDialog(primaryMainForm) != DialogResult.OK) return;
		
		string sourceProjectFile = openFileDialog.FileName;
		DebugMacro.WriteLine(sourceProjectFile);
		
		Project sourceProject = GetProject(sourceProjectFile).Result;
		if (sourceProject == null) return;	
		
		
		IEnumerable<Category> targetCategories = targetProject.AllCategories.AsEnumerable<Category>();
		IEnumerable<Category> sourceCategories = sourceProject.AllCategories.AsEnumerable<Category>();
		
		foreach(Category targetCategory in targetCategories)
		{
			Category sourceCategory = sourceCategories.FindCategory(targetCategory);
			if (sourceCategory == null) continue;

			IEnumerable<KnowledgeItem> sourceCategoryKnowledgeItems = sourceCategory.KnowledgeItems.AsEnumerable<KnowledgeItem>();
			
			foreach(KnowledgeItem sourceKnowledgeItem in sourceCategoryKnowledgeItems)
			{
				//if exists in target KI collection, then move to bottom
				KnowledgeItem targetKnowledgeItem = targetCategory.KnowledgeItems.FindKnowledgeItem(sourceKnowledgeItem);
				if (targetKnowledgeItem == null) continue;
				
				targetCategory.KnowledgeItems.MoveToBottom(targetKnowledgeItem);
			}
		}
		
		
		MessageBox.Show("Done");
		
		
	}
	
	static async Task<Project> GetProject(string path)
	{
		var configuration = await DesktopProjectConfiguration.OpenAsync(Program.Engine, path, CancellationToken.None);
		var project =  await Program.Engine.Projects.OpenAsync(configuration);
		return project;
	}
	
	static KnowledgeItem FindKnowledgeItem(this IEnumerable<KnowledgeItem> knowledgeItemCollection, KnowledgeItem knowledgeItem)
	{
		if (knowledgeItem == null) return null;
		if (knowledgeItem.StaticIds == null || !knowledgeItem.StaticIds.Any()) return null;
		if (knowledgeItemCollection == null || !knowledgeItemCollection.Any()) return null;
		
		foreach(KnowledgeItem item in knowledgeItemCollection)
		{
			if (item.StaticIds == null || !item.StaticIds.Any()) continue;
			if (item.StaticIds.Intersect(knowledgeItem.StaticIds).Any()) return item;
		}
		
		//still here, try full path
		return null;
	}
	
	static Category FindCategory(this IEnumerable<Category> categoryCollection, Category category)
	{
		if (category == null) return null;
		if (category.StaticIds == null || !category.StaticIds.Any()) return null;
		if (categoryCollection == null || !categoryCollection.Any()) return null;

		
		foreach(Category item in categoryCollection)
		{
			if (item.StaticIds == null || !item.StaticIds.Any()) continue;
			if (item.StaticIds.Intersect(category.StaticIds).Any()) return item;
		}
		
		
		string categoryFullPath = category.GetFullPath();
		//still here, try full path
		foreach(Category item in categoryCollection)
		{
			string itemFullPath = item.GetFullPath();
			if (itemFullPath.Equals(categoryFullPath)) return item;
		}
		
		return null;
	}
	
	static string GetFullPath(this Category category)
	{
		if (category == null || string.IsNullOrEmpty(category.Name)) return string.Empty;
		string categoryFullPath = category.Name;
		
		Category parentCategory = category.ParentCategory;
		while(parentCategory != null)
		{
			categoryFullPath = parentCategory.Name + " > " + categoryFullPath;
			parentCategory = parentCategory.ParentCategory;
		}
		
		return categoryFullPath;
	}
	
	static void MoveToBottom(this CategoryKnowledgeItemCollection knowledgeItemCollection, KnowledgeItem knowledgeItem)
	{
		if (knowledgeItemCollection == null || knowledgeItemCollection.Count <= 1) return;
		
		KnowledgeItem lastKnowledgeItem = knowledgeItemCollection.Last<KnowledgeItem>();
		knowledgeItemCollection.Move(knowledgeItem, lastKnowledgeItem);
	}
}

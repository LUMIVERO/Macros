using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

public static class CitaviMacro
{
	public static void Main()
	{
		//if we need a ref to the active project
		SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
		
		Category[] cs = activeProject.AllCategories.ToArray();
		Array.Sort(cs, new CategoryWithReferencesComparer());
	
		foreach(Category c in cs)
		{
			DebugMacro.WriteLine(c.FullName + "\t" + c.References.Count);
		}
	}
	
	class CategoryWithReferencesComparer : IComparer<Category>
	{
	    public int Compare(Category x, Category y)
	    {
	        return x.References.Count.CompareTo(y.References.Count) * (-1);
	    }
	}
}
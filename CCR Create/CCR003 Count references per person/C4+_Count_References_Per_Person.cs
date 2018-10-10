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
		if (Program.ActiveProjectShell == null) return;
		
		SwissAcademic.Citavi.Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Person[] ps = activeProject.Persons.ToArray();
		//Array.Sort(ps, new PersonComparerAlphabetic());
		Array.Sort(ps, new PersonComparerReferencesCount());
		
		foreach(Person p in ps)
		{
			DebugMacro.WriteLine(p.FullName + "\t" + p.References.Count);
		}
	}
	
	class PersonComparerAlphabetic : IComparer<Person>
	{
	    public int Compare(Person x, Person y)
	    {
			return x.FullName.CompareTo(y.FullName);
	    }
	}
	
	class PersonComparerReferencesCount : IComparer<Person>
	{
		public int Compare(Person x, Person y)
		{
			var countCompareResult = x.References.Count.CompareTo(y.References.Count) * (-1);
			if (countCompareResult != 0) return countCompareResult;
			
			return x.FullName.CompareTo(y.FullName);
		}
	}
}
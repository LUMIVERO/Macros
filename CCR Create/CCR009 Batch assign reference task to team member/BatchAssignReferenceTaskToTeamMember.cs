//CCR 009 Batch Assign Reference Task To Team Member
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
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null)
		{
			DebugMacro.WriteLine("No active project.");
			return;
		}
		List<Reference> references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
		if (references == null || !references.Any())
		{
			DebugMacro.WriteLine("No references selected.");
			return;
		}
		
		ContactInfo taskAssignee = activeProject.Contacts.GetMembers().FirstOrDefault<ContactInfo>(item => item.FullName.Equals("Jane Doe"));
		if (taskAssignee == null) 
		{
			DebugMacro.WriteLine("No such contact.");
			return;
		}			
		DateTime? dueDate = new DateTime(2019, 12, 31);
		KnownTaskName taskName = KnownTaskName.ExcerptQuotations;
		//string taskName = "Do something else with these references.";	
		
		foreach (Reference reference in references)
		{
			//reference.Tasks.Clear();
		    TaskItem task = reference.Tasks.Add(taskName);
		    task.AssignedTo = taskAssignee.Key;
			task.DueDate = dueDate;
		}
	
	
		DebugMacro.WriteLine("Done.");
	}
}

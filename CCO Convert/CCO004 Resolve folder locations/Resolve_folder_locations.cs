// CCO004
// Resolve folder locations

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


public static class CitaviMacro
{
	public static void Main()
	{
		bool deleteLocationAfterResolve = true; // True to delete the folder location after it has been resolved
		AttachmentAction attachmentAction = AttachmentAction.Copy; // Set the attachment action for the files in the directory. Note that for cloud and sql server project this will always be resetted to Copy.
		
		int counter = 0;
		
		try	
		{
			foreach(Location location in Program.ActiveProjectShell.Project.AllLocations.ToList())
			{
				if(location.IsExistingFolderLocation)
				{
					counter++;
					
					location.ResolveFolderLocation(attachmentAction);
					
					if(deleteLocationAfterResolve)
					{
						location.Reference.Locations.Remove(location);
					}
				}
			}
			MessageBox.Show("Resolved " + counter.ToString() + " locations");
		}
		catch(Exception x)
		{
			MessageBox.Show(x.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	
	}
}

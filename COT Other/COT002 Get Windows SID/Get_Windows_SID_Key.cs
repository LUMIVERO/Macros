using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Principal;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Citavi.Persistence;


public static class CitaviMacro
{
	public static void Main()
	{
			string key = WindowsIdentity.GetCurrent().User.Value;
			if (string.IsNullOrEmpty(key)) 
			{
				MessageBox.Show("Kein Schlüssel gefunden.");
				return;
			}
		
			
			Clipboard.SetText(key);
			MessageBox.Show(string.Format("Der Schlüssel \n\r{0} \n\rwurde in die Zwischenablage kopiert.", key));
			
	}
}
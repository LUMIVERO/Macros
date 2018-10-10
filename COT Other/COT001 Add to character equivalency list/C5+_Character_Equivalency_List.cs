using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Search;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		int counter = 0;
		
		try 
		{	
			EquivalentCharactersPairCollection definedPairs = QueryEngine.Settings.EquivalentCharacterPairs;
			NameValueCollection pairs = new NameValueCollection();
			
			//Pls. edit the following equivalent pairs by first stating a simple latin character 
			//between double quotes "1" - this is called the "destination character".
			//Then state a whole string of various 'accentuated' characters derived from the former "234567etc."
			//- these are called the "source characters".
			//Searching for the destination character will also find the source character(s).			
			pairs.Add("a", "àáâãäåǎæ");
			pairs.Add("e", "èéêëĕėęěē");
			pairs.Add("i", "ìíîïĩīĭįıǐ");
			pairs.Add("o", "őŏōöõôóòœǫǭȍȏǒ");
			pairs.Add("u", "ùúûüũūŭůűųǔǚ");
			pairs.Add("ö", "œőȍ");
			pairs.Add("ä", "æǟ");
			pairs.Add("ü", "ǖǘǚǜȕ");
			pairs.Add("c", "ćĉċčƈç");
			pairs.Add("d", "ďđ");
			pairs.Add("g", "ĝğġģǥǧ");
			pairs.Add("h", "ĥħȟ");
			pairs.Add("j", "ǰ");
			pairs.Add("k", "ǩķ");
			pairs.Add("l", "ľŀłĺļ");
			pairs.Add("n", "ňńņŉŋ");
			pairs.Add("n", "ňńņŉŋ");
			pairs.Add("r", "řŕŗ");
			pairs.Add("s", "šśŝş");
			pairs.Add("t", "ţťŧƭ");
			pairs.Add("z", "žżźƶ");
			pairs.Add("dz", "ǆ");

			
			//Reverse equivalency? I.e.: 'c also finds č' and 'č also finds c' 
			bool reverseEquivalency = true;
			
			//remove all existing pairs before adding the above? (Note that duplicates are suppressed anyway)
			bool clearAllBeforeAdding = false;
			
			
			
			//DO NOT EDIT BELOW HERE
			if (clearAllBeforeAdding) RemoveAllEquivalentCharacterPairs();
			for (int i = 0; i < pairs.Count; i++)
			{
				foreach (char c in pairs.Get(i))
				{
					AddEquivalentCharactersPair(c.ToString(), pairs.GetKey(i), ref counter);
					if (reverseEquivalency) AddEquivalentCharactersPair(pairs.GetKey(i), c.ToString(), ref counter);
				}
			}
			
			
			
		} //end try
		
		catch(Exception exception)
		{
			MessageBox.Show(exception.ToString());
		}
		
		finally 
		{
			MessageBox.Show(string.Format("Macro has finished execution.\r\n{0} character pairs were added to the Citavi Query Engine.", counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} //end finally
		
	}
	
	public static void AddEquivalentCharactersPair(string source, string destination, ref int counter)
	{
		if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination)) return;
		
		EquivalentCharactersPair newPair = new EquivalentCharactersPair(source, destination);
		EquivalentCharactersPair existingPair = null;
		
		if (QueryEngine.Settings.EquivalentCharacterPairs.TryGetEquivalentCharactersPair(source, destination, out existingPair))
		{
			if (existingPair != null && newPair.Equals(existingPair)) return;
		}
		
		//MessageBox.Show(string.Format("New pair added: {0}", newPair.ToString()));
		QueryEngine.Settings.EquivalentCharacterPairs.Add(newPair);
		counter++;
		
	}
	
	
	public static void RemoveAllEquivalentCharacterPairs()
	{
		List<EquivalentCharactersPair> removePairs = new List<EquivalentCharactersPair>();
		
		//First collect all character pairs and then ...
		foreach(EquivalentCharactersPair pair in QueryEngine.Settings.EquivalentCharacterPairs)
		{
			removePairs.Add(pair);
		}
		
		//... actually delete them from the QueryEngine
		foreach(EquivalentCharactersPair pair in removePairs)
		{
			QueryEngine.Settings.EquivalentCharacterPairs.Remove(pair);
		}
	
	}
	
}
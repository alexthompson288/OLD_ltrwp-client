// <author>Alexander Murauer</author>
// <email>alexander.murauer@kolmich.at</email>
// <date>2011-11-08</date>
// <summary>short summary</summary>

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KGFDebug))]
public class KGFDebugEditor : KGFEditor 
{
	public static KGFMessageList ValidateKGFDebugEditor(UnityEngine.Object theObject)
	{
		return new KGFMessageList();
	}
}
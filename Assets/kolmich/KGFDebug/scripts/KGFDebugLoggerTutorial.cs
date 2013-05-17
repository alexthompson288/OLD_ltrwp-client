// <author>Christoph Hausjell</author>
// <email>christoph.hausjell@kolmich.at</email>
// <date>2012-03-28</date>
// <summary></summary>

using System;
using UnityEngine;
using System.Collections;

public class KGFDebugLoggerTutorial : MonoBehaviour, KGFIDebug
{
	// minimum Log level of the Debugger
	private KGFeDebugLevel itsMinimumLogLevel = KGFeDebugLevel.eAll;
	
	#region Unity3D

	public void Awake()
	{
		KGFDebug.AddLogger(this);
	}
	
	#endregion
	
	public string GetName()
	{
		return "KGFTutorialLogger";
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void Log(KGFDebug.KGFDebugLog aLog)
	{
		Log(aLog.GetLevel(), aLog.GetCategory(), aLog.GetMessage(), aLog.GetStackTrace(), aLog.GetObject() as MonoBehaviour);
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage)
	{
		Log(theLevel, theCategory, theMessage, string.Empty, null);
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace)
	{
		Log(theLevel, theCategory, theMessage, theStackTrace, null);
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, MonoBehaviour theObject)
	{
		if(theObject != null)
		{
			Debug.Log(string.Format("{0} {1} {5} {2}{5}{3}{5}{4}", theLevel, theCategory, theMessage, theObject.name, theStackTrace, Environment.NewLine));
		}
		else
		{
			Debug.Log(string.Format("{0} {1} {4}{2}{4}{3}", theLevel, theCategory, theMessage, theStackTrace, Environment.NewLine));
		}
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void SetMinimumLogLevel(KGFeDebugLevel theLevel)
	{
		itsMinimumLogLevel = theLevel;
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public KGFeDebugLevel GetMinimumLogLevel()
	{
		return itsMinimumLogLevel;
	}
}
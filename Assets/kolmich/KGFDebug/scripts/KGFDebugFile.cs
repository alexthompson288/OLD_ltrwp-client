// <author>Christoph Hausjell</author>
// <email>christoph.hausjell@kolmich.at</email>
// <date>2012-03-13</date>
// <summary>short summary</summary>

using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// contains all data available for customization in the Unity3D inspector
/// </summary>
[System.Serializable]
public class KGFDataDebugFile
{
	// the minimum log level
	public KGFeDebugLevel itsMinimumLogLevel = KGFeDebugLevel.eError;
	
	// the seperator character
	public string itsSeparator = ";";
	
	// the file path of the logfile
	public string itsFilePath = "";
}

/// <summary>
/// the main class implements the KGFIDebug interface. use this implementaion to write logs in a file.
/// </summary>
public class KGFDebugFile : KGFModule, KGFIDebug
{
	// contains all settings of the file logger
	public KGFDataDebugFile itsDataDebugFile = new KGFDataDebugFile();
	
	public KGFDebugFile() : base(new Version(1,0,0,1), new Version(1,1,0,0))
	{
		if(itsDataDebugFile.itsFilePath == string.Empty)
		{
			itsDataDebugFile.itsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KGFLog.txt");
		}
	}
	
	#region Unity3D
	
	protected override void KGFAwake() 
	{
		base.KGFAwake();
		
		if(itsDataDebugFile.itsFilePath == string.Empty)
		{
			itsDataDebugFile.itsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KGFLog.txt");
		}
		// create a new file
		using(var file = new StreamWriter(itsDataDebugFile.itsFilePath, false, Encoding.ASCII))
		{
			file.WriteLine("".PadLeft("FileLogger started: ".Length + DateTime.Now.ToString().Length,'='));
		}
		
		KGFDebug.AddLogger(this);
	}
	
	#endregion
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public override string GetName()
	{
		return "KGFDebugFile";
	}
	
	public override string GetDocumentationPath()
	{
		return "KGFDebugFile_Manual.html";
	}
	
	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}
	
	public override Texture2D GetIcon()
	{
		return null;
	}
	
	/// <summary>
	/// sets the path of the logfile
	/// </summary>
	/// <param name="thePath">the path of the logfile</param>
	public void SetLogFilePath(string thePath)
	{
		itsDataDebugFile.itsFilePath = thePath;
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
	public void Log(KGFDebug.KGFDebugLog aLog)
	{
		Log(aLog.GetLevel(), aLog.GetCategory(), aLog.GetMessage(), aLog.GetStackTrace(), aLog.GetObject() as MonoBehaviour);
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
		#if UNITY_WEBPLAYER
		return;
		#else
		try
		{
			using(var file = new StreamWriter(itsDataDebugFile.itsFilePath, true, Encoding.ASCII))
			{
				if(theObject != null)
				{
					file.WriteLine("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", DateTime.Now.ToString(), theLevel, theCategory, theMessage, theObject.name, theStackTrace, itsDataDebugFile.itsSeparator);
				}
				else
				{
					file.WriteLine("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", DateTime.Now.ToString(), theLevel, theCategory, theMessage, string.Empty, theStackTrace, itsDataDebugFile.itsSeparator);
				}

			}
		}
		catch(Exception e)
		{
			Debug.LogError("couldn't write to file " + itsDataDebugFile.itsFilePath + ". " + e.Message);
		}
		#endif
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void SetMinimumLogLevel(KGFeDebugLevel theLevel)
	{
		itsDataDebugFile.itsMinimumLogLevel = theLevel;
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public KGFeDebugLevel GetMinimumLogLevel()
	{
		return itsDataDebugFile.itsMinimumLogLevel;
	}
	
	#region KGFIValidator
	
	public override KGFMessageList Validate()
	{
		KGFMessageList aMessageList = new KGFMessageList();
		
		if(itsDataDebugFile.itsSeparator.Length == 0)
		{
			aMessageList.AddInfo("no seperator is set");
		}
		
		if(itsDataDebugFile.itsFilePath == string.Empty)
		{
			aMessageList.AddInfo("no file path set. path will be set to desktop.");
		}
		else
		{
			if(!Directory.Exists(Path.GetDirectoryName(itsDataDebugFile.itsFilePath)))
			{
				aMessageList.AddError("the current directory doesn`t exist");
			}
		}
		
		return aMessageList;
	}
	
	#endregion
}
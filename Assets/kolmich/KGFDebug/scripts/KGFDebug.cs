// <author>Christoph Hausjell</author>
// <email>christoph.hausjell@kolmich.at</email>
// <summary>Includes the KGFIDebug interface, the KGFeDebugLevel enumeration, the KGFDataDebug class and the KGFDebug class.
// This classes and enumeration provide all basic features of the Kolmich Game Framework Debug Module.
// </summary>

using System;
using System.Text;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// defines all available debug levels. eAll is equal to 0, eOff is equal to 6
/// </summary>
public enum KGFeDebugLevel
{
	eAll = 0,
	eDebug = 1,
	eInfo = 2,
	eWarning = 3,
	eError = 4,
	eFatal = 5,
	eOff = 6
}

/// <summary>
///	Represents the central interface for all Debug and Log Messages.
/// </summary>
/// <remarks>
/// This class receives log messages and redirects them to all
/// registered loggers. Loggers can be added and removed in
/// runtime. If you like to use the KOLMICH Game Framework Debug
/// Module make sure this class is instantiated and available in
/// runtime. To keep performance side effects of logging as low
/// as possible, make sure to use the highest possible minimal
/// log level for each registered logger. This class is
/// implemented as Singleton (only one instance possible).
/// </remarks>
public class KGFDebug : KGFModule, KGFIValidator
{
	public KGFDebug() : base(new Version(1,2,0,0), new Version(1,2,0,0))
	{

	}
	
	#region internal classes
	
	/// <remarks>
	/// contains all data available for customization in the Unity3D inspector
	/// </remarks>
	[System.Serializable]
	public class KGFDataDebug
	{
		public Texture2D itsIconModule = null;
		public KGFeDebugLevel itsMinimumStackTraceLevel = KGFeDebugLevel.eFatal;
	}
	
	/// <remarks>
	/// contains all data for one log message (level, category, message, time, stack trace, filename, object reference)
	/// </remarks>
	public class KGFDebugLog
	{
		// level of this message
		private KGFeDebugLevel itsLevel;
		// category of this log message
		private string itsCategory;
		// message of this log entry
		private string itsMessage;
		// time when this log was created
		private DateTime itsLogTime;
		// stack trace of the log message
		private string itsStackTrace;
		// reference to the object that created this log message
		private object itsObject;
		
		/// <summary>
		/// creates a KGFDebugLog instance with no reference to a MonoBehaviour game object
		/// </summary>
		/// <param name="theLevel">the log level of this log entry</param>
		/// <param name="theCategory">the category of this log entry</param>
		/// <param name="theMessage">the message of this log entry</param>
		/// <param name="theStackTrace">the stack trace of this log entry</param>
		/// <param name="theObject">the game object referenced to this log entry</param>
		public KGFDebugLog(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace)
		{
			itsLevel = theLevel;
			itsCategory = theCategory;
			itsMessage = theMessage;
			itsLogTime = DateTime.Now;
			itsStackTrace = theStackTrace;
			itsObject = null;
		}
		
		/// <summary>
		/// creates a KGFDebugLog instance with a reference to a MonoBehaviour game object
		/// </summary>
		/// <param name="theLevel">the log level of this log entry</param>
		/// <param name="theCategory">the category of this log entry</param>
		/// <param name="theMessage">the message of this log entry</param>
		/// <param name="theStackTrace">the stack trace of this log entry</param>
		/// <param name="theObject">the game object referenced to this log entry</param>
		public KGFDebugLog(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, object theObject)
		{
			itsLevel = theLevel;
			itsCategory = theCategory;
			itsMessage = theMessage;
			itsLogTime = DateTime.Now;
			itsStackTrace = theStackTrace;
			itsObject = theObject;
		}
		
		/// <summary>
		/// use this method to get the log level of this entry
		/// </summary>
		/// <returns>the log level of this log entry</returns>
		public KGFeDebugLevel GetLevel()
		{
			return itsLevel;
		}
		
		/// <summary>
		/// use this method to get the log categoryl of this entry
		/// </summary>
		/// <returns>the log category of this log entry</returns>
		public string GetCategory()
		{
			return itsCategory;
		}
		
		/// <summary>
		/// use this method to get the log message of this entry
		/// </summary>
		/// <returns>the log message of this log entry</returns>
		public string GetMessage()
		{
			return itsMessage;
		}
		
		/// <summary>
		/// use this method to get the log time of this entry
		/// </summary>
		/// <returns>the log time of this log entry</returns>
		public DateTime GetLogTime()
		{
			return itsLogTime;
		}
		
		/// <summary>
		/// use this method to get the stack trace of this entry
		/// </summary>
		/// <returns>the stack trace of this log entry</returns>
		public string GetStackTrace()
		{
			return itsStackTrace;
		}
		
		/// <summary>
		/// use this method to get object that created this log entry
		/// </summary>
		/// <returns>the object wich created this log entry</returns>
		public object GetObject()
		{
			return itsObject;
		}
	}
	
	#endregion
	
	#region members
	
	//holds the only instance of the debugger
	static KGFDebug itsInstance = null;
	
	/// <summary>
	/// Contains all that that can be modified in the Unity3D inspector.
	/// </summary>
	/// <remarks>
	/// This class goups all available customizable modifiers of the Debug Module. For example the minimum stack trace level of the Debug Module.
	/// </remarks>
	public KGFDataDebug itsDataModuleDebugger = new KGFDataDebug();
	
	// list containing all registered loggers
	private static List<KGFIDebug> itsRegisteredLogger = new List<KGFIDebug>();
	
	// list containing all cahced logs on start
	private static List<KGFDebugLog> itsCachedLogs = new List<KGFDebugLog>();
	
	private static bool itsAlreadyChecked = false;
	
	#endregion
	
	#region methods
	
	#region Unity3D methods
	
	/// <summary>
	/// This method is called from Unity3D Engine when the script instance is being loaded.
	/// </summary>
	/// <remarks>
	/// Awake is called before the game starts. Any state settings and variable initialization can be placed here.
	/// In this class Awake checks if there is already an instance of KGFDebug. If there is already an instance running, the calling
	/// gameObject will be deleted.
	/// </remarks>
	/// <example>
	/// How to ensure that there is only one instance.
	/// <code>
	/// private static KGFDebug itsInstance = null;
	/// 
	/// protected void Awake()
	/// {
	///		//check if a KGFDebug instance is already running
	///		if (itsInstance == null)
	///			itsInstance = this;
	///		else
	///			Destroy(gameObject);
	///}
	/// </code>
	/// </example>
	protected override void KGFAwake()
	{
		base.KGFAwake();
		//check if a KGFDebug instance is already running
		if (itsInstance == null)
		{
			itsInstance = this;
		}
		else
		{
			if(itsInstance != this)
			{
				UnityEngine.Debug.Log("there is more than one KFGDebug instance in this scene. please ensure there is always exactly one instance in this scene");
				Destroy(gameObject);
			}
		}
	}
	
	/// <summary>
	/// This method is called from Unity3D Engine when the script instance is being loaded and after all Awake methods were called.
	/// </summary>
	/// <remarks>
	/// Start is called before the game starts but after all Awake methods were called. Any state settings and variable initialization can be placed here.
	/// In this class Start checks if the itsFillDemoData Flag is set. If so the start method will fill the debugger with some demo data.
	/// </remarks>
	protected void Start()
	{
		if(itsCachedLogs != null)
		{
			// clear all cached logs
			itsCachedLogs.Clear();
			itsCachedLogs = null;
		}
	}
	
	#endregion
	
	#region methods
	
	#region public methods
	
	/// <summary>
	/// Use this Method to get the only instance of this class.
	/// </summary>
	/// <returns>returns the only instance of the KGFDebug class</returns>
	public static KGFDebug GetInstance()
	{
		return itsInstance;
	}
	
	public override string GetName()
	{
		return "KGFDebug";
	}
	
	public override string GetDocumentationPath()
	{
		return "KGFDebug_Manual.html";
	}
	
	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}
	
	/// <summary>
	/// Use this method to get the icon of the Kolmich Game Framework Debug Module.
	/// </summary>
	/// <remarks>
	/// Each module of the Kolmich Game Framework provides its own icon. Use this method to get the icon of the Debug Module.
	/// </remarks>
	/// <example>
	/// How to get the Debug Module Icon
	/// <code>
	/// Texture2D aIcon = KGFDebug.GetIcon();
	/// </code>
	/// </example>
	/// <returns>returns the icon of the Kolmich Game Framework Debug Module. retuns null if no icon was found</returns>
	public override Texture2D GetIcon()
	{
		CheckInstance();
		
		if(itsInstance != null)
		{
			return itsInstance.itsDataModuleDebugger.itsIconModule;
		}
		else
		{
			
			return null;
		}
	}
	
	/// <summary>
	/// returns the logger with the specified name.
	/// </summary>
	/// <remarks>
	/// Using this method you can receive the first logger matching the name given as parameter.
	/// </remarks>
	/// <example>
	/// How to get a logger implementation by name
	/// <code>
	/// KGFIDebug aLogger = KGFDebug.GetLogger("myLogger");
	/// </code>
	/// </example>
	/// <param name="theName">name of the logger</param>
	/// <returns>Returns null if the name could not be found in the list of registered loggers. if there are more loggers registered with the same name, the first match will be returned.</returns>
	public static KGFIDebug GetLogger(string theName)
	{
		foreach(KGFIDebug aLog in itsRegisteredLogger)
		{
			if(aLog.GetName().Equals(theName))
			{
				return aLog;
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// Adds a new logger to the logger list.
	/// </summary>
	/// <remarks>
	/// Loggers can be used to process the input of the KGFDebug class in a custom way. For example display messages on screen, write them into
	/// a file or end them to a webserver. To create your own logger implement the KGFIDebug interface and attach your instance to the KGFDebug class by
	/// calling Add(). An instance of a logger can be added only once, but there can be multiple loggers with the same name.
	/// </remarks>
	/// <example>
	/// How to add a logger to the KGFDebug class
	/// <code>
	/// public class KGFSampleLogger : MonoBehaviour, KGFIDebug
	/// {
	/// 	public void Start()
	/// 	{
	/// 		KGFDebug.AddLogger(this);
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theLogger">instance of the logger that should be added to the list</param>
	public static void AddLogger(KGFIDebug theLogger)
	{
		CheckInstance();
		
		if(!itsRegisteredLogger.Contains(theLogger))
		{
			itsRegisteredLogger.Add(theLogger);
			
			if(itsCachedLogs != null)
			{
				foreach(KGFDebugLog aLog in itsCachedLogs)
				{
					theLogger.Log(aLog);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("this logger is already registered.");
		}
	}
	
	/// <summary>
	/// removes a new logger to the logger list.
	/// </summary>
	/// <remarks>
	/// By removing your logger from the list of registered loggers, it doesn`t receive any further log messages. This method can be called at runtime to deactivate loggers.
	/// </remarks>
	/// <example>
	/// How to remove a logger to the KGFDebug class
	/// <code>
	/// public class KGFSampleLogger : MonoBehaviour, KGFIDebug
	/// {
	/// 	public void Awake()
	/// 	{
	/// 		KGFDebug.AddLogger(this);
	/// 	}
	/// }
	/// 
	/// public class MyGameScript : MonoBehaviour
	/// {
	/// 	private KGFIDebug itsLogger = new KGFSampleLogger();
	/// 
	/// 	public void Awake()
	/// 	{
	/// 		//activate the Logger
	/// 		KGFDebug.AddLogger(itsLogger);
	/// 	}
	/// 
	/// 	public void Start()
	/// 	{
	/// 		//deactivate the Logger
	/// 		KGFDebug.RemoveLogger(itsLogger);
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theLogger">instance of the logger that should be added to the list</param>
	public static void RemoveLogger(KGFIDebug theLogger)
	{
		CheckInstance();

		if(itsRegisteredLogger.Contains(theLogger))
		{
			itsRegisteredLogger.Remove(theLogger);
		}
		else
		{
			UnityEngine.Debug.LogError("the logger you tried to remove wasnt found.");
		}
	}
	
	private static void CheckInstance()
	{
		// check if the cheat module is already activated
		if(itsInstance == null)
		{
			UnityEngine.Object theObject = UnityEngine.Object.FindObjectOfType(typeof(KGFDebug));
			
			if(theObject != null)
			{
				itsInstance = theObject as KGFDebug;
			}
			else
			{
				if(!itsAlreadyChecked)
				{
					UnityEngine.Debug.LogError("Kolmich Debug Module is not running. Make sure that there is an instance of the KGFDebug prefab in the current scene. Adding loggers in Awake() can cause problems, prefer to add loggers in Start().");
					itsAlreadyChecked = true;
				}
			}
		}
	}
	
	#region public log methods
	
	#region eDebug
	
	/// <summary>
	/// creates a debug message with text only
	/// </summary>
	/// <remarks>
	/// Generates a basic debug message without category. All messages with no category can be found under "uncategorized".
	/// </remarks>
	/// <example>
	/// How to create a debug message
	/// <code>
	/// KGFDebug.LogDebug("this is my first debug message");
	/// </code>
	/// </example>
	/// <param name="theMessage">the text of the debug message</param>
	public static void LogDebug(string theMessage)
	{
		Log(KGFeDebugLevel.eDebug, "uncategorized", theMessage);
	}
	
	/// <summary>
	/// creates a debug message with text and category
	/// </summary>
	/// <remarks>
	/// Generates a basic debug message with category.
	/// </remarks>
	/// <example>
	/// How to create a debug message with category
	/// <code>
	/// KGFDebug.LogDebug("this is my first debug message", "FirstCategory");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the debug message</param>
	/// <param name="theCategory">category of the debug message</param>
	public static void LogDebug(string theMessage, string theCategory)
	{
		Log(KGFeDebugLevel.eDebug, theCategory, theMessage);
	}
	
	/// <summary>
	/// creates an extended debug message with text, category and object reference
	/// </summary>
	/// <remarks>
	/// Generates a extended debug message with category and object reference.
	/// </remarks>
	/// <example>
	/// How to create a debug message
	/// <code>
	/// public class Enemy : MonoBehaviour
	/// {
	/// 	private int itsHealth = 0;
	/// 
	/// 	public void Awake()
	/// 	{
	/// 		itsHealth = 100;
	/// 		KGFDebug.LogDebug("enemy with 100 health was created", "Enemys", this);
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the debug message</param>
	/// <param name="theCategory">category of the debug message</param>
	/// <param name="theObject">object which created this debug message</param>
	public static void LogDebug(string theMessage, string theCategory, MonoBehaviour theObject)
	{
		Log(KGFeDebugLevel.eDebug, theCategory, theMessage, theObject);
	}
	
	#endregion
	
	#region eInfo
	
	/// <summary>
	/// creates an info message with text only
	/// </summary>
	/// <remarks>
	/// Generates a simple info message without category. All messages with no category can be found under "uncategorized".
	/// </remarks>
	/// <example>
	/// How to create a info message
	/// <code>
	/// KGFDebug.LogInfo("this is my first info message");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the info message</param>
	/// <param name="theCategory">category of the info message</param>
	public static void LogInfo(string theMessage)
	{
		Log(KGFeDebugLevel.eInfo, "uncategorized", theMessage);
	}
	
	/// <summary>
	/// creates an info message with text and category
	/// </summary>
	/// <remarks>
	/// Generates an info debug message with category.
	/// </remarks>
	/// <example>
	/// How to create an info message with category
	/// <code>
	/// KGFDebug.LogInfo("this is my first info message", "FirstCategory");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the info message</param>
	/// <param name="theCategory">category of the info message</param>
	public static void LogInfo(string theMessage, string theCategory)
	{
		Log(KGFeDebugLevel.eInfo, theCategory, theMessage);
	}
	
	/// <summary>
	/// creates an extended info message with text, category and object reference
	/// </summary>
	/// <remarks>
	/// Generates a extended info message with category and object reference.
	/// </remarks>
	/// <example>
	/// How to create a info message
	/// <code>
	/// public class Enemy : MonoBehaviour
	/// {
	/// 	private int itsHealth = 0;
	/// 
	/// 	public void Awake()
	/// 	{
	/// 		itsHealth = 100;
	/// 	}
	/// 
	/// 	public void AddDamage(int theAmount)
	/// 	{
	/// 		itsHealth -= theAmount;
	///
	/// 		if(itsHealth < 0)
	/// 			KGFDebug.LogInfo("enemy died", "Enemys", this);
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the info message</param>
	/// <param name="theCategory">category of the info message</param>
	/// <param name="theObject">object which created this info message</param>
	public static void LogInfo(string theMessage, string theCategory, MonoBehaviour theObject)
	{
		Log(KGFeDebugLevel.eInfo, theCategory, theMessage, theObject);
	}
	
	#endregion
	
	#region eWarning
	
	/// <summary>
	/// creates a warning message with text only
	/// </summary>
	/// <remarks>
	/// Generates a simple warning message without category. All messages with no category can be found under "uncategorized".
	/// </remarks>
	/// <example>
	/// How to create a warning message
	/// <code>
	/// KGFDebug.LogWarning("this is my first warning message");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the warning message</param>
	public static void LogWarning(string theMessage)
	{
		Log(KGFeDebugLevel.eWarning, "uncategorized", theMessage);
	}
	
	/// <summary>
	/// creates a warning message with text and category
	/// </summary>
	/// <remarks>
	/// Generates a basic warning message with category.
	/// </remarks>
	/// <example>
	/// How to create a warning message with category
	/// <code>
	/// KGFDebug.LogWarning("this is my first warning message", "FirstCategory");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the warning message</param>
	/// <param name="theCategory">category of the warning message</param>
	public static void LogWarning(string theMessage, string theCategory)
	{
		Log(KGFeDebugLevel.eWarning, theCategory, theMessage);
	}
	
	/// <summary>
	/// creates an extended warning message with text, category and object reference
	/// </summary>
	/// <remarks>
	/// Generates a extended warning message with category and object reference.
	/// </remarks>
	/// <example>
	/// How to create a warning message
	/// <code>
	/// public class Enemy : MonoBehaviour
	/// {
	/// 	private int itsHealth = 0;
	/// 	private bool itsAlive = true;
	/// 
	/// 	public void FixedUpdate()
	/// 	{
	/// 		if(itsHealth < 0 && itsAlive == true)
	/// 		{
	/// 			KGFDebug.LogWarning("enemys health is below 0, but the enemy is still alive.", "Enemys", this);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the warning message</param>
	/// <param name="theCategory">category of the warning message</param>
	/// <param name="theObject">object which created this warning message</param>
	public static void LogWarning(string theMessage, string theCategory, MonoBehaviour theObject)
	{
		Log(KGFeDebugLevel.eWarning, theCategory, theMessage, theObject);
	}
	
	#endregion
	
	#region eError
	
	/// <summary>
	/// creates an error message with text only
	/// </summary>
	/// <remarks>
	/// Generates a simple error message without category. All messages with no category can be found under "uncategorized".
	/// </remarks>
	/// <example>
	/// How to create an error message
	/// <code>
	/// KGFDebug.LogError("this is my first error message");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the error message</param>
	public static void LogError(string theMessage)
	{
		Log(KGFeDebugLevel.eError, "uncategorized", theMessage);
	}
	
	/// <summary>
	/// creates an error message with text and category
	/// </summary>
	/// <remarks>
	/// Generates a basic error message with category.
	/// </remarks>
	/// <example>
	/// How to create an error message with category
	/// <code>
	/// KGFDebug.LogError("this is my first error message", "FirstCategory");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the error message</param>
	/// <param name="theCategory">category of the error message</param>
	public static void LogError(string theMessage, string theCategory)
	{
		Log(KGFeDebugLevel.eError, theCategory, theMessage);
	}
	
	/// <summary>
	/// creates an extended warning message with text, category and object reference
	/// </summary>
	/// <remarks>
	/// Generates an extended warning message with category and object reference.
	/// </remarks>
	/// <example>
	/// How to create an error message
	/// <code>
	/// public class Enemy : MonoBehaviour
	/// {
	/// 	private AIScript itsAI = null;
	/// 
	/// 	public void Update()
	/// 	{
	/// 		if(itsAI = null)
	/// 		{
	/// 			KGFDebug.LogError("no AI script attached to this enemy. All enemys must haven an AI script attached.", "Enemys", this);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the error message</param>
	/// <param name="theCategory">category of the error message</param>
	/// <param name="theObject">object which created this error message</param>
	public static void LogError(string theMessage, string theCategory, MonoBehaviour theObject)
	{
		Log(KGFeDebugLevel.eError, theCategory, theMessage, theObject);
	}
	
	#endregion
	
	#region eFatal
	
	/// <summary>
	/// creates a fatal error message with text only
	/// </summary>
	/// <remarks>
	/// Generates a simple fatal error message without category. All messages with no category can be found under "uncategorized".
	/// </remarks>
	/// <example>
	/// How to create a fatal error message
	/// <code>
	/// KGFDebug.LogFatal("this is my first fatal error message");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the fatal error message</param>
	public static void LogFatal(string theMessage)
	{
		Log(KGFeDebugLevel.eFatal, "uncategorized", theMessage);
	}
	
	/// <summary>
	/// creates a fatal error message with text and category
	/// </summary>
	/// <remarks>
	/// Generates a basic fatal error message with category.
	/// </remarks>
	/// <example>
	/// How to create a fatal error message with category
	/// <code>
	/// KGFDebug.LogFatal("this is my first fatal error message", "FirstCategory");
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the fatal error message</param>
	/// <param name="theCategory">category of the fatal error message</param>
	public static void LogFatal(string theMessage, string theCategory)
	{
		Log(KGFeDebugLevel.eFatal, theCategory, theMessage);
	}
	
	/// <summary>
	/// creates an extended fatal error message with text, category and object reference
	/// </summary>
	/// <remarks>
	/// Generates an extended fatal error message with category and object reference.
	/// </remarks>
	/// <example>
	/// How to create an fatal error  message
	/// <code>
	/// public class MayaCalender : MonoBehaviour
	/// {
	/// 	public Update()
	/// 	{
	/// 		if(DateTime.Now > new DateTime(2012, 12, 21, 0, 0, 0)
	/// 		{
	/// 			KGFDebug.LogFatal("the world has come to an end. No reason to continue.", "End of the World", this);
	/// 			Detroy(gameObject);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <param name="theMessage">text of the fatal error message</param>
	/// <param name="theCategory">category of the fatal error message</param>
	/// <param name="theObject">object which created this fatal error message</param>
	public static void LogFatal(string theMessage, string theCategory, MonoBehaviour theObject)
	{
		Log(KGFeDebugLevel.eFatal, theCategory, theMessage, theObject);
	}
	
	#endregion
	
	#endregion
	
	#endregion
	
	#region private log methods
	
	/// <summary>
	/// sends the log command to all registered loggers
	/// </summary>
	/// <param name="theLevel">level of the log message</param>
	/// <param name="theCategory">category of the log message</param>
	/// <param name="theMessage">message of the log entry</param>
	private static void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage)
	{
		Log(theLevel, theCategory, theMessage, null);
	}
	
	/// <summary>
	/// sends the log command to all registered loggers
	/// </summary>
	/// <param name="theLevel">level of the log message</param>
	/// <param name="theCategory">category of the log message</param>
	/// <param name="theMessage">message of the log entry</param>
	/// <param name="theObject">gameObject referenced to this log entry</param>
	private static void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage, MonoBehaviour theObject)
	{
		CheckInstance();
		
		if(itsInstance != null)
		{
			StringBuilder aStackString = new StringBuilder();
			
			// only create the stack trace if the level of this message is high enough
			if(theLevel >= itsInstance.itsDataModuleDebugger.itsMinimumStackTraceLevel)
			{
				// create the stack trace
				StackTrace aStackTrace = new StackTrace(true);
				
				// remove the first two frames because of the internam method calls
				for(int i = 2; i < aStackTrace.FrameCount; i++)
				{
					aStackString.Append(aStackTrace.GetFrames()[i].ToString());
					aStackString.Append(Environment.NewLine);
				}
			}
			
			KGFDebugLog aLog = new KGFDebugLog(theLevel, theCategory, theMessage, aStackString.ToString(), theObject);
			
			if(itsCachedLogs != null)
			{
				itsCachedLogs.Add(aLog);
			}

			// send the message to all registered implementations
			foreach(KGFIDebug aLogger in itsRegisteredLogger)
			{
				if(aLogger.GetMinimumLogLevel() <= theLevel)
				{
					aLogger.Log(aLog);
				}
			}
		}
	}
	
	#endregion
	
	#endregion
	
	#endregion
	
	#region KGFIValidator
	
	public override KGFMessageList Validate()
	{
		KGFMessageList aMessageList = new KGFMessageList();
		
		if(itsDataModuleDebugger.itsIconModule == null)
		{
			aMessageList.AddWarning("the module icon is missing");
		}
		
		return aMessageList;
	}
	
	#endregion
}
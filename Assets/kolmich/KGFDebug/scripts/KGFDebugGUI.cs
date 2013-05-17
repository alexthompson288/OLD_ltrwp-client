// <author>Christoph Hausjell</author>
// <email>christoph.hausjell@kolmich.at</email>
// <date>2012-03-13</date>
// <summary>short summary</summary>

using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// defines the item count visible at one log page. e10 is equal to 10, e50 is equal to 50, ...
/// </summary>
public enum KGFeItemsPerPage
{
	e10 = 10,
	e25 = 25,
	e50 = 50,
	e100 = 100,
	e250 = 250,
	e500 = 500
}

public class KGFDebugGUI : KGFModule, KGFIDebug
{
	#region interal classes
	
	/// <summary>
	/// contains all data available for custimization in the UNity3D inspector
	/// </summary>
	[System.Serializable]
	public class KGFDataModuleGUILogger
	{
		// minimum Log level of the Debugger
		public KGFeDebugLevel itsMinimumLogLevel = KGFeDebugLevel.eAll;
		// current item display count per page
		public KGFeItemsPerPage itsLogsPerPage = KGFeItemsPerPage.e25;
		// minimum Log level when the gui is expanded
		public KGFeDebugLevel itsMinimumExpandLogLevel = KGFeDebugLevel.eOff;

		// color of debug messages
		public Color itsColorDebug = Color.white;
		// solor of info messages
		public Color itsColorInfo = Color.grey;
		// color of warning messages
		public Color itsColorWarning = Color.yellow;
		// color of error messasges
		public Color itsColorError = Color.red;
		// color of fatal error messages
		public Color itsColorFatal = Color.magenta;
		
		// icon of debug messages
		public Texture2D itsIconDebug = null;
		// icon of info messages
		public Texture2D itsIconInfo = null;
		// icon of warning messages
		public Texture2D itsIconWarning = null;
		// icon of error messages
		public Texture2D itsIconError = null;
		// icon of fatal error messages
		public Texture2D itsIconFatal = null;
		
		// icon of help button
		public Texture2D itsIconHelp = null;
		// icon of left button
		public Texture2D itsIconLeft = null;
		// icon of right button
		public Texture2D itsIconRight = null;
		
		public float itsFPSUpdateInterval = 0.5f;
		
		public KeyCode itsHideKeyModifier = KeyCode.None;
		public KeyCode itsHideKey = KeyCode.F1;
		
		public KeyCode itsExpandKeyModifier = KeyCode.LeftAlt;
		public KeyCode itsExpandKey = KeyCode.F1;
		
		// flag if the debugger is hidden
		public bool itsVisible = true;
	}
	
	/// <summary>
	/// contains all data for one log category (message count, category name, selected state)
	/// </summary>
	private class KGFDebugCategory
	{
		// number of messages in this category
		private int itsCount;
		// name of thios category
		private string itsName;
		// sate if this category is visible
		public bool itsSelectedState;
		
		/// <summary>
		/// creates a new category instance with name and execution count 0
		/// </summary>
		/// <param name="theName">the name of this debug category</param>
		public KGFDebugCategory(string theName)
		{
			itsName = theName;
			itsCount = 0;
		}
		
		/// <summary>
		/// use this method to get the name of the category
		/// </summary>
		/// <returns>returns the name of the category</returns>
		public string GetName()
		{
			return itsName;
		}
		
		/// <summary>
		/// increases the message count of the category by steps of 1
		/// </summary>
		public void IncreaseCount()
		{
			itsCount++;
		}
		/// <summary>
		/// increases the message count of the category by steps of 1
		/// </summary>
		public void DecreaseCount()
		{
			itsCount--;
		}
		
		/// <summary>
		/// use this method to get the message count of the category
		/// </summary>
		/// <returns>returns the message count of the category</returns>
		public int GetCount()
		{
			return itsCount;
		}
	}
	
	#endregion
	
	// holds the only instance of the GUILogger
	private static KGFDebugGUI itsInstance;
	
	// all items fitting to the current filter options
	private KGFDataTable itsLogTable = new KGFDataTable();
	// the table control that shows all log entries
	private KGFGUIDataTable itsTableControl;
	
	// all logs
	private List<KGFDebug.KGFDebugLog> itsLogList = new List<KGFDebug.KGFDebugLog>();
	// all log categorys
	private Dictionary<string, KGFDebugCategory> itsLogCategories = new Dictionary<string, KGFDebugCategory>();
	// list to watch the state of the log level filter
	private Dictionary<KGFeDebugLevel, bool> itsLogLevelFilter = new Dictionary<KGFeDebugLevel, bool>();
	
	// scrollview position of the category column
	private Vector2 itsCategoryScrollViewPosition = Vector2.zero;
	
	// search filter for the messages
	private string itsSearchFilterMessage = "Search";
	// search filter for the categories
	private string itsSearchFilterCategory = "Search";
	
	// the current window height
	private float itsCurrentHeight = Screen.height;
	// flag if the debugger is open
	private bool itsOpen = false;
	
	// flag if the live search text has changed
	private bool itsLiveSearchChanged = false;
	// last time a change was registered in the search field
	private float itsLastChangeTime = 0.0f;
	
	// current page of the log list
	private uint itsCurrentPage = 0;
	// current selected row to display the details
	private KGFDataRow itsCurrentSelectedRow = null;
	
	// list that counts the messages of categorys
	private Dictionary<KGFeDebugLevel, uint> itsLogCategoryCount = new Dictionary<KGFeDebugLevel, uint>();
	
	// contains all settings
	public KGFDataModuleGUILogger itsDataModuleGUILogger = new KGFDataModuleGUILogger();
	
	// defindes the rectangle of the open window
	private Rect itsOpenWindow;
	private Rect itsMinimizedWindow;
	
	#region "FPS counter"
	
	private float itsAccumulatedFrames = 0;
	private int itsFramesInInterval = 0;
	private float itsTimeLeft;
	private float itsCurrentFPS = 0.0f;
	
	#endregion
	
	private bool itsHover = false;
	private bool itsFocus = false;
	private bool itsLeaveFocus = false;
	//private bool itsOldCursorState;
	
	public KGFDebugGUI() : base(new Version(1,0,0,1), new Version(1,1,0,0))
	{

	}

	#region unity console auto display
	// log callback from unity
	void OnEnable()
	{
		Application.RegisterLogCallback(HandleLog);
	}
	void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}
	void HandleLog(string theLogString, string theStackTrace, LogType theLogType)
	{
		KGFeDebugLevel aDebugLevel = KGFeDebugLevel.eInfo;
		switch(theLogType)
		{
			case LogType.Assert:
				aDebugLevel = KGFeDebugLevel.eFatal;
				break;
			case LogType.Error:
				aDebugLevel = KGFeDebugLevel.eError;
				break;
			case LogType.Exception:
				aDebugLevel = KGFeDebugLevel.eError;
				break;
			case LogType.Log:
				aDebugLevel = KGFeDebugLevel.eInfo;
				break;
			case LogType.Warning:
				aDebugLevel = KGFeDebugLevel.eWarning;
				break;
		}
		Log(aDebugLevel,"CONSOLE",theLogString,theStackTrace);
	}
	#endregion
	
	#region Unity3D
	
	protected override void KGFAwake()
	{
		if (itsInstance == null)
		{
			Init();
			
			KGFDebug.AddLogger(this);
		}
		else
		{
			if(itsInstance != this)
			{
				UnityEngine.Object.Destroy(itsInstance.gameObject);
				UnityEngine.Debug.Log("multiple instances of KGFGUILogger are not allowed");
				return;
			}
		}
	}
	
	public void Start()
	{
		itsTimeLeft = itsDataModuleGUILogger.itsFPSUpdateInterval;
	}
	
	public void Update()
	{
		itsTimeLeft -= Time.deltaTime;
		itsAccumulatedFrames += Time.timeScale/Time.deltaTime;
		itsFramesInInterval++;
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			itsLeaveFocus = true;
		}
		
		if(itsDataModuleGUILogger.itsHideKey != KeyCode.None && !Input.GetKey(itsDataModuleGUILogger.itsExpandKeyModifier))
		{
			if((Input.GetKey(itsDataModuleGUILogger.itsHideKeyModifier) && Input.GetKey(itsDataModuleGUILogger.itsHideKeyModifier) && Input.GetKeyDown(itsDataModuleGUILogger.itsHideKey))
			   || (itsDataModuleGUILogger.itsHideKeyModifier == KeyCode.None && Input.GetKeyDown(itsDataModuleGUILogger.itsHideKey)))
			{
				itsDataModuleGUILogger.itsVisible = !itsDataModuleGUILogger.itsVisible;
			}
		}
		
		if(itsDataModuleGUILogger.itsExpandKey != KeyCode.None && !Input.GetKey(itsDataModuleGUILogger.itsHideKeyModifier))
		{
			if((Input.GetKey(itsDataModuleGUILogger.itsExpandKeyModifier) && Input.GetKeyDown(itsDataModuleGUILogger.itsExpandKey))
			   || (itsDataModuleGUILogger.itsExpandKeyModifier == KeyCode.None && Input.GetKeyDown(itsDataModuleGUILogger.itsExpandKey)))
			{
				itsOpen = !itsOpen;
				if(itsOpen == true)
					itsDataModuleGUILogger.itsVisible = true;
			}
		}
	}
	
	/// <summary>
	/// returns the KGFDebugGUI singleton
	/// </summary>
	/// <remarks>
	/// use this method to check if the mouse is currently over the gui of this module
	/// </remarks>
	private static KGFDebugGUI GetInstance()
	{
		return itsInstance;
	}
	
	/// <summary>
	/// returns if the KGFDebugGUI is currently active
	/// </summary>
	/// <remarks>
	/// use this method to check if one of the controls is currently focused
	/// </remarks>
	public static bool GetFocused()
	{
		if(itsInstance != null && itsInstance.itsDataModuleGUILogger.itsVisible && (itsInstance.itsHover || itsInstance.itsFocus))
		{
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// returns if the mouse is currently over the KGFDebugGUI module GUI
	/// </summary>
	public static bool GetHover()
	{
		if(itsInstance != null && itsInstance.itsDataModuleGUILogger.itsVisible && itsInstance.itsHover)
		{
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// renders the KGFDebugGUI
	/// </summary>
	public static void Render()
	{
		KGFGUIUtility.SetSkinIndex(0);
		if(itsInstance != null)
		{
			if(!itsInstance.itsDataModuleGUILogger.itsVisible)
			{
				return;
			}
			
			RenderHelpWindow();
			
			// check if the window is open
			if (itsInstance.itsOpen)
			{
				itsInstance.itsOpenWindow.x = 0;
				itsInstance.itsOpenWindow.y = 0;
				itsInstance.itsOpenWindow.width = Screen.width;
				itsInstance.itsOpenWindow.height = itsInstance.itsCurrentHeight;
				
				if(itsInstance.itsCurrentHeight < KGFGUIUtility.GetSkinHeight() * 11.0f)
				{
					itsInstance.itsCurrentHeight = KGFGUIUtility.GetSkinHeight() * 11.0f;
				}
				else if(itsInstance.itsCurrentHeight > Screen.height / 3.0f * 2.0f)
				{
					itsInstance.itsCurrentHeight = Screen.height / 3.0f * 2.0f;
				}
				
				GUILayout.BeginArea(itsInstance.itsOpenWindow);
				{
					KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxDecorated);
					{
						Texture2D aIcon = null;
						
						if(KGFDebug.GetInstance() != null)
						{
							aIcon = KGFDebug.GetInstance().GetIcon();
						}
						
						// draw title bar
						KGFGUIUtility.BeginWindowHeader("KGFDebugger", aIcon);
						{
							itsInstance.DrawSummary();
							GUILayout.FlexibleSpace();
							
							float aOldSize = itsInstance.itsCurrentHeight;
							
							itsInstance.itsCurrentHeight = KGFGUIUtility.HorizontalSlider(itsInstance.itsCurrentHeight, Screen.height / 3.0f * 2.0f, KGFGUIUtility.GetSkinHeight() * 11.0f, GUILayout.Width(75));
							
							if(aOldSize != itsInstance.itsCurrentHeight)
							{
								itsInstance.SaveSizeToPlayerPrefs();
							}
							
							if(KGFGUIUtility.Button(itsInstance.itsDataModuleGUILogger.itsIconHelp, KGFGUIUtility.eStyleButton.eButton))
							{
								OpenHelpWindow(itsInstance);
							}
							itsInstance.itsOpen = KGFGUIUtility.Toggle(itsInstance.itsOpen,"",KGFGUIUtility.eStyleToggl.eTogglSwitch,GUILayout.Width(KGFGUIUtility.GetSkinHeight()));
						}
						KGFGUIUtility.EndWindowHeader(false);
						
						GUILayout.Space(5.0f);
						
						// begin content area
						GUILayout.BeginHorizontal();
						{
							// draw left category column
							KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxDecorated, GUILayout.Width(Screen.width * 0.2f));
							{
								itsInstance.DrawCategoryColumn();
							}
							KGFGUIUtility.EndVerticalBox();
							
							// draw right content column
							KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxDecorated);
							{
								itsInstance.DrawContentColumn();
							}
							KGFGUIUtility.EndVerticalBox();
						}
						GUILayout.EndHorizontal();
					}
					KGFGUIUtility.EndVerticalBox();
				}
				GUILayout.EndArea();
			}
			else
			{
				itsInstance.DrawMinimizedWindow();
			}
			
			#region Mouse Cursor
			
			Vector3 aMousePosition = Input.mousePosition;
			aMousePosition.y = Screen.height - aMousePosition.y;
			
			if(itsInstance.itsOpen)
			{
				if(itsInstance.itsOpenWindow.Contains(aMousePosition))
				{
					if(!itsInstance.itsHover)
					{
						itsInstance.itsHover = true;
						//itsOldCursorState = Screen.showCursor;
						//Screen.showCursor = false;
					}
					
					//DrawMouseCursor();
				}
				else if(itsInstance.itsHover && !itsInstance.itsOpenWindow.Contains(aMousePosition))
				{
					itsInstance.itsHover = false;
					//Screen.showCursor = itsOldCursorState;
				}
			}
			else if(!itsInstance.itsOpen)
			{
				if(itsInstance.itsMinimizedWindow.Contains(aMousePosition))
				{
					if(!itsInstance.itsHover)
					{
						itsInstance.itsHover = true;
						//itsOldCursorState = Screen.showCursor;
						//Screen.showCursor = false;
					}
					
					//DrawMouseCursor();
				}
				else if(itsInstance.itsHover && !itsInstance.itsMinimizedWindow.Contains(aMousePosition))
				{
					itsInstance.itsHover = false;
					//Screen.showCursor = itsOldCursorState;
				}
			}
			
			#endregion
			
			GUI.SetNextControlName("");
			if(itsInstance.itsLeaveFocus && itsInstance.itsFocus)
			{
				UnityEngine.Debug.Log("unfocus KGFDebugGUI");
				GUI.FocusControl("");
				itsInstance.itsLeaveFocus = false;
			}
			else
			{
				itsInstance.itsLeaveFocus = false;
			}
		}
		KGFGUIUtility.SetSkinIndex(1);
	}
	
	/// <summary>
	/// expands the KGFDebugGUI
	/// </summary>
	public static void Expand()
	{
		if(itsInstance != null)
		{
			itsInstance.itsOpen = true;
		}
	}
	
	public static bool GetExpanded()
	{
		if(itsInstance != null)
		{
			return itsInstance.itsOpen;
		}
		return false;
	}
	
	/// <summary>
	/// minimizes the KGFDebugGUI
	/// </summary>
	public static void Minimize()
	{
		if(itsInstance != null)
		{
			itsInstance.itsOpen = false;
		}
	}
	
	/// <summary>
	/// for performance reasons this method should be deleted.
	/// when integrating KGFDebugGUI invoke KGFDebugGUI.GetInstance().Render(); in your own one and only OnGUI method
	/// </summary>
	public void OnGUI()
	{
		Render();
	}
	
	#endregion
	
	private void Init()
	{
		if(itsInstance != null)
		{
			return;
		}
		else
		{
			itsInstance = this;
		}
		
		// create all columns that are used to show the log messages
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("Lvl",typeof(string)));
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("Time",typeof(string)));
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("Category",typeof(string)));
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("Message",typeof(string)));
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("StackTrace",typeof(string)));
		itsInstance.itsLogTable.Columns.Add(new KGFDataColumn("Object",typeof(object)));
		
		// create the log table control
		itsTableControl = new KGFGUIDataTable(itsLogTable, GUILayout.ExpandHeight(true));
		
		// subscribe to the table control events to customize the appearance
		itsTableControl.OnClickRow += new EventHandler(OnLogTableRowIsClicked);
		itsTableControl.PostRenderRow += new EventHandler(PostLogTableRowHook);
		itsTableControl.PreCellContentHandler += PreCellContentHook;
		
		// configure the width and visibility of the columns
		itsTableControl.SetColumnWidth(0, 40);
		itsTableControl.SetColumnWidth(1, 90);
		itsTableControl.SetColumnWidth(2, 150);
		itsTableControl.SetColumnWidth(3, 0);
		itsTableControl.SetColumnVisible(4, false);
		itsTableControl.SetColumnVisible(5, false);
		
		// add to the count list and filter level list all kind of enums
		itsLogLevelFilter.Clear();
		itsLogCategoryCount.Clear();
		foreach(KGFeDebugLevel aLevel in Enum.GetValues(typeof(KGFeDebugLevel)))
		{
			itsLogLevelFilter.Add(aLevel, true);
			itsLogCategoryCount.Add(aLevel, 0);
		}
		
		// Load Player Prefs
		itsDataModuleGUILogger.itsLogsPerPage = (KGFeItemsPerPage)Enum.Parse(typeof(KGFeItemsPerPage), PlayerPrefs.GetInt("KGF.KGFModuleDebugger.itsLogsPerPage", (int)KGFeItemsPerPage.e25).ToString());
		LoadCategoryFilterFromPlayerPrefs();
		
		itsOpenWindow = new Rect(0, 0, Screen.width, itsCurrentHeight);
		itsMinimizedWindow = new Rect(0, 0, Screen.width, 100);
		
		itsInstance.LoadSizeFromPlayerPrefs();
		
		if(itsCurrentHeight == 0.0f)
		{
			//itsOldCursorState = Screen.showCursor;
			itsCurrentHeight = Screen.height * 0.5f;
		}
	}
	
	private void DrawMouseCursor()
	{
		if(KGFGUIUtility.GetStyleCursor() != null)
		{
			Vector3 aMousePosition = Input.mousePosition;
			Rect aRectangle = new Rect(aMousePosition.x, Screen.height - aMousePosition.y, KGFGUIUtility.GetStyleCursor().fixedWidth, KGFGUIUtility.GetStyleCursor().fixedHeight);
			GUI.Label(aRectangle, string.Empty, KGFGUIUtility.GetStyleCursor());
		}
	}
	
	private void SaveSizeToPlayerPrefs()
	{
		PlayerPrefs.SetFloat("KGFDebugGUI.WindowSize", itsCurrentHeight);
		PlayerPrefs.Save();
	}
	
	private void LoadSizeFromPlayerPrefs()
	{
		itsCurrentHeight = PlayerPrefs.GetFloat("KGFDebugGUI.WindowSize", 0.0f);
	}
	
	#region KGFIDebug
	
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
		Init();
		
		itsLogCategoryCount[theLevel]++;
		
		KGFDebug.KGFDebugLog aLogItem;
		
		aLogItem = new KGFDebug.KGFDebugLog(theLevel, theCategory, theMessage, theStackTrace, theObject);
		itsLogList.Add(aLogItem);
		
		if (!itsLogCategories.ContainsKey(theCategory))
		{
			itsLogCategories[theCategory] = new KGFDebugCategory(theCategory);
			itsLogCategories[theCategory].itsSelectedState = true;
		}
		itsLogCategories[theCategory].IncreaseCount();
		
		//only add the item to the result if the log fits to the current filter
		if(FilterDebugLog(aLogItem))
		{
			KGFDataRow aRow = itsLogTable.NewRow();
			
			aRow[0].Value = aLogItem.GetLevel().ToString();
			aRow[1].Value = aLogItem.GetLogTime().ToString("HH:mm:ss.fff");
			aRow[2].Value = aLogItem.GetCategory();
			aRow[3].Value = aLogItem.GetMessage();
			aRow[4].Value = aLogItem.GetStackTrace();
			aRow[5].Value = aLogItem.GetObject();
			
			itsLogTable.Rows.Add(aRow);
		}
		
		if(aLogItem.GetLevel() >= itsDataModuleGUILogger.itsMinimumExpandLogLevel)
		{
			itsDataModuleGUILogger.itsVisible = true;
			itsOpen = true;
		}
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public void SetMinimumLogLevel(KGFeDebugLevel theLevel)
	{
		itsDataModuleGUILogger.itsMinimumLogLevel = theLevel;
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public KGFeDebugLevel GetMinimumLogLevel()
	{
		return itsDataModuleGUILogger.itsMinimumLogLevel;
	}
	
	#endregion
	
	/// <summary>
	/// Gets all categories that are registers in the logger
	/// </summary>
	/// <returns>IEnumerable of DebugCategoies</returns>
	private IEnumerable<KGFDebugCategory> GetAllCategories()
	{
		return itsLogCategories.Values;
	}
	
	/// <summary>
	/// filters the LogItem
	/// </summary>
	/// <param name="theLogItem">the item to filter</param>
	/// <returns>if the item fits to the current selected filter</returns>
	private bool FilterDebugLog(KGFDebug.KGFDebugLog theLogItem)
	{
		// filter levels
		if(itsLogLevelFilter[theLogItem.GetLevel()] == false)
		{
			return false;
		}
		
		// filter Message
		if(!(itsSearchFilterMessage.Equals("Search") || itsSearchFilterMessage.Equals(string.Empty)))
		{
			if(!theLogItem.GetMessage().Trim().ToLower().Contains(itsSearchFilterMessage.Trim().ToLower()))
			{
				return false;
			}
		}
		
		// filter category
		foreach(KGFDebugCategory aCategory in itsLogCategories.Values)
		{
			if(aCategory.itsSelectedState && aCategory.GetName().ToLower().Contains(theLogItem.GetCategory().ToLower()))
			{
				return true;
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// gets the filters LogItem list
	/// </summary>
	/// <returns>IEnumerable of DebugLogs</returns>
	private IEnumerable<KGFDebug.KGFDebugLog> GetFilteredLogList()
	{
		foreach (KGFDebug.KGFDebugLog aDebugLog in itsLogList)
		{
			if (FilterDebugLog(aDebugLog))
			{
				yield return aDebugLog;
			}
		}
		yield break;
	}
	
	/// <summary>
	/// updates the data table with all visible items
	/// </summary>
	private void UpdateLogList()
	{
		// clear the old log table list
		itsLogTable.Rows.Clear();
		
		// add all filter matching items to the log tabe
		foreach(KGFDebug.KGFDebugLog aLog in GetFilteredLogList())
		{
			KGFDataRow aRow = itsLogTable.NewRow();
			
			aRow[0].Value = aLog.GetLevel().ToString();
			aRow[1].Value = aLog.GetLogTime().ToString("HH:mm:ss.fff");
			aRow[2].Value = aLog.GetCategory();
			aRow[3].Value = aLog.GetMessage();
			aRow[4].Value = aLog.GetStackTrace();
			aRow[5].Value = aLog.GetObject();
			
			itsLogTable.Rows.Add(aRow);
		}
		
		UpdateLogListPageNumber();
	}
	
	/// <summary>
	/// updates the displayed page number
	/// </summary>
	private void UpdateLogListPageNumber()
	{
		if(itsLogTable.Rows.Count <= (int)itsDataModuleGUILogger.itsLogsPerPage)
		{
			itsCurrentPage = 0;
		}
		else
		{
			itsCurrentPage = (uint)Mathf.CeilToInt((float)itsLogTable.Rows.Count / (float)itsDataModuleGUILogger.itsLogsPerPage) - 1;
		}
	}
	
	#region GUI helper methods
	
	/// <summary>
	/// Draws the minimized Window
	/// </summary>
	
	private void DrawMinimizedWindow()
	{
		float aHeight = KGFGUIUtility.GetSkinHeight() + KGFGUIUtility.GetStyleButton(KGFGUIUtility.eStyleButton.eButton).margin.vertical + KGFGUIUtility.GetStyleBox(KGFGUIUtility.eStyleBox.eBoxDecorated).padding.vertical;
		
		itsMinimizedWindow.x = 0;
		itsMinimizedWindow.y = 0;
		itsMinimizedWindow.width = Screen.width;
		itsMinimizedWindow.height = aHeight;
		
		GUILayout.BeginArea(itsMinimizedWindow);
		{
			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal();
				{
					KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxDecorated);
					{
						Texture2D aIcon = null;
						
						if(KGFDebug.GetInstance() != null)
						{
							aIcon = KGFDebug.GetInstance().GetIcon();
						}
						
						KGFGUIUtility.BeginWindowHeader("KGFDebugger", aIcon);
						{
							DrawSummary();
							GUILayout.FlexibleSpace();
							if(KGFGUIUtility.Button(itsDataModuleGUILogger.itsIconHelp, KGFGUIUtility.eStyleButton.eButton))
							{
								OpenHelpWindow(itsInstance);
							}
							itsOpen = KGFGUIUtility.Toggle(itsOpen,"",KGFGUIUtility.eStyleToggl.eTogglSwitch,GUILayout.Width(KGFGUIUtility.GetSkinHeight()));
						}
						KGFGUIUtility.EndWindowHeader(false);
					}
					KGFGUIUtility.EndVerticalBox();
				}
				GUILayout.EndHorizontal();
				
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
	
	/// <summary>
	/// draws the summary of the category log counts
	/// </summary>
	private void DrawSummary()
	{
		float aSpaceBetween = 10.0f;
		GUILayout.Space(10.0f);
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxInvisible);
		{
			KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconDebug,KGFGUIUtility.eStyleLabel.eLabel);
			KGFGUIUtility.Label(string.Format("{0}",itsLogCategoryCount[KGFeDebugLevel.eDebug]),KGFGUIUtility.eStyleLabel.eLabel);
			GUILayout.Space(aSpaceBetween);

			KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconInfo,KGFGUIUtility.eStyleLabel.eLabel);
			KGFGUIUtility.Label(string.Format("{0}",itsLogCategoryCount[KGFeDebugLevel.eInfo]),KGFGUIUtility.eStyleLabel.eLabel);
			GUILayout.Space(aSpaceBetween);

			KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconWarning,KGFGUIUtility.eStyleLabel.eLabel);
			KGFGUIUtility.Label(string.Format("{0}",itsLogCategoryCount[KGFeDebugLevel.eWarning]),KGFGUIUtility.eStyleLabel.eLabel);
			GUILayout.Space(aSpaceBetween);

			KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconError,KGFGUIUtility.eStyleLabel.eLabel);
			KGFGUIUtility.Label(string.Format("{0}",itsLogCategoryCount[KGFeDebugLevel.eError]),KGFGUIUtility.eStyleLabel.eLabel);
			GUILayout.Space(aSpaceBetween);

			KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconFatal,KGFGUIUtility.eStyleLabel.eLabel);
			KGFGUIUtility.Label(string.Format("{0}",itsLogCategoryCount[KGFeDebugLevel.eFatal]),KGFGUIUtility.eStyleLabel.eLabel);
			GUILayout.Space(aSpaceBetween);
			
			if(itsTimeLeft < 0.0f)
			{
				itsCurrentFPS = itsAccumulatedFrames / itsFramesInInterval;
				itsTimeLeft = itsDataModuleGUILogger.itsFPSUpdateInterval;
				itsAccumulatedFrames = 0.0f;
				itsFramesInInterval = 0;
			}
			
			KGFGUIUtility.Label(System.String.Format("FPS: {0:F2}", itsCurrentFPS), KGFGUIUtility.eStyleLabel.eLabel);
		}
		KGFGUIUtility.EndHorizontalBox();
	}
	
	/// <summary>
	/// draws the category column on the left side of the window (including scrollbar and search filter)
	/// </summary>
	private void DrawContentColumn()
	{
		#region category filterbuttons
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxDarkTop);
		{
			KGFGUIUtility.BeginVerticalPadding();
			{
				DrawFilterButtons();
			}
			KGFGUIUtility.EndVerticalPadding();
		}
		KGFGUIUtility.EndHorizontalBox();
		
		#endregion
		
		#region content table
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxMiddleVertical);
		{
			itsTableControl.SetStartRow((uint)(itsCurrentPage * (int)itsDataModuleGUILogger.itsLogsPerPage));
			itsTableControl.SetDisplayRowCount((uint)itsDataModuleGUILogger.itsLogsPerPage);
			
			KGFGUIUtility.BeginVerticalPadding();
			{
				itsTableControl.Render();
			}
			KGFGUIUtility.EndVerticalPadding();
		}
		KGFGUIUtility.EndHorizontalBox();
		
		#endregion
		
		#region search bar
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxDarkBottom);
		{
			DrawContentFilterBar();
		}
		KGFGUIUtility.EndHorizontalBox();
		
		#endregion
	}
	
	/// <summary>
	/// draws the category column on the left side of the window (including scrollbar and search filter)
	/// </summary>
	private void DrawCategoryColumn()
	{
		// determines if a value has changed and the list must be refreshed
		bool aChange = false;
		
		#region top area
		
		// top area with all and none button
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxDarkTop);
		{
			KGFGUIUtility.BeginVerticalPadding();
			{
				if (KGFGUIUtility.Button("All", KGFGUIUtility.eStyleButton.eButtonLeft, GUILayout.ExpandWidth(true)))
				{
					foreach(KGFDebugCategory aCategory in GetAllCategories())
					{
						aCategory.itsSelectedState = true;
						aChange = true;
					}
				}
				
				if (KGFGUIUtility.Button("None", KGFGUIUtility.eStyleButton.eButtonRight, GUILayout.ExpandWidth(true)))
				{
					foreach(KGFDebugCategory aCategory in GetAllCategories())
					{
						aCategory.itsSelectedState = false;
						aChange = true;
					}
				}
			}
			KGFGUIUtility.EndVerticalPadding();
		}
		KGFGUIUtility.EndHorizontalBox();
		
		#endregion
		
		#region category list area
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxMiddleVertical);
		{
			itsCategoryScrollViewPosition = KGFGUIUtility.BeginScrollView(itsCategoryScrollViewPosition, false, true, GUILayout.ExpandWidth(true));
			{
				// counts the visible categories
				int aCounter = 0;
				
				foreach(KGFDebugCategory aCategory in GetAllCategories())
				{
					// check if the category fits the current filter
					if(itsSearchFilterCategory.Trim().Equals("Search") || aCategory.GetName().ToLower().Contains(itsSearchFilterCategory.ToLower()))
					{
						aCounter++;
						bool aNewValue	= KGFGUIUtility.Toggle(aCategory.itsSelectedState, string.Format("{0} ({1})", aCategory.GetName(), aCategory.GetCount().ToString()), KGFGUIUtility.eStyleToggl.eTogglSuperCompact);
						
						if(aCategory.itsSelectedState != aNewValue)
						{
							aCategory.itsSelectedState = aNewValue;
							aChange = true;
						}
					}
				}
				
				// display "no items found" if no category matches the current filter
				if(aCounter == 0)
				{
					KGFGUIUtility.Label("no items found");
				}
			}
			GUILayout.EndScrollView();
		}
		KGFGUIUtility.EndHorizontalBox();
		
		#endregion
		
		#region Search Box
		
		// determines if the string value has changed
		string aOldValue = itsSearchFilterCategory;
		
		KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxDarkBottom);
		{
			GUI.SetNextControlName("categorySearch");
			itsSearchFilterCategory = KGFGUIUtility.TextField(itsSearchFilterCategory,KGFGUIUtility.eStyleTextField.eTextField, GUILayout.ExpandWidth(true));
		}
		KGFGUIUtility.EndHorizontalBox();
		
		// reset the text to empty if the control is clicked and the current Text is Search
		if(GUI.GetNameOfFocusedControl().Equals("categorySearch"))
		{
			itsFocus = true;
			
			if(itsSearchFilterCategory.Equals("Search"))
			{
				itsSearchFilterCategory = string.Empty;
			}
		}
		
		// reset the text to Search if the current text is empty
		if(!GUI.GetNameOfFocusedControl().Equals("categorySearch"))
		{
			if(!GUI.GetNameOfFocusedControl().Equals("messageSearch"))
			{
				itsFocus = false;
			}
			
			if(itsSearchFilterCategory.Equals(string.Empty))
			{
				itsSearchFilterCategory = "Search";
			}
		}
		
		// check if there were any changes
		if(!aOldValue.Equals(itsSearchFilterCategory))
		{
			aChange = true;
		}
		
		// update the displayed result list if there were any changes
		if(aChange)
		{
			UpdateLogList();
		}
		
		#endregion
	}
	
	/// <summary>
	/// draws the log level filter buttons on top of the window
	/// </summary>
	private void DrawFilterButtons()
	{
		if(KGFGUIUtility.Button("All", KGFGUIUtility.eStyleButton.eButtonLeft, GUILayout.Width(70)))
		{
			foreach(KGFeDebugLevel aLevel in Enum.GetValues(typeof(KGFeDebugLevel)))
			{
				itsLogLevelFilter[aLevel] = true;
				UpdateLogList();
			}
			
			SaveCategoryFilterToPlayerPrefs();
		}
		
		if(KGFGUIUtility.Button("None", KGFGUIUtility.eStyleButton.eButtonRight, GUILayout.Width(70)))
		{
			foreach(KGFeDebugLevel aLevel in Enum.GetValues(typeof(KGFeDebugLevel)))
			{
				itsLogLevelFilter[aLevel] = false;
				UpdateLogList();
			}
			
			SaveCategoryFilterToPlayerPrefs();
		}
		
		//Filter Buttons
		foreach(KGFeDebugLevel aLevel in Enum.GetValues(typeof(KGFeDebugLevel)))
		{
			if(aLevel != KGFeDebugLevel.eOff && aLevel != KGFeDebugLevel.eAll)
			{
				bool aNewValue = KGFGUIUtility.Toggle(itsLogLevelFilter[aLevel], aLevel.ToString().Substring(1, aLevel.ToString().Length - 1), KGFGUIUtility.eStyleToggl.eTogglSuperCompact);
				GUILayout.Space(10.0f);
				
				if (aNewValue != itsLogLevelFilter[aLevel])
				{
					itsLogLevelFilter[aLevel] = aNewValue;
					UpdateLogList();
					
					SaveCategoryFilterToPlayerPrefs();
				}
			}
		}
		GUILayout.FlexibleSpace();
	}
	
	/// <summary>
	/// draws the search box for log message search
	/// </summary>
	private void DrawContentFilterBar()
	{
		//Message Filter
		string aOldValue = itsSearchFilterMessage;
		bool aEnabledChanged = false;
		
		GUI.SetNextControlName("messageSearch");
		itsSearchFilterMessage = KGFGUIUtility.TextField(itsSearchFilterMessage,KGFGUIUtility.eStyleTextField.eTextField, GUILayout.Width(Screen.width * 0.2f));
		//GUILayout.Space(50.0f);
		GUILayout.FlexibleSpace();
		
		if(GUI.GetNameOfFocusedControl().Equals("messageSearch"))
		{
			itsFocus = true;
			
			if(itsSearchFilterMessage.Equals("Search"))
			{
				itsSearchFilterMessage = string.Empty;
				aEnabledChanged = true;
			}
		}
		
		if(!GUI.GetNameOfFocusedControl().Equals("messageSearch"))
		{
			if(!GUI.GetNameOfFocusedControl().Equals("categorySearch"))
			{
				itsFocus = false;
			}
			
			if(itsSearchFilterMessage.Equals(string.Empty))
			{
				itsSearchFilterMessage = "Search";
				aEnabledChanged = true;
			}
		}
		
		if (aOldValue != itsSearchFilterMessage && !aEnabledChanged)
		{
			itsLiveSearchChanged = true;
			itsLastChangeTime = Time.time;
		}

		//only update if the OnGUI is in Layout mode
		if(itsLiveSearchChanged && (Time.time - itsLastChangeTime) > 1 && Event.current.type != EventType.Layout)
		{
			itsLiveSearchChanged = false;
			UpdateLogList();
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		{
			KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxInvisible);
			{
				//number of items in List
				if(KGFGUIUtility.Button(itsDataModuleGUILogger.itsIconLeft,KGFGUIUtility.eStyleButton.eButtonLeft))
				{
					switch(itsDataModuleGUILogger.itsLogsPerPage)
					{
						case KGFeItemsPerPage.e25:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e10;
							break;
						case KGFeItemsPerPage.e50:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e25;
							break;
						case KGFeItemsPerPage.e100:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e50;
							break;
						case KGFeItemsPerPage.e250:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e100;
							break;
						case KGFeItemsPerPage.e500:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e250;
							break;
							
						default:
							break;
					}
					
					PlayerPrefs.SetInt("KGF.KGFModuleDebugger.itsLogsPerPage", (int)itsDataModuleGUILogger.itsLogsPerPage);
					PlayerPrefs.Save();
					UpdateLogListPageNumber();
				}
				
				KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal);
				{
					string aLogsPerPageString = itsDataModuleGUILogger.itsLogsPerPage.ToString().Substring(1) + " entries per page";
					KGFGUIUtility.Label(aLogsPerPageString,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox);
				}
				KGFGUIUtility.EndVerticalBox();
				
				if(KGFGUIUtility.Button(itsDataModuleGUILogger.itsIconRight,KGFGUIUtility.eStyleButton.eButtonRight))
				{
					switch(itsDataModuleGUILogger.itsLogsPerPage)
					{
						case KGFeItemsPerPage.e10:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e25;
							break;
						case KGFeItemsPerPage.e25:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e50;
							break;
						case KGFeItemsPerPage.e50:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e100;
							break;
						case KGFeItemsPerPage.e100:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e250;
							break;
						case KGFeItemsPerPage.e250:
							itsDataModuleGUILogger.itsLogsPerPage = KGFeItemsPerPage.e500;
							break;
							
						default:
							break;
					}
					
					PlayerPrefs.SetInt("KGF.KGFModuleDebugger.itsLogsPerPage", (int)itsDataModuleGUILogger.itsLogsPerPage);
					PlayerPrefs.Save();

					UpdateLogListPageNumber();
				}

				GUILayout.Space(10.0f);
				
				if(KGFGUIUtility.Button(itsDataModuleGUILogger.itsIconLeft,KGFGUIUtility.eStyleButton.eButtonLeft) && itsCurrentPage > 0)
				{
					itsCurrentPage--;
				}
				
				KGFGUIUtility.BeginVerticalBox(KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal);
				{
					int itsNumberOfPages = (int)Math.Ceiling((float)itsLogTable.Rows.Count / (float)itsDataModuleGUILogger.itsLogsPerPage);
					string aString = string.Format("page {0}/{1}", itsCurrentPage + 1, Math.Max(itsNumberOfPages, 1));
					KGFGUIUtility.Label(aString,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox);
				}
				KGFGUIUtility.EndVerticalBox();
				
				if(KGFGUIUtility.Button(itsDataModuleGUILogger.itsIconRight,KGFGUIUtility.eStyleButton.eButtonRight) && itsLogTable.Rows.Count > ((itsCurrentPage + 1) * (int)itsDataModuleGUILogger.itsLogsPerPage))
				{
					itsCurrentPage++;
				}
				
				if(KGFGUIUtility.Button("clear", KGFGUIUtility.eStyleButton.eButton))
				{
					ClearCurrentLogs();
				}
			}
			KGFGUIUtility.EndHorizontalBox();
		}
		GUILayout.EndVertical();
	}
	
	private void ClearCurrentLogs()
	{
		List<KGFDebug.KGFDebugLog> aList = new List<KGFDebug.KGFDebugLog>();
		
		foreach(KGFDebug.KGFDebugLog aLog in GetFilteredLogList())
		{
			aList.Add(aLog);
			
			if(itsLogCategories.ContainsKey(aLog.GetCategory()))
			{
				itsLogCategories[aLog.GetCategory()].DecreaseCount();
			}
			
			if(itsLogCategoryCount.ContainsKey(aLog.GetLevel()))
			{
				itsLogCategoryCount[aLog.GetLevel()]--;
			}
		}
		
		foreach(KGFDebug.KGFDebugLog aLog in aList)
		{
			itsLogList.Remove(aLog);
		}
		
		UpdateLogList();
	}
	
	/// <summary>
	/// this event is triggered after a row is rendered completely
	/// </summary>
	/// <param name="theSender">the DataRow that created this event</param>
	private void PostLogTableRowHook(object theSender, EventArgs theArguments)
	{
		KGFDataRow aRow = theSender as KGFDataRow;
		
		// save the default background
		Color aBackgroundColor = GUI.backgroundColor;
		
		// check if the row contains a level column and change the background
		if(aRow != null && !aRow.IsNull("Lvl"))
		{
			if (Enum.IsDefined(typeof(KGFeDebugLevel), aRow["Lvl"].ToString()))
			{
				GUI.backgroundColor = GetColorForLevel((KGFeDebugLevel)Enum.Parse(typeof(KGFeDebugLevel), aRow["Lvl"].ToString()));
			}
			else
			{
				UnityEngine.Debug.Log("the color is not defined");
			}
		}
		
		if(aRow != null)
		{
			GUI.contentColor = Color.white;
			
			// if the row is the current selected row
			if(aRow == itsCurrentSelectedRow)
			{
				KGFGUIUtility.BeginHorizontalBox(KGFGUIUtility.eStyleBox.eBoxDarkBottom, GUILayout.ExpandWidth(true));
				{
					GUILayout.BeginVertical();
					{
						GUILayout.TextArea(string.Format("Object Path: {1}{0}{0}Time: {2}{0}{0}Category: {3}{0}{0}Message: {4}{0}{0}Stack Trace: {5}",
						                                 Environment.NewLine, GetObjectPath(aRow[5].Value as MonoBehaviour), aRow[1].Value, aRow[2].Value, aRow[3].Value, aRow[4].Value),
						                   GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
						
						if(KGFGUIUtility.Button("copy to file", KGFGUIUtility.eStyleButton.eButton, GUILayout.ExpandWidth(true)))
						{
							// creatre a temp file
							string aTempFile = CreateTempFile(new Dictionary<string,string>(){ {"Message", aRow[3].Value.ToString()}, {"Time", aRow[1].Value.ToString()}, {"StackTrace", aRow[4].Value.ToString()} });
							// open the temp file with the default text editor
							OpenFile(aTempFile);
						}
					}
					GUILayout.EndVertical();
				}
				KGFGUIUtility.EndHorizontalBox();
				
				GUILayout.Space(5.0f);
			}
		}
		
		// reset background to default
		GUI.backgroundColor = aBackgroundColor;
	}
	
	/// <summary>
	/// this event is triggered before a cell content is rendered
	/// </summary>
	/// <param name="theRow">the DataRow that created this event</param>
	/// <param name="theColumn">the DataColumn that created this event</param>
	/// <returns>true if the default draw operation should be disabled, returns false is the default draw operation should be continued</returns>
	private bool PreCellContentHook(KGFDataRow theRow, KGFDataColumn theColumn, uint theWidth)
	{
		if(theColumn.ColumnName.Equals("Lvl"))
		{
			switch(theRow[theColumn.ColumnName].ToString())
			{
				case "eDebug":
					KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconDebug,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox,GUILayout.Width(theWidth));
//					GUILayout.BeginHorizontal(GUILayout.Width(itsTableControl.GetColumnWidth(0)));
//					{
//						GUILayout.FlexibleSpace();
//						KGFGUIUtility.Image(itsDataModuleGUILogger.itsIconDebug,KGFGUIUtility.eStyleImage.eImageFreeSize,GUILayout.Width(KGFGUIUtility.GetSkinHeight()),GUILayout.Height(KGFGUIUtility.GetSkinHeight()));
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndHorizontal();
					return true;
				case "eInfo":
					KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconInfo,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox,GUILayout.Width(theWidth));
//					GUILayout.BeginHorizontal(GUILayout.Width(itsTableControl.GetColumnWidth(0)));
//					{
//						GUILayout.FlexibleSpace();
//						KGFGUIUtility.Image(itsDataModuleGUILogger.itsIconInfo,KGFGUIUtility.eStyleImage.eImage,GUILayout.Width(KGFGUIUtility.GetSkinHeight()),GUILayout.Height(KGFGUIUtility.GetSkinHeight()));
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndHorizontal();
					return true;
				case "eWarning":
					KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconWarning,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox,GUILayout.Width(theWidth));
//					GUILayout.BeginHorizontal(GUILayout.Width(itsTableControl.GetColumnWidth(0)));
//					{
//						GUILayout.FlexibleSpace();
//						KGFGUIUtility.Image(itsDataModuleGUILogger.itsIconWarning,KGFGUIUtility.eStyleImage.eImage,GUILayout.Width(KGFGUIUtility.GetSkinHeight()),GUILayout.Height(KGFGUIUtility.GetSkinHeight()));
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndHorizontal();
					return true;
				case "eError":
					KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconError,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox,GUILayout.Width(theWidth));
//					GUILayout.BeginHorizontal(GUILayout.Width(itsTableControl.GetColumnWidth(0)));
//					{
//						GUILayout.FlexibleSpace();
//						KGFGUIUtility.Image(itsDataModuleGUILogger.itsIconError,KGFGUIUtility.eStyleImage.eImage,GUILayout.Width(KGFGUIUtility.GetSkinHeight()),GUILayout.Height(KGFGUIUtility.GetSkinHeight()));
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndHorizontal();
					//KGFGUIUtility.Label(string.Empty, itsDataModuleGUILogger.itsIconError, KGFGUIUtility.eStyleLabel.eLabelFitIntoBox);
					return true;
				case "eFatal":
					KGFGUIUtility.Label("",itsDataModuleGUILogger.itsIconFatal,KGFGUIUtility.eStyleLabel.eLabelFitIntoBox,GUILayout.Width(theWidth));
//					GUILayout.BeginHorizontal(GUILayout.Width(itsTableControl.GetColumnWidth(0)));
//					{
//						GUILayout.FlexibleSpace();
//						KGFGUIUtility.Image(itsDataModuleGUILogger.itsIconFatal,KGFGUIUtility.eStyleImage.eImage,GUILayout.Width(KGFGUIUtility.GetSkinHeight()),GUILayout.Height(KGFGUIUtility.GetSkinHeight()));
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndHorizontal();
					return true;
				default:
					return false;
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// this event is triggered when a new class that implements the KGFICustomGUI Interface is added
	/// </summary>
	private void OnDebugGuiAdd(object theSender)
	{
		KGFICustomGUI aGui = (KGFICustomGUI)theSender;
		if (!itsLogCategories.ContainsKey(aGui.GetName()))
		{
			itsLogCategories[aGui.GetName()] = new KGFDebugCategory(aGui.GetName());
		}
	}
	
	/// <summary>
	/// this event is triggered when a mouse click is received on the table row
	/// </summary>
	/// <param name="theSender">the DataRow that created this event</param>
	private void OnLogTableRowIsClicked(object theSender, EventArgs theArguments)
	{
		KGFDataRow aRow = theSender as KGFDataRow;
		
		if(aRow != null)
		{
			if(aRow != itsCurrentSelectedRow)
			{
				itsCurrentSelectedRow = aRow;
			}
			else
			{
				itsCurrentSelectedRow = null;
			}
		}
	}
	
	#endregion
	
	#region Playerprefs
	
	/// <summary>
	/// saves the current category filter to players prefs
	/// </summary>
	private void SaveCategoryFilterToPlayerPrefs()
	{
		StringBuilder aString = new StringBuilder();
		
		foreach(KGFeDebugLevel aLevel in Enum.GetValues(typeof(KGFeDebugLevel)))
		{
			aString.Append(string.Format("{0}.{1}:", aLevel.ToString(), itsLogLevelFilter[aLevel].ToString()));
		}
		
		aString.Remove(aString.Length - 1, 1);

		PlayerPrefs.SetString("KGF.KGFModuleDebug", aString.ToString());
		PlayerPrefs.Save();
	}
	
	/// <summary>
	/// loads the category filter from players prefs
	/// </summary>
	private void LoadCategoryFilterFromPlayerPrefs()
	{
		string aString = PlayerPrefs.GetString("KGF.KGFModuleDebug");
		
		string[] aFilterPairs = aString.Split(':');
		
		foreach(string aFilterPair in aFilterPairs)
		{
			string[] aComponent = aFilterPair.Split('.');
			
			if(aComponent.Length != 2)
			{
				continue;
			}
			else
			{
				if(Enum.IsDefined(typeof(KGFeDebugLevel), aComponent[0]))
				{
					bool aValue;
					
					if(bool.TryParse(aComponent[1], out aValue))
					{
						itsLogLevelFilter[(KGFeDebugLevel)Enum.Parse(typeof(KGFeDebugLevel), aComponent[0])] = aValue;
					}
				}
				else
				{
					continue;
				}
			}
		}
	}
	
	#endregion
	
	/// <summary>
	/// returns the current selected Color for the debug level
	/// </summary>
	/// <param name="theLevel">the KGFDebugLevel to request the color</param>
	/// <returns>returns the associated color to the log level</returns>
	public Color GetColorForLevel(KGFeDebugLevel theLevel)
	{
		switch(theLevel)
		{
			case KGFeDebugLevel.eDebug:
				return itsDataModuleGUILogger.itsColorDebug;
			case KGFeDebugLevel.eInfo:
				return itsDataModuleGUILogger.itsColorInfo;
			case KGFeDebugLevel.eWarning:
				return itsDataModuleGUILogger.itsColorWarning;
			case KGFeDebugLevel.eError:
				return itsDataModuleGUILogger.itsColorError;
			case KGFeDebugLevel.eFatal:
				return itsDataModuleGUILogger.itsColorFatal;
			default:
				return Color.white;
		}
	}
	
	/// <summary>
	/// Create a temp file and fill it with ordered information for debug
	/// </summary>
	/// <param name="theContent"></param>
	/// <returns></returns>
	private string CreateTempFile(Dictionary<string,string> theContent)
	{
		#if UNITY_WEBPLAYER
		return null;
		#else
		string aNewTempFile = Path.GetTempFileName();
		UnityEngine.Debug.Log("temp file path: " + aNewTempFile);
		string aNewTempFileTxt = Path.ChangeExtension(aNewTempFile,"txt");
		
		File.Move(aNewTempFile, aNewTempFileTxt);
		
		using (StreamWriter aStream = new StreamWriter(aNewTempFileTxt, true, Encoding.ASCII))
		{
			foreach (string aKey in theContent.Keys)
			{
				if (theContent[aKey] == null)
				{
					continue;
				}
				aStream.WriteLine(aKey);
				aStream.WriteLine("".PadLeft(aKey.Length,'='));
				
				foreach (string aLine in theContent[aKey].Split(Environment.NewLine.ToCharArray()))
				{
					aStream.WriteLine(aLine);
				}
				
				aStream.WriteLine();
			}
		}
		return aNewTempFileTxt;
		#endif
	}
	
	/// <summary>
	/// Tell the operating system to open a file with the registered application
	/// </summary>
	/// <param name="theFilePath"></param>
	private void OpenFile(string theFilePath)
	{
		#if UNITY_WEBPLAYER
		return;
		#else
		if(File.Exists(theFilePath))
		{
			Process aProcess = new Process();
			aProcess.StartInfo.FileName = theFilePath;
			aProcess.Start();
		}
		else
		{
			UnityEngine.Debug.LogWarning("the file path was not found: " + theFilePath);
		}
		#endif
	}
	
	/// <summary>
	/// tries to determine the object path
	/// </summary>
	/// <param name="theObject">the object wich path should be determined</param>
	/// <returns>the path of the object. if no path found string.Empty will be returned</returns>
	private static string GetObjectPath(MonoBehaviour theObject)
	{
		if(theObject != null)
		{
			List<string> aStringList = new List<string>();
			Transform aTransform = theObject.transform;
			
			do
			{
				aStringList.Add(aTransform.name);
				aTransform = aTransform.parent;
			}while (aTransform != null);
			
			aStringList.Reverse();
			return string.Join("/",aStringList.ToArray());
		}
		else
		{
			return string.Empty;
		}
	}
	
	/// <summary>
	/// look into KGFIDebug documentation for further information
	/// </summary>
	public override string GetName()
	{
		return "KGFDebugGUI";
	}
	
	public override string GetDocumentationPath()
	{
		return "KGFDebugGUI_Manual.html";
	}
	
	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}
	
	public override Texture2D GetIcon()
	{
		return null;
	}
	
	#region KGFIValidator
	
	public override KGFMessageList Validate()
	{
		KGFMessageList aMessageList = new KGFMessageList();
		
		if(itsDataModuleGUILogger.itsIconDebug == null)
		{
			aMessageList.AddWarning("the debug icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconInfo == null)
		{
			aMessageList.AddWarning("the info icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconWarning == null)
		{
			aMessageList.AddWarning("the warning icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconError == null)
		{
			aMessageList.AddWarning("the error icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconFatal == null)
		{
			aMessageList.AddWarning("the fatal error icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconHelp == null)
		{
			aMessageList.AddWarning("the help icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconLeft == null)
		{
			aMessageList.AddWarning("the left arrow icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsIconRight == null)
		{
			aMessageList.AddWarning("the right arrow icon is missing");
		}
		
		if(itsDataModuleGUILogger.itsFPSUpdateInterval < 0)
		{
			aMessageList.AddError("the FPS update intervalöl must be greater than zero");
		}
		
		return aMessageList;
	}
	
	#endregion
}
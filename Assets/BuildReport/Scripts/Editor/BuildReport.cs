//#define BUILD_REPORT_TOOL_EXPERIMENTS

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

/*

Editor
Editor log can be brought up through the Open Editor Log button in Unity's Console window.

Mac OS X	~/Library/Logs/Unity/Editor.log (or /Users/username/Library/Logs/Unity/Editor.log)
Windows XP *	C:\Documents and Settings\username\Local Settings\Application Data\Unity\Editor\Editor.log
Windows Vista/7 *	C:\Users\username\AppData\Local\Unity\Editor\Editor.log

(*) On Windows the Editor log file is stored in the local application data folder: %LOCALAPPDATA%\Unity\Editor\Editor.log, where LOCALAPPDATA is defined by CSIDL_LOCAL_APPDATA.





need to parse contents of editor log.
this part is what we're interested in:

[quote]
Textures      196.4 kb	 3.4%
Meshes        0.0 kb	 0.0%
Animations    0.0 kb	 0.0%
Sounds        0.0 kb	 0.0%
Shaders       0.0 kb	 0.0%
Other Assets  37.4 kb	 0.6%
Levels        8.5 kb	 0.1%
Scripts       228.4 kb	 3.9%
Included DLLs 5.2 mb	 91.7%
File headers  12.5 kb	 0.2%
Complete size 5.7 mb	 100.0%

Used Assets, sorted by uncompressed size:
 39.1 kb	 0.7% Assets/BTX/GUI/Skin/Window.png
 21.0 kb	 0.4% Assets/BTX/GUI/BehaviourTree/Resources/BehaviourTreeGuiSkin.guiskin
 20.3 kb	 0.3% Assets/BTX/Fonts/DejaVuSans-SmallSize.ttf
 20.2 kb	 0.3% Assets/BTX/Fonts/DejaVuSans-Bold.ttf
 20.1 kb	 0.3% Assets/BTX/Fonts/DejaVuSansCondensed 1.ttf
 12.0 kb	 0.2% Assets/BTX/Fonts/DejaVuSansCondensed.ttf
 10.8 kb	 0.2% Assets/BTX/GUI/BehaviourTree/Nodes2/White.png
 8.1 kb	 0.1% Assets/BTX/GUI/BehaviourTree/Nodes/RoundedBox.png
 8.1 kb	 0.1% Assets/BTX/GUI/BehaviourTree/Nodes/Decorator.png
 4.9 kb	 0.1% Assets/BTX/GUI/Skin/Box.png
 4.6 kb	 0.1% Assets/BTX/GUI/BehaviourTree/GlovedHand.png
 4.5 kb	 0.1% Assets/BTX/GUI/Skin/TextField_Normal.png
 4.5 kb	 0.1% Assets/BTX/GUI/Skin/Button_Toggled.png
 4.5 kb	 0.1% Assets/BTX/GUI/Skin/Button_Normal.png
 4.5 kb	 0.1% Assets/BTX/GUI/Skin/Button_Active.png
 4.1 kb	 0.1% Assets/BTX/GUI/BehaviourTree/RunState/Visiting.png
 4.1 kb	 0.1% Assets/BTX/GUI/BehaviourTree/RunState/Success.png
 4.1 kb	 0.1% Assets/BTX/GUI/BehaviourTree/RunState/Running.png
 (etc. goes on and on until all files used are listed)
[/quote]


This part can also be helpful:

[quote]
Mono dependencies included in the build
Boo.Lang.dll
Mono.Security.dll
System.Core.dll
System.Xml.dll
System.dll
UnityScript.Lang.dll
mscorlib.dll
Assembly-CSharp.dll
Assembly-UnityScript.dll

[/quote]


so we're gonna flex our string parsing skills here.

just get this string since it seems to be constant enough:
"Used Assets, sorted by uncompressed size:"

then starting from that line going upwards, get the line that begins with "Textures"

we're relying on the assumption that this format won't get changed

in short, this is all complete hackery that won't be futureproof

hopefully UT would provide proper script access to this

*/


[System.Serializable]
public partial class BuildReport
{

#if BUILD_REPORT_TOOL_EXPERIMENTS

	[MenuItem("Window/Test1")]
	public static void Test1()
	{
		Debug.Log("EditorApplication.applicationContentsPath: " + EditorApplication.applicationContentsPath);
		Debug.Log("EditorApplication.applicationPath: " + EditorApplication.applicationPath);
	}

#endif



	[SerializeField]
	static BuildInfo _lastKnownBuildInfo = null;

	[SerializeField]
	static string _lastEditorLogPath = "";

	// given values only upon building
	static Dictionary<string, bool> _prefabsUsedInScenes = new Dictionary<string, bool>();



	public static BuildInfo CreateNewBuildInfo()
	{
		return new BuildInfo();
		//return ScriptableObject.CreateInstance<BuildInfo>();
	}


	// have to be called from the main thread
	public static void Init()
	{
		Init(ref _lastKnownBuildInfo);
	}

	public const string TIME_OF_BUILD_FORMAT = "yyyy MMM dd ddd h:mm:ss tt UTCz";

	public static void Init(ref BuildInfo buildInfo)
	{
		if (buildInfo == null)
		{
			buildInfo = CreateNewBuildInfo();
		}

		buildInfo.TimeGot = DateTime.Now;
		buildInfo.TimeGotReadable = buildInfo.TimeGot.ToString(TIME_OF_BUILD_FORMAT);

		buildInfo.EditorAppContentsPath = EditorApplication.applicationContentsPath;
		buildInfo.ProjectAssetsPath = Application.dataPath;
		buildInfo.BuildFilePath = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);

		buildInfo.ScenesIncludedInProject = BuildReportUtil.GetAllScenesUsedInProject();


		buildInfo.IncludedSvnInUnused = BuildReportOptions.IncludeSvnInUnused;
		buildInfo.IncludedGitInUnused = BuildReportOptions.IncludeGitInUnused;

		buildInfo.CodeStrippingLevel = PlayerSettings.strippingLevel;
		buildInfo.MonoLevel = PlayerSettings.apiCompatibilityLevel;

		//Debug.Log("getting _lastEditorLogPath");
		_lastEditorLogPath = BuildReportUtil.UsedEditorLogPath;
		_lastSavePath = BuildReportOptions.BuildReportSavePath;
	}



	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		//Debug.Log("post process build called");
		if (BuildReportOptions.CollectBuildInfo == false)
		{
			return;
		}
		Init();
		CommitAdditionalInfoToCache(_lastKnownBuildInfo);
		GetBuildValuesSeparateThread();

		bool buildReportWindowInFocus = BuildReportWindow.IsOpen;//EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType() == typeof(BuildReportWindow);

		if (buildReportWindowInFocus || BuildReportOptions.ShowAfterBuild)
		{
			ShowBuildReportWithLastValues();
		}
		//Debug.Log("post process build finished");
	}

	[SerializeField]
	static bool _loadingValuesFromThread = false;

	public static bool LoadingValuesFromThread { get{ return _loadingValuesFromThread; } }

	[SerializeField]
	static string _lastSavePath = "";

	public static void GetBuildValuesSeparateThread()
	{
		Thread workerThread = new Thread(BuildReport.GetBuildInfoDelayed);
		workerThread.Start();
	}

	public static void GetBuildInfoDelayed()
	{
		_loadingValuesFromThread = true;
		//Debug.Log("Waiting 500 ms...");
		do
		{
			Thread.Sleep(500);
		} while (!BuildReportUtil.FileHasContents(_lastEditorLogPath, "Used Assets, sorted by uncompressed size:"));

		//Debug.Log("Populating Build Info Values...");
		PopulateLastBuildValues();
		_loadingValuesFromThread = false;

		//Debug.Log("About to save...");
		BuildReportUtil.SerializeBuildInfoAtFolder(_lastKnownBuildInfo, _lastSavePath);
	}



	[PostProcessScene]
	public static void OnPostprocessScene()
	{
		// get used prefabs on each scene
		//

		//Debug.Log("post process scene called");
		//Debug.Log(" at " + EditorApplication.currentScene);
		AddAllPrefabsUsedInCurrentSceneToList();

		//Debug.Log("post process scene finished");
	}

	static void AddAllPrefabsUsedInCurrentSceneToList()
	{
		GameObject[] allObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		foreach(GameObject GO in allObjects)
		{
			if (PrefabUtility.GetPrefabType(GO) == PrefabType.PrefabInstance)
			{
				UnityEngine.Object GO_prefab = PrefabUtility.GetPrefabParent(GO);

				string prefabPath = AssetDatabase.GetAssetPath(GO_prefab);
				//Debug.Log("   prefab: " + o.name + " path: " + AssetDatabase.GetAssetPath(o));
				if (!_prefabsUsedInScenes.ContainsKey(prefabPath))
				{
					_prefabsUsedInScenes.Add(prefabPath, false);
				}
			}
		}
	}

	public static void RefreshListOfAllPrefabsUsedInAllScenes()
	{
		_prefabsUsedInScenes.Clear();

		foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				//Debug.Log("scene: " + name);
				//temp.Add(name);
				UnityEngine.Object sceneAsset = AssetDatabase.LoadMainAssetAtPath(S.path);
				UnityEngine.Object[] deps = EditorUtility.CollectDependencies(new UnityEngine.Object[]{sceneAsset});
				foreach (UnityEngine.Object o in deps)
				{
					if (o != null && PrefabUtility.GetPrefabType(o) == PrefabType.Prefab)
					{
						string prefabPath = AssetDatabase.GetAssetPath(o);
						//Debug.Log("   prefab: " + o.name + " path: " + AssetDatabase.GetAssetPath(o));
						if (!_prefabsUsedInScenes.ContainsKey(prefabPath))
						{
							//Debug.Log("   prefab used: " + o.name + " path: " + prefabPath);
							_prefabsUsedInScenes.Add(prefabPath, false);
						}
					}
				}
			}
		}
	}

	static void CommitAdditionalInfoToCache(BuildInfo buildInfo)
	{
		if (_prefabsUsedInScenes != null)
		{
			//Debug.Log("addInfo: " + (addInfo != null));

			buildInfo.PrefabsUsedInScenes = new string[_prefabsUsedInScenes.Keys.Count];
			_prefabsUsedInScenes.Keys.CopyTo(buildInfo.PrefabsUsedInScenes, 0);
			//Debug.Log("assigned to addInfo.PrefabsUsedInScenes: " + addInfo.PrefabsUsedInScenes.Length);
		}
	}


	static string GetBuildTypeFromEditorLog(string editorLog)
	{
		const string buildTypeKey = "*** Completed 'Build.Player.";

		int buildTypeIdx = editorLog.LastIndexOf(buildTypeKey);
		//Debug.Log("buildTypeIdx: " + buildTypeIdx);

		if (buildTypeIdx == -1)
		{
			return "";
		}

		int buildTypeEndIdx = editorLog.IndexOf("' in ", buildTypeIdx);
		//Debug.Log("buildTypeEndIdx: " + buildTypeEndIdx);

		string buildType = editorLog.Substring(buildTypeIdx+buildTypeKey.Length, buildTypeEndIdx-buildTypeIdx-buildTypeKey.Length);
		//Debug.Log("buildType got: " + buildType);
		return buildType;
	}



	static BuildSizePart[] ParseBuildSizePartsFromString(string inText)
	{
		// now parse the build parts to an array of `BuildSizePart`
		List<BuildSizePart> buildSizes = new List<BuildSizePart>();

		string[] buildPartsSplitted = inText.Split(new Char[] {'\n', '\r'});
		foreach (string b in buildPartsSplitted)
		{
			if (!string.IsNullOrEmpty(b))
			{
				//Debug.Log("got: " + b);

				string gotName = "???";
				string gotSize = "?";
				string gotPercent = "?";

				Match match = Regex.Match(b, @"^[a-z \t]+[^0-9]", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotName = match.Groups[0].Value;
					gotName = gotName.Trim();
					if (gotName == "Scripts") gotName = "Script DLLs";
					//Debug.Log("    name? " + gotName);
				}

				match = Regex.Match(b, @"[0-9.]+ (kb|mb|b|gb)", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotSize = match.Groups[0].Value.ToUpper();
					//Debug.Log("    size? " + gotSize);
				}

				match = Regex.Match(b, @"[0-9.]+%", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotPercent = match.Groups[0].Value;
					gotPercent = gotPercent.Substring(0, gotPercent.Length-1);
					//Debug.Log("    percent? " + gotPercent);
				}

				BuildSizePart inPart = new BuildSizePart();
				inPart.Name = gotName;
				inPart.Size = gotSize;
				inPart.Percentage = Double.Parse(gotPercent);
				inPart.DerivedSize = BuildReportUtil.GetApproxSizeFromString(gotSize);

				buildSizes.Add(inPart);
			}
		}

		return buildSizes.ToArray();
	}

	static BuildSizePart[] ParseAssetSizesFromEditorLog(string editorLog, int offset, string[] prefabsUsedInScenes)
	{
		List<BuildSizePart> assetSizes = new List<BuildSizePart>();
		Dictionary<string, bool> prefabsInBuildDict = new Dictionary<string, bool>();

		int assetListStaIdx = editorLog.IndexOf("\n", offset);
		//Debug.Log("assetListStaIdx: " + assetListStaIdx);

		//Debug.Log(editorLog.Substring(assetListStaIdx, 500));

		int currentIdx = assetListStaIdx+1;
		while (true)
		{
			int lineEndIdx = editorLog.IndexOf("\n", currentIdx);
			string line = editorLog.Substring(currentIdx, lineEndIdx-currentIdx);
			//Debug.Log("line: " + line);

			Match match = Regex.Match(line, @"^ [0-9].*[a-z0-9 ]$", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				// it's an asset entry. parse it
				//string b = match.Groups[0].Value;

				string gotName = "???";
				string gotSize = "?";
				string gotPercent = "?";

				match = Regex.Match(line, @"Assets/.+", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotName = match.Groups[0].Value;
					gotName = gotName.Trim();
					//Debug.Log("    name? " + gotName);
				}

				match = Regex.Match(line, @"[0-9.]+ (kb|mb|b|gb)", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotSize = match.Groups[0].Value.ToUpper();
					//Debug.Log("    size? " + gotSize);
				}
				else
				{
					Debug.Log("didn't find size for :" + line);
				}

				match = Regex.Match(line, @"[0-9.]+%", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					gotPercent = match.Groups[0].Value;
					gotPercent = gotPercent.Substring(0, gotPercent.Length-1);
					//Debug.Log("    percent? " + gotPercent);
				}
				else
				{
					Debug.Log("didn't find percent for :" + line);
				}
				//Debug.Log("got: " + gotName + " size: " + gotSize);

				BuildSizePart inPart = new BuildSizePart();
				inPart.Name = gotName;
				inPart.Size = gotSize;
				inPart.SizeBytes = -1;
				inPart.DerivedSize = BuildReportUtil.GetApproxSizeFromString(gotSize);
				inPart.Percentage = Double.Parse(gotPercent);
				assetSizes.Add(inPart);

				if (gotName.EndsWith(".prefab"))
				{
					prefabsInBuildDict.Add(gotName, false);
				}
			}
			else
			{
				break;
			}
			currentIdx = lineEndIdx+1;
		}

		// include prefabs that are instantiated in scenes (they are not by default)
		//Debug.Log("addInfo.PrefabsUsedInScenes: " + addInfo.PrefabsUsedInScenes.Length);
		foreach (string p in prefabsUsedInScenes)
		{
			if (p.IndexOf("/Resources/") != -1) continue; // prefabs in resources folder are already included in the editor log build info
			if (prefabsInBuildDict.ContainsKey(p)) continue; // if already in assetSizes, continue

			BuildSizePart inPart = new BuildSizePart();
			inPart.Name = p;
			inPart.Size = "N/A";
			inPart.Percentage = -1;

			//Debug.Log("   prefab added in used assets: " + p);

			assetSizes.Add(inPart);
		}

		return assetSizes.ToArray();
	}




	public static BuildSizePart[][] SegregateAssetSizesPerCategory(BuildSizePart[] assetSizesAll, FileFilterGroup filters)
	{
		if (assetSizesAll == null || assetSizesAll.Length == 0) return null;

		// we do filters.Count+1 for Unrecognized category
		List< List<BuildSizePart> > ret = new List< List<BuildSizePart> >(filters.Count+1);
		for (int n = 0, len = filters.Count+1; n < len; ++n)
		{
			ret.Add(new List<BuildSizePart>());
		}

		bool foundAtLeastOneMatch = false;
		for (int idxAll = 0, lenAll = assetSizesAll.Length; idxAll < lenAll; ++idxAll)
		{
			foundAtLeastOneMatch = false;
			for (int n = 0, len = filters.Count; n < len; ++n)
			{
				if (filters[n].IsFileInFilter(assetSizesAll[idxAll].Name))
				{
					foundAtLeastOneMatch = true;
					ret[n].Add(assetSizesAll[idxAll]);
				}
			}

			if (!foundAtLeastOneMatch)
			{
				ret[ret.Count-1].Add(assetSizesAll[idxAll]);
			}
		}

		BuildSizePart[][] retArr = new BuildSizePart[filters.Count+1][];
		for (int n = 0, len = filters.Count+1; n < len; ++n)
		{
			retArr[n] = ret[n].ToArray();
		}

		return retArr;
	}


	static BuildSizePart[] GetAllUnusedAssets(BuildSizePart[] allUsedAssets, string[] scenesIncludedInProject, string projectAssetsPath, bool includeSvn, bool includeGit)
	{
		List<BuildSizePart> unusedAssets = new List<BuildSizePart>();
		Dictionary<string, bool> usedAssetsDict = new Dictionary<string, bool>();

		for (int n = 0, len = allUsedAssets.Length; n < len; ++n)
		{
			usedAssetsDict[allUsedAssets[n].Name] = true;
		}

		// consider scenes used to be part of used assets
		if (scenesIncludedInProject != null)
		{
			for (int n = 0, len = scenesIncludedInProject.Length; n < len; ++n)
			{
				usedAssetsDict[scenesIncludedInProject[n]] = true;
			}
		}

		// now loop through all assets in the whole project,
		// check if that file exists in the usedAssetsDict,
		// if not, include it in the unusedAssets list,
		// then sort by size

		string[] allAssets = BuildReportUtil.GetAllAssetsInProject(projectAssetsPath, includeSvn, includeGit);

		BuildSizePart newEntry;

		string projectPathWithoutAssetFolder = projectAssetsPath;
		const string suffixStringToRemove = "/Assets";
		projectPathWithoutAssetFolder = BuildReportUtil.RemoveSuffix(suffixStringToRemove, projectPathWithoutAssetFolder, 1);

		for (int n = 0, len = allAssets.Length; n < len; ++n)
		{
			// early-out: anything in Resources folder is always considered used
			if (allAssets[n].IndexOf("/Resources/") != -1)
			{
				continue;
			}

			if (!usedAssetsDict.ContainsKey(allAssets[n]) && !allAssets[n].EndsWith(".mask"))
			{
				string fullFilePath = projectPathWithoutAssetFolder + allAssets[n];

				newEntry = new BuildSizePart();
				newEntry.Name = allAssets[n];
				newEntry.SizeBytes = BuildReportUtil.GetFileSizeInBytes(fullFilePath);
				newEntry.Size = BuildReportUtil.GetFileSizeReadable(fullFilePath);
				newEntry.Percentage = -1;

				unusedAssets.Add(newEntry);
			}
		}

		unusedAssets.Sort(delegate(BuildSizePart b1, BuildSizePart b2) {
			if (b1.SizeBytes > b2.SizeBytes) return -1;
			if (b1.SizeBytes < b2.SizeBytes) return 1;
			return 0;
		});

		return unusedAssets.ToArray();
	}


	static void ParseDLLs(string editorLog, bool wasWebBuild, string buildFilePath, string projectAssetsPath, string editorAppContentsPath, ApiCompatibilityLevel monoLevel, StrippingLevel codeStrippingLevel, out BuildSizePart[] includedDLLs, out BuildSizePart[] scriptDLLs)
	{
		List<BuildSizePart> includedDLLsList = new List<BuildSizePart>();
		List<BuildSizePart> scriptDLLsList = new List<BuildSizePart>();

		string buildManagedDLLsFolder = BuildReportUtil.GetBuildManagedFolder(buildFilePath);
		string buildScriptDLLsFolder = buildManagedDLLsFolder;
		string buildManagedDLLsFolderHigherPriority = "";

		bool wasAndroidApkBuild = buildFilePath.EndsWith(".apk");

		if (wasWebBuild)
		{
			string tryPath;
			bool success = BuildReportUtil.AttemptGetWebTempStagingArea(projectAssetsPath, out tryPath);
			if (success)
			{
				buildManagedDLLsFolder = tryPath;
				buildScriptDLLsFolder = tryPath;
			}
		}
		else if (wasAndroidApkBuild)
		{
			string tryPath;
			bool success = BuildReportUtil.AttemptGetAndroidTempStagingArea(projectAssetsPath, out tryPath);
			if (success)
			{
				buildManagedDLLsFolder = tryPath;
				buildScriptDLLsFolder = tryPath;
			}
		}

		string unityFolderManagedDLLs;
		bool unityfoldersSuccess = BuildReportUtil.AttemptGetUnityFolderMonoDLLs(wasWebBuild, wasAndroidApkBuild, editorAppContentsPath, monoLevel, codeStrippingLevel, out unityFolderManagedDLLs, out buildManagedDLLsFolderHigherPriority);


		//Debug.Log("buildManagedDLLsFolder: " + buildManagedDLLsFolder);
		//Debug.Log("Application.dataPath: " + Application.dataPath);

		if (unityfoldersSuccess && (string.IsNullOrEmpty(buildManagedDLLsFolder) || !Directory.Exists(buildManagedDLLsFolder)))
		{
			Debug.LogWarning("Could not find build folder. Using Unity install folder instead for getting mono DLL file sizes.");
			buildManagedDLLsFolder = unityFolderManagedDLLs;
		}

		if (!Directory.Exists(buildManagedDLLsFolder))
		{
			Debug.LogWarning("Could not find folder for getting DLL file sizes. Got: \"" + buildManagedDLLsFolder + "\"");
		}


		const string PREFIX_REMOVE = "Dependency assembly - ";

		//int gotTotalSizeBytes = 0;

		//int gotScriptTotalSizeBytes = 0;

		BuildSizePart inPart;
		int currentIdx = 0;
		while (true)
		{
			int lineEndIdx = editorLog.IndexOf("\n", currentIdx);

			if (lineEndIdx == -1)
			{
				lineEndIdx = editorLog.Length;
			}

			string filename = editorLog.Substring(currentIdx, lineEndIdx-currentIdx);

			filename = BuildReportUtil.RemovePrefix(PREFIX_REMOVE, filename);

			string filepath;
			if (BuildReportUtil.IsAScriptDLL(filename))
			{
				filepath = buildScriptDLLsFolder + filename;
				//Debug.LogWarning("Script \"" + filepath + "\".");
			}
			else
			{
				filepath = buildManagedDLLsFolder + filename;

				if (!File.Exists(filepath) && unityfoldersSuccess && (buildManagedDLLsFolder != unityFolderManagedDLLs))
				{
					Debug.LogWarning("Failed to find file \"" + filepath + "\". Attempting to get from Unity folders.");
					filepath = unityFolderManagedDLLs + filename;

					if (!string.IsNullOrEmpty(buildManagedDLLsFolderHigherPriority) && File.Exists(buildManagedDLLsFolderHigherPriority + filename))
					{
						filepath = buildManagedDLLsFolderHigherPriority + filename;
					}
				}
			}

			if ((buildManagedDLLsFolder == unityFolderManagedDLLs) && !string.IsNullOrEmpty(buildManagedDLLsFolderHigherPriority) && File.Exists(buildManagedDLLsFolderHigherPriority + filename))
			{
				filepath = buildManagedDLLsFolderHigherPriority + filename;
			}

			inPart = BuildReportUtil.CreateBuildSizePartFromFile(filename, filepath);

			//gotTotalSizeBytes += inPart.SizeBytes;

			if (BuildReportUtil.IsAScriptDLL(filename))
			{
				//gotScriptTotalSizeBytes += inPart.SizeBytes;
				scriptDLLsList.Add(inPart);
			}
			else
			{
				includedDLLsList.Add(inPart);
			}


			currentIdx = lineEndIdx+1;
			if (currentIdx >= editorLog.Length)
			{
				break;
			}
		}

		// somehow, the editor logfile
		// doesn't include UnityEngine.dll
		// even though it gets included in the final build (for desktop builds)
		//
		// for web builds though, it makes sense not to put UnityEngine.dll in the build. and it isn't.
		// Instead, it's likely residing in the browser plugin to save bandwidth.
		//
		// begs the question though, why not have the whole Mono Web Subset DLLs be
		// installed alongside the Unity web browser plugin?
		// no need to bundle Mono DLLs in the web build itself.
		// would have shaved 1 whole MB when a game uses System.Xml.dll for example
		//
		//if (!wasWebBuild)
		{
			string filename = "UnityEngine.dll";
			string filepath = buildManagedDLLsFolder + filename;

			if (File.Exists(filepath))
			{
				inPart = BuildReportUtil.CreateBuildSizePartFromFile(filename, filepath);
				//gotTotalSizeBytes += inPart.SizeBytes;
				includedDLLsList.Add(inPart);
			}
		}


		//Debug.Log("total size: " + EditorUtility.FormatBytes(gotTotalSizeBytes) + " (" + gotTotalSizeBytes + " bytes)");
		//Debug.Log("total assembly size: " + EditorUtility.FormatBytes(gotScriptTotalSizeBytes) + " (" + gotScriptTotalSizeBytes + " bytes)");
		//Debug.Log("total size without assembly: " + EditorUtility.FormatBytes(gotTotalSizeBytes - gotScriptTotalSizeBytes) + " (" + (gotTotalSizeBytes-gotScriptTotalSizeBytes) + " bytes)");


		includedDLLs = includedDLLsList.ToArray();
		scriptDLLs = scriptDLLsList.ToArray();
	}


	const string NO_BUILD_INFO_WARNING = "Build Report Tool: No build info found. Build the project first. If you have more than one instance of the Unity Editor open, close all of them and open only one.";


	public static void GetValues(BuildInfo buildInfo, string[] scenesIncludedInProject, string buildFilePath, string projectAssetsPath, string editorAppContentsPath)
	{
		buildInfo.ProjectName = BuildReportUtil.GetProjectName(projectAssetsPath);

		string editorLog = BuildReportUtil.GetTextFileContents(_lastEditorLogPath);

		editorLog = editorLog.Replace("\r\n", "\n");



		buildInfo.BuildType = GetBuildTypeFromEditorLog(editorLog);

		if (string.IsNullOrEmpty(buildInfo.BuildType))
		{
			Debug.LogWarning(NO_BUILD_INFO_WARNING);
			return;
		}




		const string REPORT_START_KEY = "100.0% \n\nUsed Assets, sorted by uncompressed size:";

		int usedAssetsIdx = editorLog.LastIndexOf(REPORT_START_KEY);

		if (usedAssetsIdx == -1)
		{
			Debug.LogWarning("Build Report Window: No build info found in current session. Looking at data from previous session...");
			editorLog = BuildReportUtil.EditorPrevLogContents;
			editorLog = editorLog.Replace("\r\n", "\n");
		}

		usedAssetsIdx = editorLog.LastIndexOf(REPORT_START_KEY);

		if (usedAssetsIdx == -1)
		{
			Debug.LogWarning(NO_BUILD_INFO_WARNING);
			return;
		}

		//Debug.Log("usedAssetsIdx: " + usedAssetsIdx);

		int texturesIdx = editorLog.LastIndexOf("Textures", usedAssetsIdx);
		//Debug.Log("texturesIdx: " + texturesIdx);

		int completeSizeIdx = editorLog.IndexOf("Complete size ", texturesIdx);
		//Debug.Log("completeSizeIdx: " + completeSizeIdx);

		completeSizeIdx = editorLog.IndexOf("\n", completeSizeIdx);

		string buildParts = editorLog.Substring(texturesIdx, completeSizeIdx-texturesIdx);
		//Debug.Log("count: " + (completeSizeIdx-texturesIdx));


		//Debug.Log("STA\n" + buildParts + "\nEND\n");





		// build sizes per category

		buildInfo.BuildSizes = ParseBuildSizePartsFromString(buildParts);

		Array.Sort(buildInfo.BuildSizes, delegate(BuildSizePart b1, BuildSizePart b2) {
			if (b1.Percentage > b2.Percentage) return -1;
			else if (b1.Percentage < b2.Percentage) return 1;
			// if percentages are equal, check actual file size (approximate values)
			else if (b1.DerivedSize > b2.DerivedSize) return -1;
			else if (b1.DerivedSize < b2.DerivedSize) return 1;
			return 0;
		});



		// getting total build size (uncompressed)

		buildInfo.TotalBuildSize = "";

		foreach (BuildSizePart b in buildInfo.BuildSizes)
		{
			if (b.IsTotal)
			{
				buildInfo.TotalBuildSize = b.Size;
			}
		}



		// getting compressed build size

		buildInfo.CompressedBuildSize = "";
		const string COMPRESSED_BUILD_SIZE_KEY = "Total compressed size ";
		int compressedBuildSizeIdx = editorLog.LastIndexOf(COMPRESSED_BUILD_SIZE_KEY, usedAssetsIdx, 800);
		if (compressedBuildSizeIdx != -1)
		{
			// this data in the editor log only shows in web builds so far
			// meaning we do not get a compressed result in other builds (except android)
			//
			int compressedBuildSizeEndIdx = editorLog.IndexOf(". Total uncompressed size ", compressedBuildSizeIdx);
			buildInfo.CompressedBuildSize = editorLog.Substring(compressedBuildSizeIdx+COMPRESSED_BUILD_SIZE_KEY.Length, compressedBuildSizeEndIdx - compressedBuildSizeIdx - COMPRESSED_BUILD_SIZE_KEY.Length);
			//Debug.Log("compressed: " + compressedBuildSize);
		}
		else
		{
			// for getting android compressed build size
			if (buildFilePath.EndsWith(".apk"))
			{
				buildInfo.CompressedBuildSize = BuildReportUtil.GetFileSizeReadable(buildFilePath);
			}
		}




		// asset list

		buildInfo.FileFilters = new FileFilterGroup(CreateFileFilters());


		BuildSizePart[] all;
		BuildSizePart[][] perCategory;

		all = ParseAssetSizesFromEditorLog(editorLog, usedAssetsIdx+REPORT_START_KEY.Length, buildInfo.PrefabsUsedInScenes);
		perCategory = SegregateAssetSizesPerCategory(all, buildInfo.FileFilters);

		buildInfo.UsedAssets = new AssetList();
		buildInfo.UsedAssets.Init(all, perCategory, buildInfo.FileFilters);





		all = GetAllUnusedAssets(buildInfo.UsedAssets.All, scenesIncludedInProject, projectAssetsPath, buildInfo.IncludedSvnInUnused, buildInfo.IncludedGitInUnused);
		perCategory = SegregateAssetSizesPerCategory(all, buildInfo.FileFilters);

		buildInfo.UnusedAssets = new AssetList();
		buildInfo.UnusedAssets.Init(all, perCategory, buildInfo.FileFilters);


		//Debug.Log("buildInfo.UsedAssets.All: " + buildInfo.UsedAssets.All.Length);



		// DLLs

		const string MONO_DLL_KEY = "Mono dependencies included in the build\n";
		int monoDllsStaIdx = editorLog.LastIndexOf(MONO_DLL_KEY, texturesIdx);
		int monoDllsEndIdx = editorLog.IndexOf("\n\n", monoDllsStaIdx);
		string DLLs = editorLog.Substring(monoDllsStaIdx+MONO_DLL_KEY.Length, monoDllsEndIdx - monoDllsStaIdx-MONO_DLL_KEY.Length);
		//Debug.Log("STA\n" + DLLs + "\nEND\n");

		bool wasWebBuild = buildInfo.BuildType == "WebPlayer";

		ParseDLLs(DLLs, wasWebBuild, buildFilePath, projectAssetsPath, editorAppContentsPath, buildInfo.MonoLevel, buildInfo.CodeStrippingLevel, out buildInfo.MonoDLLs, out buildInfo.ScriptDLLs);

		Array.Sort(buildInfo.MonoDLLs, delegate(BuildSizePart b1, BuildSizePart b2) {
			if (b1.SizeBytes > b2.SizeBytes) return -1;
			if (b1.SizeBytes < b2.SizeBytes) return 1;
			return 0;
		});
		Array.Sort(buildInfo.ScriptDLLs, delegate(BuildSizePart b1, BuildSizePart b2) {
			if (b1.SizeBytes > b2.SizeBytes) return -1;
			if (b1.SizeBytes < b2.SizeBytes) return 1;
			return 0;
		});

		//foreach (string d in EditorUserBuildSettings.activeScriptCompilationDefines)
		//{
		//	Debug.Log("define: " + d);
		//}

		buildInfo.FlagOkToRefresh();
	}

	public static void ChangeSavePathToUserPersonalFolder()
	{
		BuildReportOptions.BuildReportSavePath = BuildReportUtil.GetUserHomeFolder();
	}

	public static void ChangeSavePathToProjectFolder()
	{
		string projectParent;
		if (_lastKnownBuildInfo != null)
		{
			projectParent = _lastKnownBuildInfo.ProjectAssetsPath;
		}
		else
		{
			projectParent = Application.dataPath;
		}

		const string suffixStringToRemove = "/Assets";
		projectParent = BuildReportUtil.RemoveSuffix(suffixStringToRemove, projectParent);

		int lastSlashIdx = projectParent.LastIndexOf("/");
		projectParent = projectParent.Substring(0, lastSlashIdx);

		BuildReportOptions.BuildReportSavePath = projectParent;
		//Debug.Log("projectParent: " + projectParent);
	}


	public static void RefreshData(ref BuildInfo buildInfo)
	{
		Init(ref buildInfo);
		RefreshListOfAllPrefabsUsedInAllScenes();
		CommitAdditionalInfoToCache(buildInfo);

		GetValues(buildInfo, buildInfo.ScenesIncludedInProject, buildInfo.BuildFilePath, buildInfo.ProjectAssetsPath, buildInfo.EditorAppContentsPath);
	}

	public static void RecategorizeAssetList(BuildInfo buildInfo)
	{
		buildInfo.FileFilters = new FileFilterGroup(CreateFileFilters());

		buildInfo.UsedAssets.AssignPerCategoryList( SegregateAssetSizesPerCategory(buildInfo.UsedAssets.All, buildInfo.FileFilters) );
		buildInfo.UnusedAssets.AssignPerCategoryList( SegregateAssetSizesPerCategory(buildInfo.UnusedAssets.All, buildInfo.FileFilters) );

		buildInfo.FlagOkToRefresh();
	}

	public static void RecategorizeAssetList()
	{
		if (_lastKnownBuildInfo == null)
		{
			Debug.LogError("_lastKnownBuildInfo uninitialized");
		}
		RecategorizeAssetList(_lastKnownBuildInfo);
	}

	[MenuItem("Window/Show Build Report")]
	public static void ShowBuildReport()
	{
		//RefreshData(ref _lastKnownBuildInfo);

		ShowBuildReportWithLastValues();
	}

	static void PopulateLastBuildValues()
	{
		if (string.IsNullOrEmpty(_lastKnownBuildInfo.BuildFilePath))
		{
			Debug.LogError("Can't populate last build values, BuildFilePath not initialized");
		}
		GetValues(_lastKnownBuildInfo, _lastKnownBuildInfo.ScenesIncludedInProject, _lastKnownBuildInfo.BuildFilePath, _lastKnownBuildInfo.ProjectAssetsPath, _lastKnownBuildInfo.EditorAppContentsPath);
	}

	// has to be called in main thread
	static void ShowBuildReportWithLastValues()
	{
		//BuildReportWindow window = ScriptableObject.CreateInstance<BuildReportWindow>();
		//window.ShowUtility();

		//System.Type[] desiredDockNextTo = new System.Type[]{typeof(UnityEditor.GameView)};

		//Debug.Log("showing build report window...");

		BuildReportWindow window = EditorWindow.GetWindow<BuildReportWindow>("Build Report", true, typeof(SceneView));
		window.Init(_lastKnownBuildInfo);
	}
}

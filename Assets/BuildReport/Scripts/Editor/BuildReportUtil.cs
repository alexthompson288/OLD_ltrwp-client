using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public static class BuildReportUtil
{
	public static bool IsAScriptDLL(string filename)
	{
		return filename.StartsWith("Assembly-");
	}

	public static string RemovePrefix(string prefix, string val)
	{
		if (val.StartsWith(prefix))
		{
			return val.Substring(prefix.Length, val.Length - prefix.Length);
		}
		return val;
	}

	public static string RemoveSuffix(string suffix, string val, int offset = 0)
	{
		if (val.EndsWith(suffix))
		{
			return val.Substring(0, val.Length - suffix.Length + offset);
		}
		return val;
	}


	public static int GetFileSizeInBytes(string filename)
	{
		FileInfo fi = new FileInfo(filename);
		return (int)fi.Length;
	}

	public static string GetFileSizeReadable(string filename)
	{
		//return EditorUtility.FormatBytes(GetFileSizeInBytes(filename));
		return MyFileSizeReadable(GetFileSizeInBytes(filename));
	}

	const double ONE_TERABYTE = 1099511627776.0;
	const double ONE_GIGABYTE = 1073741824.0;
	const double ONE_MEGABYTE = 1048576.0;
	const double ONE_KILOBYTE = 1024.0;

	public static string MyFileSizeReadable(double bytes)
	{

		double converted = bytes;
		string units = "B";

		if (bytes >= ONE_TERABYTE)
		{
			converted = bytes / ONE_TERABYTE;
			units = "TB";
		}
		else if (bytes >= ONE_GIGABYTE)
		{
			converted = bytes / ONE_GIGABYTE;
			units = "GB";
		}
		else if (bytes >= ONE_MEGABYTE)
		{
			converted = bytes / ONE_MEGABYTE;
			units = "MB";
		}
		else if (bytes >= ONE_KILOBYTE)
		{
			converted = bytes / ONE_KILOBYTE;
			units = "KB";
		}

		return String.Format("{0:0.##} {1}", converted, units);
	}

	public static double GetApproxSizeFromString(string size)
	{
		if (size == "N/A")
		{
			return 0;
		}

		string units = size.Substring(size.Length - 2, 2);
		string numOnly = size.Substring(0, size.Length - 2);

		double num = double.Parse(numOnly);

		//Debug.Log(size + " " + num);

		switch (units)
		{
			case "KB":
				return num * ONE_KILOBYTE;
			case "MB":
				return num * ONE_MEGABYTE;
			case "GB":
				return num * ONE_GIGABYTE;
			case "TB":
				return num * ONE_TERABYTE;
		}
		return 0;
	}


	static bool TextFileHasContents(string filepath, string contents)
	{
		FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader sr = new StreamReader(fs);

		bool ret = false;

		string line;
		while ((line = sr.ReadLine()) != null)
		{
			if (line.IndexOf(contents) != -1)
			{
				ret = true;
				break;
			}
		}

		fs.Close();
		return ret;
	}

	public static bool FileHasContents(string filepath, string contents)
	{
		return TextFileHasContents(filepath, contents);
	}



	public static string GetTextFileContents(string file)
	{
		// thanks to http://answers.unity3d.com/questions/167518/reading-editorlog-in-the-editor.html
		FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader sr = new StreamReader(fs);

		string contents = "";
		if (fs != null && sr != null)
		{
			contents = sr.ReadToEnd();
		}

		fs.Close();
		return contents;
	}



	// thanks to http://answers.unity3d.com/questions/16804/retrieving-project-name.html
	public static string GetProjectName(string projectAssetsFolderPath)
	{
		var dp = projectAssetsFolderPath;
		string[] s = dp.Split("/"[0]);
		return s[s.Length - 2];
	}




	// from http://stackoverflow.com/questions/1143706/getting-the-path-of-the-home-directory-in-c
	public static string UserHomePath
	{
		get
		{
			string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
				Environment.OSVersion.Platform == PlatformID.MacOSX)
				? Environment.GetEnvironmentVariable("HOME")
				: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

			return homePath;
		}
	}

	// for windows only
	public static string LocalAppDataPath
	{
		get
		{
			return Environment.GetEnvironmentVariable("LOCALAPPDATA").Replace("\\", "/");
		}
	}

	public static string EditorLogDefaultPath
	{
		get
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix ||
				Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				return UserHomePath + "/Library/Logs/Unity/Editor.log";
			}
			return LocalAppDataPath + "/Unity/Editor/Editor.log";
		}
	}

	public static string UsedEditorLogPath
	{
		get
		{
			if (IsDefaultEditorLogPathOverriden)
			{
				return BuildReportOptions.EditorLogOverridePath;
			}
			return EditorLogDefaultPath;
		}
	}

	public static string EditorLogPathOverrideMessage
	{
		get
		{
			if (IsDefaultEditorLogPathOverriden)
			{
				return "(Overriden)";
			}
			return "(Default)";
		}
	}

	public static bool IsDefaultEditorLogPathOverriden
	{
		get
		{
			return !string.IsNullOrEmpty(BuildReportOptions.EditorLogOverridePath);
		}
	}

	public static bool UsedEditorLogExists
	{
		get
		{
			return File.Exists(UsedEditorLogPath);
		}
	}

	public static string EditorPrevLogPath
	{
		get
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix ||
				Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				return UserHomePath + "/Library/Logs/Unity/Editor-prev.log";
			}
			return LocalAppDataPath + "/Unity/Editor/Editor-prev.log";
		}
	}

	public static string EditorPrevLogContents
	{
		get
		{
			return GetTextFileContents(EditorPrevLogPath);
		}
	}




	public static string GetBuildManagedFolder(string buildFilePath)
	{
		string buildFolder = buildFilePath;

		const string WINDOWS_APP_FILE_TYPE = ".exe";
		const string MAC_APP_FILE_TYPE = ".app";

		if (buildFolder.EndsWith(WINDOWS_APP_FILE_TYPE)) // Windows
		{
			//
			// example:
			// "/Users/Ferds/Unity Projects/BuildReportTool/testwin64.exe"
			//
			// need to remove ".exe" at end
			// then append "_Data" at end
			//
			buildFolder = buildFolder.Substring(0, buildFolder.Length - WINDOWS_APP_FILE_TYPE.Length);
			buildFolder += "_Data/Managed";
		}
		else if (buildFolder.EndsWith(MAC_APP_FILE_TYPE)) // Mac OS X
		{
			//
			// example:
			// "/Users/Ferds/Unity Projects/BuildReportTool/testmac.app"
			//
			// .app is really just a folder.
			//
			buildFolder += "/Contents/Data/Managed";
		}
		else if (Directory.Exists(buildFolder + "/Data/Managed/")) // iOS
		{
			buildFolder += "/Data/Managed";
		}
		else if (!Directory.Exists(buildFolder))
		{
			// happens with users who use custom builders
			//Debug.LogWarning("Folder \"" + buildFolder + "\" does not exist.");
			return "";
		}

		buildFolder += "/";

		return buildFolder;
	}

	static string GetProjectTempStagingArea(string projectDataPath)
	{
		string tempFolder = projectDataPath;
		const string assets = "Assets";
		tempFolder = tempFolder.Substring(0, tempFolder.Length - assets.Length);
		tempFolder += "Temp/StagingArea";
		return tempFolder;
	}

	public static bool AttemptGetWebTempStagingArea(string projectDataPath, out string path)
	{
		string tempFolder = GetProjectTempStagingArea(projectDataPath) + "/Data/Managed/";

		if (Directory.Exists(tempFolder))
		{
			path = tempFolder;
			return true;
		}
		path = "";
		return false;
	}

	public static bool AttemptGetAndroidTempStagingArea(string projectDataPath, out string path)
	{
		string tempFolder = GetProjectTempStagingArea(projectDataPath) + "/assets/bin/Data/Managed/";

		//Debug.Log(tempFolder);

		if (Directory.Exists(tempFolder))
		{
			path = tempFolder;
			return true;
		}
		path = "";
		return false;
	}

	public static bool AttemptGetUnityFolderMonoDLLs(bool wasWebBuild, bool wasAndroidApkBuild, string editorAppContentsPath, ApiCompatibilityLevel monoLevel, StrippingLevel codeStrippingLevel, out string path, out string higherPriorityPath)
	{
		bool success = false;

		// more hackery
		// attempt to get DLL size info
		// from Unity install folder
		//
		// this only happens in:
		//  1. Web build
		//  2. Android build
		//  3. Custom builders
		//
		string[] pathTries = new string[]
		{
			editorAppContentsPath + "/Frameworks/Mono/lib/mono",
			editorAppContentsPath + "/Mono/lib/mono",
			"/Applications/Unity/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity/Editor/Data/Mono/lib/mono",
#if UNITY_3_5
			"/Applications/Unity3/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 3/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity3.5/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 3.5/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3.5/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3.5/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3/Editor/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3/Editor/Data/Mono/lib/mono",
#endif
#if (UNITY_4 || UNITY_4_1)
			"/Applications/Unity4/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 4/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity4/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 4/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity4/Editor/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 4/Editor/Data/Mono/lib/mono",
#endif
		};

		string tryPath = "";

		for (int n = 0, len = pathTries.Length; n < len; ++n)
		{
			tryPath = pathTries[n];
			if (Directory.Exists(tryPath))
			{
				break;
			}
			tryPath = "";
		}

		if (!string.IsNullOrEmpty(tryPath))
		{
			success = true;

			// "unity_web" is obviously for the web build. Presumably, this one has DLLs removed that can compromise web security.
			// "2.0" is likely the full featured Mono libraries
			// "unity" is most likely the one used when selecting 2.0 subset in the player settings. this is the setting by default.
			// "micro" is probably the one used in StrippingLevel.UseMicroMSCorlib. only makes sense to be here when building on Android.
			//   since in iOS, we already have the DLL files. No need for this hackery in iOS. But since in Android we do not have a project folder,
			//   we resort to this.

			if (wasWebBuild)
			{
				path = tryPath + "/unity_web/";
			}
			else if (monoLevel == ApiCompatibilityLevel.NET_2_0_Subset)
			{
				path = tryPath + "/unity/";
			}
			else
			{
				path = tryPath + "/2.0/";
			}

			if (wasAndroidApkBuild && codeStrippingLevel == StrippingLevel.UseMicroMSCorlib)
			{
				higherPriorityPath = tryPath + "/micro/";
			}
			else
			{
				higherPriorityPath = "";
			}
		}
		else
		{
			path = "";
			higherPriorityPath = "";
		}

		return success;
	}


	//[MenuItem("Window/Test2")]
	public static string[] GetAllAssetsInProject(string projectAssetsFolder, bool includeSvn, bool includeGit)
	{
		string[] allAssets = Directory.GetFiles(projectAssetsFolder, "*.*", SearchOption.AllDirectories)
			.Where(s =>
				!s.EndsWith(".meta")
				&& ((!includeSvn && (s.ToLower().IndexOf("\\.svn\\") == -1 && s.ToLower().IndexOf("/.svn/") == -1)) || includeSvn)
				&& ((!includeGit && (s.ToLower().IndexOf("\\.git\\") == -1 && s.ToLower().IndexOf("/.git/") == -1)) || includeGit)
			).ToArray();


		//string log = "all assets:\n";
		for (int n = 0, len = allAssets.Length; n < len; ++n)
		{
			allAssets[n] = allAssets[n].Replace("\\", "/");
			allAssets[n] = allAssets[n].Replace(projectAssetsFolder, "Assets");
			//log += allAssets[n] + "\n";
		}
		//Debug.Log(log);

		return allAssets;
	}


	public static string[] GetAllScenesUsedInProject()
	{
		return EditorBuildSettings.scenes.Where(s => s.enabled).Select(n => n.path).ToArray();
	}


	public static BuildSizePart CreateBuildSizePartFromFile(string filename, string filepath)
	{
		BuildSizePart inPart = new BuildSizePart();
		inPart.Name = filename;

		if (File.Exists(filepath))
		{
			int fileSizeBytes = GetFileSizeInBytes(filepath);
			string readableSize = GetFileSizeReadable(filepath);
			//Debug.Log("file: " + filename + " size: " + readableSize + " (" + fileSizeBytes + " bytes)");


			inPart.Size = readableSize;
			/// \todo compute percentage: file size of this DLL out of total build size (would need to convert string of total build size into an int of bytes)
			inPart.Percentage = 0;
			inPart.SizeBytes = fileSizeBytes;
		}
		else
		{
			inPart.Size = "???";
			inPart.SizeBytes = -1;
		}

		return inPart;
	}





	//[MenuItem("Window/Test 3")]
	public static string GetUserHomeFolder()
	{
		string ret;
		ret = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString();
		//Debug.Log("GetUserHomeFolder: " + ret);
		ret = ret.Replace("\\", "/");
		return ret;
	}


	public static BuildInfo OpenSerializedBuildInfo(string serializedBuildInfoFilePath)
	{
		BuildInfo ret = null;

		using(FileStream fs = new FileStream(serializedBuildInfoFilePath, FileMode.Open))
		{
			XmlSerializer x = new XmlSerializer( typeof(BuildInfo) );
			XmlReader reader = new XmlTextReader(fs);

			ret = (BuildInfo)x.Deserialize(reader);

			if (BuildInfoHasContents(ret))
			{
				ret.OnUnserialize();
				ret.SetSavedPath(serializedBuildInfoFilePath);
			}
			else
			{
				Debug.LogError("Build Report Tool: Invalid data in build info file: " + serializedBuildInfoFilePath);
			}

			fs.Close();
		}

		return ret;
	}

	public static void SerializeBuildInfoAtFolder(BuildInfo buildInfo, string pathToSave)
	{
		string filePath = pathToSave;
		if (!string.IsNullOrEmpty(pathToSave))
		{
			if (!Directory.Exists(pathToSave))
			{
				Directory.CreateDirectory(pathToSave);
			}
			filePath += "/";
		}
		//Debug.Log("built folder");
		filePath += buildInfo.ProjectName + "-" + buildInfo.BuildType + buildInfo.TimeGot.ToString("-yyyyMMMdd-HHmmss") + ".xml";
		//Debug.Log("built filepath " + filePath);

		SerializeBuildInfo(buildInfo, filePath);
	}

	public static void SerializeBuildInfo(BuildInfo buildInfo, string serializedBuildInfoFilePath)
	{
		XmlSerializer x = new XmlSerializer( typeof(BuildInfo) );
		TextWriter writer = new StreamWriter(serializedBuildInfoFilePath);
		x.Serialize(writer, buildInfo);
		writer.Close();

		buildInfo.SetSavedPath(serializedBuildInfoFilePath);

		Debug.Log("Build Report Tool: Saved build info at \"" + buildInfo.SavedPath + "\"");
	}

	public static bool BuildInfoHasContents(BuildInfo n)
	{
		return n != null && n.HasContents;
	}
}

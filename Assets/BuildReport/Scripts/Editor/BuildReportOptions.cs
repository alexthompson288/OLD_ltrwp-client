using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public static class BuildReportOptions
{
	public static string EditorLogOverridePath
	{
		get
		{
			return EditorPrefs.GetString("BRT_EditorLogOverridePath", "");
		}
		set
		{
			EditorPrefs.SetString("BRT_EditorLogOverridePath", value);
		}
	}

	public static bool IncludeSvnInUnused
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeSvnInUnused", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeSvnInUnused", value);
		}
	}

	public static bool IncludeGitInUnused
	{
		get
		{
			return EditorPrefs.GetBool("BRT_IncludeGitInUnused", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_IncludeGitInUnused", value);
		}
	}

	public static FileFilterDisplay GetOptionFileFilterDisplay()
	{
		switch (FileFilterDisplayInt)
		{
			case 0:
				return FileFilterDisplay.DropDown;
			case 1:
				return FileFilterDisplay.Buttons;
		}
		return FileFilterDisplay.DropDown;
	}

	public static int FileFilterDisplayInt
	{
		get
		{
			return EditorPrefs.GetInt("BRT_FileFilterDisplay", 0);
		}
		set
		{
			EditorPrefs.SetInt("BRT_FileFilterDisplay", value);
		}
	}

	public static bool ShowAfterBuild
	{
		get
		{
			return EditorPrefs.GetBool("BRT_ShowAfterBuild", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_ShowAfterBuild", value);
		}
	}


	public static bool CollectBuildInfo
	{
		get
		{
			return EditorPrefs.GetBool("BRT_CollectBuildInfo", true);
		}
		set
		{
			EditorPrefs.SetBool("BRT_CollectBuildInfo", value);
		}
	}

	/*public static int GetOptionEditorLogMegaByteSizeReadLimit()
	{
		return EditorPrefs.GetInt("BRT_EditorLogMegaByteSizeReadLimit", 100);
	}
	public static void SetOptionEditorLogMegaByteSizeReadLimit(int val)
	{
		EditorPrefs.SetInt("BRT_EditorLogMegaByteSizeReadLimit", val);
	}*/


	public static string BuildReportFolderName
	{
		get
		{
			return EditorPrefs.GetString("BRT_BuildReportFolderName", "UnityBuildReports");
		}
		set
		{
			EditorPrefs.SetString("BRT_BuildReportFolderName", value);
		}
	}


	public static string BuildReportSavePath
	{
		get
		{
			return EditorPrefs.GetString("BRT_SavePath", BuildReportUtil.GetUserHomeFolder() + "/" + BuildReportFolderName);
		}
		set
		{
			EditorPrefs.SetString("BRT_SavePath", value + "/" + BuildReportFolderName);
		}
	}




	public static int SaveType
	{
		get
		{
			return EditorPrefs.GetInt("BRT_SaveType", SAVE_TYPE_PERSONAL);
		}
		set
		{
			EditorPrefs.SetInt("BRT_SaveType", value);
		}
	}

	public enum FileFilterDisplay
	{
		DropDown = 0,
		Buttons = 1
	}

	public const int SAVE_TYPE_PERSONAL = 0;
	public const int SAVE_TYPE_PROJECT = 1;
}






#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public partial class BuildReportWindow : EditorWindow
{
	[SerializeField]
	BuildInfo _buildInfo;

	GUISkin _usedSkin = null;

	Vector2 _assetListScrollPos;

	string GetLastFolder(string inFolder)
	{
		inFolder = inFolder.Replace('\\', '/');

		//Debug.Log("folder: " + inFolder);
		//string folderName = Path.GetDirectoryName(folderEntries[n]);

		int lastSlashIdx = inFolder.LastIndexOf('/');
		if (lastSlashIdx == -1)
		{
			return "";
		}
		return inFolder.Substring(lastSlashIdx+1, inFolder.Length-lastSlashIdx-1);

	}

	string FindAssetFolder(string folderToStart, string desiredFolderName)
	{
		string[] folderEntries = Directory.GetDirectories(folderToStart);

		for (int n = 0, len = folderEntries.Length; n < len; ++n)
		{
			string folderName = GetLastFolder(folderEntries[n]);
			//Debug.Log("folderName: " + folderName);

			if (folderName == desiredFolderName)
			{
				return folderEntries[n];
			}
			else
			{
				string recursed = FindAssetFolder(folderEntries[n], desiredFolderName);
				string recursedFolderName = GetLastFolder(recursed);
				if (recursedFolderName == desiredFolderName)
				{
					return recursed;
				}
			}
		}
		return "";
	}



	public static bool IsOpen { get; set; }

	void OnDisable()
	{
		IsOpen = false;
	}

	void OnEnable()
	{
		IsOpen = true;

		InitGUISkin();


		if (BuildReportUtil.BuildInfoHasContents(_buildInfo))
		{
			//Debug.Log("recompiled " + _buildInfo.SavedPath);
			if (!string.IsNullOrEmpty(_buildInfo.SavedPath))
			{
				_buildInfo = BuildReportUtil.OpenSerializedBuildInfo(_buildInfo.SavedPath);
			}
			else
			{
				_buildInfo.UsedAssets.AssignPerCategoryList( BuildReport.SegregateAssetSizesPerCategory(_buildInfo.UsedAssets.All, _buildInfo.FileFilters) );
				_buildInfo.UnusedAssets.AssignPerCategoryList( BuildReport.SegregateAssetSizesPerCategory(_buildInfo.UnusedAssets.All, _buildInfo.FileFilters) );
			}
		}
	}

	void InitGUISkin()
	{
		string guiSkinToUse = DEFAULT_GUI_SKIN_FILENAME;
		if (EditorGUIUtility.isProSkin)
		{
			guiSkinToUse = DARK_GUI_SKIN_FILENAME;
		}

		// try default path
		_usedSkin = AssetDatabase.LoadAssetAtPath(BUILD_REPORT_TOOL_DEFAULT_PATH + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;

		if (_usedSkin == null)
		{
			Debug.LogWarning(BUILD_REPORT_PACKAGE_MOVED_MSG);

			string folderPath = FindAssetFolder(Application.dataPath, BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);
			if (!string.IsNullOrEmpty(folderPath))
			{
				folderPath = folderPath.Replace('\\', '/');
				int assetsIdx = folderPath.IndexOf("/Assets/");
				if (assetsIdx != -1)
				{
					folderPath = folderPath.Substring(assetsIdx+8, folderPath.Length-assetsIdx-8);
				}
				//Debug.Log(folderPath);

				_usedSkin = AssetDatabase.LoadAssetAtPath("Assets/" + folderPath + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;
			}
			else
			{
				Debug.LogError(BUILD_REPORT_PACKAGE_MISSING_MSG);
			}
			//Debug.Log("_usedSkin " + (_usedSkin != null));
		}
	}


	public void Init(BuildInfo buildInfo)
	{
		_buildInfo = buildInfo;
	}

	void Refresh()
	{
		BuildReport.RefreshData(ref _buildInfo);
	}

	void Update()
	{
		if (_buildInfo != null)
		{
			if (_buildInfo.RequestedToRefresh)
			{
				Repaint();
				_buildInfo.FlagFinishedRefreshing();
			}
		}
	}



	void DrawNames(BuildSizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildSizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Name, styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}
	void DrawReadableSizes(BuildSizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildSizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Size, styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}
	void DrawPercentages(BuildSizePart[] list)
	{
		GUILayout.BeginVertical();
		bool useAlt = false;
		foreach (BuildSizePart b in list)
		{
			if (b.IsTotal) continue;
			string styleToUse = useAlt ? LIST_NORMAL_ALT_STYLE_NAME : LIST_NORMAL_STYLE_NAME;
			GUILayout.Label(b.Percentage + "%", styleToUse);
			useAlt = !useAlt;
		}
		GUILayout.EndVertical();
	}


	void DrawTotalSize()
	{
		GUILayout.BeginVertical();

				GUILayout.Label(TIME_OF_BUILD_LABEL, INFO_TITLE_STYLE_NAME);
				GUILayout.Label(_buildInfo.GetTimeReadable(), INFO_SUBTITLE_STYLE_NAME);

				GUILayout.Space(30);

		if (!string.IsNullOrEmpty(_buildInfo.CompressedBuildSize))
		{
				GUILayout.BeginVertical();
					GUILayout.Label(UNCOMPRESSED_TOTAL_SIZE_LABEL, INFO_TITLE_STYLE_NAME);
					GUILayout.Space(5);
					GUILayout.Label(_buildInfo.TotalBuildSize, BIG_NUMBER_STYLE_NAME);
				GUILayout.EndVertical();

				GUILayout.Space(30);

				GUILayout.BeginVertical();
					GUILayout.Label(COMPRESSED_TOTAL_SIZE_LABEL, INFO_TITLE_STYLE_NAME);
					GUILayout.Space(5);
					GUILayout.Label(_buildInfo.CompressedBuildSize, BIG_NUMBER_STYLE_NAME);
				GUILayout.EndVertical();
		}
		else
		{
				GUILayout.Label(TOTAL_SIZE_LABEL, INFO_TITLE_STYLE_NAME);
				GUILayout.Space(5);
				GUILayout.Label(_buildInfo.TotalBuildSize, BIG_NUMBER_STYLE_NAME);
		}

		GUILayout.EndVertical();
	}

	void DrawBuildSizes()
	{
		if (!string.IsNullOrEmpty(_buildInfo.CompressedBuildSize))
		{
			GUILayout.BeginVertical();
		}

		GUILayout.Label(TOTAL_SIZE_BREAKDOWN_LABEL, INFO_TITLE_STYLE_NAME);

		if (!string.IsNullOrEmpty(_buildInfo.CompressedBuildSize))
		{
			GUILayout.BeginHorizontal();
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_PRE_BOLD, INFO_SUBTITLE_STYLE_NAME);
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_BOLD, INFO_SUBTITLE_BOLD_STYLE_NAME);
				GUILayout.Label(TOTAL_SIZE_BREAKDOWN_MSG_POST_BOLD, INFO_SUBTITLE_STYLE_NAME);
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}

		if (_buildInfo.BuildSizes != null)
		{
			GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
			DrawNames(_buildInfo.BuildSizes);
			DrawReadableSizes(_buildInfo.BuildSizes);
			DrawPercentages(_buildInfo.BuildSizes);
			GUILayout.EndHorizontal();
		}
	}

	void DrawDLLList()
	{
		GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
				GUILayout.Label(MONO_DLLS_LABEL, INFO_TITLE_STYLE_NAME);
				{
					GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
						DrawNames(_buildInfo.MonoDLLs);
						DrawReadableSizes(_buildInfo.MonoDLLs);
					GUILayout.EndHorizontal();
				}
			GUILayout.EndVertical();

			GUILayout.Space(15);

			GUILayout.BeginVertical();
				GUILayout.Label(SCRIPT_DLLS_LABEL, INFO_TITLE_STYLE_NAME);
				{
					GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
						DrawNames(_buildInfo.ScriptDLLs);
						DrawReadableSizes(_buildInfo.ScriptDLLs);
					GUILayout.EndHorizontal();
				}
			GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}

	void PingAssetInProject(string file)
	{
		// thanks to http://answers.unity3d.com/questions/37180/how-to-highlight-or-select-an-asset-in-project-win.html
		GUI.skin = null;
		EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(file, typeof(Object)));
		var asset = AssetDatabase.LoadMainAssetAtPath(file);
		if (asset != null)
		{
			Selection.activeObject = asset;
		}
		GUI.skin = _usedSkin;
	}

	void DrawAssetList(AssetList list, FileFilterGroup filter)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_PRE_BOLD, INFO_SUBTITLE_STYLE_NAME);
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_BOLD, INFO_SUBTITLE_BOLD_STYLE_NAME);
			GUILayout.Label(ASSET_SIZE_BREAKDOWN_MSG_POST_BOLD, INFO_SUBTITLE_STYLE_NAME);
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		if (list != null)
		{
			BuildSizePart[] assetListToUse = list.GetListToDisplay(filter);

			if (assetListToUse != null)
			{
				if (assetListToUse.Length == 0)
				{
					GUILayout.Label(NO_FILES_FOR_THIS_CATEGORY, INFO_TITLE_STYLE_NAME);
				}
				else
				{
					const int LIST_HEIGHT = 20;
					const int ICON_DISPLAY_SIZE = 15;
					EditorGUIUtility.SetIconSize(Vector2.one * ICON_DISPLAY_SIZE);
					bool useAlt = false;


					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical();
						foreach (BuildSizePart b in assetListToUse)
						{
							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							if (list.InSumSelection(b))
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							Texture icon = AssetDatabase.GetCachedIcon(b.Name);
							if (GUILayout.Button(new GUIContent(b.Name, icon), styleToUse, GUILayout.Height(LIST_HEIGHT)))
							{
								PingAssetInProject(b.Name);
							}
							useAlt = !useAlt;
						}
					GUILayout.EndVertical();

					GUILayout.BeginVertical();
						useAlt = false;
						foreach (BuildSizePart b in assetListToUse)
						{
							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							if (list.InSumSelection(b))
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							if (GUILayout.Button(b.Size, styleToUse, GUILayout.MinWidth(50), GUILayout.Height(LIST_HEIGHT)))
							{
								list.ToggleSumSelection(b);
							}
							useAlt = !useAlt;
						}
					GUILayout.EndVertical();

					GUILayout.BeginVertical();
						useAlt = false;
						foreach (BuildSizePart b in assetListToUse)
						{
							string styleToUse = useAlt ? LIST_SMALL_ALT_STYLE_NAME : LIST_SMALL_STYLE_NAME;
							if (list.InSumSelection(b))
							{
								styleToUse = LIST_SMALL_SELECTED_NAME;
							}

							string text = b.Percentage + "%";
							if (b.Percentage < 0)
							{
								text = NON_APPLICABLE_PERCENTAGE;
							}

							GUILayout.Label(text, styleToUse, GUILayout.MinWidth(30), GUILayout.Height(LIST_HEIGHT));
							useAlt = !useAlt;
						}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}
		}
	}


	void DrawOverviewScreen()
	{
		GUILayout.Space(10); // extra top padding

		GUILayout.BeginHorizontal();
			GUILayout.Space(10); // extra left padding
			DrawTotalSize();
			GUILayout.Space(CATEGORY_HORIZONTAL_SPACING);
			GUILayout.BeginVertical();
				DrawBuildSizes();
				GUILayout.Space(CATEGORY_VERTICAL_SPACING);
				DrawDLLList();
			GUILayout.EndVertical();
			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();
	}

	string[] _fileFilterDisplayTypeLabels = new string[] {FILE_FILTER_DISPLAY_TYPE_DROP_DOWN_LABEL, FILE_FILTER_DISPLAY_TYPE_BUTTONS_LABEL};

	string[] _saveTypeLabels = new string[] {SAVE_PATH_TYPE_PERSONAL_LABEL, SAVE_PATH_TYPE_PROJECT_LABEL};

	void DrawOptionsScreen()
	{
		GUILayout.Space(10); // extra top padding

		GUILayout.BeginHorizontal();
			GUILayout.Space(20); // extra left padding
			GUILayout.BeginVertical();

				BuildReportOptions.CollectBuildInfo = GUILayout.Toggle(BuildReportOptions.CollectBuildInfo, COLLECT_BUILD_INFO_LABEL);
				BuildReportOptions.ShowAfterBuild = GUILayout.Toggle(BuildReportOptions.ShowAfterBuild, SHOW_AFTER_BUILD_LABEL);

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);

				BuildReportOptions.IncludeSvnInUnused = GUILayout.Toggle(BuildReportOptions.IncludeSvnInUnused, INCLUDE_SVN_LABEL);
				BuildReportOptions.IncludeGitInUnused = GUILayout.Toggle(BuildReportOptions.IncludeGitInUnused, INCLUDE_GIT_LABEL);

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);

				GUILayout.BeginHorizontal();
					GUILayout.Label(FILE_FILTER_DISPLAY_TYPE_LABEL);
					BuildReportOptions.FileFilterDisplayInt = GUILayout.SelectionGrid(BuildReportOptions.FileFilterDisplayInt, _fileFilterDisplayTypeLabels, _fileFilterDisplayTypeLabels.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);

				GUILayout.Label(SAVE_PATH_LABEL + BuildReportOptions.BuildReportSavePath);

				GUILayout.BeginHorizontal();
					GUILayout.Label(SAVE_FOLDER_NAME_LABEL);
					BuildReportOptions.BuildReportFolderName = GUILayout.TextField(BuildReportOptions.BuildReportFolderName);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
					GUILayout.Label(SAVE_PATH_TYPE_LABEL);
					BuildReportOptions.SaveType = GUILayout.SelectionGrid(BuildReportOptions.SaveType, _saveTypeLabels, _saveTypeLabels.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(CATEGORY_VERTICAL_SPACING);



				GUILayout.Label(EDITOR_LOG_LABEL + BuildReportUtil.EditorLogPathOverrideMessage + ": " + BuildReportUtil.UsedEditorLogPath);

				if (!BuildReportUtil.UsedEditorLogExists)
				{
					GUILayout.Label(EDITOR_LOG_INVALID_MSG);
				}

				GUILayout.BeginHorizontal();
					if (GUILayout.Button(SET_OVERRIDE_LOG_LABEL))
					{
						string filepath = EditorUtility.OpenFilePanel(
							"", // title
							"", // default path
							""); // file type (only one type allowed?)

						if (!string.IsNullOrEmpty(filepath))
						{
							BuildReportOptions.EditorLogOverridePath = filepath;
						}
					}
					if (GUILayout.Button(CLEAR_OVERRIDE_LOG_LABEL))
					{
						BuildReportOptions.EditorLogOverridePath = "";
					}
				GUILayout.EndHorizontal();



			GUILayout.EndVertical();
			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();

		if (BuildReportOptions.SaveType == BuildReportOptions.SAVE_TYPE_PERSONAL)
		{
			// changed to user's personal folder
			BuildReport.ChangeSavePathToUserPersonalFolder();
		}
		else if (BuildReportOptions.SaveType == BuildReportOptions.SAVE_TYPE_PROJECT)
		{
			// changed to project folder
			BuildReport.ChangeSavePathToProjectFolder();
		}
	}


	int _selectedCategoryIdx = 0;
	string[] _categories = new string[] {OVERVIEW_CATEGORY_LABEL, USED_ASSETS_CATEGORY_LABEL, UNUSED_ASSETS_CATEGORY_LABEL, OPTIONS_CATEGORY_LABEL};

	const int OVERVIEW_IDX = 0;
	const int USED_ASSETS_IDX = 1;
	const int UNUSED_ASSETS_IDX = 2;
	const int OPTIONS_IDX = 3;


	void OnGUI()
	{
		if (_usedSkin == null)
		{
			GUI.Label(new Rect(20, 20, 500, 100), BUILD_REPORT_PACKAGE_MISSING_MSG);
			return;
		}

		GUI.skin = _usedSkin;

		if (GUI.Button(new Rect(5, 5, 100, 20), REFRESH_LABEL))
		{
			Refresh();
		}
		if (GUI.Button(new Rect(110, 5, 100, 20), OPEN_LABEL))
		{
			string filepath = EditorUtility.OpenFilePanel(
				OPEN_SERIALIZED_BUILD_INFO_TITLE,
				BuildReportOptions.BuildReportSavePath,
				"xml");

			if (!string.IsNullOrEmpty(filepath))
			{
				_buildInfo = BuildReportUtil.OpenSerializedBuildInfo(filepath);
			}
		}
		if (GUI.Button(new Rect(215, 5, 100, 20), SAVE_LABEL) && BuildReportUtil.BuildInfoHasContents(_buildInfo))
		{
			string filepath = EditorUtility.SaveFilePanel(
				SAVE_MSG,
				BuildReportOptions.BuildReportSavePath,
				_buildInfo.GetDefaultFilename(),
				"xml");

			if (!string.IsNullOrEmpty(filepath))
			{
				BuildReportUtil.SerializeBuildInfo(_buildInfo, filepath);
			}
		}
		if (!BuildReportUtil.BuildInfoHasContents(_buildInfo) && GUI.Button(new Rect(320, 5, 100, 20), OPTIONS_CATEGORY_LABEL))
		{
			_selectedCategoryIdx = OPTIONS_IDX;
		}

		if (_buildInfo == null || string.IsNullOrEmpty(_buildInfo.ProjectName) || _buildInfo.MonoDLLs == null)
		{
			if (_selectedCategoryIdx == OPTIONS_IDX)
			{
				GUILayout.Space(40);
				DrawOptionsScreen();
			}
			else
			{
				float w = 300;
				float h = 100;
				float x = (position.width - w) * 0.5f;
				float y = (position.height - h) * 0.5f;

				if (BuildReport.LoadingValuesFromThread)
				{
					GUI.Label(new Rect(x, y, w, h), LOADING_PLEASE_WAIT);
				}
				else
				{
					GUI.Label(new Rect(x, y, w, h), NO_BUILD_INFO_FOUND_MSG);
				}
			}

			return;
		}

		GUILayout.Space(10); // top padding


		GUILayout.BeginVertical();
			GUILayout.Space(20);
			GUILayout.Label(_buildInfo.ProjectName, MAIN_TITLE_STYLE_NAME);
			GUILayout.Label(BUILD_TYPE_PREFIX_MSG + _buildInfo.BuildType + BUILD_TYPE_SUFFIX_MSG, MAIN_SUBTITLE_STYLE_NAME);
		GUILayout.EndVertical();

		_selectedCategoryIdx = GUILayout.SelectionGrid(_selectedCategoryIdx, _categories, _categories.Length);

		if (_selectedCategoryIdx == USED_ASSETS_IDX || _selectedCategoryIdx == UNUSED_ASSETS_IDX)
		{
			GUILayout.Space(5);
			_buildInfo.FileFilters.Draw(_selectedCategoryIdx == USED_ASSETS_IDX ? _buildInfo.UsedAssets : _buildInfo.UnusedAssets, position.width - 100);


			//if (AtLeastOneSelectedForSum)
			{
				AssetList assetListUsed = (_selectedCategoryIdx == USED_ASSETS_IDX) ? _buildInfo.UsedAssets : _buildInfo.UnusedAssets;
				GUILayout.BeginHorizontal();
					if (GUILayout.Button(SELECT_NONE_LABEL))
					{
						assetListUsed.ClearSelection();
					}
					GUILayout.Label(SELECTED_QTY_LABEL + assetListUsed.GetSelectedCount() + ". " + SELECTED_SIZE_LABEL + assetListUsed.GetReadableSizeOfSumSelection() + ". " + SELECTED_PERCENT_LABEL + assetListUsed.GetPercentageOfSumSelection() + "%", INFO_SUBTITLE_STYLE_NAME);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}

		}
		else
		{
			GUILayout.Space(10);
		}

		GUILayout.BeginHorizontal();
			GUILayout.Space(15); // left padding
			GUILayout.BeginVertical();


				_assetListScrollPos = GUILayout.BeginScrollView(
					_assetListScrollPos);


					if (_selectedCategoryIdx == OVERVIEW_IDX)
					{
						DrawOverviewScreen();
					}
					else if (_selectedCategoryIdx == USED_ASSETS_IDX)
					{
						DrawAssetList(_buildInfo.UsedAssets, _buildInfo.FileFilters);
					}
					else if (_selectedCategoryIdx == UNUSED_ASSETS_IDX)
					{
						DrawAssetList(_buildInfo.UnusedAssets, _buildInfo.FileFilters);
					}
					else if (_selectedCategoryIdx == OPTIONS_IDX)
					{
						DrawOptionsScreen();
					}

					GUILayout.FlexibleSpace();
				GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(5); // right padding
		GUILayout.EndHorizontal();


		GUILayout.Space(10); // bottom padding
	}
}
#endif

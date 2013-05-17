#if UNITY_EDITOR
using UnityEditor;

public partial class BuildReportWindow : EditorWindow
{
	// GUI messages, labels

	const string BUILD_REPORT_PACKAGE_MOVED_MSG = "BuildReport package seems to have been moved. Finding...";
	const string BUILD_REPORT_PACKAGE_MISSING_MSG = "Unable to find BuildReport package folder! Cannot find suitable GUI Skin.\nTry editing the source code and change the value\nof `BUILD_REPORT_TOOL_DEFAULT_PATH` to what path the Build Report Tool is in.\nMake sure the folder is named \"BuildReport\".";
	const string NO_BUILD_INFO_FOUND_MSG = "No Build Info.\nClick Refresh to retrieve the last build info from the Editor log. Click Open to manually open a previously saved build info file.";
	const string LOADING_PLEASE_WAIT = "Loading Build Info. Please wait. Click this window if not in focus to refresh.";

	const string TIME_OF_BUILD_LABEL = "Time of Build:";


	const string UNCOMPRESSED_TOTAL_SIZE_LABEL = "Uncompressed\nTotal Build Size:";
	const string COMPRESSED_TOTAL_SIZE_LABEL = "Compressed\nTotal Build Size:";
	const string TOTAL_SIZE_LABEL = "Total Build Size:";
	const string MONO_DLLS_LABEL = "Included DLLs:";
	const string SCRIPT_DLLS_LABEL = "Script DLLs:";

	const string OPEN_SERIALIZED_BUILD_INFO_TITLE = "Open Build Info XML File";

	const string TOTAL_SIZE_BREAKDOWN_LABEL = "Size Breakdown:";

	const string TOTAL_SIZE_BREAKDOWN_MSG_PRE_BOLD = "Based on";
	const string TOTAL_SIZE_BREAKDOWN_MSG_BOLD = "uncompressed";
	const string TOTAL_SIZE_BREAKDOWN_MSG_POST_BOLD = "build size";


	const string ASSET_SIZE_BREAKDOWN_LABEL = "Asset Breakdown:";

	const string ASSET_SIZE_BREAKDOWN_MSG_PRE_BOLD = "Sorted by";
	const string ASSET_SIZE_BREAKDOWN_MSG_BOLD = "uncompressed";
	const string ASSET_SIZE_BREAKDOWN_MSG_POST_BOLD = "size. Click on an asset name to select it. Click on the asset's size to select it.";

	const string NO_FILES_FOR_THIS_CATEGORY = "None";

	const string NON_APPLICABLE_PERCENTAGE = "N/A";

	const string OVERVIEW_CATEGORY_LABEL = "Overview";
	const string USED_ASSETS_CATEGORY_LABEL = "Used Assets";
	const string UNUSED_ASSETS_CATEGORY_LABEL = "Unused Assets";
	const string OPTIONS_CATEGORY_LABEL = "Options";

	const string REFRESH_LABEL = "Refresh";
	const string OPEN_LABEL = "Open";
	const string SAVE_LABEL = "Save";

	const string SAVE_MSG = "Save Build Info to XML";

	const string SELECT_NONE_LABEL = "Select None";
	const string SELECTED_QTY_LABEL = "Selected: ";
	const string SELECTED_SIZE_LABEL = "Total size: ";
	const string SELECTED_PERCENT_LABEL = "Total percentage: ";

	const string BUILD_TYPE_PREFIX_MSG = "For ";
	const string BUILD_TYPE_SUFFIX_MSG = "";

	const string COLLECT_BUILD_INFO_LABEL = "Collect and save build info automatically after building";
	const string SHOW_AFTER_BUILD_LABEL = "Show Build Report Window automatically after building";
	const string INCLUDE_SVN_LABEL = "Include SVN metadata in unused assets scan";
	const string INCLUDE_GIT_LABEL = "Include Git metadata in unused assets scan";
	const string FILE_FILTER_DISPLAY_TYPE_LABEL = "Draw file filters as:";

	const string FILE_FILTER_DISPLAY_TYPE_DROP_DOWN_LABEL = "Dropdown box";
	const string FILE_FILTER_DISPLAY_TYPE_BUTTONS_LABEL = "Buttons";

	const string SAVE_PATH_LABEL = "Current save path: ";
	const string SAVE_FOLDER_NAME_LABEL = "Folder name:";
	const string SAVE_PATH_TYPE_LABEL = "Save build reports to:";

	const string SAVE_PATH_TYPE_PERSONAL_LABEL = "At user's personal folder";
	const string SAVE_PATH_TYPE_PROJECT_LABEL = "Beside project folder";

	const string EDITOR_LOG_LABEL = "Unity Editor.log path ";
	const string EDITOR_LOG_INVALID_MSG = "Invalid path. Please change the path by clicking \"Set Override Log\"";

	const string SET_OVERRIDE_LOG_LABEL = "Set Override Log";
	const string CLEAR_OVERRIDE_LOG_LABEL = "Clear Override Log";
}
#endif

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildSizePart
{
	public string Name;
	public string Size;
	public double Percentage = 0;
	public int SizeBytes = -1;
	public double DerivedSize = 0; // in cases where we don't have exact values of file size (we just got it from editor log converted to readable format already)

	public double UsableSize
	{
		get
		{
			if (DerivedSize > 0)
				return DerivedSize;
			return SizeBytes;
		}
	}

	public bool IsTotal { get{ return Name == "Complete size"; } }
}

[System.Serializable]
public class AssetList
{
	[SerializeField]
	BuildSizePart[] _all;

	[SerializeField]
	BuildSizePart[][] _perCategory;

	public BuildSizePart[] GetListToDisplay(FileFilterGroup fileFilters)
	{
		BuildSizePart[] ret = null;
		if (fileFilters.SelectedFilterIdx == -1)
		{
			ret = All;
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			ret = PerCategory[fileFilters.SelectedFilterIdx];
		}
		return ret;
	}

	[SerializeField]
	string[] _labels;

	public void Init(BuildSizePart[] all, BuildSizePart[][] perCategory, FileFilterGroup fileFilters)
	{
		_all = all;
		_perCategory = perCategory;

		_labels = new string[1 + PerCategory.Length];

		_labels[0] = "All (" + All.Length + ")";
		for (int n = 0, len = fileFilters.Count; n < len; ++n)
		{
			_labels[n+1] = fileFilters[n].Label + " (" + PerCategory[n].Length + ")";
		}
	}
	public BuildSizePart[] All { get{ return _all; } set{ _all = value; } }
	public BuildSizePart[][] PerCategory { get{ return _perCategory; } }

	public void AssignPerCategoryList(BuildSizePart[][] perCategory)
	{
		_perCategory = perCategory;
	}

	public string[] Labels { get{ return _labels; } set{ _labels = value; } }





	[SerializeField]
	Dictionary <string, BuildSizePart> _selectedForSum = new Dictionary<string, BuildSizePart>();

	public void ToggleSumSelection(BuildSizePart b)
	{
		if (InSumSelection(b))
		{
			RemoveFromSumSelection(b);
		}
		else
		{
			AddToSumSelection(b);
		}
	}

	public bool InSumSelection(BuildSizePart b)
	{
		return _selectedForSum.ContainsKey(b.Name);
	}

	void RemoveFromSumSelection(BuildSizePart b)
	{
		_selectedForSum.Remove(b.Name);
	}

	void AddToSumSelection(BuildSizePart b)
	{
		_selectedForSum.Add(b.Name, b);
	}

	double GetSizeOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			total += pair.Value.UsableSize;
		}
		return total;
	}

	public double GetPercentageOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			if (pair.Value.Percentage > 0)
			{
				total += pair.Value.Percentage;
			}
		}
		return total;
	}

	public string GetReadableSizeOfSumSelection()
	{
		return BuildReportUtil.MyFileSizeReadable( GetSizeOfSumSelection() );
	}

	public bool AtLeastOneSelectedForSum
	{
		get
		{
			return _selectedForSum.Count > 0;
		}
	}

	public int GetSelectedCount()
	{
		return _selectedForSum.Count;
	}

	public void ClearSelection()
	{
		_selectedForSum.Clear();
	}
}

[System.Serializable]
public class BuildInfo
{
	public string ProjectName;
	public string BuildType;

	public DateTime TimeGot;
	public string TimeGotReadable;

	public string GetTimeReadable()
	{
		if (!string.IsNullOrEmpty(TimeGotReadable))
		{
			return TimeGotReadable;
		}
		return TimeGot.ToString(BuildReport.TIME_OF_BUILD_FORMAT);
	}

	public BuildSizePart[] BuildSizes;
	public string TotalBuildSize;
	public string CompressedBuildSize;

	public BuildSizePart[] MonoDLLs;
	public BuildSizePart[] ScriptDLLs;

	public FileFilterGroup FileFilters;

	public AssetList UsedAssets;
	public AssetList UnusedAssets;


	// unity/os environment values at time of building
	public string EditorAppContentsPath;
	public string ProjectAssetsPath;
	public string BuildFilePath;
	public string[] ScenesIncludedInProject;
	public string[] PrefabsUsedInScenes;
	public StrippingLevel CodeStrippingLevel;
	public ApiCompatibilityLevel MonoLevel;


	// build report tool options values at time of building
	public bool IncludedSvnInUnused;
	public bool IncludedGitInUnused;



	string _savedPath;

	public string SavedPath { get{ return _savedPath; } }
	public void SetSavedPath(string val)
	{
		_savedPath = val;
	}


	public string GetDefaultFilename()
	{
		return ProjectName + "-" + BuildType + TimeGot.ToString("-yyyyMMMdd-HHmmss") + ".xml";
	}


	bool _refreshRequest;

	public void FlagOkToRefresh()
	{
		_refreshRequest = true;
	}

	public void FlagFinishedRefreshing()
	{
		_refreshRequest = false;
	}

	public bool RequestedToRefresh { get{ return _refreshRequest; } }


	public bool HasContents
	{
		get
		{
			return UsedAssets != null && UsedAssets.All != null && UsedAssets.All.Length > 0 && UnusedAssets != null;
		}
	}

	public void OnUnserialize()
	{
		if (HasContents)
		{
			for (int n = 0, len = UsedAssets.All.Length; n < len; ++n)
			{
				UsedAssets.All[n].DerivedSize = BuildReportUtil.GetApproxSizeFromString(UsedAssets.All[n].Size);
			}

			UsedAssets.AssignPerCategoryList( BuildReport.SegregateAssetSizesPerCategory(UsedAssets.All, FileFilters) );
			UnusedAssets.AssignPerCategoryList( BuildReport.SegregateAssetSizesPerCategory(UnusedAssets.All, FileFilters) );
		}
	}
}

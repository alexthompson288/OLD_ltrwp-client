using UnityEditor;
using UnityEngine;

/// <summary>
/// This class does nothing other than reverts changes we made to objects with the uSequencer back to their
/// original state, this is so we don't save our current properties, we want to save the default properties.
/// </summary>
#if (UNITY_3_5)
public class USAssetModificationProcessor : AssetModificationProcessor 
#else
public class USAssetModificationProcessor : UnityEditor.AssetModificationProcessor
#endif
{
	// Use this for initialization
	static void OnWillSaveAssets (string[] paths)
	{
        foreach(string path in paths)
        {
            if(path.Contains(".unity"))
            {
				USControl.RevertForSave();
			}
		}
	}
}

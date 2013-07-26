using UnityEngine;
using System.Collections;

public class USGameObjectUtils 
{
	public static void ToggleObjectActive(GameObject GO)
	{
#if (UNITY_3_5)
		GO.active = !IsObjectActive(GO);
#else
		GO.SetActive(!IsObjectActive(GO));
#endif
	}
	
	public static void SetObjectActive(GameObject GO, bool active)
	{
#if (UNITY_3_5)
		GO.active = active;
#else
		GO.SetActive(active);
#endif
	}
	
	public static bool IsObjectActive(GameObject GO)
	{
#if (UNITY_3_5)
		return GO.active;
#else
		return GO.activeSelf;
#endif
	}
}

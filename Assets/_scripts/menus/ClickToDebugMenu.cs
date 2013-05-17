using UnityEngine;
using System.Collections;

public class ClickToDebugMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touches.Length>0 || Input.GetMouseButton(0))
		{
			Application.LoadLevel("DebugMenu");
		}
	}
}

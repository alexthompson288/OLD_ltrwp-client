using UnityEngine;
using System.Collections;

public class debugmenu_click_fullscreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPress(bool isDown)
	{
		if(isDown)
		{
			Screen.fullScreen=!Screen.fullScreen;
		}
	}
}

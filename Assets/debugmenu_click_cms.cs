using UnityEngine;
using System.Collections;

public class debugmenu_click_cms : MonoBehaviour {

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
			Application.OpenURL(@"http://altitudeeducation.herokuapp.com/");
		}
	}
}

using UnityEngine;
using System.Collections;

public class press : MonoBehaviour {

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
			Debug.Log("button pressed");
		}
	}
}

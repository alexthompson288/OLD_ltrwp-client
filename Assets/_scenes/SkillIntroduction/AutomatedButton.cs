using UnityEngine;
using System.Collections;

public class AutomatedButton : MonoBehaviour {

	public Texture2D PressedState;
	public Texture2D UnpressedState;
	public bool isPressed=false;
	bool currentState=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isPressed!=currentState)
		{
			currentState=isPressed;

			if(!currentState)
				gameObject.GetComponent<OTSprite>().image=UnpressedState;
			else
				gameObject.GetComponent<OTSprite>().image=PressedState;
		}
	}
}

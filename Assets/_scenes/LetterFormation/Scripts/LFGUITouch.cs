using UnityEngine;
using System.Collections;

public class LFGUITouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	// Simple tap
	private void On_SimpleTap(Gesture gesture){
		
		if(gesture.pickObject!=gameObject)
			return;
		
	
		if(gesture.pickObject.name=="btnExit")
		{
			Application.LoadLevel ("ContentBrowser");
		}
	}
	
}

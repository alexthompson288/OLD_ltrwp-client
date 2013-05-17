using UnityEngine;
using System.Collections;

public class ResetClick : MonoBehaviour {
	
	Manager gameManager;
	
	// Use this for initialization
	void Start () {
	
		gameManager=(Manager)GameObject.Find("Main Camera").GetComponent(typeof(Manager));
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
	private void On_SimpleTap( Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject == gameObject)
			gameManager.StopGame();
	}
}

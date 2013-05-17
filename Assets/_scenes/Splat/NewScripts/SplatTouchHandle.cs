using UnityEngine;
using System.Collections;

public class SplatTouchHandle : MonoBehaviour {

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
		
		if(gesture.pickObject==null)return;
		
		if(gesture.pickObject.name=="btnReset")
		{
			Application.LoadLevel ("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btnSwitchScene")
		{
			PersistentObject thisPO=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
			
			if(thisPO.CurrentTheme=="underwater")
				thisPO.CurrentTheme="forest";
			else if(thisPO.CurrentTheme=="forest")
				thisPO.CurrentTheme="underwater";
			
			Application.LoadLevel(Application.loadedLevelName);
		}
			
	}
}

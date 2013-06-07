using UnityEngine;
using System.Collections;

public class MissingWordSentenceTouch : MonoBehaviour {

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

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			Debug.Log("got pickobject "+gesture.pickObject.name);
		}
	}
}

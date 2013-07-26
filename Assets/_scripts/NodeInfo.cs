using UnityEngine;
using System.Collections;

public class NodeInfo : MonoBehaviour {
	
	public int sessionID;
	public bool notInteractive;

	ContentBrowserTouch cbTouch;
	
	// Use this for initialization
	void Start () {
		cbTouch=GameObject.Find("Main Camera").GetComponent<ContentBrowserTouch>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
//		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;
//		EasyTouch.On_Drag -= On_Drag;	
	}
	
	void On_SimpleTap(Gesture gesture)
	{
		if(notInteractive)return;
		
		if(gesture.pickObject==gameObject && !cbTouch.funZoneVisible)
		{
			Debug.Log ("hit node with sessionid "+sessionID);

			GameManager.Instance.SessionMgr.StartSession(sessionID);
			GameManager.Instance.SessionMgr.LogSections();

			GameManager.Instance.SessionMgr.StartActivity();
		}
	}
}

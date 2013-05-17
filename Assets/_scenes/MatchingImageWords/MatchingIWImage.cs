using UnityEngine;
using System.Collections;

public class MatchingIWImage : MonoBehaviour {
	MatchingIWManager gameManager;
	public string MyWord;
	OTSprite myself;
	// Use this for initialization
	void Start () {
		myself=GetComponent<OTSprite>();
		gameManager=GameObject.Find ("Main Camera").GetComponent<MatchingIWManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_Drag -= On_Drag;
	}
	
	void On_Drag(Gesture gesture)
	{
		if(gesture.pickObject==gameObject)
		{
			gameManager.pickupObject=gesture.pickObject.transform;
			myself.position+=gesture.deltaPosition;
		}
	}
}

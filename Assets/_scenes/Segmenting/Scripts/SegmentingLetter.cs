using UnityEngine;
using System.Collections;

public class SegmentingLetter : MonoBehaviour {
	
	SegmentingManager gameManager;
	public SegmentingContainer MyMount;
	public bool Locked=false;
	OTTextSprite mySprite;

	// Use this for initialization
	void Awake () {
		gameManager=GameObject.Find("Main Camera").GetComponent<SegmentingManager>();
		mySprite=gameObject.GetComponent<OTTextSprite>();
		mySprite.spriteContainer=gameManager.LetterFont.GetComponent<OTSpriteAtlasCocos2DFnt>();
		mySprite.ForceUpdate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_Drag -= On_Drag;
	}
	
	// Simple tap
	private void On_Drag(Gesture gesture){
		if(Locked)return;
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
//			mySprite.position=new Vector2(gesture.position.x-(Screen.currentResolution.width/2), gesture.position.y-(Screen.currentResolution.height/2));
			mySprite.position=new Vector2(mySprite.position.x+gesture.deltaPosition.x,mySprite.position.y+gesture.deltaPosition.y);
			//mySprite.position=gesture.position;
						
		}
	}
	
	private void On_TouchUp(Gesture gesture){
		if(gesture.pickObject==gameObject){
			gameManager.CheckCurrentLetterForContainerDrop(gameObject.transform);
		}
	}
}

using UnityEngine;
using System.Collections;

public class WordBankManager : MonoBehaviour {
	
	Rect SegmentingFrame;
	public AudioClip WordClip;
	bool validHit=false;
	public Texture2D playButtonUnpressed;
	public Texture2D playButtonPressed;
	bool isPlaying=false;
	bool isTouching=false;
	
	// Use this for initialization
	void Start () {
		OTSprite Frame=GameObject.Find ("Frame").GetComponent<OTSprite>();
		SegmentingFrame=new Rect(Frame.position.x-(Frame.size.x/2),Frame.position.y-(Frame.size.y/2),Frame.size.x,Frame.size.y);
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying&&isPlaying)
		{
			isPlaying=false;
			OTSprite s=GameObject.Find ("btnPlayButton").GetComponent<OTSprite>();
			s.image=playButtonUnpressed;
		}
	}
	
	void OnEnable(){
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){	
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_TouchDown -= On_TouchDown;
	}
	
	void On_TouchDown(Gesture gesture){
		if(isTouching)return;
		isTouching=true;
		validHit=false;	
		
		if(gesture.pickObject!=null){
			
			if(gesture.pickObject.name=="btnPlayButton" && !isPlaying){
				audio.clip=WordClip;
				audio.Play();
				validHit=true;
				isPlaying=true;
				
				OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
				s.image=playButtonPressed;
			}
		}
	}
	
	void On_TouchUp(Gesture gesture){
		if(!SegmentingFrame.Contains(new Vector2(gesture.position.x-512,gesture.position.y-384)) && !validHit)
			Application.LoadLevel ("WordBank");
		
		isTouching=false;
	}
	
	void On_SimpleTap(Gesture gesture){
		validHit=false;
//		Debug.Log ("Gesture pos: "+new Vector2(gesture.position.x-512,gesture.position.y-384)+" rect "+SegmentingFrame);

		

	}
}

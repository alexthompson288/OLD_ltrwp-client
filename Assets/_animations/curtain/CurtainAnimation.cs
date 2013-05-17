using UnityEngine;
using System.Collections;

public class CurtainAnimation : MonoBehaviour {

	public bool playIdle;
	public bool playOpening;
	public bool playOpening2;
	public bool isOpen=false;
	OTAnimatingSprite myself;
	OTSprite ChildSprite;
	OTTextSprite ChildText;
	public Texture2D ChildSpriteTexture;
	public string ChildTextString;
	public AudioClip MyAudio;
	public OTSprite MySpotlight;
	bool playedSmoke=false;
	bool CountdownToReveal=false;
	float CountdownToRevealTime=2.0f;
	
	IntroducingPhonemeManager gameManager;
	
	string ChildType;
	
	// Use this for initialization
	void Start () {
		gameManager=GameObject.Find ("Main Camera").GetComponent<IntroducingPhonemeManager>();
		myself=gameObject.GetComponent<OTAnimatingSprite>();
		
		foreach(Transform t in transform.parent.transform)
		{
			if(t.gameObject.name.StartsWith("ChildSprite"))
				ChildSprite=t.gameObject.GetComponent<OTSprite>();
			
			else if(t.gameObject.name.StartsWith("ChildText"))
				ChildText=t.gameObject.GetComponent<OTTextSprite>();
			
		}
		
		if(ChildText!=null && ChildSprite==null)
			ChildType="TEXT";
		else if(ChildText==null && ChildSprite!=null)
			ChildType="IMAGE";
		else 
			ChildType="WAT";
		
//		Debug.Log("Started curtain. ChildType: "+ChildType);
		
		if(ChildType=="TEXT" && ChildTextString!=null)
			ChildText.text=ChildTextString;
		else if(ChildType=="IMAGE" && ChildSpriteTexture!=null)
			ChildText.image=ChildSpriteTexture;
		
		
		if(ChildText!=null)
			ChildText.visible=false;
		
		if(MySpotlight!=null)
			MySpotlight.visible=false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying && !isOpen)
		{
			if(playIdle)
			{
				SetAnimationIdle();
			}
			else if(playOpening){
				SetAnimationOpening();
			}
			else{
				SetAnimationIdle();
			}
		}
		
		if(!myself.isPlaying && playOpening2)
		{
			SetAnimationOpening();
		}
		else if(!myself.isPlaying && isOpen && ChildType=="TEXT" && !playedSmoke)
		{
			playedSmoke=true;
			CountdownToReveal=true;
			gameManager.Smoke.playSmoke1=true;
		}
		
		if(CountdownToReveal)
		{
			CountdownToRevealTime-=Time.deltaTime;
			
			if(CountdownToRevealTime<0)
			{
				ChildText.visible=true;
				MySpotlight.visible=true;
				CountdownToReveal=false;
			}
		}
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
	
	void On_SimpleTap(Gesture gesture){
		if(gesture.pickObject==gameObject&&!isOpen&&!gameManager.isReadingIntro)
			playOpening=true;
	}
	
	void SetNonePlaying()
	{
		playIdle=false;
		playOpening=false;
		playOpening2=false;
	}
	
	void SetAnimationIdle(){
		myself.PlayOnce("still");
	}
	
	void SetAnimationOpening()
	{
		if(ChildType=="TEXT"&&!gameManager.HasOpenedOutsideCurtains())return;
		if(ChildType=="TEXT")
			gameManager.exitCountdown=true;
		
		if(playOpening)
		{
			gameManager.PlayAudio(MyAudio);
			myself.PlayOnce("opening1");
			SetNonePlaying();
			playOpening2=true;
		}
		else if(playOpening2)
		{
			myself.PlayOnce ("opening2");
			SetNonePlaying();
		}
		isOpen=true;
	}
	
}

using UnityEngine;
using System.Collections;

public class ShieldTouch : MonoBehaviour {
	
	public Transform colourShield;
	public Transform colourText;
	public Transform Mnemonic;
	public AudioClip MyAudio;
	public string MyMnemonic;

	TrophyRoomManager gameManager;
	float timer=4.0f;
	bool countdown=false;
	
	// Use this for initialization
	void Start () {
		gameManager=GameObject.Find ("Main Camera").GetComponent<TrophyRoomManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(countdown)
		{
			timer-=Time.deltaTime;
			if(timer<0)
			{
				countdown=false;
				timer=4.0f;
				fadeMnemonicOut();
				
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
	
	void fadeMnemonicIn(){
		if(gameManager.PlayingAudio)return;
		
		countdown=true;
		OTSprite s=Mnemonic.gameObject.GetComponent<OTSprite>();
		OTTween mt=new OTTween(s,0.8f, OTEasing.Linear);
		mt.Tween("alpha", 1.0f);
		
		OTTextSprite t=colourText.gameObject.GetComponent<OTTextSprite>();
		OTTween mtt=new OTTween(t,0.5f, OTEasing.Linear);
		mtt.Tween("alpha", 0.0f);
		
		gameManager.PlayAudioClip(MyAudio);
	}
	
	void fadeMnemonicOut(){
		OTSprite s=Mnemonic.gameObject.GetComponent<OTSprite>();
		OTTween mt=new OTTween(s,0.5f, OTEasing.Linear);
		mt.Tween("alpha", 0.0f);
		
		OTTextSprite t=colourText.gameObject.GetComponent<OTTextSprite>();
		OTTween mtt=new OTTween(t,0.8f, OTEasing.Linear);
		mtt.Tween("alpha", 1.0f);
	}
	
	private void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject==gameObject && !gameManager.DisableTouches)
		{
			if(colourText!=null){
				OTTextSprite t=colourText.gameObject.GetComponent<OTTextSprite>();

				gameManager.CreateBigShield(t.text, MyMnemonic);
			}
		}
		// if(gesture.pickObject==gameObject)
		// {
		// 	if(Mnemonic!=null)
		// 	{
		// 		fadeMnemonicIn();
		// 	}
		// 	else{
		// 		if(colourShield==null)return;
		// 		OTSprite s=colourShield.gameObject.GetComponent<OTSprite>();
		// 		OTTween mt=new OTTween(s,0.5f, OTEasing.Linear);
		// 		mt.Tween("alpha", 1.0f);
			
		// 		OTTextSprite t=colourText.gameObject.GetComponent<OTTextSprite>();
		// 		OTTween mtt=new OTTween(t,0.5f, OTEasing.Linear);
		// 		mtt.Tween("tintColor", new Color(255.0f,255.0f,255.0f,255.0f));
				
		// 	}
		// }
	}
}

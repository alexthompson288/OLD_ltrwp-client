using UnityEngine;
using System.Collections;
using AlTypes;

public class CurtainAnimation : MonoBehaviour {

	public bool playIdle;
	public bool playOpening;
	public bool playOpening2;
	public bool isOpen=false;
	OTAnimatingSprite myself;
	OTSprite ChildSprite;
	OTSprite Mnemonic;
	OTTextSprite ChildText;
	public Texture2D ChildSpriteTexture;
	public AudioClip MyAudio;
	public OTSprite MySpotlight;
	bool playedSmoke=false;
	bool CountdownToReveal=false;
	float CountdownToRevealTime=2.0f;
	string MyImageName;
	bool playAudioSequence;
	ArrayList AudioClipList;
	int currentClip=0;
	float countdownToNextClip=0;
	string myPhoneme;
	string myGrapheme;
	string myMnemonic;
	bool disabled=false;

	
	IntroducingPhonemeManager gameManager;
	
	string ChildType;
	
	// Use this for initialization
	void Start () {
		gameManager=GameObject.Find ("Main Camera").GetComponent<IntroducingPhonemeManager>();
		myself=gameObject.GetComponent<OTAnimatingSprite>();
		PhonemeData pd=gameManager.GetCurrentPhoneme();
		foreach(Transform t in transform.parent.transform)
		{
			if(t.gameObject.name.StartsWith("ChildSprite"))
				ChildSprite=t.gameObject.GetComponent<OTSprite>();
			
			else if(t.gameObject.name.StartsWith("ChildText"))
				ChildText=t.gameObject.GetComponent<OTTextSprite>();
			else if(t.gameObject.name.StartsWith("Mnemonic"))
				Mnemonic=t.gameObject.GetComponent<OTSprite>();
			
		}

		
		if(ChildText!=null && ChildSprite==null)
			ChildType="TEXT";
		else if(ChildText==null && ChildSprite!=null)
			ChildType="IMAGE";
		else 
			ChildType="WAT";
		
//		Debug.Log("Started curtain. ChildType: "+ChildType);
		
		if(ChildType=="TEXT"){
			ChildText.text=pd.Phoneme;
			AudioClipList=new ArrayList();
		}
		else if(ChildType=="IMAGE"){
			MyImageName=gameManager.GetCurrentImage();

			ChildSprite.image=(Texture2D)Resources.Load("Images/word_images_png_150/_"+MyImageName+"_150");
			
			if(ChildSprite.image==null)
				Debug.Log("load fail for "+MyImageName);
			// ChildSprite.image=gameManager.GetCurrentImage();
		}
		
		
		if(ChildText!=null){
			ChildText.visible=false;
			string mnemonicName=pd.Mneumonic.Replace(" ", "_");
			string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+mnemonicName;
			Debug.Log("file path"+filePathMnemonic);
			myPhoneme=pd.Phoneme;
			myMnemonic=mnemonicName;
			myGrapheme=pd.Grapheme;
			Mnemonic.image=(Texture2D)Resources.Load(filePathMnemonic);
			Mnemonic.alpha=0;
			Mnemonic.visible=false;
		}

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
		else if(!myself.isPlaying && isOpen && ChildType=="IMAGE")
		{
			gameManager.disableTouches=false;
		}
		else if(!myself.isPlaying && isOpen && ChildType=="TEXT" && !playedSmoke)
		{
			playedSmoke=true;
			CountdownToReveal=true;
			gameManager.Smoke.playSmoke1=true;
			if(myGrapheme==null)myGrapheme=myPhoneme;
			gameManager.PlayAudio((AudioClip)Resources.Load("audio/benny_mnemonics_master/benny_mnemonic_"+myPhoneme+"_"+myGrapheme+"_"+myMnemonic));
			// AudioClip firstClip=(AudioClip)AudioClipList[0];
			// countdownToNextClip=firstClip.length;
			// gameManager.PlayAudio(firstClip);
			// playAudioSequence=true;
		}

		if(playAudioSequence)
		{
			countdownToNextClip-=Time.deltaTime;
			
			if(countdownToNextClip<0)
			{
				currentClip++;
				
				if(currentClip<AudioClipList.Count)
				{
					AudioClip thisClip=(AudioClip)AudioClipList[currentClip];
					countdownToNextClip=thisClip.length;
					gameManager.PlayAudio(thisClip);
				}
				else 
				{
					playAudioSequence=false;
				}

			}
		}
		
		if(CountdownToReveal)
		{
			CountdownToRevealTime-=Time.deltaTime;
			
			if(CountdownToRevealTime<0)
			{
				gameManager.ShowTrumpets();
				SwitchBetweenTextMnemonic();
				Mnemonic.visible=true;
				ChildText.visible=true;
				MySpotlight.visible=true;
				CountdownToReveal=false;
			}
		}
	}
	
	void SwitchBetweenTextMnemonic() {
		var config=new GoTweenConfig()
			.floatProp( "alpha", 1.0f )
			.setIterations ( -1, GoLoopType.PingPong );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(Mnemonic, 1.5f, config);

		Go.addTween(tween);

		var configt=new GoTweenConfig()
			.floatProp( "alpha", 0.0f )
			.setIterations ( -1, GoLoopType.PingPong );

		
		// Go.to(s, 0.3f, config );
		GoTween tweent=new GoTween(ChildText, 1.5f, configt);

		Go.addTween(tweent);
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
		if(disabled)return;
		if(gesture.pickObject==gameObject&&!isOpen&&!gameManager.isReadingIntro&&!gameManager.disableTouches)
		{
			if(ChildType=="IMAGE"){
				gameManager.PlayAudio((AudioClip)Resources.Load("audio/words/"+MyImageName));
			}
			else if(ChildType=="TEXT")
			{
				// string[] mnemonics=myMnemonic.Split('_');
				// Debug.Log("look for audio/benny_phonemes_master/benny_phoneme_"+myPhoneme+"_"+myGrapheme+"_"+myMnemonic);
				// AudioClipList.Add((AudioClip)Resources.Load("audio/benny_phonemes_master/benny_phoneme_"+myPhoneme+"_"+myGrapheme+"_"+myMnemonic));
				// AudioClipList.Add((AudioClip)Resources.Load("audio/words/for"));

				// foreach (string word in mnemonics)
				// {
				// 	Debug.Log("audio/words/"+word);
				//     AudioClipList.Add((AudioClip)Resources.Load("audio/words/"+word));
				// }
			}

			if(gameManager.audio.clip==null)
				Debug.Log("failed audio load for "+MyImageName);
			playOpening=true;
			gameManager.disableTouches=true;
			disabled=true;
		}
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
			// gameManager.PlayAudio(MyAudio);
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

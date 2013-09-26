using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class SplatManager : MonoBehaviour {
	
	public Transform SpherePrefab;

	public bool playing=false;
	
	public AudioClip audioIntro;
	public AudioClip successClip;
	public AudioClip[] audioLetters;
	public AudioClip CorrectHit;
	public AudioClip IncorrectHit;
	public AudioClip NoInteraction;
	public AudioClip HintSound;
	public AudioClip[] PipReactionsPositive;
	public AudioClip[] BennyReactionsNegative;
	public AudioClip[] BennyReactionsComplete;
	public OTTextSprite ScoreSprite;

	public bool ContainerHasStateB=false;
	public int NumberOfContainerVariants=0;
	
	ArrayList letters;
	
	public ArrayList currentBalls;
	ArrayList destroyLetters;
	public string currentLetter;
	
	float audioReqDelay=0.0f;
	bool playedAudioReq=true;
	
	float inactivetime=0;
	public int numberOfDifferentLetters=0;
	
	int expectedCorrectLetters=8;
	int currentCorrectLetters=0;
	
	public Transform sack;
	public Transform pip;
	public PipAnimation pipani;
	public bool IsRaining = false;	
	public bool IsTimerBasedGame = false;
	public float TimerReward = 2.0f;
	public float StartingTimer = 20.0f;
	private float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	private float TimeLeft;
	public OTSprite TimeLeftSprite;
	private float TimeLeftSpriteStartingScale;
	
	public GameObject CounterBarObject;
	public GameObject TimerBarObject;
	
	public ArrayList explosions;
	
//	public ParticleSystem Bubbles;
	float countdownToExit=4.0f;
	bool exitCountdown=false;
	
	public int initedBubbles=0;
	public int expectedInitBubbles=5;
	float timeToInit=0.0f;
	
	Vector3 sackDefaultScale;
	
	public int tapsSinceLastCorrect=0;
	int bookPressesSinceLastCorrect=0;
	bool warnedAboutCorrect=false;
	public bool allowInteraction=false;
	
	float timeToIntroductionAudio=0.0f;
	bool playedIntroductionAudio=false;
	
	public SplatSceneManager sceneMgr;
	
	PersistentObject PersistentManager;
	
	ArrayList targetPhonemes;
	ArrayList dummyPhonemes;
	PhonemeData currentPhonemeData;

	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;	
		EasyTouch.On_SimpleTap -= On_SimpleTap;
	}
	
	// Use this for initialization
	void Start () {
		TimeLeft = StartingTimer;
		TimeLeftSpriteStartingScale = TimeLeftSprite.size.y;
		ReadPersistentObjectSettings();
//		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", 0.1, "time", 6.0, "lookat", lookat));
		Application.targetFrameRate=60;
		sceneMgr=GetComponent<SplatSceneManager>();
		currentBalls=new ArrayList();
		explosions=new ArrayList();
		destroyLetters=new ArrayList();
		pipani=pip.GetComponent<PipAnimation>();
		Debug.Log("App datapath: "+ Application.dataPath);
		StartGame();
		
		if(IsTimerBasedGame)
			CounterBarObject.SetActive(false);
		else
			TimerBarObject.SetActive(false);
	}
	
	
	public void CreateNewPersistentObject()
	{
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			thisPO.GetComponent<PersistentObject>().CurrentTheme="Forest";
		}
	}
	
	void ReadPersistentObjectSettings(){
		
		CreateNewPersistentObject();
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
//		OTSprite scnBackground=GameObject.Find ("Background").GetComponent<OTSprite>();
		//scnBackground.image=(Texture2D)Resources.LoadAssetAtPath ("Assets/backgrounds/splat_"+PersistentManager.CurrentTheme+".png",typeof(Texture2D));
		
		Texture2D thisBkg=null;

		
		// if(PersistentManager.CurrentTheme=="underwater")
		// {
		// 	foreach(Transform t in underwaterPrefabs)
		// 	{
		// 		Instantiate(t);
		// 	}
		// }
	}
	
	void RemoveSpentExplosions()
	{
		ArrayList spent=new ArrayList();
		
		foreach(ParticleSystem part in explosions)
		{
			if(!part.isPlaying)
				spent.Add (part);
		}
		
		foreach(ParticleSystem part in spent)
		{
			Debug.Log ("Remove explode");
			explosions.Remove (part);
			GameObject.Destroy(part);
		}
		
	}
	
	public void GotLetter(Transform callingBall)
	{
		allowInteraction=false;
		tapsSinceLastCorrect=0;
		
		if(IsTimerBasedGame)
		{
			TimeLeft += TimerReward;
			TimeLeft = Mathf.Clamp(TimeLeft, 0.0f, StartingTimer);
		}
		
		currentCorrectLetters++;
		ScoreSprite.text = currentCorrectLetters.ToString();
		ScoreSprite.ForceUpdate();
		
		initedBubbles-=1;
		
		if(GameObject.Find ("ProgressBar"))
		{
			SplatTheRatProgress progress=GameObject.Find ("ProgressBar").GetComponent<SplatTheRatProgress>();
			progress.currentNumber=currentCorrectLetters;
		}
		
		Transform ballLetter=callingBall.GetComponent<SplatBally>().MyLetter;
		//play what they just selected
		PersistentManager.PlayAudioClip(CorrectHit);
		
		audio.clip=(AudioClip)Resources.Load("audio/benny_phonemes_master/benny_phoneme_"+currentPhonemeData.Phoneme+"_"+currentPhonemeData.Grapheme+"_"+currentPhonemeData.Mneumonic.Replace(" ","_"));
		//audio.clip=audioLetters[currentLetter];
		playedAudioReq=false;
		
		destroyLetters.Add (ballLetter);
		
		//find instance of selected in letters and remove
		// letters.Remove(currentLetter);
		
		currentBalls.Remove(callingBall);
		

		if(!IsTimerBasedGame && currentCorrectLetters==expectedCorrectLetters)
		{
			StopGame();
			return;
		}
		
		pipani.playIdleSet=false;
		pipani.SetNonePlaying();
		pipani.playPositive=true;
		
		getNextLetter();
		CreateNewSphere();
		
		audioReqDelay=1.5f;
		playedAudioReq=false;
		bookPressesSinceLastCorrect=0;
	}
	
	public void CreateNewSphere()
	{
		

		Transform newObject=(Transform)Instantiate(SpherePrefab);
		
		bool IsInOtherBallsPlace = true;
		int Count = 0;
		while(IsInOtherBallsPlace == true && Count < 20)
		{
			if(!IsRaining){
				newObject.position=new Vector3(Random.Range(-400, 259), Random.Range(-250, 250), 0);
			}else{
				newObject.position=new Vector3(Random.Range(-400, 0), 476.0f, 0);
				((SplatBally)newObject.GetComponent("SplatBally")).IsRaining = true;
			}
			if(!IsRaining && newObject.position.x > -37.0f && newObject.position.y < 53.0f)
			{
				// sphere is over pip
			}else{
				IsInOtherBallsPlace = false;
				
				foreach(Transform Ball in currentBalls)
				{
				//	Debug.Log("Checking ball positions");
					if((Ball.position - newObject.position).magnitude < 210.0f)
					{
						IsInOtherBallsPlace = true;
					}
				}
			}
			Count++;
		}
		
		// get found bool
		// ballscontain current balls on screen contain current target?
		// so always a target phoneme
		// audioletters = dummyletters
		//look to see if we have one of the current letter in play
		
		bool found=ballsContainLetter(currentLetter);
		
		if(!found)
		{
			((SplatBally)newObject.GetComponent("SplatBally")).currentLetter=currentLetter;	
			Debug.Log ("next letter: " + currentLetter + " was not found");
		}else{
			((SplatBally)newObject.GetComponent("SplatBally")).currentLetter=(string)dummyPhonemes[Random.Range(0,dummyPhonemes.Count-1)];
			Debug.Log ("next letter: " + currentLetter + " was found");
		}
		((SplatBally)newObject.GetComponent("SplatBally")).materialIndex=Random.Range(0,NumberOfContainerVariants);
					
		OTSprite s=newObject.GetComponent<OTSprite>();
		s.size=new Vector2(1.0f,1.0f);
		//s.position=new Vector2(-225,-135);
		s.ForceUpdate();		
		
		OTTween mt=new OTTween(s,1.5f, OTEasing.BounceOut);
		mt.Tween("size", s.imageSize);
		
		currentBalls.Add(newObject);
		initedBubbles+=1;
	}
	
	bool ballsContainLetter(string theLetter)
	{
		bool found=false;
		foreach(Transform bt in currentBalls)
		{
			if(((SplatBally)bt.GetComponent("SplatBally")).currentLetter==theLetter)
			{
				found=true;
				break;
			}
		}
		return found;
	}
	
	public void On_TouchDown(Gesture gesture)
	{
		inactivetime=0;
	}

	public void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name.StartsWith("BalAni"))
			{
				if(bookPressesSinceLastCorrect==0){
					PlayIntroductionAudio();
				}
				else if(bookPressesSinceLastCorrect>0){
					Debug.Log("GLOW THE RIGHT ONE");
					PersistentManager.PlayAudioClip(HintSound);
				}

				bookPressesSinceLastCorrect++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!playedIntroductionAudio)
			timeToIntroductionAudio-=Time.deltaTime;
		
		if(timeToIntroductionAudio<0&&!playedIntroductionAudio)
		{
			PlayIntroductionAudio();
			playedIntroductionAudio=true;
		}
		
		if(!warnedAboutCorrect&&tapsSinceLastCorrect>2)
		{
			warnedAboutCorrect=true;
			Debug.Log ("got 2 letters in a row wrong. do stuff");
		}
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;
		
		if(countdownToExit<0 && PersistentManager.Players==2)
			Application.LoadLevel(PersistentManager.ContentBrowserName);
		else if(countdownToExit<0 && PersistentManager.Players==1)
			GameManager.Instance.SessionMgr.CloseActivity();
				
		if(playing)
		{
			inactivetime+=Time.deltaTime;
//			Debug.Log(TimeLeft);
		}
		
		if(IsTimerBasedGame)
		{
			Vector2 tmpSize = TimeLeftSprite.size;
			tmpSize.y = TimeLeftSpriteStartingScale * (TimeLeft / StartingTimer);
			TimeLeftSprite.size = tmpSize;
			
			if(tmpSize.y < 0.7f)
			{
				Color WarningColor = new Color(1.0f, 0.35f, 0.35f, 1.0f);
				float KlaxonValue = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 2.5f)) * (1.0f - (TimeLeft / (StartingTimer * 0.7f)));
				
				TimeLeftSprite.tintColor = ( WarningColor * KlaxonValue) + (Color.white * (1.0f - KlaxonValue));
			}
			
			if(allowInteraction)
			{
				TimeLeft -= Time.deltaTime * TimerModifier;
				TimerModifier += TimerModifierIncrease * Time.deltaTime;
				
				if(TimeLeft < 0.0f)
				{
					TimeLeft = 0.0f;
					allowInteraction = false;
					StopGame();					
				}
			}
		}
	
		if(inactivetime>15.0f && allowInteraction)
		{
			inactivetime=0.0f;
			Invoke("PlayIntroductionAudio", 1.5f);
			pip.audio.clip=NoInteraction;
			pip.audio.Play();
		}

		if(EasyTouch.GetTouchCount()>0 && !playing && !exitCountdown)	
		{
			StartGame();
		}
		
		audioReqDelay-=Time.deltaTime;
		if(audioReqDelay<0 && !playedAudioReq && playing)
		{
			allowInteraction=true;
			//TODO: FIX AUDIO
			// audio.clip=audioLetters[currentLetter];
			audio.clip=(AudioClip)Resources.Load("audio/benny_phonemes_master/benny_phoneme_"+currentPhonemeData.Phoneme+"_"+currentPhonemeData.Grapheme+"_"+currentPhonemeData.Mneumonic.Replace(" ","_"));
			audio.Play();
			playedAudioReq=true;
			audioReqDelay=2.0f;
		}
		
		if(initedBubbles<expectedInitBubbles && playing)
		{
			timeToInit+=Time.deltaTime;
			
			if(timeToInit>2.0f)
			{
				CreateNewSphere();
				timeToInit=0.0f;
			}
			
		}
		
	}
	
	void StartGame() {

		playing=true;

		//what we would like
		targetPhonemes=new ArrayList();
		dummyPhonemes = new ArrayList();
		
		DataPhonemeData[] dpd=GameManager.Instance.GetTargetDataPhonemesForSection(1387);//GameManager.Instance.SessionMgr.CurrentTargetDataPhonemes;

		if(dpd!=null)
		{
			foreach(DataPhonemeData dp in dpd)
			{
				if(dp.IsTarget == true)
				{
					targetPhonemes.Add(dp.Phoneme);
					dummyPhonemes.Add(dp.Phoneme);
				}
				if(dp.IsDummy == true)
				{
					dummyPhonemes.Add(dp.Phoneme);
				}
			}
		}
		
		if(targetPhonemes.Count==0)
		{
			Debug.Log("No target phonemes found, supplementing basic ones");
			targetPhonemes.Add("z");
			targetPhonemes.Add("x");
			targetPhonemes.Add("w");
			targetPhonemes.Add("v");
			targetPhonemes.Add("q");
		}

		// //create array of required solutions
		// letters=new ArrayList();
		// for (int i=0; i<targetPhonemes.Count; i++)
		// {
		// 	letters.Add(targetPhonemes[i]);
		// 	letters.Add(targetPhonemes[i]);
		// 	letters.Add(targetPhonemes[i]);
		// }
		
		getNextLetter();
		
		timeToIntroductionAudio=1.5f;
		
		for(int i = 0; i < dummyPhonemes.Count; i++)
		{
			Debug.Log( "Phoneme data for dummy: " + dummyPhonemes[i]);	
		}
		for(int i = 0; i < targetPhonemes.Count; i++)
		{
			Debug.Log( "Phoneme data for target: " + targetPhonemes[i]);	
		}
		
	//	dummyPhonemes=GameManager.Instance.GetDistributedDataPoints("phoneme", 0.8f, 20);
	//	dummyPhonemes = new ArrayList();
		if(dummyPhonemes.Count==0)
		{
			Debug.Log("No dummy phonemes found, supplementing basic ones");
			dummyPhonemes.Add("l");
			dummyPhonemes.Add("m");
			dummyPhonemes.Add("n");
			dummyPhonemes.Add("o");
			dummyPhonemes.Add("p");


		}
		
		
	}
	
	public void PlayIntroductionAudio()
	{
		audio.clip=audioIntro;
		audio.Play();
		
		audioReqDelay=3.53f;
		playedAudioReq=false;
	}

	public void PlayPipPositiveHit() 
	{
		int thisIndex=Random.Range(0,PipReactionsPositive.Length);
		AudioClip ac=PipReactionsPositive[thisIndex];
		pip.audio.clip=ac;
		pip.audio.Play();
	}

	public void PlayBennyNegativeHit()
	{
		int thisIndex=Random.Range(0,BennyReactionsNegative.Length);
		AudioClip ac=BennyReactionsNegative[thisIndex];
		pip.audio.clip=ac;
		pip.audio.Play();
	}

	public void PlayBennyComplete()
	{
		int thisIndex=Random.Range(0,BennyReactionsComplete.Length);
		AudioClip ac=BennyReactionsComplete[thisIndex];
		PersistentManager.PlayAudioClip(ac);
	}

	public void PlayAudio(AudioClip thisClip)
	{
		audio.clip=thisClip;
		audio.Play();
	}
	
	public void StopGame() {
//		Bubbles.Stop();
		playing=false;
		inactivetime=1.0f;
		
		foreach(Transform t in currentBalls)
		{
			GameObject.Destroy(t.gameObject);
		}
		
		currentBalls.Clear();
		
		if(audio.isPlaying)audio.Stop();
		audio.clip=successClip;
		audio.Play();
		PlayBennyComplete();
		exitCountdown=true;
		pipani.playPositive2 = true;
	}
	
	void getNextLetter() {
		// random string out of target phonemes
		// currentLetter=(int)letters[Random.Range(0, letters.Count-1)];
		currentLetter=(string)targetPhonemes[Random.Range(0,targetPhonemes.Count-1)];
		currentPhonemeData=GameManager.Instance.GetPhonemeInfoForPhoneme(currentLetter);
		// currentLetter="a";
	}
	
	public void backpackSquashAndBounce(string part){
		
		
	    switch(part){
	
	        case "first":
				sackDefaultScale=sack.localScale;
				iTween.ScaleTo(sack.gameObject,new Vector3(sack.localScale.x,sack.localScale.y/2,sack.localScale.z),0.3f);
				iTween.MoveBy(sack.gameObject,iTween.Hash ("time",0.3,"y",50.0, "onCompleteTarget",gameObject, "onComplete","backpackSquashAndBounce", "onCompleteParams","second"));
	        break;
	        
	        case "second":
				iTween.ScaleTo(sack.gameObject,sackDefaultScale,0.3f);
				iTween.MoveBy(sack.gameObject,iTween.Hash ("time",0.3,"y",-50.0));
	        break;
	
	    }


	}
}

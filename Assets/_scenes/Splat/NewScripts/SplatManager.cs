using UnityEngine;
using System.Collections;
using AlTypes;

public class SplatManager : MonoBehaviour {
	
	public Transform SpherePrefab;
	public Transform VortexStart;
	public Transform VortexEnd;
	
	public Texture2D[] BackgroundStore;
	
	public Transform[] underwaterPrefabs;
	
	public bool playing=false;
	public Transform uiSplash;
	
	public AudioClip audioIntro;
	public AudioClip successClip;
	public AudioClip[] audioLetters;
	public AudioClip CorrectHit;
	public AudioClip IncorrectHit;

	public bool ContainerHasStateB=false;
	public int NumberOfContainerVariants=0;
	
	ArrayList letters;
	
	public ArrayList currentBalls;
	ArrayList destroyLetters;
	public int currentLetter;
	
	float audioReqDelay=0.0f;
	bool playedAudioReq=true;
	
	float inactivetime=0;
	public int numberOfDifferentLetters=0;
	
	int expectedCorrectLetters=8;
	int currentCorrectLetters=0;
	
	public Transform sack;
	public Transform pip;
	public PipAnimation pipani;
	
	public ArrayList explosions;
	
//	public ParticleSystem Bubbles;
	float countdownToExit=4.0f;
	bool exitCountdown=false;
	
	public int initedBubbles=0;
	public int expectedInitBubbles=5;
	float timeToInit=0.0f;
	
	Vector3 sackDefaultScale;
	
	public int tapsSinceLastCorrect=0;
	bool warnedAboutCorrect=false;
	public bool allowInteraction=false;
	
	float timeToIntroductionAudio=0.0f;
	bool playedIntroductionAudio=false;
	
	public SplatSceneManager sceneMgr;
	
	PersistentObject PersistentManager;
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;	
	}
	
	// Use this for initialization
	void Start () {
		
		ReadPersistentObjectSettings();
//		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", 0.1, "time", 6.0, "lookat", lookat));
		Application.targetFrameRate=60;
		sceneMgr=GetComponent<SplatSceneManager>();
		currentBalls=new ArrayList();
		explosions=new ArrayList();
		destroyLetters=new ArrayList();
		pipani=pip.GetComponent<PipAnimation>();
		Debug.Log("App datapath: "+Application.dataPath);
		StartGame ();
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

		
		if(PersistentManager.CurrentTheme=="underwater")
		{
			foreach(Transform t in underwaterPrefabs)
			{
				Instantiate(t);
			}
		}
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
		currentCorrectLetters++;
		initedBubbles-=1;
		
		SplatTheRatProgress progress=GameObject.Find ("ProgressBar").GetComponent<SplatTheRatProgress>();
		progress.currentNumber=currentCorrectLetters;
		
		
		Transform ballLetter=callingBall.GetComponent<SplatBally>().MyLetter;
		//play what they just selected
		PersistentManager.PlayAudioClip(CorrectHit);
		audio.clip=audioLetters[currentLetter];
		playedAudioReq=false;
		
		destroyLetters.Add (ballLetter);
		
		//find instance of selected in letters and remove
		letters.Remove(currentLetter);
		
		currentBalls.Remove(callingBall);
		
		if(currentCorrectLetters==expectedCorrectLetters)
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
	}
	
	public void CreateNewSphere()
	{
		Transform newObject=(Transform)Instantiate(SpherePrefab);
		
		newObject.position=new Vector3(226, 123, 0);
		
		//look to see if we have one of the current letter in play
		bool found=ballsContainLetter(currentLetter);
		
		
		((SplatBally)newObject.GetComponent("SplatBally")).materialIndex=Random.Range(0,NumberOfContainerVariants);
		
		if(!found) ((SplatBally)newObject.GetComponent("SplatBally")).letterIndex=currentLetter;
		
		else ((SplatBally)newObject.GetComponent("SplatBally")).letterIndex=Random.Range(0,audioLetters.Length-1);
			
		OTSprite s=newObject.GetComponent<OTSprite>();
		s.size=new Vector2(1.0f,1.0f);
		s.position=new Vector2(-225,-135);
		s.ForceUpdate();
		
		
		OTTween mt=new OTTween(s,1.5f, OTEasing.BounceOut);
		mt.Tween("size", s.imageSize);
		
		currentBalls.Add(newObject);
		initedBubbles+=1;
	}
	
	bool ballsContainLetter(int theLetter)
	{
		bool found=false;
		foreach(Transform bt in currentBalls)
		{
			if(((SplatBally)bt.GetComponent("SplatBally")).letterIndex==theLetter)
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
		
		if(countdownToExit<0)
			GameManager.Instance.SessionMgr.CloseActivity();
//			Application.LoadLevel(PersistentManager.ContentBrowserName);
				
		if(playing)
			inactivetime+=Time.deltaTime;
	
		if(inactivetime>15.0f && allowInteraction)
		{
			inactivetime=0.0f;
			PlayIntroductionAudio();
		}

			if(EasyTouch.GetTouchCount()>0 && !playing && !exitCountdown)	
			{
				StartGame();
			}
			
			audioReqDelay-=Time.deltaTime;
			if(audioReqDelay<0 && !playedAudioReq && playing)
			{
				allowInteraction=true;
				audio.clip=audioLetters[currentLetter];
				audio.Play();
				playedAudioReq=true;
				audioReqDelay=2.0f;
			}
		
		if(initedBubbles<expectedInitBubbles && playing)
		{
			timeToInit+=Time.deltaTime;
			
			if(timeToInit>1.5f)
			{
				CreateNewSphere();
				timeToInit=0.0f;
			}
			
		}
		
	}
	
	void StartGame() {

		playing=true;
		uiSplash.renderer.enabled=false;

		//create array of required solutions
		letters=new ArrayList();
		for (int i=0; i<numberOfDifferentLetters; i++)
		{
			letters.Add(i);
			letters.Add(i);
			letters.Add(i);
		}
		
		getNextLetter();
		
		timeToIntroductionAudio=5.0f;
		
	}
	
	public void PlayIntroductionAudio()
	{
		audio.clip=audioIntro;
		audio.Play();
		
		audioReqDelay=3.53f;
		playedAudioReq=false;
	}
	
	public void StopGame() {
//		Bubbles.Stop();
		playing=false;
//		uiSplash.renderer.enabled=true;
		inactivetime=1.0f;
		
		foreach(Transform t in currentBalls)
		{
			GameObject.Destroy(t.gameObject);
		}
		
		currentBalls.Clear();
		
		audio.clip=successClip;
		audio.Play();
		exitCountdown=true;
	}
	
	void getNextLetter() {
		currentLetter=(int)letters[Random.Range(0, letters.Count-1)];
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

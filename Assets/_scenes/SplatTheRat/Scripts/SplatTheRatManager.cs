using UnityEngine;
using System.Collections;
using AlTypes;

public class SplatTheRatManager : MonoBehaviour {
	
	public bool playing=false;
	
	public Transform[] Moles;
	public ArrayList ActiveMoles;
	float creationTime=3.5f;
	float removalTime=0.0f;
	float posYOffset=250.0f;
	
	float minCreate=0.5f;
	float maxCreate=2.0f;
	float minRemove=1.0f;
	float maxRemove=2.5f;

	float TimeSinceInteraction=0.0f;
	float IdleTimeOut=15.0f;
	
	OTSprite BackgroundCloud;
	
	public AudioClip[] VivCorrectReactions;
	public AudioClip[] BennyCompleteReactions;

	public AudioClip IntroductionClip;
	public AudioClip IntroductionClipKeyword;
	
	int lettersFound=0;
	int p2LettersFound=0;
	int lettersRequired=0;
	bool autoRemove=true;
	public Transform LetterFont;
	
	public ArrayList DummyLetters;
	public ArrayList CorrectLetters;
	public ArrayList AllLetters;
	
	public AudioClip PopUpSound;
	public AudioClip CorrectSound;
	public AudioClip IncorrectSound;
	public AudioClip SuccessClip;
	public AudioClip IdleAudio;
	public AudioClip IdleAudioKeyword;
	public AudioClip HintSound;
	public AudioClip Trumpets;
	
	public Transform WinText;
	
	public AudioClip[] LetterSounds;

	public Transform KeywordSign;
	bool ShouldFlashKeywordSign;
	float TimeToNextSignChange=0.6f;
	float TimeBetweenKeywordSignChanges=0.6f;
	float TimeSignShownFor=0.0f;
	float TimeSignShouldShowFor=5.0f;
	public Texture2D KeywordSignLit;
	public Texture2D KeywordSignUnlit;

	string GameType="phoneme";

	PhonemeData CurrentPhonemeData;
	
	bool exitCountdown;
	float countdownToExit=4.0f;
	bool hasPlayedIntroAudio=false;
	int bookPressesSinceLastCorrect=0;

	public Transform vivDance;
	public Transform vivExit;
	bool ShowingVivDance;
	bool ShowingVivExit;
	
//	Transform lastMole;
	
	PersistentObject PersistentManager;
	
	// Use this for initialization
	void Start () {
		
		ReadPersistentObjectSettings();
		PersistentManager.KeywordGame=true;
		// move the sign down
		PlayIntro();
		if(PersistentManager.KeywordGame)
		{
			ShowKeywordSign();
			ShowVivDance();
		}


		if(PersistentManager.Players==2)
		{
			OTSprite ProgressP2=GameObject.Find ("ProgressBarP2").GetComponent<OTSprite>();
			ProgressP2.position=new Vector2(-440, -85);
		}
		else
		{
			GameObject.Destroy(GameObject.Find("ProgressBarP2"));
		}
		
//		creationTime=Random.Range(minCreate,maxCreate);
		// DummyLetters=new ArrayList();
		CorrectLetters=new ArrayList();
		AllLetters=new ArrayList();
		ActiveMoles=new ArrayList();

		DataPhonemeData[] dpd=GameManager.Instance.SessionMgr.CurrentTargetDataPhonemes;

		if(dpd!=null)
		{
			foreach(DataPhonemeData dp in dpd)
			{
				CorrectLetters.Add(dp.Phoneme);
				CorrectLetters.Add(dp.Phoneme);
			}
		}

		if(CorrectLetters.Count==0)
		{
			CorrectLetters.Add("empty");
		}

		DummyLetters=GameManager.Instance.GetDistributedDataPoints(GameType, 0.8f, 20);

		if(DummyLetters.Count==0)
		{
			DummyLetters.Add("empty");
		}		
// 		for(int i=0;i<3;i++)
// 		{
// 			if(i==0){
// 				if(PersistentManager.KeywordGame)
// 					DummyLetters.Add("as");
// 				else
// 					DummyLetters.Add ("d");
// 				CorrectLetters.Add (PersistentManager.CurrentLetter);
// 			}
// 			else if(i==1){
// 				if(PersistentManager.KeywordGame)
// 					DummyLetters.Add("sat");
// 				else
// 					DummyLetters.Add ("m");
// //				CorrectLetters.Add ("a");				
// 				CorrectLetters.Add (PersistentManager.CurrentLetter);
// 			}
			
// 			else if(i==2){
// 				CorrectLetters.Add (PersistentManager.CurrentLetter);				
// 			}
// 		}
		
		for(int i=0;i<CorrectLetters.Count;i++)
		{
			AllLetters.Add (CorrectLetters[i]);
		}
		for(int i=0;i<DummyLetters.Count;i++)
		{
			AllLetters.Add (DummyLetters[i]);
		}
		
		lettersRequired=8;

		if(PersistentManager.KeywordGame)
			LetterFont=GameObject.Find("PipFont-20pt").transform;
		else
			LetterFont=GameObject.Find("Font").transform;
		
		// BackgroundCloud=GameObject.Find ("BackgroundCloud").GetComponent<OTSprite>();
		
//		CreateMole();
		
	}
	
	void ReadPersistentObjectSettings(){
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	

		if(PersistentManager.CurrentLetter==null)
			PersistentManager.CurrentLetter="a";
	}	

	public void CreateNewPersistentObject()
	{
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0){
			PersistentManager.Players=1;
			GameManager.Instance.SessionMgr.CloseActivity();
//			Application.LoadLevel(PersistentManager.ContentBrowserName);
		}
		
		if(ShouldFlashKeywordSign)
		{
			TimeSignShownFor+=Time.deltaTime;
			TimeToNextSignChange-=Time.deltaTime;

			if(TimeToNextSignChange<0)
			{
				if(KeywordSign.GetComponent<OTSprite>().image==KeywordSignUnlit)
					KeywordSign.GetComponent<OTSprite>().image=KeywordSignLit;
				else 
					KeywordSign.GetComponent<OTSprite>().image=KeywordSignUnlit;

				TimeToNextSignChange=TimeBetweenKeywordSignChanges;
			}

			if(TimeSignShownFor>TimeSignShouldShowFor)
			{
				HideKeywordSign();
			}
		}

		if(ShowingVivDance)
		{
			if(vivDance.GetComponent<ALVideoTexture>().hasFinished && playing)
				ShowVivExit();
		}
		else if(ShowingVivExit)
		{
			if(vivExit.GetComponent<ALVideoTexture>().hasFinished)
				HideVivExit();
		}

		if(playing){
			TimeSinceInteraction+=Time.deltaTime;
			creationTime-=Time.deltaTime;
			
			// BackgroundCloud.position=new Vector2(BackgroundCloud.position.x-0.5f,BackgroundCloud.position.y);
			
			// if(BackgroundCloud.position.x<-995.0f)
			// 	BackgroundCloud.position=new Vector2(995.0f,BackgroundCloud.position.y);
			
			if(TimeSinceInteraction>IdleTimeOut)
			{
				// PersistentManager.PlayAudioClip(IdleAudio);
				PlayIdle();
			}

			if(creationTime<0)
			{
				if(!hasPlayedIntroAudio)
				{
					PlayCorrectAudioClip(PersistentManager.CurrentLetter);
					hasPlayedIntroAudio=true;
				}
				CreateMole();
			}
		}
	}

	void ShowVivDance()
	{
		ShowingVivDance=true;
		vivDance.gameObject.SetActive(true);
	}

	void ShowVivExit()
	{
		ShowingVivDance=false;
		ShowingVivExit=true;
		vivDance.gameObject.SetActive(false);	
		vivExit.gameObject.SetActive(true);
	}

	void HideVivExit()
	{
		ShowingVivExit=false;
		vivExit.gameObject.SetActive(false);
	}

	void ShowKeywordSign()
	{
		KeywordSign.gameObject.SetActive(true);
		TimeSignShownFor=0.0f;

		foreach(Transform t in KeywordSign)
		{
			if(t.name=="Word")
			{
				t.GetComponent<OTTextSprite>().text=PersistentManager.CurrentLetter;
				t.GetComponent<OTTextSprite>().depth=-150;

			}
		}

		OTSprite kwSign=KeywordSign.GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(246.0f, 246.0f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(kwSign, 0.8f, config);
		tween.setOnCompleteHandler(c => FlashKeywordSign());
		Go.addTween(tween);
	}

	void HideKeywordSign() 
	{
		ShouldFlashKeywordSign=false;
		OTSprite kwSign=KeywordSign.GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(246.0f, 640.0f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(kwSign, 0.8f, config);
		tween.setOnCompleteHandler(c => DisableKeywordSign());
		Go.addTween(tween);		
	}

	void DisableKeywordSign()
	{
		KeywordSign.gameObject.SetActive(false);
	}

	void FlashKeywordSign()
	{
		ShouldFlashKeywordSign=true;
	}

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

	public void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name.StartsWith("BalAni"))
			{
				if(bookPressesSinceLastCorrect==0){
					if(PersistentManager.KeywordGame)
					{
						ShowKeywordSign();
					}
					else 
					{
						Debug.Log("first book hit");
					}
				}
				else if(bookPressesSinceLastCorrect>0){
					Debug.Log("GLOW THE RIGHT ONE");
					PersistentManager.PlayAudioClip(HintSound);
				}

				bookPressesSinceLastCorrect++;
			}
		}
	}
	
	void CreateMole()
	{
		creationTime=Random.Range (minCreate,maxCreate);
		CreateNewMole();
	}
	
	void CreateNewMole()
	{
		if(!playing)return;
		
		int playerID=1;
		
		if(PersistentManager.Players==2)
			playerID=Random.Range (1,3);
		
		int moleIndex=0;
		
		// NEW BIT HERE
		
		if(ActiveMoles.Count==Moles.Length)return;
		
		while(ActiveMoles.Contains(moleIndex))
		{
			moleIndex=Random.Range (0,Moles.Length);	
		}
		
		Transform CreateMole=Moles[moleIndex];
		
		ActiveMoles.Add (moleIndex);
		
		
		// END NEW BIT
		
		int letterIndex=Random.Range (0,AllLetters.Count);
		
		Transform lastMole=(Transform)Instantiate(CreateMole);
		MoleTouch lastMoleTouch=lastMole.GetComponent<MoleTouch>();
		MoleAnimation lastMoleAni=lastMole.GetComponent<MoleAnimation>();
		
		lastMoleAni.Player=playerID;
		lastMoleTouch.RemoveTime=Random.Range (minRemove,maxRemove);
		lastMoleTouch.IndexNumber=moleIndex;
		lastMoleTouch.bigSign=PersistentManager.KeywordGame;
		
		foreach(Transform c in lastMole.transform)
		{
			if(c.gameObject.name=="MyLetter")
			{
				OTTextSprite txt=c.GetComponent<OTTextSprite>();
				txt.spriteContainer=LetterFont.GetComponent<OTSpriteAtlasCocos2DFnt>();
				txt.text=(string)AllLetters[letterIndex];
				txt.depth=-1;
				lastMole.GetComponent<MoleTouch>().myLetter=txt.text;
				txt.ForceUpdate();
				break;
			}
		}
		
		
		PlayPopup();
	}	
	
	public void RemoveActiveMole(int thisIndex)
	{
		if(ActiveMoles.Contains (thisIndex))
			ActiveMoles.Remove (thisIndex);
		
	}
	
	void StartPlaying()
	{
		playing=true;	
	}
	
	void StopPlaying()
	{
		playing=false;
		audio.clip=SuccessClip;
		audio.Play ();
//		RemoveLastMole();
		exitCountdown=true;
		
		PlayBennyComplete();

		if(PersistentManager.Players==2)
		{	
//			GameObject newlabel=new GameObject("WinLabel");
//			newlabel.AddComponent<OTTextSprite>();
			Transform newLabel=(Transform)Instantiate(WinText);
			OTTextSprite lbl=newLabel.GetComponent<OTTextSprite>();
//			OTTextSprite label=OT.CreateObject(OTObjectType.TextSprite).GetComponent<OTTextSprite>();
//			label.position=new Vector2(0,0);
			newLabel.parent=GameObject.Find ("layer-4").transform;
			lbl.tintColor=new Color(0,0,0,1);
//			label.depth=-20;
//			label.spriteContainer=LetterFont.GetComponent<OTSpriteAtlasCocos2DFnt>();
//			Debug.Log ("Got past setting layer and font");
			
			Debug.Log ("p1 score "+lettersFound+" p2 score "+p2LettersFound+" req score "+lettersRequired);
			
			if(lettersFound==lettersRequired)
				lbl.text="One Wins";
			
			if(p2LettersFound==lettersRequired)
				lbl.text="Two Wins";
			
			
		}
	}
	
	public void PlayIntro(){
		if(PersistentManager.KeywordGame)
		{
			audio.clip=IntroductionClipKeyword;
		}
		else 
		{
			audio.clip=IntroductionClip;
		}
		
		audio.Play();
		hasPlayedIntroAudio=false;
		creationTime=audio.clip.length;
	}
	
	void PlayIdle(){
		if(PersistentManager.KeywordGame)
		{
			audio.clip=IdleAudio;
		}
		else 
		{
			audio.clip=IdleAudioKeyword;
		}

		audio.Play();
		TimeSinceInteraction=0.0f;
	}

	void PlayPopup(){
		if(hasPlayedIntroAudio){
			audio.clip=PopUpSound;
			audio.Play();
		}
	}
	
	void PlayCorrectAudioClip(string letter)
	{
		if(PersistentManager.KeywordGame)
		{
			PersistentManager.PlayAudioClip((AudioClip)Resources.Load("audio/words/"+PersistentManager.CurrentLetter.ToLower()));
		}
		else
		{
			CurrentPhonemeData=GameManager.Instance.GetPhonemeInfoForPhoneme(letter);
			PersistentManager.PlayAudioClip((AudioClip)Resources.Load("audio/benny_phonemes_master/benny_phoneme_"+CurrentPhonemeData.Phoneme+"_"+CurrentPhonemeData.Grapheme+"_"+CurrentPhonemeData.Mneumonic.Replace(" ","_")));
		}

		// foreach(AudioClip a in LetterSounds)
		// {
		// 	if(a.name==letter)
		// 	{
		// 		PersistentManager.PlayAudioClip(a);
		// 		break;
		// 	}
		// }
	}

	public void PlayVivCorrect()
	{
		AudioClip ac=(AudioClip)VivCorrectReactions[Random.Range(0,VivCorrectReactions.Length)];
		PersistentManager.PlayAudioClip(ac);
	}

	public void PlayBennyComplete()
	{
		AudioClip ac=(AudioClip)BennyCompleteReactions[Random.Range(0,BennyCompleteReactions.Length)];
		PersistentManager.PlayAudioClip(ac);
	}
	
	public bool CorrectLetterOnMole(string letter)
	{
		return CorrectLetterOnMole(letter, 1);
	}
	
	public bool CorrectLetterOnMole(string letter, int playerID)
	{	
		if(CorrectLetters.Contains(letter)){
			PlayCorrectAudioClip(letter);
			
			Debug.Log ("check correct letter for player "+playerID);
			
			if(playerID==1){
				lettersFound++;
				SplatTheRatProgress progress=GameObject.Find ("ProgressBar").GetComponent<SplatTheRatProgress>();
				progress.currentNumber=lettersFound;
			}
			else if(playerID==2)
			{
				p2LettersFound++;
				SplatTheRatProgress progress=GameObject.Find ("ProgressBarP2").GetComponent<SplatTheRatProgress>();
				progress.currentNumber=p2LettersFound;
			}
			if(lettersFound==lettersRequired||p2LettersFound==lettersRequired)
				StopPlaying();
			int bookPressesSinceLastCorrect=0;
			GameManager.Instance.LogDataPoint(GameType, PersistentManager.CurrentLetter, "1");
			return true;	
		}
		else{
			GameManager.Instance.LogDataPoint(GameType, PersistentManager.CurrentLetter, "-1");
			GameManager.Instance.LogDataPoint(GameType, letter, "-1");
			return false;
		}
	}

	void On_TouchDown(Gesture gesture)
	{
		TimeSinceInteraction=0.0f;
	}
}


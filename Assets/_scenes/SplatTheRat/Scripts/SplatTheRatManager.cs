using UnityEngine;
using System.Collections;

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
	
	OTSprite BackgroundCloud;
	
	public AudioClip[] VivCorrectReactions;

	public AudioClip IntroductionClip;
	
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
	
	public Transform WinText;
	
	public AudioClip[] LetterSounds;
	
	bool exitCountdown;
	float countdownToExit=4.0f;
	bool hasPlayedIntroAudio=false;
	
//	Transform lastMole;
	
	PersistentObject PersistentManager;
	
	// Use this for initialization
	void Start () {
		
		ReadPersistentObjectSettings();
		
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
		DummyLetters=new ArrayList();
		CorrectLetters=new ArrayList();
		AllLetters=new ArrayList();
		ActiveMoles=new ArrayList();
		
		for(int i=0;i<3;i++)
		{
			if(i==0){
				if(PersistentManager.KeywordGame)
					DummyLetters.Add("as");
				else
					DummyLetters.Add ("d");
				CorrectLetters.Add (PersistentManager.CurrentLetter);
			}
			else if(i==1){
				if(PersistentManager.KeywordGame)
					DummyLetters.Add("sat");
				else
					DummyLetters.Add ("m");
//				CorrectLetters.Add ("a");				
				CorrectLetters.Add (PersistentManager.CurrentLetter);
			}
			
			else if(i==2){
				CorrectLetters.Add (PersistentManager.CurrentLetter);				
			}
		}
		
		for(int i=0;i<CorrectLetters.Count;i++)
		{
			AllLetters.Add (CorrectLetters[i]);
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
		
		BackgroundCloud=GameObject.Find ("BackgroundCloud").GetComponent<OTSprite>();
		
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
	
	// Update is called once per frame
	void Update () {
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0){
			PersistentManager.Players=1;
			GameManager.Instance.SessionMgr.CloseActivity();
//			Application.LoadLevel(PersistentManager.ContentBrowserName);
		}
		
		if(playing){
			creationTime-=Time.deltaTime;
			
			BackgroundCloud.position=new Vector2(BackgroundCloud.position.x-0.5f,BackgroundCloud.position.y);
			
			if(BackgroundCloud.position.x<-995.0f)
				BackgroundCloud.position=new Vector2(995.0f,BackgroundCloud.position.y);
			
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
		audio.clip=IntroductionClip;
		audio.Play();
		hasPlayedIntroAudio=false;
		creationTime=4.0f;
	}
	
	void PlayPopup(){
		if(hasPlayedIntroAudio){
			audio.clip=PopUpSound;
			audio.Play();
		}
	}
	
	void PlayCorrectAudioClip(string letter)
	{
		foreach(AudioClip a in LetterSounds)
		{
			if(a.name==letter)
			{
				PersistentManager.PlayAudioClip(a);
				break;
			}
		}
	}

	public void PlayVivCorrect()
	{
		AudioClip ac=(AudioClip)VivCorrectReactions[Random.Range(0,VivCorrectReactions.Length)];
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

			return true;	
		}
		else{
			return false;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class SegmentingManager : MonoBehaviour {
	
	// game state
	public int EnabledContainerIndex=0;
	bool showingLetters;
	
	// game parameters
	public bool ShowContainerButtons;
	public bool StartLettersMounted;
	public bool CanReuseSoundButtons;
	public bool HighlightLettersFromSoundButtons;
	
	
	// audio
	public AudioClip IntroductionClip;
	public AudioClip SuccessClip;
	public AudioClip HitClip;
	public AudioClip MountClip;
	public AudioClip WrongMountClip;
	
	// game requirements
	public string[] LettersToUse;
	public string[] CorrectLetters;
	public AudioClip[] LetterAudio;
	public Transform LetterPrefab;
	public Transform LetterFont;
	public Transform ContainerPrefab;
	public Transform containerBtnPrefab;
	public ArrayList CreatedLetters;
	public ArrayList CreatedContainers;

	public float scaffoldStartXPos=-118.0f;
	public float scaffoldStartYPos=-35.0f;
	public float letterStartXPos=-180.0f;
	public float letterStartYPos=238.0f;
	public float spaceFor1Letter=60;
	public float spaceFor2Letters=200;
	public float spaceFor3Letters=240;
	public Vector2 defaultContainerSize=new Vector2(128.0f,128.0f);
	
	
	
	// return to contentbrowser vars
	bool exitCountdown=false;
	float countdownToExit=4.0f;
	
	// persistent gameobject
	PersistentObject PersistentManager;
	
	// Use this for initialization
	void Start () {
		Application.targetFrameRate=60;
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}
		
		CreatedLetters=new ArrayList();
		CreatedContainers=new ArrayList();
		
		if(Application.loadedLevelName=="WordBank-Scaffold"||Application.loadedLevelName=="WordLadder")
		{
				
			GameManager cmsLink=GameManager.Instance;
			
			List<String> letters=cmsLink.GetSortedPhonemesForWord(PersistentManager.WordBankWord);
			
			CorrectLetters=new string[letters.Count];
			
			int curLetter=0;
			
			for(int i=0;i<CorrectLetters.Length;i++)
			{
				CorrectLetters[i]=letters[curLetter];
				curLetter++;
			}
			
			LettersToUse=CorrectLetters;
		}
		
		StartGame();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0)
			Application.LoadLevel(PersistentManager.ContentBrowserName);
		
		if(EvalProblem() && !exitCountdown)
		{
			exitCountdown=true;
			audio.clip=SuccessClip;
			audio.Play ();
		}
		
		if(!showingLetters && EnabledContainerIndex==CreatedContainers.Count)
			ShowLetters();
	}
	
	void StartGame () {
		
		if(StartLettersMounted)
			ShowContainerButtons=false;
		if(ShowContainerButtons)
			StartLettersMounted=false;
		
		// setup our scene now
		
		float contXPos=scaffoldStartXPos;
		
		Debug.Log("contXPos is "+scaffoldStartXPos);

		if(CorrectLetters.Length==4)
			contXPos-=80.0f;
		else if(CorrectLetters.Length==5)
			contXPos-=160.0f;
		
		float contYPos=scaffoldStartYPos;
		float letXPos=letterStartXPos;
		float letYPos=letterStartYPos;

		bool is1Letter=false;
		bool is2Letters=false;
		bool is3Letters=false;

		bool isInDigraph=false;
		SegmentingContainer StartDigraph=null;
		
		for(int i=0;i<CorrectLetters.Length;i++)
		{
			Transform cont=(Transform)Instantiate(ContainerPrefab);
			SegmentingContainer contprefs=cont.GetComponent<SegmentingContainer>();
			OTSprite scont=cont.GetComponent<OTSprite>();
			scont.size=defaultContainerSize;
			contprefs.ExpectedLetter=LettersToUse[i];
			contprefs.AudioLetter=LettersToUse[i];

			Debug.Log("scont size "+scont.size);

			if(contprefs.ExpectedLetter.Length==1 || contprefs.ExpectedLetter.Contains("-")){
				is1Letter=true;
				contXPos+=(spaceFor1Letter/2);
				// scont.size=new Vector2(scont.size.x, scont.size.y);
			}
			else if(contprefs.ExpectedLetter.Length==2){
				is2Letters=true;
				contXPos+=(spaceFor2Letters/2);
				// scont.size=new Vector2(scont.size.x+60, scont.size.y);
				contprefs.isMultiPartLetter=true;
			}
			else if(contprefs.ExpectedLetter.Length==3 && !contprefs.ExpectedLetter.Contains("-")){
				is3Letters=true;
				contXPos+=(spaceFor3Letters/2);
				// scont.size=new Vector2(scont.size.x+90, scont.size.y);
				contprefs.isMultiPartLetter=true;
			}


			scont.position=new Vector2(contXPos, contYPos);

			if(contprefs.ExpectedLetter.Contains("-") && !isInDigraph)
			{
				StartDigraph=contprefs;
				contprefs.ExpectedLetter=contprefs.ExpectedLetter[0].ToString();
				contprefs.AudioLetter=contprefs.ExpectedLetter;
				contprefs.isSplitDigraph=true;
				isInDigraph=true;
			}
			else if(contprefs.ExpectedLetter.Contains("-") && isInDigraph)
			{
				contprefs.firstDigraphPart=StartDigraph	;
				contprefs.AudioLetter=contprefs.ExpectedLetter[0].ToString();
				contprefs.ExpectedLetter=contprefs.ExpectedLetter[contprefs.ExpectedLetter.Length-1].ToString();
				contprefs.isSplitDigraph=true;
				isInDigraph=false;
			}
			
			if(ShowContainerButtons){
				contprefs.ContainerEnabled=false;
				contprefs.HiddenContainer=true;
				
				if(i==0)
					contprefs.HiddenButton=false;
				else
					contprefs.HiddenButton=true;
			}
			else if(StartLettersMounted)
			{
				contprefs.ContainerEnabled=false;
				contprefs.HiddenButton=false;
				contprefs.HiddenContainer=false;
			}
			else
			{
				contprefs.ContainerEnabled=true;
				contprefs.HiddenContainer=false;
				contprefs.HiddenButton=true;
			}
			
			contprefs.CanReuseButton=CanReuseSoundButtons;
			contprefs.HighlightLettersFromButton=HighlightLettersFromSoundButtons;
			
			CreatedContainers.Add (cont);
			
			if(is1Letter)
				contXPos+=(spaceFor1Letter/2);
			else if(is2Letters)
				contXPos+=(spaceFor2Letters/2);
			else if(is3Letters)
				contXPos+=(spaceFor3Letters/2);
			
			is1Letter=false;
			is2Letters=false;
			is3Letters=false;
		}
		
		for(int i=0;i<LettersToUse.Length;i++){
			Transform let=(Transform)Instantiate(LetterPrefab);
			OTTextSprite slet=let.GetComponent<OTTextSprite>();
			slet.spriteContainer=LetterFont.GetComponent<OTSpriteAtlasCocos2DFnt>();
			slet.text=LettersToUse[i];

			if(slet.text.Contains("-") && !isInDigraph)
			{
				slet.text=slet.text[0].ToString();
				isInDigraph=true;
			}
			else if(slet.text.Contains("-") && isInDigraph)
			{
				slet.text=slet.text[slet.text.Length-1].ToString();	
				isInDigraph=false;
			}
			
			if(!StartLettersMounted){
				slet.position=new Vector2(letXPos, letYPos);
			}
			else{
				Transform t=(Transform)CreatedContainers[i];
				OTSprite s=t.GetComponent<OTSprite>();
				SegmentingContainer contpref=t.GetComponent<SegmentingContainer>();
				contpref.ExpectedLetter=slet.text;
				contpref.MountedLetter=let;
				
				slet.position=s.position;
				slet.GetComponent<SegmentingLetter>().Locked=true;
			}
			
			if(ShowContainerButtons)
				slet.visible=false;
			else if(StartLettersMounted)
				slet.visible=true;
			else
				slet.visible=true;
			
			
			slet.ForceUpdate();
			
			CreatedLetters.Add (let);
			
			letXPos+=100;	
		}
			
//		Transform firstCont=(Transform)CreatedContainers[0];
//		firstCont.GetComponent<SegmentingContainer>().ShowButton();
	
	}
	
	void ShowLetters() {
		
		if(showingLetters)return;
		
		showingLetters=true;
		
		for(int i=0;i<CreatedLetters.Count;i++)
		{
			Transform let=(Transform)CreatedLetters[i];
			OTTextSprite slet=let.GetComponent<OTTextSprite>();
			slet.visible=true;
		}
	}
	
	public void StopGame () {
	
		
	}

	public Transform CreateNewButton(Vector3 position, Quaternion rotation)
	{
		Debug.Log("my instans pos"+position);
		return (Transform)Instantiate(containerBtnPrefab, position, rotation);
	}
	
	public void CheckCurrentLetterForContainerDrop(Transform thisLetter)
	{
		OTTextSprite letter=thisLetter.GetComponent<OTTextSprite>();
		SegmentingLetter letterPref=thisLetter.GetComponent<SegmentingLetter>();
		
		
		for(int i=0;i<CreatedContainers.Count;i++)
		{
			Transform thisCont=(Transform)CreatedContainers[i];
			OTSprite s=thisCont.GetComponent<OTSprite>();
			SegmentingContainer contPref=thisCont.GetComponent<SegmentingContainer>();
			
			float adjPosX=s.position.x-(s.size.x/2);
			float adjPosY=s.position.y-(s.size.y/2);
			
			Rect thisContRect=new Rect(adjPosX,adjPosY,s.size.x,s.size.y);
			
			if(thisContRect.Contains(letter.position)&&letter.text==contPref.ExpectedLetter&&contPref.MountedLetter==null&&contPref.ContainerEnabled)
			{
				letterPref.MyMount=contPref;
				letter.position=s.position;
				contPref.MountedLetter=thisLetter;
				contPref.audio.clip=MountClip;
				contPref.audio.Play ();
				break;
			}
			else if(!thisContRect.Contains(letter.position)&&letterPref.MyMount==contPref&&contPref.MountedLetter==thisLetter)
			{
				letterPref.MyMount=null;
				contPref.MountedLetter=null;
			}
			else if(thisContRect.Contains(letter.position)&&letter.text!=contPref.ExpectedLetter)
			{
				OTTextSprite thisLetterSprite=thisLetter.GetComponent<OTTextSprite>();
				
				OTTween mt=new OTTween(thisLetterSprite,0.5f, OTEasing.BounceInOut);
				mt.Tween("position", new Vector2(transform.position.x,transform.position.y+200.0f));
				contPref.audio.clip=WrongMountClip;
				contPref.audio.Play ();
			}
		}
	}
	
	bool EvalProblem()
	{
		if(StartLettersMounted)return false;
		
		int correctCount=0;
		
		for(int i=0;i<CreatedContainers.Count;i++)
		{
			Transform thisCont=(Transform)CreatedContainers[i];
			SegmentingContainer contPref=thisCont.GetComponent<SegmentingContainer>();
			
			if(contPref.MountedLetter!=null)
				correctCount++;
		}
		
		if(correctCount==CorrectLetters.Length)
			return true;
		else
			return false;
	}
	
	public void ShowNextButton()
	{
		EnabledContainerIndex++;
		
		if(EnabledContainerIndex<=CreatedContainers.Count-1){
		
			Transform NextCont=(Transform)CreatedContainers[EnabledContainerIndex];
			NextCont.GetComponent<SegmentingContainer>().ShowButton();
		}
	}
	
	public void PlayIntro()
	{
		
	}
	
	public AudioClip AudioLetter(string thisLetter)
	{
		for(int i=0;i<LetterAudio.Length;i++)
		{
			if(LetterAudio[i].name==thisLetter)
			{
				return LetterAudio[i];	
			}
		}
		
		return null;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using AlTypes;

public class CorrectPathManager : MonoBehaviour {

	bool DisableTap=false;
	int currentIndex=0;
	int totalScalableSections=3;
	float sectionHeight=768.0f;
	public OTSprite platform;
	public OTSprite[] SceneImages;
	PipAnimation Pip;
	DataWordData[] datawords;
	public string CurrentWord;
	public string DummyWord;
	List<string> CorrectWords = new List<string>();
	List<string>  DummyWords = new List<string>();
	int currentCorrectIndex=0;
	int currentDummyIndex=0;
	public OTAnimation reactions;
	public OTAnimatingSprite pip;
	
	public Transform AnswersSection;

	public AudioClip completeClip;

	public AudioClip[] audioWords;

	// Use this for initialization
	void Start () {
		
		datawords=GameManager.Instance.SessionMgr.CurrentDataWords;
//		CorrectWords=new ArrayList();
	//	DummyWords=new ArrayList();
		
		// datawords appears to be broken at present
		
		/*foreach(DataWordData dw in datawords)
		{
			Debug.Log(dw.Word + " (target: " + dw.IsTargetWord + " nonsense: " + dw.Nonsense.ToString() + " dummy: " + dw.IsDummyWord + " linking index: " + dw.LinkingIndex);
			if(dw.IsDummyWord)
				DummyWords.Add(dw.Word);
			else
				CorrectWords.Add(dw.Word);
		}*/
		
		CorrectWords.Add ("arm");
		CorrectWords.Add ("bath");
		CorrectWords.Add ("boot");
		CorrectWords.Add ("cap");
		CorrectWords.Add ("car");
		CorrectWords.Add ("bird");
		CorrectWords.Add ("bell");
		CorrectWords.Add ("cake");
		CorrectWords.Add ("bus");
		CorrectWords.Add ("bunny");
		CorrectWords.Add ("dog");
		CorrectWords.Add ("fish");

		for(int i=0;i<DummyWords.Count;i++)
		{
			Debug.Log("Dummy word: "+DummyWords[i]);
		}
		for(int i=0;i<CorrectWords.Count;i++)
		{
			Debug.Log("Correct word: "+CorrectWords[i]);
		}

		bool thisCorrect=false;

		// This is the image swapout shiznat.
		foreach(OTSprite s in SceneImages)
		{
			GenericAnswer a=s.GetComponent<GenericAnswer>();

			if(thisCorrect)	
			{
				s.image=NextCorrectWord();
				a.isAnswer=true;
			}
			else 
			{
				s.image=NextDummyWord();
				a.isAnswer=false;
			}
			thisCorrect=!thisCorrect;
		}

		PlayAudio();
		
		Vector2 newPos=new Vector2(platform.position.x, -265.0f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.CubicOut );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(platform, 1.2f, config);
		Go.addTween(tween);
	}
	
	void Awake() {
		Pip=GameObject.Find("Pip").GetComponent<PipAnimation>();
		Pip.StopAnimations();
		
		// hack to stop it autoplaying pips animations
		pip.animation = reactions;
		Pip.StopAnimations();
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	public void StartRound(){
		CurrentWord = CorrectWords[UnityEngine.Random.Range(0, CorrectWords.Count)];
		DummyWord = CorrectWords[UnityEngine.Random.Range(0, CorrectWords.Count)];
		
		while(DummyWord == CurrentWord)
			DummyWord = CorrectWords[UnityEngine.Random.Range(0, CorrectWords.Count)];
		
		Transform t = (Transform)Instantiate(AnswersSection, new Vector3(-512.0f, 768.0f, 20.0f), Quaternion.identity);
		t.GetComponent<CorrectPathAnswers>().LoadAnswers(CurrentWord, DummyWord);
	}

	public Texture2D NextCorrectWord(){
		String w=(String)CorrectWords[currentCorrectIndex];
		Texture2D image=(Texture2D)Resources.Load("Images/word_images_png_350/_"+w+"_350");

		if(image==null)
			Debug.Log("load fail for correct "+w);

		currentCorrectIndex++;
		return image;
	}

	public Texture2D NextDummyWord(){
		String w=(String)CorrectWords[ UnityEngine.Random.Range(0,10) ];
		Texture2D image=(Texture2D)Resources.Load("Images/word_images_png_350/_"+w+"_350");

		if(image==null)
			Debug.Log("load fail for dummy "+w);

		currentDummyIndex++;
		return image;
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

	void ReturnToMap(){
		Invoke("CloseSession", 2.0f);
	}

	void CloseSession()
	{
		GameManager.Instance.SessionMgr.CloseActivity();
	}

	void CorrectAnswer()
	{
		currentIndex++;
		
		var config=new GoTweenConfig()
			.position( new Vector3(Camera.main.transform.position.x, currentIndex*sectionHeight, Camera.main.transform.position.z), false )
			.setEaseType( GoEaseType.ExpoOut );

		GoTween tween=new GoTween(Camera.main.transform, 1.8f, config);
		if(currentIndex>=totalScalableSections){
			tween.setOnCompleteHandler(c => RevealTreasure());
		}
		else{ 
			tween.setOnCompleteHandler(c => EnableTaps());
		}
		Go.addTween(tween);

		MovePlatform();
		AnimatePip(true);
	}

	void PlayAudio()
	{
		String w=(String)CorrectWords[currentIndex];
		foreach(AudioClip ac in audioWords)
		{
			if(ac.name==w)
			{
				audio.clip=ac;
				audio.Play();
				break;
			}
		}
	}

	void MovePlatform() 
	{	
		Vector2 newPos=new Vector2(platform.position.x, -265.0f+(currentIndex*sectionHeight));
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.CubicOut );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(platform, 1.2f, config);
		Go.addTween(tween);
	}

	void AnimatePip(bool correct)
	{
		Pip.SetNonePlaying();

		if(correct)
		{
			Pip.playPositive=true;
		}
		else
		{
			Pip.playDoubt=true;
		}
	}

	void EnableTaps()
	{
		DisableTap=false;
		PlayAudio();
	}

	void RevealTreasure()
	{
		audio.clip=completeClip;
		audio.Play();
		OTSprite treecover=GameObject.Find("4-treecover").GetComponent<OTSprite>();
		
		Vector2 newPos=new Vector2(-1000.0f, treecover.position.y);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.ExpoIn );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(treecover, 1.8f, config);
		tween.setOnCompleteHandler(c => ReturnToMap());

		Go.addTween(tween);

	}

	void On_SimpleTap(Gesture gesture)
	{
		if(DisableTap)return;

		if(gesture.pickObject)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer thisAns=gesture.pickObject.GetComponent<GenericAnswer>();
				if(thisAns.isAnswer)
				{
					DisableTap=true;
					CorrectAnswer();
				}	

				else
				{
					AnimatePip(false);
					Debug.Log("Incorrect");
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using AlTypes;

public class CrossTheBridgeManager : MonoBehaviour {

	int requiredAnswers=3;
	int correctAnswers=0;
	public Transform sign;
	int currentDummyIndex=0;
	int currentCorrectIndex=0;
	DataWordData[] datawords;
	ArrayList CorrectWords;
	ArrayList DummyWords;

	public AudioClip[] audioWords;

	public OTSprite[] AnswerSprites;

	TrollAnimation troll;

	// Use this for initialization
	void Start () {
	
	}
	
	void Awake() {
		datawords=GameManager.Instance.SessionMgr.CurrentDataWords;
		CorrectWords=new ArrayList();
		DummyWords=new ArrayList();

		foreach(DataWordData dw in datawords)
		{
			Debug.Log(dw.Word + " (target: " + dw.IsTargetWord + " nonsense: " + dw.Nonsense.ToString() + " dummy: " + dw.IsDummyWord + " linking index: " + dw.LinkingIndex);
			if(dw.IsDummyWord)
				DummyWords.Add(dw.Word);
			else
				CorrectWords.Add(dw.Word);
			
		}

		int defDummyCount=DummyWords.Count;

		for(int i=0;i<defDummyCount;i++)
		{
			String w=(String)DummyWords[i];
			DummyWords.Add(w);
		}
		for(int i=0;i<defDummyCount;i++)
		{
			String w=(String)DummyWords[i];
			DummyWords.Add(w);
		}
		for(int i=0;i<DummyWords.Count;i++)
		{
			Debug.Log("DummyWords word: "+DummyWords[i]);
		}
		for(int i=0;i<CorrectWords.Count;i++)
		{
			Debug.Log("Correct word: "+CorrectWords[i]);
		}

		sign=GameObject.Find("sign").transform;
		troll=GameObject.Find("Troll").GetComponent<TrollAnimation>();
		OTSprite sSign=sign.GetComponent<OTSprite>();
		sign.position=new Vector2(sign.position.x, sign.position.y+500);
		DropSignIn();
	}


	void DropSignIn()
	{
			OTSprite s=sign.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y-500);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, 0.8f, config);
			tween.setOnCompleteHandler(c => ShuffleAnswers());

			Go.addTween(tween);
	}

	void TakeSignOut()
	{
			OTSprite s=sign.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y+500);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, 0.8f, config);

			Go.addTween(tween);
	}

	// Update is called once per frame
	void Update () {
	
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

	void CloseSession()
	{
		GameManager.Instance.SessionMgr.CloseActivity();
	}

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer a=gesture.pickObject.GetComponent<GenericAnswer>();
				if(a.isAnswer)
				{
					correctAnswers++;
					 
					if(correctAnswers==requiredAnswers)
					{
						HideAnswers();
						Invoke("CloseSession", 2.0f);	
					}
					else
					{
						troll.MakeBellyBigger();
						ShuffleAnswers();
					}
				}
				else
				{
					troll.SetNonePlaying();
					troll.playNegative=true;
					Debug.Log("incorrect");
				}
			}
		}
	}

	void ShuffleAnswers()
	{
		int newCorrectAnswer=UnityEngine.Random.Range(0,3);

		for(int i=0;i<AnswerSprites.Length;i++)
		{
			OTSprite s=AnswerSprites[i];
			GenericAnswer a=s.GetComponent<GenericAnswer>();
			if(i==newCorrectAnswer)
			{
				String cword=(String)CorrectWords[currentCorrectIndex];
				currentCorrectIndex++;
				s.image=(Texture2D)Resources.Load("Images/word_images_png_150/_"+cword+"_150");
				s.size=new Vector2(150.0f,150.0f);

				a.isAnswer=true;
				var configr=new GoTweenConfig()
					.floatProp( "alpha", 0.5f );
			

		
				// Go.to(s, 0.3f, config );
				GoTween tweenr=new GoTween(s, 0.5f, configr);

				Go.addTween(tweenr);
			}
			else
			{
				String dword=(String)DummyWords[currentDummyIndex];
				currentDummyIndex++;
				s.image=(Texture2D)Resources.Load("Images/word_images_png_150/_"+dword+"_150");
				s.size=new Vector2(150.0f,150.0f);
				a.isAnswer=false;
				var configw=new GoTweenConfig()
					.floatProp( "alpha", 1.0f );
			

		
				// Go.to(s, 0.3f, config );
				GoTween tweenw=new GoTween(s, 0.5f, configw);

				Go.addTween(tweenw);
			}
		}
	}

	void HideAnswers() 
	{
		for(int i=0;i<AnswerSprites.Length;i++)
		{
			OTSprite s=AnswerSprites[i];

			var configr=new GoTweenConfig()
			.floatProp( "alpha", 0.0f );
	


			// Go.to(s, 0.3f, config );
			GoTween tweenr=new GoTween(s, 0.5f, configr);
			tweenr.setOnCompleteHandler(c => TakeSignOut());
			Go.addTween(tweenr);
		}
	}

	void PlayAudio(string forWord)
	{
		foreach(AudioClip ac in audioWords)
		{
			if(ac.name==forWord)
			{
				audio.clip=ac;
				audio.Play();
				break;
			}
		}
	}
}

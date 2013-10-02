using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using AlTypes;

public class CorrectSentence : MonoBehaviour {
	
	public string CurrentWord = "cat";
	public string DummyWord = "cat";
	private List<string> PictureNames = new List<string>();
	private PictureFrame pictureFrame;
	public int Score = 0;
	private bool GotAnswerWrong = false;
	
	public CorrectSentenceInfo sentenceInfo;
	public Transform SentenceButtonPrefab;
	
	public bool IsTimerBasedGame = false;
	public float TimerReward = 2.0f;
	public float StartingTimer = 20.0f;
	private float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	private float TimeLeft;
	public OTSprite TimeLeftSprite;
	private float TimeLeftSpriteStartingScale;
	private List<Transform> Buttons = new List<Transform>();
	
	public Transform StarBurst;
	
	// If this is beneath 0.0f then the user can make selections
	private float IsInactiveTimer = 0.0f;
	
	public GameObject CounterBarObject;
	public GameObject TimerBarObject;
	public TransitionScreen _transitionScreen;
	public OTTextSprite ScoreText;

	// Use this for initialization
	void Start ()
	{		
		pictureFrame = GameObject.Find("PictureFrame").GetComponent<PictureFrame>();
				
		_transitionScreen = GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>();
		TimeLeft = StartingTimer;
		TimeLeftSpriteStartingScale = TimeLeftSprite.size.y;
		
		if(IsTimerBasedGame)
			CounterBarObject.SetActive(false);
		else
			TimerBarObject.SetActive(false);
		
		//Texture2D image= (Texture2D)Resources.Load("Images/word_images_png_350/_"+w+"_350");

/*		DirectoryInfo dir = new DirectoryInfo(Application.datapath + "/Assets/Resources/Images/word_images_png_350");
		Debug.Log("Directory path: " + Application.datapath + "/Assets/Resources/Images/word_images_png_350");
		FileInfo[] info = dir.GetFiles("*.*");
		foreach (FileInfo f in info) 
		{
			Debug.Log(f.Name);
		}*/
			
		StartRound();
	}
	
	// Update is called once per frame
	void Update () {
		IsInactiveTimer -= Time.deltaTime;
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
			
			//if(allowInteraction)
			{
				TimeLeft -= Time.deltaTime * TimerModifier;
				TimerModifier += TimerModifierIncrease * Time.deltaTime;
				
				if(TimeLeft < 0.0f)
				{
					TimeLeft = 0.0f;
					//allowInteraction = false;
					StopGame();					
				}
			}
		}
	}
	
	public void StopGame()
	{
		
	}
	
	IEnumerator StartNewRoundDelayed(float duration)
	{
		yield return new WaitForSeconds(duration);		
		StartRound();
	}
	
	public void StartRound()
	{		
		foreach(Transform t in Buttons)
			Destroy(t.gameObject);
		Buttons.Clear();		
		
		// fetch sentence info from db
		
		sentenceInfo.index = 0;
		sentenceInfo.Word = "dog";
		sentenceInfo.TargetSentence = "the dog is brown";
		sentenceInfo.DummySentences = new List<string>();
		sentenceInfo.DummySentences.Clear();
		sentenceInfo.DummySentences.Add("the dog is red");
		sentenceInfo.DummySentences.Add("the cat is gray");
		sentenceInfo.DummySentences.Add("the fish is brown");
		sentenceInfo.DummySentences.Add(sentenceInfo.TargetSentence);
		
		// shuffle sentences
		for(int i = 0; i < sentenceInfo.DummySentences.Count; i++)
		{
			int index = Random.Range(0,sentenceInfo.DummySentences.Count);
			string tmpString = sentenceInfo.DummySentences[index];
			sentenceInfo.DummySentences[index] = sentenceInfo.DummySentences[i];
			sentenceInfo.DummySentences[i] = tmpString;
		}	
		
		int curX=0;
		int curY=0;
		float startX=-280.0f;
		float StartY=-120.0f;
		
		float incX=	440.0f;
		float incY=170.0f;
		
		bool HaveWeMadeCorrectWord = false;
		
		for(int i=0;i <  4;i++)
		{			
			Transform t=(Transform)Instantiate(SentenceButtonPrefab);
			OTTextSprite sentenceText = t.GetChild(0).GetComponent<OTTextSprite>();				
			
			sentenceText.text = sentenceInfo.DummySentences[i];
			sentenceText.ForceUpdate();
			
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX),StartY-(curY*incY));
			
			curX++;
			
			if(curX>1)
			{
				curX=0;
				curY++;
			}
			Buttons.Add(t);
		}	
		
		pictureFrame.ShowPicture(sentenceInfo.Word);
		
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

	public void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name.StartsWith("SentenceFrame") )
			{
				OTTextSprite SelectedSentence = gesture.pickObject.transform.GetChild(0).GetComponent<OTTextSprite>();
				if(SelectedSentence.text == sentenceInfo.TargetSentence)
				{
					Instantiate( StarBurst , gesture.pickObject.transform.position + new Vector3(0,0,-20), Quaternion.identity );
					gesture.pickObject.GetComponent<SentenceButton>().SetToDownState();
					IsInactiveTimer = 1.9f;
					
					if(!GotAnswerWrong)
					{
						Score++;
					
						if(IsTimerBasedGame)
						{
							TimeLeft += TimerReward;
							TimeLeft = Mathf.Clamp(TimeLeft, 0.0f, StartingTimer);
							ScoreText.text = Score.ToString();
							ScoreText.ForceUpdate();
						}else{
							CounterBarObject.GetComponent<SplatTheRatProgress>().currentNumber = Score;	
						}
					}
					
					StartCoroutine(StartNewRoundDelayed(1.2f));
					GotAnswerWrong = false;
					
				}else{
					GotAnswerWrong = true;
					iTween.ShakePosition(gesture.pickObject, iTween.Hash("amount", new Vector3(15.0f, 10.0f, 0.0f), "time", 0.5f));
				//	SelectedPF.Shake();
				}
				
			}
		}
	}
}

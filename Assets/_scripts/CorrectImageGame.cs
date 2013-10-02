using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using AlTypes;

public class CorrectImageGame : MonoBehaviour {
	
	public string CurrentWord = "cat";
	public string DummyWord = "cat";
	private List<string> PictureNames = new List<string>();
	private PictureFrame pictureFrameLeft;
	private PictureFrame pictureFrameRight;
	private PipPad pipPad;
	public int Score = 0;
	private bool GotAnswerWrong = false;
	
	public bool IsTimerBasedGame = false;
	public float TimerReward = 2.0f;
	public float StartingTimer = 20.0f;
	private float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	private float TimeLeft;
	public OTSprite TimeLeftSprite;
	private float TimeLeftSpriteStartingScale;
	
	// If this is beneath 0.0f then the user can make selections
	private float IsInactiveTimer = 0.0f;
	
	public GameObject CounterBarObject;
	public GameObject TimerBarObject;
	public TransitionScreen _transitionScreen;
	public OTTextSprite ScoreText;

	// Use this for initialization
	void Start ()
	{		
		pictureFrameLeft = GameObject.Find("PictureFrameLeft").GetComponent<PictureFrame>();
		pictureFrameRight = GameObject.Find("PictureFrameRight").GetComponent<PictureFrame>();
		pipPad = GameObject.Find("PipPad").GetComponent<PipPad>();
		
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
		
		PictureNames.Add ("arm");
		PictureNames.Add ("bath");
		PictureNames.Add ("boot");
		PictureNames.Add ("cap");
		PictureNames.Add ("car");
		PictureNames.Add ("bird");
		PictureNames.Add ("bell");
		PictureNames.Add ("cake");
		PictureNames.Add ("bus");
		PictureNames.Add ("bunny");
		PictureNames.Add ("dog");
		PictureNames.Add ("fish");
			
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
	
	public void StartRound()
	{		
		CurrentWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		DummyWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		
		// make sure dummy and target/current arent the same
		while(DummyWord == CurrentWord)
		{
			DummyWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		}
		
		if(Random.Range(0,2) == 0)
		{
			pictureFrameLeft.ShowPicture(CurrentWord);
			pictureFrameRight.ShowPicture(DummyWord);			
		}else{
			pictureFrameLeft.ShowPicture(DummyWord);
			pictureFrameRight.ShowPicture(CurrentWord);	
		}
		
		if(CurrentWord.Length > 1)
		{
			List<PhonemeData> _PhonemeData = new List<PhonemeData>();
			try {
				_PhonemeData = GameManager.Instance.GetOrderedPhonemesForWord(CurrentWord);
   			 }
    		catch  {
        		CurrentWord = "notFound";
    		} 
			pipPad.MakeAppear(_PhonemeData, CurrentWord.ToLower());				
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

	public void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name.StartsWith("PictureFrame") && IsInactiveTimer < 0.0f)
			{
				PictureFrame SelectedPF = gesture.pickObject.GetComponent<PictureFrame>();
				if(SelectedPF.MyWord == CurrentWord)
				{
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
					
					StartRound();
					GotAnswerWrong = false;
					
				}else{
					GotAnswerWrong = true;
					SelectedPF.Shake();
				}
				
			}
		}
	}
}

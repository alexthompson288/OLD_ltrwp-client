using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MPPictureGame : MonoBehaviour {
	
	public Transform Bars;	
	public Transform PurpleButtonPrefab;
	public Transform YellowButtonPrefab;
	public string CurrentWord = "cat";
	private List<string> PictureNames = new List<string>();
	public List<Transform> Buttons = new List<Transform>();
	public PictureFrame pictureFrame;
	public int PlayerID = 1;
	public MPPictureGame OtherPlayer;
	private int wordIndex = 0;
	public int score = 0;
	public OTTextSprite Score;
	
	// timer stuff
	public float StartingTimer = 20.0f;
	private float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	private float TimeLeft;
	public OTSprite TimeLeftSprite;
	private float TimeLeftSpriteStartingScale;
	private bool HasGameStarted = false;
	public bool isPurple = false;
	public GameObject PurpleChoiceButton;
	public GameObject YellowChoiceButton;
	
	// if we just go forwards through words or pick em randomly
	public bool isOrderedRound = false;
	
	private Vector3 StartingPosition;

	// Use this for initialization
	void Start ()
	{		
		TimeLeft = StartingTimer;
		TimeLeftSpriteStartingScale = TimeLeftSprite.size.y;
		StartingPosition = transform.position;
		pictureFrame = GameObject.Find("PictureFrame" + PlayerID.ToString() ).GetComponent<PictureFrame>();
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
			
		//StartRound();
	}
	
	// Update is called once per frame
	void Update () {
		if(HasGameStarted)
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
					
			TimeLeft -= Time.deltaTime * TimerModifier;
			TimerModifier += TimerModifierIncrease * Time.deltaTime;
			
			if(TimeLeft < 0.0f)
			{
				TimeLeft = 0.0f;
				
				//StopGame();					
			}	
		}
	
	}
	
	public void AddPoint()
	{
		if(HasGameStarted)
		{
			score++;
			Score.text = score.ToString();
			Score.ForceUpdate();
		}
	}
	
	public void StartRound()
	{
		Destroy(PurpleChoiceButton);
		Destroy(YellowChoiceButton);
		
		if(!HasGameStarted)
			return;
		if(Buttons.Count > 0)
		{
			for (int i = 0; i < Buttons.Count; i++)
			{
				Destroy( Buttons[i].gameObject );
			}
			Buttons.Clear();
		}
		
		if(!isOrderedRound)
			CurrentWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		else{
			
			CurrentWord = PictureNames[ wordIndex ];	
			wordIndex++;
			if(wordIndex > PictureNames.Count)
				wordIndex = 0;
		}
		
		int curX=0;
		int curY=0;
		float startX=-250.0f;
		float StartY=-70.0f;
		
		float incX=	400.0f;
		float incY=180.0f;
		
		bool HaveWeMadeCorrectWord = false;
		
		for(int i=0;i <  4;i++)
		{	
			Transform t;
			if(isPurple)
				t=(Transform)Instantiate(PurpleButtonPrefab);
			else
				t=(Transform)Instantiate(YellowButtonPrefab);
			PictureWordGameTouch pwgt=t.GetComponent<PictureWordGameTouch>();
			pwgt.MyWord=PictureNames[ Random.Range(0, PictureNames.Count) ];
			pwgt.PlayerID = PlayerID;
			pwgt.Init();
			pwgt.AnswerColliderName = "AnswerButton" + PlayerID.ToString() + (i + 1).ToString();			
			
			pwgt.TextPrefab.GetComponent<OTTextSprite>().text=pwgt.MyWord;
			pwgt.TextPrefab.GetComponent<OTTextSprite>().ForceUpdate();
			
			if(pwgt.MyWord == CurrentWord)
				HaveWeMadeCorrectWord = true;
			
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX) + StartingPosition.x,StartY-(curY*incY) + StartingPosition.y);
			
			curX++;
			
			if(curX>1)
			{
				curX=0;
				curY++;
			}
			Buttons.Add(t);
		}	
		
		if(!HaveWeMadeCorrectWord)
		{
			int index = Random.Range(0, Buttons.Count);
			Buttons[index].GetComponent<PictureWordGameTouch>().MyWord=CurrentWord;
			Buttons[index].GetChild(0).GetComponent<OTTextSprite>().text=CurrentWord;
			Buttons[index].GetChild(0).GetComponent<OTTextSprite>().ForceUpdate();
		}	
		
		pictureFrame.ShowPicture(CurrentWord);
		
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
	
	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture) 
	{
		if(gesture.pickObject== PurpleChoiceButton)
		{
			// we're purple
			isPurple = true;
			OtherPlayer.isPurple = false;
			HasGameStarted = true;
			StartRound();
			OtherPlayer.HasGameStarted = true;
			OtherPlayer.StartRound();			
			
		}else if(gesture.pickObject== YellowChoiceButton)
		{
			// we're yellow
			isPurple = false;
			OtherPlayer.isPurple = true;
			HasGameStarted = true;
			StartRound();
			OtherPlayer.HasGameStarted = true;
			OtherPlayer.StartRound();			
			
		}
		
	}
}

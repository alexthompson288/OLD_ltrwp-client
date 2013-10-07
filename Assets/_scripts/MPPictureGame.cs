using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using AlTypes;

public class MPPictureGame : MonoBehaviour {
	
	public Transform Bars;	
	public Transform PurpleButtonPrefab;
	public Transform YellowButtonPrefab;
	public Transform BlueButtonPrefab;
	public Transform GreenButtonPrefab;
	public string CurrentWord = "cat";
	private List<string> PictureNames = new List<string>();
	public List<Transform> Buttons = new List<Transform>();
	public PictureFrame pictureFrame;
	public int PlayerID = 1;
	public MPPictureGame OtherPlayer;
	public int wordIndex = 0;
	public int score = 0;
	public OTTextSprite Score;
	public float RoundTimer = 0.0f;
	
	// timer stuff
	public float StartingTimer = 20.0f;
	private float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	private float TimeLeft;
	public OTSprite TimeLeftSprite;
	private float TimeLeftSpriteStartingScale;
	private bool HasGameStarted = false;
	private float GameEndedTimer = 0.0f;
		
	public GameObject PipChoiceButton;
	public GameObject DotChoiceButton;
	public GameObject BunnyChoiceButton;
	public GameObject RodChoiceButton;
	public enum Characters {Pip, Rod, Dot, Bunny, none};
	public GameObject CharacterSelectParent;
	public GameObject Title;
	
	
	// end screens
	public bool HasEnded = false;
	public GameObject EndScreenParent;
	public OTSprite LeftEopBg;
	public OTSprite RighttEopBg;
	public OTTextSprite LeftScore;
	public OTTextSprite RightScore;
	public OTTextSprite LeftText;
	public OTTextSprite RightText;
	
	private GameObject TryAgain1;
	private GameObject TryAgain2;
	
	public bool IsPlayer1 = true;
	
	public string CharacterName = "none";
	
	public Characters SelectedCharacter = Characters.none;
	
	// if we just go forwards through words or pick em randomly
	public bool isOrderedRound = false;
	
	private Vector3 StartingPosition;
	
	void Awake()
	{
		TimeLeft = StartingTimer;
		TimeLeftSpriteStartingScale = TimeLeftSprite.size.y;
		StartingPosition = transform.position;
		pictureFrame = GameObject.Find("PictureFrame" + PlayerID.ToString() ).GetComponent<PictureFrame>();
		
		TryAgain1 = GameObject.Find("TryAgain1");
		TryAgain2 = GameObject.Find("TryAgain2");	
	}
	
	// Use this for initialization
	void Start ()
	{		
		
		TryAgain1.SetActive(false);
		TryAgain2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(HasGameStarted)
		{
			RoundTimer += Time.deltaTime;
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
				
				EndGame();				
			}	
		}
		
		if( HasEnded )
		{
			GameEndedTimer += Time.deltaTime;	
		}
	}
	
	public void LoadWords(int Difficulty)
	{
		DataWordData[] dwds;
		if(Difficulty == 0)
			dwds = GameManager.Instance.GetDataWordsForSection(1388);
		else if(Difficulty == 1)
			dwds = GameManager.Instance.GetDataWordsForSection(1389);
		else
			dwds = GameManager.Instance.GetDataWordsForSection(1390);
		
		foreach(DataWordData dwd in dwds)
		{
			if(	dwd.IsTargetWord == true)
			{
				PictureNames.Add(dwd.Word);	
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
	
	public IEnumerator StartGame()
	{
		if(IsPlayer1)
			iTween.MoveTo(CharacterSelectParent, iTween.Hash("position",CharacterSelectParent.transform.position - new Vector3( 400.0f, 0.0f, 0.0f),"time", 1.0f, "delay", 0.5f));
		else
			iTween.MoveTo(CharacterSelectParent, iTween.Hash("position",CharacterSelectParent.transform.position + new Vector3( 400.0f, 0.0f, 0.0f),"time", 1.0f, "delay", 0.5f));
		iTween.MoveTo(Title, iTween.Hash("position",Title.transform.position + new Vector3( 0.0f, 200.0f, 0.0f),"time", 1.2f));
		yield return new WaitForSeconds(1.4f);
		HasGameStarted = true;
		StartRound();

	}
	
	public void StartRound()
	{
		Destroy(PipChoiceButton);
		Destroy(DotChoiceButton);
		Destroy(BunnyChoiceButton);
		Destroy(RodChoiceButton);
		
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
		RoundTimer = 0.0f;
		if(!isOrderedRound)
			CurrentWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		else{
			if(wordIndex > PictureNames.Count)
				wordIndex = 0;
			CurrentWord = PictureNames[ wordIndex ];	
			wordIndex++;
			
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
			if(SelectedCharacter == Characters.Dot)
				t=(Transform)Instantiate(PurpleButtonPrefab);
			else if(SelectedCharacter == Characters.Pip)
				t=(Transform)Instantiate(YellowButtonPrefab);
			else if(SelectedCharacter == Characters.Bunny)
				t=(Transform)Instantiate(GreenButtonPrefab);
			else 
				t=(Transform)Instantiate(BlueButtonPrefab);
			
			PictureWordGameTouch pwgt=t.GetComponent<PictureWordGameTouch>();
			// check for duplicate words
			bool isWordDuplicate = true;
			while(isWordDuplicate)
			{
				isWordDuplicate = false;
				pwgt.MyWord=PictureNames[ Random.Range(0, PictureNames.Count) ];
				foreach (Transform tb in Buttons )
				{
					if(tb.GetComponent<PictureWordGameTouch>().MyWord == pwgt.MyWord)
						isWordDuplicate = true;
				}
			}
			pwgt.PlayerID = PlayerID;
			pwgt.Init();
			pwgt.AnswerColliderName = "AnswerButton" + PlayerID.ToString() + (i + 1).ToString();			
			
			pwgt.TextPrefab.GetComponent<OTTextSprite>().text=pwgt.MyWord;
			
			
			
			pwgt.TextPrefab.GetComponent<OTTextSprite>().ForceUpdate();
			
			if(pwgt.MyWord == CurrentWord)
				HaveWeMadeCorrectWord = true;
			
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX) + StartingPosition.x,StartY-(curY*incY) + StartingPosition.y);
			s.size = new Vector2(20.0f, 20.0f);
			
			curX++;
			
			if(curX>1)
			{
				curX=0;
				curY++;
			}
			
			iTween.ScaleTo(t.gameObject, new Vector3(375, 185, 1), 0.5f);
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
	
	public void EndGame()
	{
		if(HasEnded)
			return;
		HasEnded = true;
		TryAgain1.SetActive(true);
		TryAgain2.SetActive(true);
		
		if(Buttons.Count > 0)
		{
			for (int i = 0; i < Buttons.Count; i++)
			{
				Destroy( Buttons[i].gameObject );
			}
			Buttons.Clear();
		}
		
		pictureFrame.MakeDisappear();
		
		wordIndex = Mathf.Max(wordIndex, OtherPlayer.wordIndex);
		OtherPlayer.wordIndex = wordIndex;
		
		iTween.MoveTo(	EndScreenParent, EndScreenParent.transform.position + new Vector3(0,750,0), 1.0f);
				
		if(score == OtherPlayer.score)
		{
			LeftText.text = "It's a draw!";
			LeftText.ForceUpdate();
			RightText.text = "Try Again?";
			RightText.ForceUpdate();
			SetBGColour(LeftEopBg, SelectedCharacter );
			SetBGColour(RighttEopBg, OtherPlayer.SelectedCharacter );
			LeftScore.text = score.ToString();
			LeftScore.ForceUpdate();
			RightScore.text = OtherPlayer.score.ToString();
			RightScore.ForceUpdate();
		}else if(score < OtherPlayer.score){
			// we lost :(
			LeftText.text = OtherPlayer.CharacterName + " won!";
			LeftText.ForceUpdate();
			RightText.text = "Try Again?";
			RightText.ForceUpdate();
			SetBGColour(LeftEopBg, OtherPlayer.SelectedCharacter );
			SetBGColour(RighttEopBg, SelectedCharacter );
			LeftScore.text = OtherPlayer.score.ToString();
			LeftScore.ForceUpdate();
			RightScore.text = score.ToString();
			RightScore.ForceUpdate();
		}else{
			// we won	
			LeftText.text = CharacterName + " won!";
			LeftText.ForceUpdate();
			RightText.text = "Try Again?";
			RightText.ForceUpdate();
			SetBGColour(LeftEopBg, SelectedCharacter );
			SetBGColour(RighttEopBg, OtherPlayer.SelectedCharacter );
			LeftScore.text = score.ToString();
		LeftScore.ForceUpdate();
		RightScore.text = OtherPlayer.score.ToString();
		RightScore.ForceUpdate();
		}
		LeftEopBg.ForceUpdate();
		RighttEopBg.ForceUpdate();
	}
	
	void ResetGame()
	{
		if(HasEnded == false)
			return;
		iTween.MoveTo(	EndScreenParent, EndScreenParent.transform.position + new Vector3(0,-750,0), 1.0f);
		iTween.MoveTo(	OtherPlayer.EndScreenParent, OtherPlayer.EndScreenParent.transform.position + new Vector3(0,-750,0), 1.0f);
		score = 0;
		Score.text = score.ToString();
		Score.ForceUpdate();
		TimeLeft = StartingTimer;
		HasEnded = false;
		StartRound();
		HasGameStarted = true;
		
		OtherPlayer.score = 0;
		OtherPlayer.Score.text = OtherPlayer.score.ToString();
		OtherPlayer.Score.ForceUpdate();
		OtherPlayer.TimeLeft = StartingTimer;
		OtherPlayer.HasEnded = false;
		OtherPlayer.StartRound();
		HasGameStarted = true;
		
		TryAgain1.SetActive(false);
		TryAgain2.SetActive(false);
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
		if(gesture == null || gesture.pickObject == null)
			return;
		if(gesture.pickObject.name == "TryAgain1" || gesture.pickObject.name == "TryAgain2")
		{
			if(GameEndedTimer > 2.0f)
			{
				ResetGame();
				OtherPlayer.ResetGame();
				GameEndedTimer = 0.0f;
			}
		}
		if(gesture.pickObject== PipChoiceButton)
		{
			// check other players choice isnt this character
			if(OtherPlayer.SelectedCharacter != Characters.Pip)
			{
				SelectedCharacter = Characters.Pip;
				CharacterName = "Pip";
				ResetCharacterStates();
				PipChoiceButton.GetComponent<SimpleSpriteButton>().PressDown();
				OtherPlayer.GreyOutCharacter(Characters.Pip);
			}			
		}	
		if(gesture.pickObject== DotChoiceButton)
		{
			// check other players choice isnt this character
			if(OtherPlayer.SelectedCharacter != Characters.Dot)
			{
				SelectedCharacter = Characters.Dot;
				CharacterName = "Dot";
				ResetCharacterStates();
				DotChoiceButton.GetComponent<SimpleSpriteButton>().PressDown();
				OtherPlayer.GreyOutCharacter(Characters.Dot);
			}			
		}	
		if(gesture.pickObject== BunnyChoiceButton)
		{
			// check other players choice isnt this character
			if(OtherPlayer.SelectedCharacter != Characters.Bunny)
			{
				SelectedCharacter = Characters.Bunny;
				CharacterName = "Bunny";
				ResetCharacterStates();
				BunnyChoiceButton.GetComponent<SimpleSpriteButton>().PressDown();
				OtherPlayer.GreyOutCharacter(Characters.Bunny);
			}			
		}	
		if(gesture.pickObject== RodChoiceButton)
		{
			// check other players choice isnt this character
			if(OtherPlayer.SelectedCharacter != Characters.Rod)
			{
				SelectedCharacter = Characters.Rod;
				CharacterName = "Rod";
				ResetCharacterStates();
				RodChoiceButton.GetComponent<SimpleSpriteButton>().PressDown();
				OtherPlayer.GreyOutCharacter(Characters.Rod);
			}			
		}	
		if(SelectedCharacter != Characters.none && OtherPlayer.SelectedCharacter!= Characters.none && HasGameStarted == false)
		{
			StartCoroutine( StartGame() );
			StartCoroutine( OtherPlayer.StartGame() );
		}
	}
	
	void ResetCharacterStates()
	{
		PipChoiceButton.GetComponent<SimpleSpriteButton>().ReleaseButton();
		DotChoiceButton.GetComponent<SimpleSpriteButton>().ReleaseButton();
		BunnyChoiceButton.GetComponent<SimpleSpriteButton>().ReleaseButton();
		RodChoiceButton.GetComponent<SimpleSpriteButton>().ReleaseButton();
	}
	
	public void GreyOutCharacter(Characters CharacterToGreyOut)
	{
		PipChoiceButton.GetComponent<OTSprite>().tintColor = Color.white;
		DotChoiceButton.GetComponent<OTSprite>().tintColor = Color.white;
		BunnyChoiceButton.GetComponent<OTSprite>().tintColor = Color.white;
		RodChoiceButton.GetComponent<OTSprite>().tintColor = Color.white;
		
		if(CharacterToGreyOut == Characters.Bunny)
			BunnyChoiceButton.GetComponent<OTSprite>().tintColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
		else if (CharacterToGreyOut == Characters.Pip)
			PipChoiceButton.GetComponent<OTSprite>().tintColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);	
		else if (CharacterToGreyOut == Characters.Dot)
			DotChoiceButton.GetComponent<OTSprite>().tintColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);		
		else if (CharacterToGreyOut == Characters.Rod)
			RodChoiceButton.GetComponent<OTSprite>().tintColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	}
	
	void SetBGColour( OTSprite s, Characters Character )
	{
		if(	Character == Characters.Pip)
		{
			s.frameIndex = 1;
		}else if (	Character == Characters.Dot)
		{
			s.frameIndex = 0;
		}else if(	Character == Characters.Rod)
		{
			s.frameIndex = 2;
		}else
		{
			s.frameIndex = 3;
		}
	}
}

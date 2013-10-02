using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class LetterWordBank : MonoBehaviour {
	
	public Transform Bars;	
	GameManager cmsLink;
	private float velocity = 0.0f;
	public Transform[] ButtonPrefabs;
	private float BarsHeight = 0.0f;
	PersistentObject PersistentManager;
	public bool isGame = false;
	public GameObject TimerBar;
	public float TimeLeftSpriteStartingScale;
	public OTSprite TimeLeftSprite;
	private float TimeLeft = 20.0f;
	public float StartingTimer = 20.0f;
	public float TimerModifier = 1.0f;
	public float TimerModifierIncrease = 0.1f;
	public string CurrentLetter = "a";
	private List<string> letters;
	private int score = 0;
	public OTTextSprite ScoreBoard;
	public PictureFrame pictureFrame;
	
		
	// Use this for initialization
	void Start () {
		TimeLeft = StartingTimer;
		TimeLeftSpriteStartingScale = TimeLeftSprite.size.y;
		
		cmsLink=GameManager.Instance;
		letters = new List<string>();
		PopulateAlphabet(letters);
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}		
		
		if(!isGame)
		{
			TimerBar.SetActive(false);	
		}
		
		int curX=0;
		int curY=0;
		float startX=-410.0f;
		float StartY=135.0f;
		
		float incX=	820.0f/6.0f;
		float incY=130.0f;
		
		for(int i=0;i < letters.Count;i++)
		{			
			Transform t=(Transform)Instantiate(ButtonPrefabs[ Random.Range(0,3) ]);
			t.parent = Bars.transform;
			LetterBankTouch lbt=t.GetComponent<LetterBankTouch>();
			lbt.MyWord=letters[i];
			lbt.TextPrefab.GetComponent<OTTextSprite>().text=letters[i];
			
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX),StartY-(curY*incY));
			
			curX++;
			
			if(curX>6)
			{
				curX=0;
				curY++;
			}
		}
		
		if(isGame)
		{
			NewRound();
			Bars.transform.position = new Vector3(-55.55f, 0.0f, 0.0f);	
			Bars.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
		}		
		
		BarsHeight = (curY*incY);
	}
	
	// Update is called once per frame
	void Update () {
	/*	velocity *= 1.0f - (Time.deltaTime * 4.0f);
		Bars.position=new Vector3(Bars.position.x,Bars.position.y+(velocity * Time.deltaTime),Bars.position.z);
		
		if(Bars.position.y - BarsHeight >  150.0f)
		{	
			velocity -= 25.0f;
		}else if(Bars.position.y <  -170.0f)
		{	
			velocity += 25.0f;
		}	*/
		
		if(isGame)
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
						
			}
				
		}
	}
	
	public void LetterSelected(string letter)
	{
		if(letter == CurrentLetter)
		{
			TimeLeft += 4.0f;
			TimeLeft = Mathf.Clamp(TimeLeft, 0.0f, StartingTimer);
			score++;
			ScoreBoard.text = score.ToString();
			NewRound();
		}
		
	}
	
	void NewRound()
	{
		
		//pictureFrame.PlayMnemonic(MyWord);
		CurrentLetter = letters[Random.Range(0, letters.Count-2)];
		while(CurrentLetter == "q")
			CurrentLetter = letters[Random.Range(0, letters.Count-2)];
		pictureFrame.ShowMnemonic(CurrentLetter, 2.0f);
		PhonemeData PD = GameManager.Instance.GetPhonemeInfoForPhoneme(CurrentLetter);
		string baclip="benny_phoneme_" + PD.Grapheme.ToLower() + "_" + PD.Phoneme.ToLower() + "_" + PD.Mneumonic.ToLower();
		baclip= baclip.Replace(" ", "_");
		Debug.Log("Looking for :"+ baclip);
		audio.clip=  (AudioClip)Resources.Load("audio/benny_phonemes_master/"+ baclip);
		audio.Play();
	}
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
//		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;
//		EasyTouch.On_Drag -= On_Drag;	
	}
	
	void On_TouchDown(Gesture gesture)
	{
		velocity += gesture.deltaPosition.y * 4.0f;
	}
	
	void PopulateAlphabet(List<string> Letters)
	{
		Letters.Add("a");
		Letters.Add("b");
		Letters.Add("c");
		Letters.Add("d");
		Letters.Add("e");
		Letters.Add("f");
		Letters.Add("g");
		Letters.Add("h");
		Letters.Add("i");
		Letters.Add("j");
		Letters.Add("k");
		Letters.Add("l");
		Letters.Add("m");
		Letters.Add("n");
		Letters.Add("o");
		Letters.Add("p");
		Letters.Add("q");
		Letters.Add("r");
		Letters.Add("s");
		Letters.Add("t");
		Letters.Add("u");
		Letters.Add("v");
		Letters.Add("x");
		Letters.Add("y");
		Letters.Add("z");
		
	}
}

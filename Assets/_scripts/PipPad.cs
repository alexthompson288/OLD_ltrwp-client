using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class PipPad : MonoBehaviour {
	
	public OTTextSprite _phonemesSprite;
	private List<SimpleButton> buttons = new List<SimpleButton>();
	public List<OTTextSprite> letters = new List<OTTextSprite>();
	private List<WordButtonContainer> wordButtonContainers = new List<WordButtonContainer>();
	public GameObject SmallButtonPrefab;
	public GameObject MediumButtonPrefab;
	public GameObject LargeButtonPrefab;
	public GameObject LetterPrefab;
	public GameObject PipWordNotFound;
	public GameObject LetterButtonContainer;
	public bool isMachineUp = false;
	public bool isMachineMoving = false;
	private GameObject Sparkle;
	public string Word;
	float MaximumWordWidth = 200.0f;
	float MinimumWordWidth = 50.0f;
	public SpriteMotion _trickyStar;
	public float TextScaleModifier = 1.0f;
	private Vector2 StartingScale;
	private float StartingYValue;
	public float YellowHighLightDepth = -77.0f;
	public bool UseCustomOnScreenPosition = false;
	public Vector3 CustomOnScreenPosition;
	public bool ShouldStayOnScreen = false;
	private float Scale = 1.0f;
	private PictureFrame pictureFrame = null;
	
	public AudioClip AppearSound;
	public AudioClip DisappearSound;
	
	// Use this for initialization
	void Start () {
		Scale = transform.localScale.x;
		Sparkle = GameObject.Find("WordSelectionGlow");
		_trickyStar.FadeOut(2.0f);
		GameObject gl = letters[0].transform.parent.gameObject;
		OTTextSprite LS = gl.transform.GetChild(0).GetComponent<OTTextSprite>();
		StartingScale = LS.size;
		StartingYValue = transform.position.y;
		pictureFrame = GameObject.Find("PictureFrame").GetComponent<PictureFrame>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator MakeAppearDelayed(List<PhonemeData> PhonemeData, string word){
		yield return new WaitForSeconds( 0.6f);
		MakeAppear(PhonemeData, word);
	}
	
	public void MakeAppear(List<PhonemeData> PhonemeData, string word){
		if(isMachineMoving)
		{
			return;
		}
		if(isMachineUp)
		{
			MakeDisappearQuick();
			StartCoroutine(MakeAppearDelayed(PhonemeData,word));
			return;
		}
		
		deleteLettersAndButtons();	
		
		Word = word;
		
		// temparory pip word not found
		if(word.ToLower() == "notfound")
		{
			Vector3 newPos2=new Vector3(0.0f, -154.0f, -25.0f);
			var config2=new GoTweenConfig()
				.vector3Prop( "position", newPos2 )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween2=new GoTween(PipWordNotFound.transform, 1.28f, config2);
			//tween2.setOnCompleteHandler(c => MachineUp());
			Go.addTween(tween2);			
			return;
		}	
		
		StartCoroutine(MachineUp(1.28f));
		isMachineMoving = true;
		// calculate spacing
		_phonemesSprite.gameObject.SetActive( true );
		List<float> letterWidths = new List<float>();
		float LetterWidth = 70.0f;
		float wordWidth = LetterWidth * word.Length;
		
		float spaceWidth = (MaximumWordWidth - wordWidth) / (word.Length - 1);
		spaceWidth = Mathf.Clamp(spaceWidth, 30.0f, 80.0f);
		
		Debug.Log("spaceWidth: "+spaceWidth);
		
		// set up the phoneme text
		// add spacing between letters
		_phonemesSprite.text = word;
		_phonemesSprite.gameObject.SetActive( false );
		
		if(word.Length > 6)
		{
			spaceWidth *= 0.5f;
			LetterWidth *= 0.5f;
			wordWidth *= 0.5f;
		}
		
		// place letters
		float currentXPos = - ((wordWidth * 0.5f ) + (((float)( Word.Length - 1) * 0.5f) * spaceWidth));
		Debug.Log("currentXPos: "+ currentXPos);
		for(int i = 0; i < Word.Length; i++)
		{
			// use one of the letters from the precreated array
			GameObject gl = letters[i].transform.parent.gameObject;
			gl.transform.localPosition = new Vector3(currentXPos + (LetterWidth * 0.5f) , -10.0f, -23.0f);
			OTTextSprite LS = gl.transform.GetChild(0).GetComponent<OTTextSprite>();
			LS.text = Word[i].ToString();
			
			if(word.Length > 6)
			{
				LS.size = new Vector2(1.0f, 1.0f) * TextScaleModifier * Scale;
			}else{
				LS.size = new Vector2(2.0f, 2.0f) * TextScaleModifier * Scale;
			}
			
			currentXPos += LetterWidth + spaceWidth;
		}
		// if its tricky we dont need to do the buttons
		if(!GameManager.Instance.isWordTricky(Word))
		{
			// place buttons
			currentXPos = - (wordWidth  * 0.5f ) - (((float)( Word.Length - 1) * 0.5f) * spaceWidth) ;// + transform.position.x;
			// used to track how many letters we are through the word, because of multi letter phonemes
			int CurrentPhonemePosition = 0;
			for(int i = 0; i < PhonemeData.Count; i++)
			{
				Debug.Log("Phoneme letters: " + PhonemeData[i].LetterInWord);
				int PhonemeLength = PhonemeData[i].LetterInWord.Length;
				GameObject go = new GameObject();
				// goDia is for dual vowels diaphemes
				GameObject goDia = new GameObject();
				if(PhonemeLength == 1)
				{
					Destroy(go);	Destroy(goDia);	goDia = null;
					go = Instantiate(SmallButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
					go.transform.parent = transform; 
					go.transform.localScale = new Vector3(Scale,Scale,Scale);
					go.transform.localPosition = new Vector3(currentXPos + (LetterWidth * 0.5f) , -165.0f, -23.0f);
				}else if(PhonemeLength == 2)
				{
					Destroy(go);	Destroy(goDia);	goDia = null;
					go = Instantiate(MediumButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
					go.transform.parent = transform; 
					go.transform.localScale = new Vector3(Scale,Scale,Scale);
					go.transform.localPosition = new Vector3(currentXPos + (LetterWidth ) + (spaceWidth * 0.5f) , -165.0f, -23.0f);
				}else if(!PhonemeData[i].LetterInWord.Contains("-"))
				{
					Destroy(go);	Destroy(goDia);	goDia = null;
					go = Instantiate(LargeButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
					go.transform.parent = transform; 
					go.transform.localScale = new Vector3(Scale,Scale,Scale);
					go.transform.localPosition = new Vector3(currentXPos + ((LetterWidth + spaceWidth )* 1.5f), -165.0f, -23.0f);
				}else{
					// its a diaphemes
					Destroy(go);	Destroy(goDia);	goDia = null;
					go = Instantiate(SmallButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
					goDia = Instantiate(SmallButtonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
					go.transform.parent = transform; 
					goDia.transform.parent = transform; 
					go.transform.localPosition = new Vector3(currentXPos + (LetterWidth * 0.5f) , -165.0f, -23.0f);
					goDia.transform.localPosition = new Vector3(currentXPos + (LetterWidth * 0.5f) + (LetterWidth * 2.0f) + (spaceWidth * 2.0f) , -165.0f, -23.0f);
					if(word.Length > 6)
					{
						goDia.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * Scale;
					}else{
						goDia.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Scale;
					}						
				}
				
				if(word.Length > 6)
				{
					go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * Scale;
				}else{
					go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Scale;
				}
				
				SimpleButton BS = go.GetComponent<SimpleButton>();
				buttons.Add(BS);
				
				int SpaceBetweenSplitDiagraph = 0;
				if(PhonemeData[i].LetterInWord.Contains("-"))
				{
					PhonemeLength = 1;
					SpaceBetweenSplitDiagraph = word.Length - CurrentPhonemePosition -1;
					
				}
				
				// generate Containers for letter and button
				GameObject Container = Instantiate(LetterButtonContainer, new Vector3(currentXPos + (LetterWidth * 0.5f), -837.1434f, 16.0f), Quaternion.identity) as GameObject;
				Container.transform.localScale = new Vector3(Scale,Scale,1.0f);
				Container.transform.parent  = transform; 
				if (PhonemeLength == 1)
				{
					Container.transform.localPosition = new Vector3(currentXPos + (LetterWidth * 0.5f), -837.1434f, 16.0f);
					
				}else if (PhonemeLength == 3){
					Container.transform.localPosition = new Vector3(currentXPos + ((LetterWidth + spaceWidth )* 1.5f), -837.1434f, 16.0f);
				}
				if (PhonemeLength == 2)
				{
					Container.transform.localPosition = new Vector3(currentXPos + (LetterWidth ) + (spaceWidth * 0.5f) , -837.1434f, 16.0f);
					
				}else if (PhonemeLength == 3){
					Container.transform.localPosition = new Vector3(currentXPos + ((LetterWidth + spaceWidth )* 1.5f), -837.1434f, 16.0f);
				}
				
				WordButtonContainer WBC = Container.GetComponent<WordButtonContainer>();
				WBC.PD = PhonemeData[i];
				WBC.Create(BS, PhonemeData[i].LetterInWord.Length, Word.Length, LetterWidth, spaceWidth, SpaceBetweenSplitDiagraph, YellowHighLightDepth, Scale);
				
				
				//WBC.transform.parent = transform; 
				
				if(goDia != null)
				{
					SimpleButton BSD = goDia.GetComponent<SimpleButton>();
					buttons.Add(BSD);
					WBC._DiaButton = BSD;
				}
					
				wordButtonContainers.Add(WBC);
				
				Vector3 pos = Container.transform.localPosition;
					//pos.x += transform.position.x;
					pos.y = -99.28836f;
					Container.transform.localPosition = pos;
				
				//Container.transform.localScale = new Vector3(Scale,Scale,Scale);
				for(int j = 0; j < PhonemeLength; j++)
				{
					currentXPos += LetterWidth + spaceWidth;
					CurrentPhonemePosition++;
				}			
			}
		}else{
			_trickyStar.FadeIn(1.0f);	
		}
				
		// bounce the PipPad up
		Vector3 newPos=new Vector3(0.0f, -50.0f, -20.0f);
		if(UseCustomOnScreenPosition)
			newPos = CustomOnScreenPosition;
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(transform, 1.28f, config);
	//	tween.setOnCompleteHandler(c => SlideMachineUp());

		Go.addTween(tween);
		
	}
			
	void deleteLettersAndButtons()
	{
		if(buttons.Count > 0)
		{
			for(int i = buttons.Count-1; i > -1; i--)
				Destroy(buttons[i].gameObject);
			for(int i = letters.Count-1; i > -1; i--)
				letters[i].transform.parent.transform.localPosition = new Vector3(-1000.0f, -1000.0f, 0.0f);
				//Destroy(letters[i].gameObject);
			for(int i = wordButtonContainers.Count-1; i > -1; i--)
				Destroy(wordButtonContainers[i].gameObject);
			
			buttons.Clear();
			//letters.Clear();
			wordButtonContainers.Clear();
		}			
	
	}
	
	public void MakeDisappear()
	{
		audio.clip = DisappearSound;
		audio.Play();
		isMachineUp = false;
		Vector3 newPos=new Vector3(0.0f, StartingYValue, -20.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.QuadIn );

		GoTween tween=new GoTween(transform, 1.0f, config);
		
		StartCoroutine(MachineDown(1.28f));
		//tween.setOnCompleteHandler(c => MachineDown());

		Go.addTween(tween);
		
		if(pictureFrame != null)
			pictureFrame.MakeDisappear();
	}
	
	public void MakeDisappearQuick()
	{
		audio.clip = DisappearSound;
		audio.Play();
		isMachineUp = false;
		Vector3 newPos=new Vector3(0.0f, StartingYValue, -20.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.QuadIn );

		GoTween tween=new GoTween(transform, 0.4f, config);
		
		StartCoroutine(MachineDown(0.42f));
		//tween.setOnCompleteHandler(c => MachineDown());

		Go.addTween(tween);
	}
	
	IEnumerator MachineUp (float time) {
		audio.clip = AppearSound;
		audio.PlayDelayed(0.3f);
		yield return new WaitForSeconds(time);
		//Debug.Log("PipPad up");
		isMachineUp = true;	
		isMachineMoving = false;
		
	}
	
	IEnumerator MachineDown (float time) {
		// if its a tricky word get rid of the star
		if(GameManager.Instance.isWordTricky(Word))
		{
			_trickyStar.FadeOut(1.0f);
		}
		isMachineMoving = true;
		// get rid of the sparkle word selector
		if(Sparkle != null)
		{
			Sparkle.transform.position = new Vector3(1000.0f, 0.0f, Sparkle.transform.position.z);
			Sparkle.GetComponent<SpriteMotion>().FadeOut(2.0f);
			//Sparkle.transform.GetChild(0).GetComponent<ParticleSystem>().ClearParticles();
		}
		yield return new WaitForSeconds(time);
		//Debug.Log("PipPad down");
		isMachineUp = false;	
		isMachineMoving = false;
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

	void On_SimpleTap(Gesture gesture) {
		if(gesture.pickObject==gameObject && !ShouldStayOnScreen)
		{
			MakeDisappear();
		}
	}
}

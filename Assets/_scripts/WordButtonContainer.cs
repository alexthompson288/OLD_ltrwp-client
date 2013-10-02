using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class WordButtonContainer : MonoBehaviour {
	
	public SimpleButton _button = null;
	public SimpleButton _DiaButton = null;
	
	public List<OTTextSprite> _letters;
	private float SoundTimer = 0.0f;
	public PhonemeData PD;
	public GameObject _yellowHighlight;
	public GameObject _yellowHighlightDia = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SoundTimer -= Time.deltaTime;
		if(SoundTimer < 0.0f && _button != null)
		{
			_button.ReleaseButton();
			_yellowHighlight.SetActive(false);
			
			if(_yellowHighlightDia != null)
			{
				_DiaButton.ReleaseButton();
				_yellowHighlightDia.SetActive(false);
			}
		}
	}
	
	// split diagraph
	public void Create(SimpleButton Button, int length, int WordLength, float letterWidth, float spaceWidth, int numberOfLettersBetweenSplitDiagraph, float YellowHighLightDepth, float scale)
	{
		BoxCollider bc = gameObject.GetComponent<BoxCollider>();		
		
		bc.isTrigger = true;
		Vector3 Scale = _yellowHighlight.transform.localScale;		
		
		if(WordLength > 6)
		{	
			Scale.x = Scale.x * 0.5f;
			bc.size = new Vector3(length * 50.0f, 300.0f, 50.0f);
		}else{
			bc.size = new Vector3(length * 100.0f, 300.0f, 50.0f);
		}
		
		if(PD.LetterInWord.Contains("-"))
		{
			length = 1;
			_yellowHighlightDia = Instantiate(_yellowHighlight, _yellowHighlight.transform.position + new Vector3((spaceWidth + letterWidth) * (numberOfLettersBetweenSplitDiagraph) * scale, 0.0f, 0.0f), _yellowHighlight.transform.rotation) as GameObject;
			_yellowHighlightDia.transform.parent = transform;
			_yellowHighlightDia.transform.localScale = Scale;
			Vector3 center = bc.center;
			if(WordLength > 6)
			{	
				bc.size = new Vector3(length * 50.0f * 3.0f, 300.0f, 48.0f);
				
				center.x += letterWidth + spaceWidth;
			}else{
				bc.size = new Vector3(length * 100.0f * 3.0f, 300.0f, 48.0f);
				center.x += letterWidth + spaceWidth;
			}
			bc.center = center;
		}else{
			Scale.x *= length;	
		}
				
		_button = Button;
		_yellowHighlight.transform.localScale = Scale;
		Vector3 pos = _yellowHighlight.transform.localPosition;
		pos.z = YellowHighLightDepth;
		_yellowHighlight.transform.localPosition = pos;
		
		if(PD.LetterInWord.Contains("-"))
		{
			pos = _yellowHighlightDia.transform.localPosition;
			pos.z = YellowHighLightDepth;
			_yellowHighlightDia.transform.localPosition = pos;
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

	void On_SimpleTap(Gesture gesture) {
		if(gesture.pickObject==gameObject)
		{
			
			if(SoundTimer < 0.0f){
				
				string baclip="benny_phoneme_" + PD.Grapheme.ToLower() + "_" + PD.Phoneme.ToLower() + "_" + PD.Mneumonic.ToLower();
				 baclip= baclip.Replace(" ", "_");
				Debug.Log("Looking for :"+ baclip);
				audio.clip=  (AudioClip)Resources.Load("audio/benny_phonemes_master/"+ baclip);
				audio.Play ();
				SoundTimer = 1.0f;
				_yellowHighlight.SetActive(true);
				_button.PressDown();
				if(_yellowHighlightDia != null)
				{
					_DiaButton.PressDown();
					_yellowHighlightDia.SetActive(true);
				}
			}
		}
		
	}
}

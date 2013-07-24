using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using AlTypes;



public class TrophyRoomManager : MonoBehaviour {
	
	public Transform ShieldsHolder;
	public bool PlayingAudio;
	public Transform[] ColourShields;
	public Transform GreyShield;
	public Transform Label;
	public Transform cMnemonic;
	public Transform pMnemonic;
	public Transform sMnemonic;
	public AudioClip cAudio;
	public AudioClip pAudio;
	public AudioClip sAudio;
	public Transform BigShieldPrefab;
	public Texture2D MnemonicA;
	public Texture2D MnemonicP;
	public Texture2D MnemonicS;
	public Texture2D MnemonicT;
	public OTSpriteAtlasCocos2DFnt ShieldFont;
	public bool DisableTouches;
	public BigShield CurrentBigShield;
	int ShieldRowIndex=0;
	int InverseShieldRowIndex=0;
	float startXPos=-380.0f;
	float startYPos=190.0f; // was 140
	float incXPos=190.0f;
	float incYPos=190.0f;
	bool ShieldsMoving=false;
	ArrayList ShieldRows;

	GameManager cmsLink;
	
	// Use this for initialization
	void Start () {
		cmsLink=GameManager.Instance;
		string[] letters=cmsLink.GetUserLetters();
		string[] mneumonics=cmsLink.AllMneumonics();
		ShieldRows=new ArrayList();

		ArrayList thisRow=new ArrayList();
		
		int curX=0;
		int curY=0;
		
		int mneumonicIndex=0;
		
		for(int i=0;i<letters.Length;i++)
		{
			if(!ShieldRows.Contains(thisRow))
				ShieldRows.Add(thisRow);

			Transform gshield=(Transform)Instantiate(GreyShield);
			gshield.parent=ShieldsHolder;
			OTSprite gs=gshield.GetComponent<OTSprite>();
			
			gs.position=new Vector2(startXPos+(curX*incXPos),startYPos-(curY*incYPos));
			ShieldTouch spref=gshield.GetComponent<ShieldTouch>();
			Transform tLabel=(Transform)Instantiate(Label);
			tLabel.parent=ShieldsHolder;

			OTTextSprite txt=tLabel.GetComponent<OTTextSprite>();
			// txt.spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTSpriteAtlasCocos2DFnt>();
			txt.spriteContainer=ShieldFont;
			txt.ForceUpdate();
			txt.position=gs.position;
			txt.text=letters[i];
			txt.size=new Vector2(1.0f, 1.0f);
			spref.colourText=tLabel;
			
			PhonemeData thisP;

			// List<PhonemeData> pd=GameManager.Instance.GetPhonemesForWord(txt.text);
			if(txt.text=="q")
				thisP=GameManager.Instance.GetPhonemeInfoForPhoneme("qu");
			else 
				thisP=GameManager.Instance.GetPhonemeInfoForPhoneme(txt.text);
			// string mn=mneumonics[mneumonicIndex];
			string mn=(string)thisP.Mneumonic.ToString();
			spref.myMnemonic=mn;
			mn=txt.text+"_"+mn+"_0.png";
			
			Debug.Log ("current letter is "+txt.text+" / mnemonic is "+mn.Replace(" ", "_").ToLower ());
			
			mneumonicIndex++;

			thisRow.Add(gs);
			
			// if(letters[i]=="p"||letters[i]=="c"||letters[i]=="s")
			// {
			// 	int ShieldIndex=UnityEngine.Random.Range (0,3);
			// 	Transform cshield=(Transform)Instantiate(ColourShields[ShieldIndex]);
			// 	cshield.parent=ShieldsHolder;
				
			// 	OTSprite cs=cshield.GetComponent<OTSprite>();
			// 	cs.position=gs.position;
			// 	spref.colourShield=cshield;
				
			// 	if(letters[i]=="p"){
			// 		spref.Mnemonic=pMnemonic;
			// 		spref.MyAudio=pAudio;
			// 	}
			// 	else if(letters[i]=="s"){
			// 		spref.Mnemonic=sMnemonic;
			// 		spref.MyAudio=sAudio;
			// 	}
			// 	else if(letters[i]=="c"){
			// 		spref.Mnemonic=cMnemonic;
			// 		spref.MyAudio=cAudio;
			// 	}
				
			// 	if(spref.Mnemonic!=null)
			// 	{
			// 		OTSprite m=spref.Mnemonic.GetComponent<OTSprite>();
			// 		m.position=cs.position;
			// 	}
			// }
			
			curX++;
			if(curX>4)
			{
				curX=0;
				curY++;
				thisRow=new ArrayList();
			}
		}

		MoveShields(0);

		Debug.Log("shieldrows: "+ShieldRows.Count);
		foreach(ArrayList al in ShieldRows)
		{
			Debug.Log("count in this "+al.Count);
		}
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_TouchDown -= On_TouchDown;	
	}
	
	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name=="ArrowDown")
			{
				MoveShields(1);
			}
			else if(gesture.pickObject.name=="ArrowUp")
			{
				MoveShields(-1);
			}
		}
	}

	void On_TouchDown(Gesture gesture)
	{
		if(DisableTouches){
			CurrentBigShield.HideAndEnableTouches();
			return;
		}

		ShieldsHolder.position=new Vector3(ShieldsHolder.position.x,ShieldsHolder.position.y+gesture.deltaPosition.y,ShieldsHolder.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(audio.isPlaying)
			PlayingAudio=true;
		else
			PlayingAudio=false;
		
				
		float newYPos=0;
		
		if(Input.GetKey("up")&&!DisableTouches)
		{
			MoveShields(-1);
			//newYPos=ShieldsHolder.position.y+3.0f;
			//ShieldsHolder.position=new Vector3(ShieldsHolder.position.x, newYPos, ShieldsHolder.position.z);
		}
		else if(Input.GetKey ("down")&&!DisableTouches)
		{
			MoveShields(1);
			// newYPos=ShieldsHolder.position.y-3.0f;
			// ShieldsHolder.position=new Vector3(ShieldsHolder.position.x, newYPos, ShieldsHolder.position.z);
		}
	}

	void MoveShields(int dir)
	{
		if(ShieldsMoving)return;
		ShieldsMoving=true;
		ShieldRowIndex+=dir;
		InverseShieldRowIndex-=dir;

		foreach(ArrayList al in ShieldRows)
		{
			int thisInd=ShieldRows.IndexOf(al);
			
			if(thisInd>=InverseShieldRowIndex && thisInd<InverseShieldRowIndex+3)
			{
				foreach(OTSprite s in al)
				{
					s.gameObject.SetActive(true);
					ShieldTouch spref=s.GetComponent<ShieldTouch>();
					spref.colourText.gameObject.SetActive(true);
				}
			}
			else
			{
				foreach(OTSprite s in al)
				{
					s.gameObject.SetActive(false);
					ShieldTouch spref=s.GetComponent<ShieldTouch>();
					spref.colourText.gameObject.SetActive(false);
				}
			}
		}
		var config=new GoTweenConfig()
			.position( new Vector3(0.0f, -(ShieldRowIndex*incYPos), 0.0f) )
			.setEaseType( GoEaseType.BounceOut );

	
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(ShieldsHolder, 0.3f, config);
		tween.setOnCompleteHandler(c => EnableMoving());
		Go.addTween(tween);		
	}

	void EnableMoving()
	{
		ShieldsMoving=false;
	}
	
	public void PlayAudioClip(AudioClip thisClip)
	{
		audio.clip=thisClip;
		audio.Play();
	}

	public void CreateBigShield(string letter, string mnemonic)
	{
		if(CurrentBigShield!=null)return;
		// if(letter!="a"&&letter!="p"&&letter!="s"&&letter!="t")return;
		Transform newshield=(Transform)Instantiate(BigShieldPrefab);
		BigShield bspref=newshield.GetComponent<BigShield>();
		CurrentBigShield=bspref;
		Texture2D thisImg=(Texture2D)Resources.Load("Images/mnemonics_images_png_250/"+letter+"_"+mnemonic.Replace(" ","_"));

		bspref.DisplayString=mnemonic;
		bspref.MyLetter=letter;

		if(thisImg==null)
			Debug.Log("Load failed for "+letter+"_"+mnemonic.Replace(" ","_"));
		else 
			bspref.DisplayImage=thisImg;

	}
}

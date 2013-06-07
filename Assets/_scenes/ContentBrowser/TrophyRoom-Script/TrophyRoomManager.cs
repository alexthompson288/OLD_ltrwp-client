using UnityEngine;
using System.Collections;

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

	GameManager cmsLink;
	
	// Use this for initialization
	void Start () {
		cmsLink=GameManager.Instance;
		string[] letters=cmsLink.GetUserLetters();
		string[] mneumonics=cmsLink.AllMneumonics();
		
		
		int curX=0;
		int curY=0;
		float startXPos=-380.0f;
		float startYPos=140.0f;
		float incXPos=190.0f;
		float incYPos=190.0f;
		
		int mneumonicIndex=0;
		
		for(int i=0;i<letters.Length;i++)
		{
			Transform gshield=(Transform)Instantiate(GreyShield);
			gshield.parent=ShieldsHolder;
			OTSprite gs=gshield.GetComponent<OTSprite>();
			
			gs.position=new Vector2(startXPos+(curX*incXPos),startYPos-(curY*incYPos));
			ShieldTouch spref=gshield.GetComponent<ShieldTouch>();
			Transform tLabel=(Transform)Instantiate(Label);
			tLabel.parent=ShieldsHolder;

			OTTextSprite txt=tLabel.GetComponent<OTTextSprite>();
			txt.spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTSpriteAtlasCocos2DFnt>();
			txt.ForceUpdate();
			txt.position=gs.position;
			txt.text=letters[i];
			txt.size=new Vector2(1.0f, 1.0f);
			spref.colourText=tLabel;
			
			string mn=mneumonics[mneumonicIndex];
			mn=txt.text+"_"+mn+"_0.png";
			
			Debug.Log ("current letter is "+txt.text+" / mneumonic is "+mn.Replace(" ", "_").ToLower ());
			
			mneumonicIndex++;
			
			if(letters[i]=="p"||letters[i]=="c"||letters[i]=="s")
			{
				int ShieldIndex=Random.Range (0,3);
				Transform cshield=(Transform)Instantiate(ColourShields[ShieldIndex]);
				cshield.parent=ShieldsHolder;
				
				OTSprite cs=cshield.GetComponent<OTSprite>();
				cs.position=gs.position;
				spref.colourShield=cshield;
				
				if(letters[i]=="p"){
					spref.Mnemonic=pMnemonic;
					spref.MyAudio=pAudio;
				}
				else if(letters[i]=="s"){
					spref.Mnemonic=sMnemonic;
					spref.MyAudio=sAudio;
				}
				else if(letters[i]=="c"){
					spref.Mnemonic=cMnemonic;
					spref.MyAudio=cAudio;
				}
				
				if(spref.Mnemonic!=null)
				{
					OTSprite m=spref.Mnemonic.GetComponent<OTSprite>();
					m.position=cs.position;
				}
			}
			
			curX++;
			if(curX>4)
			{
				curX=0;
				curY++;
			}
		}
	}
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;	
	}
	
	void On_TouchDown(Gesture gesture)
	{
		ShieldsHolder.position=new Vector3(ShieldsHolder.position.x,ShieldsHolder.position.y+gesture.deltaPosition.y,ShieldsHolder.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(audio.isPlaying)
			PlayingAudio=true;
		else
			PlayingAudio=false;
		
				
		float newYPos=0;
		
		if(Input.GetKey("up"))
		{
			newYPos=ShieldsHolder.position.y+3.0f;
			ShieldsHolder.position=new Vector3(ShieldsHolder.position.x, newYPos, ShieldsHolder.position.z);
		}
		else if(Input.GetKey ("down"))
		{
			newYPos=ShieldsHolder.position.y-3.0f;
			ShieldsHolder.position=new Vector3(ShieldsHolder.position.x, newYPos, ShieldsHolder.position.z);
		}
	}
	
	public void PlayAudioClip(AudioClip thisClip)
	{
		audio.clip=thisClip;
		audio.Play();
	}

	public void CreateBigShield(string letter)
	{
		if(letter!="a"&&letter!="p"&&letter!="s"&&letter!="t")return;
		Transform newshield=(Transform)Instantiate(BigShieldPrefab);
		BigShield bspref=newshield.GetComponent<BigShield>();

		if(letter=="a")
		{
			bspref.DisplayImage=MnemonicA;
			bspref.DisplayString="angry anus";
		}
		else if(letter=="p")
		{
			bspref.DisplayImage=MnemonicP;
			bspref.DisplayString="pasta penis";	
		}
		else if(letter=="s")
		{
			bspref.DisplayImage=MnemonicS;
			bspref.DisplayString="squelchy sausage";		
		}
		else if(letter=="t")
		{
			bspref.DisplayImage=MnemonicT;
			bspref.DisplayString="tasty twat";
		}

	}
}

using UnityEngine;
using System.Collections;
using AlTypes;

public class IntroducingPhonemeManager : MonoBehaviour {
	
	public Transform[] OuterCurtains;
	PersistentObject PersistentManager;
	public bool exitCountdown;
	float countdownToExit=8.0f;//8.0f;
	public bool isReadingIntro=true;
	public SmokeAnimation Smoke;
	public Transform[] LeftTrumpets;
	public Transform[] RightTrumpets;
	public string TargetPhoneme;
	public bool disableTouches;
	PhonemeData[] phoneme;
	DataWordData[] datawords;
	int currentImageIndex=0;
	
	void Awake() {
		HideTrumpets();
		phoneme=GameManager.Instance.SessionMgr.CurrentPhonemes;
		datawords=GameManager.Instance.SessionMgr.CurrentDataWords;

		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!audio.isPlaying && isReadingIntro)
			isReadingIntro=false;
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0)
			GameManager.Instance.SessionMgr.CloseActivity();
//			Application.LoadLevel(PersistentManager.ContentBrowserName);
		
		TintNextCurtains();
		
	}
	
	public bool HasOpenedOutsideCurtains(){
		int countOpen=0;
		foreach(Transform t in OuterCurtains)
		{
			CurtainAnimation ca=t.gameObject.GetComponent<CurtainAnimation>();
			
			if(ca.isOpen)countOpen++;
		}
		
		if(countOpen==OuterCurtains.Length)
			return true;
		else
			return false;
	}
	
	public void TintNextCurtains(){
		
		if(isReadingIntro)return;
		
		Color colorTintTo=new Color(0.0f,1.0f,0.0f,1.0f);
		Color colorTintFrom=new Color(1.0f,1.0f,1.0f,1.0f);
		
		foreach(Transform t in OuterCurtains)
		{
			CurtainAnimation ca=t.gameObject.GetComponent<CurtainAnimation>();
			OTSprite s=t.gameObject.GetComponent<OTSprite>();
			
			if(ca.isOpen)
			{
				if(!s.tintColor.Equals(colorTintFrom)){
					OTTween np1=new OTTween(s,0.8f, OTEasing.Linear);
					np1.Tween("tintColor", colorTintFrom);
				}
			}
			else 
			{
				
//				Debug.Log ("cur col "+s.tintColor+" // from col "+colorTintFrom+" // to col "+colorTintTo);
				
				if(s.tintColor.Equals(colorTintFrom)){
//					Debug.Log ("colour match tint from");
					OTTween np1=new OTTween(s,0.8f, OTEasing.Linear);
					np1.Tween("tintColor", colorTintTo);
				}
				else if(s.tintColor.Equals (colorTintTo))
				{
//					Debug.Log ("colour match tint to");
					OTTween np1=new OTTween(s,0.8f, OTEasing.Linear);
					np1.Tween("tintColor", colorTintFrom);					
				}
				break;
			}
		}
		
	}

	public string GetCurrentImage(){
		DataWordData dw=datawords[currentImageIndex];
		string image=(string)dw.Word;


		currentImageIndex++;
		return image;
	}

	public PhonemeData GetCurrentPhoneme(){
		PhonemeData pd=phoneme[0];
		return pd;
	}

	public void ShowTrumpets()
	{
		Vector2 newSize=new Vector2(371,407);
		var config=new GoTweenConfig()
			.vector2Prop( "size", newSize )
			.setEaseType( GoEaseType.Punch );

		foreach(Transform t in LeftTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=true;

			GoTween tween=new GoTween(s, 0.2f, config);
			Go.addTween(tween);
		}
		foreach(Transform t in RightTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=true;

			GoTween tween=new GoTween(s, 0.2f, config);
			Go.addTween(tween);
		}
	}

	public void HideTrumpets()
	{
		foreach(Transform t in LeftTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=false;
		}
		foreach(Transform t in RightTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=false;
		}
	}

	public void PlayAudio(AudioClip thisClip)
	{
		audio.clip=thisClip;
		audio.Play();
	}
}

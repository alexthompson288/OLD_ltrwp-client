using UnityEngine;
using System.Collections;
using AlTypes;

public class IntroducingPhonemeManager : MonoBehaviour {
	
	public Transform[] OuterCurtains;
	PersistentObject PersistentManager;
	public bool exitCountdown;
	float countdownToExit=30.0f;//8.0f;
	public bool isReadingIntro=true;
	public SmokeAnimation Smoke;
	public Transform[] LeftTrumpets;
	public Transform[] RightTrumpets;
	PhonemeData[] phoneme;
	DataWordData[] datawords;
	
	void Awake() {
		HideTrumpets();
	}

	// Use this for initialization
	void Start () {

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

	public void ShowTrumpets()
	{
		foreach(Transform t in LeftTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=true;
		}
		foreach(Transform t in RightTrumpets)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.visible=true;
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

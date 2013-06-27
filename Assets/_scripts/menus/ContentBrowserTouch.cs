using UnityEngine;
using System.Collections;

public class ContentBrowserTouch : MonoBehaviour {
	
	PersistentObject PersistentManager;
	public AudioClip TapSound;
	public Texture2D fzOpen;
	public Texture2D fzClose;
	bool TintPickObject;
	Transform funZone;
	bool isTouching=false;
	MoveCamera cameraMover;
	bool fzVisible;
	string NextSceneType="last";
	int currentMapIndex=0;
	ContentBrowserManager cbMan;
	
	public Transform[] LayersToMove;
	
	float countdownToNewScene=1.0f;
	bool countdown=false;
	
	void Awake() {
		cbMan=GameObject.Find("Main Camera").GetComponent<ContentBrowserManager>();
	}

	// Use this for initialization
	void Start () {
		ReadPersistentObjectSettings();	
		
		funZone=null;
		
		if(GameObject.Find ("FunZoneLayer")!=null)
			funZone=GameObject.Find ("FunZoneLayer").GetComponent<Transform>();
		
		cameraMover=GameObject.Find ("Main Camera").GetComponent<MoveCamera>();
		
		if(funZone!=null){
			OTSprite fz=funZone.GetComponent<OTSprite>();
			// fz.visible=false;
		}
	}
	

	void ReadPersistentObjectSettings(){
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		PersistentManager.ContentBrowserName="ContentBrowser-Scrolling";
		
	}
	
	// Update is called once per frame
	void Update () {
		if(countdown)
			countdownToNewScene-=Time.deltaTime;
		
		if(countdownToNewScene<0)
		{
			if(NextSceneType=="last")
				Application.LoadLevel(PersistentManager.LastScene);
			else if(NextSceneType=="current")
				Application.LoadLevel(PersistentManager.NextLevel);
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
	
	
	void On_TouchDown(Gesture gesture){
		if(Application.loadedLevelName!="ContentBrowser-Scrolling")return;
		foreach(Transform t in LayersToMove)
		{
			t.position=new Vector3(t.position.x,t.position.y+gesture.deltaPosition.y,t.position.z);
		}
	}
	
		
	// Simple tap
	private void On_SimpleTap(Gesture gesture){
		
		if(gesture.pickObject==null)return;
		
		if(funZone!=null){
		
			OTSprite fz=funZone.GetComponent<OTSprite>();
			
			if(gesture.pickObject.name=="btnFunZone" && fzVisible)
			{
				OTSprite fzbtn=gesture.pickObject.GetComponent<OTSprite>();
				
				iTween.MoveTo(fz.gameObject, iTween.Hash("position", new Vector3(1024.0f,0.0f,-100.1024f), "easetype",iTween.EaseType.easeInOutSine, "time",0.5f));
				fzbtn.position=new Vector2(475,0);
				fzbtn.image=fzOpen;
				fzVisible=false;
				return;
			}
			else if(gesture.pickObject.name=="btnFunZone" && !fzVisible)
			{
				OTSprite fzbtn=gesture.pickObject.GetComponent<OTSprite>();
				iTween.MoveTo(fz.gameObject, iTween.Hash("position", new Vector3(0.0f,0.0f,-100.0f), "easetype",iTween.EaseType.easeInOutSine, "time",0.5f));
				fzbtn.position=new Vector2(-475,0);
				fzbtn.image=fzClose;
				fzVisible=true;
				return;
			}
			else if(gesture.pickObject.name=="btnMPSEasy" && fz.visible)
			{
				PersistentManager.Players=2;
				PersistentManager.LastScene=Application.loadedLevelName;
				Application.LoadLevel(PersistentManager.NextLevel);
			}
			else if(gesture.pickObject.name=="btnMPSMedium" && fz.visible)
			{
				PersistentManager.Players=2;
				PersistentManager.LastScene=Application.loadedLevelName;
				Application.LoadLevel(PersistentManager.NextLevel);
			}
			else if(gesture.pickObject.name=="btnMPSHard" && fz.visible)
			{
				PersistentManager.Players=2;
				PersistentManager.LastScene=Application.loadedLevelName;
				Application.LoadLevel(PersistentManager.NextLevel);
			}
			else if(gesture.pickObject.name=="btnFZ1" && fz.visible)
			{
				PersistentManager.NextLevel="SplatTheRat PX";
				Application.LoadLevel("MultiplayerSelect");
			}
			else if(gesture.pickObject.name=="btnFZ2" && fz.visible)
			{
				PersistentManager.NextLevel="MatchingBonds";
				Application.LoadLevel("MultiplayerSelect");
			}
			else if(gesture.pickObject.name=="btnFZ4" && fz.visible)
			{
				Application.LoadLevel("CertificateRoom");
			}
			else if(gesture.pickObject.name=="btnFZ6" && fz.visible)
			{
				Application.LoadLevel("PhotoBooth");
			}
		}

		Debug.Log("pickObject name is "+gesture.pickObject.name);
		
		if(gesture.pickObject.name=="btnBook")
		{
			Debug.Log ("bookpress");	
		}
		else if(gesture.pickObject.name=="btnFunnyVoices")
		{
			Application.LoadLevel("FunnyVoices");
		}
		else if(gesture.pickObject.name=="btnLetterFormation")
		{
			Debug.Log ("scribbles");
		}	
		else if(gesture.pickObject.name=="btnSegmentPip")
		{
			Application.LoadLevel("Segmenting-Pip");
		}	
		else if(gesture.pickObject.name=="btnSegmentSip")
		{
			Application.LoadLevel("Segmenting-Sip");
		}	
		else if(gesture.pickObject.name=="btnSegmentSit")
		{
			Application.LoadLevel("Segmenting-Sit");
		}	
		else if(gesture.pickObject.name=="btnSegmentTip")
		{
			Application.LoadLevel("Segmenting-Tip");
		}	
		else if(gesture.pickObject.name=="btnSplat")
		{
			Application.LoadLevel("Balls");
		}		
		else if(gesture.pickObject.name=="btnLetterFormationG")
		{
			Application.LoadLevel("follow-g");
		}
		else if(gesture.pickObject.name=="btnLetterFormationC")
		{
			Application.LoadLevel("follow-c");
		}
		else if(gesture.pickObject.name=="btnLetterFormationB")
		{
			Application.LoadLevel("follow-b");
		}
		else if(gesture.pickObject.name=="btnDebugMenu")
		{
			Application.LoadLevel("DebugMenu");
		}
		else if(gesture.pickObject.name=="btn0-1")
		{
			TintPickObject=true;
			Application.LoadLevel("IntroducePhoneme A");	
		}
		else if(gesture.pickObject.name=="btn0-2")
		{
			TintPickObject=true;
			Application.LoadLevel("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btn0-3")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="a";
			Application.LoadLevel("SplatTheRat PX");
		}
		else if(gesture.pickObject.name=="btn0-4")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="a";
			Application.LoadLevel("AlphabetBook");
		}
		else if(gesture.pickObject.name=="btn1-1")
		{
			TintPickObject=true;
			Application.LoadLevel("IntroducePhoneme T");	
		}
		else if(gesture.pickObject.name=="btn1-2")
		{
			TintPickObject=true;
			Application.LoadLevel("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btn1-3")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="t";
			Application.LoadLevel("SplatTheRat PX");
		}
		else if(gesture.pickObject.name=="btn1-4")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="t";
			Application.LoadLevel("AlphabetBook");
		}
		else if(gesture.pickObject.name=="btn2-1")
		{
			TintPickObject=true;
			Application.LoadLevel("IntroducePhoneme S");	
		}
		else if(gesture.pickObject.name=="btn2-2")
		{
			TintPickObject=true;
			Application.LoadLevel("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btn2-3")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="s";	
			Application.LoadLevel("SplatTheRat PX");
		}
		else if(gesture.pickObject.name=="btn2-4")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="s";
			Application.LoadLevel("AlphabetBook");
		}
		else if(gesture.pickObject.name=="btn3-1")
		{
			TintPickObject=true;
			Application.LoadLevel("IntroducePhoneme P");	
		}
		else if(gesture.pickObject.name=="btn3-2")
		{
			TintPickObject=true;
			Application.LoadLevel("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btn3-3")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="p";
			Application.LoadLevel("SplatTheRat PX");
		}
		else if(gesture.pickObject.name=="btn3-4")
		{
			TintPickObject=true;
			PersistentManager.CurrentLetter="p";	
			Application.LoadLevel("AlphabetBook");
		}
		else if(gesture.pickObject.name=="btn1")
		{
			TintPickObject=true;
			Application.LoadLevel("Splat-SAT");	
		}
		else if(gesture.pickObject.name=="btn2")
		{
			TintPickObject=true;
			Application.LoadLevel("SplatTheRat PX");
		}
		else if(gesture.pickObject.name=="btn3")
		{
			TintPickObject=true;
			Application.LoadLevel("WordLadder");
		}
		else if(gesture.pickObject.name=="btn4")
		{
			TintPickObject=true;;
			Application.LoadLevel("follow-g");
		}
		else if(gesture.pickObject.name=="btn5")
		{
			TintPickObject=true;
			Application.LoadLevel("follow-s-2d");
		}
		else if(gesture.pickObject.name=="btn6")
		{
			TintPickObject=true;
			Application.LoadLevel("AlphabetBook");
		}
		else if(gesture.pickObject.name=="btn7")
		{
			TintPickObject=true;
			Application.LoadLevel("IntroducePhoneme");
		}
		else if(gesture.pickObject.name=="btnExit"||gesture.pickObject.name=="btnBackmark")
		{
			TintPickObject=true;
			countdown=true;
			OTTween fo=new OTTween(gesture.pickObject.GetComponent<OTSprite>(), 0.8f, OTEasing.BounceIn);
			fo.Tween("position", new Vector2(gesture.pickObject.GetComponent<OTSprite>().position.x,gesture.pickObject.GetComponent<OTSprite>().position.y+200.0f));	
//			OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
//			OTTween mt=new OTTween(s,0.4f, OTEasing.BackIn);
//			mt.Tween("position", new Vector2(gameObject.transform.position.x,gameObject.transform.position.y+100));
			
			// Application.LoadLevel(PersistentManager.ContentBrowserName);

			GameManager.Instance.SessionMgr.CloseActivity();
		}
		else if(gesture.pickObject.name=="UpBtn")
		{
			MoveMap(-1);
		}
		else if(gesture.pickObject.name=="DownBtn")
		{
			MoveMap(1);
		}
		else if(gesture.pickObject.name=="btnBalAni")
		{
			Debug.Log ("loaded level name: "+Application.loadedLevelName);
			if(Application.loadedLevelName=="ContentBrowser-Scrolling"){
				Application.LoadLevel("BookMenu");
			}
			else if(Application.loadedLevelName=="SplatTheRat PX"){
				GameObject.Find ("Main Camera").GetComponent<SplatTheRatManager>().PlayIntro();
			}
			else if(Application.loadedLevelName=="Splat-SAT"){
				GameObject.Find ("Main Camera").GetComponent<SplatManager>().PlayIntroductionAudio();
			}
			else if(Application.loadedLevelName=="WordLadder"){
				GameObject.Find("Main Camera").GetComponent<SegmentingManager>().PlayIntro();
			}
			else{
				PersistentManager.NextLevel="BookMenu";
				gesture.pickObject.GetComponent<BalAnimation>().StopAll();
				gesture.pickObject.GetComponent<BalAnimation>().playOpening=true;
				gesture.pickObject.GetComponent<BalAnimation>().loadOnFinish=true;				
			}
		}
		else
		{
			return;	
		}
		

		if(TintPickObject)
		{
			OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
			s.tintColor=new Color(255.0f,0.0f,0.0f,255.0f);
		}
		
		PersistentManager.PlayAudioClip(TapSound);
			
	}

	void MoveMap(int direction)
	{
		Debug.Log("MoveMap");
		currentMapIndex+=direction;

		if(currentMapIndex<0)
			currentMapIndex=0;
		else if(currentMapIndex>6)
			currentMapIndex=6;

		cbMan.UpdateAudio(currentMapIndex);

		var config=new GoTweenConfig()
			.position( new Vector3(0.0f, -(currentMapIndex*825.0f), -1000.0f) )
			.setEaseType( GoEaseType.BounceOut );

	
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(camera.transform, 0.3f, config);

		Go.addTween(tween);
	}


}

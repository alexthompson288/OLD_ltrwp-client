using UnityEngine;
using System.Collections;

public class ChallengeMenuTouch : MonoBehaviour {

	PersistentObject PersistentManager;
	public GameObject LearnBoard;
	public GameObject PlayBoard;
	public TransitionScreen _transitionScreen;
	
	public ButtonSlideInScript [] _levelButtons;

	void Awake() {
		GameManager.Instance.SessionMgr.ReturnSceneForActivityClose="ChallengeMenu";
	}

	// Use this for initialization
	void Start () {
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}

		PersistentManager.Players=1;
		_transitionScreen = GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>();
		
		
		// bring down boards
		var config=new GoTweenConfig()
			.vector3Prop( "position", new Vector3(-294.9874f, 165.692f, -12f) )
			.setDelay( 0.4f)
			.setEaseType( GoEaseType.BounceOut );
		GoTween tween=new GoTween(LearnBoard.transform, 1.2f, config);
		Go.addTween(tween);
		StartCoroutine( LearnBoard.transform.GetComponent<Wobble>().SeedWobblesDelayed(1.0f, 7.0f, 1.2f + 0.4f));
		
		config=new GoTweenConfig()
			.vector3Prop( "position", new Vector3(296.5377f, 165.692f, -12f) )
			.setDelay( 0.8f)
			.setEaseType( GoEaseType.BounceOut );
		tween=new GoTween(PlayBoard.transform, 1.2f, config);
		Go.addTween(tween);
		StartCoroutine( PlayBoard.transform.GetComponent<Wobble>().SeedWobblesDelayed(1.0f, 7.0f, 1.2f + 0.8f));
	}
	
	// Update is called once per frame
	void Update () {
	
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

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			
			if(gesture.pickObject.name=="LearnBoard")
			{
				_transitionScreen.ChangeLevel("LetterBankGame");
			}
			if(gesture.pickObject.name=="PlayBoard" && PlayBoard.transform.GetComponent<Wobble>().isActive)
			{
				var config=new GoTweenConfig()
					.vector3Prop( "position", new Vector3(-294.9874f, 774f, -12f) )
					.setDelay( 0.0f)
					.setEaseType( GoEaseType.QuadIn );
				GoTween tween=new GoTween(LearnBoard.transform, 0.6f, config);
				Go.addTween(tween);
				
				config=new GoTweenConfig()
					.vector3Prop( "position", new Vector3(294.5377f, 774f, -12f) )
					.setDelay( 0.4f)
					.setEaseType( GoEaseType.QuadIn );
				tween=new GoTween(PlayBoard.transform, 0.6f, config);
				Go.addTween(tween);
				PlayBoard.transform.GetComponent<Wobble>().isActive = false;
				LearnBoard.transform.GetComponent<Wobble>().isActive = false;
				
				for(int i = 0; i < _levelButtons.Length; i++)
					_levelButtons[i].SlideIn(0.8f, 0.25f * i + 0.7f);
			}
			if(gesture.pickObject.name=="btn1Castle")
			{
				_transitionScreen.ChangeLevel("Splat");
				GameManager.Instance.SessionMgr.StartSession(9119);
			}
			else if(gesture.pickObject.name=="btn2Earth")
			{
				_transitionScreen.ChangeLevel("LetterBankGame");
				GameManager.Instance.SessionMgr.StartSession(9122);
			}
			else if(gesture.pickObject.name=="btn3Farm")
			{
				_transitionScreen.ChangeLevel("PictureGame");
				GameManager.Instance.SessionMgr.StartSession(9120);
			}
			else if(gesture.pickObject.name=="btn4Forest")
			{
				_transitionScreen.ChangeLevel("SplatFalling");
				GameManager.Instance.SessionMgr.StartSession(9124);
			}
			else if(gesture.pickObject.name=="btn5Space")
			{
				_transitionScreen.ChangeLevel("SplatTheRatPX");
				GameManager.Instance.SessionMgr.StartSession(9125);
			}
			else if(gesture.pickObject.name=="btn6Underwater")
			{
				GameManager.Instance.SessionMgr.StartSession(9126);
			}

			GameManager.Instance.SessionMgr.StartActivity();
		}
	}
}

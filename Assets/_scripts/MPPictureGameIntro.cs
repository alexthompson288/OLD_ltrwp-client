using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MPPictureGameIntro : MonoBehaviour {
	
	public Transform Player1CharacterSelection;	
	public Transform Player2CharacterSelection;
	
	public GameObject TwoPlayerBoard;
	public GameObject SinglePlayerBoard;
	
	public GameObject EasyBoard;
	public GameObject MediumBoard;
	public GameObject HardBoard;
	
	public MPPictureGame player1Manager;
	public MPPictureGame player2Manager;
	
	PersistentObject PersistentManager;
	
	// these objects get destroyed before starting the MP game
	public GameObject []IntroObjects;
	
	public bool isSinglePlayer = false;
	
	public TransitionScreen transitionScreen;

	// Use this for initialization
	void Start ()
	{		
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}	
		
		transitionScreen = GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>();
		
		iTween.MoveTo( SinglePlayerBoard, iTween.Hash(	"position", SinglePlayerBoard.transform.position - new Vector3(0.0f, 550.0f, 0.0f),
													"time", 1.28f,
													"delay", 0.8f,
													"easetype", iTween.EaseType.easeOutBounce));
		
		iTween.MoveTo( TwoPlayerBoard, iTween.Hash(	"position", TwoPlayerBoard.transform.position - new Vector3(0.0f, 550.0f, 0.0f),
													"time", 1.28f,
													"delay", 1.0f,
													"easetype", iTween.EaseType.easeOutBounce));
													
			
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
	
	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture) 
	{
		if(gesture.pickObject == null)
			return;
		
		if(gesture.pickObject== SinglePlayerBoard)
		{
			isSinglePlayer = true;
			SinglePlayerBoard.GetComponent<OTSprite>().collidable = false;
			TwoPlayerBoard.GetComponent<OTSprite>().collidable = false;
			
			iTween.MoveTo( EasyBoard, iTween.Hash(	"position", EasyBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
													"time", 1.2f,
													"delay", 0.2f,
													"easetype", iTween.EaseType.easeOutBounce));
			
			iTween.MoveTo( MediumBoard, iTween.Hash(	"position", MediumBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
												"time", 1.2f,
												"delay", 0.4f,
												"easetype", iTween.EaseType.easeOutBounce));
			
			iTween.MoveTo( HardBoard, iTween.Hash(	"position", HardBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
												"time", 1.2f,
												"delay", 0.6f,
												"easetype", iTween.EaseType.easeOutBounce));
	
		}else if(gesture.pickObject== TwoPlayerBoard)
		{
			isSinglePlayer = false;
			SinglePlayerBoard.GetComponent<OTSprite>().collidable = false;
			TwoPlayerBoard.GetComponent<OTSprite>().collidable = false;
			
			iTween.MoveTo( EasyBoard, iTween.Hash(	"position", EasyBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
													"time", 1.2f,
													"delay", 0.2f,
													"easetype", iTween.EaseType.easeOutBounce));
			
			iTween.MoveTo( MediumBoard, iTween.Hash(	"position", MediumBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
												"time", 1.2f,
												"delay", 0.4f,
												"easetype", iTween.EaseType.easeOutBounce));
			
			iTween.MoveTo( HardBoard, iTween.Hash(	"position", HardBoard.transform.position + new Vector3(0.0f, 250.0f, 0.0f),
												"time", 1.2f,
												"delay", 0.6f,
												"easetype", iTween.EaseType.easeOutBounce));			
		}
		
		if(gesture.pickObject== EasyBoard)
		{
			if(isSinglePlayer)
			{
				PersistentManager.SectionID = 1388;
				transitionScreen.ChangeLevel("GameWordBank");	
			}else{
				transitionScreen.FlashTransitionScreen();
				foreach(GameObject go in IntroObjects)
					Destroy(go,0.9f);
				
				iTween.MoveTo( Player1CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(-504.0f, 172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				
				iTween.MoveTo( Player2CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(506.0f, -172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				player1Manager.LoadWords(0);
				player2Manager.LoadWords(0);
			}
		}else if(gesture.pickObject== MediumBoard){
			if(isSinglePlayer)
			{
				PersistentManager.SectionID = 1389;
				transitionScreen.ChangeLevel("GameWordBank");	
			}else{
				transitionScreen.FlashTransitionScreen();
				foreach(GameObject go in IntroObjects)
					Destroy(go,0.9f);
				
				iTween.MoveTo( Player1CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(-504.0f, 172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				
				iTween.MoveTo( Player2CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(506.0f, -172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				
				player1Manager.LoadWords(1);
				player2Manager.LoadWords(1);
			}
		}else if(gesture.pickObject== HardBoard){
			if(isSinglePlayer)
			{
				PersistentManager.SectionID = 1390;
				transitionScreen.ChangeLevel("GameWordBank");	
			}else{
				transitionScreen.FlashTransitionScreen();
				foreach(GameObject go in IntroObjects)
					Destroy(go,0.9f);
				
				iTween.MoveTo( Player1CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(-504.0f, 172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				
				iTween.MoveTo( Player2CharacterSelection.gameObject, iTween.Hash(	"position", new Vector3(506.0f, -172.0f, 0.0f),
												"time", 1.2f,
												"delay", 1.9f,
												"easetype", iTween.EaseType.easeOutBounce));
				
				player1Manager.LoadWords(2);
				player2Manager.LoadWords(2);
				
			}
		}
		
	}
}

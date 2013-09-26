using UnityEngine;
using System.Collections;

public class MenuBrowser : MonoBehaviour {

	PersistentObject PersistentManager;
	public AudioClip NavigationTap;
	float countdownToNewScene=1.0f;
	bool countdown=false;
	
	// Use this for initialization
	void Start () {
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		PersistentManager.ContentBrowserName="ContentBrowser-Full";
		PersistentManager.LastScene=PersistentManager.CurrentScene;
		PersistentManager.CurrentScene=Application.loadedLevelName;
		
		OTTextSprite LevelNameText=null;
		
		if(GameObject.Find ("LevelName")!=null)
			LevelNameText=GameObject.Find ("LevelName").GetComponent<OTTextSprite>();
		
		if(LevelNameText!=null)
			LevelNameText.text=Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update () {
		if(countdown)
			countdownToNewScene-=Time.deltaTime;
		
		if(countdownToNewScene<0)
		{
			Debug.Log ("timed escape");
			countdown=false;
			countdownToNewScene=1.0f;
			Application.LoadLevel(PersistentManager.LastScene);
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
	

	private void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null && !gesture.pickObject.name.Contains("Letter") && !gesture.pickObject.name.Contains("Play") && !gesture.pickObject.name.Contains("PipPad"))
		{
			Debug.Log ("tappy tappy!");		
			audio.clip=NavigationTap;
			audio.Play ();
			Debug.Log ("Got pickobject "+gesture.pickObject.name);
			bool TintPickObject=false;
			if(gesture.pickObject.name=="btnBackmark"||gesture.pickObject.name.StartsWith("Backmark"))
			{
				countdown=true;
				TintPickObject=true;
				Debug.Log ("backnmark: "+PersistentManager.LastScene);
				OTTween fo=new OTTween(gesture.pickObject.GetComponent<OTSprite>(), 0.8f, OTEasing.BounceIn);
				fo.Tween("position", new Vector2(gesture.pickObject.GetComponent<OTSprite>().position.x,gesture.pickObject.GetComponent<OTSprite>().position.y+200.0f));	
			}
			else if(gesture.pickObject.name=="btnBackmarkCB")
			{
				TintPickObject=true;
				countdown=true;
				Debug.Log ("Content browser backnmark: "+PersistentManager.ContentBrowserName);
				OTTween fo=new OTTween(gesture.pickObject.GetComponent<OTSprite>(), 0.8f, OTEasing.BounceIn);
				fo.Tween("position", new Vector2(gesture.pickObject.GetComponent<OTSprite>().position.x,gesture.pickObject.GetComponent<OTSprite>().position.y+200.0f));	
				PersistentManager.LastScene="ContentBrowser-Full";	
			}
			else if(gesture.pickObject.name=="btnTrophyRoom")
			{
				Application.LoadLevel ("TrophyRoom");
			}
			else if(gesture.pickObject.name=="btnCore")
			{
				Application.LoadLevel ("CoreSkills");
			}
			else if(gesture.pickObject.name=="btnLearningPages")
			{
				Application.LoadLevel("LearningPages");
			}
			else if(gesture.pickObject.name=="btnLettersPhonemes")
			{
				Application.LoadLevel("LettersIndex");
			}
			else if(gesture.pickObject.name=="btnPhotos")
			{
				Application.LoadLevel("Photos");
			}
			else if(gesture.pickObject.name=="btnSettings")
			{
				Application.LoadLevel ("Settings");
			}
			else if(gesture.pickObject.name=="btnStories")
			{
				Application.LoadLevel ("StoryBrowser");
			}
			else if(gesture.pickObject.name=="btnWordBank")
			{
				Application.LoadLevel ("WordBank");
			}
			else if(gesture.pickObject.name=="btnSet2")
			{
				PersistentManager.WordBankWord="pip";
				Application.LoadLevel("WordBank-Scaffold");
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
		}
	}
}

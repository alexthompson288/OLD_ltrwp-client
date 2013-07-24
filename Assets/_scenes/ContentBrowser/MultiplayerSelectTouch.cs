using UnityEngine;
using System.Collections;

public class MultiplayerSelectTouch : MonoBehaviour {

	PersistentObject PersistentManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake() {
		ReadPersistentObjectSettings();
	}

	void ReadPersistentObjectSettings(){
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		PersistentManager.ContentBrowserName="ContentBrowser-Full";
		
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
		if(gesture.pickObject==null)return;

		if(gesture.pickObject.name=="btnMPSEasy")
		{
			PersistentManager.Players=2;
			PersistentManager.LastScene=Application.loadedLevelName;
			Application.LoadLevel(PersistentManager.NextLevel);
		}
		else if(gesture.pickObject.name=="btnMPSMedium")
		{
			PersistentManager.Players=2;
			PersistentManager.LastScene=Application.loadedLevelName;
			Application.LoadLevel(PersistentManager.NextLevel);
		}
		else if(gesture.pickObject.name=="btnMPSHard")
		{
			PersistentManager.Players=2;
			PersistentManager.LastScene=Application.loadedLevelName;
			Application.LoadLevel(PersistentManager.NextLevel);
		}
	}
}

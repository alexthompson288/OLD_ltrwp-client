using UnityEngine;
using System.Collections;

public class ChallengeMenuTouch : MonoBehaviour {

	PersistentObject PersistentManager;

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
			if(gesture.pickObject.name=="btn1Castle")
			{
				GameManager.Instance.SessionMgr.StartSession(9119);
			}
			else if(gesture.pickObject.name=="btn2Earth")
			{
				GameManager.Instance.SessionMgr.StartSession(9122);
			}
			else if(gesture.pickObject.name=="btn3Farm")
			{
				GameManager.Instance.SessionMgr.StartSession(9120);
			}
			else if(gesture.pickObject.name=="btn4Forest")
			{
				GameManager.Instance.SessionMgr.StartSession(9121);
			}
			else if(gesture.pickObject.name=="btn5Space")
			{
				GameManager.Instance.SessionMgr.StartSession(9118);
			}
			else if(gesture.pickObject.name=="btn6Underwater")
			{
				GameManager.Instance.SessionMgr.StartSession(9123);
			}

			GameManager.Instance.SessionMgr.StartActivity();
		}
	}
}

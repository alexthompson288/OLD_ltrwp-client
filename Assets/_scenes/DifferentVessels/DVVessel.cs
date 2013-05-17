using UnityEngine;
using System.Collections;

public class DVVessel : MonoBehaviour {
	
	public string myLetter;
	DifferentVesselsManager gameManager;
	
	bool PickedUp=false;
	OTSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTSprite>();
		gameManager=GameObject.Find ("Main Camera").GetComponent<DifferentVesselsManager>();
	}
	
	public void Destroy(){
		GameObject.Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){	
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_TouchDown -= On_TouchDown;
	}
	
	void On_TouchDown(Gesture gesture){
		if(gesture.pickObject==gameObject)
		{
			PickedUp=true;
			
			myself.position+=gesture.deltaPosition;
		}
	}
	
	void On_TouchUp(Gesture gesture){
		if(gesture.pickObject==gameObject)
		{
			PickedUp=false;
			gameManager.CheckValidContainer(myself.position, gesture.pickObject.transform);
		}		
	}
}

using UnityEngine;
using System.Collections;

public class CorrectPathManager : MonoBehaviour {

	bool DisableTap=false;
	int currentIndex=0;
	int totalScalableSections=3;

	// Use this for initialization
	void Start () {
	
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

	void ReturnToMap(){
		Debug.Log("end of sections brah");
	}

	void CorrectAnswer()
	{
		currentIndex++;

		if(currentIndex>=totalScalableSections)
		{
			ReturnToMap();
		}
		else {
			var config=new GoTweenConfig()
				.position( new Vector3(Camera.main.transform.position.x, currentIndex*768.0f, Camera.main.transform.position.z), false )
				.setEaseType( GoEaseType.ExpoOut );

			GoTween tween=new GoTween(Camera.main.transform, 1.8f, config);
			tween.setOnCompleteHandler(c => EnableTaps());

			Go.addTween(tween);
		}
}

	void EnableTaps()
	{
		DisableTap=false;
	}

	void On_SimpleTap(Gesture gesture)
	{
		if(DisableTap)return;

		if(gesture.pickObject)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer thisAns=gesture.pickObject.GetComponent<GenericAnswer>();
				if(thisAns.isAnswer)
				{
					DisableTap=true;
					CorrectAnswer();
				}	

				else
				{
					Debug.Log("Incorrect");
				}
			}
		}
	}
}

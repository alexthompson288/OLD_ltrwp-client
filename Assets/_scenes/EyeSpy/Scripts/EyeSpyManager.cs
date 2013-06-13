using UnityEngine;
using System.Collections;

public class EyeSpyManager : MonoBehaviour {

	int requiredAnswers=4;
	int correctAnswers=0;

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

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer a=gesture.pickObject.GetComponent<GenericAnswer>();
				if(a.isAnswer)
				{
					correctAnswers++;
					Debug.Log("correct");
					GameObject.Destroy(gesture.pickObject);
					if(correctAnswers==requiredAnswers)
					{
						Debug.Log("target met");	
					}
				}
				else
				{
					Debug.Log("incorrect");
				}
			}
		}
	}
}

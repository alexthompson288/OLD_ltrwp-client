using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
	
	public bool isStoryBackButton = false;

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

	void On_SimpleTap(Gesture gesture) {
		if(gesture.pickObject==gameObject)
		{
			if(isStoryBackButton)
				GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>().ChangeLevel("StoryBrowser");
			//Application.LoadLevel("StoryWordBank");
		}
	}
}

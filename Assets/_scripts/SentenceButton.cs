using UnityEngine;
using System.Collections;

public class SentenceButton : MonoBehaviour {
	
	public Texture2D UpState;
	public Texture2D DownState;
	
	public void SetToDownState()
	{
		GetComponent<OTSprite>().image = DownState;
		GetComponent<OTSprite>().ForceUpdate();
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

	public void On_SimpleTap(Gesture gesture)
	{
		
	}
}

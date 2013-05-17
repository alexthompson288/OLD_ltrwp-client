using UnityEngine;
using System.Collections;

public class PhotoBoothManager : MonoBehaviour {
	
	bool captureFromCamera=false;
	public OTSprite Pip;
	public OTSprite Rob;
	public OTSprite Mole;
	
	// Use this for initialization
	void Start () {
		Pip.visible=false;
		Rob.visible=false;
		Mole.visible=false;
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

		if(gesture.pickObject==null)return;
		
		Debug.Log ("pickobject is "+gesture.pickObject.name);
		

		if(gesture.pickObject.name=="cameraWindow"){
//			if(LiveTextureBinding.isCaptureAvailable() && !captureFromCamera)			
//			{
//				captureFromCamera=true;
//				GameObject.Find ("cameraWindow").GetComponent<OTSprite>().image=LiveTextureBinding.startCameraCapture(true,LTCapturePreset.Size1280x720);
//			}
//			else if(LiveTextureBinding.isCaptureAvailable() && captureFromCamera)
//			{
//				captureFromCamera=false;
//				LiveTextureBinding.stopCameraCapture();
//			}
		}
		else if(gesture.pickObject.name=="pip")
		{
			if(Pip.visible)
				Pip.visible=false;
			else 
				Pip.visible=true;
			
		}
		else if(gesture.pickObject.name=="rob")
		{
			if(Rob.visible)
				Rob.visible=false;
			else 
				Rob.visible=true;
			
		}
		else if(gesture.pickObject.name=="mole")
		{
			if(Mole.visible)
				Mole.visible=false;
			else 
				Mole.visible=true;
			
		}

	}
}

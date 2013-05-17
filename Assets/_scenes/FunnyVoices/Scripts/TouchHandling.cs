using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TouchHandling : MonoBehaviour {
	
	Vector3 OriginalPosition;
	Vector3 OriginalScale;
	FunnyVoicesManager gameManager;
	public AudioClip MySound;
	
	// Use this for initialization
	void Start () {
		
		if( audio == null )
			gameObject.AddComponent<AudioSource>();
		
		OriginalPosition=gameObject.transform.position;
		OriginalScale=gameObject.transform.localScale;
//		gameManager=(Manager)Camera.mainCamera.GetComponent("Manager");
		gameManager=(FunnyVoicesManager)GameObject.Find("Main Camera").GetComponent(typeof(FunnyVoicesManager));
		if(gameManager.Buttons==null)
			gameManager.Buttons=new ArrayList();
		
		gameManager.Buttons.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
//		GameObject btnPlay=GameObject.Find ("btnPlay");
//		
//		if(AudioRecorderBinding.isRecording())
//			btnPlay.renderer.enabled=false;
//		else if(!AudioRecorderBinding.isRecording() && gameManager.HasSelection)
//			btnPlay.renderer.enabled=true;
//		else
//			btnPlay.renderer.enabled=false;
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
	
	// Simple tap
	private void On_SimpleTap(Gesture gesture){
		
		if(gesture.pickObject!=gameObject){
			if(gameObject.name!="Main Camera")
				return;
		}
		
		bool tweenObject=false;
			
		
		
		if(gesture.pickObject==null)
		{
			Debug.Log ("No go");
			gameManager.LastPlayed="";
			ResetTween ();
			return;
		}
		
		if(gesture.pickObject==gameObject){
		
			if(gesture.pickObject.name=="pig")
			{
				tweenObject=true;
				ResetTween("pig");
				Debug.Log ("piggie wiggie");
			}
			else if(gesture.pickObject.name=="dog")
			{
				tweenObject=true;
				ResetTween("dog");
				Debug.Log ("doggie woggie");
			}
			else if(gesture.pickObject.name=="cow")
			{
				tweenObject=true;
				ResetTween("cow");
				Debug.Log ("cowwie wowwie");
			}
			else if(gesture.pickObject.name=="exit" && gameManager.exitShowing)
			{
				Application.LoadLevel("ContentBrowser");
			}
			
			else if(gesture.pickObject.name=="next" && gameManager.HasSelection)
			{
				ResetTween();
				gameManager.HideSelectionPanel();
			}
			else if(gesture.pickObject.name=="replay" && gameManager.HasSelection)
			{
//				gameManager.playCuriousPip();
				GameObject.Find (gameManager.LastPlayed).audio.Play ();
				gameManager.prepareAndRecord(gameManager.LastPlayed+".wav");
			}
			else if(gesture.pickObject.name=="play" && gameManager.HasSelection)
			{
				gameManager.playbackRecording(gameManager.LastPlayed+".wav");
			}	
			
			else
			{
				
			}

		}
		if(tweenObject)
		{
			gameManager.LastPlayed=gesture.pickObject.name;
			gameManager.CurrentLabel.text=gesture.pickObject.name;
			gameManager.prepareAndRecord(gameManager.LastPlayed+".wav");
			gameManager.ShowSelectionPanel();
			gameManager.showExitButton();
			PlayMySound ();
//			gameManager.playCuriousPip();
			iTween.MoveTo(gesture.pickObject, new Vector3(0.0f,5.0f,-1.0f),0.5f);
  	    	iTween.ScaleTo(gesture.pickObject, new Vector3(1.5f,1.5f,1.5f),0.5f);	
		}
			
	}
	
	private void ResetTween()
	{
		gameManager.CurrentLabel.text="";
		gameManager.HideSelectionPanel();
		ResetTween(null);	
	}
	
	private void PlayMySound()
	{
		audio.clip=MySound;
		audio.Play();	
	}
	
	private void ResetTween(string ignored)
	{
		foreach(GameObject go in gameManager.Buttons)
		{
			if(go.name==ignored)continue;
			
			TouchHandling th=go.GetComponent<TouchHandling>();
				
			iTween.MoveTo(go, th.OriginalPosition, 0.5f);
			iTween.ScaleTo(go, th.OriginalScale, 0.5f);
		}
	}
}

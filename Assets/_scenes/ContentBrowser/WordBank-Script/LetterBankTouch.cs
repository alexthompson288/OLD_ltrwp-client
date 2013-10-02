using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class LetterBankTouch : MonoBehaviour {
	
	public string MyWord;
	public Transform TextPrefab;
	PersistentObject PersistentManager;
	private PictureFrame pictureFrame;
	bool isSelected = false;
	float SelectedTimer = -1.0f;
	OTSprite barSprite;
	LetterWordBank LWB;
	
	// Use this for initialization
	void Start () {
		//transform.parent=GameObject.Find ("bars").GetComponent<Transform>();
		pictureFrame = GameObject.Find("PictureFrame").GetComponent<PictureFrame>();	
		LWB = Camera.main.GetComponent<LetterWordBank>();
	}
	
	// Update is called once per frame
	void Update () {
		SelectedTimer -= Time.deltaTime;
		if(isSelected && SelectedTimer < 0.0f)
		{
			isSelected = false;

			iTween.ScaleTo(gameObject, iTween.Hash("x", 140.0f, "y", 140.0f, "time", 0.7f));

			barSprite.depth=0;
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
	
	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture) 
	{
		if(gesture.pickObject==gameObject)
		{			
			LWB.LetterSelected(MyWord);
			
			isSelected = true;
			
			OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
			barSprite = s;
			
			SelectedTimer = 4.0f;
			iTween.ScaleTo(gameObject, iTween.Hash("x", 200.0f, "y", 200.0f, "time", 0.7f));

			s.depth=-10;
			
			if(LWB.isGame == false)
			{
			pictureFrame.ShowMnemonic(MyWord, 5.0f);
			pictureFrame.PlayMnemonic(MyWord);
			}
		}
	}
	

}

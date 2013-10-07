using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class PictureWordGameTouch : MonoBehaviour {
	
	public string MyWord;
	public Transform TextPrefab;
	private PictureFrame pictureFrame;
	PersistentObject PersistentManager;
	bool isSelected = false;
	float SelectedTimer = -1.0f;
	OTSprite barSprite;
	PictureGame PG;
	MPPictureGame MPG;
	public Transform StarBurst;
	public int PlayerID = 0;
	
	// refers to the name of the colliders on the main camera
	public string AnswerColliderName = "null";
	
	// Use this for initialization
	void Start () {
		//transform.parent=GameObject.Find ("bars").GetComponent<Transform>();
		// if single player
		
		GameObject pfo = GameObject.Find("PictureFrame");
		if(pfo != null)
		{
			pictureFrame = pfo.GetComponent<PictureFrame>();	
			PG = Camera.main.GetComponent<PictureGame>();
		}
		
	}
	
	// only used for setting up multiplayer values
	public void Init()
	{
		if(PlayerID > 0)
		{
			pictureFrame = GameObject.Find("PictureFrame" + PlayerID.ToString()).GetComponent<PictureFrame>();	
			MPG = GameObject.Find("Player"+ PlayerID.ToString() + "Camera").GetComponent<MPPictureGame>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		SelectedTimer -= Time.deltaTime;
		if(isSelected && SelectedTimer < 0.0f)
		{
			isSelected = false;

			iTween.ScaleTo(gameObject, iTween.Hash("x", 375.0f, "y", 187.0f, "time", 0.7f));

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
		if(gesture.pickObject == null)
			return;
		
		if(gesture.pickObject==gameObject || gesture.pickObject.name == AnswerColliderName)
		{					
			isSelected = true;
			
			OTSprite s= gameObject.GetComponent<OTSprite>();
			barSprite = s;
			
			SelectedTimer = 4.0f;
			//iTween.ScaleTo(gameObject, iTween.Hash("x", 375.0f * 1.4f, "y", 187.0f * 1.4f, "time", 0.7f));
			
			s.depth=-10;
			
			if(PlayerID > 0)
			{
				if(MPG.RoundTimer > 1.28f){
					if(MyWord == MPG.CurrentWord )
					{
						// we won	
						MPG.AddPoint();
						MPG.StartRound();
						Instantiate( StarBurst, gesture.pickObject.transform.position, Quaternion.identity);
					}else{
						iTween.ShakePosition(gameObject, new Vector3(5.0f, 5.0f, 0.0f), 1.0f);
						s.tintColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
					}
				}
			}else{
				if(MyWord == PG.CurrentWord)
				{
					// we won	
					
						MPG.StartRound();
					
						PG.StartRound();
						Instantiate( StarBurst, gesture.pickObject.transform.position, Quaternion.identity);
					
				}else{
					// we lost
					iTween.ShakePosition(gameObject, new Vector3(5.0f, 5.0f, 0.0f), 1.0f);
					s.tintColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
				}
			}
		
		}
	}	

}

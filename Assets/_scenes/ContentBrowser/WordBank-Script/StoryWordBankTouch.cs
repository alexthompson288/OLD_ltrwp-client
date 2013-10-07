using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class StoryWordBankTouch : MonoBehaviour {
	
	public string MyWord;
	public Transform TextPrefab;
	PersistentObject PersistentManager;
	private PipPad pipPad;
	bool isSelected = false;
	float SelectedTimer = -1.0f;
	OTSprite barSprite;
	private PictureFrame pictureFrame;
	
	// Use this for initialization
	void Start () {
		pictureFrame = GameObject.Find("PictureFrame").GetComponent<PictureFrame>();
		transform.parent=GameObject.Find ("bars").GetComponent<Transform>();
//		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		pipPad = GameObject.Find("PipPad").GetComponent<PipPad>();
		
		if(TextPrefab!=null){
			Transform txt=(Transform)Instantiate(TextPrefab);
			OTTextSprite t=txt.GetComponent<OTTextSprite>();
			t.spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTSpriteAtlasCocos2DFnt>();
			t.size = new Vector2(1f, 1f);
			t.ForceUpdate();
			t.position=gameObject.GetComponent<OTSprite>().position;
			t.text=MyWord;			
			txt.parent=transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		SelectedTimer -= Time.deltaTime;
		if(isSelected && SelectedTimer < 0.0f)
		{
			isSelected = false;

			iTween.ScaleTo(gameObject, iTween.Hash("x", 256.0f, "y", 128.0f, "time", 0.7f));

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
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture) {
		if(gesture.pickObject==gameObject)
		{
			
			isSelected = true;
			
			OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
			barSprite = s;
			s.frameIndex = s.frameIndex + 2;
			
			SelectedTimer = 4.0f;
			iTween.ScaleTo(gameObject, iTween.Hash("x", 400.0f, "y", 172.0f, "time", 0.7f));

			s.depth=-10;
			List<PhonemeData> _PhonemeData = new List<PhonemeData>();
			
			if(MyWord.Length > 1)
			{
				try {
					_PhonemeData = GameManager.Instance.GetOrderedPhonemesForWord(MyWord);
	   			 }
	    		catch  {
	        		MyWord = "notFound";
	    		} 
				pipPad.MakeAppear(_PhonemeData, MyWord.ToLower());		
				
				pictureFrame.ShowPicture(MyWord.ToLower());
			}
			
			//sceneChangeTimer=true;
			//Application.LoadLevel ("Settings2");		

		}
	}
	

}

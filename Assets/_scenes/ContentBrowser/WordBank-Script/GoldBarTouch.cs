using UnityEngine;
using System.Collections;

public class GoldBarTouch : MonoBehaviour {
	
	public string MyWord;
	public Transform TextPrefab;
	PersistentObject PersistentManager;
	bool sceneChangeTimer=false;
	float sceneChangeCount=1.0f;
	
	// Use this for initialization
	void Start () {
		transform.parent=GameObject.Find ("bars").GetComponent<Transform>();
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		
		if(TextPrefab!=null){
			Transform txt=(Transform)Instantiate(TextPrefab);
			OTTextSprite t=txt.GetComponent<OTTextSprite>();
			t.spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTSpriteAtlasCocos2DFnt>();
			t.ForceUpdate();
			t.position=gameObject.GetComponent<OTSprite>().position;
			t.text=MyWord;
			
			txt.parent=transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(sceneChangeTimer)
			sceneChangeCount-=Time.deltaTime;
		
		if(sceneChangeCount<0)
		{
			sceneChangeTimer=false;
			sceneChangeCount=1.0f;
			PersistentManager.WordBankWord=MyWord;
			Application.LoadLevel("WordBank-Scaffold");
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
			bool TintPickObject=true;
			OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
			if(TintPickObject)
			{
				s.tintColor=new Color(255.0f,0.0f,0.0f,255.0f);
			}

			iTween.ScaleTo(gameObject, iTween.Hash("x", 400.0f, "y", 172.0f, "time", 0.7f));

			s.depth=-100;
			
			sceneChangeTimer=true;
			//Application.LoadLevel ("Settings2");

		}
	}
}

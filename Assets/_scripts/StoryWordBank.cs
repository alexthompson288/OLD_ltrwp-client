using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class StoryWordBank : MonoBehaviour {
	
	public Transform Bars;	
	GameManager cmsLink;
	private float velocity = 0.0f;
	public Transform[] ButtonPrefabs;
	private float BarsHeight = 0.0f;
	public OTTextSprite Title;
	PersistentObject PersistentManager;
	
	// Use this for initialization
	void Start () {
		cmsLink=GameManager.Instance;
		List<string> userWords = new List<string>();
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}		
		
		if(PersistentManager.StoryID == 0)
			PersistentManager.StoryID++;
		userWords=cmsLink.GetWordsFromStory(PersistentManager.StoryID);
		Title.text = cmsLink.GetStoryTitle(PersistentManager.StoryID);
		//foreach(string s in userWords)
		//	Debug.Log("Story word: " + s);
		
		int curX=0;
		int curY=0;
		float startX=-335.0f;
		float StartY=-45.0f;
		
		float incX=335.0f;
		float incY=130.0f;
		
		for(int i=0;i<userWords.Count;i++)
		{
			bool isTricky = cmsLink.isWordTrickyOrUndecodable(userWords[i]);
			
			int barIndex = 0;
			if(isTricky)
				barIndex = 1;
				
			Transform t=(Transform)Instantiate(ButtonPrefabs[barIndex]);
						
			StoryWordBankTouch sbt=t.GetComponent<StoryWordBankTouch>();
			sbt.MyWord=userWords[i];
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX),StartY-(curY*incY));
			
			curX++;
			
			if(curX>2)
			{
				curX=0;
				curY++;
			}
		}
		
		BarsHeight = (curY*incY);
	}
	
	// Update is called once per frame
	void Update () {
		velocity *= 1.0f - (Time.deltaTime * 4.0f);
		Bars.position=new Vector3(Bars.position.x,Bars.position.y+(velocity * Time.deltaTime),Bars.position.z);
		
		if(Bars.position.y - BarsHeight >  150.0f)
		{	
			velocity -= 25.0f;
		}else if(Bars.position.y <  -170.0f)
		{	
			velocity += 25.0f;
		}	
	}
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
//		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;
//		EasyTouch.On_Drag -= On_Drag;	
	}
	
	void On_TouchDown(Gesture gesture)
	{
		velocity += gesture.deltaPosition.y * 4.0f;
	}
}

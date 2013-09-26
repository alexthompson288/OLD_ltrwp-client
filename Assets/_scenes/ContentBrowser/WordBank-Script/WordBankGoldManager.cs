using UnityEngine;
using System.Collections;

public class WordBankGoldManager : MonoBehaviour {
	
	public Transform Bars;
	GameManager cmsLink;
	public Transform[] BarPrefabs;
	
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
	
	// Use this for initialization
	void Start () {
		cmsLink=GameManager.Instance;
		string[] userWords;
		
		userWords=cmsLink.GetUserWordIndex();
		
		OTSprite shimmer=GameObject.Find ("lightshimmer").GetComponent<OTSprite>();
//		iTween.ShakeScale(shimmer.gameObject, )
		
//		iTween.ShakeScale(shimmer.gameObject, iTween.Hash("x", 2.5f, "y", 2.5f, "loopType", "loop", "delay", 2.0f, "time", 0.3f));
		iTween.PunchScale(shimmer.gameObject, iTween.Hash("x", 30.5f, "y", 30.5f, "loopType", "loop", "delay", 2.0f, "time", 0.7f));
//		iTween.ShakeScale(shimmer.gameObject, new Vector3(shimmer.transform.localScale.x+20, shimmer.transform.localScale.y+20, shimmer.transform.localScale), 5.0f);
		
		int curX=0;
		int curY=0;
		float startX=-335.0f;
		float StartY=-45.0f;
		
		float incX=335.0f;
		float incY=130.0f;
		
		for(int i=0;i<userWords.Length;i++)
		{
			int barIndex=Random.Range (0,3);

			Transform t=(Transform)Instantiate(BarPrefabs[barIndex]);
						
			GoldBarTouch gbt=t.GetComponent<GoldBarTouch>();
			gbt.MyWord=userWords[i];
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX),StartY-(curY*incY));
			
			curX++;
			
			if(curX>2)
			{
				curX=0;
				curY++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {		
		float newYPos=0;
		
		if(Input.GetKey("up"))
		{
			newYPos=Bars.position.y+3.0f;
			Bars.position=new Vector3(Bars.position.x, newYPos, Bars.position.z);
		}
		else if(Input.GetKey ("down"))
		{
			newYPos=Bars.position.y-3.0f;
			Bars.position=new Vector3(Bars.position.x, newYPos, Bars.position.z);
		}		
	}

	private void On_Drag(Gesture gesture){
		
		float newYPos=0;
		
		if(Bars.position.y>0.5f)
			newYPos=0.5f;
		else if(Bars.position.y<-405.0f)
			newYPos=-405.0f;
		else
			newYPos=Bars.position.y+gesture.deltaPosition.y;
		
		Bars.position=new Vector3(Bars.position.x, newYPos, Bars.position.z);
	}

	
	void On_TouchDown(Gesture gesture)
	{
//		float newYPos=0;
//		
//		if(Bars.position.y<0.5f)
//			newYPos=0.5f;
//		else if(Bars.position.y>-405.0f)
//			newYPos=-405.0f;
//		else
//			newYPos=Bars.position.y+gesture.deltaPosition.y;
//		
//		Bars.position=new Vector3(Bars.position.x, newYPos, Bars.position.z);
		Bars.position=new Vector3(Bars.position.x,Bars.position.y+gesture.deltaPosition.y,Bars.position.z);
	}
}

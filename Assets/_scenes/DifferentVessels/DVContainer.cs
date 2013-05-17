using UnityEngine;
using System.Collections;

public class DVContainer : MonoBehaviour {
	
	DifferentVesselsManager gameManager;
	string sceneType;
	public string ExpectedLetter;
	bool isActive=false;
	public Rect BoundingBox;
	
	// Use this for initialization
	void Start () {
		gameManager=GameObject.Find ("Main Camera").GetComponent<DifferentVesselsManager>();
		sceneType="castle";
		if(isActive)CloseVessel();
		
		float width=0;
		float height=0;
		
		if(gameObject.name=="1")
			BoundingBox=new Rect(-440, -354, 253, 323);
		else if(gameObject.name=="2")
			BoundingBox=new Rect(-58, -338, 200, 238);
		else if(gameObject.name=="3")
			BoundingBox=new Rect(200, -340, 210, 288);
		
		Debug.Log ("my bbox "+BoundingBox);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool TakesThisLetter(string thisLetter)
	{
		if(isActive&&thisLetter==ExpectedLetter)
			return true;
		else 
			return false;
	}
	
	public void OpenVessel()
	{
		OpenContainer();
	}
	
	public void CloseVessel()
	{
		CloseContainer();
	}
	
	public void OpenContainer()
	{	
		if(sceneType=="castle")
		{
			float reqRot=0;
			if(gameObject.name=="1")
				reqRot=35;
			
			else if(gameObject.name=="2")
				reqRot=2;
			
			else if(gameObject.name=="3")
				reqRot=20;
			
			OTSprite goT = GameObject.Find ("Bottom-"+gameObject.name).GetComponent<OTSprite>();
			
			goT.rotation=reqRot;
			
			isActive=true;
		}
	}
	
	public void CloseContainer()
	{	
		if(sceneType=="castle")
		{
			float reqRot=0;
			if(gameObject.name=="1")		
				reqRot=348;
			
			else if(gameObject.name=="2")
				reqRot=336;
			
			else if(gameObject.name=="3")
				reqRot=348;
			
			
			OTSprite goT = GameObject.Find ("Bottom-"+gameObject.name).GetComponent<OTSprite>();
			
			goT.rotation=reqRot;
		}
		isActive=false;
	}
	
//	Rect UnionTheseRects(Rect thisRect, Rect thatRect)
//	{
//		Rect leftMost;
//		Rect rightMost;
//		Rect topMost;
//		Rect bottomMost;
//	
//		if(thisRect.x<thatRect.x)
//		{
//			leftMost=thisRect;
//			rightMost=thatRect;
//		}
//		else
//		{
//			leftMost=thatRect;
//			rightMost=thisRect;
//		}
//		
//		if(thisRect.y<thatRect.y)
//		{
//			bottomMost=thisRect;
//			topMost=thatRect;
//		}
//		else
//		{
//			bottomMost=thatRect;
//			topMost=thisRect;
//		}
//	
//	
//		topMost=new Rect(topMost.x, topMost.y+topMost.height, 0,0);
//		rightMost=new Rect(rightMost.x+rightMost.width, rightMost.y, 0,0);
//	
//		Rect ReturnRect=new Rect(leftMost.x,bottomMost.y,topMost.x-leftMost.x,topMost.y-bottomMost.y);
//	
//		
//		
//		return ReturnRect;
//
//	}


	public Rect UnionRect(Rect r1, Rect r2)
	{
		
		Debug.Log ("Rect1 "+r1);
		Debug.Log ("Rect2 "+r2);
	    Vector2 p1=new Vector2();
	    Vector2 p2=new Vector2();
	
	    if(r1.x<r2.x) p1.x=r1.x;
	    else p1.x=r2.x;
	
	    if(r1.y<r2.y) p1.y=r1.y;
	    else p1.y=r2.y;
	
	    if(r1.x>r2.x)p2.x=r1.x;
	    else p2.x=r2.x;
	
	    if(r1.y>r2.y)p2.y=r1.y;
	    else p2.y=r2.y;
	
		Rect ReturnRect=new Rect(r1.x, r1.y, r2.x-r1.x, r2.y-r1.y);
		
		Debug.Log ("return rect is "+ReturnRect);
		
	    return ReturnRect;
	}
}

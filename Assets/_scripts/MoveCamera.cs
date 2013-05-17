using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	
	public string parallaxDirection="x";
	public float maxAmount=330.0f;
	public float minAmount=-330.0f;
	public float incr=1.2f;
	public float ydelta=0.0f;
	bool right;
	bool up;
	
	
	// Use this for initialization
	void Start () {
		if(parallaxDirection=="x")
			right=true;
		else 
			up=true;
//		iTween.MoveBy(gameObject, iTween.Hash("y", 200, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", 0.1, "time", 6.0, "lookat", lookat));
		//iTween.MoveBy(gameObject, iTween.Hash(parallaxDirection, amount, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", 0.3, "time", 3.0));
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 OTPos=new Vector2(OT.view.position.x, OT.view.position.y);
		
		if(parallaxDirection=="x"){
			if(right)
			{
				OTPos.x+=incr;
				OT.view.position=OTPos;
				
				if(OT.view.position.x>maxAmount)
					right=false;
			}
			else if(!right)
			{
				OTPos.x-=incr;
				OT.view.position=OTPos;
				
				if(OT.view.position.x<minAmount)
					right=true;
			}
		}
		else if(parallaxDirection=="y"){
			if(up)
			{
				OTPos.y+=incr;
				OT.view.position=OTPos;
				
				if(OT.view.position.y>maxAmount)
					up=false;
			}
			else if(!up)
			{
				OTPos.y-=incr;
				OT.view.position=OTPos;
				
				if(OT.view.position.y<minAmount)
					up=true;
			}
		}
		if(parallaxDirection=="xydelta"){
			
			if(right)
			{
				OTPos.x+=incr;
				OTPos.y+=ydelta;
				OT.view.position=OTPos;
				
				if(OT.view.position.x>maxAmount)
					right=false;
			}
			else if(!right)
			{
				OTPos.x-=incr;
				OTPos.y+=ydelta;
				OT.view.position=OTPos;
				
				if(OT.view.position.x<minAmount)
					right=true;
				
			}
			
			ydelta=0;
		}
		
	}
}

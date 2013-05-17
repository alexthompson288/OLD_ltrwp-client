using UnityEngine;
using System.Collections;

public class GenericContainer : MonoBehaviour {
	
	GenericMountable MyMountObject;
	GenericContainer Myself;
	public Vector2 MyPosition;
	
	// Use this for initialization
	void Start () {
		MyPosition=gameObject.GetComponent<OTSprite>().position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public bool CanBeMounted(GenericMountable thisObject, Vector2 touchPos)
	{
		OTSprite s=gameObject.GetComponent<OTSprite>();
		float myPosX=s.worldPosition.x-(s.size.x/2);
		float myPosY=s.worldPosition.y-(s.size.y/2);
			
		Rect MyRect=new Rect(myPosX, myPosY, s.size.x, s.size.y);
		
		Debug.Log ("Check for mount possibility from touch "+touchPos+" in rect "+MyRect);
		
		if(MyMountObject==null&&MyRect.Contains(touchPos))return true;
		else return false;
	}
	
	public void SetMountedObject(GenericMountable thisObject)
	{
		MyMountObject=thisObject;
	}
	
	public void UnsetMountedObject(GenericMountable thisObject)
	{
		if(thisObject==MyMountObject){
			MyMountObject.MyContainer=null;
			MyMountObject=null;	
		}
	}
}
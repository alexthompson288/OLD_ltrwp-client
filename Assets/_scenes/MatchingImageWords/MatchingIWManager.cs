using UnityEngine;
using System.Collections;

public class MatchingIWManager : MonoBehaviour {
	public Transform pickupObject;
	public ArrayList SceneSprites;
	int xTouchOffset=512;
	int yTouchOffset=384;
	
	// Use this for initialization
	void Start () {
		SceneSprites=new ArrayList();
		SceneSprites.Add(GameObject.Find("image").transform);
		SceneSprites.Add(GameObject.Find("word").transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		EasyTouch.On_TouchUp+=On_TouchUp;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchUp-=On_TouchUp;
	}
	
	void On_TouchUp(Gesture gesture){
		
		Vector2 touchPos=new Vector2(gesture.position.x-xTouchOffset,gesture.position.y-yTouchOffset);
		
		if(pickupObject!=null){
			foreach(Transform t in SceneSprites)
			{	
				if(t==pickupObject)continue;
				
				OTSprite s=t.GetComponent<OTSprite>();
				Rect sRect=new Rect(s.position.x-(s.size.x/2), s.position.y-(s.size.y/2), s.size.x, s.size.y);
				
				Debug.Log ("pickupObject: "+pickupObject.gameObject.name+" // current Transform: "+t.gameObject.name+" // current Transform ");
				
				if(sRect.Contains(touchPos))
				{
					Debug.Log ("("+Time.deltaTime+")"+"valid hit brah");
				}
			}
		}
		
		pickupObject=null;
	}
}

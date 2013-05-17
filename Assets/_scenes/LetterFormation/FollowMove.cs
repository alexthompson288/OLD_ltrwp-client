using UnityEngine;
using System.Collections;

public class FollowMove : MonoBehaviour {
	
	public OTSpline LetterSpline;
	public Transform PushObject;
	
	float trackingPos=0.0f;
	const float PUSH_GAP=10.0f;
	const float PUSH_SPEED=0.01f;
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 dist=transform.position-PushObject.position;
		float l=Mathf.Abs(dist.magnitude);
		
		//Debug.Log ("l: "+l);
		
		if(l<PUSH_GAP)
		{
			trackingPos+=PUSH_SPEED;
			transform.position=LetterSpline.GetPosition(trackingPos);
//			Debug.Log ("thingy "+trackingPos+" letterspline "+LetterSpline.gameObject.name);
		}
	}
}

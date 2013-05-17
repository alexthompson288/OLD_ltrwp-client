using UnityEngine;
using System.Collections;

public class MatchingBondsObject : MonoBehaviour {
	
	public string MyLetter;
	public int playerID;
	float timeSinceMoved=0;
	
	// Use this for initialization
	void Start () {
		foreach(Transform t in transform)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				t.GetComponent<OTTextSprite>().text=MyLetter;
				t.GetComponent<OTTextSprite>().spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTContainer>();
				t.GetComponent<OTTextSprite>().ForceUpdate();
				break;
			}
		}
		
		if(playerID==2)
			transform.GetComponent<OTSprite>().tintColor=new Color(0,1,0,1);
		
		MoveMe(true);
	}
	
	void MoveMe (bool hasJustStarted) {
		if(hasJustStarted)
			rigidbody.AddForce(0,Random.Range(1000,1500),0);
		else
			rigidbody.AddForce(Random.Range(-300,300),Random.Range(-300,300),0);
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceMoved+=Time.deltaTime;
		if(timeSinceMoved>1 && (rigidbody.velocity.x<400.0f||rigidbody.velocity.y<400.0f))
		{
			MoveMe(false);
			timeSinceMoved=0;
		}
	}

}

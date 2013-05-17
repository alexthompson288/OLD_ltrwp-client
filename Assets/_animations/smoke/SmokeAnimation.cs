using UnityEngine;
using System.Collections;

public class SmokeAnimation : MonoBehaviour {

	public bool playSmoke1;
	public bool playSmoke2;

	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			
			if(playSmoke1)
				SetAnimationSmoke();
			else if(playSmoke2)
				SetAnimationSmoke2();
		}
	}
	
	void SetNonePlaying()
	{
		playSmoke1=false;
		playSmoke2=false;
	}
	
	void SetAnimationSmoke()
	{
		myself.PlayOnce("puff1");
		playSmoke1=false;
		playSmoke2=true;
	}
	void SetAnimationSmoke2()
	{
		myself.PlayOnce("puff2");
		playSmoke2=false;
	}
}

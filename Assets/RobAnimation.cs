using UnityEngine;
using System.Collections;

public class RobAnimation : MonoBehaviour {

	public bool playBlink;
	public bool playBlink2;
	public bool playDoubt;
	public bool playPositive;
	public bool playPositive2;
	public bool playPositive2p2;
	public bool playPositive2p3;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			
			if(playBlink)
				SetAnimationBlink();
			else if(playBlink2)
				SetAnimationBlink2();
			else if(playDoubt)
				SetAnimationDoubt();
			else if(playPositive)
				SetAnimationPositive();
			else if(playPositive2||playPositive2p2||playPositive2p3)
				SetAnimationPositive2();
			else
				SetAnimationBlink();
		}
	}
	
	void SetNonePlaying()
	{
		playBlink=false;
		playBlink2=false;
		playDoubt=false;
		playPositive=false;
		playPositive2=false;
		playPositive2p2=false;
	}
	
	void SetAnimationBlink()
	{
		myself.PlayOnce("blink");
		playBlink=false;
	}
	
	void SetAnimationBlink2()
	{
		myself.PlayOnce("blink2");
		playBlink2=false;
	}
	
	void SetAnimationDoubt()
	{
		myself.PlayOnce("doubt");
		playDoubt=false;
	}
	
	void SetAnimationPositive()
	{
		myself.PlayOnce("positive");
		playPositive=false;
	}
	
	void SetAnimationPositive2()
	{
		if(playPositive2){
			myself.PlayOnce("positive2-1");
			playPositive2=false;
			playPositive2p2=true;
			
		}
		else if(playPositive2p2)
		{
			myself.PlayOnce("positive2-2");
			playPositive2p2=false;
			playPositive2p3=true;
		}
		else if(playPositive2p3)
		{
			myself.PlayOnce("positive2-3");
			playPositive2p3=false;
		}
	}

}

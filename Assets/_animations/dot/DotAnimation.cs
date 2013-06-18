using UnityEngine;
using System.Collections;

public class DotAnimation : MonoBehaviour {

	public bool playIdleSet;
	public bool playIdle;
	public bool playBlink;
	public bool playDoubt;
	public bool playPositive;
	public bool playPositive2;
	public bool playTalking;
	public bool playTalking2;
	
	// blink
	// doubt
	// positive1
	// positive2
	// talking1
	// talking2

	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playIdleSet&&(!playBlink||!playDoubt||!playPositive||!playPositive2||!playTalking||!playTalking2))
			{
				SetAnimationIdleSet();
				return;
			}
			else if(playBlink){
				SetAnimationBlink();
			}
			else if(playDoubt){
				SetAnimationDoubt();
			}
			else if(playIdle)
			{
				SetAnimationIdle();	
			}
			else if(playPositive){
				SetAnimationPositive();
			}
			else if(playPositive2){
				SetAnimationPositive2();
			}
			else if(playTalking){
				SetAnimationTalking();
			}
			else if(playTalking2){
				SetAnimationTalking2();
			}
			else{
				SetAnimationIdleSet();
			}
		}
	}
	
	public void SetNonePlaying()
	{
		playIdleSet=false;
		playIdle=false;
		playBlink=false;
		playDoubt=false;
		playPositive=false;
		playPositive2=false;
		playTalking=false;
		playTalking2=false;
	}
	
	void SetAnimationIdleSet()
	{
		if(!playIdleSet){
			playIdleSet=true;
			playBlink=true;
		}
		if(playIdleSet&&playBlink){
			SetAnimationBlink();
			playIdle=true;
		}
		else if(playIdleSet&&playIdle){
			SetAnimationIdle();
			playIdle=true;
		}
	}
	
	void SetAnimationIdle(){
		myself.PlayOnce("idle");
		SetNonePlaying();
	}
	
	void SetAnimationBlink()
	{
		myself.PlayOnce("blink");
		SetNonePlaying();
	}
	
	void SetAnimationDoubt()
	{
		myself.PlayOnce("doubt");
		SetNonePlaying();
	}
	
	void SetAnimationPositive()
	{
		myself.PlayOnce("positive");
		SetNonePlaying();
	}	
	
	void SetAnimationPositive2()
	{
		myself.PlayOnce("positive2");
		SetNonePlaying();
	}

	void SetAnimationTalking()
	{
		myself.PlayOnce("talking");
		SetNonePlaying();
	}
	
	void SetAnimationTalking2()
	{
		myself.PlayOnce("talking2");
		SetNonePlaying();
			
	}
}

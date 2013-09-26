using UnityEngine;
using System.Collections;

public class PipAnimation : MonoBehaviour {
	
	public bool playIdleSet;
	public bool playIdle;
	public bool playBlink;
	public bool playBlink2;
	public bool playDoubt;
	public bool playPositive;
	public bool playPositive2;
	public bool playPositive2p2;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playIdleSet&&(!playBlink||!playBlink2||!playDoubt||!playIdle||!playPositive||!playPositive2||!playPositive2p2))
			{
				SetAnimationIdleSet();
				return;
			}
			else if(playBlink){
				SetAnimationBlink();
			}
			else if(playBlink2){
				SetAnimationBlink2();
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
			else if(playPositive2||playPositive2p2){
				SetAnimationPositive2();
			}
			else{
				SetAnimationIdleSet();
			}
		}
		if(playPositive2||playPositive2p2){
				SetAnimationPositive2();
			}
	}
	
	public void SetNonePlaying()
	{
		playIdleSet=false;
		playIdle=false;
		playBlink=false;
		playBlink2=false;
		playDoubt=false;
		playPositive=false;
		playPositive2=false;
		playPositive2p2=false;
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
			playBlink2=true;
		}
		else if(playIdleSet&&playBlink2)
		{
			SetAnimationBlink2();
			playIdle=true;
			playIdleSet=false;
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
	
	void SetAnimationBlink2()
	{
		myself.PlayOnce("blink2");
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
		if(playPositive2){
			myself.PlayOnce("positive2-1");
			playPositive2=false;
			playPositive2p2=true;
			
		}
		else if(playPositive2p2)
		{
			myself.PlayOnce("positive2-2");
			playPositive2p2=false;
			SetNonePlaying();
		}
	}
}

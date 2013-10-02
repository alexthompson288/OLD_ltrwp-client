using UnityEngine;
using System.Collections;

public class PipAnimation : MonoBehaviour {
	
	public bool playMainMenuSet = false;
	public bool playIdleSet;
	public bool playIdle;
	public bool playBlink;
	public bool playBlink2;
	public bool playDoubt;
	public bool playPositive;
	public bool playPositive2;
	public bool playPositive2p2;
	private int timesSinceThumbs = 0;
	
	private float TimeSinceLastPlay = 10.0f;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Awake () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
		myself.PlayOnce("PipBlink");
	}
	
	// Update is called once per frame
	void Update () {
		if(playMainMenuSet)
		{
			TimeSinceLastPlay += Time.deltaTime;
			if(!myself.isPlaying && TimeSinceLastPlay > 4.0f)
			{
				TimeSinceLastPlay = 0.0f;
				int index = Random.Range(0,3);
				if(timesSinceThumbs > 2 &&  index == 0)
				{
					SetAnimationPositive();
				}else{ 
					SetAnimationBlink();
					timesSinceThumbs++;
				}
				return;
			}
		}else
		{
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
	
	public void StopAnimations()
	{
		myself.Stop();	
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

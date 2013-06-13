using UnityEngine;
using System.Collections;

public class TrollAnimation : MonoBehaviour {

	int bellySize=1;
	public bool playIdleSet;
	public bool playIdle;
	public bool playBlink;
	public bool playPositive;
	public bool playPositiveP2;
	public bool playNegative;
	public bool playNegativeP2;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playIdleSet&&(!playBlink||!playIdle||!playPositive||!playPositiveP2||!playNegative||!playNegativeP2))
			{
				SetAnimationIdleSet();
				return;
			}

			else if(playIdle)
			{
				SetAnimationIdle();	
			}
			else if(playBlink)
			{
				SetAnimationBlink();
			}
			else if(playPositive||playPositiveP2){
				SetAnimationPositive();
			}
			else if(playNegative||playNegativeP2){
				SetAnimationNegative();
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
		playPositive=false;
		playPositiveP2=false;
		playNegative=false;
		playNegativeP2=false;
	}
	
	public void MakeBellyBigger()
	{
		bellySize++;
		if(bellySize==2)
			myself.PlayOnce("bellygrow12");
		else if(bellySize==3)
			myself.PlayOnce("bellygrow23");
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
		string AnimationName="idle"+bellySize;
		myself.PlayOnce(AnimationName);
		SetNonePlaying();
	}
	
	void SetAnimationBlink()
	{
		string AnimationName="belly"+bellySize+"-blink";
		myself.PlayOnce(AnimationName);
		SetNonePlaying();
	}
	
	void SetAnimationPositive()
	{
		if(playPositive){
			string AnimationName="belly"+bellySize+"-positive1";
			myself.PlayOnce(AnimationName);
			playPositive=false;

			if(bellySize<3)
				playPositiveP2=true;
			else 
				SetNonePlaying();
		}
		else if(playPositiveP2)
		{
			string AnimationName="belly"+bellySize+"-positive2";
			myself.PlayOnce(AnimationName);
			playPositiveP2=false;
			SetNonePlaying();
		}
	}

	void SetAnimationNegative()
	{
		if(playNegative){
			string AnimationName="belly"+bellySize+"-negative1";
			myself.PlayOnce(AnimationName);
			playNegative=false;

			if(bellySize<3)
				playNegativeP2=true;
			else 
				SetNonePlaying();
		}
		else if(playNegativeP2)
		{
			string AnimationName="belly"+bellySize+"-negative2";
			myself.PlayOnce(AnimationName);
			playNegativeP2=false;
			SetNonePlaying();
		}
	}
}

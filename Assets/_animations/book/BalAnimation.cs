using UnityEngine;
using System.Collections;

public class BalAnimation : MonoBehaviour {
	
	public bool playIdle;
	public bool playBlink;
	public bool playIdleSet;
	public bool playStill;
	public bool playOpening;
	public bool playOpening2;
	public bool playClosing;
	public bool playClosing2;
	public bool loadOnFinish;
	public bool playTalking;
	public bool playTalking2;
	public bool playTalking3;
	public bool playTalking4;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Awake () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
		myself.spriteContainer=GameObject.Find("BookIdle").GetComponent<OTSpriteAtlasCocos2D>();
//		myself.PlayOnce("goingback2");
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playIdleSet&&(!playIdle||!playBlink||!playStill||!playOpening||!playOpening2||!playClosing||!playClosing2||!playTalking||!playTalking2||!playTalking3||!playTalking4))
			{
				SetAnimationIdleSet();
				return;
			}
			if(playIdle||playBlink)
				SetAnimationIdle();
			else if(playStill)
				SetAnimationStill();
			else if(playOpening)
				SetAnimationOpening();
			else if(playOpening2)
				SetAnimationOpening2();
			else if(playClosing)
				SetAnimationClosing();
			else if(playClosing2)
				SetAnimationClosing2();
			else if(playTalking)
				StartTalking(1);
			else if(playTalking2)
				StartTalking(2);
			else if(playTalking3)
				StartTalking(3);
			else if(playTalking4)
				StartTalking(4);
			else if(loadOnFinish)
				Application.LoadLevel(GameObject.Find("PersistentManager").GetComponent<PersistentObject>().NextLevel);
			else
				SetAnimationIdleSet();
		}
	}
	
	public void StopAll(){
		playIdle=false;
		playIdleSet=false;
		playStill=false;
		playBlink=false;
		playOpening=false;
		playOpening2=false;
		playClosing=false;
		playClosing2=false;
		playTalking=false;
		playTalking2=false;
		playTalking3=false;
		playTalking4=false;
	}

	void StartTalking(int cycle)
	{
		if(cycle==1)
			myself.Play("talkingc1");
		else if(cycle==2)
			myself.Play("talkingc2");
		else if(cycle==3)
			myself.Play("talkingc3");
		else if(cycle==4)
			myself.Play("talkingc4");
		
		playTalking=false;
		playTalking2=false;
		playTalking3=false;
		playTalking4=false;
	}
	
	void SetAnimationIdleSet()
	{
		if(!playIdleSet){
			playIdleSet=true;
			playIdle=true;
		}
		if(playIdleSet&&playIdle){
			SetAnimationIdle();
			playIdleSet=true;
			playStill=true;
		}
		else if(playIdleSet&&!playIdle&&playStill){	
			SetAnimationStill();
			
			int rndChance=Random.Range(0,2);

			if(rndChance==0){
				playIdleSet=false;
			}
			else
			{
				playIdleSet=true;
				playStill=true;
			}
		}
		
	}
	
	void SetAnimationStill(){
		myself.PlayOnce("still");
		playStill=false;
	}
	
	void SetAnimationIdle(){
		myself.PlayOnce("idle");
		playIdle=false;
	}

	void SetAnimationBlink(){
		myself.PlayOnce("idle");
		StopAll();
	}
	
	void SetAnimationOpening()
	{
		myself.PlayOnce("comingfront1");
		playOpening=false;
		playOpening2=true;
	}
	
	void SetAnimationOpening2()
	{
		myself.PlayOnce("comingfront2");
		playOpening2=false;
	}
	
	void SetAnimationClosing()
	{
		myself.PlayOnce("goingback1");
		playClosing=false;
		playClosing2=true;
	}
	
	void SetAnimationClosing2()
	{
		myself.PlayOnce("goingback2");
		playClosing2=false;
	}
}

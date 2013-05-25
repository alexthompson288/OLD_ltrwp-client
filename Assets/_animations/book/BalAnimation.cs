using UnityEngine;
using System.Collections;

public class BalAnimation : MonoBehaviour {
	
	public bool playIdle;
	public bool playIdleSet;
	public bool playStill;
	public bool playOpening;
	public bool playOpening2;
	public bool playClosing;
	public bool playClosing2;
	public bool loadOnFinish;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Awake () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
//		myself.PlayOnce("goingback2");
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playIdleSet)
			{
				SetAnimationIdleSet();
				return;
			}
			if(playIdle)
				SetAnimationIdle();
			else if(playOpening)
				SetAnimationOpening();
			else if(playOpening2)
				SetAnimationOpening2();
			else if(playClosing)
				SetAnimationClosing();
			else if(playClosing2)
				SetAnimationClosing2();
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
		playOpening=false;
		playOpening2=false;
		playClosing=false;
		playClosing2=false;
	}

	void StartTalking(int cycle)
	{
		if(cycle==1)
			myself.Play("talk-corner-1");
		else if(cycle==2)
			myself.Play("talk-corner-2");
		else if(cycle==3)
			myself.Play("talk-corner-3");
		else if(cycle==4)
			myself.Play("talk-corner-4");
		
	}
	
	void SetAnimationIdleSet()
	{
		if(!playIdleSet){
			playIdleSet=true;
			playIdle=true;
		}
		if(playIdleSet&&playIdle){
			SetAnimationIdle();
			playStill=true;
		}
		else if(playIdleSet&&playStill){
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

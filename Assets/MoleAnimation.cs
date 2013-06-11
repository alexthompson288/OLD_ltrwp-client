using UnityEngine;
using System.Collections;

public class MoleAnimation : MonoBehaviour {

	public bool playJump;
	public bool playJump2;
	public bool playSplat;
	public int Player=1;
	bool bigSign=false;
	
	OTAnimatingSprite myself;
	
	// Use this for initialization
	void Start () {
		myself=gameObject.GetComponent<OTAnimatingSprite>();
		bigSign=gameObject.GetComponent<MoleTouch>().bigSign;
	}
	
	// Update is called once per frame
	void Update () {
		if(!myself.isPlaying)
		{
			if(playJump||playJump2)
				SetAnimationJump();
			else if(playSplat)
				SetAnimationSplat();
			else
				SetAnimationJump();
		}
		
		if(Player==2)
			myself.tintColor=new Color(0,1,0,1);
		
	}
	
	void SetNonePlaying()
	{
		playJump=false;
		playJump2=false;
		playSplat=false;
	}
	
	void SetAnimationSplat()
	{
		SetAnimationSplat(false);	
	}
	
	public void SetAnimationSplat(bool repeat)
	{
		myself.Stop();
		if(repeat && bigSign)
			myself.PlayLoop("bigsplat");
		else if(repeat && !bigSign)
			myself.PlayLoop("splat");
		else if(!repeat && bigSign)
			myself.PlayOnce("bigsplat");
		else if(!repeat && !bigSign)
			myself.PlayOnce("splat");
	}
	
	void SetAnimationJump()
	{
		if(!playJump&&!playJump2)playJump=true;
		
		if(playJump && !bigSign){
			myself.PlayOnce("jump1");
			playJump=false;
			playJump2=true;
			
		}
		else if(playJump && bigSign)
		{
			myself.PlayOnce("bigjump");
			playJump=false;
		}
		else if(playJump2)
		{
			myself.PlayOnce("jump2");
			playJump2=false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class animateMesh : MonoBehaviour {
	bool isPlaying=false;
	public float framesPerSecond=12.0f;
	public Material[] MaterialList;
	public Texture2D[] TextureList;
	public float frameTime=0.0f;
	public float elapsedTime=0.0f;
	public float timeUntilStart=0.0f;
	public bool loopAnimation=false;
	bool countdownStart=false;
	public bool destroyOnComplete=false;
	public animateMesh NextPart;
	
	OTSprite Mysprite;
	
	// Use this for initialization
	void Start () {
		Mysprite=gameObject.GetComponent<OTSprite>();
		Debug.Log ("Started "+gameObject.name);
		Mysprite.visible=false;
		frameTime=1.0f/framesPerSecond;
		Mysprite.image = (Texture2D)TextureList[0];
	}
	
	// Update is called once per frame
	void Update () {
		float index = 0;
		
		if(countdownStart)
		{
			timeUntilStart-=Time.deltaTime;
			
			if(timeUntilStart<0)
			{
				countdownStart=false;
				Play ();	
			}
		}
		
		if(isPlaying){
			elapsedTime+=Time.deltaTime;
			index = elapsedTime/frameTime;
		    index = (int)index;
		}
		
		if(isPlaying && index<TextureList.Length){
		    Mysprite.image = (Texture2D)TextureList[(int)index];
		}
		else if(isPlaying&&index>=TextureList.Length){
			Stop ();
		}
	}
	
	public void Play () {
		Mysprite.visible=true;
		isPlaying=true;
	}
	
	public void PlayAndDestroy () {
		destroyOnComplete=true;
		loopAnimation=false;
		Play ();
	}
	
	public void PlayWithDelay(float countdown)
	{
		timeUntilStart=countdown;
		countdownStart=true;
	}
	
	public void Stop() {
		isPlaying=false;
		Mysprite.visible=false;
		elapsedTime=0.0f;
		Mysprite.image = (Texture2D)TextureList[0];
		
		if(loopAnimation)
			Play ();
		
		if(NextPart!=null)
		{
			animateMesh MyNextPart=(animateMesh)Instantiate(NextPart);
			MyNextPart.Play ();
		}
		
		if(destroyOnComplete)
			GameObject.Destroy(gameObject);
	}
	
	public void Pause(){
		isPlaying=false;	
	}
	
	public float AnimationTime() {
		return frameTime*MaterialList.Length;	
	}
	
	public bool AnimationPlaying() {
		return isPlaying;	
	}
}

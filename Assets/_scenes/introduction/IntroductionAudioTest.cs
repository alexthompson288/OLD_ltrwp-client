using UnityEngine;
using System.Collections;

public class IntroductionAudioTest : MonoBehaviour {
	
	bool playAudio1=false;
	bool playedAudio1=false;
	float audio1time=14.5f;
	
	bool playAudio2=false;
	bool playedAudio2=false;
	float audio2time=6.5f;
	
	bool playAudio3=false;
	bool playedAudio3=false;
	float audio3time=5.5f;
	
	bool playAudio4=false;
	bool playedAudio4=false;
	float audio4time=26.5f;
	
	bool playAudio5=false;
	bool playedAudio5=false;
	float audio5time=12.5f;
	
	bool playAudio6=false;
	bool playedAudio6=false;
	float audio6time=17.5f;
	
	int currentAudioIndex=0;
	public AudioClip[] SceneAudio;
	AudioClip CurrentAudio;
	
	PipAnimation pip;
	RobAnimation rob;
	
	// Use this for initialization
	void Start () {
		playAudio1=true;
		audio.clip=SceneAudio[currentAudioIndex];
		
		pip=GameObject.Find ("Pip").GetComponent<PipAnimation>();
		rob=GameObject.Find ("Rob").GetComponent<RobAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playAudio1)
		{
			if(!rob.playBlink)rob.playBlink=true;
			
			audio1time-=Time.deltaTime;
			
			if(!playedAudio1)
			{
				audio.Play();
				currentAudioIndex++;
				playedAudio1=true;
			}
			
			if(audio1time<0.0f)
			{
				playAudio1=false;
				playAudio2=true;
			}
		}
		else if(playAudio2)
		{
			audio2time-=Time.deltaTime;
			
			if(!rob.playPositive)rob.playPositive=true;
			
			if(!playedAudio2)
			{
				audio.clip=SceneAudio[currentAudioIndex];
				currentAudioIndex++;
				audio.Play();
				playedAudio2=true;
			}
			
			if(audio2time<0.0f)
			{
				playAudio2=false;
				playAudio3=true;
			}
		}
		else if(playAudio3)
		{
			audio3time-=Time.deltaTime;
			
			if(!pip.playPositive)pip.playPositive=true;
			
			if(!playedAudio3)
			{
				audio.clip=SceneAudio[currentAudioIndex];
				currentAudioIndex++;
				audio.Play();
				playedAudio3=true;
			}
			
			if(audio3time<0.0f)
			{
				playAudio3=false;
				playAudio4=true;
			}
		}
		else if(playAudio4)
		{
			audio4time-=Time.deltaTime;
			
			
			if(!playedAudio4)
			{
				audio.clip=SceneAudio[currentAudioIndex];
				currentAudioIndex++;
				audio.Play();
				playedAudio4=true;
			}
			
			if(audio4time<0.0f)
			{
				playAudio4=false;
				playAudio5=true;
			}
		}
		else if(playAudio5)
		{
			audio5time-=Time.deltaTime;
			
			if(!rob.playBlink2)rob.playBlink2=true;
			
			if(!playedAudio5)
			{
				audio.clip=SceneAudio[currentAudioIndex];
				currentAudioIndex++;
				audio.Play();
				playedAudio5=true;
			}
			
			if(audio5time<0.0f)
			{
				playAudio5=false;
				playAudio6=true;
			}
		}
		else if(playAudio6)
		{
			audio6time-=Time.deltaTime;
			
			if(!rob.playPositive2&&!rob.playPositive2p2&&!rob.playPositive2p3)rob.playPositive2=true;
			
			if(!playedAudio6)
			{
				audio.clip=SceneAudio[currentAudioIndex];
				currentAudioIndex++;
				audio.Play();
				playedAudio6=true;
			}
			
			if(audio6time<0.0f)
			{
				playAudio6=false;
			}
		}
	}
}

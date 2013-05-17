using UnityEngine;
using System.Collections;

// Universal Video Texture Ver. 1.5

public class VideoTexture_FullScreen : MonoBehaviour 
{
	
	public float FPS = 30;
	
	public int firstFrame;
	public int lastFrame;
	
	public string FileName = "VidTex";
	public string digitsFormat = "0000";
	
	public enum digitsLocation {Prefix, Postfix};
	public digitsLocation DigitsLocation = digitsLocation.Postfix;
	
	public float aspectRatio = 1.78f;
	public enum playMode {Once, Loop, PingPong, Random};
	public playMode Playmode;
	
	public enum lowMemoryMode {Disabled, Normal, BruteForce};
	public lowMemoryMode LowMemoryMode = lowMemoryMode.Normal;
	
	public Texture ctiTexture;
	public Texture backgroundTexture;
	public Texture scrollBarTexture;
	
	public float scrollBarLength = 250f;
	public float scrollBarHeight = 50f;
	public float scrollBarOffset = 50f;
	public int timecodeSize = 16;
	
	public bool showScrollBar = true;
	public bool showTimecode = true;
	public bool enableAudio = false;
	
	bool togglePlay = true;
	bool scrubbing = false;
	bool audioAttached = false;
	
	bool firstPlay = true;
	
	string indexStr = "";
	
	Texture newTex;
	Texture lastTex;
	
	float playFactor = 0; // Direction of playback (Forwards / Backwards)
	float index = 0;
	
	int intIndex = 0;
	int lastIndex = -1;
	
	AttachedAudio myAudio = new AttachedAudio(); // Creates an audio class for audio management 
	
	enum audioPlayMode {Play, Toggle, Sync}
		
	Rect CTI; 
	
	GUIStyle style = new GUIStyle();
	
	void Awake()
	{
		//Application.targetFrameRate = 60; (Optional for smoother scrubbing on capable systems)
		
		audioAttached = GetComponent("AudioSource");
		
	// Zeros camera range - effectively blackens the screen
	
		camera.farClipPlane = 0;
		camera.nearClipPlane = 0;
	}
	
	void Start ()
	{	
		CTI = new Rect(0,0,0,0);
		
		style.fontSize = timecodeSize;
	 	style.normal.textColor = Color.white;
		
		index = firstFrame;
		
		if (audioAttached)
		{
			myAudio.attachedAudioSource = audio;
			myAudio.fps = FPS;
			myAudio.frameIndex = firstFrame;
		}
	}
 	
	void Update () 
	{
		// Forces audio sync on first play (helpful for some devices)
		
		if ((firstPlay) && (index < firstFrame + 1))
		{
			myAudio.frameIndex = index;
			myAudio.Sync();
			myAudio.Play();
		}
	
		if (Input.GetMouseButtonDown(0))
		{
			if (showScrollBar && CTI.Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y,0)) && (Input.GetMouseButton(0)))
			{
				myAudio.Stop();
				scrubbing = true;
			}
			
			else
				
			{
				TogglePlay();
				myAudio.frameIndex = index;
				myAudio.Toggle();
			}
		}
			
		if ((scrubbing) && (Input.mousePosition.x > Screen.width / 2 - (scrollBarLength / 2) 
					    && ((Input.mousePosition.x < Screen.width / 2 - (scrollBarLength / 2) + scrollBarLength))))
		{
			CTI = new Rect(Input.mousePosition.x - scrollBarHeight/2, Screen.height - scrollBarOffset, scrollBarHeight, scrollBarHeight);
			index = ((Input.mousePosition.x - (Screen.width / 2 - (scrollBarLength / 2))) / scrollBarLength) * (firstFrame + lastFrame);
			lastIndex = intIndex;
		}
		
		else
			
		{
			CTI = new Rect(Screen.width / 2 - (scrollBarLength / 2) + (scrollBarLength) * index / lastFrame - scrollBarHeight/2, Screen.height-scrollBarOffset,scrollBarHeight,scrollBarHeight);
			index += playFactor * (FPS * Time.deltaTime);
				
			if (intIndex <= firstFrame && audioAttached && enableAudio && playFactor > 0)
			{
				myAudio.frameIndex = index;
				myAudio.Sync();
				myAudio.Play();
			}
		}
		
		if ((scrubbing) && (Input.GetMouseButtonUp(0)))
		{	
				scrubbing = false;
				if (enableAudio && myAudio.togglePlay && playFactor > 0)
			{
					myAudio.frameIndex = index;
					myAudio.Sync();
					myAudio.Play();	
			}
		}
		
		intIndex = (int)index;
		
		//Default to normal play
		
		if (intIndex <= firstFrame)
		{
			playFactor = 1;
		}
		
		//Handle custom play modes
		
		if (index >= lastFrame)
			if (Playmode == playMode.Loop)
				index = firstFrame;
			else
				if (Playmode == playMode.Once)
					index = lastFrame;
				else
					if (Playmode == playMode.PingPong)
						playFactor = -1;
		
		if ((Playmode == playMode.Random) && (intIndex != lastIndex))
			{
				intIndex = Random.Range(firstFrame,lastFrame);
				index = intIndex;
			}
		
		//Memory management
				
		if (intIndex != lastIndex)	
		{
			if (LowMemoryMode == lowMemoryMode.Normal)
			{
				Resources.UnloadAsset(lastTex);
				lastTex = newTex;
			}
			
			else
				
			if (LowMemoryMode == lowMemoryMode.BruteForce)
				Resources.UnloadUnusedAssets();
				
			indexStr = string.Format("{0:" + digitsFormat + "}", intIndex); 
			
			if (DigitsLocation == digitsLocation.Postfix)
				newTex = Resources.Load(FileName + indexStr) as Texture;
			else
				newTex = Resources.Load(indexStr + FileName) as Texture;
			
			lastIndex = intIndex;
		}
	}
	
	void TogglePlay()
	{
		if (togglePlay)
			{
				togglePlay = false;
				Time.timeScale = 0;
				
			}
			else
			{
				togglePlay = true;
				Time.timeScale = 1;
			}
	}
	
	void OnGUI()
		
	{
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),backgroundTexture,ScaleMode.StretchToFill,true,aspectRatio); // Background Texture draw	to avoid ghosting
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),newTex,ScaleMode.ScaleToFit,true,aspectRatio); // Actual video texture draw
		
		if (showTimecode)
		{
			GUI.Label(new Rect(Screen.width / 2 - timecodeSize * 2, Screen.height - scrollBarOffset + scrollBarHeight, scrollBarLength, scrollBarHeight),(string.Format("{0:" + "00" + "}", ((int)(index / FPS / 60)))) + ":" // Minutes
																																			     + (string.Format("{0:" + "00" + "}", ((int)(index / FPS)) % 60) + ":" 	// Seconds
																																				 + (string.Format("{0:" + "00" + "}", intIndex % FPS ))),style); // Frames
		}	
		
		if (showScrollBar)
		{	
			GUI.DrawTexture(new Rect(Screen.width / 2 - (scrollBarLength / 2), Screen.height - scrollBarOffset, scrollBarLength, scrollBarHeight),scrollBarTexture,ScaleMode.StretchToFill,true);
			GUI.DrawTexture(CTI,ctiTexture,ScaleMode.StretchToFill,true);
		}
	}
}

// Class for audio management

public class AttachedAudio
{
	public AudioSource attachedAudioSource;
	
	public float frameIndex = 0;
	public float fps = 0;
	
	public bool togglePlay = true;
	
	public void Play()
	{
		if (attachedAudioSource)
			if (!attachedAudioSource.isPlaying)
				attachedAudioSource.Play();
	}
	
	public void Stop()
	{
		if (attachedAudioSource)
			attachedAudioSource.Stop();
	}
	
	public void Toggle()
	{
		if (togglePlay)
			{
				togglePlay = false;
				
				if (attachedAudioSource)
				attachedAudioSource.Pause();
			}
			else
			{
				togglePlay = true;
				
				if (attachedAudioSource)
				{
					attachedAudioSource.time = frameIndex / fps;
					attachedAudioSource.Play();
				}
			}
	}	
	
	public void Sync()
	{
		if (attachedAudioSource)
			attachedAudioSource.time = frameIndex / fps;
	}
}
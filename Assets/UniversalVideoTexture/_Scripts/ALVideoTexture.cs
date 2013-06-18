using UnityEngine;
using System.Collections;

// Universal Video Texture Ver. 1.5

public class ALVideoTexture : MonoBehaviour {

	public float FPS = 30;
	
	public int firstFrame;
	public int lastFrame;
	
	public string FileName = "VidTex";
	public string digitsFormat = "0000";
	
	public enum digitsLocation {Prefix, Postfix};
	public digitsLocation DigitsLocation = digitsLocation.Postfix;
	
	public enum playMode {Once, Loop, PingPong, Random};
	public playMode Playmode;
	
	public enum textureType {Diffuse, BumpMap, DetailMap, Illumination};
	public textureType TextureType;
	
	public enum lowMemoryMode {Disabled, Normal, BruteForce};
	public lowMemoryMode LowMemoryMode = lowMemoryMode.Normal;
	
	public bool sharedMaterial = false;
	
	public bool enableAudio = false;
	
	bool audioAttached = false;
	
	AttachedAudio myAudio = new AttachedAudio(); // Creates an audio class for audio management //
	
	enum audioPlayMode {Play, Toggle, Sync}
	
	string texType = "";
	string indexStr = "";
	
	Texture newTex;
	Texture lastTex;
	
 	float playFactor = 0; // Direction of playback (Forwards / Backwards)
	float index = 0;
	
	int intIndex = 0;
	int lastIndex = -1;

	public bool hasFinished=false;

	void Awake()
	{
		audioAttached = GetComponent("AudioSource");
	}
	
	void Start ()
	{
		
		index = firstFrame;
	
		if (audioAttached)
		{
			myAudio.attachedAudioSource = audio;
			myAudio.fps = FPS;
			myAudio.frameIndex = firstFrame;
		}
		
		
		switch (TextureType)
		{
		case textureType.Diffuse:
			texType = "_MainTex";
			break;
		case textureType.BumpMap:
			texType = "_BumpMap";
			break;
		case textureType.DetailMap:
			texType = "_Detail";
			break;
		case textureType.Illumination:
			texType = "_Illum";
			break;
		}
	}
 	
	void Update () 
	{
		
		intIndex = (int)index;
		
		//Default to normal play
		
		if (intIndex <= firstFrame)
		{
			
			playFactor = 1;
		}
		
		//Handle custom play modes
		
		if (index >= lastFrame){
			if (Playmode == playMode.Loop){
				index = firstFrame;
			}
			else{
				if (Playmode == playMode.Once){
					index = lastFrame;
					hasFinished=true;
				}
				else{
					if (Playmode == playMode.PingPong){
						playFactor = -1;
					}
				}
			}
		}
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
			
			if (sharedMaterial)					
				renderer.sharedMaterial.SetTexture(texType, newTex);
			else
				renderer.material.SetTexture(texType, newTex);
				
			lastIndex = intIndex;
		}
			
			index += playFactor * (FPS * Time.deltaTime);
				
		if (intIndex <= firstFrame && (audioAttached && enableAudio))
		{
			myAudio.frameIndex = index;
			myAudio.Sync();
			myAudio.Play();
		}
	}
}
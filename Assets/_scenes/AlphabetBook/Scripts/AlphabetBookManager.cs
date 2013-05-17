using UnityEngine;
using System.Collections;

public class AlphabetBookManager : MonoBehaviour {
	
	public OTSprite LetterArea;
	public OTSprite LetterTexture;
	public OTTextSprite Letter;
	public Texture2D[] LetterTextures;
	public OTSprite RecordButton;
	public OTSprite PlaybackButton;
	bool HasRecorded;
	public AudioClip TapSound;
	public GenericContainer[] Containers;
	public Vector2 lastTouchPos;
	PersistentObject PersistentManager;
	public OTSprite camera;
	bool caputreFromCamera=false;
	bool chosenColour=false;
	bool chosenTexture=false;
	bool playedPickPictureAudio;
	public AudioClip PickPicture;
	bool chosenPicture=false;
	bool playedAudioRecordAudio=false;
	public AudioClip RecordAudio;
	bool takenPhoto=false;
	bool playedAudioTakePhoto;
	public AudioClip TakePhoto;
	
	
	// Use this for initialization
	void Start () {
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		PersistentManager.ContentBrowserName="ContentBrowser-Scroll";
		
		if(PersistentManager.CurrentLetter!=null)
			Letter.text=PersistentManager.CurrentLetter;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!HasRecorded)
			PlaybackButton.visible=false;
		else if(HasRecorded && !AudioRecorderBinding.isRecording())
			PlaybackButton.visible=true;
		else PlaybackButton.visible=false;
		
		if(!AudioRecorderBinding.isRecording())
			RecordButton.visible=true;
		else
			RecordButton.visible=false;
		
		if(!playedPickPictureAudio&&chosenColour&&chosenTexture)
		{
			audio.clip=PickPicture;
			audio.Play();
			playedPickPictureAudio=true;
		}
		if(!playedAudioRecordAudio&&chosenPicture)
		{
			audio.clip=RecordAudio;
			audio.Play ();
			playedAudioRecordAudio=true;
		}
		if(!playedAudioTakePhoto && HasRecorded)
		{
			audio.clip=TakePhoto;
			audio.Play();
			playedAudioTakePhoto=true;
		}
		
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_Drag += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_Drag -= On_Drag;
	}
	
	void On_Drag (Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name=="Mountable1"||gesture.pickObject.name=="Mountable2"||gesture.pickObject.name=="Mountable3")
			{
				OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
				s.position+=gesture.deltaPosition;
				lastTouchPos=gesture.position;
			}
			
		}
	}
	
	void On_TouchUp(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name=="Mountable1"||gesture.pickObject.name=="Mountable2"||gesture.pickObject.name=="Mountable3")
			{
				OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
				GenericMountable m=gesture.pickObject.GetComponent<GenericMountable>();
				s.position+=gesture.deltaPosition;
				lastTouchPos=gesture.position-new Vector2(512,384);

				foreach(GenericContainer c in Containers)
				{
					if(m.MountToContainer(c)){
						chosenPicture=true;
						continue;
					}
					else{
						m.UnmountFromContainer(c);
					}
				}
			}
			
		}
	}
	
	private void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null){
		
			if(gesture.pickObject.name=="btnRed")
			{
				Letter.tintColor=gesture.pickObject.gameObject.GetComponent<OTSprite>()._tintColor;
				chosenColour=true;
			}
			else if(gesture.pickObject.name=="btnBlue")
			{
				Letter.tintColor=gesture.pickObject.gameObject.GetComponent<OTSprite>()._tintColor;
				chosenColour=true;
			}
			else if(gesture.pickObject.name=="btnGreen")
			{
				Letter.tintColor=gesture.pickObject.gameObject.GetComponent<OTSprite>()._tintColor;
				chosenColour=true;
			}
			else if(gesture.pickObject.name=="btnTex1")
			{
				LetterTexture.image=(Texture2D)LetterTextures[1];
				chosenTexture=true;
			}
			else if(gesture.pickObject.name=="btnTex2")
			{
				LetterTexture.image=(Texture2D)LetterTextures[2];
				chosenTexture=true;
			}
			else if(gesture.pickObject.name=="btnTex3")
			{
				LetterTexture.image=(Texture2D)LetterTextures[3];
				chosenTexture=true;
			}
			else if(gesture.pickObject.name=="btnRecord")
			{
				AudioRecorderBinding.prepareToRecordFile("g.wav");
				AudioRecorderBinding.recordForDuration(3.0f);
				HasRecorded=true;
			}
			else if(gesture.pickObject.name=="btnPlaybackRec")
			{
				playbackRecording("g.wav");	
			}
			else if(gesture.pickObject.name=="cameraWindow")
			{
//				Debug.Log("we get signal. main screen turn on.");
//				if(LiveTextureBinding.isCaptureAvailable() && !caputreFromCamera){
//					caputreFromCamera=true;
//					camera.image=LiveTextureBinding.startCameraCapture(true,LTCapturePreset.Size640x480);
//				}
//				else if(LiveTextureBinding.isCaptureAvailable() && caputreFromCamera){
//					caputreFromCamera=false;
//					LiveTextureBinding.stopCameraCapture();
//				}
			}
			else 
			{
				return;	
			}
			
			PersistentManager.PlayAudioClip(TapSound);
		}
	}
	
	public void playbackRecording(string filename)
	{
		string file="file://localhost" + Application.persistentDataPath + "/" + filename;
		StartCoroutine( AudioRecorderBinding.loadAudioFileAtPath( file, onError, onSuccess ) );
	}
	
	public void onError( string error )
	{
		Debug.LogError( "error loading audio file: " + error );
	}
	
	public void onSuccess( AudioClip clip )
	{
		audio.clip = clip;
		audio.Play();
	}
}

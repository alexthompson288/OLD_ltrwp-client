using UnityEngine;
using System.Collections;

public class FunnyVoicesManager : MonoBehaviour {
	
	public ArrayList Buttons;
	public string LastPlayed;
	public OTTextSprite CurrentLabel;
	public bool HasSelection;
	public GameObject pip;
	public bool exitShowing=false;
	
	// Use this for initialization
	void Start () {
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject PersistentManager=new GameObject("PersistentManager");
			PersistentManager.AddComponent<PersistentObject>();
		}		
		
		HideSelectionPanel();
		
		GameObject btnExit=GameObject.Find ("btnExit");
		btnExit.renderer.enabled=false;
		
		if(Buttons==null)
			Buttons=new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
		
		GameObject btnPlay=GameObject.Find ("btnPlay");
		
		if(HasSelection && !AudioRecorderBinding.isRecording())
		{
			btnPlay.renderer.enabled=true;
		}
		else
		{
			btnPlay.renderer.enabled=false;
		}
		
		
	}
	
	public void HideSelectionPanel() {
		
		HasSelection=false;
		
		GameObject btnNext=GameObject.Find ("btnNext");
		btnNext.renderer.enabled=false;
		
		GameObject btnReplay=GameObject.Find ("btnReplay");
		btnReplay.renderer.enabled=false;
	
	}
	
	public void ShowSelectionPanel() {
		
		HasSelection=true;
		
		GameObject btnNext=GameObject.Find ("btnNext");
		btnNext.renderer.enabled=true;
		
		GameObject btnReplay=GameObject.Find ("btnReplay");
		btnReplay.renderer.enabled=true;
	}
	
	public void showExitButton() {
	
		if(exitShowing)return;
		GameObject btnExit=GameObject.Find ("btnExit");
		btnExit.renderer.enabled=true;
		exitShowing=true;
		
	}
	
	public void prepareAndRecord(string filename)
	{
		AudioRecorderBinding.prepareToRecordFile(filename);
		AudioRecorderBinding.recordForDuration(4.0f);
		
	}
	
	public void playbackRecording(string filename)
	{
		string file="file://localhost" + Application.persistentDataPath + "/" + filename;
		Debug.Log ("play dis"+file);
		
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

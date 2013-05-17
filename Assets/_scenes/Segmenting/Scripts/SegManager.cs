using UnityEngine;
using System.Collections;

public class SegManager : MonoBehaviour {
	
	public int mountedLetters;
	public int lettersInProblem=3;
	public int containerButtonsPressed=0;
	public bool playing=false;
	
	public AudioClip audioIntro;
	public AudioClip[] audioLetters;

	
	float audioReqDelay=0;
	bool playedAudioReq=true;
	
	float inactivetime=0;
	bool playedParticle;
	
	public OTTextSprite[] letterThings;
	
	public Container[] ActiveContainers;
	
	public ParticleSystem successParticle;
	float countdownToExit=4.0f;
	bool exitCountdown=false;
	PersistentObject PersistentManager;
	
	// Use this for initialization
	void Start () {
		Application.targetFrameRate=60;
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}
		
		successParticle.Stop();
		foreach(OTTextSprite s in letterThings)
		{
			s.enabled=false;
		}
		
	}	
	
	// Update is called once per frame
	void Update () {
	
		if(lettersInProblem==containerButtonsPressed && !playing)
		{
			StartGame();
		}
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0)
			Application.LoadLevel(PersistentManager.ContentBrowserName);
		
		
		if(!playedParticle){
			int letters=0;
			
			foreach(Container c in ActiveContainers)
			{
					if(c.mountedLetter!=null)letters++;
			}
			
			if(letters==lettersInProblem)
			{
				successParticle.Play();
				successParticle.audio.Play ();
				playedParticle=true;
				exitCountdown=true;
			}
			
		}
		if(inactivetime>0)
		{
			inactivetime-=Time.deltaTime;
		}
		else
		{	
			audioReqDelay-=Time.deltaTime;
			if(audioReqDelay<0 && !playedAudioReq)
			{
				audio.Play();
				playedAudioReq=true;
			}
				
		}
	}
	
	void StartGame () {
	
		playing=true;
	
		foreach(OTTextSprite s in letterThings)
		{
			s.enabled=true;
		}
	
	}
	
	public void StopGame () {
	
		playing=false;
		
	}

	public void pressContainerButton(){
		containerButtonsPressed++;
	}
	
	public void mountedCounter(int thisValue)
	{
		mountedLetters+=thisValue;
	}
}

using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	public Transform SpherePrefab;
	public Transform FishPrefab;
	public Transform VortexStart;
	public Transform VortexEnd;
	
	public bool playing=false;
	public Transform uiSplash;
	
	public AudioClip audioIntro;
	public AudioClip successClip;
	public AudioClip[] audioLetters;
	
	ArrayList letters;
	
	public ArrayList currentBalls;
	ArrayList destroyLetters;
	public int currentLetter;
	
	float audioReqDelay=0;
	bool playedAudioReq=true;
	
	float inactivetime=0;
	
	public Transform sack;
	public Transform pip;
	public Transform lookat;
	public Transform seaweed;
	
	public ArrayList explosions;
	
	public ParticleSystem Bubbles;
	float countdownToExit=4.0f;
	bool exitCountdown=false;
	
	int initedBubbles=0;
	int expectedInitBubbles=5;
	float timeToInit=0.0f;
	
	Vector3 sackDefaultScale;
	
	// Use this for initialization
	void Start () {
//		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutQuad", "loopType", "pingPong", "delay", 0.1, "time", 6.0, "lookat", lookat));
		Application.targetFrameRate=60;
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject PersistentManager=new GameObject("PersistentManager");
			PersistentManager.AddComponent<PersistentObject>();
		}
		
		Transform vortex=(Transform)Instantiate(VortexStart);
		animateMesh anim=vortex.GetComponent<animateMesh>();
		anim.PlayWithDelay(0.1f);
		anim.destroyOnComplete=true;
		
		currentBalls=new ArrayList();
		explosions=new ArrayList();
		destroyLetters=new ArrayList();
		Bubbles.Stop ();
	}
	
	void RemoveSpentExplosions()
	{
		ArrayList spent=new ArrayList();
		
		foreach(ParticleSystem part in explosions)
		{
			if(!part.isPlaying)
				spent.Add (part);
		}
		
		foreach(ParticleSystem part in spent)
		{
			Debug.Log ("Remove explode");
			explosions.Remove (part);
			GameObject.Destroy(part);
		}
		
	}
	
	public void GotLetter(Transform callingBall)
	{
		Transform ballLetter=callingBall.GetComponent<Bally>().MyLetter;
		//play what they just selected
		audio.Play ();
		audio.clip=audioLetters[currentLetter];
//		audio.Play();
		playedAudioReq=false;
		
		ballLetter.parent=null;
		iTween.MoveTo(ballLetter.gameObject,new Vector3(sack.position.x, sack.position.y, 0.8f), 0.5f);
		backpackSquashAndBounce("first");
		destroyLetters.Add (ballLetter);
		
		//find instance of selected in letters and remove
		letters.Remove(currentLetter);
		
		//iTween.PunchScale(sack.gameObject, new Vector3(0.002f, 0.002f, 0), 0.5f);
		
		currentBalls.Remove(callingBall);
		
		if(letters.Count==0)
		{
			StopGame();
			return;
		}
		
		getNextLetter();
		CreateNewSphere();
		
		audioReqDelay=1.5f;
		playedAudioReq=false;
		
//		Debug.Log("letters: " + letters.Count.ToString() + " current letter: " + currentLetter.ToString() + " ")
	}
	
	public void CreateNewSphere()
	{
		Transform newObject=(Transform)Instantiate(SpherePrefab);
		
		newObject.position=new Vector3(Random.Range(-5.89f,-2.9f), Random.Range(-1.77f,0.54f), 0);
//		newObject.position=new Vector3(Random.Range(-6.1f,1.2f), Random.Range(-3.2f,5.3f), 0);
//		newObject.position=new Vector3(-1.09f, 1.62f, 0.0f);
		newObject.localScale=new Vector3(0.1f,0.1f,0.1f);
		
		//look to see if we have one of the current letter in play
		bool found=ballsContainLetter(currentLetter);
		
		((Bally)newObject.GetComponent("Bally")).materialIndex=Random.Range(0,2);
		
		if(!found) ((Bally)newObject.GetComponent("Bally")).letterIndex=currentLetter;
		else ((Bally)newObject.GetComponent("Bally")).letterIndex=Random.Range(0,7);
		
		iTween.ScaleTo(newObject.gameObject, new Vector3(2.0f,2.0f,2.0f), 1.5f);
		
		currentBalls.Add(newObject);
	}
	
	public void CreateFish() {
		Instantiate(FishPrefab);
	}
	
	bool ballsContainLetter(int theLetter)
	{
		bool found=false;
		foreach(Transform bt in currentBalls)
		{
			if(((Bally)bt.GetComponent("Bally")).letterIndex==theLetter)
			{
				found=true;
				break;
			}
		}
		return found;
	}
	
	public void OnMouseDown()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;
		
		if(countdownToExit<0)
			Application.LoadLevel("ContentBrowser");
				
		
		if(inactivetime>0)
		{
			inactivetime-=Time.deltaTime;
		}
		else
		{
			if(EasyTouch.GetTouchCount()>0 && !playing)	
			{
				StartGame();
			}
			
			audioReqDelay-=Time.deltaTime;
			if(audioReqDelay<0 && !playedAudioReq && playing)
			{
				audio.clip=audioLetters[currentLetter];
				audio.Play();
				playedAudioReq=true;
				audioReqDelay=2.0f;
			}
				
		}
		
		if(initedBubbles<expectedInitBubbles && playing)
		{
			timeToInit+=Time.deltaTime;
			
			if(timeToInit>1.0f)
			{
				CreateNewSphere();
				timeToInit=0.0f;
				initedBubbles++;
			}
			
		}
		
		ArrayList removeStuff=new ArrayList();
		
		foreach(Transform t in destroyLetters)
		{
			if((t.position-sack.position).magnitude<=1.88f){
				removeStuff.Add(t);
				GameObject.Destroy (t.gameObject);
			}
		}
		
		foreach(Transform t in removeStuff)
		{
			destroyLetters.Remove(t);	
		}
		
		//RemoveSpentExplosions();
	}
	
	void StartGame() {
		Bubbles.Play ();
		playing=true;
		uiSplash.renderer.enabled=false;
		
		CreateFish();
		
		
		//create array of required solutions
		letters=new ArrayList();
		for (int i=0; i<8; i++)
		{
			letters.Add(i);
			letters.Add(i);
		}
		
		getNextLetter();
		
//		for(int i=0;i<5;i++)
//		{
//			CreateNewSphere();	
//		}	
		
		audio.clip=audioIntro;
		audio.Play();
		
		audioReqDelay=2.0f;
		playedAudioReq=false;
		
	}
	
	public void StopGame() {
		Bubbles.Stop();
		playing=false;
//		uiSplash.renderer.enabled=true;
		inactivetime=1.0f;
		
		foreach(Transform t in currentBalls)
		{
			GameObject.Destroy(t.gameObject);
		}
		
		currentBalls.Clear();
		
		audio.clip=successClip;
		audio.Play();
		exitCountdown=true;
		
		Transform vortex=(Transform)Instantiate(VortexEnd);
		animateMesh anim=vortex.GetComponent<animateMesh>();
		anim.PlayWithDelay(3.0f);
		
//		currentBalls=new ArrayList();
	}
	
	void getNextLetter() {
		currentLetter=(int)letters[Random.Range(0, letters.Count-1)];
	}
	
	void backpackSquashAndBounce(string part){
		
		
	    switch(part){
	
	        case "first":
				sackDefaultScale=sack.localScale;
//				iTween.PunchScale(sack.gameObject,new Vector3(sack.localScale.x,sack.localScale.y-0.006f,sack.localScale.z),0.6f);
				iTween.ScaleTo(sack.gameObject,new Vector3(sack.localScale.x,sack.localScale.y-0.002f,sack.localScale.z),0.3f);
				iTween.MoveBy(sack.gameObject,iTween.Hash ("time",0.3,"y",1.0, "onCompleteTarget",gameObject, "onComplete","backpackSquashAndBounce", "onCompleteParams","second"));
//				iTween.ColorTo(sack.gameObject,new Color(255.0f,255.0f,108.0f,255.0f),0.3f);
	        break;
	        
	        case "second":
				iTween.ScaleTo(sack.gameObject,sackDefaultScale,0.3f);
				iTween.MoveBy(sack.gameObject,iTween.Hash ("time",0.3,"y",-1.0));
//				iTween.ColorTo(sack.gameObject,new Color(255.0f,255.0f,255.0f,255.0f),0.3f);
	        break;
	
	    }


	}
}

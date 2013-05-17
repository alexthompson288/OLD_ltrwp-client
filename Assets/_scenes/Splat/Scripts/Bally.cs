using UnityEngine;
using System.Collections;

public class Bally : MonoBehaviour {
	
	float timeSinceMoved=0;
	float timeSinceClicked=0;
	Manager gameManager;
	bool haveBeenPressed=false;
	public Material[] materiallist;
	public string[] letterlist;
	public int letterIndex=0;
	public int materialIndex=0;
	public ParticleSystem ExplodeParticle;
	public OTTextSprite SpriteFont;
	public Transform MyLetter;
	bool exploded=false;
	
	
	
	// Use this for initialization
	void Start () {
		
		MoveBall(true);
		transform.Find("Cube").renderer.material=materiallist[materialIndex];
		
		SpriteFont.text=letterlist[letterIndex];
		
		
		//renderer.material=renderer.materials[letterIndex];
		
		gameManager=(Manager)GameObject.Find("Main Camera").GetComponent(typeof(Manager));
	}
	
	
	void MoveBall (bool hasJustStarted) {
		//Debug.Log ("Move things");
		if(hasJustStarted)
			rigidbody.AddForce(0,Random.Range(10,30),0);
		else
			rigidbody.AddForce(Random.Range(-20,20),Random.Range(-20,20),0);
	}
	
	void OnCollisionEnter(Collision c) {
		if(haveBeenPressed)
		{
			Physics.IgnoreCollision(collider, c.collider);
		}
	}
	
	void OnMouseDown() {

	}
	
	// Update is called once per frame
	void Update () {
		timeSinceMoved+=Time.deltaTime;
		
		if(haveBeenPressed)
		{
			timeSinceClicked+=Time.deltaTime;
			ExplodeBubble();
		}
		
		if(timeSinceClicked>0.2f)
			Destroy(gameObject);
		
		if(timeSinceMoved>1 && (rigidbody.velocity.x<0.05f||rigidbody.velocity.y<0.05f))
		{
			MoveBall(false);
			timeSinceMoved=0;
		}
	}
	
		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
			
			if(!gameManager.playing)return;
		
			if(!haveBeenPressed){
			
			if(gameManager.currentLetter==letterIndex)
			{
				rigidbody.isKinematic=true;
				collider.isTrigger=true;
				ExplodeBubble ();
				//tell mgr to get new letter, create new sphere etc
				gameManager.GotLetter(transform);
			
//				gameManager.currentBalls.Remove(transform);
				
				haveBeenPressed=true;
			}
			else 
			{
				iTween.ShakePosition(gameObject, new Vector3(0.5f, 0.0f, 0.0f), 0.3f);		
			}
			
		}
			
		}
	}
	
	private void ExplodeBubble()
	{
		if(exploded)return;
		exploded=true;
		iTween.ScaleTo(gameObject, new Vector3(gameObject.transform.localScale.x*1.2f, gameObject.transform.localScale.y*1.2f, gameObject.transform.localScale.z*1.2f),0.2f);
		ParticleSystem newObject=(ParticleSystem)Instantiate(ExplodeParticle);
		newObject.transform.position=gameObject.transform.position;
		newObject.loop=false;
		newObject.Play();
		gameManager.explosions.Add(newObject);
	}
}

using UnityEngine;
using System.Collections;

public class SplatBally : MonoBehaviour {
	
	float timeSinceMoved=0;
	float timeSinceClicked=0;
	SplatManager gameManager;
	bool haveBeenPressed=false;
	public string[] letterlist;
	public int letterIndex=0;
	public int materialIndex=0;
	public ParticleSystem ExplodeParticle;
	public OTTextSprite SpriteFont;
	public Transform MyLetter;
	bool exploded=false;
	public bool isOffScreen;
	public Texture2D SplatImage;
	PersistentObject PersistentManager;
	
	
	
	// Use this for initialization
	void Start () {

		gameManager=(SplatManager)GameObject.Find("Main Camera").GetComponent<SplatManager>();
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			thisPO.GetComponent<PersistentObject>().CurrentTheme="Forest";
		}
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		gameObject.GetComponent<OTSprite>().spriteContainer=gameManager.sceneMgr.ContainerSprites;
		gameObject.GetComponent<OTSprite>().frameIndex=materialIndex;
		gameObject.GetComponent<OTSprite>().ForceUpdate();
		// gameObject.GetComponent<OTSprite>().frameName="Splat_"+gameManager.sceneMgr.setting+"_A_"+materialIndex+".png";

		MoveBall(true);
		
		SpriteFont.text=letterlist[letterIndex];	
		MyLetter.GetComponent<OTTextSprite>().spriteContainer=GameObject.Find ("Font Arial-Black-64").GetComponent<OTSpriteAtlasCocos2DFnt>();
	
		rigidbody.freezeRotation=true;
		
		//renderer.material=renderer.materials[letterIndex];
	}
	
	
	void MoveBall (bool hasJustStarted) {
		if(isOffScreen)return;
		//Debug.Log ("Move things");
		if(hasJustStarted)
			rigidbody.AddForce(0,Random.Range(1000,1200),0);
		else
			rigidbody.AddForce(Random.Range(-300,300),Random.Range(-300,300),0);
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
		
		gameObject.GetComponent<OTSprite>().rotation=0.0f;
		
		if(haveBeenPressed)
		{
			timeSinceClicked+=Time.deltaTime;
			ExplodeBubble();
		}
		
		if(timeSinceClicked>0.5f)
			Destroy(gameObject);
		
		if(timeSinceMoved>1 && (rigidbody.velocity.x<400.0f||rigidbody.velocity.y<400.0f))
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
	private void On_SimpleTap(Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
			if(!gameManager.playing||!gameManager.allowInteraction||gameManager.initedBubbles<gameManager.expectedInitBubbles)return;
		
			if(!haveBeenPressed){
			
			if(gameManager.currentLetter==letterIndex)
			{
				rigidbody.isKinematic=true;
				collider.isTrigger=true;
				if(gameManager.ContainerHasStateB){
					gameObject.GetComponent<OTSprite>().frameIndex=materialIndex+gameManager.NumberOfContainerVariants;
					gameObject.GetComponent<OTSprite>().ForceUpdate();
				}
				if(PersistentManager.CurrentTheme=="underwater")
					ExplodeBubble ();
				else if(PersistentManager.CurrentTheme=="forest")
					SplatMushroom ();
					//tell mgr to get new letter, create new sphere etc
				gameManager.GotLetter(transform);
				gameManager.backpackSquashAndBounce("first");
				gameManager.PlayPipPositiveHit();		
//				gameManager.currentBalls.Remove(transform);
				
				haveBeenPressed=true;
			}
			else 
			{
				PersistentManager.PlayAudioClip(gameManager.IncorrectHit);
				gameManager.PlayBennyNegativeHit();

//				OTSprite s=gameObject.GetComponent<OTSprite>();
//				OTTween mt=new OTTween(s,0.3f, OTEasing.ElasticInOut);
//				mt.Tween("position", new Vector2(s.position.x+1.0f,s.position.y));

				iTween.ShakePosition(gameObject, new Vector3(20.0f, 0.0f, 0.0f), 0.3f);
				gameManager.pipani.playIdleSet=false;
				gameManager.pipani.SetNonePlaying();
				gameManager.pipani.playDoubt=true;
				gameManager.tapsSinceLastCorrect++;
			}
			
		}
			
		}
	}
	
	private void SplatMushroom()
	{
		if(exploded)return;
			exploded=true;
		
		OTSprite s=gameObject.GetComponent<OTSprite>();
		
		if(SplatImage!=null)
			s.image=SplatImage;
		
		foreach(Transform t in transform)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				OTTextSprite txt=t.GetComponent<OTTextSprite>();
				OTTween fot=new OTTween(txt, 1.0f, OTEasing.Linear);
				fot.Tween ("alpha",0.3f);
			}
		}
		OTTween mt=new OTTween(s,0.8f, OTEasing.ElasticIn);
		OTTween fo=new OTTween(s, 0.5f, OTEasing.Linear);
		mt.Tween("size", new Vector2(0,0));	
		fo.Tween("alpha", 0.5f);
	}
	
	private void ExplodeBubble()
	{
		if(exploded)return;
		exploded=true;
		OTSprite s=gameObject.GetComponent<OTSprite>();
		OTTween mt=new OTTween(s,0.5f, OTEasing.BackIn);
		mt.Tween("size", new Vector2(0, 0));
	}
}

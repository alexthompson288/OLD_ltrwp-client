using UnityEngine;
using System.Collections;

public class SegBally : MonoBehaviour {
	
	float timeSinceMoved=0;
	SegManager gameManager;
	bool haveBeenPressed=false;
	public Material[] materiallist;
	public int letterIndex=0;
	bool pickedUp=false;
//	public Transform TargetContainer;
	public Transform[] PotentialTargets;
	public AudioClip MountSound;
	public Plane basePlane;
	
	// Use this for initialization
	void Start () {
		//renderer.material=renderer.materials[letterIndex];
		gameManager=(SegManager)GameObject.Find("Main Camera").GetComponent(typeof(SegManager));
		basePlane=new Plane(-Vector3.forward, new Vector3(0, 0, gameObject.transform.position.z));
	}
	
	void OnCollisionEnter(Collision c) {
		if(haveBeenPressed)
		{
			Physics.IgnoreCollision(collider, c.collider);
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceMoved+=Time.deltaTime;
		

	}
	
		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
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
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_Drag -= On_Drag;
	}
	
	// Simple tap
	private void On_TouchStart(Gesture gesture){
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
			Debug.Log ("Touched");
			if(!gameManager.playing)return;
	
			
				
			collider.isTrigger=true;
			pickedUp=true;
			//iTween.MoveTo(gameObject, new Vector3(-6.2f,-3.38f,0), 1.0f);				
				//tell mgr to get new letter, create new sphere etc
			//gameManager.GotLetter(transform);
			

			
		}
	}

	private void On_Drag(Gesture gesture){
		if(gesture.pickObject==gameObject&&pickedUp)
		{
//		   Vector3 nowMousePos=Input.mousePosition;
			
			Ray ray = Camera.main.ScreenPointToRay(gesture.position);
			
			float distance;
           	if (basePlane.Raycast(ray, out distance)){
//				Debug.Log ("Distance " + distance);
            	Vector3 vec = ray.GetPoint(distance); // get the plane point hit 
				gameObject.transform.position=new Vector3(vec.x, vec.y+1.2f, vec.z-0.5f);
			}
			
       }
	}
	
	private void On_TouchUp(Gesture gesture){

		if(gesture.pickObject==gameObject){
			
			foreach(Transform t in PotentialTargets)
			{
				Vector3 objPos3 = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				Vector2 objPos = new Vector2(objPos3.x, objPos3.y);
				
				Vector3 contPos3 = Camera.main.WorldToScreenPoint(t.position);
				Vector2 contPos = new Vector2(contPos3.x, contPos3.y);
				
				Vector3 endPos = new Vector3(t.position.x, t.position.y, gameObject.transform.position.z);
				
				Container myContainer=t.GetComponent<Container>();
				
				if((objPos-contPos).magnitude<80.0f)
				{
					if(myContainer.lastContainer!=null&&myContainer.lastContainer.mountedLetter==null)return;
					
					gameObject.transform.position=endPos;
					myContainer.mountedLetter=gameObject;
					myContainer.ChangeButtonMaterial();
					gameManager.mountedCounter(1);
					audio.clip=MountSound;
					audio.Play();
					break;
				}
				else
				{
					if(myContainer.mountedLetter==gameObject){
						myContainer.mountedLetter=null;
						gameManager.mountedCounter(-1);
					}
				}
			}
			
			pickedUp=false;
		}
	}
	
	public Rect BoundsToScreenRect(Bounds bounds)
	{
	    // Get mesh origin and farthest extent (this works best with simple convex meshes)
	    Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
	    Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x-bounds.min.x, bounds.max.y-bounds.min.y, 0f));
		
	    // Create rect in screen space and return - does not account for camera perspective
	    return new Rect(origin.x, Screen.height - origin.y, extent.x, extent.y);
	}
}

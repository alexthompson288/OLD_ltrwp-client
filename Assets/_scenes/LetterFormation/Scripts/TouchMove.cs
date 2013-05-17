using UnityEngine;
using System.Collections;
using SplineUtilities;

public class TouchMove : MonoBehaviour {
	
	public OTSpline LetterSpline;
	Vector3 deltaPosition;
	bool endOfCurrentSpline;
	public ParticleSystem Pather;
	public ParticleSystem Positioner;
	public Transform PointTouchPrefab;
	bool endOfAllSplines;
	
	float permitDistance=1.0f;
	
	// Update is called once per frame
	void Update () {
		if(endOfCurrentSpline&&LetterSpline.GetComponent<LFLetterPart>().NextPart!=null)
		{
			LetterSpline=LetterSpline.GetComponent<LFLetterPart>().NextPart;
			GameObject follower=GameObject.Find ("PointFollow");
			follower.GetComponent<FollowMove>().LetterSpline=LetterSpline;
			Transform newPointTouch=(Transform)Instantiate(PointTouchPrefab, LetterSpline.points[0].position, gameObject.transform.rotation);
			newPointTouch.GetComponent<TouchMove>().LetterSpline=LetterSpline;
			Pather.Pause ();
			Positioner.Stop ();
			Debug.Log ("Changed letter spline to "+LetterSpline.gameObject.name);
			endOfCurrentSpline=false;
		}
		else if(endOfCurrentSpline&&LetterSpline.GetComponent<LFLetterPart>().NextPart==null)
		{
			Debug.Log ("complete");	
			endOfCurrentSpline=false;
			endOfAllSplines=true;
		}
		else if(endOfAllSplines)
		{
			Debug.Log ("All done");
			GameObject follower=GameObject.Find ("PointFollow");

			foreach(Transform t in follower.transform)
			{
				ParticleSystem ps=t.GetComponent<ParticleSystem>();
				ps.Stop ();
			}
						
			
			Pather.Pause ();
			Positioner.Stop ();
			endOfAllSplines=false;
		}
	}
	
	
		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_DragStart += On_DragStart;
		EasyTouch.On_DragEnd += On_DragEnd;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_DragStart -= On_DragStart;
		EasyTouch.On_DragEnd -= On_DragEnd;
	}	
	
	
	void Start(){
		//textMesh = transform.Find("TextDrag").transform.gameObject.GetComponent("TextMesh") as TextMesh;
	}
	
	// At the drag beginning 
	void On_DragStart(Gesture gesture){
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
//			gameObject.renderer.material.color = new Color( Random.Range(0.0f,1.0f),  Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
//		
//			// the world coordinate from touch for z=5
//			Vector3 position = gesture.GetTouchToWordlPoint(10);
//			
//			float param=LetterSpline.distance;
//			
//			float param = SplineUtils.WrapValue( LetterSpline.GetClosestPointParam(position, 5 ) + 0, 0f, 1f, WrapMode.Clamp );
//			Vector2 positionOnSpline = LetterSpline.GetPosition(param);
//			
//			Debug.Log ("param "+param+" and pos on spline "+positionOnSpline);
//			deltaPosition = positionOnSpline - transform.position;
		}	
	}
	
	void On_SimpleTap(Gesture gesture){
		
		Debug.Log ("tappy tappy");
	}
	
	// During the drag
	void On_Drag(Gesture gesture){
	
		// Verification that the action on the object
		if (gesture.pickObject == gameObject && !endOfCurrentSpline){
//			
//			// the world coordinate from touch for z=5
//			Vector3 position = gesture.GetTouchToWordlPoint(10);
//			float param = SplineUtils.WrapValue(LetterSpline.GetClosestPointParam(position, 5 ) + 0, 0f, 1f, WrapMode.Clamp );
//			Vector3 positionOnSpline = LetterSpline.GetPositionOnSpline(param);
//			
//			//only move if within range of object
//			Vector3 distToObj=(positionOnSpline-deltaPosition) - transform.position;
//			float absDist=Mathf.Abs(distToObj.magnitude);
//			
//			if(absDist<permitDistance)
//				transform.position = positionOnSpline - deltaPosition;
//			
//			
////			Debug.Log ("Pos param: "+param+" // current LetterSpline: "+LetterSpline.gameObject.name);
////			
//			if(param==1){
//				endOfCurrentSpline=true;
//				return;
//			}
			// Get the drag angle
//			float angle = gesture.GetSwipeOrDragAngle();
			
//			textMesh.text = gesture.swipe.ToString() + " / angle :" + angle.ToString("f2");
		}
	}
	
	// At the drag end
	void On_DragEnd(Gesture gesture){
	
		// Verification that the action on the object
		if (gesture.pickObject == gameObject){
//			transform.position= new Vector3(3f,1.8f,-5f);
//			gameObject.renderer.material.color = Color.white;
//			textMesh.text="Drag me";
		}
	}
}

using UnityEngine;
using System.Collections;

public class SegmentingContainerButton : MonoBehaviour {
	
	SegmentingManager gameManager;
	SegmentingContainer myContainer;
	public Texture2D pressedState;
	public Texture2D unpressedState;
	public Texture2D splitDigraph;
	public Texture2D barPressedState;
	public Texture2D barUnpressedState;
	bool isPressed;
	bool hasPlayedSound=false;
	bool PlaySound=true;
	bool ReusableButton=false;
	
	// Use this for initialization
	void Start () {

	}
	
	void Awake () {
		gameManager=GameObject.Find("Main Camera").GetComponent<SegmentingManager>();
		myContainer=transform.parent.GetComponent<SegmentingContainer>();
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
	}

	void On_TouchUp (Gesture gesture)
	{
		if(gesture.pickObject==gameObject)
		{
			if(ReusableButton)
			{
				OTSprite s=gameObject.GetComponent<OTSprite>();
				
				if(myContainer.isMultiPartLetter)
					s.image=barUnpressedState;
				else
					s.image=unpressedState;				
				
				hasPlayedSound=false;
				
			}
			if(myContainer.HighlightLettersFromButton)
			{
				myContainer.HighlightMountedObject(false);
			}
		}
	}

	void On_TouchDown (Gesture gesture)
	{
		OTSprite s=transform.GetComponent<OTSprite>();
		
		float leftMost=s.worldPosition.x-(s.worldSize.x/2);
		float bottomMost=s.worldPosition.y-(s.worldSize.y/2);
		Vector2 gesturePos=new Vector2(gesture.position.x-(Screen.width/2), gesture.position.y-(Screen.height/2));
		// Vector2 gesturePos=new Vector2(gesture.position.x, gesture.position.y);
		 Rect myHitBox=new Rect(leftMost-10,bottomMost-10,s.worldSize.x+20,s.worldSize.y+20);

		// Debug.Log("HitBox: "+myHitBox+" gesture pos "+gesturePos);

		if(myHitBox.Contains(gesturePos))
		{
			SegmentingContainer cont=transform.parent.gameObject.GetComponent<SegmentingContainer>();
			
			if(!isPressed||ReusableButton)
			{
				isPressed=true;

				if(myContainer.isMultiPartLetter)
					s.image=barPressedState;
				else
					s.image=pressedState;				

				if(PlaySound && !hasPlayedSound){
					hasPlayedSound=true;
					cont.audio.clip=gameManager.AudioLetter(cont.AudioLetter);
					cont.audio.Play ();
				}

			}
			
			if(myContainer.HighlightLettersFromButton)
			{
				myContainer.HighlightMountedObject(true);
			}
			
			if(!cont.ContainerEnabled)
			{
				cont.ContainerEnabled=true;
				cont.ShowContainer();
				gameManager.ShowNextButton();
			}
		}
	
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}
		
	public void Show() {

		if(myContainer.isMultiPartLetter)
		{
			float scaleX=0;
			if(myContainer.ExpectedLetter.Length==2)
				scaleX=0.46875f;
			else if(myContainer.ExpectedLetter.Length==3)
				scaleX=0.703125f;

			OTSprite s=gameObject.GetComponent<OTSprite>();
			s.image=barUnpressedState;
			s.size=new Vector2(s.size.x*(1-scaleX), s.size.y);
		}

		renderer.enabled=true;
	}
	public void Hide() {
		renderer.enabled=false;	
	}
	
	public void ReuseButton(bool canReuse)
	{
		ReusableButton=canReuse;
	}
}

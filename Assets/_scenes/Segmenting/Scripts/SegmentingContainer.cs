using UnityEngine;
using System.Collections;

public class SegmentingContainer : MonoBehaviour {
	
	SegmentingManager gameManager;
	public string ExpectedLetter;
	public string AudioLetter;
	public Transform MountedLetter;
	public bool Hidden;
	public bool HiddenButton;
	public bool HiddenContainer;
	public bool ContainerEnabled;
	public bool CanReuseButton;
	public bool HighlightLettersFromButton;
	public SegmentingContainer firstDigraphPart;
	public bool isSplitDigraph=false;
	public bool isMultiPartLetter=false;
	SegmentingContainerButton MyButton;
	
	// Use this for initialization
	void Awake () {
		gameManager=GameObject.Find("Main Camera").GetComponent<SegmentingManager>();
		Transform newBtn = gameManager.CreateNewButton(transform.position, transform.rotation);
		// newBtn.position=new Vector3(newBtn.position.x, newBtn.position.y+100, newBtn.position.z);
		newBtn.parent=transform;

		SetMyButton();
		
		
		if(Hidden)
		{
			HideButton();
			HideContainer();
		}
		
		if(HiddenButton)
			HideButton();
		
		if(HiddenContainer)
			HideContainer();
		
		// SetButtonReuse(CanReuseButton);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	bool SetMyButton(){

		if(MyButton==null)
		{
			foreach(Transform t in transform)
			{
				if(t.gameObject.GetComponent<SegmentingContainerButton>()!=null){
					MyButton=t.gameObject.GetComponent<SegmentingContainerButton>();
					return true;
				}
			}	
			
		}
		return false;
	}
	
	public void ShowButton() {
		
		if(MyButton==null)
			SetMyButton();
		
		HiddenButton=false;
		
		MyButton.Show ();
	}
	public void HideButton() {
		if(MyButton==null)
			SetMyButton();
	
		HiddenButton=true;
		
		MyButton.Hide ();
	}
	
	public void HideContainer() {
		HiddenContainer=true;
		renderer.enabled=false;
	}
	
	public void ShowContainer() {
		HiddenContainer=false;
		renderer.enabled=true;

		SegmentingContainer myContainer=gameObject.GetComponent<SegmentingContainer>();

		OTSprite mySprite=gameObject.GetComponent<OTSprite>();

		if(myContainer.isSplitDigraph && myContainer.ExpectedLetter!=myContainer.AudioLetter)
		{
			OTSprite sprite=OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
			// sprite.transform.parent=transform;
			sprite.image=MyButton.splitDigraph;
			sprite.pivotPoint=new Vector2(0.5f,2.1f);
			sprite.transparent=true;
			// sprite.position=new Vector2(1.0f, 0.5f);
			sprite.position=mySprite.position;
			sprite.size=new Vector2(sprite.size.x-40,sprite.size.y);
			// new Vector2(mySprite.position.x, mySprite.position.y-40);
			sprite.depth=mySprite.depth;
		}
	}
	
	void SetButtonReuse(bool CanReuse){
		if(MyButton==null)
			SetMyButton();
		
		MyButton.ReuseButton(CanReuse);
		
		
	}
	
	public void HighlightMountedObject(bool on)
	{
		OTSprite s=MountedLetter.GetComponent<OTSprite>();
		if(on)
		{
			s.tintColor=new Color(255.0f, 0.0f, 0.0f, 1);
		}
		else
		{
			s.tintColor=new Color(255.0f, 255.0f, 255.0f, 1);
		}
	}
}

using UnityEngine;
using System.Collections;

public class GenericDraggable : MonoBehaviour {

	public bool isPotentialAnswer=false;
	public bool correctDropCreatesNew;
	public string answerValue="0";
	public OTSprite mySprite;
	public Transform expectedDropPoint;

	public bool isDraggable=true;
	public bool isCollidable=false;
	public bool canInput=false;
	public bool inOutOfView=false;
	public bool onOffEvents=false;

	AnswerManager answerMan;

	// Use this for initialization
	void Awake () {
		answerMan=GameObject.Find("Main Camera").GetComponent<AnswerManager>();
		mySprite=gameObject.GetComponent<OTSprite>();

		if(canInput)
		{
			mySprite.registerInput=true;

			mySprite.onReceiveDrop=ReceiveDrop;
			mySprite.onInput=OnInput;
		}
		if(isDraggable)
		{
			mySprite.draggable=true;

			mySprite.onDragStart=DragStart;
			mySprite.onDragging=Dragging;
			mySprite.onDragEnd=DragEnd;			
		}
		if(inOutOfView)
		{
			mySprite.onIntoView=IntoView;
			mySprite.onOutOfView=OutOfView;
		}
		if(isCollidable)
		{
			mySprite.collidable=true;

			mySprite.onCollision=OnCollision;
			mySprite.onEnter=OnEnter;
			mySprite.onExit=OnExit;
			mySprite.onStay=OnStay;
		}
		if(onOffEvents)
		{
			mySprite.onCreateObject=CreateObject;
			mySprite.onDestroyObject=DestroyObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// when the object is created
	void CreateObject (OTObject owner)
	{

	}

	void DestroyObject (OTObject owner)
	{

	}

	// when the object comes into view
	void IntoView (OTObject owner)
	{

	}

	// when the object drag starts
	void DragStart(OTObject owner)
	{

	}

	// whilst dragging
	void Dragging(OTObject owner)
	{

	}

	// drag ends
	void DragEnd(OTObject owner)
	{
		if(isPotentialAnswer){
			if(owner.dropTarget==null)
			{
				return;
			}
			else if(owner.dropTarget.transform == expectedDropPoint)
			{
				Debug.Log("dropped in right place brah");
				GameObject.Destroy(gameObject);

				if(correctDropCreatesNew)
				{
					answerMan.CreateNewObject();
				}
			}
			else
			{
				Debug.Log("incorrect drop brah, praise christ");
			}
		}
	}

	void OutOfView(OTObject owner)
	{

	}

	// whilst colliding
	void OnCollision(OTObject owner)
	{
		Debug.Log("collide!");
	}

	// when collision starts
	void OnEnter(OTObject owner)
	{

	}

	// when collision ends
	void OnExit(OTObject owner)
	{

	}

	// if it stays in collision zone
	void OnStay(OTObject owner)
	{
	
	}


	// if object dropped into me
	void OnInput(OTObject owner)
	{

	}

	void ReceiveDrop(OTObject owner)
	{
		OTSprite dropSprite=owner.dropTarget.gameObject.GetComponent<OTSprite>();
	}
}

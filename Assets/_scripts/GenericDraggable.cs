using UnityEngine;
using System.Collections;

public class GenericDraggable : MonoBehaviour {

	public bool isPotentialAnswer=false;
	public OTSprite mySprite;


	// Use this for initialization
	void Awake () {
		mySprite=gameObject.GetComponent<OTSprite>();
		mySprite.onReceiveDrop=ReceiveDrop;
		mySprite.onInput=OnInput;
		mySprite.onIntoView=IntoView;
		mySprite.onCreateObject=CreateObject;
		mySprite.onDragStart=DragStart;
		mySprite.onDragging=Dragging;
		mySprite.onDragEnd=DragEnd;
		mySprite.onCollision=OnCollision;
		mySprite.onOutOfView=OutOfView;
		mySprite.onEnter=OnEnter;
		mySprite.onExit=OnExit;
		mySprite.onStay=OnStay;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// when the object is created
	void CreateObject (OTObject owner)
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

	}

	void OutOfView(OTObject owner)
	{

	}

	// whilst colliding
	void OnCollision(OTObject owner)
	{

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

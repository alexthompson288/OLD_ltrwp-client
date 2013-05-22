using UnityEngine;
using System.Collections;

public class MissingWordSentenceDropzone : MonoBehaviour {
	OTSprite mySprite;
	// Use this for initialization
	void Start () {
		mySprite=gameObject.GetComponent<OTSprite>();
		mySprite.onReceiveDrop=ReceiveDrop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ReceiveDrop(OTObject owner)
	{
		OTSprite dropSprite=owner.dropTarget.gameObject.GetComponent<OTSprite>();
		DragDropAnswer answerPrefs=owner.dropTarget.gameObject.GetComponent<DragDropAnswer>();
		dropSprite.position=mySprite.position;

		Debug.Log("Received drop - "+owner.dropTarget.gameObject.name+" - is Correct? "+answerPrefs.isCorrect);
	}
}

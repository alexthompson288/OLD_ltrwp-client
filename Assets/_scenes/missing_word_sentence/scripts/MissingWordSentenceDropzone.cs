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

		if(answerPrefs.isCorrect)
		{
			dropSprite.transform.parent=mySprite.transform.parent;
			dropSprite.position=mySprite.position;
			MissingWordSentenceManager mgr=GameObject.Find("Main Camera").GetComponent<MissingWordSentenceManager>();
			mgr.FixFrame();
		}
		else 
		{
			OTTween mt=new OTTween(dropSprite,0.5f, OTEasing.BounceInOut);
				mt.Tween("position", new Vector2(transform.position.x,transform.position.y-300.0f));
		}
	}
}

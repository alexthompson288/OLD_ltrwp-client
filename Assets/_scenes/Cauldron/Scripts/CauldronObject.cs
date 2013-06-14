using UnityEngine;
using System.Collections;

public class CauldronObject : MonoBehaviour {

	public string myWord="";

	// Use this for initialization
	void Start () {
		foreach(Transform t in transform)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				OTTextSprite txt=t.GetComponent<OTTextSprite>();
				txt.text=myWord;
				txt.ForceUpdate();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

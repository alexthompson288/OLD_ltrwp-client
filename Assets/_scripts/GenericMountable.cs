using UnityEngine;
using System.Collections;

public class GenericMountable : MonoBehaviour {

	public GenericContainer MyContainer;
	GenericMountable Myself;
	OTSprite MySprite;
	AlphabetBookManager gameManager;
	
	// Use this for initialization
	void Start () {
		Myself=gameObject.GetComponent<GenericMountable>();
		MySprite=gameObject.GetComponent<OTSprite>();
		gameManager=GameObject.Find ("Main Camera").GetComponent<AlphabetBookManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool MountToContainer(GenericContainer thisContainer) {
		
		if(thisContainer.CanBeMounted(Myself, gameManager.lastTouchPos)){
			
			if(MyContainer!=null && MyContainer!=thisContainer)
				UnmountFromContainer(MyContainer);
			
			thisContainer.SetMountedObject(Myself);
			MyContainer=thisContainer;
			MySprite.position=thisContainer.MyPosition;
			return true;
		}
		
		return false;
	}
	
	public void UnmountFromContainer(GenericContainer thisContainer){
		if(thisContainer==MyContainer)
			thisContainer.UnsetMountedObject(Myself);
	}
}

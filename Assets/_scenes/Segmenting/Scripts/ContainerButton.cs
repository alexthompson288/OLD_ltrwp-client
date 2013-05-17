using UnityEngine;
using System.Collections;

public class ContainerButton : MonoBehaviour {
	public Material OnMaterial;
	public Material MountMaterial;
	public SegManager gameManager;
	public ContainerButton LastContainer;
	bool pressed;
	
	// Use this for initialization
	void Start () {
		gameManager=(SegManager)GameObject.Find("Main Camera").GetComponent(typeof(SegManager));	
		transform.parent.GetComponent<Container>().HideMe();
		
		if(LastContainer==null)
			renderer.enabled=true;
		else
			renderer.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(LastContainer==null)
			renderer.enabled=true;
		else if(LastContainer!=null && LastContainer.pressed)
			renderer.enabled=true;
		else
			renderer.enabled=false;
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	private void On_SimpleTap(Gesture gesture){
		if (gesture.pickObject == gameObject && !pressed){
			
			if(LastContainer!=null && !LastContainer.pressed)return;
			audio.Play ();
			gameObject.renderer.material=OnMaterial;
			gameManager.pressContainerButton();
			pressed=true;
			transform.parent.GetComponent<Container>().ShowMe();
		}
		
	}
}

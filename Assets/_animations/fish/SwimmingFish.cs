using UnityEngine;
using System.Collections;

public class SwimmingFish : MonoBehaviour {
	
	GameObject Me;
	Manager gameManager;
	
	// Use this for initialization
	void Start () {
		Me=gameObject;
		gameManager=(Manager)GameObject.Find("Main Camera").GetComponent(typeof(Manager));
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=new Vector3(transform.position.x+0.01f, transform.position.y, transform.position.z);
		
		if(Me.transform.position.x>9.99f)
		{
			gameManager.CreateFish();
			GameObject.Destroy(Me);
		}
	}
}

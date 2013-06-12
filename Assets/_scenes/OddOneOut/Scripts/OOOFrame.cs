using UnityEngine;
using System.Collections;
using AlTypes;

public class OOOFrame : MonoBehaviour {

	OddOneOutManager gameManager;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		gameManager=GameObject.Find("Main Camera").GetComponent<OddOneOutManager>();
		
		DataWordData dw=gameManager.ReturnCurrentWordData();
		Debug.Log("this frame to use dw word "+dw.Word+" is dummy? "+dw.IsDummyWord);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

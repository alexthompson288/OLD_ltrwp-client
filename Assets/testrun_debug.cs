using UnityEngine;
using System.Collections;

public class testrun_debug : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("running");
	}
	
	// Update is called once per frame
	void Update () {

		int numUsers = User.AllUsers.Count;
		Debug.Log("user count: " + numUsers.ToString());
	
	}
}

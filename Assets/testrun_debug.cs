using UnityEngine;
using System.Collections;

public class testrun_debug : MonoBehaviour {

	public OTSprite thisSprite;

	// Use this for initialization
	void Start () {
		Debug.Log("running");
		thisSprite.image=(Texture2D)Resources.Load("Images/mnemonics_images_png_250/a_angry_ant");
	}
	
	// Update is called once per frame
	void Update () {

		int numUsers = User.AllUsers.Count;
		Debug.Log("user count: " + numUsers.ToString());
	
	}
}

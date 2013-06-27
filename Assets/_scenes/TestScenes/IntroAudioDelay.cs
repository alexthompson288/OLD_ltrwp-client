using UnityEngine;
using System.Collections;

public class IntroAudioDelay : MonoBehaviour {

	public float audioDelay=3.0f;

	// Use this for initialization
	void Start () {
		Invoke("StartAudio", audioDelay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartAudio() {
		GetComponent<AudioSource>().Play();
	}
}

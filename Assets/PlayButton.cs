using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	
	private PipPad pipPad;
	public GameObject UpButton;
	public GameObject DownButton;
	private float UpTimer = 0.0f;

	// Use this for initialization
	void Start () {
		pipPad = GameObject.Find("PipPad").GetComponent<PipPad>();
	}
	
	// Update is called once per frame
	void Update () {
		UpTimer -= Time.deltaTime;
		if(UpTimer < 0.0f)
		{
			DownButton.SetActive(false);
			UpButton.SetActive(true);
		}
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

	void On_SimpleTap(Gesture gesture) {
		if(gesture.pickObject==gameObject)
		{
			DownButton.SetActive(true);
			UpButton.SetActive(false);
			audio.clip = (AudioClip)Resources.Load("audio/words/"+pipPad.Word);
			if(audio.clip == null)
			{
				// bounce up no sound pip	
			}
			audio.Play();
			UpTimer = 1.2f;
		}
	}
}

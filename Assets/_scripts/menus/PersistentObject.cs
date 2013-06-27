using UnityEngine;
using System.Collections;

public class PersistentObject : MonoBehaviour {
	
	public string ContentBrowserName="ContentBrowser-Scrolling";
	public string CurrentTheme="Forest";
	public string LastScene="ContentBrowser-Scrolling";
	public string CurrentScene="ContentBrowser-Scrolling";
	public string WordBankWord="non";
	public string NextLevel;
	public string CurrentLetter="a";
	public int Players=1;
	public bool KeywordGame=false;
	
	// Use this for initialization
	void Start () {
		
		if(gameObject.audio==null){
			gameObject.AddComponent<AudioSource>();
			gameObject.audio.playOnAwake=false;
		}
		GameObject.DontDestroyOnLoad(gameObject);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlayAudioClip(AudioClip thisClip){
		audio.clip=thisClip;
		audio.Play ();
	}
}

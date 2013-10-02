using UnityEngine;
using System.Collections;

public class CorrectPathAnswers : MonoBehaviour {
	
	public OTSprite Answer1;
	public OTSprite Answer2;
	
	public CorrectPathManager manager;

	// Use this for initialization
	void Start () {
		manager = Camera.main.GetComponent<CorrectPathManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void LoadAnswers(string Word1, string Word2)
	{
		Answer1.image=(Texture2D)Resources.Load("Images/word_images_png_350/_"+Word1+"_350");
		Answer2.image=(Texture2D)Resources.Load("Images/word_images_png_350/_"+Word2+"_350");
	}
}

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PictureGame : MonoBehaviour {
	
	public Transform Bars;	
	public Transform ButtonPrefab;
	public string CurrentWord = "cat";
	private List<string> PictureNames = new List<string>();
	public List<Transform> Buttons = new List<Transform>();
	public PictureFrame pictureFrame;

	// Use this for initialization
	void Start ()
	{		
		pictureFrame = GameObject.Find("PictureFrame").GetComponent<PictureFrame>();
		//Texture2D image= (Texture2D)Resources.Load("Images/word_images_png_350/_"+w+"_350");

/*		DirectoryInfo dir = new DirectoryInfo(Application.datapath + "/Assets/Resources/Images/word_images_png_350");
		Debug.Log("Directory path: " + Application.datapath + "/Assets/Resources/Images/word_images_png_350");
		FileInfo[] info = dir.GetFiles("*.*");
		foreach (FileInfo f in info) 
		{
			Debug.Log(f.Name);
		}*/
		
		PictureNames.Add ("arm");
		PictureNames.Add ("bath");
		PictureNames.Add ("boot");
		PictureNames.Add ("cap");
		PictureNames.Add ("car");
		PictureNames.Add ("bird");
		PictureNames.Add ("bell");
		PictureNames.Add ("cake");
		PictureNames.Add ("bus");
		PictureNames.Add ("bunny");
		PictureNames.Add ("dog");
		PictureNames.Add ("fish");
			
		StartRound();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartRound()
	{
		if(Buttons.Count > 0)
		{
			for (int i = 0; i < Buttons.Count; i++)
			{
				Destroy( Buttons[i].gameObject );
			}
			Buttons.Clear();
		}
		
		CurrentWord = PictureNames[ Random.Range(0, PictureNames.Count) ];
		
		int curX=0;
		int curY=0;
		float startX=-250.0f;
		float StartY=-150.0f;
		
		float incX=	400.0f;
		float incY=150.0f;
		
		bool HaveWeMadeCorrectWord = false;
		
		for(int i=0;i <  4;i++)
		{			
			Transform t=(Transform)Instantiate(ButtonPrefab);
			PictureWordGameTouch pwgt=t.GetComponent<PictureWordGameTouch>();
			pwgt.MyWord=PictureNames[ Random.Range(0, PictureNames.Count) ];
			pwgt.TextPrefab.GetComponent<OTTextSprite>().text=pwgt.MyWord;
			pwgt.TextPrefab.GetComponent<OTTextSprite>().ForceUpdate();
			
			if(pwgt.MyWord == CurrentWord)
				HaveWeMadeCorrectWord = true;
			
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(startX+(curX*incX),StartY-(curY*incY));
			
			curX++;
			
			if(curX>1)
			{
				curX=0;
				curY++;
			}
			Buttons.Add(t);
		}	
		
		if(!HaveWeMadeCorrectWord)
		{
			int index = Random.Range(0, Buttons.Count);
			Buttons[index].GetComponent<PictureWordGameTouch>().MyWord=CurrentWord;
			Buttons[index].GetChild(0).GetComponent<OTTextSprite>().text=CurrentWord;
			Buttons[index].GetChild(0).GetComponent<OTTextSprite>().ForceUpdate();
		}	
		
		pictureFrame.ShowPicture(CurrentWord);
		
	}
}

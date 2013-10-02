using UnityEngine;
using System.Collections;
using System.IO;

public class CollectionRoom : MonoBehaviour {
	
	public GameObject WebcamTexture;
	public OTSprite SceneBG;
	
	public Texture2D BG1;
	public Texture2D BG2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

	public void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject != null)
		{
			if(gesture.pickObject.name == "BGThumb1")
			{
				SceneBG.image = BG1;
				WebcamTexture.transform.position = new Vector3(62.16077f, -28.84f, 17.5f);
				WebcamTexture.transform.localScale = new Vector3(13.5f,13.5f,13.5f);
				
			}
			
			if(gesture.pickObject.name == "BGThumb2")
			{
				SceneBG.image = BG2;
				WebcamTexture.transform.position = new Vector3(18.1f, 217.0f, 17.5f);
				WebcamTexture.transform.localScale = new Vector3(19.5f,19.5f,19.5f);
			}
			
			if(gesture.pickObject.name == "CameraButton")
			{
				StartCoroutine(TakeSnapshot());								
			}
		}
	}
	
	IEnumerator TakeSnapshot()
	{
		yield return new WaitForEndOfFrame();
		
		var startX = 0;
		var startY = 146;
		
		var width = 820;
		var height = 768 - startY;		

		var tex = new Texture2D (width, height, TextureFormat.RGB24, false);
		tex.ReadPixels (new Rect(startX, startY, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		var bytes = tex.EncodeToPNG();
		Destroy(tex);
		//File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
		File.WriteAllBytes("/Users/douglasallen/SavedScreen.png", bytes);
	}
}

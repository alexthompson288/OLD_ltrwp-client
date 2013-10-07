using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class StickerScrollWheel : MonoBehaviour {
	
	public int NumberOfVisibleStickers = 10;
	private List<OTSprite> StickerSprites = new List<OTSprite>();
	private List<string> StickerImageFiles = new List<string>();
	private List<Vector2> ListPositions = new List<Vector2>();
	private List<OTSprite> StickerBGs = new List<OTSprite>();
	
	private int ListIndex = 0;
	private float ScrollVelocity = 0.0f;
	public float SpaceBetweenStickers = 220.0f;
	private float startingPosition = -600.0f;
	public float XPos = 390.0f;
	public float StickerSize = 256.0f;
	private float ListTopLimit;
	public Transform StarBurst;
	private PersistentObject PersistentManager;
	public Transform StickerBG;
	
	// Use this for initialization
	void Start () {
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}	
		
		// debug stickers
#if UNITY_EDITOR
		PlayerPrefs.SetInt("UnlockedSticker" + "a", 1);
		PlayerPrefs.SetInt("UnlockedSticker" + "g", 1);
		PlayerPrefs.SetInt("UnlockedSticker" + "t", 1);
		PlayerPrefs.SetInt("UnlockedSticker" + "qu", 1);
#endif
		
		PlayerPrefs.SetInt("UnlockedSticker" + PersistentManager.CurrentLetter, 1);
		
		ListTopLimit = startingPosition + (NumberOfVisibleStickers * SpaceBetweenStickers);	
		
		PopulateAlphabet();
		
		for(int i = 0; i < NumberOfVisibleStickers; i++)
		{
			GameObject g = new GameObject("Sticker_" + i.ToString());
			StickerScript ss = g.AddComponent<StickerScript>();
			ss.ListIndex = i;
			OTSprite s = g.AddComponent<OTSprite>();
			
			// load mneumonic image for sticker
			PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(StickerImageFiles[i]);		
			string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+pd.Mneumonic;
			filePathMnemonic = filePathMnemonic.Replace(" ", "_");
			Debug.Log("file path"+filePathMnemonic);
			s.image=(Texture2D)Resources.Load(filePathMnemonic);
			
		//	s.image = (Texture2D)Resources.Load("Images/word_images_png_350/_" + StickerImageFiles[i] + "_350");	
			
			// fade out if not unlocked yet
			if(PlayerPrefs.GetInt("UnlockedSticker"+StickerImageFiles[ss.ListIndex], 0)==0)
				s.tintColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
			else
				s.draggable = true;	
			
			s.position = new Vector2(XPos, startingPosition + (i * SpaceBetweenStickers));
			StickerSprites.Add(s);
			ListPositions.Add(s.position);
			s.size = new Vector2(StickerSize, StickerSize);
			s.ForceUpdate();	
			s.depth = -5;
			
			// generate the backgrounds
			StickerBGs.Add ( ((Transform)Instantiate(StickerBG)).gameObject.GetComponent<OTSprite>() );
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		for(int i = 0; i < ListPositions.Count; i++)
		{
			ListPositions[i] += new Vector2(0, ScrollVelocity * Time.deltaTime);
			if(!StickerSprites[i].GetComponent<StickerScript>().IsGettingDragged)
				StickerSprites[i].position = ListPositions[i];
			
			StickerBGs[i].position = ListPositions[i];
			
			if(ScrollVelocity > 0 && ListPositions[i].y > ListTopLimit)
			{
				// load new sticker at the bottom
				Vector2 PreviousPosition = ListPositions[i];
				int PreviousIndex = StickerSprites[i].GetComponent<StickerScript>().ListIndex;
				Destroy(StickerSprites[i].gameObject);
				StickerSprites.RemoveAt(i);
				ListPositions.RemoveAt(i);
				
				GameObject g = new GameObject("Sticker_" + (PreviousIndex - NumberOfVisibleStickers).ToString());
				StickerScript ss = g.AddComponent<StickerScript>();
				ss.ListIndex = PreviousIndex - NumberOfVisibleStickers;
				
				if(ss.ListIndex < 0)
					ss.ListIndex += StickerImageFiles.Count;
				
				OTSprite s = g.AddComponent<OTSprite>();
				
				
				// load mneumonic image for sticker
				PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(StickerImageFiles[ss.ListIndex]);		
				string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+pd.Mneumonic;
				filePathMnemonic = filePathMnemonic.Replace(" ", "_");
				Debug.Log("file path"+filePathMnemonic);
				s.image=(Texture2D)Resources.Load(filePathMnemonic);
				
				// fade out if not unlocked yet
				if(PlayerPrefs.GetInt("UnlockedSticker"+StickerImageFiles[ss.ListIndex], 0)==0)
					s.tintColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
				else
					s.draggable = true;
				
				s.position = PreviousPosition - new Vector2(0.0f, (NumberOfVisibleStickers * SpaceBetweenStickers));
				StickerSprites.Add(s);
				ListPositions.Add(s.position);
				s.size = new Vector2(StickerSize, StickerSize);
				s.ForceUpdate();				
				s.depth = -5;
				
			}else if(ScrollVelocity < 0 && ListPositions[i].y < startingPosition)
			{
				// load new sticker at the top
				Vector2 PreviousPosition = ListPositions[i];
				int PreviousIndex = StickerSprites[i].GetComponent<StickerScript>().ListIndex;
				Destroy(StickerSprites[i].gameObject);
				StickerSprites.RemoveAt(i);
				ListPositions.RemoveAt(i);
				
				GameObject g = new GameObject("Sticker_" + (PreviousIndex + NumberOfVisibleStickers).ToString());
				StickerScript ss = g.AddComponent<StickerScript>();
				ss.ListIndex = PreviousIndex + NumberOfVisibleStickers;
				
				if(ss.ListIndex >= StickerImageFiles.Count)
					ss.ListIndex -= StickerImageFiles.Count;
				
				OTSprite s = g.AddComponent<OTSprite>();
				
				// load mneumonic image for sticker
				PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(StickerImageFiles[ss.ListIndex]);		
				string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+pd.Mneumonic;
				filePathMnemonic = filePathMnemonic.Replace(" ", "_");
				Debug.Log("file path"+filePathMnemonic);
				s.image=(Texture2D)Resources.Load(filePathMnemonic);
						
				// fade out if not unlocked yet
				if(PlayerPrefs.GetInt("UnlockedSticker"+StickerImageFiles[ss.ListIndex], 0)==0)
					s.tintColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
				else
					s.draggable = true;
				
				s.position = PreviousPosition + new Vector2(0.0f, (NumberOfVisibleStickers * SpaceBetweenStickers));
				StickerSprites.Add(s);
				ListPositions.Add(s.position);
				s.size = new Vector2(StickerSize, StickerSize);
				s.ForceUpdate();
				s.depth = -5;
			}
		}
		
		ScrollVelocity *= 1.0f - Time.deltaTime;
	}
	
	public void ReplaceSprite(int index)
	{
		int PreviousListIndex = 0;
		// find previous sprite in List
		for(int i = 0; i < ListPositions.Count; i++)
		{
			if(StickerSprites[i].gameObject.GetComponent<StickerScript>().ListIndex == index)
				PreviousListIndex = i;
		}
		
		GameObject g = new GameObject("Sticker_" + index.ToString());
		StickerScript ss = g.AddComponent<StickerScript>();
		ss.ListIndex = index;		
		OTSprite s = g.AddComponent<OTSprite>();
		
		// load mneumonic image for sticker
		PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(StickerImageFiles[ss.ListIndex]);		
		string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+pd.Mneumonic;
		filePathMnemonic = filePathMnemonic.Replace(" ", "_");
		Debug.Log("file path"+filePathMnemonic);
		s.image=(Texture2D)Resources.Load(filePathMnemonic);
		
	//	s.image = (Texture2D)Resources.Load("Images/word_images_png_350/_" + StickerImageFiles[ss.ListIndex] + "_350");	
		s.position = ListPositions[PreviousListIndex];
		StickerSprites.RemoveAt(PreviousListIndex);
		StickerSprites.Insert(PreviousListIndex,s);
		s.size = new Vector2(00, 00);
		s.ForceUpdate();
		s.draggable = true;		
		s.depth = -5;
		
		Vector2 newScale=new Vector2(StickerSize, StickerSize);		
		
		var config=new GoTweenConfig()
			.vector2Prop( "size", newScale )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(s, 1.28f, config);
		Go.addTween(tween);
		
		Instantiate(StarBurst, new Vector3(s.position.x , s.position.y, -50.0f), Quaternion.identity);
		
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
			if(gesture.pickObject.name == "UpArrow")
			{
				ScrollVelocity = 350.0f;
			}
			
			if(gesture.pickObject.name == "DownArrow")
			{
				ScrollVelocity = -350.0f;
			}
		}
	}
	
	void PopulateAlphabet()
	{
		// this should be populated from db
		StickerImageFiles.Add ("a");
		StickerImageFiles.Add ("b");
		StickerImageFiles.Add ("c");
		StickerImageFiles.Add ("d");
		StickerImageFiles.Add ("e");
		StickerImageFiles.Add ("f");
		StickerImageFiles.Add ("g");
		StickerImageFiles.Add ("h");
		StickerImageFiles.Add ("i");
		StickerImageFiles.Add ("j");
		StickerImageFiles.Add ("k");
		StickerImageFiles.Add ("l");
		StickerImageFiles.Add ("m");
		StickerImageFiles.Add ("n");
		StickerImageFiles.Add ("o");
		StickerImageFiles.Add ("p");
		StickerImageFiles.Add ("qu");
		StickerImageFiles.Add ("r");
		StickerImageFiles.Add ("s");
		StickerImageFiles.Add ("t");
		StickerImageFiles.Add ("u");
		StickerImageFiles.Add ("v");
		StickerImageFiles.Add ("w");
		StickerImageFiles.Add ("x");
		StickerImageFiles.Add ("y");
		StickerImageFiles.Add ("z");
	}
	
}

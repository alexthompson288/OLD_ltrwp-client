using UnityEngine;
using System.Collections;

public class SplatSceneManager : MonoBehaviour {
	
	public string name="Splat";
	public string setting="Mushroom";
	SplatManager gameManager;
	
	public Texture2D[] BGAssets;
	public OTSpriteAtlasCocos2D[] CAssets;

	public Texture2D[] BGForest;
	public Texture2D[] BGFarm;
	public Texture2D[] BGCastle;
	public Texture2D[] BGSchool;
	public Texture2D[] BGSpace;
	
	public OTSpriteAtlasCocos2D CForest;
	public OTSpriteAtlasCocos2D CFarm;
	public OTSpriteAtlasCocos2D CCastle;
	public OTSpriteAtlasCocos2D CSchool;
	public OTSpriteAtlasCocos2D CSpace;
	
	public OTSpriteAtlasCocos2D ContainerSprites;
	
	PersistentObject persistentManager;
	
	// Use this for initialization
	void Start () {

		string GameSetting=GameManager.Instance.SessionMgr.CurrentGameSetting;

		Debug.Log("GameSetting: '"+GameSetting+"'");

		if(GameSetting==null)
			GameSetting="school_splat";
		
		Debug.Log("GameSetting: '"+GameSetting+"'");

		gameManager=GameObject.Find ("Main Camera").GetComponent<SplatManager>();
		
		gameManager.CreateNewPersistentObject();
		
		persistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		
		Texture2D[] Backgrounds=BGForest;
		bool HasClouds=false;
		
		ContainerSprites=CForest;

		if(GameSetting=="forest_splat"){
			Backgrounds=BGForest;
			ContainerSprites=CForest;
			HasClouds=true;
		}
		else if(GameSetting=="farm_splat"){
			Backgrounds=BGFarm;
			ContainerSprites=CFarm;
			HasClouds=true;
		}
		else if(GameSetting=="castle_splat"){
			Backgrounds=BGCastle;
			ContainerSprites=CCastle;
			HasClouds=true;
		}
		else if(GameSetting=="school_splat"){
			Backgrounds=BGSchool;
			ContainerSprites=CSchool;
			HasClouds=false;
		}
		else if(GameSetting=="alien_splat"){
			HasClouds=false;
			Backgrounds=BGSpace;
			ContainerSprites=CSpace;
		}
		
		int currentDepth=5;
		int currentBGIndex=0;

		for (int i=0;i<Backgrounds.Length;i++)
		{
			Texture2D t=Backgrounds[i];
			Debug.Log("This texture name: "+t.name+" / looking for "+GameSetting+"_bg_"+currentBGIndex);
			if(t.name==GameSetting+"_bg_"+currentBGIndex)
			{
				OTSprite sprite=OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
				sprite.size=new Vector2(1024,768);
				sprite.image=t;
				sprite.depth=currentDepth;
				sprite.transparent=true;

				currentDepth+=10;	
				currentBGIndex++;
				continue;
			}
		}

		foreach(OTSpriteAtlasCocos2D a in CAssets)
		{
			if(a.name==GameSetting+"_containers")
			{
				ContainerSprites=a;
				break;
			}
		}

		// for(int i=0;i<Backgrounds.Length;i++)
		// {
		// 	OTSprite sprite=OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
		// 	sprite.size=new Vector2(1024,768);
		// 	sprite.image=Backgrounds[i];
		// 	sprite.depth=currentDepth;
		// 	sprite.transparent=true;

		// 	currentDepth+=10;			
		// }
		
//		Debug.Log ("cSprite name "+ContainerSprites.name);
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class SplatSceneManager : MonoBehaviour {
	
	public string name="Forest";
	public string setting="Mushroom";
	SplatManager gameManager;
	
	public OTSpriteAtlasCocos2D[] CAssets;
	
	public OTSpriteAtlasCocos2D ContainerSprites;
	
	PersistentObject persistentManager;
	
	// Use this for initialization
	void Start () {

		string GameSetting=GameManager.Instance.SessionMgr.CurrentGameSetting;

		Debug.Log("GameSetting: '"+GameSetting+"'");

		if(GameSetting==null)
			GameSetting="underwater_splat_falling";
		
		Debug.Log("GameSetting: '"+GameSetting+"'");

		gameManager=GameObject.Find ("Main Camera").GetComponent<SplatManager>();
		
		gameManager.CreateNewPersistentObject();
		
		persistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		
		bool HasClouds=false;
		
		int currentDepth=5;
		int currentBGIndex=0;

		for(int i=0;i<100;i++)
		{
				// layer-0 is background
				Debug.Log("This texture looking for Images/scene_assets/"+GameSetting+"/"+GameSetting+"_bg_"+currentBGIndex);
				Texture2D t=(Texture2D)Resources.Load("Images/scene_assets/"+GameSetting+"/"+GameSetting+"_bg_"+currentBGIndex);
				if(t==null)break;
				OTSprite sprite=OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
				sprite.size=new Vector2(1024,768);
				sprite.image=t;
				sprite.depth=currentDepth;
				sprite.transparent=true;

				sprite.transform.parent=GameObject.Find("layer-"+i).transform;

				currentDepth+=10;	
				currentBGIndex++;
				continue;
		}

		foreach(OTSpriteAtlasCocos2D a in CAssets)
		{
			if(a.name==GameSetting+"_containers")
			{
				ContainerSprites=a;
				break;
			}
		}

		foreach(OTAtlasData ad in ContainerSprites.atlasData)
		{
			if(ad.name.EndsWith("_a"))
				gameManager.NumberOfContainerVariants++;
			else if(ad.name.EndsWith("_b"))
				gameManager.ContainerHasStateB=true;

		}
		Debug.Log("variants: "+gameManager.NumberOfContainerVariants+" // state Bs? "+gameManager.ContainerHasStateB);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

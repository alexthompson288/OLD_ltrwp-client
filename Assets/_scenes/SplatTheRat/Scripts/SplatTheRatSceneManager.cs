using UnityEngine;
using System.Collections;

public class SplatTheRatSceneManager : MonoBehaviour {

	SplatTheRatManager gameManager;
	PersistentObject persistentManager;
	// Use this for initialization
	void Start () {

		string GameSetting=GameManager.Instance.SessionMgr.CurrentGameSetting;

		Debug.Log("GameSetting: '"+GameSetting+"'");

		if(GameSetting==null)
			GameSetting="splat_the_rat_forest";
		
		Debug.Log("GameSetting: '"+GameSetting+"'");

		gameManager=GameObject.Find ("Main Camera").GetComponent<SplatTheRatManager>();
		
		gameManager.CreateNewPersistentObject();
		
		persistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		
		bool HasClouds=false;
		
		int currentDepth=5;
		int currentBGIndex=0;
		int currentPXLayer=3;

		for(int i=0;i<100;i++)
		{
				// layer-0 is background
				Debug.Log("This texture looking for Images/scene_assets/"+GameSetting+"/"+GameSetting+"_bg_"+currentBGIndex);
				Texture2D t=(Texture2D)Resources.Load("Images/scene_assets/"+GameSetting+"/"+GameSetting+"_bg_"+currentBGIndex);
				if(t==null)break;
				OTSprite sprite=OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
				sprite.size=new Vector2(t.width,t.height);
				sprite.image=t;
				sprite.depth=currentDepth;
				sprite.transparent=true;

				sprite.transform.parent=GameObject.Find("layer-"+currentPXLayer).transform;

				currentDepth+=10;	
				currentBGIndex++;

				if(currentPXLayer>0)
					currentPXLayer--;

				continue;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

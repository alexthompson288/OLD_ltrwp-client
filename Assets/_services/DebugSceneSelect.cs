using UnityEngine;
using System.Collections;

public class DebugSceneSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CreateNewPersistentObject();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateNewPersistentObject()
	{
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			thisPO.GetComponent<PersistentObject>().CurrentTheme="Forest";
		}
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(10,10,1004,748));
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 12;
		style.normal.textColor = Color.grey;

		int sessionID=0;
		
		if (GUI.Button(new Rect(125,0,120,25), "alien_splat"))
		{
			sessionID=9100;
		}
		if (GUI.Button(new Rect(250,0,120,25), "alien_splat_falling"))
		{
			sessionID=9101;
		}
		if (GUI.Button(new Rect(375,0,120,25), "alien_splat_meteors"))
		{
			sessionID=9102;
		}
		if (GUI.Button(new Rect(125,30,120,25), "castle_splat"))
		{
			sessionID=9103;
		}
		if (GUI.Button(new Rect(250,30,120,25), "castle_splat_falling"))
		{
			sessionID=9104;
		}
		if (GUI.Button(new Rect(375,30,120,25), "farm_splat"))
		{
			sessionID=9105;
		}
		if (GUI.Button(new Rect(125,60,120,25), "farm_splat_falling"))
		{
			sessionID=9106;
		}
		if (GUI.Button(new Rect(250,60,120,25), "farm_splat_hay"))
		{
			sessionID=9107;
		}
		if (GUI.Button(new Rect(375,60,120,25), "forest_splat"))
		{
			sessionID=9108;
		}
		if (GUI.Button(new Rect(125,90,120,25), "forest_splat_falling"))
		{
			sessionID=9109;
		}
		if (GUI.Button(new Rect(250,90,120,25), "forest_splat_fircones"))
		{
			sessionID=9110;
		}
		if (GUI.Button(new Rect(375,90,120,25), "forest_splat_fireflys"))
		{
			sessionID=9111;
		}
		if (GUI.Button(new Rect(125,120,120,25), "forest_splat_nuts"))
		{
			sessionID=9112;
		}
		if (GUI.Button(new Rect(250,120,120,25), "school_splat"))
		{
			sessionID=9113;
		}
		if (GUI.Button(new Rect(375,120,120,25), "school_splat_falling"))
		{
			sessionID=9114;
		}
		if (GUI.Button(new Rect(125,150,120,25), "school_splat_notebook"))
		{
			sessionID=9115;
		}
		if (GUI.Button(new Rect(250,150,120,25), "underwater_splat"))
		{
			sessionID=9116;
		}
		if (GUI.Button(new Rect(375,150,120,25), "underwater_splat_falling"))
		{
			sessionID=9117;
		}
		if (GUI.Button(new Rect(125,190,120,25), "splat_the_rat_alien"))
		{
			sessionID=9118;
		}
		if (GUI.Button(new Rect(250,190,120,25), "splat_the_rat_castle"))
		{
			sessionID=9119;
		}
		if (GUI.Button(new Rect(375,190,120,25), "splat_the_rat_farm"))
		{
			sessionID=9120;
		}
		if (GUI.Button(new Rect(125,220,120,25), "splat_the_rat_forest"))
		{
			sessionID=9121;
		}
		if (GUI.Button(new Rect(250,220,120,25), "splat_the_rat_school"))
		{
			sessionID=9122;
		}
		if (GUI.Button(new Rect(375,220,120,25), "splat_the_rat_underwater"))
		{
			sessionID=9123;
		}
		
		StartIt(sessionID);

		GUI.EndGroup();
	}

	void StartIt(int id)
	{
		if(id==0)return;

		GameManager.Instance.SessionMgr.StartSession(id);
		GameManager.Instance.SessionMgr.LogSections();

		GameManager.Instance.SessionMgr.StartActivity();
	}
}

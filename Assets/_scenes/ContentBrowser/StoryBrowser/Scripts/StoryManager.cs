using UnityEngine;
using System.Collections;
using AlTypes;

public class StoryManager : MonoBehaviour {

	PersistentObject PersistentManager;
	int CurrentPage=0;
	public OTSprite PageSprite;
	public AudioClip EndClip;
	public Transform TheEnd;
	bool shownTheEnd=false;
	public OTTextSprite txtTL;
	public OTTextSprite txtTC;
	public OTTextSprite txtTR;

	void Awake () {
		CreateNewPersistentObject();
	}


	// Use this for initialization
	void Start () {
		txtTL.visible=false;
		txtTC.visible=false;
		txtTR.visible=false;
		NextPage();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void NextPage () {
		txtTL.visible=false;
		txtTC.visible=false;
		txtTR.visible=false;
		Texture2D nPage=(Texture2D)Resources.Load("Images/stories/"+PersistentManager.StoryID+"/"+CurrentPage);
		if(nPage==null && !shownTheEnd)
			ShowTheEnd();
		else if(nPage==null && shownTheEnd)
			Application.LoadLevel("StoryBrowser");
		else
			PageSprite.image=nPage;

		if(PersistentManager.StoryID<16 && !shownTheEnd){
			int modPageIndex=CurrentPage+1;
			StoryPageData thisPage=GameManager.Instance.GetStoryPageFor(PersistentManager.StoryID, modPageIndex);
			GetPageText(thisPage);
		}
		CurrentPage++;
	}

	void GetPageText(StoryPageData thisPage)
	{
		string anchor=thisPage.AnchorPoint;

		if(anchor=="topleft"){
			txtTL.text=thisPage.PageText;
			txtTL.visible=true;
		}
		else if(anchor=="topcenter"){
			txtTC.text=thisPage.PageText;
			txtTC.visible=true;
		}
		else if(anchor=="topright"){
			txtTR.text=thisPage.PageText;
			txtTR.visible=true;
		}

		if(thisPage.AudioName!="")
		{
			AudioClip ac=(AudioClip)Resources.Load("audio/stories/"+PersistentManager.StoryID+"/"+thisPage.AudioName);
			audio.clip=ac;
			audio.Play();
		}
	}

	public void CreateNewPersistentObject()
	{
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else 
		{
			PersistentManager=GameObject.Find("PersistentManager").GetComponent<PersistentObject>();
		}
	}

	void ShowTheEnd()
	{
		shownTheEnd=true;
		TheEnd.gameObject.SetActive(true);
		audio.clip=EndClip;
		audio.Play();
	}

	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;
	}

	void On_SimpleTap(Gesture gesture)
	{
		NextPage();
	}

	void On_SwipeEnd(Gesture gesture)
	{
		if(gesture.swipe == EasyTouch.SwipeType.Left)
		{
			NextPage();
		}
	}
}

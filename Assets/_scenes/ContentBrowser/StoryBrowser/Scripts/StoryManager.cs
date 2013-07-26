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
	bool pageHasAudio;
	public OTTextSprite txtTL;
	public OTTextSprite txtTC;
	public OTTextSprite txtTR;
	public OTSprite btnPlay;
	public Texture2D btnPlayPress;
	public Texture2D btnPlayUnpress;

	void Awake () {
		CreateNewPersistentObject();
	}


	// Use this for initialization
	void Start () {
		Debug.Log("in story");
		txtTL.visible=false;
		txtTC.visible=false;
		txtTR.visible=false;
		NextPage();
	}
	
	// Update is called once per frame
	void Update () {
		if(pageHasAudio){
			if(audio.isPlaying && btnPlay.image==btnPlayUnpress)
			{
				btnPlay.image=btnPlayPress;
			}
			else if(!audio.isPlaying && btnPlay.image==btnPlayPress)
			{
				btnPlay.image=btnPlayUnpress;
			}
		}
	}

	void NextPage () {
		txtTL.visible=false;
		txtTC.visible=false;
		txtTR.visible=false;
		btnPlay.visible=false;
		pageHasAudio=false;
		int modPageIndex=CurrentPage+1;
		StoryPageData thisPage=GameManager.Instance.GetStoryPageFor(PersistentManager.StoryID, modPageIndex);
		Debug.Log("thisPage Image Images/storypages/"+thisPage.ImageName);
		Texture2D nPage=(Texture2D)Resources.Load("Images/storypages/"+thisPage.ImageName);
		if(nPage==null && !shownTheEnd)
			ShowTheEnd();
		else if(nPage==null && shownTheEnd)
			Application.LoadLevel("StoryBrowser");
		else
			PageSprite.image=nPage;

		if(PersistentManager.StoryID<16 && !shownTheEnd){
			GetPageText(thisPage);
		}
		CurrentPage++;
	}

	void GetPageText(StoryPageData thisPage)
	{
		if(thisPage.PageText=="null")return;

		string anchor=thisPage.AnchorPoint;
		string pageText=thisPage.PageText.Replace("\\n", "\n");


		if(anchor=="topleft"){
			txtTL.text=pageText;
			txtTL.visible=true;
		}
		else if(anchor=="topcenter"){
			txtTC.text=pageText;
			txtTC.visible=true;
		}
		else if(anchor=="topright"){
			txtTR.text=pageText;
			txtTR.visible=true;
		}

		if(thisPage.AudioName!="")
		{
			btnPlay.visible=true;
			pageHasAudio=true;
			AudioClip ac=(AudioClip)Resources.Load("audio/stories/"+thisPage.AudioName);
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
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.name=="btnPlay" && !audio.isPlaying)
				audio.Play();
			else 
				NextPage();
		}
	}

	void On_SwipeEnd(Gesture gesture)
	{
		if(gesture.swipe == EasyTouch.SwipeType.Left)
		{
			NextPage();
		}
	}
}

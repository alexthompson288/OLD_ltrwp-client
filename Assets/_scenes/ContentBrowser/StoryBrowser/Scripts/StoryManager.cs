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
	public WordColliderGenerator WordGen;

	void Awake () {
		CreateNewPersistentObject();
		WordGen = new WordColliderGenerator();
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
		{
		//	Application.LoadLevel("StoryBrowser");
			GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>().ChangeLevel("StoryBrowser");
		}else
			PageSprite.image=nPage;

		if( !shownTheEnd ){//&& PersistentManager.StoryID<16){
			GetPageText(thisPage);
		}
		
		CurrentPage++;
	}

	void GetPageText(StoryPageData thisPage)
	{
		if(thisPage.PageText=="null")return;

		string anchor=thisPage.AnchorPoint;
		string pageText=thisPage.PageText.Replace("\\n", "\n");			
		
		bool isFirstPage = false;
		if(CurrentPage < 1)
			isFirstPage = true;
			
		if(anchor=="topleft"){
			txtTL.visible=true;
			txtTL.text=pageText;
			StartCoroutine( WordGen.BreakUpSentenceIntoWords(txtTL,isFirstPage));
		}
		else if(anchor=="topcenter"){
			txtTC.visible=true;
			txtTC.text=pageText;
			StartCoroutine( WordGen.BreakUpSentenceIntoWords(txtTC,isFirstPage) );
		}
		else if(anchor=="topright"){
			txtTR.visible=true;
			txtTR.text=pageText;			
			StartCoroutine(WordGen.BreakUpSentenceIntoWords(txtTR,isFirstPage));
		}

		if(thisPage.AudioName!="")
		{
			btnPlay.visible=true;
			pageHasAudio=true;
			AudioClip ac=(AudioClip)Resources.Load("audio/stories/"+thisPage.AudioName);
			audio.clip=ac;
		//	audio.Play();
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
		if(gesture.pickObject!=null && (gesture.pickObject.name.Contains("sPage") || gesture.pickObject.name.Contains("btnPlay")) )//!gesture.pickObject.name.Contains("Word") && !gesture.pickObject.name.Contains("PipPad") && !gesture.pickObject.name.Contains("btnPlay") )
		{
			if(gesture.pickObject.name=="btnPlay" && !audio.isPlaying)
			{
				audio.Play();
			}else{ 
				NextPage();
			}
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

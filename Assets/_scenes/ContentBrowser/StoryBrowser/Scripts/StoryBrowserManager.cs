using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;

public class StoryBrowserManager : MonoBehaviour {
	//public OTSprite[] ActiveBooks;
	public AudioClip BookTap;
	public int currentIndex=1;
	public int NumberOfStories = 5;
	public float BookYPosition = 0.0f;
	public float BookXSpacing = 100.0f;
	public GameObject BookParent;
	bool isBooksMoving = false;
	public Transform PadlockPrefab;
	
	public List<GameObject> ActiveBooks = new List<GameObject>();

	PersistentObject PersistentManager;

	// Use this for initialization

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

	void Awake() {
		CreateNewPersistentObject();
		BookParent = GameObject.Find("BookParent");
	}

	void Start () {
		if(PersistentManager.LastStoryBrowserPageIndex>0)currentIndex=PersistentManager.LastStoryBrowserPageIndex;
			SetupBooks();
	}
	
	void SetupBooks()
	{
		PersistentManager.LastStoryBrowserPageIndex=currentIndex;
		
		int LoadingIndex = currentIndex - 3;		
		
		for(int i = 0; i < 7; i++)
		{			
			if(LoadingIndex < 0 )
				LoadingIndex += NumberOfStories;	
			if(LoadingIndex > NumberOfStories )
				LoadingIndex -= NumberOfStories;	
			
			GameObject g = new GameObject("StoryBook" + LoadingIndex.ToString());
			StoryBrowserBook thisBook = g.AddComponent<StoryBrowserBook>();
			g.AddComponent<MeshFilter>();
			g.AddComponent<MeshRenderer>();
			g.AddComponent<BoxCollider>();
			OTSprite s = g.AddComponent<OTSprite>();
			
			Texture2D newImage=(Texture2D)Resources.Load("Images/story_covers/book_cover_"+LoadingIndex.ToString());
			thisBook.bookId=LoadingIndex;
			
			if(newImage==null){
				s.visible=false;
				s.collider.enabled=false;
			}
			else 
			{
				s.visible=true;
				s.collider.enabled=true;
				s.image=newImage;
			}
			s.position = new Vector2(BookXSpacing * (i- 3) + 10.0f,  BookYPosition);
			s.collidable = true;
				s.ForceUpdate();
			g.transform.parent = BookParent.transform;
			LoadingIndex++;
			ActiveBooks.Add(g);
			
		}
		

	}

	void ShowPrevBooks()
	{
		if(isBooksMoving)
			return;
		isBooksMoving = true;
		iTween.MoveTo(BookParent, iTween.Hash("position", BookParent.transform.position + new Vector3( BookXSpacing, 0.0f, 0.0f),"time", 0.5f, "oncompletetarget", gameObject, "oncomplete", "finishedBookScroll"));
		currentIndex--;
		
		GameObject tmp = ActiveBooks[ActiveBooks.Count - 1];
		ActiveBooks.RemoveAt(ActiveBooks.Count - 1);
		Destroy(tmp);
		
		int LoadingIndex = currentIndex - 3;
		if(LoadingIndex < 0 )
				LoadingIndex += NumberOfStories;	
			if(LoadingIndex > NumberOfStories )
				LoadingIndex -= NumberOfStories;	
		
		GameObject g = new GameObject("StoryBook" + LoadingIndex.ToString());
		StoryBrowserBook thisBook = g.AddComponent<StoryBrowserBook>();
		g.AddComponent<MeshFilter>();
		g.AddComponent<MeshRenderer>();
		g.AddComponent<BoxCollider>();
		OTSprite s = g.AddComponent<OTSprite>();
		
		Texture2D newImage=(Texture2D)Resources.Load("Images/story_covers/book_cover_"+ LoadingIndex.ToString());
		thisBook.bookId=LoadingIndex;
		
		if(newImage==null){
			s.visible=false;
			s.collider.enabled=false;
		}
		else 
		{
			s.visible=true;
			s.collider.enabled=true;
			s.image=newImage;
		}
		s.collidable = true;
		s.position = new Vector2((BookXSpacing * - 4 )+ 10.0f,  BookYPosition);
		s.ForceUpdate();
		g.transform.parent = BookParent.transform;
		LoadingIndex++;
		ActiveBooks.Insert(0,g);
		
		//Debug.Log ("Current book is:" + currentIndex.ToString());
	}

	void ShowNextBooks()
	{
		if(isBooksMoving)
			return;
		
		isBooksMoving = true;
		iTween.MoveTo(BookParent, iTween.Hash("position", BookParent.transform.position - new Vector3( BookXSpacing, 0.0f, 0.0f),"time", 0.5f, "oncompletetarget", gameObject, "oncomplete", "finishedBookScroll"));
		currentIndex++;
			
		GameObject tmp = ActiveBooks[0];
		ActiveBooks.RemoveAt(0);
		Destroy(tmp);
		
		int LoadingIndex = currentIndex + 3;
		if(LoadingIndex < 0 )
				LoadingIndex += NumberOfStories;	
			if(LoadingIndex > NumberOfStories )
				LoadingIndex -= NumberOfStories;	
		
		GameObject g = new GameObject("StoryBook" + LoadingIndex.ToString());
		StoryBrowserBook thisBook = g.AddComponent<StoryBrowserBook>();
		g.AddComponent<MeshFilter>();
		g.AddComponent<MeshRenderer>();
		g.AddComponent<BoxCollider>();
		OTSprite s = g.AddComponent<OTSprite>();
		
		Texture2D newImage=(Texture2D)Resources.Load("Images/story_covers/book_cover_"+ LoadingIndex.ToString());
		thisBook.bookId=LoadingIndex;		
		
		if(newImage==null){
			s.visible=false;
			s.collider.enabled=false;
		}
		else 
		{
			s.visible=true;
			s.collider.enabled=true;
			s.image=newImage;
		}
		s.collidable = true;
		s.position = new Vector2((BookXSpacing * 4 ) + 10.0f,  BookYPosition);
		g.transform.parent = BookParent.transform;
		LoadingIndex++;
			s.ForceUpdate();
		// randomly lock some books
		if(Random.Range(0,10) < 2)
		{
			s.tintColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
			thisBook.isLocked = true;
			Transform pt = (Transform)Instantiate( PadlockPrefab );//, g.transform.position , Quaternion.identity);
			pt.parent = g.transform;
			pt.localPosition = new Vector3(0,0,0);
			pt.GetComponent<OTSprite>().ForceUpdate();
		}
				
		ActiveBooks.Add(g);
	}
	
	void finishedBookScroll()
	{
		isBooksMoving = false;	
	}

	// Update is called once per frame
	void Update () {
	
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
			if(gesture.pickObject.name=="BuyButton")
			{
				Debug.Log ("Trying to buy book " + currentIndex.ToString());
				Transform padlock = ActiveBooks[6].transform.GetChild(0);
				if(padlock != null)
				{
					Destroy(padlock.gameObject);
					ActiveBooks[6].GetComponent<OTSprite>().tintColor = Color.white;
				}
			}
			if(gesture.pickObject.name=="btnPrev")
			{
				ShowPrevBooks();
			}
			else if(gesture.pickObject.name=="btnNext")
			{
				ShowNextBooks();
			}else if(gesture.pickObject.name=="StoryWordBank")
			{
				PersistentManager.StoryID=ActiveBooks[6].GetComponent<StoryBrowserBook>().bookId;
				GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>().ChangeLevel("StoryWordBank");	
			}
			else if(gesture.pickObject.name.StartsWith("StoryBook"))
			{
				StoryBrowserBook thisBook=gesture.pickObject.GetComponent<StoryBrowserBook>();

				audio.clip=BookTap;
				audio.Play();

				PersistentManager.StoryID=thisBook.bookId;
				StoryPageData thisPage=GameManager.Instance.GetStoryPageFor(thisBook.bookId, 1);
				Debug.Log("picked story "+thisBook.bookId+"// thisPage Image Images/storypages/"+thisPage.ImageName);

				if(Resources.Load("Images/storypages/"+thisPage.ImageName))
				{
					GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>().ChangeLevel("Stories");
					//Application.LoadLevel("Stories");
				}
				else
					Debug.Log("do not have pages for story");
			}
		}

		else
		{
			if(gesture.position.x<50)
			{
				ShowPrevBooks();
			}
			else if(gesture.position.x>(1024-50))
			{
				ShowNextBooks();
			}
		}

	}

	void On_SwipeEnd(Gesture gesture)
	{
		if(gesture.swipe == EasyTouch.SwipeType.Left)
		{
			ShowNextBooks();
		}
		else if(gesture.swipe == EasyTouch.SwipeType.Right)
		{
			ShowPrevBooks();
		}
	}
}

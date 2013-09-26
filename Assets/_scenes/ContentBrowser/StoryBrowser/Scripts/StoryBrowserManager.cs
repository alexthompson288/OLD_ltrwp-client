using UnityEngine;
using System.Collections;
using AlTypes;

public class StoryBrowserManager : MonoBehaviour {
	public OTSprite[] ActiveBooks;
	public AudioClip BookTap;
	public int currentIndex=0;

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
	}

	void Start () {
		if(PersistentManager.LastStoryBrowserPageIndex>0)currentIndex=PersistentManager.LastStoryBrowserPageIndex;
			SetupBooks();
	}
	
	void SetupBooks()
	{
		PersistentManager.LastStoryBrowserPageIndex=currentIndex;

		foreach(OTSprite s in ActiveBooks)
		{
			int imageIndex=currentIndex+1;
			Texture2D newImage=(Texture2D)Resources.Load("Images/story_covers/book_cover_"+imageIndex);
			StoryBrowserBook thisBook=s.GetComponent<StoryBrowserBook>();
			thisBook.bookId=imageIndex;

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

			currentIndex++;
		}
	}

	void ShowPrevBooks()
	{
		currentIndex-=6;
		
		if(currentIndex>=0)
			SetupBooks();
		else 
			currentIndex=0;
	}

	void ShowNextBooks()
	{
		if(currentIndex<50)
			SetupBooks();
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
			if(gesture.pickObject.name=="btnPrev")
			{
				ShowPrevBooks();
			}
			else if(gesture.pickObject.name=="btnNext")
			{
				ShowNextBooks();
			}
			else if(gesture.pickObject.name.StartsWith("book-"))
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

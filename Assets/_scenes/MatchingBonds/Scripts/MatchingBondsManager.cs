using UnityEngine;
using System.Collections;

public class MatchingBondsManager : MonoBehaviour {
	public Transform MatcherPrefab;
	public string[] LettersToUse;
	Transform LetterHolder;
	int xTouchOffset=512;
	int yTouchOffset=384;
	int correctMatchesRequired=8;
	int correctMatchesFound=0;
	int p2CorrectMatchesFound=0;
	Vector2 touchStartPos;
	ArrayList DrawnDots;
	public Transform DotSprite;
	PersistentObject PersistentManager;
	bool exitCountdown;
	float countdownToExit=4.0f;
	
	Transform LastTouchObject;
	// Use this for initialization
	void Start () {
		StartGame();
		DrawnDots=new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		
		if((correctMatchesFound==correctMatchesRequired||p2CorrectMatchesFound==correctMatchesRequired)&&!exitCountdown)
			exitCountdown=true;
		
		if(exitCountdown)
			countdownToExit-=Time.deltaTime;

		if(countdownToExit<0){
			PersistentManager.Players=1;
			Application.LoadLevel(PersistentManager.ContentBrowserName);
		}
	}
	
	void StartGame(){
		ReadPersistentObjectSettings();
		LetterHolder=GameObject.Find ("letters").GetComponent<Transform>();
		
		for(int i=0;i<3;i++)
		{
			CreateNewPair();
		}
	}
	
	void ReadPersistentObjectSettings(){
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		
		PersistentManager.Players=2;
		
		if(PersistentManager.CurrentLetter==null)
			PersistentManager.CurrentLetter="a";
	}	
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;	
		EasyTouch.On_TouchDown -= On_TouchUp;
	}

	void On_TouchDown(Gesture gesture){
		Vector2 touchPos=new Vector2(gesture.position.x-xTouchOffset,gesture.position.y-yTouchOffset);
		float distanceDrawn=0.0f;
		int dotSpacing=20;
		int dotsRequired=0;
		
		
		if(touchStartPos==Vector2.zero)
			touchStartPos=gesture.position;
		
		
		distanceDrawn=(touchStartPos-gesture.position).magnitude;
		dotsRequired=(int)distanceDrawn/dotSpacing;
		
		Debug.Log ("dots to draw: "+dotsRequired);
		
		if(dotsRequired>0){
			Transform newDot=(Transform)Instantiate(DotSprite);
			newDot.GetComponent<OTSprite>().position=touchPos;
			DrawnDots.Add (newDot);
			touchStartPos=Vector2.zero;
		}
		
		if(gesture.pickObject!=null)
		{
			LastTouchObject=gesture.pickObject.transform;
//			if(gesture.pickObject.GetComponent<OTSprite>())
//			{
//				OTSprite s=gesture.pickObject.GetComponent<OTSprite>();
//				s.position=new Vector2(gesture.position.x-xTouchOffset,gesture.position.y-yTouchOffset);
//			}
		}
	}
	
	void On_TouchUp(Gesture gesture){
		
		Vector2 touchPos=new Vector2(gesture.position.x-xTouchOffset,gesture.position.y-yTouchOffset);
		if(LastTouchObject!=null)
		{
			foreach(Transform t in LetterHolder)
			{
				Transform tTransform=t.GetComponent<Transform>();
				Transform poTransform=LastTouchObject.GetComponent<Transform>();
				
				if(poTransform==t)
					continue;
				
				MatchingBondsObject tMBO=tTransform.GetComponent<MatchingBondsObject>();
				MatchingBondsObject poMBO=gesture.pickObject.GetComponent<MatchingBondsObject>();
				
				OTSprite s=t.GetComponent<OTSprite>();
				Rect sRect=new Rect(s.position.x-(s.size.x/2), s.position.y-(s.size.y/2), s.size.x, s.size.y);

				if((tMBO.playerID==poMBO.playerID)
					&& sRect.Contains(touchPos)
					&&(tMBO.MyLetter.ToUpper()==poMBO.MyLetter.ToUpper() && tMBO.MyLetter!=poMBO.MyLetter)){
					
					
					if(tMBO.playerID==1)
						correctMatchesFound++;
					else if(tMBO.playerID==2)
						p2CorrectMatchesFound++;
					
					DestroyBothObjects(tTransform,poTransform);
					
					CreateNewPair();
					Debug.Log ("got match. source letter: "+poMBO.MyLetter+" dest letter: "+tMBO.MyLetter+" p1 score "+correctMatchesFound+" p2 score "+p2CorrectMatchesFound);
					
					break;
				}

			}
		
		}
		
		foreach(Transform t in DrawnDots)
		{
			GameObject g=t.gameObject;
			GameObject.Destroy(g);
		}
		DrawnDots.Clear();
		touchStartPos=Vector2.zero;
		LastTouchObject=null;
	}
	
	
	void DestroyBothObjects(Transform t1, Transform t2)
	{
		
		OTSprite s1=t1.GetComponent<OTSprite>();
		OTSprite s2=t2.GetComponent<OTSprite>();
		
		int halfwayDot=DrawnDots.Count/2;
		Transform hWd=(Transform)DrawnDots[halfwayDot];
		OTSprite dSprite=hWd.GetComponent<OTSprite>();
		
		Vector2 MoveToPoint=hWd.position;
		
		 
		
		foreach(Transform t in t1)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				OTTextSprite txt=t.GetComponent<OTTextSprite>();
				OTTween fot=new OTTween(txt, 1.0f, OTEasing.Linear);
				fot.Tween ("alpha",0.3f);
			}
		}
		foreach(Transform t in t2)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				OTTextSprite txt=t.GetComponent<OTTextSprite>();
				OTTween fot=new OTTween(txt, 1.0f, OTEasing.Linear);
				fot.Tween ("alpha",0.3f);
			}
		}
		
		OTTween np1=new OTTween(s1,0.8f, OTEasing.ElasticIn);
		OTTween np2=new OTTween(s2, 0.8f, OTEasing.ElasticIn);
		np1.Tween("position", MoveToPoint);	
		np2.Tween("position", MoveToPoint);
		
		OTTween mt=new OTTween(s1,1.3f, OTEasing.ElasticIn);
		OTTween fo=new OTTween(s2, 1.3f, OTEasing.ElasticIn);
		mt.Tween("size", new Vector2(0,0));	
		fo.Tween("size", new Vector2(0,0));
	}
	
	void CreateNewPair()
	{
		int playerID=1;
		if(PersistentManager.Players==2)
			playerID=Random.Range (1,3);
	
		Debug.Log ("playerID for this pair "+playerID);
		
		int rndLetter=Random.Range(0,LettersToUse.Length);
		Transform ThisMatch=(Transform)Instantiate(MatcherPrefab);
		ThisMatch.parent=LetterHolder;
		MatchingBondsObject mBO=ThisMatch.GetComponent<MatchingBondsObject>();
		OTSprite thisSprite=ThisMatch.GetComponent<OTSprite>();
		mBO.MyLetter=LettersToUse[rndLetter];
		mBO.playerID=playerID;
		thisSprite.position=new Vector2(Random.Range (-400.0f,400.0f), Random.Range (-250.0f,250.0f));
		
		
		
		Transform ThatMatch=(Transform)Instantiate(MatcherPrefab);
		ThatMatch.parent=LetterHolder;
		MatchingBondsObject tmBO=ThatMatch.GetComponent<MatchingBondsObject>();
		OTSprite thatSprite=ThatMatch.GetComponent<OTSprite>();
		tmBO.MyLetter=LettersToUse[rndLetter].ToUpper();
		tmBO.playerID=playerID;
		thatSprite.position=new Vector2(Random.Range (-400.0f,400.0f), Random.Range (-250.0f,250.0f));
	}
}

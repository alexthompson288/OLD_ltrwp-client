using UnityEngine;
using System.Collections;

public class LFManager : MonoBehaviour {
	
	public Texture2D FairyUp;
	public Texture2D FairyDown;
	OTSprite fairy;
	
	public string SceneLetter;
	public OTTextSprite SceneTextSprite;

	public OTSpline[] LetterSplines;

	float timeToAutomate=0.0f;
	bool hasAutomated=false;
	bool hasToldToFollow=false;
	
	bool isAutomating;
	float automatingSpeed=0.05f;
	
	float startPercOnSpline=1.0f;
	
	
	public OTSpline currentLetter;
	OTSpline startLetter;
	LFLetterPart letterPart;
	
	float percOnSpline=0.0f;
	
	bool onTrack=false;
	
	
	// Use this for initialization
	void Start () {
		fairy=GameObject.Find ("mvFairy").GetComponent<OTSprite>();
		
		foreach(OTSpline s in LetterSplines)
		{
			if(s.name==SceneLetter)
			{
				currentLetter=s;
			}
		}

		startLetter=currentLetter;
		letterPart=currentLetter.GetComponent<LFLetterPart>();

		SceneTextSprite.text=SceneLetter;

		if(SceneLetter=="a")
		{
			SceneTextSprite.position=new Vector2(4, 146);
			SceneTextSprite.size=new Vector2(10,10);
		}
		else if(SceneLetter=="b")
		{
			SceneTextSprite.position=new Vector2(-65, 14);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="c")
		{
			SceneTextSprite.position=new Vector2(-23, 100);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="d")
		{
			SceneTextSprite.position=new Vector2(-32, 9);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="e")
		{
			SceneTextSprite.position=new Vector2(-40, 150);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="f")
		{
			SceneTextSprite.position=new Vector2(-23, 74);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="g")
		{
			SceneTextSprite.position=new Vector2(-36, 181);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="h")
		{
			SceneTextSprite.position=new Vector2(-53, 23);
			SceneTextSprite.size=new Vector2(8,8);	
		}
		else if(SceneLetter=="i")
		{
			SceneTextSprite.position=new Vector2(-37, 14);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="j")
		{
			SceneTextSprite.position=new Vector2(75, 44);
			SceneTextSprite.size=new Vector2(6,6);	
		}
		else if(SceneLetter=="k")
		{
			SceneTextSprite.position=new Vector2(-19, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="l")
		{
			SceneTextSprite.position=new Vector2(-34, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="m")
		{
			SceneTextSprite.position=new Vector2(-28, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="n")
		{
			SceneTextSprite.position=new Vector2(-36, 75);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="o")
		{
			SceneTextSprite.position=new Vector2(-65, 102);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="p")
		{
			SceneTextSprite.position=new Vector2(-46, 182);
			SceneTextSprite.size=new Vector2(8,8);	
		}
		else if(SceneLetter=="q")
		{
			SceneTextSprite.position=new Vector2(-23, 166);
			SceneTextSprite.size=new Vector2(8,8);	
		}
		else if(SceneLetter=="r")
		{
			SceneTextSprite.position=new Vector2(-45, 135);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="s")
		{
			SceneTextSprite.position=new Vector2(-23, 123);
			SceneTextSprite.size=new Vector2(12,12);	
		}
		else if(SceneLetter=="t")
		{
			SceneTextSprite.position=new Vector2(140, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="u")
		{
			SceneTextSprite.position=new Vector2(-65, 111);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="v")
		{
			SceneTextSprite.position=new Vector2(-34, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="w")
		{
			SceneTextSprite.position=new Vector2(-14, 100);
			SceneTextSprite.size=new Vector2(9,9);	
		}
		else if(SceneLetter=="x")
		{
			SceneTextSprite.position=new Vector2(-39, 44);
			SceneTextSprite.size=new Vector2(10,10);	
		}
		else if(SceneLetter=="y")
		{
			SceneTextSprite.position=new Vector2(-41, 183);
			SceneTextSprite.size=new Vector2(8,8);	
		}
		else if(SceneLetter=="z")
		{
			SceneTextSprite.position=new Vector2(-25, 136);
			SceneTextSprite.size=new Vector2(10,10);	
		}

		
		if(letterPart.ReverseSpline){
			fairy.position=currentLetter.GetPosition(1.0f);
			percOnSpline=1.0f;
		}
		else{
			fairy.position=currentLetter.GetPosition(0.0f);
		}
		
		startPercOnSpline=percOnSpline;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!hasAutomated||!hasToldToFollow)
			timeToAutomate-=Time.deltaTime;
		
		if(timeToAutomate<0)
		{
			fairy.GetComponent<TrailRenderer>().time=5.0f;
			bool finished=false;
			
			if(isAutomating)
			{
//				Debug.Log ("the thingy / isAutomating? "+isAutomating+" / hasAutomated? "+hasAutomated+" / hasToldToFollow? "+hasToldToFollow);
					if(letterPart.ReverseSpline){
						percOnSpline-=automatingSpeed;
						if(percOnSpline<0 && !letterPart.NextPart){
							percOnSpline=0;
							currentLetter=startLetter;
							letterPart=currentLetter.GetComponent<LFLetterPart>();
							fairy.GetComponent<TrailRenderer>().time=0;
							finished=true;
						}
						else if(percOnSpline<0 && letterPart.NextPart)
						{
							currentLetter=letterPart.NextPart;
							letterPart=letterPart.NextPart.GetComponent<LFLetterPart>();
							ResetLetterPartPerc();
							fairy.GetComponent<TrailRenderer>().time=0;
						}
					}
					else{ 
						percOnSpline+=automatingSpeed;
						if(percOnSpline>1 && !letterPart.NextPart)
						{
							currentLetter=startLetter;
							letterPart=currentLetter.GetComponent<LFLetterPart>();
							percOnSpline=1;
							fairy.GetComponent<TrailRenderer>().time=0;
							finished=true;
						}
						else if(percOnSpline>1 && letterPart.NextPart)
						{
							currentLetter=letterPart.NextPart;
							letterPart=letterPart.NextPart.GetComponent<LFLetterPart>();
							ResetLetterPartPerc();
							fairy.GetComponent<TrailRenderer>().time=0;
						}
					}
				fairy.position=currentLetter.GetPosition(percOnSpline);
				
				if(finished)
				{
					isAutomating=false;
					
					if(hasAutomated&&!hasToldToFollow)
					{
						hasToldToFollow=true;
					}
					else if(!hasAutomated&&!hasToldToFollow)
					{
						hasAutomated=true;
					}
					
					percOnSpline=startPercOnSpline;
					
					fairy.position=currentLetter.GetPosition(startPercOnSpline);
					return;
				}
				
			}
			
			if(!hasAutomated&&!hasToldToFollow)
			{
				isAutomating=true;
				automatingSpeed=0.02f;
			}
			else if(hasAutomated&&!hasToldToFollow)
			{
				//Debug.Log ("isAutomating? "+isAutomating+" / hasAutomated? "+hasAutomated+" / hasToldToFollow? "+hasToldToFollow);
				automatingSpeed=0.01f;
				isAutomating=true;
			}
		}
		
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_TouchDown += On_Drag;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_TouchDown -= On_Drag;
	}
	
	void On_Drag (Gesture gesture)
	{
		if(hasAutomated&&hasToldToFollow)
		{
			fairy.GetComponent<TrailRenderer>().time=5.0f;
		}

		Vector2 gesPos=new Vector2(gesture.position.x-512,gesture.position.y-384);
		
		if((gesPos-fairy.position).magnitude<80.0f)
		{
			onTrack=true;
			if(letterPart.ReverseSpline){
				percOnSpline-=0.04f;
				if(percOnSpline<0 && !letterPart.NextPart){percOnSpline=0;}
				else if(percOnSpline<0&&letterPart.NextPart){
					currentLetter=letterPart.NextPart;
					letterPart=letterPart.NextPart.GetComponent<LFLetterPart>();
					fairy.GetComponent<TrailRenderer>().time=0;
					ResetLetterPartPerc();
					fairy.GetComponent<TrailRenderer>().time=5;
				}
			}
			else{ 
				percOnSpline+=0.04f;
				if(percOnSpline>1 && !letterPart.NextPart){percOnSpline=1;}
				else if(percOnSpline>1 && letterPart.NextPart){
					currentLetter=letterPart.NextPart;
					letterPart=letterPart.NextPart.GetComponent<LFLetterPart>();
					fairy.GetComponent<TrailRenderer>().time=0;
					ResetLetterPartPerc();
					fairy.GetComponent<TrailRenderer>().time=5;
				}
			}
			fairy.position=currentLetter.GetPosition(percOnSpline);
			
//			s.position=gesture.position-new Vector2(512,340);
			fairy.image=FairyDown;
		}
		else{
			if(onTrack)
			{
				Debug.Log ("no longer on track");
				onTrack=false;
			}
			fairy.image=FairyUp;
		}
	}
	
	void On_TouchUp(Gesture gesture)
	{
		fairy.image=FairyUp;
		onTrack=false;
	}

	void ResetLetterPartPerc()
	{
		if(letterPart.ReverseSpline){
			percOnSpline=1;
		}
		else 
		{
			percOnSpline=0;
		}
	}
	
	private void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null){
			// nothing
		}
	}
}

using UnityEngine;
using System.Collections;

public class MoleTouch : MonoBehaviour {
	
	SplatTheRatManager gameManager;
	MoleAnimation myAnimation;
	public string myLetter;
	public float RemoveTime=4.0f;
	public float DestroyCountdown=0.0f;
	public bool ShouldRemove;
	bool Destroying;
	float posYOffset=200.0f;
	OTSprite s;
	Vector2 StartPosition;
	public int IndexNumber;
	public Transform MyParent;
	public bool bigSign;
	
	// Use this for initialization
	void Start () {
		if(gameObject.name.StartsWith("MolePx1"))
		{
			transform.parent=GameObject.Find("layer-1").GetComponent<Transform>();
		}
		else if(gameObject.name.StartsWith("MolePx2"))
		{
			transform.parent=GameObject.Find("layer-2").GetComponent<Transform>();
		}
		else if(gameObject.name.StartsWith("MolePx3"))
		{
			transform.parent=GameObject.Find("layer-3").GetComponent<Transform>();
		}
		
		gameManager=GameObject.Find ("Main Camera").GetComponent<SplatTheRatManager>();
		myAnimation=gameObject.GetComponent<MoleAnimation>();
		
		s=gameObject.GetComponent<OTSprite>();
		
		StartPosition=s.position;
		
		float timeToTweenUp=(RemoveTime/4)*3;
		
		Vector2 newPos=new Vector2(gameObject.transform.position.x,gameObject.transform.position.y+posYOffset);

		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.ExpoOut );

		GoTween tween=new GoTween(s, RemoveTime, config);
		Go.addTween(tween);		

		// OTTween mt=new OTTween(s,RemoveTime, OTEasing.ExpoOut);
		// mt.Tween("position", new Vector2(gameObject.transform.position.x,gameObject.transform.position.y+posYOffset));
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Destroying)
			DestroyCountdown-=Time.deltaTime;
		if(DestroyCountdown<0){
			gameManager.RemoveActiveMole(IndexNumber);
			GameObject.Destroy(gameObject);
		}
		
		RemoveTime-=Time.deltaTime;
		if(RemoveTime<0&&!Destroying){
			HideAndDestroy();
		}

	}
	
	void HideAndDestroy()
	{
		if(Destroying)return;
		Destroying=true;
		DestroyCountdown=0.6f;
		
		// OTTween mt=new OTTween(s,0.5f, OTEasing.ExpoIn);
		// mt.Tween("position", StartPosition);

		var config=new GoTweenConfig()
			.vector2Prop( "position", StartPosition )
			.setEaseType( GoEaseType.ExpoIn );

		GoTween tween=new GoTween(s, 0.5f, config);
		Go.addTween(tween);		
		
	}
	
	void UserRemoveCorrect(){
		Destroying=true;
		DestroyCountdown=1.0f;
		foreach(Transform t in transform)
		{
			if(t.GetComponent<OTTextSprite>())
			{
				OTTextSprite txt=t.GetComponent<OTTextSprite>();

				var config=new GoTweenConfig()
					.floatProp( "alpha", 0.3f )
					.setEaseType( GoEaseType.Linear );

				GoTween tween=new GoTween(txt, 1.0f, config);
				Go.addTween(tween);		

				// OTTween fot=new OTTween(txt, 1.0f, OTEasing.Linear);
				// fot.Tween ("alpha",0.3f);
			}
		}
		var configfo=new GoTweenConfig()
			.floatProp( "alpha", 0.5f )
			.setEaseType( GoEaseType.Linear );

		GoTween fo=new GoTween(s, 0.5f, configfo);
		Go.addTween(fo);		

		var configmt=new GoTweenConfig()
			.vector2Prop( "size", new Vector2(0, 0) )
			.setEaseType( GoEaseType.ElasticIn );

		GoTween mt=new GoTween(s, 0.8f, configmt);
		Go.addTween(mt);		

		// OTTween mt=new OTTween(s,0.8f, OTEasing.ElasticIn);
		// OTTween fo=new OTTween(s, 0.5f, OTEasing.Linear);
		// mt.Tween("size", new Vector2(0,0));	
		// fo.Tween("alpha", 0.5f);
		
		myAnimation.SetAnimationSplat(true);
		audio.clip=gameManager.CorrectSound;
		gameManager.PlayVivCorrect();
		audio.Play ();
	}
	
	void UserRemoveIncorrect(){
//		gameManager.RemoveMoleFromTouch(gameObject.transform, "incorrect");

		Destroying=true;
		DestroyCountdown=0.3f;

		var config=new GoTweenConfig()
			.vector2Prop( "position", StartPosition )
			.setEaseType( GoEaseType.ExpoIn );

		GoTween tween=new GoTween(s, 0.3f, config);
		Go.addTween(tween);		

		// OTTween mt=new OTTween(s,0.3f, OTEasing.ExpoIn);
		// mt.Tween("position", StartPosition);
		audio.clip=gameManager.IncorrectSound;
		audio.Play ();
	}
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	// Simple tap
	private void On_SimpleTap(Gesture gesture){
		
		if(gesture.pickObject==gameObject){
			
			int Player=gameObject.GetComponent<MoleAnimation>().Player;
			
			if(Destroying)return;
			
			if(gameManager.CorrectLetterOnMole(myLetter, Player))
			{
				UserRemoveCorrect();
			}
			else 
			{
				UserRemoveIncorrect();
			}
			
		}

			
	}
}

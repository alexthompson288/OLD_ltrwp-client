using UnityEngine;
using System.Collections;

public class FruitMachineTouch : MonoBehaviour {

	FruitMachineManager fmMgr;

	void Awake() {
		fmMgr=GameObject.Find("Main Camera").GetComponent<FruitMachineManager>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=gameObject)return;
		if(gameObject.name=="button-red")
		{
			Debug.Log("Hit red button");
		}
		else if(gameObject.name=="button-green")
		{
			Debug.Log("Hit green button");
		}
		else if(gameObject.name=="lever")
		{
			Debug.Log("Hit lever");
			MoveLeverDown();
		}
		else if(gameObject.name=="btnPlayButton")
		{
			Debug.Log("play button");
			fmMgr.PlayCurrentWordAudio();
		}
	}

	void MoveLeverDown()
	{
		fmMgr.PlayLeverDownSound();

		OTSprite s=gameObject.GetComponent<OTSprite>();

		Vector2 newPos=new Vector2(s.position.x, 0.11f);

		if(Application.loadedLevelName=="FruitMachine-Zoomed")
			newPos=new Vector2(0.31f, -0.08f);
		
		var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.floatProp("rotation", -80.0f)
				.setEaseType( GoEaseType.QuadIn );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(s, 0.47f, config);
		tween.setOnCompleteHandler(c => MoveLeverUp());

		Go.addTween(tween);

	}

	void MoveLeverUp()
	{
		fmMgr.PlayLeverUpSound();
		OTSprite s=gameObject.GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(s.position.x, 0.1990806f);

		if(Application.loadedLevelName=="FruitMachine-Zoomed")
			newPos=new Vector2(0.35f, -0.03f);
		//0.1469531
		

		var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.floatProp("rotation", s.rotation+80.0f)
				.setEaseType( GoEaseType.QuadOut );

		GoTween tween=new GoTween(s, 0.365f, config);
		tween.setOnCompleteHandler(c => ReturnToMenu());

		Go.addTween(tween);

		// Go.to(s, 0.365f, config );
	}

	void ReturnToMenu() {
		Application.LoadLevel("ChallengeMenu");
	}
}

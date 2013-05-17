using UnityEngine;
using System.Collections;

public class FruitMachineTouch : MonoBehaviour {

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
	}

	void MoveLeverDown()
	{
		OTSprite s=gameObject.GetComponent<OTSprite>();

		Vector2 newPos=new Vector2(s.position.x, 0.11f);

		if(Application.loadedLevelName=="FruitMachine-Zoomed")
			newPos=new Vector2(s.position.x, 0.16f);

		var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.floatProp("rotation", -80.0f)
				.setEaseType( GoEaseType.QuadIn );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(s, 0.3f, config);
		tween.setOnCompleteHandler(c => MoveLeverUp());

		Go.addTween(tween);

	}

	void MoveLeverUp()
	{
		OTSprite s=gameObject.GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(s.position.x, 0.1469531f);

		if(Application.loadedLevelName=="FruitMachine-Zoomed")
			newPos=new Vector2(s.position.x, 0.1159259f);
		//0.1469531
		

		var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.floatProp("rotation", s.rotation+80.0f)
				.setEaseType( GoEaseType.QuadOut );

		
		Go.to(s, 0.3f, config );
	}
}

using UnityEngine;
using System.Collections;

public class FruitMachineManager : MonoBehaviour {

	public OTSprite panel;

	// Use this for initialization
	void Start () {
		// This is a zoomed FruitMachine
		if(panel!=null)
		{
			Vector2 newPos=new Vector2(-250.0f, 320.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(panel, 0.8f, config);
			tween.setOnCompleteHandler(c => SlideMachineUp());

			Go.addTween(tween);
		}
		// this is a non-zoomed fruitmachine
		else
		{

			// -205,113
			SegmentingManager segMan=gameObject.GetComponent<SegmentingManager>();
			segMan.scaffoldStartXPos=-240.0f;
			segMan.scaffoldStartYPos=80.0f;
			segMan.letterStartXPos=-250.0f;
			OTSprite SlotMac=GameObject.Find("FruitMachine").GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(-54.0f, -33.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(SlotMac, 0.8f, config);
			tween.setOnCompleteHandler(c => EnableScaffold());
			Go.addTween(tween);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SlideMachineUp()
	{
		OTSprite SlotMac=GameObject.Find("FruitMachine").GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(-35.0f, -74.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(SlotMac, 0.8f, config);
		tween.setOnCompleteHandler(c => EnableScaffold());
		Go.addTween(tween);
	}

	void EnableScaffold()
	{
		gameObject.GetComponent<SegmentingManager>().enabled=true;
	}
} 
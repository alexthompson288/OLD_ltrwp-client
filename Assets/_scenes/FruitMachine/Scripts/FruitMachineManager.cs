using UnityEngine;
using System.Collections;

public class FruitMachineManager : MonoBehaviour {

	public OTSprite panel;

	// Use this for initialization
	void Start () {
		if(panel!=null)
		{
			Vector2 newPos=new Vector2(-250.0f, 320.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

		
		// Go.to(s, 0.3f, config );
			GoTween tween=new GoTween(panel, 0.8f, config);
			tween.setOnCompleteHandler(c => SlideMachineUp());

			Go.addTween(tween);
		}
		else
		{

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
		Go.addTween(tween);
	}
} 
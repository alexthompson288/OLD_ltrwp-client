using UnityEngine;
using System.Collections;

public class SkillIntroductionFruitMachineDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		OTSprite SlotMac=GameObject.Find("FruitMachine").GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(13.0f, -7.8f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(SlotMac, 0.8f, config);
		Go.addTween(tween);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

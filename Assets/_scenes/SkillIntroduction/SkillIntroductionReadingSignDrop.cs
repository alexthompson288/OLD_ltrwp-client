using UnityEngine;
using System.Collections;

public class SkillIntroductionReadingSignDrop : MonoBehaviour {


	// Use this for initialization
	void Start () {
		OTSprite Sign=GameObject.Find("Sign").GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(90.0f, 182.0f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(Sign, 0.8f, config);
		Go.addTween(tween);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

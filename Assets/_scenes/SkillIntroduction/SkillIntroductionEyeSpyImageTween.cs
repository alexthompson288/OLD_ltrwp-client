using UnityEngine;
using System.Collections;

public class SkillIntroductionEyeSpyImageTween : MonoBehaviour {

	public bool hasTweened;
	public bool shakeMe;
	public bool punchMe;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!hasTweened&&punchMe)
			PunchObject();
		else if(!hasTweened&&shakeMe)
			ShakeObject();

	}

	void ShakeObject(){
		OTSprite mySprite=gameObject.GetComponent<OTSprite>();

		Vector2 StartPos=mySprite.position;
		
		var configr1=new GoTweenConfig()
			.vector2Prop( "position", new Vector2(mySprite.position.x+0.1f, mySprite.position.y) )
			.setEaseType( GoEaseType.ExpoOut );

		var configl1=new GoTweenConfig()
			.vector2Prop( "position", new Vector2(mySprite.position.x-0.2f, mySprite.position.y) )
			.setEaseType( GoEaseType.ExpoOut );

		var configr2=new GoTweenConfig()
			.vector2Prop( "position", new Vector2(mySprite.position.x+0.2f, mySprite.position.y) )
			.setEaseType( GoEaseType.ExpoOut );

		var configl2=new GoTweenConfig()
			.vector2Prop( "position", new Vector2(mySprite.position.x-0.2f, mySprite.position.y) )
			.setEaseType( GoEaseType.ExpoOut );

		var configr3=new GoTweenConfig()
			.vector2Prop( "position", StartPos )
			.setEaseType( GoEaseType.ExpoOut );

		GoTween tween1=new GoTween(mySprite, 0.1f, configr1);
		GoTween tween2=new GoTween(mySprite, 0.1f, configl1);
		GoTween tween3=new GoTween(mySprite, 0.1f, configr2);
		GoTween tween4=new GoTween(mySprite, 0.1f, configl2);
		GoTween tween5=new GoTween(mySprite, 0.1f, configr3);

		// Go.addTween(tween);

		var chain = new GoTweenChain();
		// chain.append(configr1).append(configl1);
		// chain.append(configl1).append(configr2);
		// chain.append(configr2).append(configl2);
		// chain.append(configl2).append(configr3);

		chain.append(tween1);
		chain.append(tween2);
		chain.append(tween3);
		chain.append(tween4);
		chain.append(tween5);

		chain.play();

		hasTweened=true;
	}

	void PunchObject(){
		OTSprite mySprite=gameObject.GetComponent<OTSprite>();
		Vector2 newScale=new Vector2(0.9f, 0.9f);
		var config=new GoTweenConfig()
			.vector2Prop( "size", newScale )
			.setEaseType( GoEaseType.Punch );

		GoTween tween=new GoTween(mySprite, 0.8f, config);
		Go.addTween(tween);

		hasTweened=true;
	}
}

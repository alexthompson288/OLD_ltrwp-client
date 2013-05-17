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
				.setEaseType( GoEaseType.BounceIn );

		
		// Go.to(s, 0.3f, config );
			GoTween tween=new GoTween(panel, 0.3f, config);
			// tween.setOnCompleteHandler(c => MoveLeverUp());

			Go.addTween(tween);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
} 
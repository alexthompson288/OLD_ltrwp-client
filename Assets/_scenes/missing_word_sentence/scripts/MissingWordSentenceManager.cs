using UnityEngine;
using System.Collections;

public class MissingWordSentenceManager : MonoBehaviour {

	public OTSprite Frame;
	public Texture2D FixedFrame;

	// Use this for initialization
	void Start () {
	
	}

	void Awake () {
		Frame=GameObject.Find("frame").GetComponent<OTSprite>();

		Vector2 newPos=new Vector2(0.0f, 320.0f);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(Frame, 0.8f, config);

		Go.addTween(tween);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FixFrame () {
		Frame.image=FixedFrame;
	}
}

using UnityEngine;
using System.Collections;

public class BigShield : MonoBehaviour {

	public string DisplayString;
	public string MyLetter;
	public Texture2D DisplayImage;
	public bool DoNotMove;

	bool IsShowing;
	float TimeToShowMeFor=3.0f;
	float TimeShownFor=0.0f;

	OTSprite Mnemonic;
	OTTextSprite ILetter;

	TrophyRoomManager gameManager;

	// Use this for initialization
	void Start () {

		gameManager=GameObject.Find("Main Camera").GetComponent<TrophyRoomManager>();

		OTSprite sprite;
		// To render an image in our scene, lets create the sprite object.
		sprite = OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
		Mnemonic=sprite;
		// Set the image to the gray star texture.

		sprite.image = DisplayImage;

		sprite.transform.parent=transform;

	 	sprite.size=new Vector2(0.35f,0.35f);

		sprite.position=new Vector2(0.0f, -0.1879085f);

		sprite.collidable=true;


		// Set the display depth all the way in front so it will overlap all others
		sprite.depth = -10;


		OTTextSprite letter;

		letter = OT.CreateObject(OTObjectType.TextSprite).GetComponent<OTTextSprite>();
		ILetter=letter;

		letter.transform.parent=transform;
		
		letter.text=MyLetter;

		letter.spriteContainer=gameManager.ShieldFont;

		letter.position=new Vector2(0.0f, -0.16f);

		letter.size=new Vector2(0.005f, 0.005f);

		letter.alpha=0.0f;


		OTTextSprite text;

		text = OT.CreateObject(OTObjectType.TextSprite).GetComponent<OTTextSprite>();

		text.transform.parent=transform;

		text.position=new Vector2(0, 0.08986928f);

		text.text=DisplayString;
			
		text.spriteContainer=gameManager.ShieldFont;
		
	 	text.depth = -50;

		if(!DoNotMove){
			OTSprite mySelf=gameObject.GetComponent<OTSprite>();
					Vector2 newPos=new Vector2(0.0f, -0.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			
			// Go.to(s, 0.3f, config );
			GoTween tween=new GoTween(mySelf, 0.8f, config);
			tween.setOnCompleteHandler(c => DisableTouches());

			Go.addTween(tween);
		}
		text.ForceUpdate();
	}
	
	void DisableTouches()
	{
		AudioClip myAC=(AudioClip)Resources.Load("audio/benny_mnemonics_master/benny_mnemonic_"+MyLetter+"_"+MyLetter+"_"+DisplayString.Replace(" ","_"));
		
		if(myAC!=null){
			gameManager.PlayAudioClip(myAC);
			TimeToShowMeFor=myAC.length;
		}
		else{
			Debug.Log("audio clip failed load");
		}

		IsShowing=true;
		gameManager.DisableTouches=true;
		SwitchBetweenTextMnemonic();
	}

	void HideAndEnableTouches()
	{
		if(!DoNotMove){
			OTSprite mySelf=gameObject.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(0.0f, -755.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceIn );

			
			// Go.to(s, 0.3f, config );
			GoTween tween=new GoTween(mySelf, 0.8f, config);
			tween.setOnCompleteHandler(c => DestroyMe());
			Go.addTween(tween);
		}

		IsShowing=false;
		gameManager.DisableTouches=false;

	}

	void DestroyMe()
	{
		GameObject.Destroy(gameObject);
	}

	void SwitchBetweenTextMnemonic() {
		var config=new GoTweenConfig()
			.floatProp( "alpha", 0.0f )
			.setIterations ( -1, GoLoopType.PingPong );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(Mnemonic, 1.5f, config);

		Go.addTween(tween);

		var configt=new GoTweenConfig()
			.floatProp( "alpha", 1.0f )
			.setIterations ( -1, GoLoopType.PingPong );

		
		// Go.to(s, 0.3f, config );
		GoTween tweent=new GoTween(ILetter, 1.5f, configt);

		Go.addTween(tweent);
	}

	// Update is called once per frame
	void Update () {
		if(IsShowing)
		{
			TimeShownFor+=Time.deltaTime;
			if(TimeShownFor>TimeToShowMeFor)
			{
				HideAndEnableTouches();
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class FruitMachineManager : MonoBehaviour {

	public OTSprite panel;
	public AudioClip AppearSound;
	public AudioClip DisappearSound;

	public AudioClip LeverUpSound;
	public AudioClip LeverDownSound;

	public AudioClip ChaChingSound;

	// Use this for initialization
	void Start () {
		// This is a zoomed FruitMachine
		if(panel!=null)
		{
			Vector2 newPos=new Vector2(-250.0f, 320.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(panel, 1.28f, config);
			tween.setOnCompleteHandler(c => SlideMachineUp());

			Go.addTween(tween);
		}
		// this is a non-zoomed fruitmachine
		else
		{
			audio.clip=AppearSound;
			audio.Play();
			// -205,113
			SegmentingManager segMan=gameObject.GetComponent<SegmentingManager>();
			// segMan.scaffoldStartXPos=-240.0f;
			segMan.scaffoldStartYPos=80.0f;
			segMan.letterStartXPos=-250.0f;
			OTSprite SlotMac=GameObject.Find("FruitMachine").GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(-54.0f, -33.0f);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(SlotMac, 1.28f, config);
			tween.setOnCompleteHandler(c => EnableScaffold());
			Go.addTween(tween);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayLeverDownSound ()
	{
		audio.clip=LeverDownSound;
		audio.Play();
	}

	public void PlayLeverUpSound ()
	{
		audio.clip=LeverUpSound;
		audio.Play();
	}

	void SlideMachineUp()
	{
		SlideMachine("up");
	}

	public void PlayCurrentWordAudio()
	{
		PersistentObject pMgr=GameObject.Find("PersistentManager").GetComponent<PersistentObject>();
		Debug.Log("play audio audio/words/"+pMgr.WordBankWord.ToLower());
		audio.clip=(AudioClip)Resources.Load("audio/words/"+pMgr.WordBankWord.ToLower());
		audio.Play();
	}

	public void SlideMachine(string dir)
	{
		audio.clip=AppearSound;
		audio.Play();
		
		if(dir=="up"){
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
		else if(dir=="down")
		{
			DisableScaffold();
			OTSprite SlotMac=GameObject.Find("FruitMachine").GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(-35.0f, -690.0f);
				var config=new GoTweenConfig()
					.vector2Prop( "position", newPos )
					.setEaseType( GoEaseType.BounceIn );

			
			// Go.to(s, 0.3f, config );
			GoTween tween=new GoTween(SlotMac, 0.8f, config);
			Go.addTween(tween);			
		}
	}

	void EnableScaffold()
	{
		gameObject.GetComponent<SegmentingManager>().enabled=true;

		if(GameObject.Find("btnPlayButton").GetComponent<OTSprite>())
			GameObject.Find("btnPlayButton").GetComponent<OTSprite>().alpha=1;
	}

	void DisableScaffold()
	{
		gameObject.GetComponent<SegmentingManager>().enabled=false;
	}
} 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using AlTypes;

public class OddOneOutManager : MonoBehaviour {

	int curShake=0;
	Transform lastIncorrect;
	DataWordData[] datawords;

	public Transform[] AnswerFrames;

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

	void Awake() {
		MoveFramesOffScreen();
	}

	// Use this for initialization
	void Start () {
		GameManager cmsLink=GameManager.Instance;
		datawords=GameManager.Instance.SessionMgr.CurrentDataWords;
		//DataWordData letters=cmsLink.GetSortedPhonemesForWord("lol");

		foreach(DataWordData dw in datawords)
		{
			Debug.Log(dw.Word+" (target: " + dw.IsTargetWord + " nonsense: " + dw.Nonsense.ToString() + " dummy: " + dw.IsDummyWord + " linking index: " + dw.LinkingIndex + ")");
		}


		BringFramesIn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MoveFramesOffScreen()
	{
		foreach(Transform t in AnswerFrames)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			s.position=new Vector2(s.position.x, s.position.y+600);
		}		
	}

	void BringFramesIn() {
		// drop the frames in on scene start.
		float currentTime=1.1f;
		bool SetCompleteAction=false;

		for(int i=0;i<AnswerFrames.Length;i++)
		{
			Transform t=AnswerFrames[i];
			OTSprite s=t.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y-600);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, currentTime, config);

			if(!SetCompleteAction && i==AnswerFrames.Length-1){
				tween.setOnCompleteHandler(c => ShakeFrame());
				SetCompleteAction=true;
			}



			currentTime+=0.2f;
			Go.addTween(tween);
		}
	}

	void TakeFramesAway() {
		// get rid of the frames on complete scene
		bool SetCompleteAction=false;
		float currentTime=1.1f;

		foreach(Transform t in AnswerFrames)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y+700);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, currentTime, config);

			if(!SetCompleteAction){
				tween.setOnCompleteHandler(c => SwitchToMap());
				SetCompleteAction=true;
			}

			currentTime+=0.2f;
			Go.addTween(tween);
		}
	}

	void ShakeFrame() {
		// shake a frame on incomplete

		float currentTime=UnityEngine.Random.Range(2.0f,3.0f);

		foreach(Transform t in AnswerFrames)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y+10);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setIterations ( -1, GoLoopType.PingPong )
				.setEaseType( GoEaseType.CubicInOut );

			GoTween tween=new GoTween(s, currentTime, config);

			currentTime=UnityEngine.Random.Range(2.0f,3.0f);
			Go.addTween(tween);
		}
	}

	void ShakeLastFrame()
	{
		// shake a frame after it's been thingied
		float currentTime=UnityEngine.Random.Range(2.0f,3.0f);
		OTSprite s=lastIncorrect.GetComponent<OTSprite>();
		Vector2 newPos=new Vector2(s.position.x, s.position.y+10);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setIterations ( -1, GoLoopType.PingPong )
			.setEaseType( GoEaseType.CubicInOut );

		GoTween tween=new GoTween(s, currentTime, config);

		currentTime=UnityEngine.Random.Range(2.0f,3.0f);
		Go.addTween(tween);
	}

	void IncorrectTap() {
		// for shaking. run incorrect tap on last incorrect
		IncorrectTap(lastIncorrect);
	}

	void IncorrectTap(Transform t) {

			OTSprite s=t.GetComponent<OTSprite>();

			if(curShake==0){
				lastIncorrect=t;
				Vector2 newPos=new Vector2(s.position.x+10, s.position.y);
				var config=new GoTweenConfig()
					.vector2Prop( "position", newPos )
					.setEaseType( GoEaseType.CubicInOut );

				GoTween tween=new GoTween(s, 0.1f, config);
				tween.setOnCompleteHandler(c => IncorrectTap());

				Go.addTween(tween);
				curShake++;
			}
			else if(curShake==1)
			{
				Vector2 newPos=new Vector2(s.position.x-20, s.position.y);
				var config=new GoTweenConfig()
					.vector2Prop( "position", newPos )
					.setEaseType( GoEaseType.CubicInOut );

				GoTween tween=new GoTween(s, 0.1f, config);
				tween.setOnCompleteHandler(c => IncorrectTap());
				Go.addTween(tween);
				curShake++;
			}
			else if(curShake==2)
			{
				Vector2 newPos=new Vector2(s.position.x+10, s.position.y);
				var config=new GoTweenConfig()
					.vector2Prop( "position", newPos )
					.setEaseType( GoEaseType.CubicInOut );

				GoTween tween=new GoTween(s, 0.1f, config);
				tween.setOnCompleteHandler(c => IncorrectTap());
				Go.addTween(tween);
				curShake++;
			}
			else if(curShake==3)
			{
				ShakeFrame();
				curShake=0;
				lastIncorrect=null;
			}
	}

	void SwitchToMap() {
		// switch back to map method
		Debug.Log("switch to map");
	}

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer thisAns=gesture.pickObject.GetComponent<GenericAnswer>();
				if(thisAns.isAnswer)
				{
					foreach(Transform t in AnswerFrames)
					{
						Go.killAllTweensWithTarget(t.GetComponent<OTSprite>());
					}
					TakeFramesAway();
				}	

				else
				{
					Go.killAllTweensWithTarget(gesture.pickObject.GetComponent<OTSprite>());
					IncorrectTap(gesture.pickObject.transform);
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class OddOneOutManager : MonoBehaviour {

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
			s.position=new Vector2(s.position.x, s.position.y+400);
		}		
	}

	void BringFramesIn() {
		foreach(Transform t in AnswerFrames)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y-400);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, 0.8f, config);
			Go.addTween(tween);
		}
	}

	void TakeFramesAway() {
		bool SetCompleteAction=false;
		foreach(Transform t in AnswerFrames)
		{
			OTSprite s=t.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y+400);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceIn );

			GoTween tween=new GoTween(s, 0.8f, config);

			if(!SetCompleteAction){
				tween.setOnCompleteHandler(c => SwitchToMap());
				SetCompleteAction=true;
			}
			Go.addTween(tween);
		}
	}

	void SwitchToMap() {
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
					TakeFramesAway();
				}	

				else
				{
					Debug.Log("Incorrect");
				}
			}
		}
	}
}

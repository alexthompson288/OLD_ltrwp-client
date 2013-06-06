using UnityEngine;
using System.Collections;

public class CorrectPathManager : MonoBehaviour {

	bool DisableTap=false;
	int currentIndex=0;
	int totalScalableSections=3;
	float sectionHeight=768.0f;
	public OTSprite platform;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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

	void ReturnToMap(){
		Debug.Log("end of sections brah");
	}

	void CorrectAnswer()
	{
		currentIndex++;


		var config=new GoTweenConfig()
			.position( new Vector3(Camera.main.transform.position.x, currentIndex*sectionHeight, Camera.main.transform.position.z), false )
			.setEaseType( GoEaseType.ExpoOut );

		GoTween tween=new GoTween(Camera.main.transform, 1.8f, config);
		if(currentIndex>=totalScalableSections)
			tween.setOnCompleteHandler(c => RevealTreasure());
		else 
			tween.setOnCompleteHandler(c => EnableTaps());

		Go.addTween(tween);

		MovePlatform();
	}

	void MovePlatform() 
	{	
		Vector2 newPos=new Vector2(platform.position.x, -265.0f+(currentIndex*sectionHeight));
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.CubicOut );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(platform, 1.2f, config);
		Go.addTween(tween);
	}

	void EnableTaps()
	{
		DisableTap=false;
	}

	void RevealTreasure()
	{
		Debug.Log("reveal");
		OTSprite treecover=GameObject.Find("4-treecover").GetComponent<OTSprite>();
		
		Vector2 newPos=new Vector2(-1000.0f, treecover.position.y);
		var config=new GoTweenConfig()
			.vector2Prop( "position", newPos )
			.setEaseType( GoEaseType.ExpoIn );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(treecover, 1.8f, config);
		tween.setOnCompleteHandler(c => ReturnToMap());

		Go.addTween(tween);

	}

	void On_SimpleTap(Gesture gesture)
	{
		if(DisableTap)return;

		if(gesture.pickObject)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer thisAns=gesture.pickObject.GetComponent<GenericAnswer>();
				if(thisAns.isAnswer)
				{
					DisableTap=true;
					CorrectAnswer();
				}	

				else
				{
					Debug.Log("Incorrect");
				}
			}
		}
	}
}

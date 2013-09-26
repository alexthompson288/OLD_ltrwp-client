using UnityEngine;
using System.Collections;

public class WordNotFound : MonoBehaviour {

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

	void On_SimpleTap(Gesture gesture) {
		if(transform.position.y > -200.0f)// gesture.pickObject==gameObject)
		{
			Vector3 newPos=new Vector3(0.0f, -950.0f, -20.0f);
			var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(transform, 1.28f, config);
			//	tween.setOnCompleteHandler(c => SlideMachineUp());

			Go.addTween(tween);
		}
		
	}
}

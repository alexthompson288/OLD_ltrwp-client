using UnityEngine;
using System.Collections;

public class TransitionScreen : MonoBehaviour {
	
	public GameObject ScreenCover;

	// Use this for initialization
	void Start () {
		Vector3 newPos=new Vector3(1024.0f, 0.0f, 0.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "localPosition", newPos )
			.setEaseType( GoEaseType.QuadInOut );

		GoTween tween=new GoTween(ScreenCover.transform, 0.8f, config);
		tween.setOnCompleteHandler(c => FinishedIntro());

		Go.addTween(tween);
	}
	
	void Awake () {
		transform.position = new Vector3(0.0f, 0.0f, -100.0f);
	}
	
	public void ChangeLevel(string level)
	{
		Vector3 newPos=new Vector3(0.0f, 0.0f, 0.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "localPosition", newPos )
			.setEaseType( GoEaseType.QuadInOut );

		GoTween tween=new GoTween(ScreenCover.transform, 0.8f, config);
		tween.setOnCompleteHandler(c => LoadNextLevel(level));

		Go.addTween(tween);
	}
	
	public IEnumerator ChangeLevelDelayed(string level, float delay)
	{
		yield return new WaitForSeconds(delay);
		ChangeLevel(level);
	}
	
	// Reset the cover to the left
	void FinishedIntro () {
		ScreenCover.transform.localPosition = new Vector3(-1024.0f, 0.0f, 0.0f);
	}
	
	// Reset the cover to the left
	void LoadNextLevel (string level) {
		Application.LoadLevel(level);
	}	
	
}

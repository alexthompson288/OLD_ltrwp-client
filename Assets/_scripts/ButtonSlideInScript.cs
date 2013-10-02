using UnityEngine;
using System.Collections;

public class ButtonSlideInScript : MonoBehaviour {
	
	public Vector3 newPos;
	private bool IsMoving = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SlideIn(float duration, float delay){
		IsMoving = true;
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setDelay( delay)
			.setEaseType( GoEaseType.QuadOut );

		GoTween tween=new GoTween(transform, duration, config);
		Go.addTween(tween);
		StartCoroutine(TweemCompleted(duration + delay));
	}
	
	IEnumerator TweemCompleted(float duration)
	{
		yield return new WaitForSeconds(duration);
		IsMoving = false;
		transform.GetComponent<Wobble>().SeedWobbles(1.0f, 3.0f);
		
	}
}

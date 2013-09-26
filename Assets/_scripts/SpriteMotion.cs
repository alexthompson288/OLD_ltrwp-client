using UnityEngine;
using System.Collections;

public class SpriteMotion : MonoBehaviour {
	
	public bool Spin = false;
	public float SpinSpeed = 200.0f;
	public bool Throb = false;
	public float ThrobScale = 1.0f;
	private Vector3 Scale;
	public float ThrobSpeed = 1.0f;
	private float FadeDirection = 0.0f;
	private OTSprite s;

	// Use this for initialization
	void Awake () {
		Scale = transform.localScale;
		s= gameObject.GetComponent<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Spin)
		{
			transform.Rotate(0.0f, 0.0f, SpinSpeed * Time.deltaTime);
		}
		if(Throb)
		{
			float newScale = (Mathf.Sin(Time.timeSinceLevelLoad * ThrobSpeed) * ThrobScale);
			transform.localScale = Scale + new Vector3( newScale,newScale, 0.0f );
		}
		
		if(FadeDirection != 0.0f)
		{
			s.alpha = Mathf.Clamp01( s.alpha + (FadeDirection * Time.deltaTime));	
		}	
	}
	
	public void FadeIn(float time)
	{		
		s.alpha = 0.0f;
		FadeDirection = 1.0f * time;
	/*	OTTween mtt=new OTTween(t,time, OTEasing.Linear);
		mtt.Tween("alpha", 1.0f);*/
	}
	
	public void FadeOut(float time)
	{
		FadeDirection = -1.0f * time;
		s.alpha = 1.0f;
		/*OTTween mt=new OTTween(s,time, OTEasing.Linear);
		mt.Tween("alpha", 0.0f);*/
	}
}

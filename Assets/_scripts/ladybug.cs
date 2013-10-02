using UnityEngine;
using System.Collections;

public class ladybug : MonoBehaviour {
	
	public Spline Path;
	public float TotalTime = 20.0f;
	private float CurrTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CurrTime += Time.deltaTime;
		transform.position = Path.GetPositionOnSpline(CurrTime / TotalTime);
		transform.right = -Path.GetTangentToSpline(CurrTime / TotalTime);
		
		if(CurrTime > TotalTime)
			CurrTime = 0.0f;
	
	}
}

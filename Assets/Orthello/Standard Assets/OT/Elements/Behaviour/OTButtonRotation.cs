using UnityEngine;
using System.Collections;

public class OTButtonRotation : MonoBehaviour {
	
	
	/// <summary>
	/// The roation speed in degrees per second
	/// </summary>
	public float rotationSpeed = 45;
	
	OTButtonElement button;
	// Use this for initialization
	void Start () {
		button = GetComponent<OTButtonElement>();
	}

	float rotation;
	// Update is called once per frame
	void Update () {
		rotation += Time.deltaTime * rotationSpeed;
		if (button.normal!=null)
			button.normal.rotation = rotation;
		if (button.hover!=null)
			button.hover.rotation = rotation;
		if (button.active!=null)
			button.active.rotation = rotation;
	}
}

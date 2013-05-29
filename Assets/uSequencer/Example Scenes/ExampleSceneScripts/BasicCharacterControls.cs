using UnityEngine;
using System.Collections;

public class BasicCharacterControls : MonoBehaviour 
{	
	public USSequencer cutscene = null;
	public float strength = 10.0f;
	
	// Update is called once per frame
	void Update () 
	{
		if(cutscene && cutscene.IsPlaying)
			return;
		
		float localStrength = strength * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.W))
		{
			rigidbody.AddRelativeForce(-localStrength, 0.0f, 0.0f);
		}
		if(Input.GetKey(KeyCode.S))
		{
			rigidbody.AddRelativeForce(localStrength, 0.0f, 0.0f);
		}
		if(Input.GetKey(KeyCode.A))
		{
			rigidbody.AddRelativeForce(0.0f, 0.0f, -localStrength);
		}
		if(Input.GetKey(KeyCode.D))
		{
			rigidbody.AddRelativeForce(0.0f, 0.0f, localStrength);
		}
	}
}

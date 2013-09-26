using UnityEngine;
using System.Collections;

public class SimpleButton : MonoBehaviour {
	
	public GameObject UpState;
	public GameObject DownState;
	
	public void PressDown()
	{
		UpState.SetActive(false);
		DownState.SetActive(true);
	}
	
	public void ReleaseButton()
	{
		UpState.SetActive(true);
		DownState.SetActive(false);
	}
}

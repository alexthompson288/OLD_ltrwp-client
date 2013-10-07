using UnityEngine;
using System.Collections;

public class SimpleSpriteButton : MonoBehaviour {
	
	public Texture2D UpState;
	public Texture2D DownState;

	
	public void PressDown()
	{
		GetComponent<OTSprite>().image = DownState;
		
	}
	
	public void ReleaseButton()
	{
		GetComponent<OTSprite>().image = UpState;
	}
}

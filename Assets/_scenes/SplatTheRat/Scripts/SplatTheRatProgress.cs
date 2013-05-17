using UnityEngine;
using System.Collections;

public class SplatTheRatProgress : MonoBehaviour {
	
	public int currentNumber=0;
	int lastNumber=0;
	public Texture2D InactiveSprite;
	public Texture2D ActiveSprite;
	
	// Use this for initialization
	void Start () {
		lastNumber=currentNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if(lastNumber!=currentNumber)
		{
			lastNumber=currentNumber;
			
			int HowMany=0;
			foreach(Transform t in transform)
			{
				if(HowMany>=lastNumber)break;
				
				OTSprite s=t.GetComponent<OTSprite>();
				s.image=ActiveSprite;
				HowMany++;
			}
			
		}
	}
}

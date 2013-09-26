using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlTypes;


public class WordCollider : MonoBehaviour {
	
	List<PhonemeData>_PhonemeData;
	PipPad pipPad;
	public string Word;
	private GameObject Sparkle;

	// Use this for initialization
	void Start () {
		pipPad = GameObject.Find("PipPad").GetComponent<PipPad>();
		Sparkle = GameObject.Find("WordSelectionGlow");
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
		if(gesture.pickObject==gameObject && pipPad.isMachineMoving == false)
		{
			if(pipPad.isMachineUp == false)
			{
				
				
				if(Word.Length > 1)
				{
					Sparkle.transform.position = new Vector3 (transform.position.x + 2.0f, transform.position.y, Sparkle.transform.position.z);
					Sparkle.GetComponent<SpriteMotion>().FadeIn(1.8f);
					if(Word.Contains(".") || Word.Contains("?") || Word.Contains("!") || Word.Contains(",") || Word.Contains(";"))
					{
						Word = Word.Remove(Word.Length - 1, 1);
					}
					Word = Word.ToLower();
					try {
						//Word = "cake";
						 _PhonemeData = GameManager.Instance.GetOrderedPhonemesForWord(Word);
		   			 }
		    		catch  {
		        		Word = "notFound";
		    		} 
					Debug.Log("Pressed word: " + Word);
					GameObject.Find("PipPad").GetComponent<PipPad>().MakeAppear(_PhonemeData, Word.ToLower());
	
				}
			}else{
				pipPad.MakeDisappear();	
				
			}
			
		}
		
	}
}

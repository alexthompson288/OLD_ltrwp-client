using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
 

public class WordColliderGenerator {
	
	private List<GameObject> WordColliders = new List<GameObject>();
	private OTTextSprite lastSprite = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(lastSprite != null)
		{
			Debug.Log("LAST PAGE TEXT: " + lastSprite._parsed[0].lines[0].words.Count); 
		}
	}
	
	public void DeleteExistingWordColliders()
	{
		for(int i = 0; i < WordColliders.Count; i++)
		{
			GameObject go = WordColliders[i];
			go.SetActive(false);
			//Destroy (go);
		}
		WordColliders.Clear();
	}
	
	public IEnumerator BreakUpSentenceIntoWords(OTTextSprite TextSprite, bool IsFirstPage)
	{
		yield return new WaitForSeconds(0.1f);
		
		lastSprite = TextSprite;
		// delete previous colliders
		DeleteExistingWordColliders();
			
		// parse/flush text otherwise it happens after the update loop
		TextSprite.ParseText();		
		TextSprite.ForceUpdate();
	
		// get line height from atlas
		OTSpriteAtlas atlas = (TextSprite.spriteContainer as OTSpriteAtlas);		
		OTAtlasData data = atlas.atlasData[0];
		float lineHeight = (float)data.frameSize.y * 1.2f;
	
		Debug.Log("PAGE TEXT: " + TextSprite.text);//._parsed[0].lines[0].words[0].text);
		
		float LineWidthSoFar = 0.0f;
		float NewLineHeight = (float)data.frameSize.y * 0.85f;
		
		// weird bug where the number of lines is stored as x2 - 1
		int NumberOfLines = TextSprite._parsed.Count;// - 1) / 2) + 1;
		Debug.Log("number of lines: " + TextSprite._parsed[0].lines[0].words.Count);
		
		// sometimes othello doesnt return correct widths, in that case we force it
		if(IsFirstPage)
		{
			/*Debug.Log("ITS THE FIRST PAGE");
			string text = TextSprite.text;
			TextSprite = GameObject.Find("SpaceCalculator").GetComponent<OTTextSprite>();
			TextSprite.text = text.ToString();
			TextSprite.ForceUpdate();
			TextSprite.ParseText();
			TextSprite*/
		//	TextSprite.ForceUpdate();
			/*string [] sentences = text.Split('\n');
			Debug.Log("number of lines GEN: " + sentences.Length);
			for(int i = 0; i < sentences.Length; i++)
			{
					Debug.Log("GEN line"+i.ToString()+": " + sentences[i]);
			}*/
		}
		
		// Add word colliders by text alignment
		for(int j = 0; j < NumberOfLines; j++)
		{
			LineWidthSoFar = 0.0f;
			if(TextSprite.pivot == OTObject.Pivot.TopLeft)
			{
				Debug.Log("left aligned");
				for (int i = 0; i < TextSprite._parsed[j].lines[0].words.Count; i ++)
				{	
					
					GameObject WCollider =  new GameObject("WordCollider" + TextSprite._parsed[j].lines[0].words[i].text);
					WordColliders.Add(WCollider);
					BoxCollider BC = WCollider.AddComponent("BoxCollider") as BoxCollider;
					BC.isTrigger = true;
					WordCollider wc = WCollider.AddComponent("WordCollider") as WordCollider;
					wc.Word = TextSprite._parsed[j].lines[0].words[i].text.ToString();
					float WordWidth = (float)TextSprite._parsed[j].lines[0].words[i].width;	
					BC.size = new Vector3(WordWidth, lineHeight, 1.0f);
					Vector3 SentencePos = TextSprite.gameObject.transform.position;
					
					WCollider.transform.position = new Vector3(SentencePos.x + (WordWidth / 2.0f ) + LineWidthSoFar,
																SentencePos.y - (lineHeight/2.0f) - (NewLineHeight * j),
																SentencePos.z);	
					LineWidthSoFar += WordWidth + (float)TextSprite._parsed[j].lines[0].words[0].space;
				}
			}else if (TextSprite.pivot == OTObject.Pivot.Top){
				Debug.Log("center aligned");
				LineWidthSoFar = -((float)TextSprite._parsed[j].lines[0].width / 2.0f);
				for (int i = 0; i < TextSprite._parsed[j].lines[0].words.Count; i ++)
				{	
					GameObject WCollider =  new GameObject("WordCollider" + TextSprite._parsed[j].lines[0].words[i].text);
					WordColliders.Add(WCollider);
					BoxCollider BC = WCollider.AddComponent("BoxCollider") as BoxCollider;
					BC.isTrigger = true;
					WordCollider wc = WCollider.AddComponent("WordCollider") as WordCollider;
					wc.Word = TextSprite._parsed[j].lines[0].words[i].text.ToString();
					float WordWidth = (float)TextSprite._parsed[j].lines[0].words[i].width;
					BC.size = new Vector3(WordWidth, lineHeight, 1.0f);
					Vector3 SentencePos = TextSprite.gameObject.transform.position;
					
					WCollider.transform.position = new Vector3(SentencePos.x + (WordWidth / 2.0f ) + LineWidthSoFar,
																SentencePos.y - (lineHeight/2.0f) - (NewLineHeight * j),
																SentencePos.z);	
					LineWidthSoFar += WordWidth + (float)TextSprite._parsed[j].lines[0].words[0].space;
				}
			}else{
				Debug.Log("right aligned");
				for (int i = TextSprite._parsed[j].lines[0].words.Count-1; i > -1; i--)
				{	
					GameObject WCollider =  new GameObject("WordCollider_" + TextSprite._parsed[j].lines[0].words[i].text);
					WordColliders.Add(WCollider);
					BoxCollider BC = WCollider.AddComponent("BoxCollider") as BoxCollider;
					BC.isTrigger = true;
					WordCollider wc = WCollider.AddComponent("WordCollider") as WordCollider;
					wc.Word = TextSprite._parsed[j].lines[0].words[i].text.ToString();
					float WordWidth = (float)TextSprite._parsed[j].lines[0].words[i].width;
					BC.size = new Vector3(WordWidth, lineHeight, 1.0f);
					Vector3 SentencePos = TextSprite.gameObject.transform.position;
					
					WCollider.transform.position = new Vector3(SentencePos.x - (WordWidth / 2.0f ) - LineWidthSoFar,
																SentencePos.y - (lineHeight/2.0f) - (NewLineHeight * j),
																SentencePos.z);	
					LineWidthSoFar += WordWidth + (float)TextSprite._parsed[j].lines[0].words[0].space;
				}
			}
		}			
	}
}

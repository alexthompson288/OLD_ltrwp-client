using UnityEngine;
using System.Collections;

public class StickerScript : MonoBehaviour {
	
	OTSprite Sprite;
	Vector3 LastKnowPosition;
	public bool IsGettingDragged = false;
	bool IsResetting = false;
	bool IsReset = true;
	bool IsInPlayArea = false;
	public int ListIndex =0;

	// Use this for initialization
	void Start () {
		Sprite = GetComponent<OTSprite>();
		Sprite.onDragStart = OnDragStart;
		Sprite.onDragEnd = OnDragEnd;
	}
	
	// Update is called once per frame
	void Update () {
		if(IsGettingDragged)
		{
			Sprite.depth = -15;
			IsReset = false;
		}else{
			if(Sprite.position.x > 280.0f && !IsReset){
				Sprite.depth = -5;
				IsResetting = true;
				iTween.MoveTo(gameObject, iTween.Hash("position",  LastKnowPosition, "time", 0.1f, "oncompletetarget", gameObject, "oncomplete", "FinishedResetting"));
			}
			
			if(IsResetting == false)
				LastKnowPosition = Sprite.position;	
		}
		
		if(!IsInPlayArea && Sprite.position.x <= 280.0f)
		{
			IsInPlayArea = true;
			GameObject.Find("StickerScrollWheel").GetComponent<StickerScrollWheel>().ReplaceSprite(ListIndex);
		}
		
		
		
	}
	
	void OnDragStart(OTObject owner)
	{
		IsGettingDragged = true;
	}
	
	void OnDragEnd(OTObject owner)
	{
		IsGettingDragged = false;
	}
	
	public void FinishedResetting()
	{
		IsResetting = false;
		IsReset = true;
	}
}

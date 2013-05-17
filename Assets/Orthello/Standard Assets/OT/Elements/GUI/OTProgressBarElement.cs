using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTProgressBarElement : OTElement {
	
	public enum Slider { Horizontal, Vertical, None };
	
	public float progress = 0;
	public Vector2 size = new Vector2(250,30);
	public OTTextSprite progressLabel = null;
	public OTElementAction progressAction;
	public Slider slider = Slider.None;
			
	OTSprite progressSprite = null;
	float _progress = 0;
	Vector2 _size = new Vector2(250,30);
	
			
	protected override void Awake()
	{
		_size = size;
		_progress = progress;		
		base.Awake();
	}
		
	// Use this for initialization
	protected override void Start () {
		base.Start();		
		progressSprite = Child("progress") as OTSprite;						
	}
	
	// Update is called once per frame
	protected override void Update () {
		
		if (!Application.isPlaying)
		{
			if (slider != Slider.None)
			{
				if (collider == null)
					gameObject.AddComponent<BoxCollider>();			
			}
			else
			{
				if (collider!=null)
					DestroyImmediate(collider);
			}
		}
		
		if (!_size.Equals(size))
		{
			_size = size;
			transform.localScale = new Vector3(size.x,size.y,1);						
		}
		
		if (_progress != progress)
		{
			if (progress <0) progress = 0;
			if (progress >1) progress = 1;
			
			
			if (progressSprite == null)
				progressSprite = Child("progress") as OTSprite;	
				
			progressSprite.size = new Vector2(1-progress,1);	
			
			if (progressLabel!=null) 
			{
				progressLabel.text = string.Format("{0}",(int)(progress * 100))+"%";
				progressLabel.ForceUpdate();
			}

			
			_progress = progress;
			Action(progressAction);
				
		}
		
		if (slider != Slider.None && Application.isPlaying)
		{
			if (Input.GetMouseButton(0))
			{
				RaycastHit hit;
	            bool found = collider.Raycast(OT.view.camera.ScreenPointToRay(Input.mousePosition), out hit, 2500f);				
				if (found)
				{
					float _pr;
					Vector2 p = transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
					if (slider == Slider.Horizontal)
						_pr = 0.5f + p.x;
					else
						_pr = 0.5f + p.y;	
					
					if (_pr < 0 ) _pr = 0;
					if (_pr > 1 ) _pr = 1;
					
					progress = _pr;					
					
					
				}
			}
		}
				
		base.Update();					
	}		
	
}

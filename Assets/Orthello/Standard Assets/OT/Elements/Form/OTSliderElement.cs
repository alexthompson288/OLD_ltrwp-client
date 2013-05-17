using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTSliderElement : OTFormElement {
	
	public enum Orientation { Horizontal, Vertical };	
	public OTElementAction slideAction;
	public Orientation orientation = Orientation.Horizontal;
	public float minValue = 0;
	public float maxValue = 100;
	public float _value = 50;
	public int divisions = 10;
	
	/// <summary>
	/// This delegate is called when this slider is active
	/// </summary>
	public ElementDelegate onSlide = null;
		
	OTSprite sliderSprite = null;
	OTSprite slidingSprite = null;
		
	protected override void SetValue()
	{		
		_value = floatValue;
		if (_value<minValue)
			_value = minValue;
		if (_value>maxValue)
			_value = maxValue;
		value_ = _value;
		
		Action(slideAction);
		if (onSlide!=null)
			onSlide(this);
				
		base.SetValue();
	}

	public override void Repaint()
	{
		base.Repaint();
		div = (maxValue - minValue)/divisions;		
		if (sliderSprite!=null && slidingSprite!=null && maxValue-minValue!=0)
		{			
			slidingSprite.transform.position = (Vector3)((Vector2)sliderSprite.transform.position + valueVector) +
				new Vector3(0,0,slidingSprite.transform.position.z);
		}
	}
	
	/// <summary>
	/// Sets the size of the sliding sprite to view to content relation
	/// </summary>
	public void SetView(float view, float content)
	{
		if (slidingSprite == null || sliderSprite == null)
			GetObjects();

		if (slidingSprite == null || sliderSprite == null)
			return;
		
		if (content>0)
		{
			Vector2 sz = slidingSprite.size;
			if (orientation == Orientation.Horizontal)
				sz.x = sliderSprite.size.x * (view/content);
			else
				sz.y = sliderSprite.size.y * (view/content);
				
			slidingSprite.size = sz;
			Repaint();
		}
		
	}
		
	/// <summary>
	/// Gets or sets the value of this slider (0-1)
	/// </summary>
	public float localValue
	{
		get
		{	
			float f = floatValue;
			if (maxValue-minValue!=0)
				return ((floatValue - minValue) / (maxValue-minValue)) -0.5f;
			else
				return 0;
		}
		set
		{
			if (value<-0.5f)
				value = -0.5f;
			if (value>0.5f)
				value = 0.5f;
			
			_value = minValue + ((value + 0.5f) * (maxValue-minValue));
			this.value = _value;
		}
	}
		
	Vector2 valueVector
	{
		get
		{
			if (orientation == Orientation.Horizontal)
				return new Vector2((sliderSprite.worldRect.width - slidingSprite.worldRect.width) * localValue,0);
			else
				return new Vector2(0,(sliderSprite.worldRect.height - slidingSprite.worldRect.height) * (localValue));
		}
	}
		
	float div = 0;
	protected override void GetObjects()
	{
		base.GetObjects();
		
		if (sliderSprite == null)
		{
			sliderSprite = Sprite("slider");		
			if (sliderSprite!=null)
			{
				if(sliderSprite.pivot != OTObject.Pivot.Center)
					sliderSprite.pivot = OTObject.Pivot.Center;
							
				if (!sliderSprite.registerInput)
					sliderSprite.registerInput = true;						
			}
		}
		if (slidingSprite == null)
		{
			slidingSprite = Sprite("sliding");			
		}
	}
	
	protected override void Awake ()
	{
		value_ = _value;						
		base.Awake ();
	}
		
	// Use this for initialization
	protected override void Start () {
		base.Start();					
	}
	
	protected override bool OverElement()
	{
		return OT.Over(sliderSprite);		
	}
	
	bool otherHandling = false;
	Orientation _orientation = Orientation.Horizontal;
	// Update is called once per frame
	new protected void Update () {				
		base.Update();							
		if (!Application.isPlaying)
		{			
			if (_orientation!=orientation)
			{
				sliderSprite = null;
				_orientation = orientation;				
			}									
			if (_value<minValue)
			{
				_value = minValue;
				value_ = _value;
			}
			else
			if (_value>maxValue)
			{
				_value = maxValue;
			}
			
			if (_value != floatValue)
				value = _value;		
		}

		if (sliderSprite!=null && slidingSprite!=null)
		{				
			if (isHandling || otherHandling)
			{
				if (!Input.GetMouseButton(0))
				{
					handling = false;
					otherHandling = false;
				}
			}
			else
			if (Input.GetMouseButton(0) && !overElement)			
				otherHandling = true;
			else
			if (!otherHandling && Input.GetMouseButton(0) && overElement)
				handling = true;
			
			if (Input.GetMouseButton(0) && handling)
			{
				if (!isChildControl) 
					Focus();												
				Vector3 hp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (over)
					hp = OT.hit.point;					
				Vector2 localPoint = sliderSprite.otTransform.worldToLocalMatrix.MultiplyPoint3x4(hp);	
				localValue = (orientation == Orientation.Horizontal)?localPoint.x:localPoint.y;				
			}
			else
			{
				if (gotFocus)
				{
					if (Input.anyKeyDown)
					{
						if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.PageUp) || 
							Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Keypad8))
							value = floatValue - div;
						else
						if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.PageDown) || 
							Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad2))
							value = floatValue + div;
					}					
				}					
			}
								
		}
					
	}		
	
}

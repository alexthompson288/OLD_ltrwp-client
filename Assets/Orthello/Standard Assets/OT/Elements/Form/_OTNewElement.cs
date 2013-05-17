using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _OTNewElement : OTFormElement {
			
	public bool _value = false;
	OTSprite onSprite;
	OTSprite offSprite;
	
	protected override void SetValue()
	{ 
		_value = boolValue;
		base.SetValue();
	}
			
	protected override void GetObjects()
	{
		base.GetObjects();
		onSprite = Sprite("on");	
		offSprite = Sprite("off");									
	}
	
	new protected void Awake()
	{
		value_ = _value;
	}
			
	protected override bool OverElement()
	{
		return false;
	}
	
	public override void Repaint()
	{
		base.Repaint();
		if (onSprite!=null) 
			onSprite.visible = boolValue;
		if (offSprite!=null) 
			offSprite.visible = boolValue;
	}	
	
	new protected void Update()
	{
		
		if (!Application.isPlaying)
		{
			if (boolValue!=_value)
				value = _value;
		}
		
		base.Update();
	}
	
		
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTCheckElement : OTFormElement {
	
	public string _labelCaption = "";
	public bool _value = false;
	
	OTSprite onSprite;
	OTSprite offSprite;
	OTTextSprite labelSprite;
	
	public string labelCaption
	{
		get
		{
			return _labelCaption;
		}
		set
		{
			_labelCaption = value;
			Repaint();
		}
	}
	
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
		labelSprite = Sprite("label") as OTTextSprite;
	}
	
	public override void Repaint()
	{
		base.Repaint();
				
		if (onSprite!=null) 
			onSprite.visible = boolValue;
		if (offSprite!=null) 
			offSprite.visible = !boolValue;		
		if (labelSprite!=null) 
		{
			string lab = _labelCaption;
			if (lab == "")
				lab = (variable=="")?name:variable;			
			labelSprite.text = lab;
			if (form!=null)
				labelSprite.tintColor = (gotFocus)?form.colorActiveText:form.colorNormalText;			
			labelSprite.ForceUpdate();			
		}
	}
	
	new protected void Awake()
	{
		value_ = _value;
	}
	
	protected override bool OverElement()
	{
		return (OT.Over(onSprite) || OT.Over(offSprite) || OT.Over(labelSprite));
	}
			
	new protected void Update()
	{
		base.Update();		
		if (!Application.isPlaying)
		{
			if (boolValue!=_value)
				value = _value;			
		}
		else
		{
			if (Input.GetMouseButtonDown(0) && overElement || (gotFocus && Input.GetKeyDown(KeyCode.Space)))
				value = !boolValue;
		}		
		
		if (labelSprite!=null && _labelCaption!=labelSprite.text)
			Repaint();			
		
		
	}
			
}

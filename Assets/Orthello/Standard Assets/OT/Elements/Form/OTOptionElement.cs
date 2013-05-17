using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTOptionElement : OTFormElement {
			
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
	
	void SetFalse()
	{
		value  = false;
	}
	void SetTrue()
	{
		value  = true;
	}

	void CheckOptions()
	{
		Transform t = container.transform;
		OTOptionElement first = null;
		bool val = false;
		
		for (int i=0; i<t.childCount; i++)
		{
			OTOptionElement opt = t.GetChild(i).GetComponent<OTOptionElement>();
			if (opt!=null) 
			{
				if (first == null)
					first = opt;
				if (opt.boolValue)
				{
					if (val)
						opt.Invoke("SetFalse",0.1f);
					else
						val = true;
				}					
			}
		}				
		if (!val)
			first.Invoke("SetTrue",0.1f);
		optionChecked = true;
	}
	
	protected override void SetValue()
	{ 
		if (boolValue && container!=null)
		{
			Transform t = container.transform;
			for (int i=0; i<t.childCount; i++)
			{
				OTOptionElement opt = t.GetChild(i).GetComponent<OTOptionElement>();
				if (opt!=null && opt != this) 
					opt.value = false;
			}		
		}
		else
		if (!boolValue && container!=null)
		{
			bool val = false;
			Transform t = container.transform;
			for (int i=0; i<t.childCount; i++)
			{
				OTOptionElement opt = t.GetChild(i).GetComponent<OTOptionElement>();
				if (opt!=null && opt != this && opt.boolValue) 
					val = true;
			}	
			
			if (!val)
			{
				value_ = true;
				return;
			}
		}
		
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
	
	bool optionChecked = false;
	public override void Repaint()
	{
		base.Repaint();
				
		if (form!=null && container!=null && !optionChecked)
		{
			CheckOptions();
			optionChecked = true;
		}
		
		
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

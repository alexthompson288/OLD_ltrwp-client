using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTFormElement : OTElement {
	
	/// <summary>
	/// The form of this form element
	/// </summary>
	/// <remarks>
	/// Set in the editor or run-time using form.Add(formElement);
	/// </remarks>
	public OTForm form;
	public string variable = "";
	public bool inXML = true;

	[HideInInspector]
	public OTElement container;
	
	
    public delegate void FormElementDelegate(OTFormElement element);
	
	
	public OTElementAction setValueAction;
	public FormElementDelegate onSetValue = null;
	public OTElementAction focusAction;
	public FormElementDelegate onFocus = null;
	public OTElementAction deFocusAction;
	public FormElementDelegate onDeFocus = null;
			
	protected List<OTSprite> focusSprites = new List<OTSprite>();
		
	protected virtual void SetValue()
	{ 
		Action(setValueAction);
		if (Application.isPlaying)
		{
			if (onSetValue!=null)
				onSetValue(this);
			form._ElementChanged(this);
		}
		Repaint();
	}
			
	protected bool handling = false;
	protected bool isHandling
	{
		get
		{
			if (handling) return true;
			if (form!=null && form.focusedElement!=null)
				if (form.focusedElement.handling)
					return true;
			return false;
		}
	}
	
	
	protected object value_;
	public  object value
	{
		get
		{
			return value_;
		}
		set
		{
			if (value_!=value)
			{
				value_ = value;
				SetValue();
			}
		}
	}		
	
	public float floatValue
	{
		get
		{
			try
			{
				return (float)System.Convert.ToDouble(value_);
			}
			catch
			{
			}
			return 0;
		}
	}		

	
	protected virtual string XMLValue()
	{
		return System.Convert.ToString(value);
	}
	
	public string xmlValue
	{
		get
		{
			return XMLValue();
		}
	}
	
	public string stringValue
	{
		get
		{
			try
			{
				return System.Convert.ToString(value);
			}
			catch
			{
			}
			return "";
		}
	}		

	public int intValue
	{
		get
		{
			try
			{
				return System.Convert.ToInt32(value);
			}
			catch
			{
			}
			return 0;
		}
	}		
	
	public bool boolValue
	{
		get
		{
			try
			{
				if (stringValue.ToLower() == "true")
					return true;
				else
				if (stringValue.ToLower() == "false")
					return false;
				else
				if (intValue == 1)
					return true;
				else
				if (intValue == 0)
					return false;
				return System.Convert.ToBoolean(value);
			}
			catch
			{
			}
			return false;
		}
	}		
	
	/// <summary>
	/// Inidicates if a formelement has focus
	/// </summary>
	public bool gotFocus
	{
		get
		{
			if (form!=null)
				return (form.focusedElement == this);
			else
				return false;
		}
	}
	
	public static OTSliderElement Slider(string name)
	{
		return (OTSliderElement)Lookup(name);
	}
	
	public static OTTextInputElement TextInput(string name)
	{
		return (OTTextInputElement)Lookup(name);
	}
	
	new public static OTButtonElement Button(string name)
	{
		return (OTButtonElement)Lookup(name);
	}
	
	new protected virtual void Awake()
	{
		base.Awake();
		if (variable == "")
			variable = name;
	}
	
	new protected virtual void Start()
	{
		base.Start();
	}

	void PrevElement()
	{
		if (form!=null)
			form.Focus(-1);
	}
	
	void NextElement()
	{
		if (form!=null)
			form.Focus(1);
	}
	
	protected override void Initialize()
	{
		Repaint();
	}
	
	bool _overElement = false;
	/// <summary>
	/// Indicates that the mouse is over this element
	/// </summary>
	public bool overElement
	{
		get
		{
			return _overElement;
		}
	}	
	protected virtual bool OverElement()
	{
		return false;		
	}
			
	public virtual void Repaint()
	{
		if (!isChildControl && focusSprites.Count>0)
		{
			int i=0;
			while (i<focusSprites.Count)
			{
				if (focusSprites[i]!=null)
				{
					focusSprites[i].visible = gotFocus;
					i++;
				}
				else
					focusSprites.RemoveAt(i);					
			}
		}
		return;
	}
	
	new protected void Update()
	{
		base.Update();
		if (!Application.isPlaying)
		{
			if (variable == "")
				variable = name;
		}
		else
		{
			_overElement = OverElement();
			if (gotFocus && Input.anyKeyDown)
			{
				if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
						Invoke("PrevElement",0.1f);
					else
						Invoke("NextElement",0.1f);
				}				
			}
			
			if (Input.GetMouseButtonDown(0))
			{
				if (!gotFocus && overElement)
					Focus();
				else
				if (gotFocus && !overElement)
					Defocus();
			}
			
		}
	}
	
	
	/// <summary>
	/// Focuses this element
	/// </summary>
	public void Focus()
	{
		if (form!=null)
		{
			if (!gotFocus)
			  form.Focus(this);
			
			Action(focusAction);
			if (onFocus!=null)
				onFocus(this);			
			
		}
	}
	
	/// <summary>
	/// Focuses this element
	/// </summary>
	public void Defocus()
	{
		if (form!=null)
		{
			if (gotFocus)
			  form.Defocus(this);
			
			Action(deFocusAction);
			if (onDeFocus!=null)
				onDeFocus(this);			
		}
	}
	
	protected bool isChildControl = false;
	OTElement parentElement = null;
	public void IsChildControlOf(OTElement parent)
	{
		parentElement = parent;
		isChildControl = true;
	}
		
	int childsFound = 0;	
	void GetFocusSpites(Transform p)
	{
		for (int i=0; i<p.childCount; i++)
		{
			Transform c = p.GetChild(i);
			OTSprite sc = c.GetComponent<OTSprite>();
			if (sc!=null)
			{
				OTSprite s = sc.Sprite("focus");
				if (s!=null) focusSprites.Add(s);				
			}
			if (c.childCount>0)
				GetFocusSpites(c);
		}
	}
	
	
	protected override void GetObjects()
	{
		base.GetObjects();		
		focusSprites.Clear();
		OTSprite s = Sprite("focus");
		if (s!=null) focusSprites.Add(s);
		GetFocusSpites(transform);		
		if (transform.childCount!=childsFound)
		{
			childsFound = transform.childCount;
			Repaint();
		}
		
		if (form==null)
		{
			Transform t = transform.parent;
			while (t!=null)
			{
				if (t.GetComponent<OTForm>()!=null)
				{
					form = t.GetComponent<OTForm>();
					break;
				}				
				t = t.parent;				
			}
			
		}
		
	}
		
	public override void _IMsg(string cmd)
	{
		switch(cmd)
		{
			default:
				base._IMsg(cmd);
			break;
		}
	}	
		
}

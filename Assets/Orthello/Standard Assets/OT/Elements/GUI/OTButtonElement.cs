using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTButtonElement : OTElement {
	
	public OTElement.ElementDelegate onClick = null;
	public OTElement.ElementDelegate onEnter = null;
	public OTElement.ElementDelegate onStay = null;
	public OTElement.ElementDelegate onExit = null;
		
	static Dictionary<string, List<OTButtonElement>> groupButtons = new Dictionary<string, List<OTButtonElement>>();
	
	public string text = "";
	public Color textColor = Color.white;
	public Color textHoverColor = Color.yellow;
	public Color textActiveColor = Color.black;
	public OTElementAction clickAction;
	public OTElementAction enterAction;
	public OTElementAction exitAction;
	public bool activeState = false;
	public bool toggleActiveOnClick = false;
	public bool toggle = false;	
	public string clickSound = "";
	public string hoverSound = "";
	public string group = "";
		
	bool _activeState = false;
	OTSprite normalSprite = null;
	OTSprite hoverSprite = null;
	OTSprite activeSprite = null;
	TextMesh textMesh = null;
	OTTextSprite textSprite = null;
	
	public float alpha
	{
		get
		{
			if (normal!=null)
				return normal.alpha;
			else
				return 1;
		}
		set
		{
			if (normal!=null)
				normal.alpha = value;
			if (hover!=null)
				hover.alpha = value;
			if (active!=null)
				active.alpha = value;
			if (caption!=null)
				caption.alpha = value;			
		}
	}
	
	/// <summary>
	/// Gets reference to the normal sprite;
	/// </summary>
	public OTSprite normal
	{
		get
		{
			return normalSprite;
		}
	}
	
	/// <summary>
	/// Gets reference to the hover sprite;
	/// </summary>
	public OTSprite hover
	{
		get
		{
			return hoverSprite;
		}
	}
	/// <summary>
	/// Gets reference to the hover sprite;
	/// </summary>
	public OTTextSprite caption
	{
		get
		{
			return textSprite;
		}
	}
	
	/// <summary>
	/// Gets reference to the active sprite;
	/// </summary>
	new public OTSprite active
	{
		get
		{
			return activeSprite;
		}
	}
	
	protected override void SetDepth()
	{
		base.SetDepth();
				
		if (normalSprite!=null)
		{
			normalSprite.depth = depth;
			normalSprite.collisionDepth = depth;			
		}
		if (hoverSprite!=null)
		{
			hoverSprite.depth = depth-1;
			hoverSprite.collisionDepth = depth-1;
		}
		if (activeSprite!=null)
		{
			activeSprite.depth = depth-1;
			activeSprite.collisionDepth = depth;
		}
		if (textMesh!=null)
			textMesh.transform.position = transform.position + new Vector3(0,0,-2);
	}
	
	protected override bool Over()
	{
		if (normalSprite!=null && OT.Over(normalSprite))
			return true;
		if (hoverSprite!=null && OT.Over(hoverSprite))
			return true;
		if (activeSprite!=null && OT.Over(activeSprite))
			return true;
		return false;
	}
	
	new void Awake()
	{
		base.Awake();
		GetNormal();
		GetHover();
		GetCaption();
		GetActive();
		_activeState = activeState;
				
	}
		
	void GetNormal()
	{
		normalSprite = Child("normal") as OTSprite;
		InitSprite(normalSprite);
	}
	
	void GetHover()
	{
		hoverSprite = Child("hover") as OTSprite;
		if (hoverSprite!=null)
			InitSprite(hoverSprite);
	}
	
	void GetCaption()
	{
		Transform tr = FindChild("caption");
		if (tr!=null)
		{
			textSprite = tr.GetComponent<OTTextSprite>();
			if (textSprite==null)
				textMesh = tr.GetComponent<TextMesh>();
			
		}		
		SetTextColor(textColor);
	}
			
	void GetActive()
	{
		activeSprite = Child("active") as OTSprite;
		if (activeSprite!=null)
			InitSprite(activeSprite);
	}
	
	// Use this for initialization
	new void Start () {
		
		if (group!="")
		{
			if (!OTButtonElement.groupButtons.ContainsKey(group))
				OTButtonElement.groupButtons.Add(group, new List<OTButtonElement>());
			if (!OTButtonElement.groupButtons[group].Contains(this))
				OTButtonElement.groupButtons[group].Add(this);		
		}		
		
		base.Start();
											
		if (activeState && activeSprite!=null)
		{
			activeSprite.visible = true;
			if (normalSprite!=null)
				normalSprite.visible = false;
			SetTextColor(textActiveColor);
		}
		else		
		if (normalSprite!=null)
			normalSprite.visible = true;

		if (hoverSprite!=null)
			hoverSprite.visible = false;
			
	}
		
	void InitSprite(OTSprite sprite)
	{
		if (sprite==null)
			return;
		
		if (!sprite.registerInput)
			sprite.registerInput = true;

	}
		
	bool isOver = false;
	// Update is called once per frame
	new void Update () {
		
		base.Update();
		if (normalSprite!=null && !OT.ClippedShowing(normalSprite.gameObject))				
			return;
		
		if (!Application.isPlaying || OT.dirtyChecks)
		{
			if (normalSprite==null)
			{
				GetNormal();
				if (normalSprite == null)
					return;
			}			
			if (hoverSprite==null)
				GetHover();			
			if (textSprite==null && textMesh == null)
				GetCaption();
			if (activeSprite==null)
				GetActive();
			
			if (normalSprite!=null) 
				InitSprite(normalSprite);
			if (hoverSprite!=null) 
				InitSprite(hoverSprite);
			
			if (!Application.isPlaying)
			{				
				if (normalSprite!=null)
					normalSprite.renderer.enabled = !toggle;
				if (hoverSprite!=null)
					hoverSprite.renderer.enabled = toggle;
												
				if (normalSprite.renderer.enabled)
					SetTextColor(textColor);
				else
				if (hoverSprite.renderer.enabled)
					SetTextColor(textHoverColor);				
			}
										
		}
		
		bool over = Over();
		if (!isOver && over)
		{			
			if (hoverSprite!=null)
			{
				hoverSprite.visible = true;
				if (normalSprite!=null)
					normalSprite.visible = false;
			}
						
			if (textSprite!=null)
				textSprite.tintColor = textHoverColor;
			else
			if (textMesh!=null)
				textMesh.renderer.material.color = textHoverColor;
						
			if (hoverSound!="")
				new OTSound(hoverSound);
			
			if (onEnter!=null)
				onEnter(this);			
			
			Action(enterAction);			
			
			isOver = true;
		}
		else
		{
			if (isOver && !over)
			{				
				if (hoverSprite!=null)					
					hoverSprite.visible = false;	
				
				SetTextColor(textColor);
				if (activeSprite!=null)
				{
					if (activeState)
					{
						activeSprite.visible = true;
						SetTextColor(textActiveColor);
					}
					else
					{
						activeSprite.visible = false;
						if (normalSprite!=null)
							normalSprite.visible = true;
					}
				}	
				else
					if (normalSprite!=null)
						normalSprite.visible = true;				
		
				
				if (onExit!=null)
					onExit(this);
				
				Action(exitAction);		
				isOver = false;
			}
		}	

		if (over)
		{
			if (onStay!=null)
				onStay(this);
			
			MouseInput(normalSprite);			
		}
			
				
		if ((textSprite!=null || textMesh!=null) && text != GetText())
				SetText();
				
		if (_activeState!=activeState && activeSprite!=null)
		{
			_activeState = activeState;
			if (activeState)
			{
				activeSprite.visible = true;
				if (normalSprite!=null)
					normalSprite.visible = false;					
				SetTextColor(textActiveColor);
			}
			else
			{
				activeSprite.visible = false;
				if (normalSprite!=null)
					normalSprite.visible = true;					
				SetTextColor(textColor);
			}						
			
		}
						
		if (normalSprite!=null && normalSprite.position!=Vector2.zero)
			normalSprite.position =Vector2.zero;
		if (hoverSprite!=null && hoverSprite.position!=Vector2.zero)
			hoverSprite.position =Vector2.zero;
		if (activeSprite!=null && activeSprite.position!=Vector2.zero)
			activeSprite.position =Vector2.zero;
	}
	
	void SetTextColor(Color c)
	{
		if (textSprite!=null)
			textSprite.tintColor = c;
		else
		if (textMesh!=null)
		{
			if (Application.isPlaying)
				textMesh.renderer.material.color = c;				
			else
				textMesh.renderer.sharedMaterial.color = c;				
		}
	}
	
	string GetText()
	{
		if (textSprite!=null)
			return textSprite.text;
		else
		if (textMesh!=null)
			return textMesh.text;
		return "";
	}
	
	void SetText()
	{
		if (textSprite!=null)
		{
			textSprite.text = text;
			textSprite.ForceUpdate();
		}
		else
		if (textMesh!=null)
			textMesh.text = text;
	}
		
	void MouseInput(OTObject owner)
	{	
		if (!toggleActiveOnClick)
		{
			if (Input.GetMouseButton(0) && activeSprite!=null)
			{
				activeSprite.visible = true;
				if (hoverSprite!=null) 
					hoverSprite.visible = false;
				else
					if (normalSprite!=null)
						normalSprite.visible = false;
				SetTextColor(textActiveColor);
			}
			else
			if (Input.GetMouseButtonUp(0) && activeSprite!=null)
			{
				if (!activeState)
				{
					activeSprite.visible = false;
					if (hoverSprite!=null) 
						hoverSprite.visible = true;	
					else
						if (normalSprite!=null)
							normalSprite.visible = true;
					SetTextColor(textHoverColor);
				}				
			}
		}
		
		
		if (Input.GetMouseButtonDown(0))
		{
						
			if (clickSound!="")
				new OTSound(clickSound);
			
			
			if (toggleActiveOnClick && activeSprite!=null)
			{
				if (group!="" && activeState == false)
				{
					int b=0;
					while (b<OTButtonElement.groupButtons[group].Count)
					{
						if (OTButtonElement.groupButtons[group] == null)
							OTButtonElement.groupButtons[group].RemoveAt(b);
						else
						{
							OTButtonElement.groupButtons[group][b].activeState = false;
							b++;
						}
					}
				}
				
				activeState = !activeState;
				if (hoverSprite) hoverSprite.visible = false;
			}

			if (onClick!=null)
				onClick(this);
			
			Action(clickAction);
			
		}
	}
	
}


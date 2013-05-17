using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class OTTextInputElement : OTFormElement {
	
	public string _value = "This is the text";
	public string validChars = "";
	public string validCharRegex = "";
	public bool password = false;
	public string passwordChar = "*";
	public int max = 0;
	public Vector2 cursorOffset;
	public int clipLayer = 0;
	public int clipMargin = 0;
		
	public bool blinkingCursor = true;
	public float blinkSpeed = 0.2f;
	
			
	OTTextSprite textSprite = null;
	OTSprite focusSprite = null;
	OTSprite fillSprite = null;
	OTClippingAreaSprite boxSprite = null;
	OTSprite cursorSprite = null;
	IVector2 cursorPos = IVector2.zero;
	
	float blinkTime = 0;
	float blinkToggle = 0;
	
	string passValue = "";
	Vector2 bDelta;
			
	protected override void SetValue()
	{
		_value = stringValue;
		if (password)
			CreatePass();
		SetText();
		base.SetValue();
		SetCursor();
	}
		
	protected override string XMLValue ()
	{
		return "<![CDATA["+stringValue+"]]>";
	}
	
	protected override void Awake()
	{
		base.Awake();
		value_ = _value;
	}
	
	protected override void Initialize ()
	{		
		if (boxSprite!=null && textSprite!=null)
			cursorPos.x = textSprite.text.Length;
				
		base.Initialize ();
	}

	void SetText()
	{
		if (textSprite!=null)
		{
			if (password)
				textSprite.text = passValue;
			else				
				textSprite.text = stringValue;
			textSprite.ForceUpdate();			
			
			if (Application.isPlaying)
				SetCursor();			
		}
	}

	void SetCursor()
	{
		if (cursorSprite!=null)
		{			
			Vector3 cp = (Vector3)(textSprite.CursorPosition(cursorPos)+cursorOffset) + new Vector3(0,0,cursorSprite.transform.position.z);
			if (boxSprite!=null && clipLayer>0)
			{
				if (cp.x < boxSprite.worldRect.xMin + clipMargin + bDelta.x)
				{
					float dx = boxSprite.worldRect.xMin + clipMargin + bDelta.x - cp.x;
					textSprite.transform.position += new Vector3( dx,0,0);
					cp.x += dx;
				}
				else
				if (cp.x - cursorSprite.worldRect.width > boxSprite.worldRect.xMax - clipMargin - bDelta.x)
				{
					float dx = boxSprite.worldRect.xMax - clipMargin - bDelta.x - cp.x - cursorSprite.worldRect.width;
					textSprite.transform.position += new Vector3( dx,0,0);
					cp.x += dx;
				}
					
			}			
			cursorSprite.transform.position = cp;			
		}
	}
	
	void TextChanged(OTObject owner)
	{
		SetCursor();
	}
	
	protected override void GetObjects()
	{		
		base.GetObjects();
		
		if (textSprite == null)
		{
			textSprite = Sprite("text") as OTTextSprite;
			if (textSprite!=null)
			{
				textSprite.text = stringValue;
				textSprite.onObjectChanged += TextChanged;
				
			}
		}
		if (cursorSprite==null)
			cursorSprite = Sprite("cursor");
		if (fillSprite==null)
			fillSprite = Sprite("fill");
		if (focusSprite==null)
			focusSprite = Sprite("focus");
		
		if (boxSprite == null)
		{
			boxSprite = Sprite("box") as OTClippingAreaSprite;
			if (boxSprite!=null)
				boxSprite.registerInput = true;
		}
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();			
		
		CheckLength();					
		if (password)
		{
			CreatePass();
			SetText();
		}					
		
	}
		
	void CreatePass()
	{
		passValue = "";
		for (int i=0; i<stringValue.Length; i++)
			passValue += passwordChar;
	}
		
	
	void CheckLength()
	{
		if (max>0 && stringValue.Length>max)
			_value = stringValue.Substring(0,max);
	}
	
	protected override bool OverElement()
	{
		if (OT.Over(boxSprite))
		{
			int a=1;
		}
		
		
		return (OT.Over(boxSprite) || OT.Over(textSprite));
	}
	
	// Update is called once per frame
	new protected void Update () {				
		base.Update();	
		
		if (!Application.isPlaying && boxSprite!=null)
		{
			boxSprite.clipLayer = clipLayer;
			boxSprite.clipMargin = clipMargin;
			if (textSprite!=null) textSprite.gameObject.layer = clipLayer;
			if (fillSprite!=null) fillSprite.gameObject.layer = clipLayer;
			if (focusSprite!=null) focusSprite.gameObject.layer = clipLayer;
			if (cursorSprite!=null) cursorSprite.gameObject.layer = clipLayer;
		}
				
		if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			return;		
				
		if (_value!=stringValue || ( !Application.isPlaying  && (password && passValue.Length!=stringValue.Length) || (!password && passValue.Length>0)))
		{
			value_ = _value;
			CheckLength();					
			if (password)
			{
				if (passValue.Length!=stringValue.Length)
					CreatePass();
			}
			else
				passValue = "";
			SetText();
		}
		
		if (Application.isPlaying)
		{						
			if (cursorSprite!=null)
			{
				if (blinkingCursor)
				{
					blinkTime+=Time.deltaTime;					
					if (blinkTime>blinkSpeed)
						blinkTime = blinkSpeed;
					cursorSprite.alpha = OTEasing.CircIn.ease(blinkTime,1-blinkToggle, (blinkToggle==0)?-1:1, blinkSpeed);
					if (blinkTime == blinkSpeed)
					{
						blinkTime = 0;
						blinkToggle = 1-blinkToggle;
					}
				}
			}						
			if (gotFocus)
				HandleInput();						
		}	
		else
			SetCursor();
	}	
	
	string ValidChar(string s)
	{
		if (validCharRegex!="")
		{
			Regex r = new Regex(@validCharRegex);
			if (r.Match(s).Success)
				return s;
			return "";			
		}
		if (validChars=="" || validChars.IndexOf(s) >= 0)
			return s;		
		return "";
	}

	float keyDelay = 0.05f;
	float keyDelayFirst = 0.15f;
	float keyTime = 0;
	bool keyFirst = true;
	void HandleInput()
	{						
		if (Input.inputString!="")
		{
			if (Input.inputString == "\b")
			{				
				if (stringValue.Length>0 && cursorPos.x > 0)
				{
					if (cursorPos.x == stringValue.Length)
					{
						cursorPos.x--;
						value = stringValue.Substring(0,stringValue.Length-1);			
					}
					else
					{
						string start = "";
						string end = stringValue.Substring(cursorPos.x, stringValue.Length-cursorPos.x);
						if (cursorPos.x > 1)
							start = stringValue.Substring(0, cursorPos.x-1);							
						cursorPos.x--;
						value = start + end;
					}
				}
			}
			else			
			if (Input.inputString == "\n")
			{
			}
			else
			{
				if (max <= 0 || (max > 0 && _value.Length<max))
					for (int s=0; s<Input.inputString.Length; s++)
					{
						string cs = ValidChar(Input.inputString.Substring(s,1));
						if (cs!="")
						{
							if (cursorPos.x < stringValue.Length)
							{
								string start = stringValue.Substring(0,cursorPos.x);
								string end = stringValue.Substring(cursorPos.x, stringValue.Length-cursorPos.x);
								cursorPos.x++;
								value = start + cs + end;							
							}
							else
							{
								cursorPos.x++;
								value += cs;
							}
						}
						if (max>0 && _value.Length == max)
							break;
					}
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Delete))
			{
				if (keyTime == 0)
				{					
					if (Input.GetKey(KeyCode.LeftArrow) && cursorPos.x>0)
					{
						cursorPos.x--;
						SetCursor();
					}
					else
					if (Input.GetKey(KeyCode.RightArrow) && cursorPos.x< textSprite.text.Length)
					{
						cursorPos.x++;
						SetCursor();
					}	
					else
					if (Input.GetKey(KeyCode.Delete))
					{
						if (cursorPos.x < stringValue.Length)
						{
							string start = stringValue.Substring(0,cursorPos.x);
							string end = "";
							if (cursorPos.x < stringValue.Length-1)
							  end = stringValue.Substring(cursorPos.x+1, stringValue.Length-cursorPos.x-1);
							value = start + end;
						}
					}
				}
				keyTime+=Time.deltaTime;
				if (keyTime>(keyFirst?keyDelayFirst:keyDelay))
				{
					keyTime = 0;
					keyFirst = false;
				}
			}
			else
			{
				
				if (Input.GetKeyDown(KeyCode.Home))
				{
					cursorPos.x = 0;
					SetCursor();
				}
				else
				if (Input.GetKeyDown(KeyCode.End))
				{
					cursorPos.x = stringValue.Length;
					SetCursor();
				}
				
				keyTime = 0;
				keyFirst = true;
			}
			
		}
	}
	
	public override void Repaint()
	{
		base.Repaint();
		if (cursorSprite!=null)			
		{
			cursorSprite.visible = gotFocus;
			if (textSprite!=null)
				cursorSprite.depth = textSprite.depth-1;
		}
		if (textSprite!=null)
			textSprite.tintColor = (gotFocus)?form.colorActiveText:form.colorNormalText;
	}	
	
}

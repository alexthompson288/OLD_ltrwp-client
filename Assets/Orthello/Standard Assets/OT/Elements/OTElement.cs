using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
/// <summary>
/// Base Orthello Element Class
/// </summary>
/// <description>
/// Elements are items that contain base game/application tasks/functionality and
/// that use one or more Orthello object for it.
/// For example : Lines, Menus, Buttons etc etc
/// </description>
public class OTElement : MonoBehaviour {
			
	public int depth = 0;
	
	int _depth = 0;
	
	protected bool initialized = false;	
	public static OTElement current = null;
	public static Dictionary<string,OTElement> lookupElements = new Dictionary<string, OTElement>();
	
    public delegate void ElementDelegate(OTElement element);
	
	public Bounds bounds
	{
		get
		{
			return OT.GetBounds(gameObject);
		}
	}
	
	/// <summary>
	/// Sets the layer of this element (and children)
	/// </summary>
	public int layer
	{
		get
		{
			return gameObject.layer;
		}
		set
		{
			gameObject.layer = value;
			OTHelper.ChildrenSetLayer(gameObject,value);
		}
	}
	
	/// <summary>
	/// Lookup an element by its 'unique' name
	/// </summary>
	public static OTElement Lookup(string name)
	{
		string lName = name.ToLower();
		if (lookupElements.ContainsKey(lName))
			return lookupElements[lName];
		return null;
	}
							
	public virtual void _IMsg(string cmd)
	{
	}
	
	/// <summary>
	/// Lookups a button element
	/// </summary>
	public static OTButtonElement Button(string name)
	{
		return (OTButtonElement)Lookup(name);
	}
		
	/// <summary>
	/// Gets an orthello child object that starts with the specified name.
	/// </summary>
	public OTObject Child(string name)
	{
		return OT.FindChild(this.gameObject,name);
	}	
	
	
	public OTElement ChildElement(string name)
	{
		for (int g=0; g<gameObject.transform.childCount; g++)
		{
			Transform tr = gameObject.transform.GetChild(g);
			if (tr.gameObject.name.IndexOf(name)==0)
				return tr.gameObject.GetComponent<OTElement>();
		}
		return null;
	}
	
	
	/// <summary>
	/// Gets an orthello child sprite that starts with the specified name.
	/// </summary>
	public OTSprite Sprite(string name)
	{
		return Child(name) as OTSprite;
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="OTElement"/> is visible.
	/// </summary>
	/// <value>
	/// <c>true</c> if visible; otherwise, <c>false</c>.
	/// </value>
	public bool visible
	{
		get
		{
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5			
			return gameObject.active;
#else
			return gameObject.activeSelf;			
#endif
		}
		set
		{
			if (value == false && visible)
				Hide();
			else
			if (value == true && !visible)
				Show();
		}
	}
		
	/// <summary>
	/// Gets a value indicating whether the mouse is over this element
	/// </summary>
	/// <value>
	/// <c>true</c> if over; otherwise, <c>false</c>.
	/// </value>
	public bool over
	{
		get
		{
			return Over();
		}
	}
	
	/// <summary>
	/// Rebuilds this element
	/// </summary>
	public void Rebuild()
	{
		Update();
	}
			
	protected virtual bool Over()
	{	
		for (int i=0; i<transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			OTObject o = child.GetComponent<OTObject>();
			if (o!=null && OT.Over(o))
				return true;
			OTElement el = child.GetComponent<OTElement>();
			if (el!=null && el.Over())
				return true;			
		}
		return false;		
	}

	void _Register()
	{
		string lName = name.ToLower();
		if (lookupElements.ContainsKey(lName))
		{
			if (lookupElements[lName]==null)
				lookupElements[lName]=this;
					
		}
		else
		 lookupElements.Add(lName,this);							
	}
		
	protected virtual void Awake()
	{
		_depth = depth;
		_Register();
	}
	
	protected virtual void GetObjects()
	{
	}
		
	// Use this for initialization
	protected virtual void Start () {	
		SetDepth();
		GetObjects();
	}
	
	protected virtual void SetDepth()
	{
		_depth = depth;		
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, depth);
	}
	
	protected virtual void Initialize()
	{
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		if (Application.isPlaying && !initialized)
		{
			Initialize();
			initialized = true;
		}
		
		if (_depth!=depth || transform.localPosition.z != depth)
			SetDepth();				
		if (!Application.isPlaying)
			GetObjects();			
	}
	
	protected Transform FindChild(string name)
	{
		for (int c=0; c<transform.childCount; c++)
		{
			Transform tr = transform.GetChild(c);
			if (tr.name == name || tr.name.IndexOf(name+"-")==0)
				return tr;
		}
		return null;
	}
	
	public virtual void Hide()
	{
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5					
		gameObject.SetActiveRecursively(false);
#else
		gameObject.SetActive(false);
#endif
	}
	
	public virtual void Show()
	{
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5			
		gameObject.SetActiveRecursively(true);
#else
		gameObject.SetActive(true);
#endif
	}	
	
	protected void Action(OTElementAction action)
	{
		if (!Application.isPlaying) 
			return;
				
		if (action.message!="")
		{
			current = this;
			
			Component component = action.component;
			if (action.component==null) 
				component = this;
			component.SendMessage(action.message,
				(action.value!="")?action.value:null, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnLevelWasLoaded()
	{
		lookupElements.Clear();
	}
	
}

[System.Serializable]
/// <summary>
/// OT element sendmessage action.
/// </summary>
public class OTElementAction
{
	public Component component;
	public string message;	
	public string value;		
}
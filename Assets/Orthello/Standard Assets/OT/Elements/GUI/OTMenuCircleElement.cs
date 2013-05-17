using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTMenuCircleElement : OTElement {
			
	public enum MenuState { Closed, Opening, Open, Closing };
	public enum OpenCommand { OnEnter, OnClick, Manual };
	public enum CloseCommand { OnExit, OnItemClick, OnClick, Manual };
	
	public float radius = 100;
	public OpenCommand openingOn = OpenCommand.OnEnter;
	public OTEasing.EaseType openEase = OTEasing.EaseType.Linear;
	public float openDuration = .5f;
	
	public CloseCommand closingOn = CloseCommand.OnExit;
	public OTEasing.EaseType closeEase = OTEasing.EaseType.Linear;
	public float closeDuration = .5f;
	public float exitDelay = 0.5f;
	
	public Vector2 itemOffset = Vector2.zero;
	public float fromRotation = 0;
	public float toRotation = 0;
	
	public bool fadeItems = true;
	public OTEasing.EaseType fadeInEase = OTEasing.EaseType.Linear;
	public OTEasing.EaseType fadeOutEase = OTEasing.EaseType.Linear;
	
	public int previewProgress = 0;
	
	public float progress
	{
		get
		{
			return _progress;
		}
	}
	
		
	public int[] activeItems
	{
		get
		{
			return _activeItems;
		}
		set
		{
			_activeItems = value;

			for (int i=0; i < items.Count; i++)
			{
				//GameObject go = items[i];
				OTButtonElement el = items[i].GetComponent<OTButtonElement>();
				if (el!=null) el.activeState = false;
			}
			
			for (int i=0; i < activeItems.Length; i++)
			{
				if (activeItems[i]<items.Count)
				{
					//GameObject go = items[activeItems[i]];
					OTButtonElement el = items[activeItems[i]].GetComponent<OTButtonElement>();
					if (el!=null) 
						el.activeState = true;
				}
			}
			
		}
	}
	
	public MenuState state
	{
		get
		{
			return _state;			
		}
		set
		{
			_state = value;
		}
	}
	
	protected override void SetDepth()
	{
		base.SetDepth();
	}
	
	public bool isOpen
	{
		get
		{
			return (state == MenuState.Open || state == MenuState.Opening);
		}
	}
	
	public bool isClosed
	{
		get
		{
			return (state == MenuState.Closed || state == MenuState.Closing);
		}
	}
	
	
	float _progress = 0;
	List<GameObject> items = new List<GameObject>();
	int[] _activeItems = new int[]{};
	MenuState _state = MenuState.Closed;
	float _radius = 100;

	GameObject center;
	OTButtonElement centerButton;
	
	
	float waitExit = 0;
	void ClickMenuElementEvent(OTElement element)
	{
		if (isOpen && closingOn == CloseCommand.OnClick)
			Close();
		else
		if (openingOn == OpenCommand.OnClick)
		{
			waitExit = 0;
			if (!isOpen)
				Open();
		}
	}
	void EnterMenuElementEvent(OTElement element)
	{		
		waitExit = 0;
		if (openingOn == OpenCommand.OnEnter)			
		{
			if (!isOpen)
				Open();
		}
	}
	void StayMenuItemElementEvent(OTElement element)
	{		
		waitExit = 0;
	}
	void ExitMenuElementEvent(OTElement element)
	{
		waitExit = Time.time;		
	}
	
	private static int CompareByName(GameObject x, GameObject y)
    {
		return string.Compare(x.name,y.name);
	}
	
	public void Load()
	{		
		items.Clear();
		for (int i=0; i<transform.childCount; i++)
		{
			GameObject go = transform.GetChild(i).gameObject;
			go.transform.position = transform.position;
			if (go.name.IndexOf("item")==0)
				items.Add(transform.GetChild(i).gameObject);
			if (go.name.IndexOf("center")==0)
			{
				center = go;
				centerButton = center.GetComponent<OTButtonElement>();
			}			
		}
		
		items.Sort(CompareByName);
			
		if (centerButton!=null)
		{
			if (openingOn == OpenCommand.OnClick || closingOn == CloseCommand.OnClick)
				centerButton.onClick = ClickMenuElementEvent;
			if (openingOn == OpenCommand.OnEnter)
			{
				centerButton.onStay = StayMenuItemElementEvent;
				centerButton.onEnter = EnterMenuElementEvent;
				for (int i =0; i<items.Count; i++)
				{
					OTButtonElement b = items[i].GetComponent<OTButtonElement>();
					if (b!=null)
						b.onStay = StayMenuItemElementEvent;
				}
			}
			if (closingOn == CloseCommand.OnExit)
			{
				centerButton.onExit = ExitMenuElementEvent;
				for (int i =0; i<items.Count; i++)
				{
					OTButtonElement b = items[i].GetComponent<OTButtonElement>();
					if (b!=null)
						b.onExit = ExitMenuElementEvent;
				}
			}
		}
				
	}
	
	new void Awake()
	{
		base.Awake();
		Load();
		previewProgress = 0;
		_progress = 0;
		_radius = radius;
	}
		
	// Use this for initialization
	new void Start () {
		base.Start();
		_progress = 0;
		SetState(_progress);
	}

	public void SetState(float pos)
	{				
		float angle = -1*(360/items.Count);
		for (int i=0; i < items.Count; i++)
		{
			GameObject go = items[i];
			if (go==null)
			{
				Load();
				return;
			}
						
			float rot = fromRotation + (toRotation-fromRotation)*_progress;
						
			Vector2 po = new Vector2(0,(float)radius * pos);
			Matrix4x4 m = new Matrix4x4();
			m.SetTRS(itemOffset,Quaternion.Euler(0,0,angle*i+rot),Vector2.one);
			po = m.MultiplyPoint3x4(po);
			go.transform.position = transform.position + (Vector3)po;
			
				
			if (fadeItems)
			{
				float alpha = 0;
				if (isOpen)
					alpha = OTEasing.Ease(fadeInEase).ease(_progress,0,1,1);
				else
					alpha = OTEasing.Ease(fadeOutEase).ease(_progress,0,1,1);
									
				OTSprite s = go.GetComponent<OTSprite>();
				if (s!=null)
					s.alpha = _progress;
				else
				{
					OTButtonElement b = go.GetComponent<OTButtonElement>();
					if (b!=null)
						b.alpha = alpha;
				}
			}							
		}
	}
	
	void SetState()
	{
		SetState(_progress);
	}
	
	public void Open()
	{
		if (state != MenuState.Open)
			state = MenuState.Opening;	
	}
	
	public void Close()
	{
		if (state != MenuState.Closed)
			state = MenuState.Closing;	
	}
			
	void SetEaseState()
	{		
		if (!Application.isPlaying)
			SetState(OTEasing.Ease(openEase).ease(_progress,0,1,1));
		else		
		if (state == MenuState.Opening)
			SetState(OTEasing.Ease(openEase).ease(_progress,0,1,1));
		else
		if (state == MenuState.Closing)
			SetState(OTEasing.Ease(closeEase).ease(_progress,0,1,1));
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		
		if (waitExit>0 && (Time.time - waitExit) > exitDelay && !isClosed)
		{
			waitExit = 0;
			Close();
		}
		
		if (!Application.isPlaying)
		{
			if (transform.childCount!=items.Count)
				Load();
			
			previewProgress = Mathf.Clamp(previewProgress,0,99);			
			if ((float)previewProgress/100f!=_progress || radius!=_radius)
			{
				_progress = (float)previewProgress/100f;
				if (_progress>1) _progress = 1;
				_radius = radius;
				SetEaseState();
			}
		}
		else
		{
			switch(state)
			{
				case MenuState.Closed:
					if (_progress>0) 
					{
						_progress = 0;
						SetState();					
					}
				break;
				case MenuState.Open:
					if (_progress<1) 
					{
						_progress = 1;
						SetState();
					}
				break;
				case MenuState.Closing:
					_progress -= Time.deltaTime / closeDuration;
					if (_progress<=0)
					{						
						_state = MenuState.Closed;
						_progress = 0;
						SetState();
					}
					else
						SetEaseState();										
				break;
				case MenuState.Opening:
					_progress += Time.deltaTime / openDuration;
					if (_progress>=1)
					{
						_state = MenuState.Open;
						_progress = 1;
						SetState();
					}
					else
						SetEaseState();										
				break;
			}
		}		
	}
	
	protected override bool Over()	
	{
		if (Vector2.Distance((Vector2)OT.view.mouseWorldPosition,(Vector2)transform.position)
			< radius * 1.25f )
			return true;
		else
			return 
				base.Over();
			
	}
			
}

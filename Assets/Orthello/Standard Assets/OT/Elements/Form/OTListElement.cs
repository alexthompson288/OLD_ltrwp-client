using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTListElement : OTFormElement {
			
	public string _value = "";
	public int clipLayer = 0;
	public int clipMargin = 2;
	public string[] _items;
	public TextAsset _itemsText;
	public bool _multiSelect = false;
	
	public OTElementAction selectAction;
	public OTElement.ElementDelegate onSelect = null;
	public OTElementAction deSelectAction;
	public OTElement.ElementDelegate onDeSelect = null;
	
	int _selectedItem = -1;
	List<int> _selectedItems = new List<int>();
	Dictionary<string,int> itemByName = new Dictionary<string, int>();
	
	OTClippingAreaSprite boxSprite;
	OTSprite fillSprite = null;
	OTSliderElement slider;
	OTButtonElement item;	
	Transform itemsContainer; 
	
	public bool multiSelect
	{
		get
		{
			return _multiSelect;
		}
		set
		{
			_multiSelect = value;
			
			for (int i=0; i<itemElements.Count; i++)
				itemElements[i].group = (value)?"":name;
			
			selectedItemIndexes = new int[]{};
			if (value)
			{
				if (selectedItemIndex!=-1)
					selectedItemIndexes = new int[]{ selectedItemIndex };
			}
						
			Repaint();
		}
	}
	
	public int selectedItemIndex
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			_selectedItem = value;
			Repaint();			
		}
	}
	
	public string selectedItem
	{
		get
		{
			if (_selectedItem>=0 && _items.Length>_selectedItem)
				return _items[_selectedItem];
			else
				return "";
		}
		set
		{
			string v = value.ToLower().Trim();
			if (itemByName.ContainsKey(v))
			    selectedItemIndex = itemByName[v];
		}
	}

	/// <summary>
	/// Gets or sets the selected items
	/// </summary>
	public int[] selectedItemIndexes
	{
		get
		{
			_selectedItems.Sort();
			return _selectedItems.ToArray();
		}
		set
		{
			_selectedItems = new List<int>(value);
			Repaint();
		}
	}
	
	/// <summary>
	/// Gets or sets the selected items
	/// </summary>
	public string[] selectedItems
	{
		get
		{
			List<string> s = new List<string>();
			_selectedItems.Sort();
			for (int i=0; i<_selectedItems.Count; i++)
			{
				if (_selectedItems[i]>=0 && _items.Length>_selectedItems[i])
					s.Add(_items[_selectedItems[i]]);
			}				
			return s.ToArray();
		}
		set
		{
			_selectedItems.Clear();
			for (int i=0; i<value.Length; i++)
			{
				string v = value[i].ToLower().Trim();
				if (itemByName.ContainsKey(v))
				  _selectedItems.Add(itemByName[v]);				
			}
			Repaint();
		}
	}
	
	/// <summary>
	/// Gets or sets the items using a array of strings
	/// </summary>
	public string[] items
	{
		get
		{
			return _items;
		}
		set
		{
			_items = value;
			for (int i=0; i<items.Length; i++)
				_items[i] = _items[i].Trim();
			loadedItems = 0;
			SetLookup();
			Repaint();
		}
	}
	
	/// <summary>
	/// Gets or sets the items using a text asset
	/// </summary>
	public TextAsset itemsText
	{
		get
		{
			return _itemsText;
		}
		set
		{
			_itemsText = value;
			if (value!=null)
				items = _itemsText.text.Split('\n');
			else
				items = new string[]{};
		}
	}

	void SetLookup()
	{
		itemByName.Clear();
		for (int i=0; i<_items.Length; i++)
			itemByName.Add(_items[i].ToLower().Trim(), i);
	}
	
	int topIndex = 0;
	void RepaintItems(int index)
	{
		topIndex = index;
		for (int i=0; i<itemElements.Count; i++)
		{
			if (i+index<items.Length)
			{
				itemElements[i].text = items[i+index];
				itemElements[i].visible = true;
				
				itemElements[i].activeState = false;
				if (multiSelect)
				{
					if (_selectedItems.Contains(i+index))
						itemElements[i].activeState = true;
				}
				else
				{
					if (selectedItemIndex == i+index)
						itemElements[i].activeState = true;
				}
				
			}
			else
				itemElements[i].visible = false;
		}								
	}
	
	float slidingDelta = 0;
	float slidingTotal = 0;
	void SliderChanged(OTElement element)
	{
		float 	sf = ((100f-slider.floatValue)/100f);
		float 	vp = sf * slidingTotal;
		int 	vi = (int)Mathf.Floor(vp / slidingDelta);
		float 	dy = vp - (vi * slidingDelta);		
		
		if (sf==1)
			dy = slidingDelta-2;
			
		itemsContainer.transform.position = transform.position + new Vector3(0,dy,0);
				
		int indexes = (int)Mathf.Floor(slidingTotal / (item.normal.worldRect.height+2));
		int index =   (int)Mathf.Floor((float)indexes * sf);
		
		RepaintItems(index);
	}
	
	protected override void Initialize ()
	{		
	
		if (item!=null)
			item.visible = false;
		
		if (_itemsText!=null && _itemsText.text!="")
			_items = _itemsText.text.Split('\n');
		
		if (slider!=null)
		{
			slider.value = 100;
			slider.visible = false;
			slider.onSlide = SliderChanged;
		}
				
		itemsContainer = transform.FindChild("items");
		if (transform.FindChild("items")==null)
		{
			itemsContainer = new GameObject().transform;
			itemsContainer.transform.position = transform.position;
			itemsContainer.parent = transform;				
			itemsContainer.transform.localScale = Vector3.one;
			itemsContainer.name = "items";
		}		
		base.Initialize ();
		
		if (Application.isPlaying)
			CheckItemElements();
		
		SetLookup();
	}
	
	
	protected override void SetValue()
	{ 
		_value = stringValue;
		
		if (multiSelect)
		{
			string[] vals = stringValue.Split(',');
			int[] ivs = new int[vals.Length];
			for (int i=0; i<vals.Length; i++)
				ivs[i] = System.Convert.ToInt16(vals[i]);
			_selectedItems = new List<int>(ivs);
		}
		else
			_selectedItem = intValue;
		
		base.SetValue();
	}
			
	protected override void GetObjects()
	{		
		if (boxSprite == null)
			boxSprite = Sprite("box") as OTClippingAreaSprite;
		
		if (boxSprite == null)
		{
			Debug.LogWarning("OTListElement : No box sprite found!");
			return;
		}
		
		if (fillSprite==null)
			fillSprite = Sprite("fill");
		
		if (slider == null)
		{
			slider = ChildElement("slider") as OTSliderElement;	
			if (Application.isPlaying && slider == null && form.verticalSlider!=null)
			{
				slider = (GameObject.Instantiate(form.verticalSlider.gameObject) as GameObject).GetComponent<OTSliderElement>();
									
				slider.transform.localScale = new Vector3(form.verticalSliderWidth,boxSprite.worldRect.height,1);
				slider.name = "slider";

				slider.transform.position = 
					new Vector3(boxSprite.worldRect.xMax + (slider.Sprite("bar").worldRect.width/2),
						boxSprite.worldRect.yMax - (slider.Sprite("bar").worldRect.height/2),transform.position.z);						
				slider.transform.parent = transform;
				slider.depth = -10;
				
				slider.visible = true;
			}
			
			
			if (slider!=null)
				slider.IsChildControlOf(this);
			
		}
		if (item == null)			
			item = ChildElement("item") as OTButtonElement;
				
		base.GetObjects();
	}
	
	new protected void Awake()
	{
		value_ = _value;
	}
			
	protected override bool OverElement()
	{
		if (OT.Over(boxSprite))
			return true;
		if (slider!=null && slider.visible && slider.overElement)
			return true;
		return false;
	}
	
	List<OTButtonElement> itemElements = new List<OTButtonElement>();
	
	
	int loadedItems = 0;
		
	void CheckItemElements()
	{
		if (item==null)
			return;

		if (itemElements.Count>0)
			return;
		
		int ci=0;
		float rh = item.normal.worldRect.height;			
		while(true)
		{			
			if (itemElements.Count==loadedItems)
			{
				OT.RuntimeCreationMode();
				itemElements.Add((GameObject.Instantiate(item.gameObject) as GameObject).GetComponent<OTButtonElement>());
			}					
			
			OTButtonElement b = itemElements[loadedItems];
			b.transform.parent = itemsContainer;
			b.transform.localScale = Vector3.one;
			
			b.transform.position = item.transform.position - new Vector3(0,rh + 2,0) * loadedItems;
			b.name = "list-item-"+loadedItems;
			b.text = items[loadedItems];
			if (!multiSelect)
				b.group = name;
			b.visible = true;
			if (loadedItems>=items.Length)
				b.visible = false;		
			
			if (boxSprite!=null && clipLayer!=0)
				b.layer = clipLayer;
			
			b.onClick = ItemClick;
			loadedItems++;
			
			if (loadedItems==items.Length || !boxSprite.worldRect.Contains((Vector2)b.transform.position + new Vector2(0, rh/2)))
			{
				if (loadedItems<items.Length)
					slidingDelta = (boxSprite.worldRect.yMin + 2 - b.transform.position.y - (rh/2));
			
				if (slider!=null)
					slider.value = 100;
				
				break;			
			}						
		}		
		
		float ih = items.Length * (rh + 2);
		if ( ih > boxSprite.worldRect.height - clipMargin)
		{
			if (slider!=null)
			{
				slider.visible = true;
				slider.SetView(boxSprite.worldRect.height - clipMargin, ih);
				slidingTotal = ih - boxSprite.worldRect.height - clipMargin;
			}
		}
		if (itemElements.Count>0)
			itemElements[0].toggle = true;	
		
		RepaintItems(0);
		
	}	
	
	string CreateValue()
	{
		if (!multiSelect)
			return ""+selectedItemIndex;
		else
		{
			string v = "";
			for (int i=0; i<_selectedItems.Count; i++)
			{
				v += _selectedItems[i];
				if (i<_selectedItems.Count-1)
					v+=",";
			}
			return v;
		}		
	}

	void ItemClick(OTElement element)
	{
		OTButtonElement btn = element as OTButtonElement;
		if (btn!=null)
		{
			int index = topIndex + itemElements.IndexOf(btn);						
			if (multiSelect)
			{			
				if (btn.activeState && !_selectedItems.Contains(index))
					_selectedItems.Add(index);
				else
				if (!btn.activeState && _selectedItems.Contains(index))
					_selectedItems.Remove(index);
			}						
		  	_selectedItem = index;

			if (!multiSelect && !btn.activeState)
				_selectedItem = -1;
						
			_value = CreateValue();			
			value_ = _value;
									
			if (btn.activeState)
			{
				Action(selectAction);			
				if (onSelect!=null)
					onSelect(this);
			}
			else
			{
				Action(deSelectAction);			
				if (onDeSelect!=null)
					onDeSelect(this);				
			}
		}			
		Focus();
	}
	
	protected override string XMLValue ()
	{
		return stringValue;
	}
		
	public override void Repaint()
	{	
		if (Application.isPlaying && initialized)
			CheckItemElements();

		// paint elements
		for (int i=0; i<itemElements.Count; i++)
		{
			if (topIndex + i == selectedItemIndex || (multiSelect && _selectedItems.Contains(i + topIndex)))
				itemElements[i].activeState = true;
			else
				itemElements[i].activeState = false;
		}				
		
		
		base.Repaint();
	}	
	
	new protected void Update()
	{
		
		if (!Application.isPlaying)
		{
			if (boxSprite!=null)
			{
				boxSprite.clipLayer = clipLayer;
				boxSprite.clipMargin = clipMargin;
			}
			
			if (fillSprite!=null)
				fillSprite.gameObject.layer = clipLayer;
			
			if (item!=null)
			{
				item.gameObject.layer = clipLayer;
				OTHelper.ChildrenSetLayer(item.gameObject,clipLayer);
			}
						
			if (stringValue!=_value)
				value = _value;
		}
		else
		{
			if (gotFocus)
				slider.value = slider.floatValue + (Input.GetAxis("Mouse ScrollWheel")*10);										
		}
		base.Update();
	}
	
		
}

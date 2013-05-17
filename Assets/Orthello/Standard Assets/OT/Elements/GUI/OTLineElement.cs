using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Create a line with (gradient) sprites
/// </summary>
public class OTLineElement : OTElement {
	
	/// <summary>
	/// The width of this line
	/// </summary>
	public float	width;
	/// <summary>
	/// The width of this line
	/// </summary>
	public float	height = 0;
	/// <summary>
	/// The color tint of the line
	/// </summary>
	public Color   	color;
	public bool useTint = true;
	/// <summary>
	/// The alpha value (transparency) of the line
	/// </summary>
	public float	alpha;
	
	/// <summary>
	/// Sets starting location of the line
	/// </summary>
	public Vector2 	start
	{
		get
		{
			return transform.localPosition;
		}
		set
		{
			transform.localPosition = new Vector3(value.x,value.y,depth);
			for (int s=0; s<sprites.Count; s++)
				sprites[s].position = Vector2.zero;				
			height = Vector2.Distance(value,_end);
			for (int s=0; s<sprites.Count; s++)
			{
				sprites[s].size = new Vector2(width* widthFactors[s],height);
				sprites[s].RotateTowards(_end);
			}
		}
	}
	
	/// <summary>
	/// Sets ending location of the line
	/// </summary>
	public Vector2 	end
	{
		get
		{
			Vector2 pe = new Vector2(0,1);
			Matrix4x4 m = new Matrix4x4();
			m.SetTRS(Vector3.zero,Quaternion.Euler(0,0,firstSprite.rotation),new Vector3(1,1,1));
			_end =  start + (Vector2)transform.worldToLocalMatrix.MultiplyPoint3x4((Vector2)(m.MultiplyPoint3x4(pe) * firstSprite.size.y));
						
			return _end;	
		}
		set
		{
			Vector2 npe = transform.localToWorldMatrix.MultiplyPoint3x4(value);
			if (transform.parent!=null)
				npe = transform.parent.localToWorldMatrix.MultiplyPoint3x4(value);
									
			Vector2 npee = transform.worldToLocalMatrix.MultiplyPoint3x4(npe);
			
			height = Vector2.Distance(npe,transform.position);
			for (int s=0; s<sprites.Count; s++)
			{
				sprites[s].size = new Vector2(width*widthFactors[s],height);
				sprites[s].RotateTowards(npee);
			}
			_end = end;
		}
	}
	
	
	
	Color _color;
	float _alpha;
	float _width;
	Vector2 _end;
	

	List<OTSprite> sprites = new List<OTSprite>();
	List<float> widthFactors = new List<float>();
	
	OTSprite firstSprite = null;
		
	void GetSprites()
	{
		sprites.Clear();
		for (int c = 0; c < transform.childCount; c++)
		{			
			OTSprite sprite = transform.GetChild(c).GetComponent<OTSprite>();
			if (sprite!=null) 
			{
				if (firstSprite == null)
				{
					firstSprite = sprite;
					sprite.transform.localPosition = Vector3.zero;
				}
								
				if (sprite.pivot!=OTObject.Pivot.Bottom)
					sprite.pivot = OTObject.Pivot.Bottom;
				
				if (sprite!=firstSprite)
				{
					sprite.transform.localPosition = Vector3.zero;
					sprite.rotation = firstSprite.rotation;
				}								
				
				sprites.Add(sprite);
				widthFactors.Add(sprite.size.x / width);				
				
			}
		}
	}
	
	new void Awake()
	{		
		base.Awake();
		_color = color;
		_alpha = alpha;
		_width = width;
		GetSprites();			
	}
		
	// Use this for initialization
	new void Start () {				
		base.Start();
		if (Application.isPlaying)
			OT.RuntimeCreationMode();
		SetDisplay();
	}

	void SetDisplay()
	{
		for (int s=0; s<sprites.Count; s++)
		{
			if (useTint)
				sprites[s].tintColor = color;
			sprites[s].alpha = alpha;
			sprites[s].size = new Vector2(width * widthFactors[s],sprites[s].size.y);
#if UNITY_EDITOR
				if (!Application.isPlaying)
					UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(sprites[s]);
#endif											
		}
		_alpha = alpha;
		_width = width;
		_color = color;
	}
	
	// Update is called once per frame
	new public void Update () {
		
		base.Update();
		if (_alpha!=alpha || color!=_color || width!=_width)
			SetDisplay();
		
		if (!Application.isPlaying)
		{
			if (height == 0 && sprites.Count > 0)
				height = sprites[0].size.y;
			if (transform.childCount != sprites.Count || widthFactors.Count!=sprites.Count)
				GetSprites();
			else
			{
				for (int i=1; i<sprites.Count; i++)
					sprites[i].rotation = firstSprite.rotation;
			}
		}
							
		if (sprites.Count>0)				
		{
			if (height!=sprites[0].size.y)
			{
				for (int s=0; s<sprites.Count; s++)
					sprites[s].size = new Vector2(width * widthFactors[s],height);
			}
			
			if (!sprites[0].visible)
				for (int s=0; s<sprites.Count; s++)
					sprites[s].visible = true;
						
		}
		
		if (!transform.localRotation.Equals(Quaternion.identity))
		{
			for (int s=0; s<sprites.Count; s++)
			{
				sprites[s].rotation += (OT.world == OT.World.WorldSide2D)?transform.localRotation.eulerAngles.z:transform.localRotation.eulerAngles.y;
				sprites[s].ForceUpdate();
			}
			transform.localRotation = Quaternion.identity;
		}
		
		
	}
}

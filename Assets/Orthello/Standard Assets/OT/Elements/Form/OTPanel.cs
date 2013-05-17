using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTPanel : OTElement {
	
	public enum Alignment { Top, Left, Bottom, Right, Center, None };
	public Alignment align = Alignment.None;
	public bool stretch = true;		
	public OTMargins margins;
	public OTMargins innerMargins;
	public Alignment alignChildren = Alignment.None;
	public Vector2 childSpacing;
	public int clipLayer = 0;
	public int clipMargin = 0;
	public OTForm sliderForm = null;
	
	static List<OTPanel> rootPanels = new List<OTPanel>();
	static bool rootPanelsDirty = true;


	// bool isRootPanel = false;
	OTClippingAreaSprite sprite = null;
	
	new protected void Awake()
	{
		base.Awake();		
	}
	
	new protected void Start()
	{
		if (transform.parent == null || transform.parent.GetComponent<OTPanel>()==null)
		{
			rootPanels.Add(this);
			// isRootPanel = true;
		}		
		sprite = GetComponent<OTClippingAreaSprite>();
		base.Start();
		
	}
	
	OTSliderElement hSlider = null;
	OTSliderElement vSlider = null;
	Bounds lb = new Bounds(Vector3.zero, Vector3.zero);

	
	void ChangeSlider(OTElement element)
	{
		float sfv = 0;
		float sfh = 0;
		if (vSlider!=null)
			sfv = ((100f-vSlider.floatValue)/100f);
		if (hSlider!=null)
			sfh = (hSlider.floatValue/100f);		
		sprite.cameraOffset = new Vector2(sfh * (lb.size.x-sprite.worldRect.width),-sfv * (lb.size.y-sprite.worldRect.height));
	}
	
	void HSlider(OTElement element)
	{
	}
	
	void HandleBounds(Bounds wb)
	{
		Rect wr = sprite.worldRect;
		lb = wb;
		
		if ((wb.size.y - wr.height) > wb.size.y/100  && sliderForm!=null && vSlider==null)
		{
			vSlider = (GameObject.Instantiate(sliderForm.verticalSlider.gameObject) as GameObject).GetComponent<OTSliderElement>();
							
			vSlider.transform.localScale = new Vector3(sliderForm.verticalSliderWidth,sprite.worldRect.height,1);
			vSlider.name = "sliderV";

			vSlider.transform.position = 
				new Vector3(sprite.worldRect.xMax + (vSlider.Sprite("bar").worldRect.width/2),
					sprite.worldRect.yMax - (vSlider.Sprite("bar").worldRect.height/2),transform.position.z);						
			vSlider.transform.parent = transform;
			vSlider.depth = -10;			
			vSlider.visible = true;
			vSlider.onSlide = ChangeSlider;
			vSlider.value = 100;
		}
		
		float ww = wb.size.x - wr.width;
		float wd = wb.size.x/100;
		
		if ((wb.size.x - wr.width) > wb.size.x/100 && sliderForm!=null && hSlider==null)
		{
			hSlider = (GameObject.Instantiate(sliderForm.horizontalSlider.gameObject) as GameObject).GetComponent<OTSliderElement>();
							
			hSlider.transform.localScale = new Vector3(sprite.worldRect.width,sliderForm.horizontalSliderHeight,1);
			hSlider.name = "sliderH";

			hSlider.transform.position = 
				new Vector3(sprite.worldRect.xMax - (hSlider.Sprite("bar").worldRect.width/2),
					sprite.worldRect.yMin - (hSlider.Sprite("bar").worldRect.height/2),transform.position.z);						
			hSlider.transform.parent = transform;
			hSlider.depth = -10;			
			hSlider.visible = true;
			hSlider.onSlide = ChangeSlider;
			hSlider.value = 0;
		}
		
		if (vSlider)
			vSlider.SetView(sprite.worldRect.height, wb.size.y);
		if (hSlider)
			hSlider.SetView(sprite.worldRect.width, wb.size.x);
				
	}	
	
	new protected void Update()
	{		
		if (!Application.isPlaying)
		{
			sprite.clipLayer = clipLayer;
			sprite.clipMargin = clipMargin;
			
			if (clipLayer>0 && sliderForm==null)
			{
				Transform t = transform.parent;
				while (t!=null)
				{
					if (t.GetComponent<OTForm>()!=null)
					{
						sliderForm = t.GetComponent<OTForm>();
						break;
					}				
					t = t.parent;				
				}				
			}
			
			
		}
		else
		{
			Bounds wb = OT.GetBounds(sprite);
			if (!wb.Equals(lb))
				HandleBounds(wb);							
		}
		
		base.Update();
	}

	void ScreenChange()
	{		
		Invoke("BuildPanels",0.2f);
	}
	
	protected override void Initialize()
	{		
		if (rootPanelsDirty && rootPanels.Count>0)
		{			
			rootPanels.Sort( delegate(OTPanel a, OTPanel b)
			{
				return string.Compare(a.name,b.name,System.StringComparison.CurrentCultureIgnoreCase);
			});
			
			OT.view.onScreenChange = rootPanels[0].ScreenChange;
			BuildPanels();			
			rootPanelsDirty = false;
		}
	}
	
	List<Transform> childTransforms = new List<Transform>();
		
	void ChildsOut()
	{
		childTransforms.Clear();
		for (int i=0; i<transform.childCount; i++)
			childTransforms.Add(transform.GetChild(i));
		
		for (int i=0; i<childTransforms.Count; i++)
			childTransforms[i].parent = null;	
	}
		
	void ChildsIn()
	{
		for (int i=0; i<childTransforms.Count; i++)
			childTransforms[i].parent = transform;
			
	}
	
	Vector2 spriteDY
	{
		get
		{
			return new Vector2 (0,((sprite.pivotPoint.y - 0.5f) * sprite.worldSize.y));		;
		}
	}
	
	Vector2 spriteDX
	{
		get
		{
			return new Vector2 (((sprite.pivotPoint.x + 0.5f) * sprite.worldSize.x),0);		;
		}
	}

	void BuildPanels()
	{
		OT.RuntimeCreationMode();
		Rect r = OT.view.worldRect;			
		for (int i=0; i<rootPanels.Count; i++)
		{
			if (r.width>0 && r.height>0)
				rootPanels[i].Build(ref r);
		}
	}

	bool ValidBounds(Transform[] children, List<Bounds> bounds, List<Transform> transforms, List<int> ignore)
	{
		bounds.Clear();
		transforms.Clear();
		for (int i=0; i<children.Length; i++)
		{
			if (!ignore.Contains(children[i].GetInstanceID()))
			{
				OTElement e = children[i].GetComponent<OTElement>();
				OTSprite spr = children[i].GetComponent<OTSprite>();
				if ((e!=null && e.visible) || (spr!=null && spr.visible))
				{			
					if (e!=null)
						bounds.Add(e.bounds);
					else
						bounds.Add(spr.renderer.bounds);
					transforms.Add(children[i]);
				}
			}
		}
		return (bounds.Count>0);
	}
	
	void Mark(Vector2 position, Color c)
	{
		OTSprite s = OT.CreateSprite("mark");
		s.ForceUpdate();
		s.depth = -500;
		s.position = position;
		s.tintColor = c;
	}
	
	
	protected void Build(ref Rect pRect)
	{
		if (align != Alignment.None)
		{
			ChildsOut();
						
			Rect r = margins.Apply(pRect);												
			if (stretch)
			{
				Vector2 size = sprite.worldSize;
				switch(align)
				{
					case Alignment.Top:				
						sprite.worldSize = new Vector2(r.width,size.y);
						pRect.yMax -= sprite.worldSize.y + margins.top + margins.bottom;
					break;
					case Alignment.Bottom:
						sprite.worldSize = new Vector2(r.width,size.y);
						pRect.yMin += sprite.worldSize.y + margins.top + margins.bottom;
					break;
					case Alignment.Left:
						sprite.worldSize = new Vector2(size.x,r.height);
						pRect.xMin += sprite.worldSize.x + margins.left + margins.right;
					break;
					case Alignment.Right:
						sprite.worldSize = new Vector2(size.x,r.height);
						pRect.xMax -= sprite.worldSize.x + margins.left + margins.right;
					break;
					case Alignment.Center:
						sprite.worldSize = new Vector2(r.width,r.height);
						pRect = new Rect(0,0,0,0);
					break;
				}
			}
			
			ChildsIn();
			// adjust position
			switch(align)
			{
				case Alignment.Top:				
					if (stretch)
						sprite.position = sprite.fromWorld(new Vector2(r.xMin, r.yMax) + spriteDY + spriteDX);
					else
						sprite.position = sprite.fromWorld(new Vector2(sprite.worldPosition.x, r.yMax) + spriteDY);
				break;
				case Alignment.Bottom:
					if (stretch)
						sprite.position = sprite.fromWorld(new Vector2(r.xMin, r.yMin + sprite.worldSize.y ) + spriteDY + spriteDX);					
					else
						sprite.position = sprite.fromWorld(new Vector2(sprite.worldPosition.x, r.yMin + sprite.worldSize.y ) + spriteDY);					
				break;
				case Alignment.Left:
					if (stretch)
						sprite.position = sprite.fromWorld(new Vector2(r.xMin, r.yMax) + spriteDY + spriteDX);					
					else
						sprite.position = sprite.fromWorld(new Vector2(r.xMin,sprite.worldPosition.y) + spriteDX);
				break;
				case Alignment.Right:
					if (stretch)
						sprite.position = sprite.fromWorld(new Vector2(r.xMax - sprite.worldSize.x, r.yMax) + spriteDY + spriteDX);					
					else
					{
						sprite.position = sprite.fromWorld(new Vector2(r.xMax - sprite.worldSize.x,sprite.worldPosition.y) + spriteDX);
					}
				break;
				case Alignment.Center:
					sprite.position = sprite.fromWorld(new Vector2(r.xMin +  (r.width - sprite.worldSize.x)/2,r.yMax -  (r.height - sprite.worldSize.y)/2) + spriteDX + spriteDY);
				break;
			}			
		}
				
		List<int>handled = new List<int>();
		// let's build child panels	
		Rect re = sprite.worldRect;
				
		for (int i=0; i<transform.childCount; i++)
		{
			OTPanel p = transform.GetChild(i).GetComponent<OTPanel>();
			if (p!=null)
			{
				p.Build(ref re);
				if (p.align!= Alignment.None)
					handled.Add(p.transform.GetInstanceID());
			}
		}		
				
		if (alignChildren != Alignment.None)
		{
			Rect r = innerMargins.Apply(re);	
									
			Transform[] children = OTHelper.ChildrenOrderedByName(transform);			
			List<Bounds> bounds = new List<Bounds>();
			List<Transform> transforms = new List<Transform>();
			float hg = 0;
			// first lets group/grid the children
			if (ValidBounds(children, bounds, transforms, handled))
			{			
				float yp = r.yMax;
				float xp = r.xMin;
				for (int i=0; i<bounds.Count; i++)
				{
					Bounds b = bounds[i];
					Vector3 d = transforms[i].position-b.center;
					
					if (xp + (b.extents.x * 2) > r.xMax)
					{
						yp-=hg;
						yp-=childSpacing.y;
						xp = r.xMin;
						hg = 0;
					}										
					transforms[i].position = new Vector3(xp+b.extents.x, yp-b.extents.y) + d;
										
					xp+=(b.extents.x*2);
					xp+=childSpacing.x;	
					
					if ((b.extents.y*2) > hg)
						hg = b.extents.y*2;
					
				}		
			}
			
			// now calculate the new bounds and allign it in the panel
			if (ValidBounds(children, bounds, transforms, handled))
			{			
				Bounds b = new Bounds();
				for (int i=0; i<bounds.Count; i++)
					if (i==0)
						b = bounds[i];
					else
						b.Encapsulate(bounds[i]);														
				
				Vector2 rd = new Vector2((r.width - (b.extents.x * 2)), (r.height - (b.extents.y * 2)));
				Vector2 d = Vector2.zero;
				switch(alignChildren)
				{
					case Alignment.Center:
						d = new Vector2(rd.x/2,-rd.y/2);										
					break;
					case Alignment.Top:
						d = new Vector2(rd.x/2,0);										
					break;
					case Alignment.Bottom:
						d = new Vector2(rd.x/2,-rd.y);										
					break;
					case Alignment.Left:
						d = new Vector2(0,-rd.y/2);										
					break;
					case Alignment.Right:
						d = new Vector2(rd.x,-rd.y/2);										
					break;
				}				
				for (int i=0; i<transforms.Count; i++)
					transforms[i].position += (Vector3)d;				
			}
		}
		
	}
	
	void OnLevelWasLoaded()
	{
		rootPanels.Clear();
		rootPanelsDirty = true;
	}
	
		
}

[System.Serializable]
public class OTMargins
{
	public float top;
	public float left;
	public float bottom;
	public float right;
								
	public Rect Apply(Rect r)
	{
		r.xMin+=left;
		r.xMax-=right;
		r.yMax-=top;
		r.yMin+=bottom;
		return r;					
	}				
}

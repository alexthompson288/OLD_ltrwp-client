using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class OTForm : OTElement {

	public Color colorActiveText = Color.yellow;
	public Color colorNormalText = Color.white;	
	public OTSliderElement horizontalSlider = null;
	public float horizontalSliderHeight = 15;
	public OTSliderElement verticalSlider = null;
	public float verticalSliderWidth = 15;
		
	public OTFormElement.FormElementDelegate onElementSetValue = null;
	
	
	List<OTFormElement> formElements = new List<OTFormElement>();	
	Dictionary<string, OTFormElement> byVariable = new Dictionary<string, OTFormElement>();
	OTFormElement _focusedElement = null;
	
	string _xml = "<xml>\n</xml>";
	
	public string xml
	{
		get
		{
			return _xml;
		}
	}
	
	public OTFormElement focusedElement
	{
		get
		{
			return _focusedElement;
		}
	}
	
	public bool IsContainer(OTElement e)
	{
		if (e is OTPanel)
			return true;
		return false;
	}
	
	void AddElements(Transform t)
	{
		List<string> children = new List<string>();
		
		for (int i=0; i<t.childCount; i++)
			children.Add(t.GetChild(i).name+"||"+i.ToString());
		
		children.Sort();			
		for (int i=0; i<children.Count; i++)
		{	
			
			string[] s = children[i].Split( new string[] {"||"} ,System.StringSplitOptions.None);
			int idx = System.Convert.ToInt16(s[1]);
			
			OTElement e = t.GetChild(idx).GetComponent<OTElement>();
			if (e!=null && e is OTFormElement)
				Add(e as OTFormElement, t.gameObject.GetComponent<OTElement>());
			if (IsContainer(e))
				AddElements(e.transform);
		}
	}
	
	protected override void Initialize ()
	{
		base.Initialize ();
		AddElements(this.transform);
		
		if (horizontalSlider==null || verticalSlider == null)
		{
			Debug.LogWarning("OTForm sliders are not both set!");
			return;
		}
		
		if (Application.isPlaying)
		{
			horizontalSlider.visible = false;
			verticalSlider.visible = false;
		}
		
	}
	
	void GenerateXML()
	{
		_xml =  "<xml>\n";		
		_xml +=  "<form name=\""+name+"\">\n";		
		_xml +=  "   <elements>\n";		
		for (int i=0; i<formElements.Count; i++)
			if (formElements[i].inXML)
			_xml += "      <element name=\""+formElements[i].variable+"\">"+formElements[i].xmlValue+"</element>\n";
		_xml +=  "   </elements>\n";					
		_xml +=  "</form>\n";					
		_xml +=  "</xml>";					
	}
	
	public void Add(OTFormElement element, OTElement container)	
	{				
		if (!formElements.Contains(element))
			formElements.Add(element);
					
		if (element.variable!="")
		{
			if (!byVariable.ContainsKey(element.variable))
				byVariable.Add(element.variable,element);
		}
		
		
		if (element.form!= this)
			element.form = this;
		if (element.container!= container)
			element.container = container;
				
		if (focusedElement==null)
			_focusedElement = element;
		element.Repaint();
		
		GenerateXML();		
	}
	
	
	public void _ElementChanged(OTFormElement el)
	{
		GenerateXML();
		if (onElementSetValue!=null)
			onElementSetValue(el);
	}
	
	/// <summary>
	/// Gets the string value of a specific form element using its variable name
	/// </summary>
	public string StringValue(string variable)
	{
		if (byVariable.ContainsKey(variable))
			return byVariable[variable].stringValue;
		return "";
	}
	
	/// <summary>
	/// Gets the float value of a specific form element using its variable name
	/// </summary>
	public float FloatValue(string variable)
	{
		if (byVariable.ContainsKey(variable))
			return byVariable[variable].floatValue;
		return 0.0f;
	}

	/// <summary>
	/// Gets the integer value of a specific form element using its variable name
	/// </summary>
	public int IntValue(string variable)
	{
		if (byVariable.ContainsKey(variable))
			return byVariable[variable].intValue;
		return 0;
	}
	
	/// <summary>
	/// Gets the boolean value of a specific form element using its variable name
	/// </summary>
	public bool BoolValue(string variable)
	{
		if (byVariable.ContainsKey(variable))
			return byVariable[variable].boolValue;
		return false;
	}
	
	/// <summary>
	/// Sets the value of a specific form element using its variable name
	/// </summary>
	public void SetValue(string variable, object value)
	{
		if (byVariable.ContainsKey(variable))
			byVariable[variable].value = value;
	}
	
	/// <summary>
	/// Sets the value of a specific form element using an Xml document
	/// </summary>
	public void SetValue(XmlDocument xmlDoc)
	{
		XmlNodeList elementList = xmlDoc.SelectNodes("/xml/form/elements/element");
		foreach (XmlNode element in elementList)
		{
			if (element.Attributes["name"]!=null)
			{
				string varName = element.Attributes["name"].Value;
				string varValue = element.InnerText;
				SetValue(varName, varValue);
			}
		}
	}
	
	/// <summary>
	/// Finds form element with the specified variable name
	/// </summary>
	public OTFormElement Element(string variable)
	{
		if (byVariable.ContainsKey(variable))
			return byVariable[variable];
		return null;
	}
	
	public void Remove(OTFormElement element)
	{
		if (formElements.Contains(element))
			formElements.Remove(element);
		
		if (element.form== this)
			element.form = null;

		if (element.variable!="")
		{
			if (byVariable.ContainsKey(element.variable))
				byVariable.Remove(element.variable);
		}		
		
		if (focusedElement == this)
		{
			if (formElements.Count>0)
			{
				_focusedElement = formElements[0];
				_focusedElement.Repaint();
			}
			else
				_focusedElement = null;
		}		
	}
	
	/// <summary>
	/// Set the focus of this form the the provided element
	/// </summary>
	public void Focus(OTFormElement element)
	{		
		if (formElements.Contains(element) && _focusedElement!=element)
		{
			OTFormElement old = _focusedElement;
			_focusedElement = element;
			if (old!=null)
				old.Repaint();
			_focusedElement.Repaint();
		}
	}
	
	public void Focus(int indexDelta)
	{
		if (_focusedElement!=null)
		{
			int i = formElements.IndexOf(_focusedElement);
			if (i!=-1)
			{
				i+=indexDelta;
				if (i<0) i = formElements.Count-1;
				if (i>formElements.Count-1) i = 0;
				Focus(formElements[i]);
			}
		}
	}
	
	
	
	public void Defocus(OTFormElement element)
	{		
		if (formElements.Contains(element) && _focusedElement==element)
		{
			OTFormElement old = _focusedElement;
			_focusedElement = null;
			if (old!=null)
				old.Repaint();
		}
	}
	
}

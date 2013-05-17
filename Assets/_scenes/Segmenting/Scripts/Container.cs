using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour {
	
	public bool letterMounted=false;
	public Container lastContainer;
	public GameObject mountedLetter;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ChangeButtonMaterial(){
	
//		ContainerButton cb=gameObject.GetComponent<ContainerButton>();
		
		foreach(Transform t in transform)
		{
			ContainerButton cb=t.gameObject.GetComponent<ContainerButton>();
			cb.renderer.material=cb.MountMaterial;
		}
		
		
	}
	
	public void ShowMe(){
		renderer.enabled=true;	
	}
	
	public void HideMe(){
		renderer.enabled=false;	
	}
	
}

using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour {
	public Material NewMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material = NewMaterial;
	}
}

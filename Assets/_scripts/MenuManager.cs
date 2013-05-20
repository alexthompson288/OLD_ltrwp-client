using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public bool HasBook=true;
	public Transform BalHolder;
	public Transform BalAnimation;
	public Transform Backmark;
	public Transform ParentObject;

	// Use this for initialization
	void Start () {
	
	}

	void Awake () {
		Transform BalBg=null;
		Transform Bal=null;
		if(HasBook)
		{
			BalBg=(Transform)Instantiate(BalHolder);
			Bal=(Transform)Instantiate(BalAnimation);
		}

		Transform Back=(Transform)Instantiate(Backmark);

		if(ParentObject!=null)
		{
			BalBg.parent=ParentObject;
			Bal.parent=ParentObject;
			Back.parent=ParentObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public bool HasBook=true;
	public Transform BalHolder;
	public Transform BalAnimation;
	public Transform Backmark;
	public Transform ParentObject;
	public bool useCustomDepth;
	public int customDepth;

	// Use this for initialization
	void Start () {
		Transform BalBg=null;
		Transform Bal=null;
		if(HasBook)
		{
			BalBg=(Transform)Instantiate(BalHolder);
			Bal=(Transform)Instantiate(BalAnimation);
			
			if(useCustomDepth)
			{
				OTSprite sBal=Bal.GetComponent<OTSprite>();
				OTSprite sBalHolder=BalBg.GetComponent<OTSprite>();

				if(customDepth<0)
					sBal.depth=customDepth-10;
				else
					sBal.depth=customDepth+10;
					
				sBalHolder.depth=customDepth;
			}
		}

		Transform Back=(Transform)Instantiate(Backmark);
		
		if(useCustomDepth){
			OTSprite sBack=Back.GetComponent<OTSprite>();
			sBack.depth=customDepth;
		}
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

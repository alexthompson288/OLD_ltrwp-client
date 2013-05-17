using UnityEngine;
using System.Collections;

public class PopulateNodes : MonoBehaviour {
	public OTSpline nodeSlice;
	public Transform BigNode;
	public Transform SmallNode;
	// Use this for initialization
	void Start () {

		GameManager.Instance.LogState();
	
		// int nodesToDraw=30;
		// int bigSmallSpacing=4;
		// int nodesFromBig=0;


		
		// for(int i=0;i<nodesToDraw;i++)
		// {
		// 	Vector2 thisPos=nodeSlice.GetPosition((15/nodesToDraw)*100);
			
		// 	Debug.Log ("thispos "+thisPos);
			
		// 	Transform curNode;
			
		// 	if(nodesFromBig==0)
		// 	{
		// 		curNode=(Transform)Instantiate(BigNode);	
		// 	}
		// 	else
		// 	{
		// 		curNode=(Transform)Instantiate(SmallNode);
		// 		nodesFromBig++;
		// 	}
			
		// 	OTSprite mySprite=curNode.GetComponent<OTSprite>();
		// 	mySprite.position=thisPos;
			
		// 	if(nodesFromBig>=bigSmallSpacing)
		// 		nodesFromBig=0;
		// }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}

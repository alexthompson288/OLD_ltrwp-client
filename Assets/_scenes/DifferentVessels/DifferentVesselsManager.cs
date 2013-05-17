using UnityEngine;
using System.Collections;

public class DifferentVesselsManager : MonoBehaviour {

	float timeBetweenChanges=4.0f;
	float timeToNextContainer=0.0f;
	int currentContainerIndex=0;
	int CorrectRequiredInEachContainer=1;
	bool isComplete;
	
	public PersistentObject PersistentManager;
	
	public Transform[] SceneContainers;
	int[] CorrectInContainers;
	
	// Use this for initialization
	void Start () {
		CorrectInContainers=new int[SceneContainers.Length];
		
		for(int i=0;i<SceneContainers.Length;i++)
		{
			CorrectInContainers[i]=0;
		}
		
		ReadPersistentObjectSettings();
	}
	
	// Update is called once per frame
	void Update () {
		if(EvalProblem() && !isComplete)
		{
			Debug.Log ("well I'll be the son of a sausage, you did it brah.");
			isComplete=true;
		}
		
		DecrementAndChangeContainer();
	}
	
	bool EvalProblem()
	{
		int amountCorrect=0;
		
		for(int i=0;i<CorrectInContainers.Length;i++)
		{
			if(CorrectInContainers[i]>=CorrectRequiredInEachContainer)
				amountCorrect++;
		}
		
		if(amountCorrect==CorrectInContainers.Length)
			return true;
		else 
			return false;
	}
	
	void ReadPersistentObjectSettings(){
		
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			thisPO.GetComponent<PersistentObject>().CurrentTheme="castle";
		}
		
		PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
	}
	
	void DecrementAndChangeContainer () {
		
		if(isComplete)return;
		
		timeToNextContainer-=Time.deltaTime;
		
		if(timeToNextContainer<0)
		{
			currentContainerIndex=Random.Range (0,SceneContainers.Length);
			
			while(CorrectInContainers[currentContainerIndex]>=CorrectRequiredInEachContainer){
				currentContainerIndex=Random.Range (0,SceneContainers.Length);	
			}
			
			MakeContainerActive(currentContainerIndex);
			
			timeToNextContainer=timeBetweenChanges;
		}
	}
	
	void MakeContainerActive (int thisContainer)
	{
		for(int i=0;i<SceneContainers.Length;i++)
		{
			Transform thisV=SceneContainers[i];
			DVContainer v=thisV.GetComponent<DVContainer>();
			
			if(i==thisContainer)
				v.OpenVessel();
			
			else
				v.CloseVessel();
			
		}
		
	}
	
	public void CheckDropOnContainer (Transform thisContainer, Transform thisObject) {
		
		 DVContainer v=thisContainer.GetComponent<DVContainer>();
		 DVVessel t=thisObject.GetComponent<DVVessel>();
		 if(v.TakesThisLetter(t.myLetter))
		 {
			int thisContIndex=0;
			
			 for(int i=0;i<SceneContainers.Length;i++)
			 {
				Transform curCont=SceneContainers[i];
				if(curCont==thisContainer)
				{
					CorrectInContainers[i]++;
					Debug.Log ("Correct letters in "+i+" = "+CorrectInContainers[i]);
					break;
				}
				
			 }
			
			t.Destroy();
		 }
		else
		{
			Debug.Log ("letter doesn't belong on this container");
		}
	
	}
	
	public void CheckValidContainer(Vector2 location, Transform pickupObject){
		foreach(Transform t in SceneContainers)
		{
			DVContainer c=t.GetComponent<DVContainer>();
			
			if(c.BoundingBox.Contains(location))
			{
				CheckDropOnContainer(t, pickupObject);
			}
			else 
			{
				//maybe reject or something here?
			}
				
		}
	}
}

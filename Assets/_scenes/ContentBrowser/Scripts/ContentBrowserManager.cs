using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContentBrowserManager : MonoBehaviour {

	public OTSprite[] Vortexes;
	public AudioClip[] BrowserAudio;
	public OTSprite[] Nodes;
	public OTSprite PipThing;
	OTSprite lastNode;
	OTSprite nextNode;

	// Use this for initialization
	void Start () {

		List<int> CompletedSessions=GameManager.Instance.GetCompletedSessions();

		foreach(OTSprite s in Nodes)
		{
			NodeInfo ni=s.GetComponent<NodeInfo>();
			if(CompletedSessions.Contains(ni.sessionID)){
				Debug.Log("set this session visible"+ni.sessionID);
				s.visible=true;

				if(!ni.notInteractive)
					lastNode=s;

			}
			else{
				s.visible=false;

				if(nextNode==null)
					nextNode=s;
			}
		}

		if(lastNode!=null)
			PipThing.position=lastNode.position;

		Invoke("MovePip", 2.0f);

		// foreach(OTSprite s in Vortexes)
		// {
		// 	float thisTime=Random.Range(2.5f,4.0f);
		// 	var config=new GoTweenConfig()
		// 		.floatProp( "rotation", -360.0f )
		// 		.setIterations(-1,GoLoopType.RestartFromBeginning)
		// 		.setEaseType( GoEaseType.Linear );

		
		// // Go.to(s, 0.3f, config );
		// GoTween tween=new GoTween(s, thisTime, config);

		// Go.addTween(tween);
		// }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MovePip()
	{
		if(lastNode==null)return;
			
		var config=new GoTweenConfig()
			.vector2Prop( "position", nextNode.position )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(PipThing, 0.8f, config);
		Go.addTween(tween);
		
	}

	public void UpdateAudio(int currentIndex)
	{
		AudioClip newAudio=BrowserAudio[currentIndex];
		AudioSource asrc=gameObject.GetComponent<AudioSource>();

		if(newAudio==asrc.clip)return;

		asrc.clip=newAudio;
		asrc.Play();
	}

}

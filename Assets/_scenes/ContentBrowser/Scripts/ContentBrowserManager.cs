using UnityEngine;
using System.Collections;

public class ContentBrowserManager : MonoBehaviour {

	public OTSprite[] Vortexes;
	public AudioClip[] BrowserAudio;

	// Use this for initialization
	void Start () {

		foreach(OTSprite s in Vortexes)
		{
			float thisTime=Random.Range(2.5f,4.0f);
			var config=new GoTweenConfig()
				.floatProp( "rotation", -360.0f )
				.setIterations(-1,GoLoopType.RestartFromBeginning)
				.setEaseType( GoEaseType.Linear );

		
		// Go.to(s, 0.3f, config );
		GoTween tween=new GoTween(s, thisTime, config);

		Go.addTween(tween);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

using UnityEngine;
using System.Collections;

public class VortexManager : MonoBehaviour {

	public ALVideoTexture[] vortexParts;
	int currentIndex=0;

	ALVideoTexture currentPart;
	bool ShownAllVortex=false;

	AsyncOperation async;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		StartLoading();
		currentPart=vortexParts[currentIndex];
		currentPart.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentPart.hasFinished && !ShownAllVortex)
		{
			currentPart.gameObject.SetActive(false);
			currentIndex++;
			if(currentIndex<=vortexParts.Length-1){
				currentPart=vortexParts[currentIndex];
				currentPart.gameObject.SetActive(true);
			}
			else 
			{
				Debug.Log("Shown all.");
				ShownAllVortex=true;
			}
		}

		if(async!=null && async.isDone && ShownAllVortex)
		{
			ActivateScene();
		}
	}
	public void StartLoading() {
	    StartCoroutine("load");
	}
	 
	IEnumerator load() {
	    Debug.LogWarning("ASYNC LOAD STARTED - " +
	       "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
	    async = Application.LoadLevelAsync("WordBank");
	    async.allowSceneActivation = false;
	    yield return async;
	}
	 
	public void ActivateScene() {
	    async.allowSceneActivation = true;
	}
}

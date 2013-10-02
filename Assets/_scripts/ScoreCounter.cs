using UnityEngine;
using System.Collections;

public class ScoreCounter : MonoBehaviour {
	
	PersistentObject PersistentManager;
	public GameObject ScoreBG;
	private bool HasStartedCounting = false;
	private bool HasSFinishedCounting = false;
	public OTTextSprite ScoreText;
	float ScoreTimer = 0.0f;
	int PlayerScore = 0;
	int CurrentScore = 0;
	public TransitionScreen _transitionScreen;
	
	public OTAnimatingSprite PipAni;
	public OTAnimatingSprite BunnyAni;

	// Use this for initialization
	void Start () {
		if(GameObject.Find ("PersistentManager")==null){
			GameObject thisPO=new GameObject("PersistentManager");
			thisPO.AddComponent<PersistentObject>();
			PersistentManager=thisPO.GetComponent<PersistentObject>();
		}
		else {
			PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();	
		}
		_transitionScreen = GameObject.Find("TransitionScreen").GetComponent<TransitionScreen>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad > 1.0f && !HasStartedCounting)
		{
			PlayerScore = PersistentManager.Score;
			HasStartedCounting = true;
			PopScoreBG();
			Debug.Log("pop pop");
		}
		if(HasStartedCounting)
		{
			
		}
		if(Time.timeSinceLevelLoad > 20.0f)
			_transitionScreen.ChangeLevel("ChallengeMenu");
	}
	
	void PopScoreBG()
	{	
		if(!HasSFinishedCounting)
		{
			if(CurrentScore < PlayerScore)
			{
				CurrentScore ++ ;	
			}else{
				HasSFinishedCounting = true;
				CurrentScore = PlayerScore;
				PipAni.Play();
				BunnyAni.Play();
			}
			ScoreText.text = CurrentScore.ToString();
			ScoreText.ForceUpdate();
			
			ScoreBG.transform.localScale = new Vector3( 211.3164f, 203.33f, 0.39f);
			Vector3 newScale=new Vector3(230.0f, 222.0f, 0.39f);
				var config=new GoTweenConfig()
					.vector3Prop( "localScale", newScale )
					.setEaseType( GoEaseType.QuadInOut );
		
				GoTween tween=new GoTween(ScoreBG.transform, 0.18f, config);
				tween.setOnCompleteHandler(c => PopScoreBG());
		
				Go.addTween(tween);
		}
	}
}

using UnityEngine;
using System.Collections;

public class CrossTheBridgeManager : MonoBehaviour {

	int requiredAnswers=3;
	int correctAnswers=0;
	public Transform sign;

	public OTSprite[] AnswerSprites;

	TrollAnimation troll;

	// Use this for initialization
	void Start () {
	
	}
	
	void Awake() {
		sign=GameObject.Find("sign").transform;
		troll=GameObject.Find("Troll").GetComponent<TrollAnimation>();
		OTSprite sSign=sign.GetComponent<OTSprite>();
		sign.position=new Vector2(sign.position.x, sign.position.y+500);
		DropSignIn();
	}


	void DropSignIn()
	{
			OTSprite s=sign.GetComponent<OTSprite>();
			Vector2 newPos=new Vector2(s.position.x, s.position.y-500);
			var config=new GoTweenConfig()
				.vector2Prop( "position", newPos )
				.setEaseType( GoEaseType.BounceOut );

			GoTween tween=new GoTween(s, 0.8f, config);

			Go.addTween(tween);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture)
	{
		if(gesture.pickObject!=null)
		{
			if(gesture.pickObject.GetComponent<GenericAnswer>())
			{
				GenericAnswer a=gesture.pickObject.GetComponent<GenericAnswer>();
				if(a.isAnswer)
				{
					correctAnswers++;
					 
					if(correctAnswers==requiredAnswers)
					{
						Debug.Log("target met");	
					}
					else
					{
						troll.MakeBellyBigger();
						ShuffleAnswers();
					}
				}
				else
				{
					troll.SetNonePlaying();
					troll.playNegative=true;
					Debug.Log("incorrect");
				}
			}
		}
	}

	void ShuffleAnswers()
	{
		int newCorrectAnswer=Random.Range(0,3);

		for(int i=0;i<AnswerSprites.Length;i++)
		{
			OTSprite s=AnswerSprites[i];
			GenericAnswer a=s.GetComponent<GenericAnswer>();
			if(i==newCorrectAnswer)
			{
				a.isAnswer=true;
				var configr=new GoTweenConfig()
					.floatProp( "alpha", 0.5f );
			

		
				// Go.to(s, 0.3f, config );
				GoTween tweenr=new GoTween(s, 0.5f, configr);

				Go.addTween(tweenr);
			}
			else
				a.isAnswer=false;
				var configw=new GoTweenConfig()
					.floatProp( "alpha", 1.0f );
			

		
				// Go.to(s, 0.3f, config );
				GoTween tweenw=new GoTween(s, 0.5f, configw);

				Go.addTween(tweenw);
		}
	}
}

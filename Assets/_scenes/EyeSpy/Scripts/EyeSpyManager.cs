using UnityEngine;
using System.Collections;
using AlTypes;

public class EyeSpyManager : MonoBehaviour {

	DataWordData[] datawords;
	public Transform[] Objects;

	public string[] RequiredLetters;

	int currentLetterIndex=0;
	string currentLetter="";


	void Awake (){
		datawords=GameManager.Instance.SessionMgr.CurrentDataWords;

		if(datawords!=null)
			Debug.Log("dws - "+datawords.Length);


		for(int i=0;i<RequiredLetters.Length;i++)
		{
			Transform t=Objects[i];
			t.gameObject.SetActive(true);
		}

		currentLetter=RequiredLetters[currentLetterIndex];

	}

	// Use this for initialization
	void Start () {
	
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

				if(a.answerValue==currentLetter)
				{
					GameObject.Destroy(gesture.pickObject);
					SetNextLetter();
				}
				else 
				{
					Debug.Log("Shit's whack");
				}
			}
		}
	}

	void SetNextLetter()
	{
		currentLetterIndex++;
		if(currentLetterIndex<RequiredLetters.Length)
			currentLetter=RequiredLetters[currentLetterIndex];
		else
			EndGame();
		Debug.Log("new letter is "+currentLetter);
	}

	void EndGame()
	{
		Debug.Log("we finish now.");
	}

}

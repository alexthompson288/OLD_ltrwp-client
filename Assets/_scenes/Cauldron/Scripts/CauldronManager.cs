using UnityEngine;
using System.Collections;

public class CauldronManager : MonoBehaviour {

	public string[] RequiredWords;
	int currentWordIndex=0;
	string currentLetter="";
	GenericDraggable Cauldron;

	public Transform objectPrefab;

	void Awake() {
		Cauldron=GameObject.Find("Cauldron").GetComponent<GenericDraggable>();
		
		for(int i=0;i<RequiredWords.Length;i++)
		{
			string thisWord=RequiredWords[i];
			string thisLetter=thisWord[0].ToString();
			Transform t=(Transform)Instantiate(objectPrefab);
			t.GetComponent<CauldronObject>().myWord=thisWord;
			t.GetComponent<GenericAnswer>().answerValue=thisLetter;
		}

		string currentWord=RequiredWords[currentWordIndex];
		currentLetter=currentWord[0].ToString();
		Cauldron.answerValue=currentLetter;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
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
		currentWordIndex++;
		if(currentWordIndex<RequiredWords.Length)
		{
			string currentWord=RequiredWords[currentWordIndex];
			currentLetter=currentWord[0].ToString();
			Cauldron.answerValue=currentLetter;
		}
		else
		{
			EndGame();
		}
		Debug.Log("new letter is "+currentLetter);
	}

	void EndGame()
	{
		Debug.Log("we finish now.");
	}
}

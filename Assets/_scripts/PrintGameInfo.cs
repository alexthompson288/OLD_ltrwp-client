using UnityEngine;
using System.Collections;
using AlTypes;

public class PrintGameInfo : MonoBehaviour {
	
	Hashtable gameInfo;
	PhonemeData[] phonemes;
	DataWordData[] datawords;
	DataSentenceData[] sentences;
	
	void Start ()
	{
		 gameInfo=GameManager.Instance.SessionMgr.CurrentSectionHash();	
		 phonemes=GameManager.Instance.SessionMgr.CurrentPhonemes;
		 datawords=GameManager.Instance.SessionMgr.CurrentDataWords;
		 sentences=GameManager.Instance.SessionMgr.CurrentDataSentences;
	}
	
	void Update ()
	{
		if(Input.touchCount>0||Input.GetMouseButtonUp(0))
			GameManager.Instance.SessionMgr.CloseActivity();
		
	}
	
	private void OnGUI ()
	{	
		GUI.BeginGroup(new Rect(10,10,1004,748));
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 12;
		style.normal.textColor = Color.grey;
		
		float yPos=0;
		
		foreach(DictionaryEntry e in gameInfo)
		{
			GUI.Label(new Rect(0, yPos, 1024, 20), e.Key +": " + e.Value);
			yPos+=15;
		}
		
		yPos+=45;
		GUI.Label(new Rect(0, yPos, 1024, 20), "data words");
		yPos+=15;

		foreach(DataWordData dw in datawords)
		{
			GUI.Label(new Rect(0, yPos, 1024, 20), dw.Word + " (target: " + dw.IsTargetWord + " nonsense: " + dw.Nonsense.ToString() + " dummy: " + dw.IsDummyWord + " linking index: " + dw.LinkingIndex + ")");
			yPos+=15;	
		}
		
		yPos+=15;
		GUI.Label(new Rect(0, yPos, 1024, 20), "data sentences");
		yPos+=15;

		foreach(DataSentenceData dsd in sentences)
		{
			GUI.Label(new Rect(0, yPos, 1024, 20), dsd.Sentence + " (linking index: " + dsd.LinkingIndex + ")");
			yPos+=15;	
		}

		yPos+=15;
		GUI.Label(new Rect(0, yPos, 1024, 20), "phonemes");
		yPos+=15;

		foreach(PhonemeData pd in phonemes)
		{
			GUI.Label(new Rect(0, yPos, 1024, 20), pd.Phoneme + " (m1: " + pd.Mneumonic + " m2: " + pd.MneumonicTwo + ")");
			yPos+=15;	
		}
		
		GUI.EndGroup();

	}
}

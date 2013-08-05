using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class MiniRouter : MonoBehaviour {

	private string usernameText = "";
	private string usernameError = "";
	private Vector2 scrollPos = Vector2.zero;	
	private string newAttemptScore = "";
	
	private void Start()
	{
		// String[]ss=GameManager.Instance.AllMneumonics();
		// Debug.Log(ss.Length);

		string activityDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "activity.db");
		string stateDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "state.db");
		
		string newUser=Guid.NewGuid().ToString();
		User.CreateNew(newUser, null);
		
		
		User.CurrentUser = User.LastCreatedUser;
		// Debug.Log(activityDbPath);
		// Debug.Log(stateDbPath);
	}

	private void OnGUI ()
	{
		string activityDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "activity.db");
		string stateDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "state.db");
		
		int numUsers = User.AllUsers.Count;
		
		GUI.BeginGroup(new Rect(10,10,1004,748));
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 12;
		style.normal.textColor = Color.grey;
		GUI.Label(new Rect(0, 0, 1024, 20), "Activity db path: " + activityDbPath, style);
		
		GUI.Label(new Rect(0, 15, 1024, 20), "State db path: " + stateDbPath, style);
		
		GUI.Label(new Rect(0, 30, 1024, 20), "ActivityBatchId: " + GameManager.Instance.ActivityBatchId, style);
		// GUI.Label(new Rect(500,470,500,25), "CMS word count: " + GameManager.Instance.CmsWordCount.ToString());
		
		GUI.BeginGroup(new Rect(0,55,1004,25));
		
		

		if (User.CurrentUser != null)
		{
			GUI.BeginGroup(new Rect(0, 250, 1004, 210));

			if(GUI.Button(new Rect(0,0, 200, 25), "Challenge Menu"))
			{
				Application.LoadLevel("ChallengeMenu");
			}

			GUI.EndGroup();
		}
		
		GUI.EndGroup();
		GUI.EndGroup();
	}
	
	private string Base64Encode(string stringToEncode)
	{
		byte[] bytesToEncode = System.Text.ASCIIEncoding.ASCII.GetBytes(stringToEncode);
      	return System.Convert.ToBase64String(bytesToEncode);
	}
	
	private string Base64Decode(string encoded)
	{
		byte[] encodedDataAsBytes = System.Convert.FromBase64String(encoded);
      	return System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
	}
}

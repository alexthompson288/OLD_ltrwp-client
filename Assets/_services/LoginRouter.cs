using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class LoginRouter : MonoBehaviour
{
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
		GUI.SetNextControlName("UsernameInput");
		usernameText = GUI.TextField(new Rect(0,0,200,25), usernameText, 24);
		usernameText = System.Text.RegularExpressions.Regex.Replace(usernameText, @"[^0-9a-zA-Z_-]", "");
		if (GUI.GetNameOfFocusedControl() == "UsernameInput" && Event.current.type == EventType.KeyUp) usernameError = "";				
		if (GUI.Button(new Rect(205,0,100,25), "Create user"))
		{
			usernameError = "";			
			if (usernameText.Length == 0)
			{
				usernameError = "Cannot create user with empty name!";
			}
			else
			{
				User.CreateNew(usernameText, error => usernameError = error == null ? "" : error);
			}
		}
		if (GUI.Button(new Rect(310,0,100,25), "Sync user"))
		{
			usernameError = "";			
			if (usernameText.Length == 0)
			{
				usernameError = "Cannot download user with empty name!";
			}
			else
			{
				User.Download(usernameText, error => usernameError = error == null ? "" : error);
			}			
		}		
		GUI.Label(new Rect (375, 0, 754, 25), usernameError);
		GUI.EndGroup();
	
		List<User> allUsers = User.AllUsers.OrderBy(u => u.Name).ToList();
		int numButtonsInView = 4;
		int btnHeight = 25;
		int btnPadding = 2;
		int scrollViewDisplayHeight = numButtonsInView*btnHeight + (numButtonsInView-1)*btnPadding;
		if (numUsers > 0)
		{
			int scrollViewInnerHeight = numUsers*btnHeight + (numUsers-1)*btnPadding;
			scrollPos = GUI.BeginScrollView(new Rect(0, 100, 220, scrollViewDisplayHeight), scrollPos, new Rect(0,0,200,scrollViewInnerHeight));
			for (int i=0; i<numUsers; i++)
			{
				if (GUI.Button(new Rect(0, i*(btnHeight+btnPadding), 200, btnHeight), "login: " + allUsers[i].Name))
				{
					User.CurrentUser = User.AllUsers.FirstOrDefault(u => u.Name == allUsers[i].Name);
				}
			}
			GUI.EndScrollView();
		}

		string currentUserText = User.CurrentUser == null ? (numUsers > 0 ? "Select, create or download a user..." : "Create or download a user...") : "CURRENT USER: \"" + User.CurrentUser.Name + "\"";
		GUI.Label(new Rect(0, 225, 500, 20), currentUserText);

		if (User.CurrentUser != null)
		{
			GUI.BeginGroup(new Rect(0, 250, 1004, 210));

			if(GUI.Button(new Rect(0,0, 200, btnHeight), "Content Browser"))
			{
				Application.LoadLevel("ContentBrowser-Full");
			}

			if(GUI.Button(new Rect(0, 35, 200, btnHeight), "Debug Session Skip"))
			{
				Application.LoadLevel("DebugSceneSelect");
			}

			if(GUI.Button(new Rect(0, 105, 200, btnHeight), "test insert dps & dist"))
			{
				GameManager.Instance.LogDataPoint("phoneme", "a", "1");
				GameManager.Instance.LogDataPoint("phoneme", "a", "1");
				GameManager.Instance.LogDataPoint("phoneme", "a", "1");
				GameManager.Instance.LogDataPoint("phoneme", "b", "1");
				GameManager.Instance.LogDataPoint("phoneme", "eh", "-1");
				GameManager.Instance.LogDataPoint("phoneme", "d", "-1");
				GameManager.Instance.LogDataPoint("phoneme", "b", "1");
				GameManager.Instance.LogDataPoint("phoneme", "eh", "-1");

				ArrayList al=GameManager.Instance.GetDistributedDataPoints("phoneme", 0f, 20);
				foreach(string pk in al)
					Debug.Log(pk);
			}

			GUI.EndGroup();
		}
		
		// if (User.CurrentUser != null)
		// {			
		// 	GUI.BeginGroup(new Rect(0, 250, 1004, 210));
		// 	System.Globalization.CultureInfo uk = new System.Globalization.CultureInfo("en-GB");
		// 	GUI.Label(new Rect(0,0,500,20), "Last Attempt:\t\t" + (User.CurrentUser.LastAttempt == null 
		// 		? "(null)" 
		// 		: String.Format(@"{{ date:""{0}"", score:{1} }}", 
		// 			UnixDate.ToSystemDate(User.CurrentUser.LastAttempt.Date).ToString(uk), User.CurrentUser.LastAttempt.Score.ToString())));
		// 	GUI.Label(new Rect(0,20,500,20), "Best Attempt:\t" + (User.CurrentUser.BestAttempt == null 
		// 		? "(null)" 
		// 		: String.Format(@"{{ date:""{0}"", score:{1} }}", 
		// 			UnixDate.ToSystemDate(User.CurrentUser.BestAttempt.Date).ToString(uk), User.CurrentUser.BestAttempt.Score.ToString())));
		// 	GUI.Label(new Rect(0,40,500,22), "Num Attempts:\t" + User.CurrentUser.NumAttempts);
		// 	GUI.Label(new Rect(0,60,500,22), "Average Score:\t" + User.CurrentUser.AverageScore);
		// 	GUI.EndGroup();
		
		// 	GUI.BeginGroup(new Rect(0, 380, 1004, 45));
		// 	GUI.Label (new Rect(0, 0, 100, 20), "NEW ATTEMPT");
		// 	GUI.Label (new Rect(0, 22, 40, 20), "Score:");
		// 	newAttemptScore = GUI.TextField(new Rect(45, 20, 80, 25), newAttemptScore, 5);
		// 	newAttemptScore = System.Text.RegularExpressions.Regex.Replace(newAttemptScore, @"[^0-9]", "");
		// 	if (GUI.Button(new Rect(130, 20, 70, 25), "SUBMIT") && newAttemptScore.Length > 0)
		// 	{
		// 		User.CurrentUser.EndAttempt(Convert.ToInt32(newAttemptScore));
		// 		newAttemptScore = "";
		// 	}
		// 	GUI.EndGroup();
		// }
		
		GUI.BeginGroup(new Rect(0,470,500,25));
		if (GUI.Button(new Rect(0,0,70,25), "SYNC"))
		{
			GameManager.Instance.SyncService.Sync();
		}
		GUI.Label(new Rect(80,2,420,23), GameManager.Instance.SyncService.IsSyncing ? "Sync in progress..." : "Ready");
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
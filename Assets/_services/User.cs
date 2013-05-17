using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class User
{
	public static List<User> AllUsers { get { return _allUsers; }}
	public static User CurrentUser
	{
		get { return _currentUser; }
		set
		{
			_currentUser = value;
			GameManager.Instance.LogEvent("USER_LOGIN", _currentUser.Id, null);
		}
	}
	
	public string Id, Name;
	public double CreationDate, AverageScore;
	public int NumAttempts;
	public Attempt LastAttempt, BestAttempt;
	
	public class Attempt
	{
		public int Score;
		public double Date;
	}
	
	public static void CreateNew (string name, System.Action<string> callback)
	{
		if (User.AllUsers.FirstOrDefault(ur => ur.Name == name) != null)
		{
			callback(String.Format(@"A user named ""{0}"" already exists locally", name));
			return;
		}
		
		// does user exist on server?
		string jsonString = String.Format(@"{{""name"":""{0}""}}", name);
		byte[] jsonBytes = (new System.Text.UTF8Encoding()).GetBytes(jsonString);
		
		Hashtable headers = new Hashtable();
		headers["Content-Type"] = "application/json";
		
		((HTTPService)GameManager.Instance.PersistentGameObject.GetComponent("HTTPService")).SendWWWRequest(CheckUsernameAvailableURL, jsonBytes, headers, www =>
		{
			if (www.error != null)
			{
				callback("error checking availability on server: " + www.error);
			}
			else if (www.text == "no")
			{
				callback("username already taken on another app installation");
			}
			else
			{			
				User u = new User();
				u.Id = Guid.NewGuid().ToString();
				u.Name = name;
				u.CreationDate = UnixDate.Now;
				
				_allUsers.Add(u);
				
				GameManager.Instance.ActivityDb.ExecuteNonQuery(String.Format(
					"INSERT INTO UserCreations(batch_id, id, user_id, user_name, date) VALUES('{0}','{1}','{2}','{3}','{4}')", 
					GameManager.Instance.ActivityBatchId, Guid.NewGuid().ToString(), u.Id, u.Name, u.CreationDate));
				
				GameManager.Instance.StateDb.ExecuteNonQuery(String.Format("INSERT INTO Users(id, name, creation_date) VALUES('{0}', '{1}', {2})", u.Id, u.Name, u.CreationDate));
				
				callback(null);
			}
		});
	}
	
	public static void Download (string name, System.Action<string> callback)
	{
		if (User.AllUsers.FirstOrDefault(ur => ur.Name == name) != null)
		{
			callback(String.Format(@"User named ""{0}"" already exists locally", name));
			return;
		}
		
		string jsonString = String.Format(@"{{""name"":""{0}"", ""installationId"":""{1}""}}", name, GameManager.Instance.InstallationId);
		byte[] jsonBytes = (new System.Text.UTF8Encoding()).GetBytes(jsonString);
		
		Hashtable headers = new Hashtable();
		headers["Content-Type"] = "application/json";
		
		((HTTPService)GameManager.Instance.PersistentGameObject.GetComponent("HTTPService")).SendWWWRequest(DownloadUserURL, jsonBytes, headers, www =>
		{
			if (www.error != null)
			{
				callback("error downloading user:" + www.error);
			}
			else
			{
				User u = JSONSerializer.Deserialize<User>(www.text);
				_allUsers.Add(u);
				string insertStatement = u.LastAttempt == null
					? String.Format("INSERT INTO Users(id, name, creation_date, num_attempts, average_score) VALUES('{0}', '{1}', {2}, 0, 0)", u.Id, u.Name, u.CreationDate)
					: String.Format("INSERT INTO Users(id, name, creation_date, last_attempt_date, last_attempt_score, best_attempt_date, best_attempt_score, num_attempts, average_score) VALUES('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8})", 
						u.Id, u.Name, u.CreationDate, u.LastAttempt.Date, u.LastAttempt.Score, u.BestAttempt.Date, u.BestAttempt.Score, u.NumAttempts, u.AverageScore);
				
				GameManager.Instance.StateDb.ExecuteNonQuery(insertStatement);
				callback("user downloaded");
			}
		});
	}
	
	public static void PopulateAllUsers ()
	{
		AllUsers.Clear();
		
		DataTable dt = GameManager.Instance.StateDb.ExecuteQuery("SELECT * FROM Users;");
		foreach(DataRow dr in dt.Rows)
		{
			User u = new User();
			u.Id = (string)dr["id"];
			u.Name = (string)dr["name"];
			u.CreationDate = (double)dr["creation_date"];
			
			var lastAttemptDate = dr["last_attempt_date"];
			if (lastAttemptDate != null)
			{
				u.LastAttempt = new Attempt();
				u.LastAttempt.Date = (double)lastAttemptDate;
				u.LastAttempt.Score = (int)dr["last_attempt_score"];
				
				double bestAttemptDate = (double)dr["best_attempt_date"];
				if (bestAttemptDate == (double)lastAttemptDate)
				{
					u.BestAttempt = u.LastAttempt;
				}
				else
				{
					u.BestAttempt = new Attempt();
					u.BestAttempt.Date = bestAttemptDate;
					u.BestAttempt.Score = (int)dr["best_attempt_score"];
				}
				
				u.NumAttempts = (int)dr["num_attempts"];
				u.AverageScore = (double)dr["average_score"];
			}
			
			AllUsers.Add(u);
		}
		
		if (CurrentUser != null)
		{
			CurrentUser = AllUsers.FirstOrDefault(ur => ur.Name == CurrentUser.Name);
		}
	}
	
	public void EndAttempt (int score)
	{
		LastAttempt = new Attempt();
		LastAttempt.Date = UnixDate.Now;
		LastAttempt.Score = score;
		
		if (BestAttempt == null || BestAttempt.Score < LastAttempt.Score)
		{
			BestAttempt = LastAttempt;
		}
		
		AverageScore = ((AverageScore * NumAttempts) + score) / ++NumAttempts;
		
		GameManager.Instance.ActivityDb.ExecuteNonQuery(String.Format(
			"INSERT INTO Attempts(batch_id, id, user_id, date, score) VALUES('{0}','{1}','{2}',{3},{4})", 
			GameManager.Instance.ActivityBatchId, Guid.NewGuid().ToString(), Id, LastAttempt.Date, score));
		
		GameManager.Instance.StateDb.ExecuteNonQuery(String.Format(
			"UPDATE Users SET last_attempt_date={0}, last_attempt_score={1}, best_attempt_date={2}, best_attempt_score={3}, num_attempts={4}, average_score={5} WHERE id='{6}'",
			LastAttempt.Date, score, BestAttempt.Date, BestAttempt.Score, NumAttempts, AverageScore, Id));
	}
	
	private const string CheckUsernameAvailableURL = "http://178.79.147.20:3000/check-username-available";
	private const string DownloadUserURL = "http://178.79.147.20:3000/download-user";
	private static List<User> _allUsers = new List<User>();
	private static User _currentUser;
	
	static User ()
	{	
		User.PopulateAllUsers();
	}
}
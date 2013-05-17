using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SyncDataService {
	public const string SyncURL = "http://178.79.147.20:3000/sync";
	
	public bool IsSyncing { get { return _isSyncing; }}
	
	// TODO Make thread-safe
	public void Sync ()
	{
		if (_isSyncing) return;
		_isSyncing = true;
		
		SyncData data = new SyncData();
				
		DataTable urCreations = GameManager.Instance.ActivityDb.ExecuteQuery("SELECT * FROM UserCreations ORDER BY date");
		DataTable attempts = GameManager.Instance.ActivityDb.ExecuteQuery("SELECT * FROM Attempts ORDER BY date");
		DataTable loggedEvents = GameManager.Instance.ActivityDb.ExecuteQuery("SELECT * FROM LoggedEvents ORDER BY date");
		
		if (urCreations.Rows.Count == 0 && attempts.Rows.Count == 0 && loggedEvents.Rows.Count == 0)
		{
			_isSyncing = false;
			return;
		}
		
		GameManager.Instance.NewBatch();
		
		foreach (DataRow dr in urCreations.Rows)
		{
			ActivityUserCreation auc = new ActivityUserCreation(dr);
			data.userCreations.Add(auc);
			if (!data.batches.Contains(auc.batch)) data.batches.Add(auc.batch);	
		}		
		foreach (DataRow dr in attempts.Rows)
		{
			ActivityAttempt aa = new ActivityAttempt(dr);
			data.attempts.Add(aa);
			if (!data.batches.Contains(aa.batch)) data.batches.Add(aa.batch);
		}		
		foreach (DataRow dr in loggedEvents.Rows)
		{
			LoggedEvent ev = new LoggedEvent(dr);
			data.loggedEvents.Add(ev);
			if (!data.batches.Contains(ev.batch)) data.batches.Add(ev.batch);
		}
		
		string jsonString = JSONSerializer.Serialize<SyncData>(data);
		byte[] jsonBytes = (new System.Text.UTF8Encoding()).GetBytes(jsonString);
		
		Hashtable headers = new Hashtable();
		headers["Content-Type"] = "application/json";
		
		_httpService.SendWWWRequest(SyncURL, jsonBytes, headers, www =>
		{
			if (www.error == null)
			{
				// overwrite state db
				FileStream fs = new FileStream(GameManager.Instance.StateDbPath, FileMode.Create, FileAccess.ReadWrite);
	            fs.Write(www.bytes, 0, www.bytes.Length);
	            fs.Close();
				
				// delete sent activity
				GameManager.Instance.ActivityDb.ExecuteScript(
					string.Format("DELETE FROM UserCreations WHERE batch_id IN ('{0}'); DELETE FROM Attempts WHERE batch_id IN ('{0}'); DELETE FROM LoggedEvents WHERE batch_id IN ('{0}');", 
					string.Join("','", data.batches.ToArray())));
				
				// update state with extant user creations activity
				DataTable urCreationsSinceSync = GameManager.Instance.ActivityDb.ExecuteQuery("SELECT user_id, user_name, date FROM UserCreations");
				if (urCreationsSinceSync.Rows.Count > 0)
				{
					IEnumerable<string> insertStatements = urCreationsSinceSync.Rows.Select(
						dr => string.Format("INSERT INTO Users(id,name,creation_date) VALUES('{0}','{1}',{2});", (string)dr["user_id"], (string)dr["user_name"], (double)dr["date"]));
					
					GameManager.Instance.StateDb.ExecuteScript(string.Join("", insertStatements.ToArray()));
				}
				
				// repopulate AllUsers from state db
				User.PopulateAllUsers();
				
				// update state with extant Attempt activity
				DataTable attemptsSinceSync = GameManager.Instance.ActivityDb.ExecuteQuery("SELECT user_id, date, score FROM Attempts");
				if (attemptsSinceSync.Rows.Count > 0)
				{
					List<User> updatedUsers = new List<User>();
					foreach (DataRow dr in attemptsSinceSync.Rows)
					{
						User u = User.AllUsers.First(ur => ur.Id == (string)dr["user_id"]);
						if (!updatedUsers.Contains(u)) updatedUsers.Add(u);
						
						User.Attempt a = new User.Attempt();
						a.Date = (double)dr["date"];
						a.Score = (int)dr["score"];
						
						if (u.LastAttempt ==  null || u.LastAttempt.Date < a.Date) u.LastAttempt = a;
						if (u.BestAttempt == null || u.BestAttempt.Score < a.Score) u.BestAttempt = a;
						
						u.AverageScore = ((u.AverageScore * u.NumAttempts) + a.Score) / ++u.NumAttempts;
					}
					
					IEnumerable<string> updateStatements = updatedUsers.Select(ur => string.Format(
						"UPDATE Users SET last_attempt_date={0}, last_attempt_score={1}, best_attempt_date={2}, best_attempt_score={3}, num_attempts={4}, average_score={5} WHERE id='{6}';",
						ur.LastAttempt.Date, ur.LastAttempt.Score, ur.BestAttempt.Date, ur.BestAttempt.Score, ur.NumAttempts, ur.AverageScore, ur.Id));
					
					GameManager.Instance.StateDb.ExecuteScript(string.Join("", updateStatements.ToArray()));
				}
				
				Debug.Log("\nSYNC COMPLETE");
			}
			
			_isSyncing = false;
		});
	}
	
	public SyncDataService (HTTPService httpService)
	{
		_httpService = httpService;
	}
	
	private class SyncData
	{
		public string type = "SYNC_DATA";
		public string _id = System.Guid.NewGuid().ToString();
		public string installationId = GameManager.Instance.InstallationId;
		public double syncDate = UnixDate.Now;
		public List<string> batches = new List<string>();
		public List<ActivityUserCreation> userCreations = new List<ActivityUserCreation>();
		public List<ActivityAttempt> attempts = new List<ActivityAttempt>();
		public List<LoggedEvent> loggedEvents = new List<LoggedEvent>();
	}
	
	private class ActivityUserCreation
	{
		public string type = "ACTIVITY_USER_CREATION";
		public string installation = GameManager.Instance.InstallationId;
		public string batch, _id, userId, userName;
		public double date;
		
		public ActivityUserCreation (DataRow dr)
		{
			batch = (string)dr["batch_id"];
			_id = (string)dr["id"];
			userId = (string)dr["user_id"];
			userName = (string)dr["user_name"];
			date = (double)dr["date"];
		}
	}
	
	private class ActivityAttempt
	{
		public string type = "ACTIVITY_ATTEMPT";
		public string installation = GameManager.Instance.InstallationId;
		public string batch, _id, user;
		public double date;
		public int score;
		
		public ActivityAttempt (DataRow dr)
		{
			batch = (string)dr["batch_id"];
			_id = (string)dr["id"];
			user = (string)dr["user_id"];
			date = (double)dr["date"];
			score = (int)dr["score"];
		}
	}
	
	private class LoggedEvent
	{
		public string type = "LOGGED_EVENT";
		public string installationId = GameManager.Instance.InstallationId;
		public string batch, user, eventType, additionalData;
		public double date;
		
		public LoggedEvent(DataRow dr)
		{
			batch = (string)dr["batch_id"];
			user = (string)dr["user_id"];
			eventType = (string)dr["event_type"];
			additionalData = (string)dr["additional_data"];
			date = (double)dr["date"];
		}
	}
	
	private HTTPService _httpService;
	private bool _isSyncing = false;
}

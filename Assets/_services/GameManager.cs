using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using AlTypes;

public class GameManager
{
	#region "Public Static"
	public static GameManager Instance { get { return _instance; }}
	#endregion
	
	#region "Public Instance"
	public GameObject PersistentGameObject { get { return GameObject.Find(_persistentGOTag); }}
	public SyncDataService SyncService { get { return _syncService; }}
	public string ActivityDbPath { get { return _activityDbPath; }}
	public string StateDbPath { get { return _stateDbPath; }}
	public SqliteDatabase ActivityDb { get { return _activityDb; }}
	public SqliteDatabase StateDb { get { return _stateDb; }}
	public SqliteDatabase CmsDb {get {return _cmsDb;}}
	public string InstallationId { get { return _installationId; }}
	public string ActivityBatchId { get { return _activityBatchId; }}

	public SessionMgr SessionMgr {get {return _sessionMgr;}}
	
	//info about cms
	public int CmsWordCount=0;
	
	public void NewBatch ()
	{
		_activityBatchId = Guid.NewGuid().ToString();
	}
	
	public void LogState()
	{
		Debug.Log("game manager is okay");
	}

	public void LogEvent (string eventType, string userId, string additionalData)
	{
		ActivityDb.ExecuteNonQuery(String.Format(
			"INSERT INTO LoggedEvents(batch_id, date, event_type, user_id, additional_data) VALUES('{0}',{1},'{2}',{3},{4})", 
			ActivityBatchId, 
			UnixDate.Now,
			eventType,
			userId == null ? "null" : String.Format("'{0}'", userId),
			additionalData == null ? "null" : String.Format("'{0}'", additionalData)));
	}
	
	public String GetCMSInfo()
	{
		// DataTable dt = CmsDb.ExecuteQuery("SELECT * FROM words;");
		// CmsWordCount=dt.Rows.Count;
//		return "word count: " + dt.Rows.Count.ToString();
		
		// String[] tests=GetUserWordIndex();
		
		// List<String> yesp=GetSortedPhonemesForWord("fan");
		// for(int i=0; i<yesp.Count; i++)
		// {
		// 	Debug.Log("phoneme returned: " + yesp[i]);
		// }
		
		// return "iterate count: " + tests.Length + " with word: " + tests[41];

		return "cms info queries disabled";
	}
	
	public String[] GetUserWordIndex()
	{
		// DataTable dt=CmsDb.ExecuteQuery("select word from words where cvc='t' and diagraph='f'");
		// String[] words=new String[dt.Rows.Count];
		// for(int i=0; i<dt.Rows.Count; i++)
		// {
		// 	words[i]=(String)dt.Rows[i]["word"];
		// }

		String[] words=new String[38];
		words[0]="cat";
		words[1]="sat";
		words[2]="sit";
		words[3]="sap";
		words[4]="pig";	
		words[5]="dog";
		words[6]="rat";
		words[7]="hen";
		words[8]="tap";
		words[9]="map";
		words[10]="pot";
		words[11]="sun";
		words[12]="pen";
		words[13]="bat";
		words[14]="pip";
		words[15]="king";
		words[16]="sock";
		words[17]="troll";
		words[18]="pat";
		words[19]="jam";
		words[20]="van";
		words[21]="hug";
		words[22]="pin";
		words[23]="tin";
		words[24]="pit";
		words[25]="man";
		words[26]="mug";
		words[27]="mad";
		words[28]="dad";
		words[29]="sad";
		words[30]="hat";
		words[31]="tag";
		words[32]="goat";
		words[33]="cap";
		words[34]="kick";
		words[35]="elf";
		words[36]="bug";
		words[37]="big";

		return words;
	}

	public PhonemeData GetPhonemeInfoForPhoneme(String phoneme)
	{
		PhonemeData rdata=new PhonemeData();
		DataTable dtp=CmsDb.ExecuteQuery("select phoneme,mneumonic,mneumonic_two,grapheme from phonemes where phoneme='"+phoneme+"'");
		
		rdata.Phoneme=(String)dtp.Rows[0]["phoneme"];
		rdata.Mneumonic=(String)dtp.Rows[0]["mneumonic"];

		if(dtp.Rows[0]["mneumonic_two"]!=null)
			rdata.MneumonicTwo=(String)dtp.Rows[0]["mneumonic_two"];

		rdata.Grapheme=(String)dtp.Rows[0]["grapheme"];

		Debug.Log("rdata phoneme="+rdata.Phoneme+"// rdata mneumonic "+rdata.Mneumonic +"// rdata grapheme"+rdata.Grapheme);

		return rdata;
	}
	
	public List<PhonemeData> GetPhonemesForWord(String word)
	{
		//get the word
		DataTable dt=CmsDb.ExecuteQuery("select id from words where word='" + word + "'");
		int wordid=(int)dt.Rows[0]["id"];
		
		//get the phonemes
		DataTable dtp=CmsDb.ExecuteQuery("select * from phonemes p INNER JOIN phonemes_words pw ON p.id=pw.phoneme_id WHERE pw.word_id=" + wordid.ToString());

		// String[] phonemes=new String[dtp.Rows.Count];

		List<PhonemeData> phonemes=new List<PhonemeData>();

		for(int i=0; i<dtp.Rows.Count; i++)
		{
			//get phoneme
			String pr=(String)dtp.Rows[i]["phoneme"];

			int ih=pr.IndexOf("-");
			if(ih>0){
				String pr1=pr.Substring(0, ih);
				String pr2=pr.Substring(ih+1, 1);

				PhonemeData p1d=PDataForRow(dtp.Rows[i]);
				p1d.LetterInWord=pr1;

				PhonemeData p2d=PDataForRow(dtp.Rows[i]);
				p2d.LetterInWord=pr2;

				phonemes.Add(p1d);
				phonemes.Add(p2d);
			}
			else
			{
				PhonemeData pd=PDataForRow(dtp.Rows[i]);
				pd.LetterInWord=pr;
				phonemes.Add(pd);
			}
		}
		return phonemes;
	}

	PhonemeData PDataForRow(DataRow dr)
	{
		Debug.Log("dr: " + dr);
		PhonemeData pd=new PhonemeData();
		pd.Phoneme=(String)dr["phoneme"];
		pd.Mneumonic=(String)dr["mneumonic"];
		pd.MneumonicTwo=(String)dr["mneumonic_two"];
		pd.Grapheme=(String)dr["grapheme"];
		pd.Id=(int)dr["id"];

		// Debug.Log("created pd with grapheme: " + pd.Grapheme);

		return pd;
	}

	static bool IsIPad1()
	{
		return (SystemInfo.deviceModel=="iPad1,1");
	}

	public DataWordData[] GetDataWordsForSection(int sectionId)
	{
		DataTable dt=CmsDb.ExecuteQuery("select * from data_words INNER JOIN words ON word_id=words.id WHERE section_id="+sectionId.ToString());
		DataWordData[] dws=new DataWordData[dt.Rows.Count];
		for(int i=0; i<dt.Rows.Count; i++)		
		{
			DataWordData dw;
			dw.Word=(String)dt.Rows[i]["word"];
			dw.WordId=(int)dt.Rows[i]["word_id"];
			dw.Nonsense=IsBool((String)dt.Rows[i]["nonsense"]);
			dw.IsTargetWord=IsBool((String)dt.Rows[i]["is_target_word"]);
			dw.IsDummyWord=IsBool((String)dt.Rows[i]["is_dummy_word"]);
			dw.LinkingIndex=(String)dt.Rows[i]["linking_index"];

			dws[i]=dw;
		}
		return dws;
	}
	
	public PhonemeData[] GetPhonemesForSection(int sectionId)
	{
		DataTable dt=CmsDb.ExecuteQuery("select id, phoneme, mneumonic, mneumonic_two from phonemes_sections INNER JOIN phonemes ON phoneme_id=id WHERE section_id="+sectionId.ToString());
		PhonemeData[] phonemes=new PhonemeData[dt.Rows.Count];
		for(int i=0; i<dt.Rows.Count; i++)
		{
			PhonemeData pd=new PhonemeData();
			pd.Phoneme=(String)dt.Rows[i]["phoneme"];
			pd.Mneumonic=(String)dt.Rows[i]["mneumonic"];
			pd.MneumonicTwo=(String)dt.Rows[i]["mneumonic_two"];
			pd.Grapheme=(String)dt.Rows[i]["grapheme"];
			pd.Id=(int)dt.Rows[i]["id"];
			phonemes[i]=pd;
		}
		return phonemes;
	}

	public String[] AllMneumonics()
	{
		DataTable dt=CmsDb.ExecuteQuery("select mneumonic, mneumonic_two from phonemes ORDER BY mneumonic ASC");
		ArrayList mnary=new ArrayList();
		for(int i=0; i<dt.Rows.Count; i++)
		{
			String s1=(String)dt.Rows[i]["mneumonic"];
			String s2=(String)dt.Rows[i]["mneumonic_two"];

			if(s1!=null && s1!="") mnary.Add(s1);
			//if(s2!=null && s2!="") mnary.Add(s2);
		}
		String[] mns=new String[mnary.Count];
		int j=0;
		foreach(String s in mnary)
		{
			mns[j]=s;
			j++;
		}
		Array.Sort (mns);
		return mns;
	}

	public DataSentenceData[] GetDataSentencesForSection(int sectionId)
	{
		DataTable dt=CmsDb.ExecuteQuery("select * from sentences WHERE section_id="+sectionId.ToString());
		DataSentenceData[] sentences=new DataSentenceData[dt.Rows.Count];
		for(int i=0; i<dt.Rows.Count; i++)
		{
			DataSentenceData dsd;
			dsd.Sentence=(String)dt.Rows[i]["text"];
			dsd.LinkingIndex=(String)dt.Rows[i]["linking_index"];
			sentences[i]=dsd;
		}
		return sentences;
	}

	public bool IsBool(String data)
	{
		if(data=="t")return true;
		else return false;
	}


	public String[] GetUserLetters()
	{
		String[] letters=new String[26];
		int current=0;
		for(int i=97;i<123;i++)
		{
			char c=(char)i;
			letters[current]=c.ToString();
			current++;
		}
		
		return letters;
	}
	

	public List<PhonemeData> GetSortedPhonemesForWord(String word)
	{
		if(word.Length==0) return null;
	
		List<PhonemeData> unsortedPhonemes=GetPhonemesForWord(word);
		List<PhonemeData> sortedPhonemes=new List<PhonemeData>();

		String remWord=word;

		do {
			bool found=false;

			foreach(PhonemeData pd in unsortedPhonemes)
			{
				if(remWord.IndexOf(pd.LetterInWord)==0)
				{
					sortedPhonemes.Add(pd);
					unsortedPhonemes.Remove(pd);
					found=true;

					//remove that letter bit from the word
					int secLen=pd.LetterInWord.Length;
					if(secLen<remWord.Length)
					{
						remWord=remWord.Substring(secLen, remWord.Length-secLen);
					}
					else remWord="";

					break;
				}
			}

			if(!found)
			{
				Debug.Log("no phoneme found to match word at remWord: " + remWord);
				break;
			}

		} while (remWord.Length>0);

		//while remaining word>0

		//step each phoneme and match its lettersInWord to the begining of the remaining word (i.e. find the one that matched at pos 0)

		//put that next on the sorted list, remove from unsorted

		return sortedPhonemes;		
	}

	public StoryPageData GetStoryPageFor(int storyID, int pageID)
	{
		StoryPageData thisPage=new StoryPageData();

		DataTable dt=CmsDb.ExecuteQuery("select text,audio,textposition from storypages where story_id='"+storyID+"' and pageorder='"+pageID+"'");

		thisPage.AnchorPoint=(String)dt.Rows[0]["textposition"];
		thisPage.PageText=(String)dt.Rows[0]["text"];
		thisPage.AudioName=(String)dt.Rows[0]["audio"];

		return thisPage;
	}

	// public List<String> GetStringSortedPhonemesForWord(String word)
	// {
	// 	if(word=="acid") return new List<String>{"a", "c", "i", "d"};
	// 	if(word=="mole") return new List<String>{"m", "o-e", "l", "o-e"};
	// 	if(word=="super") return new List<String>{"s", "u", "p", "er"};
	// 	if(word=="vulture") return new List<String>{"v", "u", "l", "t", "ure"};
	// 	if(word=="air") return new List<String>{"air"};


	// 	if(word.Length==0) return null;
		
	// 	String[]unsortedPhonemes=GetPhonemesForWord(word);
	// 	if(unsortedPhonemes.Length==0) return null;
		
	// 	String wordRemainder=word;
		
	// 	//get a size (desc) sorted list of phonemes
	// 	List<String>sizeDescPhonemes=new List<String>();
	// 	sizeDescPhonemes.Add(unsortedPhonemes[0]);
	// 	if(unsortedPhonemes.Length>1)
	// 	{
	// 		for(int j=1;j<unsortedPhonemes.Length; j++)
	// 		{
	// 			String p=unsortedPhonemes[j];
	// 			int insertAt=sizeDescPhonemes.Count;
	// 			for(int i=0; i<sizeDescPhonemes.Count; i++)
	// 			{
	// 				if(p.Length>=sizeDescPhonemes[i].Length)
	// 				{
	// 					insertAt=i;
	// 					break;
	// 				}
	// 			}
	// 			sizeDescPhonemes.Insert(insertAt, p);
	// 		}
	// 	}
		
	// 	int ip=0;
	// 	int notFoundCount=0;
	// 	List<String>sortedPhonemes=new List<String>();
	// 	while (wordRemainder.Length>0 && notFoundCount<=sizeDescPhonemes.Count) {
	// 		if(wordRemainder.IndexOf(sizeDescPhonemes[ip])==0)
	// 		{
	// 			//insert this in sorted index & remove from unsorted (and front of word)
	// 			sortedPhonemes.Add(sizeDescPhonemes[ip]);
	// 			wordRemainder=wordRemainder.Substring (sizeDescPhonemes[ip].Length);
	// 			notFoundCount=0;
	// 		}
	// 		else
	// 		{
	// 			notFoundCount++;
	// 		}

	// 		ip++;

	// 		if(ip>=sizeDescPhonemes.Count) 
	// 		{
	// 			ip=0;
	// 		}
	// 	}
	
	// 	return sortedPhonemes;
	// 	// return sizeDescPhonemes;
	// }
	
	#endregion
	
	#region "Private Static"
	private static readonly GameManager _instance = new GameManager();
	#endregion
	
	#region "Private Instance"
	private string _persistentGOTag = "PersistentGO";
	private string _installationIdPath = Path.Combine(Application.persistentDataPath, "installationid");
	private string _activityDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "activity.db");
	private string _stateDbPath = Path.Combine(UnityEngine.Application.persistentDataPath, "state.db");
	private string _cmsDbPath=Path.Combine(UnityEngine.Application.persistentDataPath, "cms.db");
	
	private SyncDataService _syncService;
	private SqliteDatabase _activityDb;
	private SqliteDatabase _stateDb;
	private SqliteDatabase _cmsDb;
	private string _installationId, _activityBatchId;

	private SessionMgr _sessionMgr;
	
	private GameManager()
	{
		GameObject go = new GameObject(_persistentGOTag);
		MonoBehaviour.DontDestroyOnLoad(go);
		go.AddComponent("HTTPService");
		
		_syncService = new SyncDataService((HTTPService)PersistentGameObject.GetComponent("HTTPService"));
		
		if (File.Exists(_installationIdPath))
		{
			_installationId = File.ReadAllText(_installationIdPath);
		}
		else
		{
			_installationId = Guid.NewGuid().ToString();
			StreamWriter sw = File.CreateText(_installationIdPath);
			sw.Write(_installationId);
			sw.Close();
		}
		
		bool initActivityDb = !File.Exists(_activityDbPath);
		bool initStateDb = !File.Exists(_stateDbPath);
		bool initCmsDb=!File.Exists(_cmsDbPath);
		
		_activityDb = new SqliteDatabase();
		_stateDb = new SqliteDatabase();
		_cmsDb=new SqliteDatabase();
		
		_activityDb.Open(_activityDbPath);
		_stateDb.Open(_stateDbPath);
		
		if (initActivityDb)
		{
			ActivityDb.ExecuteNonQuery("CREATE TABLE UserCreations(batch_id TEXT NOT NULL, id TEXT NOT NULL, user_id TEXT NOT NULL, user_name TEXT NOT NULL, date REAL)");
			ActivityDb.ExecuteNonQuery("CREATE TABLE Attempts(batch_id TEXT NOT NULL, id TEXT NOT NULL, user_id TEXT NOT NULL, date REAL, score INTEGER)");
			ActivityDb.ExecuteNonQuery("CREATE TABLE LoggedEvents(batch_id TEXT NOT NULL, date REAL, event_type TEXT NOT NULL, user_id TEXT, additional_data TEXT)");
		}
		
		if (initStateDb)
		{
			StateDb.ExecuteNonQuery("CREATE TABLE Users(id TEXT NOT NULL, name TEXT NOT NULL, creation_date REAL, last_attempt_date REAL, last_attempt_score INTEGER, best_attempt_date REAL, best_attempt_score INTEGER, num_attempts INTEGER, average_score REAL)");
		}

		//always write cms db
		TextAsset ta=(TextAsset)Resources.Load("cms");
		System.IO.File.WriteAllBytes(_cmsDbPath, ta.bytes);
		
		// if(initCmsDb)
		// {
		// 	TextAsset ta=(TextAsset)Resources.Load("cms");
		// 	System.IO.File.WriteAllBytes(_cmsDbPath, ta.bytes);
		// }
		
		_cmsDb.Open(_cmsDbPath);
		
		//log it and get the count on the instance
		Debug.Log(GetCMSInfo());
	
		
		NewBatch();

		_sessionMgr= new SessionMgr();
		_sessionMgr.LogState();
	}
	#endregion
}
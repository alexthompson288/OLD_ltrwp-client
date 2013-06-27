using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using AlTypes;

public class SessionMgr
{
	ArrayList _sectionList;
	int activityIndex;
	bool debugMode = false;
	PhonemeData[] _currentPhonemes;
	DataWordData[] _currentDataWords;
	DataSentenceData[] _currentDataSentences;
	string _currentGameSetting;

	public PhonemeData[] CurrentPhonemes { get { return _currentPhonemes; } }

	public DataWordData[] CurrentDataWords { get { return _currentDataWords; } }

	public DataSentenceData[] CurrentDataSentences { get { return _currentDataSentences; } }

	public string CurrentGameSetting { get { return _currentGameSetting; } }

	public SessionMgr ()
	{

	}

	public void LogState ()
	{
		Debug.Log ("session manager is okay");
	}

	public void LogSections ()
	{

		foreach (Hashtable hash in _sectionList) {
			Debug.Log ("thingy id " + hash ["test_id"]);
		}
	}

	public Hashtable CurrentSectionHash ()
	{
		return (Hashtable)_sectionList [activityIndex];
	}

	public void StartActivity ()
	{
		Hashtable ahash = (Hashtable)_sectionList [activityIndex];
		Debug.Log ("starting activity " + ahash ["test_id"]);

		int sectionId = (int)ahash ["section_id"];
		_currentGameSetting = (string)ahash ["game_setting"];
		_currentPhonemes = GameManager.Instance.GetPhonemesForSection (sectionId);
		_currentDataWords = GameManager.Instance.GetDataWordsForSection (sectionId);
		_currentDataSentences = GameManager.Instance.GetDataSentencesForSection (sectionId);

		String gs = (String)ahash ["game_scene"];
		String loadgs = gs;
		switch (gs) {
		case "introduce_phoneme":
			loadgs = "introduce_phoneme";
			break;
		case "video_intro":
			loadgs = "VideoIntro";
			break;
		case "module1_intro":
			loadgs = "Splat-SAT";
			break;
		case "splat":
			loadgs= "Splat-SAT";
			break;
		case "splat_the_rat":
			loadgs="SplatTheRat PX";
			break;
		case "splat_the_rat_keyword":
			loadgs="SplatTheRat PX";
			break;
		case "learn_phoneme_single_letter":
			loadgs="IntroducePhoneme P newbg";
			break;
		case "choose_correct_path":
			loadgs="CorrectPath";
			break;
		case "choose_correct_path_2":
			loadgs="CorrectPath2";
			break;
		case "eyespy_alliteration":
			loadgs="EyeSpy";
			break;
		case "skillintroduction_alliteration":
			loadgs="SkillIntroduction_Alliteration";
			break;
		case "skillintroduction_phoneme":
			loadgs="SkillIntroduction_LearnPhoneme";
			break;
		case "skillintroduction_oralblending":
			loadgs="SkillIntroduction_OralBlending";
			break;
		case "skillintroduction_blending":
			loadgs="SkillIntroduction_Blending";
			break;
		case "skillintroduction_keyword":
			loadgs="SkillIntroduction_Keyword";
			break;
		case "skillintroduction_segmenting":
			loadgs="SkillIntroduction_Segmenting";
			break;
		case "skillintroduction_letterformation":
			loadgs="SkillIntroduction_LetterFormation";
			break;
		case "skillintroduction_rhyming":
			loadgs="SkillIntroduction_Rhyming";
			break;
		case "word_bank":
			loadgs="WordBank";
			break;			
		case "letter_formation":
			loadgs="letter_formation";
			break;
		case "cross_the_river":
			loadgs="CrossTheBridge";
			break;

		default:
				//load generic scene
			loadgs = "gameinfo";

			Debug.Log ("loading unknown game scene: " + gs);
			break;
		}

		Application.LoadLevel (loadgs);
	}

	public void CloseActivity ()
	{
		activityIndex++;

		if (debugMode) {
			//always go back to debug session interstitial
		} else {
			if (activityIndex >= _sectionList.Count) {
				//todo: unload session, bail to menu
				Debug.Log ("run out of activities");
				
				Application.LoadLevel ("contentbrowser-full");
			} else {
				//goto next activity
				StartActivity ();
			}
		}
	}
	
	public void StartSession (int sessionId)
	{
		_sectionList = new ArrayList ();
		activityIndex = 0;
		_currentPhonemes = null;
		_currentDataSentences = null;
		_currentDataWords = null;

		Hashtable hash;
		
		PersistentObject PersistentManager=GameObject.Find ("PersistentManager").GetComponent<PersistentObject>();
		PersistentManager.ContentBrowserName="ContentBrowser-Scrolling";
		bool gotDebugSession=false;
		
		if(sessionId==9991)
		{
			PersistentManager.CurrentLetter="a";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "IntroducePhoneme A");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "Splat-SAT");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "SplatTheRat PX");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "AlphabetBook");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9989)
		{
			PersistentManager.CurrentLetter="a";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 414); 
			hash.Add ("game_scene", "OddOneOut");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9988)
		{
			PersistentManager.CurrentLetter="a";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 37); 
			hash.Add ("game_scene", "eyespy_alliteration");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 48); 
			hash.Add ("game_scene", "choose_correct_path_oral_blending");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9987)
		{
			PersistentManager.CurrentLetter="a";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 49); 
			hash.Add ("game_scene", "learn_phoneme_single_letter");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9992)
		{
			PersistentManager.CurrentLetter="t";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "IntroducePhoneme T");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "Splat-SAT");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "SplatTheRat PX");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "AlphabetBook");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9993)
		{
			PersistentManager.CurrentLetter="s";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "IntroducePhoneme S");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "Splat-SAT");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "SplatTheRat PX");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "AlphabetBook");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9994)
		{
			PersistentManager.CurrentLetter="p";	
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "IntroducePhoneme P");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "Splat-SAT");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "SplatTheRat PX");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
			
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "AlphabetBook");
			hash.Add ("game_setting", "castle_frames"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9995)
		{
			PersistentManager.WordBankWord="acid";
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "word_ladder"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9996)
		{
			PersistentManager.WordBankWord="mole";
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "word_ladder"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9997)
		{
			PersistentManager.WordBankWord="vulture";
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "word_ladder"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9998)
		{
			PersistentManager.WordBankWord="super";
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "word_ladder"); 
			_sectionList.Add (hash); 
		}
		else if(sessionId==9999)
		{
			PersistentManager.WordBankWord="air";
			gotDebugSession=true;
			hash = new Hashtable (); 
			hash.Add ("section_id", 9); 
			hash.Add ("game_scene", "word_ladder"); 
			_sectionList.Add (hash); 
		}


		// THIS IS WHERE OUR DEMO STARTS

		else if(sessionId==9001)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 1); 
            hash.Add("game_scene", "video_intro");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9002)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 1251); 
            hash.Add("game_scene", "module1_intro");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9003)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 2); 
            hash.Add("game_scene", "skillintroduction_alliteration");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9004)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 35); 
            hash.Add("game_scene", "eyespy_alliteration");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9005)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 38); 
            hash.Add("game_scene", "skillintroduction_phoneme");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9006)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 39); 
            hash.Add("game_scene", "introduce_phoneme");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9007)
		{
			PersistentManager.KeywordGame=false;
			PersistentManager.CurrentLetter="s";
            hash=new Hashtable(); 
            hash.Add("section_id", 39); 
            hash.Add("game_scene", "splat_the_rat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9008)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 43); 
            hash.Add("game_scene", "skillintroduction_oralblending");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9009)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 44); 
            hash.Add("game_scene", "cross_the_river");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9010)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 49); 
            hash.Add("game_scene", "introduce_phoneme");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9011)
		{
			PersistentManager.CurrentLetter="a";
			PersistentManager.KeywordGame=false;
            hash=new Hashtable(); 
            hash.Add("section_id", 49); 
            hash.Add("game_scene", "splat_the_rat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9012)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 368); 
            hash.Add("game_scene", "splat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9013)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 48); 
            hash.Add("game_scene", "choose_correct_path");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9014)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 373); 
            hash.Add("game_scene", "skillintroduction_blending");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9015)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 370); 
            hash.Add("game_scene", "introduce_phoneme");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9016)
		{
			PersistentManager.KeywordGame=false;
			PersistentManager.CurrentLetter="t";
            hash=new Hashtable(); 
            hash.Add("section_id", 370); 
            hash.Add("game_scene", "splat_the_rat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9017)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 53); 
            hash.Add("game_scene", "splat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9018)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 381); 
            hash.Add("game_scene", "skillintroduction_keyword");

            _sectionList.Add(hash); 
		}
		else if(sessionId==9019)
		{
			PersistentManager.KeywordGame=true;
			PersistentManager.CurrentLetter="a";
            hash=new Hashtable(); 
            hash.Add("section_id", 382); 
            hash.Add("game_scene", "splat_the_rat_keyword");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9020)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 386); 
            hash.Add("game_scene", "skillintroduction_segmenting");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9021)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 388); 
            hash.Add("game_scene", "word_bank");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9022)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 55); 
            hash.Add("game_scene", "introduce_phoneme");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9023)
		{
			PersistentManager.KeywordGame=false;
			PersistentManager.CurrentLetter="p";
            hash=new Hashtable(); 
            hash.Add("section_id", 55); 
            hash.Add("game_scene", "splat_the_rat");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9024)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 60); 
            hash.Add("game_scene", "choose_correct_path");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9025)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 389); 
            hash.Add("game_scene", "skillintroduction_letterformation");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9026)
		{
			PersistentManager.CurrentLetter="s";
            hash=new Hashtable(); 
            hash.Add("section_id", 1219); 
            hash.Add("game_scene", "letter_formation");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9027)
		{
			PersistentManager.KeywordGame=true;
			PersistentManager.CurrentLetter="the";
            hash=new Hashtable(); 
            hash.Add("section_id", 411); 
            hash.Add("game_scene", "splat_the_rat_keyword");
            _sectionList.Add(hash); 
		}
		else if(sessionId==9028)
		{
            hash=new Hashtable(); 
            hash.Add("section_id", 89); 
            hash.Add("game_scene", "skillintroduction_rhyming");
            _sectionList.Add(hash); 
		}

		if(gotDebugSession)
			return;
		
            if(sessionId == 1){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 9); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 15); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 20); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 26); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 13); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 2){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 10); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 18); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 22); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 31); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 29); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 3){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 11); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 16); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 21); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 27); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 14); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 4){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 12); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 19); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 23); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 28); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 30); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 5){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 368); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 369); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 370); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 371); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 372); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 17){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 2); 
            hash.Add("game_scene", "alliteration_introduction");
            hash.Add("game_setting", "forest_alliteration_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 35); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            hash.Add("game_scene", "eyespy_alliteration");
            hash.Add("game_setting", "forest_eyespy_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 36); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 414); 
            hash.Add("game_scene", "odd_one_out_alliteration");
            hash.Add("game_setting", "forest_odd_one_out_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 722); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 18){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 37);  
            hash.Add("game_scene", "eyespy_alliteration");
            hash.Add("game_setting", "forest_eyespy_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 38); 
            hash.Add("game_scene", "phoneme_introduction");
            hash.Add("game_setting", "forest_phoneme_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 39); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 40); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 41); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 19){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 42); 
            hash.Add("game_scene", "odd_one_out_alliteration");
            hash.Add("game_setting", "forest_odd_one_out_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 43); 
            hash.Add("game_scene", "rhyming_cauldron_oral_blending");
            hash.Add("game_setting", "forest_rhyming_cauldron_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 44); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 45); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "forest_match_illustrations" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 46); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 20){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 47); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 48); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 49); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 50); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 51); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 37){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 53); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 54); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 55); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 56); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 57); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 38){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 59); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 60); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 373); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 61); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 62); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 39){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 374); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 375); 
            hash.Add("game_scene", "odd_one_out_alliteration");
            hash.Add("game_setting", "forest_odd_one_out_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 376); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 377); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 378); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 40){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 379); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 380); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 381); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 382); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 383); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "forest_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 41){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 384); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 385); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 386); 
            hash.Add("game_scene", "segmenting_introduction");
            hash.Add("game_setting", "forest_segmenting_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 387); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 388); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 42){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 389); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 390); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 391); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 392); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 393); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 43){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 394); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 395); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 396); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 397); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 398); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 44){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 399); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 400); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 401); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 402); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 403); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 45){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 404); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 405); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 406); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 407); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 408); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 46){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 409); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 410); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 411); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 412); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 413); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 51){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 63); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 64); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 65); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 66); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 67); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "forest_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 52){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 68); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 69); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 70); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 71); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 72); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 53){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 73); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 74); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 75); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 76); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 77); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 54){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 78); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 79); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 80); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 81); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 82); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 55){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 83); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 84); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 85); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 86); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 87); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 56){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 88); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 89); 
            hash.Add("game_scene", "rhyming_introduction");
            hash.Add("game_setting", "forest_rhyming_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 90); 
            hash.Add("game_scene", "rhyming_cauldron_oral_blending");
            hash.Add("game_setting", "forest_rhyming_cauldron_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 91); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "forest_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 92); 
            hash.Add("game_scene", "missing_rhyming_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_rhyming_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 57){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 93); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 94); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 95); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 96); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "forest_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 97); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 58){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 98); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 99); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 100); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 101); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 102); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 59){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 103); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 104); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 105); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 106); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 107); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 60){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 108); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 109); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 110); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 111); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 112); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 61){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 113); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 114); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 115); 
            hash.Add("game_scene", "silly_sensible_sentences_reading");
            hash.Add("game_setting", "forest_silly_sensible_sentences_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 116); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 117); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 62){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 118); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 119); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 120); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 121); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 122); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 63){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 123); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 124); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 125); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 126); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "forest_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 127); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 64){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 128); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 129); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 130); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 131); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 132); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 65){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 133); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "forest_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 134); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 135); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "alien_sound_buttons_basic_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 136); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 137); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 66){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 138); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 139); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 140); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 141); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 142); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "forest_label_images_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 67){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 143); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 144); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 145); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 146); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 147); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 68){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 148); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 149); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 150); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 151); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "forest_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 152); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 69){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 153); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 154); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 155); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 156); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 157); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 70){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 158); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 159); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 160); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 161); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 162); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 71){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 163); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 164); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 165); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "forest_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 166); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 167); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "forest_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 72){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 168); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 169); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 170); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 171); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 172); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 73){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 173); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 174); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 175); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 176); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "forest_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 177); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 74){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 178); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 179); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 180); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 181); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 182); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 75){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 183); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 184); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 185); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 186); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 187); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 76){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 188); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 189); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 190); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 191); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 192); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "forest_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 77){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 193); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 194); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 195); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 196); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 197); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 78){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 198); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 199); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 200); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 201); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "forest_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 202); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 79){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 203); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 204); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 205); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 206); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 207); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 80){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 208); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 209); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 210); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 211); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 212); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 81){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 213); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 214); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 215); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 216); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 217); 
            hash.Add("game_scene", "silly_sensible_sentences_reading");
            hash.Add("game_setting", "forest_silly_sensible_sentences_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 82){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 218); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 219); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 220); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 221); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 222); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 83){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 223); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 224); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 225); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "alien_sound_buttons_basic_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 226); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 227); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 84){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 228); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 229); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 230); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 231); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 232); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 85){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 233); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 234); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 235); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 236); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 237); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 86){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 238); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 239); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 240); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "alien_sound_buttons_basic_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 241); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 242); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 588); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "underwater_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 87){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 243); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 244); 
            hash.Add("game_scene", "odd_one_out_alliteration");
            hash.Add("game_setting", "forest_odd_one_out_alliteration" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 245); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 246); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 247); 
            hash.Add("game_scene", "silly_sensible_sentences_reading");
            hash.Add("game_setting", "forest_silly_sensible_sentences_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 88){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 248); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 249); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 250); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 251); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 252); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "forest_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 89){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 253); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 254); 
            hash.Add("game_scene", "alliteration_cauldron");
            hash.Add("game_setting", "forest_alliteration_cauldron" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 255); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "forest_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 256); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "forest_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 257); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 90){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 258); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 259); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 260); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 261); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "forest_sentence_jumble" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 262); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 91){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 263); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 264); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "forest_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 265); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "forest_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 266); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "forest_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 267); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "forest_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 92){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 268); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 269); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 270); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 271); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 272); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 474); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 93){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 273); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 274); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 275); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "alien_sound_buttons_basic_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 276); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "forest_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 277); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 94){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 278); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 279); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 280); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 281); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 282); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 95){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 283); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 284); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 285); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 286); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 287); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 96){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 288); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 289); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "forest_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 290); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 291); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "forest_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 292); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 97){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 293); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 294); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 295); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 296); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 297); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 98){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 298); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 299); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 300); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 301); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 302); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 99){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 303); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 304); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 305); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 306); 
            hash.Add("game_scene", "match_images_and_labels");
            hash.Add("game_setting", "forest_match_images_and_labels" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 307); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 100){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 308); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 309); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 310); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 311); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 312); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 101){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 313); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 314); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 315); 
            hash.Add("game_scene", "keyword_introduction");
            hash.Add("game_setting", "forest_keyword_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 316); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "forest_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 317); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 102){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 318); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 319); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 320); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 321); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 322); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 103){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 323); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 324); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 325); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 326); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 327); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 104){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 328); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 329); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 330); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 331); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "forest_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 332); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 105){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 333); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 334); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 335); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 336); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 337); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 106){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 338); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 339); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_frames" ); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 340); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "forest_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 341); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "forest_make_a_sentence" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 342); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "forest_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 107){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 343); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 344); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 345); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 346); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "forest_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 347); 
            hash.Add("game_scene", "alphabet_page_blend");
            hash.Add("game_setting", "forest_alphabet_page_blend" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 108){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 348); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "forest_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 349); 
            hash.Add("game_scene", "letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 350); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 351); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 352); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 109){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 353); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 354); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "forest_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 355); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 356); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "forest_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 357); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "forest_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 110){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 358); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "forest_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 359); 
            hash.Add("game_scene", "eyespy_blending");
            hash.Add("game_setting", "forest_eyespy_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 360); 
            hash.Add("game_scene", "sound_buttons_basic_reading");
            hash.Add("game_setting", "castle_frames" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 361); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 362); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 111){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 363); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "forest_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 364); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "forest_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 365); 
            hash.Add("game_scene", "cross_the_river_oral_blending");
            hash.Add("game_setting", "forest_cross_the_river_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 366); 
            hash.Add("game_scene", "silly_sensible_sentences_reading");
            hash.Add("game_setting", "forest_silly_sensible_sentences_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 367); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "forest_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 112){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 415); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 416); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "underwater_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 417); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "castle_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 418); 
            hash.Add("game_scene", "keyword_spelling_introduction");
            hash.Add("game_setting", "forest_keyword_spelling_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 419); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 113){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 420); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 421); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "castle_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 422); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 423); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 424); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 114){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 425); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 426); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "castle_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 427); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "castle_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 428); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 429); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 115){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 430); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 431); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "castle_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 432); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 434); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 435); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 116){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 436); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 437); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 438); 
            hash.Add("game_scene", "2_syllable_words_introduction");
            hash.Add("game_setting", "underwater_2_syllable_words_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 439); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 440); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 117){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 441); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 442); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 443); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "underwater_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 444); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "underwater_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 445); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "underwater_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 118){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 451); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 452); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 453); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 454); 
            hash.Add("game_scene", "rhyming_cauldron_oral_blending");
            hash.Add("game_setting", "underwater_rhyming_cauldron_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 455); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "underwater_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 119){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 456); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 457); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 458); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 459); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 460); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 120){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 461); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "underwater_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 462); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "underwater_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 463); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "underwater_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 464); 
            hash.Add("game_scene", "missing_rhyming_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_rhyming_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 465); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "underwater_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 121){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 446); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 447); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 448); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 449); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "underwater_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 450); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 122){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 466); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 467); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 468); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "underwater_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 469); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "underwater_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 470); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 123){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 471); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 472); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 473); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 475); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 124){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 476); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 477); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 478); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "underwater_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 479); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "underwater_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 480); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "underwater_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 125){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 481); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 482); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 483); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 484); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "underwater_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 485); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "underwater_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 126){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 486); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 487); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 488); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "underwater_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 489); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 490); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 127){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 491); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 492); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 493); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "underwater_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 494); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "underwater_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 495); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 128){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 496); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 497); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 498); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "underwater_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 499); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 500); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 652); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "castle_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 129){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 501); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 502); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 503); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "underwater_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 504); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "underwater_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 505); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "underwater_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 130){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 506); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 507); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 508); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 509); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 510); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 131){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 511); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "underwater_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 512); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "underwater_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 513); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "underwater_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 514); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "underwater_sentence_jumble" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 515); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 132){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 516); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 517); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 518); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "underwater_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 519); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "underwater_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 520); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 133){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 521); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 522); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 523); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 524); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "underwater_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 525); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "underwater_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 134){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 526); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 527); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 528); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "underwater_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 529); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "underwater_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 530); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "underwater_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 682); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 135){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 531); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 532); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 533); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 534); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 535); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 136){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 536); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "underwater_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 537); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "underwater_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 538); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "underwater_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 539); 
            hash.Add("game_scene", "missing_rhyming_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_rhyming_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 540); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "underwater_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 137){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 541); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 542); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 543); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "underwater_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 544); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "underwater_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 545); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 138){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 546); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 547); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 548); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 549); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 550); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 139){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 551); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 552); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 553); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 554); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "underwater_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 555); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "underwater_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 140){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 556); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 557); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 558); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "underwater_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 559); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 560); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "underwater_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 141){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 561); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "underwater_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 562); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "underwater_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 563); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 564); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "underwater_sentence_jumble" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 565); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 142){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 566); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 567); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "underwater_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 568); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "underwater_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 569); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 570); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 143){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 571); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 572); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 573); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "underwater_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 574); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "underwater_make_a_sentence" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 575); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "underwater_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 144){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 576); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "underwater_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 577); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 578); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "underwater_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 579); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "underwater_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 580); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "underwater_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 145){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 581); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "underwater_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 582); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "underwater_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 583); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "underwater_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 584); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "underwater_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 585); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "underwater_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 146){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 586); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "underwater_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 587); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 589); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "underwater_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 590); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "underwater_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 147){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 591); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 592); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 593); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 594); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 595); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 148){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 596); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 597); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "castle_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 598); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 599); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 600); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "castle_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 149){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 601); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 602); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "forest_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 603); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 604); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "castle_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 605); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 150){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 606); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "castle_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 607); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 608); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 609); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "castle_make_a_sentence" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 610); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 151){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 611); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 612); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 613); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 614); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 615); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 152){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 616); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 617); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 618); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 619); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 620); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 153){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 621); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 622); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "underwater_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 623); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 624); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 625); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 154){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 626); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 627); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "underwater_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 628); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 629); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "castle_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 630); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 155){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 631); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 632); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 633); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 634); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 635); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 156){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 636); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 637); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 638); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 639); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "castle_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 640); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "castle_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 157){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 641); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 642); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 643); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 644); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 645); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 158){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 646); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 647); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "castle_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 648); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 649); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 650); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "castle_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 159){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 651); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 653); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 654); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "castle_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 655); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 160){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 656); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 657); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 658); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 659); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "castle_make_a_sentence" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 660); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 161){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 661); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 662); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 663); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 664); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 665); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 162){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 666); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 667); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 668); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 669); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 670); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 163){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 671); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 672); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "forest_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 673); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 674); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 675); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "castle_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 164){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 676); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 677); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "forest_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 678); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 679); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "castle_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 680); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 165){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 681); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 683); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 684); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 685); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 166){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 686); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 687); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 688); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 689); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "castle_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 690); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "castle_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 167){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 691); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 692); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 693); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 694); 
            hash.Add("game_scene", "missing_word_in_sentence_spelling_oral_blending");
            hash.Add("game_setting", "forest_missing_word_in_sentence_spelling_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 695); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 168){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 696); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 697); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "underwater_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 698); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 699); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 700); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "castle_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 169){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 701); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 702); 
            hash.Add("game_scene", "learn_cvcc_words");
            hash.Add("game_setting", "underwater_learn_cvcc_words" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 703); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 704); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "castle_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 705); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 170){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 706); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 707); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 708); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 709); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 710); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 171){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 711); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 712); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 713); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 714); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 715); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "castle_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 172){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 716); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 717); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 718); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 719); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 720); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 173){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 721); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 723); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "castle_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 724); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 725); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "castle_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 174){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 726); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 727); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 728); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "castle_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 729); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "castle_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 730); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "castle_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 175){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 731); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 732); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 733); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 734); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "castle_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 735); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 176){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 736); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 737); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 738); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 739); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "castle_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 740); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "castle_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 177){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 741); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "castle_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 742); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 743); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 744); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "castle_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 745); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "castle_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 178){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 746); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "castle_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 747); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "castle_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 748); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "castle_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 749); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 750); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 179){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 751); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 752); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 753); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "castle_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 754); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "castle_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 755); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "castle_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 180){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 756); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "castle_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 757); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 758); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "castle_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 759); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "castle_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 760); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "castle_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 181){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 761); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "castle_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 762); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "castle_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 763); 
            hash.Add("game_scene", "missing_letter_into_word_spelling");
            hash.Add("game_setting", "castle_missing_letter_into_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 764); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "castle_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 765); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "castle_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 182){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 766); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 767); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 768); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 769); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "school_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 770); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "farm_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 183){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 771); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 772); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 773); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 774); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 775); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 184){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 776); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "farm_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 777); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 778); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 779); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 780); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "farm_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 185){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 781); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 782); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 783); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 784); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 785); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 186){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 786); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 787); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 788); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 789); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 790); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 187){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 791); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 792); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 793); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 794); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 795); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "farm_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 188){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 796); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 797); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 798); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 799); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 800); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 189){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 801); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "farm_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 802); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 803); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 804); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 805); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 190){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 806); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 807); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 808); 
            hash.Add("game_scene", "missing_word_in_sentence_spelling_oral_blending");
            hash.Add("game_setting", "forest_missing_word_in_sentence_spelling_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 809); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 810); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 191){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 811); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 812); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 813); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 814); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "farm_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 815); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 192){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 816); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 817); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 818); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 819); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 820); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "farm_riddles" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 822); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 193){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 821); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 823); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 824); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 825); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 194){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 826); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "farm_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 827); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 828); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 829); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 830); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 195){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 831); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 832); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 833); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 834); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 835); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 196){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 836); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 837); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 838); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 839); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 840); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 197){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 841); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 842); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 843); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 844); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 845); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "farm_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 198){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 846); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 847); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 848); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 849); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 850); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 199){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 851); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "farm_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 852); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 853); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 854); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 855); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "farm_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 200){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 856); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 857); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 858); 
            hash.Add("game_scene", "missing_word_in_sentence_spelling_oral_blending");
            hash.Add("game_setting", "forest_missing_word_in_sentence_spelling_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 859); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 860); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 201){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 861); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 862); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 863); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 864); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "farm_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 865); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 202){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 866); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 867); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 868); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 869); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 870); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "farm_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 203){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 871); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 872); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 873); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 874); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 875); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 204){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 876); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "farm_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 877); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 878); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 879); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 880); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 205){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 881); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 882); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 883); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 884); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 885); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 206){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 886); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 887); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 888); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 889); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 890); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 207){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 891); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 892); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 893); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 894); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 895); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "farm_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 208){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 896); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 897); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 898); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 899); 
            hash.Add("game_scene", "swap_letter_in_word_challenge_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_challenge_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 900); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 209){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 901); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "farm_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 902); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 903); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 904); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 905); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 210){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 906); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 907); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 908); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 909); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 910); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "farm_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 211){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 911); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 912); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 913); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 914); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "farm_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 915); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "farm_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 212){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 916); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 917); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "farm_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 918); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "farm_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 919); 
            hash.Add("game_scene", "silly_sensible_words_reading");
            hash.Add("game_setting", "farm_silly_sensible_words_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 920); 
            hash.Add("game_scene", "highlighting");
            hash.Add("game_setting", "farm_highlighting" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 213){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 921); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "farm_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 922); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "farm_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 923); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 924); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "farm_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 925); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 214){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 926); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "farm_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 927); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "farm_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 928); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "farm_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 929); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "farm_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 930); 
            hash.Add("game_scene", "label_images_spelling");
            hash.Add("game_setting", "farm_label_images_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1084); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 215){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 931); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 932); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 933); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "farm_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 934); 
            hash.Add("game_scene", "swap_letter_in_word_spelling");
            hash.Add("game_setting", "farm_swap_letter_in_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 935); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "farm_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 216){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1215); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "farm_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1214); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "farm_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1213); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "farm_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1212); 
            hash.Add("game_scene", "rhyming_cauldron_blend");
            hash.Add("game_setting", "farm_rhyming_cauldron_blend" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1211); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "farm_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 217){ 
            }




            else if(sessionId == 218){ 
            }




            else if(sessionId == 219){ 
            }




            else if(sessionId == 220){ 
            }




            else if(sessionId == 221){ 
            }




            else if(sessionId == 222){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 936); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "school_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 937); 
            hash.Add("game_scene", "split_digraph_introduction");
            hash.Add("game_setting", "underwater_split_digraph_introduction" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 938); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "school_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 939); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "farm_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 940); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "farm_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 223){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 941); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 942); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 943); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 944); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 945); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 224){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 946); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 947); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "school_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 948); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 949); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 950); 
            hash.Add("game_scene", "riddles");
            hash.Add("game_setting", "school_riddles" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 225){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 951); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "school_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 952); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 953); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "school_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 954); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 955); 
            hash.Add("game_scene", "missing_word_in_sentence_spelling_oral_blending");
            hash.Add("game_setting", "forest_missing_word_in_sentence_spelling_oral_blending" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 226){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 956); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "school_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 957); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 958); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 959); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 960); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "school_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 227){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 961); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 962); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "school_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 963); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "school_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 964); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 965); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "school_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 228){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 966); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "school_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 967); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 968); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 969); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 970); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 229){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 971); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 972); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 973); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 974); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "school_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 975); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "school_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 230){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 976); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "school_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 977); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "school_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 978); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 979); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 980); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 231){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 981); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "school_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 982); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 983); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 984); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 985); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "school_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 232){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 986); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 987); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "school_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 988); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "school_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 989); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 990); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "school_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 233){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 991); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "school_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 992); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 993); 
            hash.Add("game_scene", "missing_rhyming_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_rhyming_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 994); 
            hash.Add("game_scene", "rhyming_words_spelling");
            hash.Add("game_setting", "school_rhyming_words_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 995); 
            hash.Add("game_scene", "make_a_poem_spelling");
            hash.Add("game_setting", "school_make_a_poem_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 234){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 996); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 997); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 998); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 999); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "school_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1000); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "school_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 235){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1001); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "school_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1002); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "school_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1003); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1004); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1005); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1186); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1187); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1188); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1189); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1190); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 236){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1006); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "school_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1007); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1008); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1009); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1010); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "school_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1191); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1192); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1193); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1194); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1195); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 237){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1011); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "farm_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1012); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "school_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1013); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "school_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1014); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1015); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1196); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1197); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1198); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1199); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1200); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 238){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1016); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "school_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1017); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1018); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1019); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1020); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1201); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1202); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1203); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1204); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1205); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 239){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1021); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1022); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1023); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1024); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "school_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1025); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "school_sentence_jumble" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1206); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1207); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1208); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1209); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1210); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 240){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1026); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "school_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1027); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "school_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1028); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1029); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1030); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 241){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1031); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "school_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1032); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1033); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1034); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1035); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "school_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 242){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1036); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1037); 
            hash.Add("game_scene", "learn_phoneme_digraph");
            hash.Add("game_setting", "school_learn_phoneme_digraph" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1038); 
            hash.Add("game_scene", "sound_buttons_spelling");
            hash.Add("game_setting", "school_sound_buttons_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1039); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1040); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 243){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1041); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "school_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1042); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1043); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1044); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1045); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 244){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1046); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "school_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1047); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "school_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1048); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "school_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1049); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "school_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1050); 
            hash.Add("game_scene", "sentence_jumble");
            hash.Add("game_setting", "school_sentence_jumble" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 245){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1051); 
            hash.Add("game_scene", "collect_the_same_word_spelling");
            hash.Add("game_setting", "school_collect_the_same_word_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1052); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "school_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1053); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "school_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1054); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "school_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1055); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "school_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 246){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1056); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "school_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1057); 
            hash.Add("game_scene", "splat_alternative_grapheme");
            hash.Add("game_setting", "school_splat_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1058); 
            hash.Add("game_scene", "odd_one_out_alternative_grapheme");
            hash.Add("game_setting", "school_odd_one_out_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1059); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "school_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1060); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "school_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 247){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1061); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1062); 
            hash.Add("game_scene", "learn_alternative_grapheme");
            hash.Add("game_setting", "alien_learn_alternative_grapheme" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1063); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1064); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1065); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "alien_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 248){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1066); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1067); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1068); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1069); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "alien_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1070); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "alien_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 249){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1071); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1072); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "alien_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1073); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1074); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1075); 
            hash.Add("game_scene", "alphabet_page_letter_formation");
            hash.Add("game_setting", "school_alphabet_page_letter_formation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 250){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1076); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1077); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1078); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1079); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1080); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 251){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1081); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "alien_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1082); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "school_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1083); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "alien_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1085); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "alien_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 252){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1086); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1087); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1088); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1089); 
            hash.Add("game_scene", "keyword_spelling");
            hash.Add("game_setting", "alien_keyword_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1090); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "alien_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 253){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1091); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1092); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1093); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1094); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "alien_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1095); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 254){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1096); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1097); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1098); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1099); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1100); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "alien_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 255){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1101); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1102); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1103); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1104); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1105); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 256){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1106); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1107); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "alien_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1108); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1109); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1110); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "alien_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 257){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1112); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1111); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1113); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1114); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1115); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "alien_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 258){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1116); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1117); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1118); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1119); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "alien_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1120); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "alien_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 259){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1121); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1122); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1123); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1124); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1125); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 260){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1126); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1127); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1128); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1129); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1130); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 261){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1131); 
            hash.Add("game_scene", "choose_correct_path_oral_blending");
            hash.Add("game_setting", "alien_choose_correct_path_oral_blending" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1132); 
            hash.Add("game_scene", "target_phoneme_spelling");
            hash.Add("game_setting", "alien_target_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1133); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "alien_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1134); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1135); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "alien_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 262){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1136); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1137); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1138); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1139); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1140); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "alien_the_artist_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1142); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 263){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1141); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1143); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1144); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "alien_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1145); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "alien_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 264){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1146); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1147); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1148); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1149); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1150); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "alien_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 265){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1151); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1152); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1153); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1154); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1155); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 266){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1156); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1157); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1158); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1159); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1160); 
            hash.Add("game_scene", "silly_sensible_sentences_reading");
            hash.Add("game_setting", "alien_silly_sensible_sentences_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 267){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1161); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1162); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1163); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1164); 
            hash.Add("game_scene", "silly_sensible_words_oral_blending_help_pip");
            hash.Add("game_setting", "alien_silly_sensible_words_oral_blending_help_pip" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1165); 
            hash.Add("game_scene", "the_artist_reading");
            hash.Add("game_setting", "alien_the_artist_reading" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 268){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1166); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1167); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1168); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1169); 
            hash.Add("game_scene", "segmenting_spelling");
            hash.Add("game_setting", "alien_segmenting_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1170); 
            hash.Add("game_scene", "make_a_sentence");
            hash.Add("game_setting", "alien_make_a_sentence" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 269){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1171); 
            hash.Add("game_scene", "splat");
            hash.Add("game_setting", "alien_splat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1172); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1173); 
            hash.Add("game_scene", "learn_phoneme_single_letter");
            hash.Add("game_setting", "alien_learn_phoneme_single_letter" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1174); 
            hash.Add("game_scene", "fruit_machine");
            hash.Add("game_setting", "alien_fruit_machine" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1175); 
            hash.Add("game_scene", "word_bank_spelling");
            hash.Add("game_setting", "alien_word_bank_spelling" ); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 270){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1176); 
            hash.Add("game_scene", "collect_the_same_letter_or_word");
            hash.Add("game_setting", "alien_collect_the_same_letter_or_word" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1177); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1178); 
            hash.Add("game_scene", "phoneme_splat_the_rat");
            hash.Add("game_setting", "alien_phoneme_splat_the_rat" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1179); 
            hash.Add("game_scene", "keyword_search_reading");
            hash.Add("game_setting", "alien_keyword_search_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1180); 
            _sectionList.Add(hash); 

            }




            else if(sessionId == 271){ 
            hash=new Hashtable(); 
            hash.Add("section_id", 1181); 
            hash.Add("game_scene", "splat_falling");
            hash.Add("game_setting", "alien_splat_falling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1182); 
            hash.Add("game_scene", "learn_alternative_pronunciation");
            hash.Add("game_setting", "alien_learn_alternative_pronunciation" ); 
            hash.Add("game_scene", "word_sort_alternative_pronunciation_reading");
            hash.Add("game_setting", "alien_word_sort_alternative_pronunciation_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1183); 
            hash.Add("game_scene", "collect_the_same_phoneme_spelling");
            hash.Add("game_setting", "alien_collect_the_same_phoneme_spelling" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1184); 
            hash.Add("game_scene", "missing_word_into_sentence_reading");
            hash.Add("game_setting", "alien_missing_word_into_sentence_reading" ); 
            _sectionList.Add(hash); 

            hash=new Hashtable(); 
            hash.Add("section_id", 1185); 
            hash.Add("game_scene", "match_illustrations");
            hash.Add("game_setting", "alien_match_illustrations" ); 
            _sectionList.Add(hash); 

            }




//		if(sessionId==1117)
//		{
//			Hashtable hash=new Hashtable();
//			hash.Add("game_scene", "Splat-SAT");
//			hash.Add ("game_setting", "forest");
//			hash.Add("section_id", 2);
//			_sectionList.Add(hash);
//			
//			hash=new Hashtable();
//			hash.Add("game_scene", "game_key_that_is_garbage");
//			hash.Add("section_id", 2);
//			_sectionList.Add(hash);
//		
//		}

	}

}
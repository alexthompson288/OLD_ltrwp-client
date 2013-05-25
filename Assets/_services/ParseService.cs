using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using AlTypes;

public class ParseService
{
	public String[] GetUserWordIndex()
	{
		SqliteDatabase CmsDb=GameManager.Instance.CmsDb;

		DataTable dt=CmsDb.ExecuteQuery("select word from words where cvc='t' and diagraph='f'");
		String[] words=new String[dt.Rows.Count];
		for(int i=0; i<dt.Rows.Count; i++)
		{
			words[i]=(String)dt.Rows[i]["word"];
		}
		return words;
	}
	
	public String[] GetPhonemesForWord(String word)
	{
		SqliteDatabase CmsDb=GameManager.Instance.CmsDb;
		
		//get the word
		DataTable dt=CmsDb.ExecuteQuery("select id from words where word='" + word + "'");
		int wordid=(int)dt.Rows[0]["id"];
		
		//get the phonemes
		DataTable dtp=CmsDb.ExecuteQuery("select phoneme from phonemes p INNER JOIN phonemes_words pw ON p.id=pw.phoneme_id WHERE pw.word_id=" + wordid.ToString());
		String[] phonemes=new String[dtp.Rows.Count];
		for(int i=0; i<dtp.Rows.Count; i++)
		{
			//get phoneme and trim hyphen bits
			String pr=(String)dtp.Rows[i]["phoneme"];
			int ih=pr.IndexOf("-");
			if(ih>0) pr=pr.Substring(0, ih);

			phonemes[i]=pr;
		}
		return phonemes;
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
	
	public List<String> GetSortedPhonemesForWord(String word)
	{
		if(word=="acid") return new List<String>{"a", "c", "i", "d"};
		if(word=="mole") return new List<String>{"m", "o-e", "l", "o-e"};
		if(word=="super") return new List<String>{"s", "u", "p", "er"};
		if(word=="vulture") return new List<String>{"v", "u", "l", "t", "ure"};
		if(word=="air") return new List<String>{"air"};


		if(word.Length==0) return null;
		
		String[]unsortedPhonemes=GetPhonemesForWord(word);
		if(unsortedPhonemes.Length==0) return null;
		
		String wordRemainder=word;
		
		//get a size (desc) sorted list of phonemes
		List<String>sizeDescPhonemes=new List<String>();
		sizeDescPhonemes.Add(unsortedPhonemes[0]);
		if(unsortedPhonemes.Length>1)
		{
			for(int j=1;j<unsortedPhonemes.Length; j++)
			{
				String p=unsortedPhonemes[j];
				int insertAt=sizeDescPhonemes.Count;
				for(int i=0; i<sizeDescPhonemes.Count; i++)
				{
					if(p.Length>=sizeDescPhonemes[i].Length)
					{
						insertAt=i;
						break;
					}
				}
				sizeDescPhonemes.Insert(insertAt, p);
			}
		}
		
		int ip=0;
		int notFoundCount=0;
		List<String>sortedPhonemes=new List<String>();
		while (wordRemainder.Length>0 && notFoundCount<=sizeDescPhonemes.Count) {
			if(wordRemainder.IndexOf(sizeDescPhonemes[ip])==0)
			{
				//insert this in sorted index & remove from unsorted (and front of word)
				sortedPhonemes.Add(sizeDescPhonemes[ip]);
				wordRemainder=wordRemainder.Substring (sizeDescPhonemes[ip].Length);
				notFoundCount=0;
			}
			else
			{
				notFoundCount++;
			}

			ip++;

			if(ip>=sizeDescPhonemes.Count) 
			{
				ip=0;
			}
		}
	
		return sortedPhonemes;
		// return sizeDescPhonemes;
	}
}
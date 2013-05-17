using System;

namespace AlTypes
{
	public struct PhonemeData
	{
		public String Phoneme;
		public String Mneumonic;
		public String MneumonicTwo;
		public int Id;
	}

	public struct DataWordData
	{
		//from the data word -- e.g. section level
		public bool IsTargetWord;
		public bool IsDummyWord;
		public String LinkingIndex;
		
		//from the word itself
		public String Word;
		public int WordId;
		public bool Nonsense;

	}

	public struct DataSentenceData
	{
		public String Sentence;
		public String LinkingIndex;
	}
}
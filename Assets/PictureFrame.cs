using UnityEngine;
using System.Collections;
using AlTypes;

public class PictureFrame : MonoBehaviour {
	
	public OTSprite PictureSprite;
	private float StartingYValue;
	public AudioClip DownSound;
	public AudioClip UpSound;
	public OTTextSprite PhonemeLetter;
	public OTTextSprite MnemonicWord1;
	public OTTextSprite MnemonicWord2;
	public OTTextSprite MnemonicLetter1;
	public OTTextSprite MnemonicLetter2;
	public bool IsMnemonic = true;

	// Use this for initialization
	void Start () {
		StartingYValue = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void ShowMnemonic(string Phoneme, float duration = -1.0f)
	{
		if(duration > 0.0f)
			StartCoroutine(MakeDisappearDelay(duration));
		
		PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(Phoneme);
		
		PhonemeLetter.text = pd.Phoneme;	
		BreakUpMnemonicWords(pd.Mneumonic);
		
		string filePathMnemonic="Images/mnemonics_images_png_250/"+pd.Phoneme+"_"+pd.Mneumonic;
		filePathMnemonic = filePathMnemonic.Replace(" ", "_");
		Debug.Log("file path"+filePathMnemonic);
		PictureSprite.image=(Texture2D)Resources.Load(filePathMnemonic);
		
		MakeAppear();
	}
	
	public void MakeAppear()
	{
		audio.clip = DownSound;
		audio.Play();
		Vector3 newPos=new Vector3(0.0f, 165.0f, -20.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.BounceOut );

		GoTween tween=new GoTween(transform, 1.28f, config);
		
		//tween.setOnCompleteHandler(c => MachineDown());

		Go.addTween(tween);
	}
	
	public IEnumerator MakeDisappearDelay(float Duration)
	{
		yield return new WaitForSeconds(Duration);
		MakeDisappear();
	}
	
	public void MakeDisappear()
	{
		audio.clip = UpSound;
		audio.Play();
		Vector3 newPos=new Vector3(0.0f, StartingYValue, -20.0f);
		var config=new GoTweenConfig()
			.vector3Prop( "position", newPos )
			.setEaseType( GoEaseType.QuadIn );

		GoTween tween=new GoTween(transform, 0.3f, config);
		
		//tween.setOnCompleteHandler(c => MachineDown());

		Go.addTween(tween);
	}
	
	void BreakUpMnemonicWords(string MnemonicSentence)
	{
		MnemonicSentence.Replace("_", " ");
		string [] words = MnemonicSentence.Split(' ');
		
		MnemonicLetter1.text = words[0][0].ToString();
		MnemonicLetter2.text = words[1][0].ToString();
		
		words[0] = words[0].Remove(0,1);
		words[1] = words[1].Remove(0,1);
		
		MnemonicWord1.text = words[0];
		MnemonicWord2.text = words[1];
		
	}	
	
}

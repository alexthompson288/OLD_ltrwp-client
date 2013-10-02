using UnityEngine;
using System.Collections;
using AlTypes;

public class PictureFrame : MonoBehaviour {
	
	public OTSprite PictureSprite;
	private Vector3 StartingPosition;
	public AudioClip DownSound;
	public AudioClip UpSound;
	public OTTextSprite PhonemeLetter;
	public OTTextSprite MnemonicWord1;
	public OTTextSprite MnemonicWord2;
	public OTTextSprite MnemonicLetter1;
	public OTTextSprite MnemonicLetter2;
	public bool IsMnemonic = true;
	public OTSprite CentralPicture;
	public bool IsDown = false;
	public bool UseCustomOnScreenPosition = false;
	public Vector3 CustomOnScreenPosition;
	public string MyWord;
	private float ShakeTimer = 0.0f;
	private Vector3 ShakeOriginalPosition;
	
	// Use this for initialization
	void Start () {
		StartingPosition = transform.position;
		ShakeOriginalPosition = StartingPosition;
	}
	
	// Update is called once per frame
	void Update () {
		ShakeTimer -= Time.deltaTime;
		if(ShakeTimer > 0.0f)
		{
			transform.position = ShakeOriginalPosition + new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * 10.0f) * 8.0f * ShakeTimer, 0.0f, 0.0f);
		}else if(ShakeTimer > -1.0f){
			transform.position = ShakeOriginalPosition;
		}
		CentralPicture.transform.localScale = new Vector3(350,350,1);
	}
	
	public void ShowMnemonic(string Phoneme, float duration = -1.0f)
	{
		IsDown = true;
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
	
	public void PlayMnemonic(string letter)
	{
		PhonemeData pd = GameManager.Instance.GetPhonemeInfoForPhoneme(letter);
		string path = "audio/benny_mnemonics_master/benny_mnemonic_"+pd.Phoneme+"_"+pd.Grapheme+"_"+pd.Mneumonic;
		path = path.Replace(" ", "_");
		Debug.Log("looking for audio: " + path);
		audio.clip = (AudioClip)Resources.Load(path);
		audio.Play();
	}
	
	public void ShowPicture(string Word)
	{
		ShakeTimer = -10.0f;
		Debug.Log("new picture: " + Word);
		PictureSprite.gameObject.SetActive(false);
		PhonemeLetter.gameObject.SetActive(false);
		MnemonicWord1.gameObject.SetActive(false);
		MnemonicWord2.gameObject.SetActive(false);
		MnemonicLetter1.gameObject.SetActive(false);
		MnemonicLetter2.gameObject.SetActive(false);
		CentralPicture.gameObject.SetActive(true);
		MyWord = Word;
		
		if(IsDown)
		{
			
		  StartCoroutine(LoadPictureAndAppearDelayed(Word));
		}else{
			CentralPicture.image = (Texture2D)Resources.Load("Images/word_images_png_350/_"+Word+"_350");
			
			MakeAppear();
		}
		
	}
	
	IEnumerator LoadPictureAndAppearDelayed(string Word)
	{
		MakeDisappear();
		yield return new WaitForSeconds( 0.6f);
		
		CentralPicture.image = (Texture2D)Resources.Load("Images/word_images_png_350/_"+Word+"_350");
		
		MakeAppear();
	}
	
	public void Shake()
	{
		ShakeOriginalPosition = transform.position;
		ShakeTimer = 1.0f;
	}
	
	public void MakeAppear()
	{
		ShakeTimer = -10.0f;
		IsDown = true;
		audio.clip = DownSound;
		audio.Play();
		
		Vector3 newPos=new Vector3(StartingPosition.x, 165.0f, StartingPosition.z);
		
		if(UseCustomOnScreenPosition)
			newPos = CustomOnScreenPosition;
		
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
		ShakeTimer = -10.0f;
		IsDown = false;
		audio.clip = UpSound;
		audio.Play();
		Vector3 newPos=new Vector3(StartingPosition.x, StartingPosition.y, StartingPosition.z);
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
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;
	}

	public void On_SimpleTap(Gesture gesture)
	{
	}
	
}

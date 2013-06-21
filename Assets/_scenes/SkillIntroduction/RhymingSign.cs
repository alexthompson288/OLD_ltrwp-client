using UnityEngine;
using System.Collections;

public class RhymingSign : MonoBehaviour {

	public bool bat;
	public bool cat;
	public bool hat;
	public bool sat;
	public bool monsterpie;
	public bool babylullaby;
	public bool pigpinkbum;
	public bool goldilocks;
	public bool bushtailedfox;
	public bool funnyhoney;
	public bool bunny;

	OTSprite mySprite;

	// Use this for initialization
	void Awake () {
		mySprite=gameObject.GetComponent<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {

		if(bat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_bat");
		else if(cat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_cat");
		else if(hat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_cup");
		else if(sat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_sat");
		else if(monsterpie)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_screw");
		else if(babylullaby)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_sleep");
		else if(pigpinkbum)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_pig");
		else if(goldilocks)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_goblin");
		else if(bushtailedfox)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_gnome");
		else if(funnyhoney)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_jar");
		else if(bunny)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_bunny");
		else
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_blank");

	}
}

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
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_bat_350");
		else if(cat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_cat_350");
		else if(hat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_cup_350");
		else if(sat)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_sat_350");
		else if(monsterpie)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_screw_350");
		else if(babylullaby)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_sleep_350");
		else if(pigpinkbum)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_pig_350");
		else if(goldilocks)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_goblin_350");
		else if(bushtailedfox)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_gnome_350");
		else if(funnyhoney)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_jar_350");
		else if(bunny)
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_bunny_350");
		else
			mySprite.image=(Texture2D)Resources.Load("Images/word_images_png_350/_blank_350");

	}
}

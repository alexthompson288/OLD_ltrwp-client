using UnityEngine;
using System.Collections;

public class CertificateRoomManager : MonoBehaviour {
	
	public Transform CertsHolder;
	public bool PlayingAudio;
	public Transform[] ColourCerts;
	public Transform GreyCert;
	
	GameManager cmsLink;
	
	// Use this for initialization
	void Start () {
		
		int curX=0;
		int curY=0;
		float startXPos=-380.0f;
		float startYPos=140.0f;
		float incXPos=190.0f;
		float incYPos=190.0f;
		
		for(int i=0;i<12;i++)
		{
			Transform gshield=(Transform)Instantiate(GreyCert);
			gshield.parent=CertsHolder;
			OTSprite gs=gshield.GetComponent<OTSprite>();
			
			gs.position=new Vector2(startXPos+(curX*incXPos),startYPos-(curY*incYPos));
			ShieldTouch spref=gshield.GetComponent<ShieldTouch>();

			
			curX++;
			if(curX>4)
			{
				curX=0;
				curY++;
			}
		}
	}
	
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;	
	}
	
	void On_TouchDown(Gesture gesture)
	{
		CertsHolder.position=new Vector3(CertsHolder.position.x,CertsHolder.position.y+gesture.deltaPosition.y,CertsHolder.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(audio.isPlaying)
			PlayingAudio=true;
		else
			PlayingAudio=false;
		
				
		float newYPos=0;
		
		if(Input.GetKey("up"))
		{
			newYPos=CertsHolder.position.y+3.0f;
			CertsHolder.position=new Vector3(CertsHolder.position.x, newYPos, CertsHolder.position.z);
		}
		else if(Input.GetKey ("down"))
		{
			newYPos=CertsHolder.position.y-3.0f;
			CertsHolder.position=new Vector3(CertsHolder.position.x, newYPos, CertsHolder.position.z);
		}
	}
	
	public void PlayAudioClip(AudioClip thisClip)
	{
		audio.clip=thisClip;
		audio.Play();
	}
}

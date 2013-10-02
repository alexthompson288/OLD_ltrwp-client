using UnityEngine;
using System.Collections;

public class Wobble : MonoBehaviour {
	
	private float Speed = 1.0f;
	private float XOffset = 0.0f;
	private float YOffset = 0.0f;
	private Vector2 variation;
	private float Magnitude = 0.0f;
	private float effect = 0.0f;
	public bool isActive = false;
	private Vector3 StartingPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	public void SeedWobbles(float speed, float magnitude)
	{
		XOffset = Random.Range(0.0f, 1.0f);
		YOffset = Random.Range(0.0f, 1.0f);
		Speed = speed;
		Magnitude = magnitude;
		isActive = true;
		variation = new Vector2(Random.Range(0.8f, 1.2f),Random.Range(0.8f, 1.2f));
		effect = 0.0f;
		StartingPosition = transform.position;
	}
	
	public IEnumerator SeedWobblesDelayed(float speed, float magnitude, float delay)
	{
		yield return new WaitForSeconds(delay);
		XOffset = Random.Range(0.0f, 1.0f);
		YOffset = Random.Range(0.0f, 1.0f);
		Speed = speed;
		Magnitude = magnitude;
		isActive = true;
		variation = new Vector2(Random.Range(0.8f, 1.2f),Random.Range(0.8f, 1.2f));
		effect = 0.0f;
		StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive == true)
		{
			effect += Time.deltaTime * 0.5f;
			effect = Mathf.Clamp01(effect);
			
			Vector3 Pos = StartingPosition;
			Pos.x += Mathf.Sin((Time.timeSinceLevelLoad + XOffset) * Speed * variation.x) * Magnitude * effect;
			Pos.y += Mathf.Sin((Time.timeSinceLevelLoad + YOffset) * Speed * variation.y ) * Magnitude * effect;
			transform.position = Pos;
			
		}	
	}
}

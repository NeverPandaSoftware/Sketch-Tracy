using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Platforms/Platform Move")]
public class PlatformMove : MonoBehaviour
{
	public float directionSwitchTime = 2;
	private float timer;
	public float moveSpeed_X = 3;
	public float moveSpeed_Y = 0;

	// Use this for initialization
	void Start ()
	{
		timer = directionSwitchTime;
		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveSpeed_X, moveSpeed_Y);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer -= Time.deltaTime;

		if (timer < 0)
		{
			GetComponent<Rigidbody2D>().velocity *= -1;
			timer = directionSwitchTime;
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 400f;
	public static bool FacingRight;
	public Rigidbody2D rb;

	void Start()
	{
		FacingRight = CharacterController2D.FacingRight;

		if (FacingRight)
		{
			rb.velocity = new Vector2(speed, 0f);
		}
		else
		{
			rb.velocity = new Vector2(-speed, 0f);
		}

		Destroy(gameObject, 1f);
	}
}
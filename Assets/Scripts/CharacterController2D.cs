using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
	[SerializeField] private bool AirControl = false;
	[SerializeField] private LayerMask WhatIsGround;
	[SerializeField] private Transform GroundCheck;
	[SerializeField] private Transform CeilingCheck;

	const float k_GroundedRadius = .2f;
	private bool Grounded;
	const float k_CeilingRadius = .2f;
	private Rigidbody2D Rigidbody2D;
	public static bool FacingRight;
	private Vector3 Velocity = Vector3.zero;
	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		Rigidbody2D = GetComponent<Rigidbody2D>();
		FacingRight = true;

		if (OnLandEvent == null)
		{
			OnLandEvent = new UnityEvent();
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = Grounded;
		Grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, k_GroundedRadius, WhatIsGround);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				Grounded = true;

				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}
	}


	public void Move(float move, bool jump)
	{
		if (Grounded || AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
			Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, MovementSmoothing);

			if (move > 0 && !FacingRight)
			{
				Flip();
			}
			else if (move < 0 && FacingRight)
			{
				Flip();
			}
		}

		if (Grounded && jump)
		{
			Grounded = false;
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	}

	private void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

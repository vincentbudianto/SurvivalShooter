using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public Animator animator;
	float horizontalMove = 0f;
	private bool m_FacingRight = true;
	public int currHp;
	public int maxHp;
	public float moveSpeed;
	public int damage;
	public float attackDist;
	private float attackTimer;
	public float attackRate;
	private float dist;
	public int scoreToGive;
	public GameObject target;
	public AudioSource audioSource;
	public AudioClip hitSound;

	void Start ()
	{
		target = GameObject.Find("Player");
		moveSpeed = Random.Range(moveSpeed, moveSpeed + 5);
		animator.SetFloat("hp", Mathf.Abs(currHp));
		audioSource.volume = PlayerPrefs.GetFloat("Volume");
	}

	void Update ()
	{
		Move();
		horizontalMove = moveSpeed;
		attackTimer += 1.0f * Time.deltaTime;
		dist = Vector2.Distance(transform.position, target.transform.position);

		if(currHp <= 0)
		{
			Destroy(gameObject);
			Game.score += scoreToGive;
			Game.killed += 1;
		}
	}

	void Move ()
	{
		if(dist > attackDist)
		{
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
			animator.SetFloat("speed", Mathf.Abs(horizontalMove));
			animator.SetBool("isAttack", false);
		}
		else
		{
			animator.SetFloat("speed", Mathf.Abs(0));

			if(attackTimer >= attackRate)
			{
				animator.SetBool("isAttack", true);
				attackTimer = 0.0f;
				Attack();
			}
		}

		Vector3 dir = target.transform.position - transform.position;

		if (dir.x > 0 && !m_FacingRight)
		{
			Flip();
		}
		else if (dir.x < 0 && m_FacingRight)
		{
			Flip();
		}
	}

	void Attack ()
	{
		target.SendMessage("Damaged", damage);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag.Equals("Bullet"))
		{
			Destroy(col.gameObject);
			Damaged(10);
		}

		if (col.gameObject.tag.Equals("PushBullet"))
		{
			Destroy(col.gameObject);
			Damaged(5);
		}

		if (col.gameObject.tag.Equals("SpecialBullet"))
		{
			Destroy(col.gameObject);
			Damaged(100);
		}
	}

	public void Damaged (int dmg)
	{
		currHp -= dmg;
		animator.SetFloat("hp", Mathf.Abs(currHp));
		audioSource.PlayOneShot(hitSound);
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

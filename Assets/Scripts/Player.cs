using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public CharacterController2D controller;
	public Animator animator;
	float horizontalMove = 0f;
	float speed = 300f;
	bool isJump = false;
	public LayerMask mask;
	public Camera cam;
	public static int currHp;
	public static int maxHp;
	public static int shootDist;
	private float attackTimer;
	private float pushAttackTimer;
	private float specialAttackTimer;
	public Transform firePoint;
	public GameObject bullet;
	public GameObject pushBullet;
	public GameObject specialBullet;

	public AudioSource audioSource;
	public AudioClip shootSound;
	public AudioClip pushShootSound;
	public AudioClip specialShootSound;
	public AudioClip hitSound;

	void Start()
	{
		PlayerPrefs.SetInt("Score", 0);
		PlayerPrefs.SetInt("Killed", 0);
		PlayerPrefs.SetInt("Time", 0);
		shootDist = 100000;
		currHp = 100;
		maxHp = 100;
		animator.SetFloat("hp", Mathf.Abs(currHp));
	}

	void Update()
	{
		CameraFollow();
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
		animator.SetFloat("speed", Mathf.Abs(horizontalMove));
		attackTimer += 1.0f * Time.deltaTime;
		pushAttackTimer += 1.0f * Time.deltaTime;
		specialAttackTimer += 1.0f * Time.deltaTime;

		if (Input.GetKey(KeyCode.Space))
		{
			if (attackTimer >= 0.5)
			{
				attackTimer = 0.0f;
				Shoot();
			}
		}

		if (Input.GetKey(KeyCode.F))
		{
			if (pushAttackTimer >= 1)
			{
				pushAttackTimer = 0.0f;
				ShootPush();
			}
		}

		if (Input.GetKey(KeyCode.Q))
		{
			if (specialAttackTimer >= 2)
			{
				specialAttackTimer = 0.0f;
				ShootSpecial();
			}
		}

		if (Input.GetButtonDown("Jump"))
		{
			isJump = true;
			animator.SetBool("isJump", true);
		}
		else if (Input.GetButtonUp("Jump"))
		{
			animator.SetBool("isJump", false);
		}

		if (currHp <= 0)
		{
			GameOver();
		}

		if (currHp > maxHp)
		{
			currHp = maxHp;
		}
	}

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);
		isJump = false;
	}

	void Shoot()
	{
		Instantiate(bullet, firePoint.position, firePoint.rotation);
		audioSource.PlayOneShot(shootSound);
	}

	void ShootPush()
	{
		Instantiate(pushBullet, firePoint.position, firePoint.rotation);
		audioSource.PlayOneShot(pushShootSound);
	}

	void ShootSpecial()
	{
		Instantiate(specialBullet, firePoint.position, firePoint.rotation);
		audioSource.PlayOneShot(specialShootSound);
	}

	public void Damaged(int dmg)
	{
		currHp -= dmg;
		animator.SetFloat("hp", Mathf.Abs(currHp));
		audioSource.PlayOneShot(hitSound);
	}

	void CameraFollow()
	{
		cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}

	void GameOver()
	{
		PlayerPrefs.SetInt("Score", Game.score);
		PlayerPrefs.SetInt("Killed", Game.killed);
		PlayerPrefs.SetInt("Time", (int)Game.time);

		if (PlayerPrefs.GetInt("Highscore") < Game.score)
		{
			PlayerPrefs.SetInt("Highscore", Game.score);
		}

		if (PlayerPrefs.GetInt("Highest Kill") < Game.killed)
		{
			PlayerPrefs.SetInt("Highest Kill", Game.killed);
		}

		if (PlayerPrefs.GetInt("Longest Time") < (int)Game.time)
		{
			PlayerPrefs.SetInt("Longest Time", (int)Game.time);
		}

		SceneManager.LoadScene(5);
	}
}

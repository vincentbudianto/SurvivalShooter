using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : MonoBehaviour
{
	public GameObject[] spawnPoints;
	public float spawnRateMin;
	public float spawnRateMax;
	private float spawnRateTimer;
	private int prevSpawn;
	public static int killed;
	public static int score;
	public static float time;
	public GameObject[] enemies;

	void Start ()
	{
		killed = 0;
		score = 0;
		time = 0f;
		Player.currHp = Player.maxHp;
	}

	void Update ()
	{
		float spawnTime = Mathf.Lerp(spawnRateMin, spawnRateMax, Time.deltaTime);
		spawnRateTimer += 1.0f * Time.deltaTime;
		time += Time.deltaTime;

		if(spawnRateTimer >= spawnTime)
		{
			spawnRateTimer = 0.0f;
			SpawnEnemy();
		}
	}

	void SpawnEnemy ()
	{
		int ranEnemy;
		int ranSpawn;

		ranEnemy = Random.Range(0, enemies.Length);
		ranSpawn = Random.Range(0, spawnPoints.Length);

		if(ranSpawn == prevSpawn)
		{
			SpawnEnemy();
			return;
		}

		Instantiate(enemies[ranEnemy], spawnPoints[ranSpawn].transform.position, transform.rotation);
		prevSpawn = ranSpawn;
	}

	public void GoToMenu ()
	{
		SceneManager.LoadScene(0);
	}
}
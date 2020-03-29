using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public GameObject rank;
	public GameObject username;
	public GameObject score;

	public void SetScore (string rank, string username, string score)
	{
		this.rank.GetComponent<Text>().text = rank;
		this.username.GetComponent<Text>().text = username;
		this.score.GetComponent<Text>().text = score;
	}
}
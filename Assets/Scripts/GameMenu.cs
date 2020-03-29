using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class GameMenu : MonoBehaviour
{
	public string username;
	public int score;
	public int kill;
	public int time;
	public GameObject gameOverPage;
	public GameObject playPage;
	public Text finalScoreTxt;
	public Text finalKilledTxt;
	public Text finalTimeTxt;

	void Start ()
	{
		score = PlayerPrefs.GetInt("Score");
		kill = PlayerPrefs.GetInt("Killed");
		time = PlayerPrefs.GetInt("Time");
		username = PlayerPrefs.GetString("Username");
		StartCoroutine(PostScore());
	}

	void Update ()
	{
		finalScoreTxt.text = "Score : " + score;
		finalKilledTxt.text = "Killed : " + kill;
		finalTimeTxt.text = "Time : " + time;
	}

	IEnumerator PostScore ()
	{
		string url = "http://134.209.97.218:5051/scoreboards/13517137";
		string json = "{ \"username\": \"" + username + "\", \"score\": " + score + " }";
		var request = new UnityWebRequest(url, "POST");
		byte[] body = Encoding.UTF8.GetBytes(json);
		request.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.Send();

		Debug.Log("json: " + json);
		Debug.Log("Status Code: " + request.responseCode);
	}

	// IEnumerator Post ()
	// {
	// 	WWWForm form = new WWWForm();

	// 	form.AddField("username", username);
	// 	form.AddField("score", score);

	// 	using (UnityWebRequest request = UnityWebRequest.Post(url, form))
	// 	{
	// 		yield return request.Send();

	// 		if (request.isNetworkError || request.isHttpError)
	// 		{
	// 			Debug.Log(request.error);
	// 		}
	// 		else
	// 		{
	// 			Debug.Log("Post Success");

	// 			if (request.isDone)
	// 			{
	// 				Debug.Log("Post Complete");
	// 			}
	// 		}
	// 	}
	// }

	public void SetPage (string page)
	{
		if(page == "play")
		{
			gameOverPage.active = false;
			playPage.active = true;
		}
	}

	public void PlayMap (string map)
	{
		if(map == "grass")
		{
			SceneManager.LoadScene(1);
		}

		if(map == "snow")
		{
			SceneManager.LoadScene(2);
		}

		if (map == "desert")
		{
			SceneManager.LoadScene(3);
		}

		if (map == "graveyard")
		{
			SceneManager.LoadScene(4);
		}
	}

	public void BackToMenu ()
	{
		SceneManager.LoadScene(0);
	}
}

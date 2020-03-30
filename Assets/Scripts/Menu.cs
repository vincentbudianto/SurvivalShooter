using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

public class Menu : MonoBehaviour
{
	public string username;
	public int highscore;
	public int highestKill;
	public int longestTime;
	public GameObject menuPage;
	public GameObject playPage;
	public GameObject scoreboardPage;
	public GameObject settingsPage;
	public GameObject score;
	public Transform scoreList;
	public Text highscoreText;
	public Text nameText;
	public Text usernameText;
	public Slider volumeSlider;

	void Start ()
	{
		username = PlayerPrefs.GetString("HighUsername", "");
		highscore = PlayerPrefs.GetInt("Highscore", 0);
		highestKill = PlayerPrefs.GetInt("Highest Kill", 0);
		longestTime = PlayerPrefs.GetInt("Longest Time", 0);
		volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
		usernameText.text = PlayerPrefs.GetString("Username", "");
		StartCoroutine(GetScoreboard());
	}

	void Update ()
	{
		nameText.text = PlayerPrefs.GetString("Username", "");
		highscoreText.text = "Name : " + username + "\nLocal Highscore : " + highscore + "\nLocal Highest Kill : " + highestKill + "\nLocal Longest Time : " + longestTime;
	}

	IEnumerator GetScoreboard ()
	{
		string url = "http://134.209.97.218:5051/scoreboards/13517137";

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			yield return request.Send();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.Log(request.error);
			}
			else
			{
				if (request.isDone)
				{
					string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

					RootObject[] data = Data.getJsonArray<RootObject>(jsonResult);
					data.OrderBy(score => score);

					for (int i = 0; i < data.Count(); i++)
					{
						GameObject temp = Instantiate(score);
						temp.GetComponent<Score>().SetScore("#" + (i + 1).ToString(), data[i].username, data[i].score);
						temp.transform.SetParent(scoreList, false);
					}
				}
			}
		}
	}

	public void SetPage (string page)
	{
		if(page == "menu")
		{
			menuPage.active = true;
			playPage.active = false;
			scoreboardPage.active = false;
			settingsPage.active = false;
		}

		if(page == "play")
		{
			menuPage.active = false;
			playPage.active = true;
			scoreboardPage.active = false;
			settingsPage.active = false;
		}

		if(page == "scoreboard")
		{
			menuPage.active = false;
			playPage.active = false;
			scoreboardPage.active = true;
			settingsPage.active = false;
		}

		if(page == "settings")
		{
			menuPage.active = false;
			playPage.active = false;
			scoreboardPage.active = false;
			settingsPage.active = true;
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

	public void SetVolume ()
	{
		PlayerPrefs.SetFloat("Volume", volumeSlider.value);
	}

	public void SetUsername()
	{
		PlayerPrefs.SetString("Username", usernameText.text);
	}

	public void QuitGame ()
	{
		Application.Quit();
	}
}

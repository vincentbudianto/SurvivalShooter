
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
	public static T[] getJsonArray<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);

		return wrapper.array;
	}

	[System.Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}
}


[System.Serializable]
public class RootObject
{
	public string _id;
	public string nim;
	public string username;
	public string score;
}
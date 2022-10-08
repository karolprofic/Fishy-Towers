using UnityEngine;

public static class PlayerPrefsExt
{
	public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

	public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

	public static void SavePlayerPrefs(string name, bool val)
	{
		PlayerPrefsExt.SetBool(name, val);
	}

	public static void SavePlayerPrefs(string name, int val)
	{
		PlayerPrefs.SetInt(name, val);
	}

	public static void SavePlayerPrefs(string name, float val)
	{
		PlayerPrefs.SetFloat(name, val);
	}

	public static void SavePlayerPrefs(string name, string val)
	{
		PlayerPrefs.SetString(name, val);
	}
}
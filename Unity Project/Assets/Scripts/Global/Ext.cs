using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Ext
{
	public static Vector2 MousePosToCanvasPos(this CanvasScaler cs, Vector3 mousePos)
		=> new Vector2(
			mousePos.x / Screen.width, mousePos.y / Screen.height
		) * cs.referenceResolution;

	public static void OnlyOne<T>(this T obj, ref T other) where T : MonoBehaviour
	{
		if(other == null)
			other = obj;
		else if(other != obj)
		{
			GameObject.Destroy(obj.gameObject);
			Debug.Log($"Cannot implement more than one {typeof(T)}");
		}
	}
}

internal static class ExtMath
{
	//public static float Clamp(float val, float min, float max) => val < min ? min : (val > max ? max : val);
	//public static float Clamp01(float val) => val < 0 ? 0 : (val > 1 ? 1 : val);
	//public static int Clamp(int val, int min, int max) => val < min ? min : (val > max ? max : val);

	public static float Clamp(this float val, float min, float max) => val < min ? min : (val > max ? max : val);

	public static float Clamp01(this float val) => val < 0 ? 0 : (val > 1 ? 1 : val);

	public static int Clamp(this int val, int min, int max) => val < min ? min : (val > max ? max : val);

	public static float Lerp(float a, float b, float t) => (1 - t) * a + t * b;

	public static float Min(float a, float b) => a < b ? a : b;

	public static float Max(float a, float b) => a > b ? a : b;

	public static int Min(int a, int b) => a < b ? a : b;

	public static int Max(int a, int b) => a > b ? a : b;

	public static int Min(this (int, int) p) => Mathf.Min(p.Item1, p.Item2);

	public static int Max(this (int, int) p) => Mathf.Max(p.Item1, p.Item2);
}

internal static class ExtDictionary
{
	public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<KeyValuePair<TKey, TValue>> action)
	{
		foreach(var item in dict)
			action(item);
	}
}

internal static class ExtColors
{
	public static Color MixColors(Color c1, Color c2, float factor) => new Color(
		c1.r - (c1.r - c2.r) * factor, c1.g - (c1.g - c2.g) * factor, c1.b - (c1.b - c2.b) * factor, c2.a
	);
}

internal static class ExtCanvasGroup
{
	public static void Disable(this CanvasGroup cg)
	{
		cg.alpha = 0;
		cg.blocksRaycasts = false;
		cg.interactable = false;
	}

	public static void Enable(this CanvasGroup cg)
	{
		cg.alpha = 1;
		cg.blocksRaycasts = true;
		cg.interactable = true;
	}
}

internal static class ExtVectors
{
	public static Vector2Int IXZ(this Vector3 vec) => new Vector2Int((int)vec.x, (int)vec.z);

	public static Vector2Int IXY(this Vector3 vec) => new Vector2Int((int)vec.x, (int)vec.y);

	public static Vector2Int IYZ(this Vector3 vec) => new Vector2Int((int)vec.y, (int)vec.z);
}

internal static class LayerExt
{
	public static int GetLayerIdx(string name)
	{
		int layer = LayerMask.GetMask(name);
		int i = 0;
		for (; layer != 0; ++i, layer >>= 1);
		return i;
	}
}
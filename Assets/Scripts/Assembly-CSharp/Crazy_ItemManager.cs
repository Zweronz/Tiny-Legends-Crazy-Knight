using System.Collections.Generic;
using UnityEngine;

public class Crazy_ItemManager
{
	private static Dictionary<ColliderMessage, GameObject> ccmitem = new Dictionary<ColliderMessage, GameObject>();

	public static GameObject CreateItem(string path, ColliderMessage cm)
	{
		if (ccmitem.ContainsKey(cm))
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate(Resources.Load(path)) as GameObject;
		ccmitem.Add(cm, gameObject);
		return gameObject;
	}

	public static void DeleteItem(GameObject obj, ColliderMessage cm)
	{
		if (ccmitem.ContainsKey(cm))
		{
			ccmitem.Remove(cm);
			Object.Destroy(obj);
		}
	}

	public static void Clear()
	{
		ccmitem.Clear();
	}
}

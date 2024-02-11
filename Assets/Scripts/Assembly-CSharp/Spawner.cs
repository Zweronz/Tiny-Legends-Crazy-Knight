using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[Serializable]
	public class ObjectCache
	{
		public GameObject prefab;

		public int cacheSize = 10;

		private GameObject[] objects;

		private int cacheIndex;

		public void Initialize()
		{
			objects = new GameObject[cacheSize];
			for (int i = 0; i < cacheSize; i++)
			{
				objects[i] = (GameObject)UnityEngine.Object.Instantiate(prefab);
				objects[i].SetActiveRecursively(false);
				objects[i].name = objects[i].name + i;
			}
		}

		public GameObject GetNextObjectInCache()
		{
			GameObject gameObject = null;
			for (int i = 0; i < cacheSize; i++)
			{
				gameObject = objects[cacheIndex];
				if (!gameObject.active)
				{
					break;
				}
				cacheIndex = (cacheIndex + 1) % cacheSize;
			}
			if (gameObject.active)
			{
				Debug.LogWarning("Spawn of " + prefab.name + " exceeds cache size of " + cacheSize + "! Reusing already active object.", gameObject);
				Destroy(gameObject);
			}
			cacheIndex = (cacheIndex + 1) % cacheSize;
			return gameObject;
		}
	}

	public static Spawner spawner;

	public ObjectCache[] caches;

	private Hashtable activeCachedObjects;

	private void Awake()
	{
		spawner = this;
		int num = 0;
		for (int i = 0; i < caches.Length; i++)
		{
			caches[i].Initialize();
			num += caches[i].cacheSize;
		}
		activeCachedObjects = new Hashtable(num);
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		ObjectCache objectCache = null;
		if ((bool)spawner)
		{
			for (int i = 0; i < spawner.caches.Length; i++)
			{
				if (spawner.caches[i].prefab == prefab)
				{
					objectCache = spawner.caches[i];
				}
			}
		}
		if (objectCache == null)
		{
			return (GameObject)UnityEngine.Object.Instantiate(prefab, position, rotation);
		}
		GameObject nextObjectInCache = objectCache.GetNextObjectInCache();
		nextObjectInCache.transform.position = position;
		nextObjectInCache.transform.rotation = rotation;
		nextObjectInCache.SetActiveRecursively(true);
		spawner.activeCachedObjects[nextObjectInCache] = true;
		return nextObjectInCache;
	}

	public static void Destroy(GameObject objectToDestroy)
	{
		if ((bool)spawner && spawner.activeCachedObjects.ContainsKey(objectToDestroy))
		{
			objectToDestroy.SetActiveRecursively(false);
			spawner.activeCachedObjects[objectToDestroy] = false;
		}
		else
		{
			UnityEngine.Object.Destroy(objectToDestroy);
		}
	}
}

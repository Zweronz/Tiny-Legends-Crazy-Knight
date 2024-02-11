using System.Collections.Generic;
using UnityEngine;

public class Crazy_ShopControl : MonoBehaviour
{
	public List<GameObject> shoplist;

	public Vector3 original;

	public Vector3 delta;

	private void Start()
	{
		for (int i = 0; i < shoplist.Count; i++)
		{
			shoplist[i].transform.localPosition = original + i * delta;
		}
	}

	private void Update()
	{
	}
}

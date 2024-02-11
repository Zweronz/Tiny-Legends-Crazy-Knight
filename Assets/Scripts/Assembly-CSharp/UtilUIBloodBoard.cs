using System.Collections.Generic;
using UnityEngine;

public class UtilUIBloodBoard : MonoBehaviour
{
	protected GameObject blood;

	protected List<GameObject> bloodlist = new List<GameObject>();

	private void Start()
	{
		blood = base.transform.Find("BloodSample").gameObject;
		int num = 3;
		for (int i = 1; i <= 6; i++)
		{
			GameObject gameObject = Object.Instantiate(blood) as GameObject;
			gameObject.name = string.Format("Blood{0:D02}", i);
			gameObject.transform.parent = blood.transform.parent;
			gameObject.transform.localPosition = new Vector3((i - 1) % num * 21, -((i - 1) / num) * 17, blood.transform.localPosition.z);
			bloodlist.Add(gameObject);
		}
	}

	public void UpdateBlood(int maxhealth)
	{
		for (int i = 0; i < 6; i++)
		{
			if (i < maxhealth)
			{
				bloodlist[i].active = true;
			}
			else
			{
				bloodlist[i].active = false;
			}
		}
	}

	private void Update()
	{
	}
}

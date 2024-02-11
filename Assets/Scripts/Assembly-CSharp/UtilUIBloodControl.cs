using System.Collections.Generic;
using UnityEngine;

public class UtilUIBloodControl : MonoBehaviour
{
	protected GameObject item;

	protected List<GameObject> bloodlist = new List<GameObject>();

	protected List<Crazy_ParticleSequenceScript> cpss = new List<Crazy_ParticleSequenceScript>();

	public Vector2 beginVector2;

	public Vector2 deltaVector2;

	private void Start()
	{
		item = base.transform.Find("BloodSample").gameObject;
		InitHealth();
	}

	private void InitHealth()
	{
		int maxHealth = Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetMaxHealth();
		int num = 3;
		for (int i = 1; i <= maxHealth; i++)
		{
			GameObject gameObject = Object.Instantiate(item) as GameObject;
			gameObject.name = string.Format("Blood{0:D02}", i);
			gameObject.transform.parent = item.transform.parent;
			gameObject.transform.localPosition = new Vector3(beginVector2.x + (float)((i - 1) % num) * deltaVector2.x, beginVector2.y + (float)((i - 1) / num) * deltaVector2.y, item.transform.localPosition.z);
			bloodlist.Add(gameObject.transform.Find("Blood").gameObject);
			cpss.Add(gameObject.transform.Find("Effect").gameObject.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript);
		}
	}

	private void UpdateHealth()
	{
		if (bloodlist == null)
		{
			return;
		}
		int maxHealth = Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetMaxHealth();
		int curHealth = Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetCurHealth();
		for (int i = 1; i <= maxHealth; i++)
		{
			GameObject gameObject = bloodlist[i - 1];
			if (i <= curHealth)
			{
				if (!gameObject.active)
				{
					gameObject.active = true;
				}
			}
			else if (gameObject.active)
			{
				gameObject.active = false;
				cpss[i - 1].EmitParticle();
			}
		}
	}

	private void Update()
	{
		UpdateHealth();
	}
}

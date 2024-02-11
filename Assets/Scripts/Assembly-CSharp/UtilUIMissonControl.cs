using System;
using UnityEngine;

public class UtilUIMissonControl : MonoBehaviour
{
	[Serializable]
	public class MissionInfo
	{
		public GameObject panel;

		public Vector3 showPosition;

		public Vector3 hidePosition;
	}

	public MissionInfo[] tabInfo;

	protected int current;

	private void Start()
	{
		switch (Crazy_GlobalData.cur_leveltype)
		{
		case Crazy_LevelType.Normal1:
		case Crazy_LevelType.Boss:
			current = 1;
			break;
		case Crazy_LevelType.Normal2:
			current = 0;
			break;
		case Crazy_LevelType.Normal3:
			current = 2;
			break;
		case Crazy_LevelType.NetBoss:
			current = -1;
			break;
		default:
			current = -1;
			break;
		}
		if (tabInfo == null)
		{
			return;
		}
		for (int i = 0; i < tabInfo.Length; i++)
		{
			if (i == current)
			{
				tabInfo[i].panel.transform.localPosition = tabInfo[i].showPosition;
				continue;
			}
			tabInfo[i].panel.transform.localPosition = tabInfo[i].hidePosition;
			HideMisson(tabInfo[i]);
		}
	}

	private void HideMisson(MissionInfo missioninfo)
	{
		Transform[] componentsInChildren = missioninfo.panel.transform.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			transform.gameObject.active = false;
		}
	}

	private void Update()
	{
	}
}

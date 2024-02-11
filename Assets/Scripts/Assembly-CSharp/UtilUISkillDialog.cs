using System.Collections.Generic;
using UnityEngine;

public class UtilUISkillDialog : MonoBehaviour
{
	public class SkillObjectInfo
	{
		public GameObject skill;

		public Vector3 showPosition;

		public Vector3 hidePosition;

		public void Show()
		{
			skill.transform.localPosition = showPosition;
		}

		public void Hide()
		{
			skill.transform.localPosition = hidePosition;
		}
	}

	protected GameObject sample;

	protected List<SkillObjectInfo> skillinfo = new List<SkillObjectInfo>();

	private void Start()
	{
		sample = base.transform.Find("Dialog/SkillInfoSample").gameObject;
		for (int i = 1; i <= 3; i++)
		{
			GameObject gameObject = Object.Instantiate(sample) as GameObject;
			gameObject.name = string.Format("SkillInfo{0:D02}", i);
			gameObject.transform.parent = sample.transform.parent;
			gameObject.transform.localPosition = new Vector3(-115f, 50 + -48 * (i - 1), sample.transform.localPosition.z);
			SkillObjectInfo skillObjectInfo = new SkillObjectInfo();
			skillObjectInfo.skill = gameObject;
			skillObjectInfo.hidePosition = new Vector3(1000f, 1000f, 0f);
			skillObjectInfo.showPosition = gameObject.transform.localPosition;
			skillinfo.Add(skillObjectInfo);
		}
	}

	private void Update()
	{
	}

	private void updateSkillInfo(List<Crazy_WeaponSkill> skill)
	{
		for (int i = 0; i < skillinfo.Count; i++)
		{
			if (i < skill.Count)
			{
				skillinfo[i].Show();
			}
			else
			{
				skillinfo[i].Hide();
			}
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UtilUISmallSkillBoard : MonoBehaviour
{
	protected GameObject skill;

	protected List<GameObject> skilllist = new List<GameObject>();

	private void Start()
	{
	}

	public void UpdateSkill(List<Crazy_WeaponSkill> wskill)
	{
		if (wskill != null)
		{
			skilllist.Clear();
			skill = base.transform.Find("SkillSample").gameObject;
			for (int i = 1; i <= wskill.Count; i++)
			{
				GameObject gameObject = Object.Instantiate(skill) as GameObject;
				gameObject.name = string.Format("Skill{0:D02}", i);
				gameObject.transform.parent = skill.transform.parent;
				gameObject.transform.localPosition = new Vector3((i - 1) * 21, 0f, skill.transform.localPosition.z);
				TUIMeshSprite component = gameObject.GetComponent<TUIMeshSprite>();
				component.frameName = "Skill_" + ((int)wskill[i - 1].GetType()).ToString("D02") + ((wskill[i - 1].GetSkillLevel() != 1) ? ("_" + wskill[i - 1].GetSkillLevel().ToString("D02")) : string.Empty) + "_M";
				component.UpdateMesh();
				skilllist.Add(gameObject);
			}
		}
	}

	private void Update()
	{
	}
}

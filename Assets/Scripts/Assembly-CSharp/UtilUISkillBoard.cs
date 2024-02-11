using System.Collections.Generic;
using UnityEngine;

public class UtilUISkillBoard : MonoBehaviour
{
	protected GameObject skill;

	protected List<GameObject> skilllist = new List<GameObject>();

	protected List<Crazy_WeaponSkill> m_skill;

	protected Crazy_UIButtonSelectGroup group;

	public Vector3 hideposition;

	public Vector3 showposition;

	public float original;

	public float step;

	protected TUIMeshText info;

	protected new TUIMeshText name;

	private void Start()
	{
		group = base.transform.Find("SkillButton").GetComponent<Crazy_UIButtonSelectGroup>();
		skill = base.transform.Find("SkillButton/SkillSample").gameObject;
		for (int i = 1; i <= 3; i++)
		{
			GameObject gameObject = Object.Instantiate(skill) as GameObject;
			gameObject.name = string.Format("Skill{0:D02}", i);
			gameObject.transform.parent = skill.transform.parent;
			gameObject.transform.localPosition = new Vector3(original + (float)(i - 1) * step, 0f, skill.transform.localPosition.z);
			skilllist.Add(gameObject);
			group.AddControl(gameObject.GetComponent<TUIButtonSelect>());
		}
		name = base.transform.Find("SkillInfo/SkillName").GetComponent<TUIMeshText>();
		info = base.transform.Find("SkillInfo/SkillText").GetComponent<TUIMeshText>();
	}

	public void UpdateSkillInfo(int id)
	{
		name.text = Crazy_SkillInfo.GetCrazySkillInfo(m_skill[id].GetType()).name;
		name.UpdateMesh();
		info.text = Crazy_SkillInfo.GetCrazySkillInfo(m_skill[id].GetType()).explain;
		info.UpdateMesh();
	}

	public List<Crazy_WeaponSkill> GetSkill()
	{
		return m_skill;
	}

	public void UpdateSkill(List<Crazy_WeaponSkill> wskill)
	{
		m_skill = wskill;
		if (wskill == null)
		{
			Hide();
			return;
		}
		Show();
		for (int i = 0; i < 3; i++)
		{
			if (i < wskill.Count)
			{
				skilllist[i].transform.localPosition = new Vector3(original + (float)i * step, 0f, skilllist[i].transform.localPosition.z);
				TUIMeshSprite component = skilllist[i].transform.Find("Normal").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component2 = skilllist[i].transform.Find("Pressed").GetComponent<TUIMeshSprite>();
				component.frameName = "Skill_" + ((int)wskill[i].GetType()).ToString("D02") + ((wskill[i].GetSkillLevel() != 1) ? ("_" + wskill[i].GetSkillLevel().ToString("D02")) : string.Empty);
				component2.frameName = component.frameName + "_d";
				component.UpdateMesh();
				component2.UpdateMesh();
				if (i == 0)
				{
					skilllist[i].GetComponent<TUIButtonSelect>().Reset();
					skilllist[i].GetComponent<TUIButtonSelect>().SetSelected(true);
					UpdateSkillInfo(i);
				}
				else
				{
					skilllist[i].GetComponent<TUIButtonSelect>().Reset();
					skilllist[i].GetComponent<TUIButtonSelect>().SetSelected(false);
				}
			}
			else
			{
				skilllist[i].transform.localPosition = new Vector3(1000f, 0f, skilllist[i].transform.localPosition.z);
			}
		}
	}

	private void Hide()
	{
		base.gameObject.transform.localPosition = new Vector3(hideposition.x, hideposition.y, base.gameObject.transform.localPosition.z);
	}

	private void Show()
	{
		base.gameObject.transform.localPosition = new Vector3(showposition.x, showposition.y, base.gameObject.transform.localPosition.z);
	}

	private void Update()
	{
	}
}

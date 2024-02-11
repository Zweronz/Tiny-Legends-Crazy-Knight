using System;
using UnityEngine;

public class UtilUIBeginnerHintControl : MonoBehaviour
{
	public enum DialogName
	{
		Attack = 0,
		Move = 1,
		PreSkill = 2,
		UseSkill = 3,
		Bow = 4,
		Roll = 5
	}

	[Serializable]
	public class HintDialog
	{
		public GameObject dialog;

		public Vector3 showPosition;

		public Vector3 hidePosition;

		public bool isshow;
	}

	public HintDialog[] hitdialog;

	private bool useskill;

	private bool isCheckRoll;

	private bool skilldown;

	private bool bowdown;

	private bool movedown;

	private bool attackdown;

	private bool rolldown;

	public void Start()
	{
		if (Crazy_Beginner.instance.isBeginner)
		{
			switch (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
				.UseWeaponType())
			{
			case Crazy_Weapon_Type.Sword:
			case Crazy_Weapon_Type.Hammer:
			case Crazy_Weapon_Type.Axe:
			case Crazy_Weapon_Type.Staff:
				Show(DialogName.Attack);
				Show(DialogName.Move);
				break;
			}
			Invoke("CheckRoll", 1f);
			isCheckRoll = true;
		}
		CheckBow();
		inCheckRoll();
		Invoke("CheckSkill", 3f);
	}

	public void inCheckRoll()
	{
		if (!isCheckRoll && Crazy_Beginner.instance.isRoll)
		{
			Show(DialogName.Roll);
		}
	}

	private void CheckRoll()
	{
		if (Crazy_Beginner.instance.isRoll)
		{
			if (!hitdialog[0].isshow && !hitdialog[4].isshow)
			{
				Show(DialogName.Roll);
			}
			else
			{
				Invoke("CheckRoll", 1f);
			}
		}
	}

	private void CheckBow()
	{
		if (Crazy_Beginner.instance.isBow && Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.UseWeaponType() == Crazy_Weapon_Type.Bow)
		{
			Show(DialogName.Bow);
			Show(DialogName.Move);
			Invoke("CheckRoll", 1f);
			isCheckRoll = true;
		}
	}

	private void CheckSkill()
	{
		if (Crazy_Beginner.instance.isSkill && !useskill && Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetSkill())
		{
			useskill = true;
		}
	}

	public void Update()
	{
		if (useskill && !skilldown && Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetEnergy() == 100)
		{
			Show(DialogName.UseSkill);
			Invoke("UseSkillDown", 2f);
			skilldown = true;
			Crazy_Beginner.instance.isSkill = false;
		}
	}

	private void Show(DialogName id)
	{
		hitdialog[(int)id].dialog.transform.localPosition = hitdialog[(int)id].showPosition;
		hitdialog[(int)id].isshow = true;
		TUIMeshSprite[] componentsInChildren = hitdialog[(int)id].dialog.GetComponentsInChildren<TUIMeshSprite>();
		TUIMeshSprite[] array = componentsInChildren;
		foreach (TUIMeshSprite tUIMeshSprite in array)
		{
			tUIMeshSprite.Static = false;
		}
	}

	private void Hide(DialogName id)
	{
		hitdialog[(int)id].dialog.transform.localPosition = hitdialog[(int)id].hidePosition;
		hitdialog[(int)id].isshow = false;
		TUIMeshSprite[] componentsInChildren = hitdialog[(int)id].dialog.GetComponentsInChildren<TUIMeshSprite>();
		TUIMeshSprite[] array = componentsInChildren;
		foreach (TUIMeshSprite tUIMeshSprite in array)
		{
			tUIMeshSprite.Static = true;
		}
	}

	public void AnimationHide(DialogName id)
	{
		Animation[] componentsInChildren = hitdialog[(int)id].dialog.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			animation.Play("MeshFadeOut");
		}
	}

	private void HidePreSkill()
	{
		Hide(DialogName.PreSkill);
	}

	private void HideUseSkill()
	{
		Hide(DialogName.UseSkill);
	}

	private void UseSkillDown()
	{
		AnimationHide(DialogName.UseSkill);
		Invoke("HideUseSkill", 2f);
	}

	private void PreSkillDown()
	{
		AnimationHide(DialogName.PreSkill);
		Invoke("HidePreSkill", 2f);
	}

	private void HideAttack()
	{
		Hide(DialogName.Attack);
	}

	private void HideMove()
	{
		Hide(DialogName.Move);
	}

	private void HideRoll()
	{
		Hide(DialogName.Roll);
	}

	private void HideBow()
	{
		Hide(DialogName.Bow);
	}

	public void AttackDown()
	{
		if (!attackdown && hitdialog[0].isshow)
		{
			AnimationHide(DialogName.Attack);
			Invoke("HideAttack", 2f);
			attackdown = true;
			if (movedown)
			{
				Crazy_Beginner.instance.isBeginner = false;
			}
		}
	}

	public void MoveDown()
	{
		if (!movedown && hitdialog[1].isshow)
		{
			AnimationHide(DialogName.Move);
			Invoke("HideMove", 2f);
			movedown = true;
			if (attackdown)
			{
				Crazy_Beginner.instance.isBeginner = false;
			}
		}
	}

	public void RollDown()
	{
		if (!rolldown && hitdialog[5].isshow)
		{
			AnimationHide(DialogName.Roll);
			Invoke("HideRoll", 2f);
			rolldown = true;
			Crazy_Beginner.instance.isRoll = false;
		}
	}

	public void BowDown()
	{
		if (!bowdown && hitdialog[4].isshow)
		{
			AnimationHide(DialogName.Bow);
			Invoke("HideBow", 2f);
			bowdown = true;
			Crazy_Beginner.instance.isBow = false;
		}
	}
}

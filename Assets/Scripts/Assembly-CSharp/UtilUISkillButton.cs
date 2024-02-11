using UnityEngine;

public class UtilUISkillButton : MonoBehaviour
{
	protected Crazy_ParticleEmitter emphasis;

	protected TUIMeshSprite normal;

	protected TUIMeshSpriteClip normal_d;

	protected TUIRect normal_dcliprect;

	public Vector3 showposition;

	private void Start()
	{
		emphasis = base.transform.Find("Emphasis").GetComponent("Crazy_ParticleEmitter") as Crazy_ParticleEmitter;
		normal = base.transform.Find("Normal").GetComponent("TUIMeshSprite") as TUIMeshSprite;
		normal_d = base.transform.Find("Normal_d").GetComponent("TUIMeshSpriteClip") as TUIMeshSpriteClip;
		normal_dcliprect = base.transform.Find("Normal_dClipRect").GetComponent("TUIRect") as TUIRect;
		UpdateComboEnergy();
	}

	private void UpdateSkillIcon()
	{
		string text = null;
		switch (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.UseWeaponType())
		{
		case Crazy_Weapon_Type.Sword:
			text = "Skill01";
			break;
		case Crazy_Weapon_Type.Axe:
			text = "Skill02";
			break;
		case Crazy_Weapon_Type.Hammer:
			text = "Skill03";
			break;
		case Crazy_Weapon_Type.Bow:
			text = "Skill04";
			break;
		case Crazy_Weapon_Type.Staff:
			text = "Skill05";
			break;
		}
		normal.frameName = text;
		normal_d.frameName = text + "_d";
		normal.UpdateMesh();
		normal_d.UpdateMesh();
	}

	private void UpdateSkill()
	{
		if (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetSkill())
		{
			base.transform.localPosition = new Vector3(showposition.x, showposition.y, base.transform.localPosition.z);
			UpdateSkillIcon();
		}
		else
		{
			base.transform.localPosition = new Vector3(1000f, 1000f, base.transform.localPosition.z);
		}
	}

	private void UpdateComboEnergy()
	{
		if (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetSkill())
		{
			int energy = Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
				.GetEnergy();
			normal_dcliprect.rect = new Rect(normal_dcliprect.rect.x, normal_dcliprect.rect.y, normal_dcliprect.rect.width, 0.5f * (float)energy);
			normal_d.UpdateMesh();
			normal_dcliprect.UpdateRect();
			if (energy == 100)
			{
				emphasis.Emit();
			}
			else
			{
				emphasis.Stop();
			}
		}
	}

	private void Update()
	{
		UpdateSkill();
		UpdateComboEnergy();
	}
}

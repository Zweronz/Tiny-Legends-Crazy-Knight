using UnityEngine;

public class UtilUIFightButton : MonoBehaviour
{
	public Vector3 showposition;

	private void Start()
	{
	}

	private void UpdateFight()
	{
		bool flag = false;
		switch (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.UseWeaponType())
		{
		case Crazy_Weapon_Type.Sword:
		case Crazy_Weapon_Type.Hammer:
		case Crazy_Weapon_Type.Axe:
		case Crazy_Weapon_Type.Staff:
			flag = true;
			break;
		case Crazy_Weapon_Type.Bow:
			flag = false;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			base.transform.localPosition = new Vector3(showposition.x, showposition.y, base.transform.localPosition.z);
		}
		else
		{
			base.transform.localPosition = new Vector3(1000f, 1000f, base.transform.localPosition.z);
		}
	}

	private void Update()
	{
		UpdateFight();
	}
}

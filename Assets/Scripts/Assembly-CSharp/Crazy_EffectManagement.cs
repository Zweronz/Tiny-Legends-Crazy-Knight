using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crazy_PlayerControl))]
public class Crazy_EffectManagement : MonoBehaviour
{
	protected Crazy_PlayerControl control;

	protected Queue<GameObject> enchanteffects;

	protected Queue<GameObject> attackeffects;

	protected Queue<GameObject> bloodeffects;

	protected int enchantcount;

	protected int attackcount;

	protected int bloodcount;

	protected GameObject rootparent;

	private void Awake()
	{
		control = base.gameObject.GetComponent<Crazy_PlayerControl>();
	}

	protected void InitEnchantEffect(int _count)
	{
		enchanteffects = new Queue<GameObject>();
		GameObject gameObject = null;
		for (int i = 0; i < _count; i++)
		{
			switch (control.UseWeaponEnchant())
			{
			case Crazy_Weapon_Enchant.Evil:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/enchant/evil_pfb")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Enchant.Fire:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/enchant/fire_pfb")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Enchant.Ice:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/enchant/ice_pfb")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Enchant.Thunder:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/enchant/thunder_pfb")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			}
			enchanteffects.Enqueue(gameObject);
		}
	}

	protected void InitAttackEffect(int _count)
	{
		attackeffects = new Queue<GameObject>();
		GameObject gameObject = null;
		for (int i = 0; i < _count; i++)
		{
			switch (control.UseWeaponType())
			{
			case Crazy_Weapon_Type.Sword:
			{
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/attack_01/attack_01")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				Crazy_PlayAnimation crazy_PlayAnimation = gameObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
				crazy_PlayAnimation.Hide();
				break;
			}
			case Crazy_Weapon_Type.Axe:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/attack_02/attack_02")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Type.Hammer:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/attack_03/attack_03")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Type.Bow:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/attack_04/attack_04")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			case Crazy_Weapon_Type.Staff:
				gameObject = Object.Instantiate(Resources.Load("Prefabs/mageAttackEffect/mobao_hit_pfb")) as GameObject;
				gameObject.transform.parent = rootparent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				break;
			}
			attackeffects.Enqueue(gameObject);
		}
	}

	protected void InitBloodEffect(int _count)
	{
		bloodeffects = new Queue<GameObject>();
		GameObject gameObject = null;
		for (int i = 0; i < _count; i++)
		{
			gameObject = Object.Instantiate(Resources.Load("Prefabs/attack/blood/hurtblood_pfb")) as GameObject;
			gameObject.transform.parent = rootparent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			bloodeffects.Enqueue(gameObject);
		}
	}

	public void InitEffect(int _enchantcount, int _attackcount, int _bloodcount, GameObject parent)
	{
		enchantcount = _enchantcount;
		attackcount = _attackcount;
		bloodcount = _bloodcount;
		rootparent = parent;
		InitEnchantEffect(enchantcount);
		InitAttackEffect(attackcount);
		InitBloodEffect(bloodcount);
	}

	public GameObject GetEnchantEffect()
	{
		GameObject gameObject = enchanteffects.Peek();
		if (gameObject == null)
		{
			return null;
		}
		if (gameObject.GetComponent<Crazy_ParticleSequenceScript>().Isplay())
		{
			return null;
		}
		GameObject gameObject2 = enchanteffects.Dequeue();
		enchanteffects.Enqueue(gameObject2);
		return gameObject2;
	}

	public GameObject GetAttackEffect()
	{
		GameObject gameObject = attackeffects.Peek();
		if (gameObject == null)
		{
			return null;
		}
		Crazy_ParticleSequenceScript component = gameObject.GetComponent<Crazy_ParticleSequenceScript>();
		if (component == null && Crazy_Data.CurData().GetPlayerClass() != Crazy_PlayerClass.Mage && gameObject.GetComponent<Crazy_PlayAnimation>().Isplay())
		{
			return null;
		}
		if (component != null && component.Isplay())
		{
			return null;
		}
		GameObject gameObject2 = attackeffects.Dequeue();
		attackeffects.Enqueue(gameObject2);
		return gameObject2;
	}

	public GameObject GetBloodEffect()
	{
		GameObject gameObject = bloodeffects.Peek();
		if (gameObject == null)
		{
			return null;
		}
		if (gameObject.GetComponent<Crazy_ParticleSequenceScript>().Isplay())
		{
			return null;
		}
		GameObject gameObject2 = bloodeffects.Dequeue();
		bloodeffects.Enqueue(gameObject2);
		return gameObject2;
	}

	private void Update()
	{
	}
}

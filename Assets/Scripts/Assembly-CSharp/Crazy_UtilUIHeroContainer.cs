using System.Collections.Generic;
using UnityEngine;

public class Crazy_UtilUIHeroContainer : TUIContainer
{
	public GameObject effect;

	public List<int> weaponidlist;

	public List<GameObject> heros;

	private new void Start()
	{
		UpdateEffect((int)Crazy_Data.CurData().GetPlayerClass());
	}

	private void UpdateEffect(int i)
	{
		effect.transform.parent = heros[i].transform;
		effect.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
		effect.transform.localPosition = Vector3.zero;
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Button" && eventType == 1)
		{
			int num = int.Parse(control.transform.parent.name.Replace("StateBoard", string.Empty));
			UpdateEffect(num - 1);
			Crazy_Data.CurData().SetPlayerClass((Crazy_PlayerClass)(num - 1));
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}

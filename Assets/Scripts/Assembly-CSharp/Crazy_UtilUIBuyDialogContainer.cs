using System.Collections.Generic;
using UnityEngine;

public class Crazy_UtilUIBuyDialogContainer : TUIContainer
{
	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "BuyButton" && eventType == 3)
		{
			BuyButtonClick();
		}
		else if (control.name == "EquipButton" && eventType == 3)
		{
			EquipButtonClick();
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}

	protected void BuyButtonClick()
	{
		SendMessage("OnBuyButtonClick", SendMessageOptions.DontRequireReceiver);
	}

	protected void EquipButtonClick()
	{
		SendMessage("OnEquipButtonClick", SendMessageOptions.DontRequireReceiver);
	}
}

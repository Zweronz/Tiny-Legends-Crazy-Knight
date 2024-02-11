using System.Collections.Generic;
using UnityEngine;

public class LoadPropItem : MonoBehaviour
{
	private static PropItem currenrProp;

	public GameObject PropItemPrefab;

	public TUIRect rect;

	public PropPanel panel;

	private void Awake()
	{
		List<Crazy_PropItem> skillItems = Crazy_PropItem.GetSkillItems();
		Crazy_PropItem crazy_PropItem = null;
		GameObject gameObject = null;
		int count = skillItems.Count;
		for (int i = 0; i < count; i++)
		{
			crazy_PropItem = skillItems[i];
			gameObject = Spawner.Spawn(PropItemPrefab, new Vector3(1f, 1f, -100f), Quaternion.Euler(0f, 0f, 0f));
			gameObject.transform.parent = base.transform;
			gameObject.layer = base.transform.parent.gameObject.layer;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			PropItem component = gameObject.GetComponent<PropItem>();
			component.propID = skillItems[i].m_nPropID;
			component.amount = Crazy_Data.CurData().GetPropCount(component.propID);
			component.SetPropPanel(panel);
			component.SetClipRect(rect);
			component.SetIcon(skillItems[i].m_strSmallIcon, skillItems[i].priceItems[0].strPayType, skillItems[i].priceItems[0].nPrice);
		}
	}

	public static void UpdateCurrentProp(PropItem prop)
	{
		if (currenrProp != null)
		{
			currenrProp.OnLostFocus();
		}
		currenrProp = prop;
	}
}

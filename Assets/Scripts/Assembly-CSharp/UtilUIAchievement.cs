using System.Collections.Generic;
using UnityEngine;

public class UtilUIAchievement : MonoBehaviour
{
	private TUIScroll m_scroll;

	private List<Crazy_Achievement> achievement;

	private int achievement_count;

	private int show_achievement_count;

	public void Start()
	{
		GameObject gameObject = base.transform.Find("MoveObject").Find("ItemPrefab").gameObject;
		achievement = Crazy_Achievement.GetAchievementInfoList();
		achievement_count = achievement.ToArray().GetLength(0);
		show_achievement_count = achievement_count;
		int num = 0;
		int order = 0;
		while (true)
		{
			Crazy_Achievement crazy_Achievement = Crazy_Achievement.FindAchievementByOrder(achievement, order);
			if (crazy_Achievement == null)
			{
				break;
			}
			order = crazy_Achievement.seq;
			if (crazy_Achievement.hide && Crazy_Data.CurData().GetTask(crazy_Achievement.id) != 1f && ((crazy_Achievement.condition != -1 && Crazy_Data.CurData().GetTask(crazy_Achievement.condition) != 1f) || crazy_Achievement.condition == -1))
			{
				show_achievement_count--;
			}
			else
			{
				Crazy_Award awardInfo = Crazy_Award.GetAwardInfo(crazy_Achievement.award);
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				gameObject2.name = "Achievement" + crazy_Achievement.id;
				gameObject2.transform.parent = gameObject.transform.parent;
				gameObject2.transform.localPosition = new Vector3(0f, (float)num * -60f, 0f);
				TUIMeshSprite component = gameObject2.transform.Find("Bk").GetComponent<TUIMeshSprite>();
				component.frameName = ((Crazy_Data.CurData().GetTask(crazy_Achievement.id) != 1f) ? "Achievement_d" : "Achievement");
				component.UpdateMesh();
				TUIMeshSprite component2 = gameObject2.transform.Find("Mask").GetComponent<TUIMeshSprite>();
				component2.frameName = ((Crazy_Data.CurData().GetTask(crazy_Achievement.id) != 1f) ? "AchievementMask" : string.Empty);
				component2.UpdateMesh();
				TUIMeshText component3 = gameObject2.transform.Find("Title").GetComponent<TUIMeshText>();
				component3.text = crazy_Achievement.name.ToUpper();
				component3.UpdateMesh();
				TUIMeshSprite component4 = gameObject2.transform.Find("Icon").GetComponent<TUIMeshSprite>();
				string frameName = null;
				if (awardInfo != null && awardInfo.item.Count != 0 && awardInfo.item[0].type == Crazy_Award_Item_Type.Currency)
				{
					switch (awardInfo.item[0].id)
					{
					case 0:
						frameName = "GoldBig";
						break;
					case 1:
						frameName = "CrystalBig";
						break;
					}
				}
				component4.frameName = frameName;
				component4.UpdateMesh();
				TUIMeshText component5 = gameObject2.transform.Find("Text").GetComponent<TUIMeshText>();
				component5.text = crazy_Achievement.des;
				component5.UpdateMesh();
				TUIMeshText component6 = gameObject2.transform.Find("Count").GetComponent<TUIMeshText>();
				component6.text = awardInfo.item[0].count.ToString();
				component6.UpdateMesh();
				num++;
			}
			order++;
		}
		GameObject gameObject3 = base.transform.Find("MoveObject").gameObject;
		gameObject3.SendMessage("SetYMax", Mathf.Max(0f, (float)num * 60f - 220f));
		gameObject3.SendMessage("SetYMin", (object)0f);
	}
}

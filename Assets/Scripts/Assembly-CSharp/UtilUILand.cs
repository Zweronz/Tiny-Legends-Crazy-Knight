using System.Collections.Generic;
using UnityEngine;

public class UtilUILand : MonoBehaviour
{
	protected int maxstage = 1;

	protected int minstage = 1;

	public Crazy_UIButtonSelectGroup container;

	public void Start()
	{
		maxstage = Crazy_Data.CurData().GetMaxStage();
		minstage = Crazy_Data.CurData().GetMinStage();
		GameObject gameObject = base.transform.Find("StageSample").gameObject;
		int bossLevel = Crazy_Data.CurData().GetBossLevel();
		int level = Crazy_Data.CurData().GetLevel();
		int level2 = 0;
		bool flag = false;
		if (Crazy_LevelModify.FindLevel(Crazy_LevelType.Boss, bossLevel, level, ref level2))
		{
			Crazy_Data.CurData().SetBossLevel(level2);
			flag = true;
		}
		else if (Crazy_Data.CurData().GetBossLevel() > 30)
		{
			float num = Random.Range(0, 100);
			if (num < 25f && Crazy_LevelModify.FindLevel(Crazy_LevelType.Boss, Random.Range(0, 30), level, ref level2))
			{
				flag = true;
			}
		}
		Crazy_Process activeProcess = Crazy_Global.GetActiveProcess();
		if (activeProcess != null && activeProcess.stage)
		{
			int count = activeProcess.stages.Count;
			Dictionary<int, Crazy_Land> land = Crazy_Land.GetLand();
			for (int i = 0; i < activeProcess.stages.Count; i++)
			{
				int landid = activeProcess.stages[i].landid;
				Crazy_Land landinfo = Crazy_Land.GetLandinfo(landid);
				Vector2 vector = landinfo.point[Random.Range(0, landinfo.point.Count)];
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				Crazy_MapStage crazy_MapStage = gameObject2.GetComponent("Crazy_MapStage") as Crazy_MapStage;
				crazy_MapStage.leveltype = activeProcess.stages[i].leveltype;
				crazy_MapStage.landid = landid;
				if (activeProcess.stages[i].level != -1)
				{
					crazy_MapStage.deltalevel = activeProcess.stages[i].level - Crazy_Data.CurData().GetLevel();
				}
				else
				{
					crazy_MapStage.deltalevel = Random.RandomRange(-2, 3);
				}
				crazy_MapStage.waveid = activeProcess.stages[i].waveid;
				crazy_MapStage.icons = new List<string>();
				if (Crazy_Data.CurData().GetMixMonster())
				{
					foreach (int item in landinfo.icon)
					{
						crazy_MapStage.icons.Add("normal" + item.ToString("D02"));
					}
				}
				else
				{
					crazy_MapStage.icons.Add("normal" + landinfo.icon[0].ToString("D02"));
				}
				if (Crazy_Data.CurData().GetRanged())
				{
					foreach (int item2 in landinfo.rangedicon)
					{
						crazy_MapStage.icons.Add("normal" + item2.ToString("D02"));
					}
				}
				TUIMeshSprite component = gameObject2.transform.Find("Stage/StageButton").Find("Normal").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component2 = gameObject2.transform.Find("Stage/StageButton").Find("Pressed").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component3 = gameObject2.transform.Find("Stage/StageButton").Find("Disabled").GetComponent<TUIMeshSprite>();
				component.frameName = "normal" + landinfo.icon[0].ToString("D02");
				component2.frameName = "normal" + landinfo.icon[0].ToString("D02") + "_d";
				component3.frameName = "normal" + landinfo.icon[0].ToString("D02") + "_f";
				component.UpdateMesh();
				component2.UpdateMesh();
				component3.UpdateMesh();
				TUIMeshText component4 = gameObject2.transform.Find("Stage/StageText/Text").GetComponent<TUIMeshText>();
				component4.text = "LV:" + Mathf.Max(Crazy_Data.CurData().GetLevel() + crazy_MapStage.deltalevel, 1);
				gameObject2.name = string.Format("Stage{0:D02}", landid);
				gameObject2.transform.parent = gameObject.transform.parent;
				gameObject2.transform.localPosition = new Vector3(vector.x, vector.y, 0f);
				container.AddControl(gameObject2.transform.Find("Stage/StageButton").GetComponent<TUIButtonSelect>());
				if (Crazy_Beginner.instance.isMap)
				{
					GameObject gameObject3 = GameObject.Find("TUI/TUIControl/MapHint");
					gameObject3.transform.localPosition = new Vector3(vector.x - 80f, vector.y + 50f, gameObject3.transform.localPosition.z);
				}
			}
			return;
		}
		int num2 = Random.Range(minstage, maxstage + 1);
		Dictionary<int, Crazy_Land> land2 = Crazy_Land.GetLand();
		if (num2 > land2.Count)
		{
			return;
		}
		List<int> list = Crazy_Global.RandomRange(0, land2.Count, num2);
		int num3 = -1;
		if (flag)
		{
			num3 = Random.Range(0, list.Count);
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num4 = list[j];
			Crazy_Land landinfo2 = Crazy_Land.GetLandinfo(num4);
			Vector2 vector2 = landinfo2.point[Random.Range(0, landinfo2.point.Count)];
			GameObject gameObject4 = Object.Instantiate(gameObject) as GameObject;
			Crazy_MapStage crazy_MapStage2 = gameObject4.GetComponent("Crazy_MapStage") as Crazy_MapStage;
			if (j == num3)
			{
				crazy_MapStage2.leveltype = Crazy_LevelType.Boss;
				crazy_MapStage2.deltalevel = level2 - Crazy_Data.CurData().GetLevel();
				crazy_MapStage2.waveid = Crazy_LevelModify.GetModify(crazy_MapStage2.leveltype, Mathf.Max(Crazy_Data.CurData().GetLevel() + crazy_MapStage2.deltalevel, 1)).waveid;
				crazy_MapStage2.icons.Add("boss01");
				if (Crazy_Data.CurData().GetMixMonster())
				{
					foreach (int item3 in landinfo2.icon)
					{
						crazy_MapStage2.icons.Add("normal" + item3.ToString("D02"));
					}
				}
				else
				{
					crazy_MapStage2.icons.Add("normal" + landinfo2.icon[0].ToString("D02"));
				}
				if (Crazy_Data.CurData().GetRanged())
				{
					foreach (int item4 in landinfo2.rangedicon)
					{
						crazy_MapStage2.icons.Add("normal" + item4.ToString("D02"));
					}
				}
				crazy_MapStage2.landid = num4;
				TUIMeshSprite component5 = gameObject4.transform.Find("Stage/StageButton").Find("Normal").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component6 = gameObject4.transform.Find("Stage/StageButton").Find("Pressed").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component7 = gameObject4.transform.Find("Stage/StageButton").Find("Disabled").GetComponent<TUIMeshSprite>();
				component5.frameName = "boss01";
				component6.frameName = "boss01_d";
				component7.frameName = "boss01_f";
				component5.UpdateMesh();
				component6.UpdateMesh();
				component7.UpdateMesh();
			}
			else
			{
				crazy_MapStage2.leveltype = (Crazy_LevelType)Random.Range(0, 3);
				crazy_MapStage2.deltalevel = Random.Range(-2, 3);
				crazy_MapStage2.waveid = Crazy_LevelModify.GetModify(crazy_MapStage2.leveltype, Mathf.Max(Crazy_Data.CurData().GetLevel() + crazy_MapStage2.deltalevel, 1)).waveid;
				if (Crazy_Data.CurData().GetMixMonster())
				{
					foreach (int item5 in landinfo2.icon)
					{
						crazy_MapStage2.icons.Add("normal" + item5.ToString("D02"));
					}
				}
				else
				{
					crazy_MapStage2.icons.Add("normal" + landinfo2.icon[0].ToString("D02"));
				}
				if (Crazy_Data.CurData().GetRanged())
				{
					foreach (int item6 in landinfo2.rangedicon)
					{
						crazy_MapStage2.icons.Add("normal" + item6.ToString("D02"));
					}
				}
				crazy_MapStage2.landid = num4;
				TUIMeshSprite component8 = gameObject4.transform.Find("Stage/StageButton").Find("Normal").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component9 = gameObject4.transform.Find("Stage/StageButton").Find("Pressed").GetComponent<TUIMeshSprite>();
				TUIMeshSprite component10 = gameObject4.transform.Find("Stage/StageButton").Find("Disabled").GetComponent<TUIMeshSprite>();
				component8.frameName = "normal" + landinfo2.icon[0].ToString("D02");
				component9.frameName = "normal" + landinfo2.icon[0].ToString("D02") + "_d";
				component10.frameName = "normal" + landinfo2.icon[0].ToString("D02") + "_f";
				component8.UpdateMesh();
				component9.UpdateMesh();
				component10.UpdateMesh();
			}
			TUIMeshText component11 = gameObject4.transform.Find("Stage/StageText/Text").GetComponent<TUIMeshText>();
			component11.text = "LV:" + Mathf.Max(Crazy_Data.CurData().GetLevel() + crazy_MapStage2.deltalevel, 1);
			gameObject4.name = string.Format("Stage{0:D02}", num4);
			gameObject4.transform.parent = gameObject.transform.parent;
			gameObject4.transform.localPosition = new Vector3(vector2.x, vector2.y, 0f);
			container.AddControl(gameObject4.transform.Find("Stage/StageButton").GetComponent<TUIButtonSelect>());
		}
	}

	public void Update()
	{
	}

	public void ResetAllStage()
	{
		container.ResetControl();
	}

	public void SetFontName(TUIButtonSelect control)
	{
		TUIMeshText component = control.transform.parent.Find("StageText/Text").GetComponent<TUIMeshText>();
		component.fontName = "MAPFONTD";
		component.UpdateMesh();
	}
}

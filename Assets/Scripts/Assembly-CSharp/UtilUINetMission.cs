using System.Collections.Generic;
using UnityEngine;

public class UtilUINetMission : MonoBehaviour
{
	private TUIScroll m_scroll;

	private List<Crazy_NetMission> netmission;

	private int netmission_count;

	private int show_netmission_count;

	private float interval = 91f;

	private List<GameObject> showlist = new List<GameObject>();

	public void Start()
	{
		GameObject gameObject = base.transform.Find("MoveObject").Find("ItemPrefab").gameObject;
		Crazy_NetUISequence netUISequenceInfo = Crazy_NetUISequence.GetNetUISequenceInfo(0);
		for (int i = 0; i < netUISequenceInfo.mass.Count; i++)
		{
			Crazy_NetUIMass netUIMassInfo = Crazy_NetUIMass.GetNetUIMassInfo(netUISequenceInfo.mass[i]);
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.name = "Mass" + i.ToString("D02");
			gameObject2.transform.parent = gameObject.transform.parent;
			gameObject2.transform.localPosition = new Vector3(0f, (float)i * (0f - interval) - 45f, 0f);
			TUIMeshSprite component = gameObject2.transform.Find("Puzzle/M_Bk").GetComponent<TUIMeshSprite>();
			component.frameName = netUIMassInfo.bk;
			component.UpdateMesh();
			TUIMeshSprite component2 = gameObject2.transform.Find("Puzzle/L_Bk").GetComponent<TUIMeshSprite>();
			component2.frameName = netUIMassInfo.left;
			component2.UpdateMesh();
			TUIMeshSprite component3 = gameObject2.transform.Find("Puzzle/R_Bk").GetComponent<TUIMeshSprite>();
			component3.frameName = netUIMassInfo.right;
			component3.UpdateMesh();
			TUIButtonClick component4 = gameObject2.transform.Find("Button").GetComponent<TUIButtonClick>();
			component4.SetDisabled(true);
			showlist.Add(gameObject2);
		}
		netmission = Crazy_NetMission.GetNetMissionInfoList();
		netmission_count = netmission.ToArray().GetLength(0);
		show_netmission_count = netmission_count;
		int num = 0;
		int order = 0;
		while (true)
		{
			Crazy_NetMission crazy_NetMission = Crazy_NetMission.FindNetMissionByOrder(netmission, order);
			if (crazy_NetMission == null)
			{
				break;
			}
			order = crazy_NetMission.id;
			GameObject gameObject3 = showlist[num];
			gameObject3.name = "NetMission" + crazy_NetMission.id;
			bool flag = true;
			if (Crazy_Data.CurData().GetLevel() >= crazy_NetMission.lv)
			{
				flag = false;
			}
			TUIMeshSprite component5 = gameObject3.transform.Find("SceneBk").GetComponent<TUIMeshSprite>();
			component5.frameName = ((!flag) ? crazy_NetMission.scenetexture : (crazy_NetMission.scenetexture + "_d"));
			component5.UpdateMesh();
			TUIMeshSprite component6 = gameObject3.transform.Find("BossBk").GetComponent<TUIMeshSprite>();
			component6.frameName = ((!flag) ? crazy_NetMission.bosstexture : (crazy_NetMission.bosstexture + "_d"));
			component6.UpdateMesh();
			TUIMeshText component7 = gameObject3.transform.Find("Title").GetComponent<TUIMeshText>();
			component7.text = crazy_NetMission.name.ToUpper();
			component7.color = ((!flag) ? new Color(component7.color.r, component7.color.g, component7.color.b, 1f) : new Color(component7.color.r, component7.color.g, component7.color.b, 0.5f));
			component7.UpdateMesh();
			TUIMeshText component8 = gameObject3.transform.Find("Text").GetComponent<TUIMeshText>();
			component8.text = crazy_NetMission.des.Replace("\\n", "\n");
			component8.color = ((!flag) ? new Color(component8.color.r, component8.color.g, component8.color.b, 1f) : new Color(component8.color.r, component8.color.g, component8.color.b, 0.5f));
			component8.UpdateMesh();
			TUIMeshText component9 = gameObject3.transform.Find("Lv").GetComponent<TUIMeshText>();
			component9.text = "L" + crazy_NetMission.lv;
			component9.color = ((!flag) ? new Color(component9.color.r, component9.color.g, component9.color.b, 1f) : new Color(component9.color.r, component9.color.g, component9.color.b, 0.5f));
			component9.UpdateMesh();
			TUIButtonClick component10 = gameObject3.transform.Find("Button").GetComponent<TUIButtonClick>();
			component10.SetDisabled(flag);
			num++;
			order++;
		}
		GameObject gameObject4 = base.transform.Find("MoveObject").gameObject;
		gameObject4.SendMessage("SetYMax", Mathf.Max(0f, (float)netUISequenceInfo.mass.Count * interval - 265f));
		gameObject4.SendMessage("SetYMin", (object)0f);
	}
}

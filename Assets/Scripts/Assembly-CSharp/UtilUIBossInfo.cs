using UnityEngine;

public class UtilUIBossInfo : MonoBehaviour
{
	private TUIMeshSprite scenebk;

	private TUIMeshSprite sceneft;

	private TUIMeshSprite bossicon;

	private TUIMeshText lvinfotext;

	private void Start()
	{
		scenebk = base.transform.Find("SceneBk").GetComponent<TUIMeshSprite>();
		sceneft = base.transform.Find("SceneFt").GetComponent<TUIMeshSprite>();
		bossicon = base.transform.Find("BossIcon").GetComponent<TUIMeshSprite>();
		lvinfotext = base.transform.Find("LvInfo/Text").GetComponent<TUIMeshText>();
		InitBossInfo();
	}

	private void InitBossInfo()
	{
		Crazy_NetMission netMissionInfo = Crazy_NetMission.GetNetMissionInfo(Crazy_GlobalData_Net.Instance.netmissionID);
		scenebk.frameName = netMissionInfo.scenetexture;
		sceneft.frameName = netMissionInfo.scenetexture + "_d";
		bossicon.frameName = netMissionInfo.bosstexture;
		lvinfotext.text = "PARTY SIZE: " + netMissionInfo.maxcount + "   LV: " + netMissionInfo.lv;
		scenebk.UpdateMesh();
		sceneft.UpdateMesh();
		bossicon.UpdateMesh();
		lvinfotext.UpdateMesh();
	}

	private void Update()
	{
	}
}

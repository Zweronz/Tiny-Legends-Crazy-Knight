using System.Collections;
using UnityEngine;

public class NetBattleEventHandle : MonoBehaviour
{
	public TUIFade m_fadeScene;

	public GameObject m_quitBtn;

	public GameObject m_connectInfo;

	private TUIMeshText m_hintText;

	public GameObject m_versionUpdate;

	public GameObject m_connectFail;

	private bool m_bLeavingNetBattle;

	private bool m_bStart;

	public GameObject versionUpBtn;

	public GameObject exitVersionUpBtn;

	private void Awake()
	{
		m_hintText = m_connectInfo.GetComponentInChildren<TUIMeshText>();
		MyGUIEventListener.Get(m_quitBtn).EventHandleOnClicked += QuitNetBattle;
		MyGUIEventListener.Get(exitVersionUpBtn).EventHandleOnClicked += QuitNetBattle;
		MyGUIEventListener.Get(versionUpBtn).EventHandleOnClicked += OpenVersionUp;
	}

	public void Start()
	{
		m_hintText.text = "Connecting...";
		m_hintText.UpdateMesh();
	}

	private void QuitNetBattle(GameObject go)
	{
		if (!m_bStart)
		{
			base.gameObject.SendMessage("sendLeaveRoom");
			UpdateConnectInfo("Disconnect...");
			m_bLeavingNetBattle = true;
			m_fadeScene.FadeOut("CrazyMap");
		}
	}

	private void OpenVersionUp(GameObject go)
	{
		Crazy_Global.OpenReviewURL();
		QuitNetBattle(go);
	}

	public void ShowVersionUpdate()
	{
		m_versionUpdate.gameObject.SetActiveRecursively(true);
	}

	public void UpdatePlayerCount(string text)
	{
		if (!m_bLeavingNetBattle)
		{
			m_hintText.text = "Finding players...(" + text + ")";
			m_hintText.UpdateMesh();
		}
	}

	public void UpdateConnectInfo(string text)
	{
		if (!m_bLeavingNetBattle)
		{
			m_hintText.text = text;
			m_hintText.UpdateMesh();
		}
	}

	public void StartGame()
	{
		m_bStart = true;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("IsBeginGame", "YES");
		FlurryPlugin.endTimedEvent("Find_Game", hashtable);
		Crazy_GlobalData.next_scene = "CrazyScene" + Crazy_GlobalData_Net.Instance.sceneID.ToString("D03");
		m_fadeScene.FadeOut("CrazyUILoading");
	}
}

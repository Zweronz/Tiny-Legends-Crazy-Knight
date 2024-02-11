using System.Collections;
using UnityEngine;

public class UICrazyUI : MonoBehaviour, TUIHandler
{
	private TUI m_tui_down;

	private TUI m_tui_up;

	public GameObject cannotdialog;

	public void Start()
	{
		m_tui_down = TUI.Instance("TUI/Down/TUI");
		m_tui_down.SetHandler(this);
		m_tui_up = TUI.Instance("TUI/Up/TUI");
		m_tui_up.SetHandler(this);
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		Crazy_Global.PlayBGM("BGM_Menu01");
		Crazy_GlobalData.cur_UI3Times++;
		Crazy_GlobalData.cur_ui = 3;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		hashtable.Add("Gold", Crazy_Data.CurData().GetMoney());
		hashtable.Add("Crystal", Crazy_Data.CurData().GetCrystal());
		FlurryPlugin.logEvent("Enter_Shop", hashtable);
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			if (!m_tui_up.HandleInput(input[i]))
			{
				m_tui_down.HandleInput(input[i]);
			}
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "BackButton" && eventType == 3)
		{
			OnMap();
		}
		else if ((control.name == "CrystalButton" || control.name == "SPurchaseButton") && eventType == 3)
		{
			OnIAP();
		}
		else if (control.name == "OkButton" && eventType == 3)
		{
			HideDialog();
		}
	}

	protected void ShowDialog(string str)
	{
		cannotdialog.transform.localPosition = new Vector3(0f, 0f, cannotdialog.transform.localPosition.z);
		TUIMeshText component = cannotdialog.transform.Find("Text").GetComponent<TUIMeshText>();
		component.text = str;
		component.UpdateMesh();
	}

	protected void HideDialog()
	{
		cannotdialog.transform.localPosition = new Vector3(1000f, 1000f, cannotdialog.transform.localPosition.z);
	}

	public void OnIAP()
	{
		Crazy_GlobalData.pre_scene = "CrazyUI";
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyIAP");
		Resources.UnloadUnusedAssets();
	}

	public void OnMap()
	{
		Crazy_GlobalData.next_scene = "CrazyMap";
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyUILoading");
		Resources.UnloadUnusedAssets();
	}
}

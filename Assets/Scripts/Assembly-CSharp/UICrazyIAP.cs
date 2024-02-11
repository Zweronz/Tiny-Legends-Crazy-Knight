using System.Collections;
using UnityEngine;

public class UICrazyIAP : MonoBehaviour, TUIHandler
{
	private TUI m_tui_down;

	private TUI m_tui_up;

	private GameObject mask;

	private GameObject dialog;

	private bool isiap;

	private string iapid;

	public UtilUIIAP iap;

	private MonoBehaviour eventlistener;

	public void Start()
	{
		m_tui_down = TUI.Instance("TUI/Down/TUI");
		m_tui_down.SetHandler(this);
		m_tui_up = TUI.Instance("TUI/Up/TUI");
		m_tui_up.SetHandler(this);
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		mask = m_tui_up.transform.Find("TUIControl/Mask").gameObject;
		dialog = m_tui_up.transform.Find("TUIControl/WarningDialog").gameObject;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		hashtable.Add("Gold", Crazy_Data.CurData().GetMoney());
		hashtable.Add("Crystal", Crazy_Data.CurData().GetCrystal());
		FlurryPlugin.logEvent("View_IAP", hashtable);
		InitIAP();
		if (Crazy_Const.AMAZON_IAP_CONST)
		{
			eventlistener = base.gameObject.AddComponent<AmazonIAPEventListener>();
		}
		else
		{
			eventlistener = base.gameObject.AddComponent<IABAndroidEventListener>();
		}
		eventlistener.SendMessage("InitIAPEventListener", base.gameObject, SendMessageOptions.DontRequireReceiver);
	}

	private void InitIAP()
	{
		if (Crazy_Const.AMAZON_IAP_CONST)
		{
			GameObject gameObject = GameObject.Find("AmazonIAPManager");
			if (gameObject == null)
			{
				gameObject = new GameObject("AmazonIAPManager");
				gameObject.AddComponent<AmazonIAPManager>();
				AmazonIAP.initiateItemDataRequest(new string[8] { "com.trinitigame.tinysagainvasion.299centsv22", "com.trinitigame.tinysagainvasion.499centsv22", "com.trinitigame.tinysagainvasion.999centsv22", "com.trinitigame.tinysagainvasion.1999centsv22", "com.trinitigame.tinysagainvasion.4999centsv22", "com.trinitigame.tinysagainvasion.499cents2", "com.trinitigame.tinysagainvasion.1999cents2", "com.trinitigame.tinysagainvasion.4999cents2" });
			}
		}
		else
		{
			GameObject gameObject2 = GameObject.Find("IABAndroidManager");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("IABAndroidManager");
				gameObject2.AddComponent<IABAndroidManager>();
				IABAndroid.init(Crazy_Const.IAB_KEY);
			}
		}
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
			OnBack();
		}
		if (control.name == "Button" && eventType == 3)
		{
			if ((control.transform.parent.name == "com.trinitigame.tinysagainvasion.299centsv22" && Crazy_Data.CurData().IsGetNewbiePack()) || control.transform.parent.name == "Tapjoy")
			{
				return;
			}
			BuyIAP(control.transform.parent.name);
			control.GetComponent<TUIButtonClick>().Reset();
			OnMask();
		}
		if (control.name == "WarningYesButton" && eventType == 3)
		{
			HideDialog();
		}
	}

	public void ShowDialog(string text)
	{
		TUIMeshText component = dialog.transform.Find("Text").GetComponent<TUIMeshText>();
		component.text = text;
		component.UpdateMesh();
		dialog.transform.localPosition = new Vector3(0f, 0f, dialog.transform.localPosition.z);
	}

	public void HideDialog()
	{
		dialog.transform.localPosition = new Vector3(1000f, 1000f, dialog.transform.localPosition.z);
	}

	private void IAPFailed(string reason)
	{
		OffMask();
		if (reason != string.Empty)
		{
			ShowDialog(reason);
		}
	}

	private void IAPSuccess(string id)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		hashtable.Add("IAPID", id);
		FlurryPlugin.logEvent("Buy_IAP", hashtable);
		if ("com.trinitigame.tinysagainvasion.299centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_2.99_NewbiePack", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.CurData().SetGetNewbiePack(true);
			iap.go_newbiePack.SetActiveRecursively(true);
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.499centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_4.99", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.999centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_9.99", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 140);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 140;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.1999centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_19.99", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 320);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 320;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.4999centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_49.99", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 1000);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 1000;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.499cents2" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_4.99_Gold", hashtable);
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + 60000);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task23, 0, 60000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task24, 0, 60000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task25, 0, 60000f);
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.1999cents2" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_19.99_Gold", hashtable);
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + 350000);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task23, 0, 350000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task24, 0, 350000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task25, 0, 350000f);
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.4999cents2" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_49.99_Gold", hashtable);
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + 1200000);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task23, 0, 1200000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task24, 0, 1200000f);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task25, 0, 1200000f);
			Crazy_Data.SaveData();
		}
		OffMask();
	}

	private void BuyIAP(string id)
	{
		iapid = id;
		if (Crazy_Const.AMAZON_IAP_CONST)
		{
			AmazonIAP.initiatePurchaseRequest(id);
		}
		else
		{
			IABAndroid.purchaseProduct(id);
		}
		isiap = true;
	}

	private void OffMask()
	{
		mask.transform.localPosition = new Vector3(1000f, 1000f, mask.transform.localPosition.z);
		isiap = false;
	}

	private void OnMask()
	{
		mask.transform.localPosition = new Vector3(0f, 0f, mask.transform.localPosition.z);
	}

	private void OnBack()
	{
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut(Crazy_GlobalData.pre_scene);
	}

	public void OnDestroy()
	{
	}
}

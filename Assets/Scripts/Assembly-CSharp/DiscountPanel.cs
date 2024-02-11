using System;
using System.Collections;
using UnityEngine;

public class DiscountPanel : MonoBehaviour
{
	public GameObject go_title;

	public GameObject go_time;

	public GameObject go_intro;

	public GameObject go_percent;

	public GameObject go_discountPrice;

	public GameObject go_originalPrice;

	public GameObject go_crystalCount;

	public GameObject go_crystalIcon;

	public GameObject go_purchaseBtn;

	public GameObject go_closeBtn;

	public GameObject go_discountBtn;

	public GameObject go_connectWait;

	public GameObject dialog;

	private TUIMeshText text_time;

	private bool bShowNewbiePack;

	private bool bShowDiscount;

	private bool bShowZombieUserDiscount;

	private string strZombieUserType = string.Empty;

	private float fDelta;

	private bool isiap;

	private string iapid;

	private string m_strIAP = string.Empty;

	public int m_nCountZero = 600;

	private MonoBehaviour eventlistener;

	public void Init()
	{
		Awake();
	}

	private void Awake()
	{
		text_time = go_time.transform.Find("Time").GetComponent<TUIMeshText>();
		if (Crazy_Data.CurData().IsShowNewbie())
		{
			if (Crazy_GlobalData.cur_fight_num == 1)
			{
				ShowNewbiePanel();
			}
		}
		else if (Crazy_Data.CurData().IsShowDiscount() && Crazy_GlobalData.g_bActiveUser && Crazy_GlobalData.cur_fight_num == 1)
		{
			ShowDiscountPanel();
			Crazy_Data.CurData().SetShowDiscount(false);
			Crazy_Data.CurData().SetDiscountTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Crazy_Data.SaveData();
		}
		if (Crazy_GlobalData.g_ZombieUser != 0)
		{
			switch (Crazy_GlobalData.g_ZombieUser)
			{
			case Crazy_GlobalData.ZombieUser.ZombieUser7:
				strZombieUserType = "ZombieUser7";
				break;
			case Crazy_GlobalData.ZombieUser.ZombieUser14:
				strZombieUserType = "ZombieUser14";
				break;
			case Crazy_GlobalData.ZombieUser.ZombieUser21:
				strZombieUserType = "ZombieUser21";
				break;
			default:
				strZombieUserType = string.Empty;
				break;
			}
			ShowZombieUserDiscountPanel();
			Crazy_GlobalData.g_ZombieUser = Crazy_GlobalData.ZombieUser.None;
			Crazy_Data.CurData().SetDiscountType(strZombieUserType);
			Crazy_Data.CurData().SetDiscountTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Crazy_Data.SaveData();
		}
	}

	private void Start()
	{
		MyGUIEventListener.Get(go_purchaseBtn).EventHandleOnClicked += OnPurchase;
		MyGUIEventListener.Get(go_closeBtn).EventHandleOnClicked += OnClose;
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
				AmazonIAP.initiateItemDataRequest(new string[5] { "com.trinitigame.tinysagainvasion.099centssale", "com.trinitigame.tinysagainvasion.199centssale", "com.trinitigame.tinysagainvasion.299centssale", "com.trinitigame.tinysagainvasion.299centsv22", "com.trinitigame.tinysagainvasion.1199centssale" });
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

	private void OnPurchase(GameObject go)
	{
		if (m_strIAP == string.Empty)
		{
			Debug.Log("-------------------unknow iap name-");
			return;
		}
		BuyIAP(m_strIAP);
		isiap = false;
		go_connectWait.SetActiveRecursively(false);
	}

	private void IAPSuccess(string id)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		hashtable.Add("IAPID", id);
		FlurryPlugin.logEvent("Buy_IAP", hashtable);
		if ("com.trinitigame.tinysagainvasion.099centssale" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_0.99_80OFF", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.199centssale" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_1.99_60OFF", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.299centssale" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_2.99_40OFF", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.299centsv22" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_2.99_NewbiePack", hashtable);
			Crazy_Data.CurData().SetGetNewbiePack(true);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 60);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 60;
			Crazy_Data.SaveData();
		}
		else if ("com.trinitigame.tinysagainvasion.1199centssale" == id)
		{
			hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_IAP_11.99_40OFF", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + 320);
			Crazy_GlobalData.cur_tc_times++;
			Crazy_GlobalData.cur_tc_num += 320;
			Crazy_Data.SaveData();
		}
		isiap = false;
		go_discountBtn.SetActiveRecursively(false);
		go_connectWait.SetActiveRecursively(false);
		base.gameObject.SetActiveRecursively(false);
		Crazy_Data.CurData().SetDiscountTime(string.Empty);
		Crazy_Data.SaveData();
	}

	private void Update()
	{
		fDelta += Time.deltaTime;
		if ((bShowDiscount || bShowZombieUserDiscount) && !(Crazy_Data.CurData().GetDiscountTime() == string.Empty))
		{
			TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Crazy_Data.CurData().GetDiscountTime());
			if (timeSpan.TotalSeconds >= 600.0)
			{
				base.gameObject.SetActiveRecursively(false);
				Crazy_Data.CurData().SetDiscountType(string.Empty);
				Crazy_Data.SaveData();
			}
			else if (timeSpan.TotalSeconds <= 0.0)
			{
				text_time.text = "09:59";
				text_time.UpdateMesh();
			}
			else if (fDelta >= 1f)
			{
				fDelta = 0f;
				m_nCountZero--;
				string text = (m_nCountZero / 60).ToString("D02");
				string text2 = (m_nCountZero % 60).ToString("D02");
				text_time.text = text + ":" + text2;
				text_time.UpdateMesh();
			}
		}
	}

	private void ShowNewbiePanel()
	{
		m_strIAP = "com.trinitigame.tinysagainvasion.299centsv22";
		bShowNewbiePack = true;
		base.gameObject.SetActiveRecursively(true);
		go_time.SetActiveRecursively(false);
		go_title.GetComponent<TUIMeshText>().text = "Newbie Pack";
		go_title.GetComponent<TUIMeshText>().UpdateMesh();
		go_intro.GetComponent<TUIMeshText>().text = "Restricted to one per player.";
		go_intro.GetComponent<TUIMeshText>().UpdateMesh();
		go_percent.GetComponent<TUIMeshText>().text = "40%";
		go_percent.GetComponent<TUIMeshText>().UpdateMesh();
		go_discountPrice.GetComponent<TUIMeshText>().text = "$2.99";
		go_discountPrice.GetComponent<TUIMeshText>().UpdateMesh();
		go_originalPrice.GetComponent<TUIMeshText>().text = "$4.99";
		go_originalPrice.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalCount.GetComponent<TUIMeshText>().text = "60";
		go_crystalCount.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalIcon.GetComponent<TUIMeshSprite>().frameName = "crystal02";
		go_crystalIcon.GetComponent<TUIMeshSprite>().UpdateMesh();
		Crazy_Data.CurData().SetShowNewbie(false);
		Crazy_Data.SaveData();
	}

	private void ShowDiscountPanel()
	{
		m_strIAP = "com.trinitigame.tinysagainvasion.1199centssale";
		bShowDiscount = true;
		base.gameObject.SetActiveRecursively(true);
		go_title.GetComponent<TUIMeshText>().text = "SPECIAL OFFER";
		go_title.GetComponent<TUIMeshText>().UpdateMesh();
		go_intro.GetComponent<TUIMeshText>().text = "You were selected for a major discount!";
		go_intro.GetComponent<TUIMeshText>().UpdateMesh();
		go_percent.GetComponent<TUIMeshText>().text = "40%";
		go_percent.GetComponent<TUIMeshText>().UpdateMesh();
		go_discountPrice.GetComponent<TUIMeshText>().text = "$11.99";
		go_discountPrice.GetComponent<TUIMeshText>().UpdateMesh();
		go_originalPrice.GetComponent<TUIMeshText>().text = "$19.99";
		go_originalPrice.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalCount.GetComponent<TUIMeshText>().text = "320";
		go_crystalCount.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalIcon.GetComponent<TUIMeshSprite>().frameName = "crystal04";
		go_crystalIcon.GetComponent<TUIMeshSprite>().UpdateMesh();
		go_time.SetActiveRecursively(true);
	}

	private void ShowZombieUserDiscountPanel()
	{
		bShowZombieUserDiscount = true;
		base.gameObject.SetActiveRecursively(true);
		go_time.SetActiveRecursively(true);
		go_title.GetComponent<TUIMeshText>().text = "SPECIAL OFFER";
		go_title.GetComponent<TUIMeshText>().UpdateMesh();
		go_intro.GetComponent<TUIMeshText>().text = "You were selected for a major discount!";
		go_intro.GetComponent<TUIMeshText>().UpdateMesh();
		go_originalPrice.GetComponent<TUIMeshText>().text = "$4.99";
		go_originalPrice.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalCount.GetComponent<TUIMeshText>().text = "60";
		go_crystalCount.GetComponent<TUIMeshText>().UpdateMesh();
		go_crystalIcon.GetComponent<TUIMeshSprite>().frameName = "crystal04";
		go_crystalIcon.GetComponent<TUIMeshSprite>().UpdateMesh();
		switch (strZombieUserType)
		{
		case "ZombieUser7":
			m_strIAP = "com.trinitigame.tinysagainvasion.299centssale";
			go_percent.GetComponent<TUIMeshText>().text = "40%";
			go_percent.GetComponent<TUIMeshText>().UpdateMesh();
			go_discountPrice.GetComponent<TUIMeshText>().text = "$2.99";
			go_discountPrice.GetComponent<TUIMeshText>().UpdateMesh();
			break;
		case "ZombieUser14":
			m_strIAP = "com.trinitigame.tinysagainvasion.199centssale";
			go_percent.GetComponent<TUIMeshText>().text = "60%";
			go_percent.GetComponent<TUIMeshText>().UpdateMesh();
			go_discountPrice.GetComponent<TUIMeshText>().text = "$1.99";
			go_discountPrice.GetComponent<TUIMeshText>().UpdateMesh();
			break;
		case "ZombieUser21":
			m_strIAP = "com.trinitigame.tinysagainvasion.099centssale";
			go_percent.GetComponent<TUIMeshText>().text = "80%";
			go_percent.GetComponent<TUIMeshText>().UpdateMesh();
			go_discountPrice.GetComponent<TUIMeshText>().text = "$0.99";
			go_discountPrice.GetComponent<TUIMeshText>().UpdateMesh();
			break;
		}
	}

	private void OnClose(GameObject go)
	{
		if (bShowDiscount || bShowZombieUserDiscount)
		{
			go_discountBtn.SetActiveRecursively(true);
		}
		base.gameObject.SetActiveRecursively(false);
	}

	public void ShowDialog(string text)
	{
		TUIMeshText component = dialog.transform.Find("Text").GetComponent<TUIMeshText>();
		component.text = text;
		component.UpdateMesh();
		dialog.SetActiveRecursively(true);
	}

	public void SetCountZero(int DeltaTime)
	{
		if (Crazy_Data.CurData().GetDiscountType() != string.Empty)
		{
			strZombieUserType = Crazy_Data.CurData().GetDiscountType();
			ShowZombieUserDiscountPanel();
		}
		else
		{
			ShowDiscountPanel();
		}
		m_nCountZero = DeltaTime;
		string text = (m_nCountZero / 60).ToString("D02");
		string text2 = (m_nCountZero % 60).ToString("D02");
		text_time.text = text + ":" + text2;
		text_time.UpdateMesh();
	}
}

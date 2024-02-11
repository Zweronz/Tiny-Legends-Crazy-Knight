using System;
using System.Collections;
using UnityEngine;

public class MageSellPanel : MonoBehaviour
{
	public GameObject mTUI;

	public MyGUIImageButton purchaseBtn;

	public TUIMeshText crystalCount;

	public GameObject go_closeBtn;

	public GameObject go_mageSellBtn;

	public TUIMeshText text_time;

	public MyGUIImageButton bankBtn;

	public int m_nCountZero = 600;

	private float fDelta;

	public void SetShowTime()
	{
		Crazy_Data.CurData().SetMageSellTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		Crazy_Data.SaveData();
	}

	private void Start()
	{
		MyGUIEventListener.Get(purchaseBtn.gameObject).EventHandleOnClicked += OnPurchase;
		MyGUIEventListener.Get(go_closeBtn).EventHandleOnClicked += OnClose;
		MyGUIEventListener.Get(bankBtn.gameObject).EventHandleOnClicked += OnGoBank;
		crystalCount.text = Crazy_Data.CurData().GetCrystal().ToString();
		crystalCount.UpdateMesh();
		if (Crazy_Data.CurData().GetCrystal() < 50)
		{
			purchaseBtn.disabled = true;
		}
		else
		{
			purchaseBtn.disabled = false;
		}
	}

	private void OnEnable()
	{
		crystalCount.text = Crazy_Data.CurData().GetCrystal().ToString();
		crystalCount.UpdateMesh();
		if (Crazy_Data.CurData().GetCrystal() < 50)
		{
			purchaseBtn.disabled = true;
		}
		else
		{
			purchaseBtn.disabled = false;
		}
	}

	private void OnGoBank(GameObject go)
	{
		Crazy_GlobalData.pre_scene = "CrazyMap";
		mTUI.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyIAP");
		Resources.UnloadUnusedAssets();
	}

	private void OnPurchase(GameObject go)
	{
		if (!purchaseBtn.disabled)
		{
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - 50);
			Crazy_Data.CurData().SetPlayerClass(Crazy_PlayerClass.Mage);
			Crazy_Data.CurData().SetUnlock(Crazy_PlayerClass.Mage, true);
			Crazy_Data.SaveData();
			mTUI.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("CrazyHero");
			Resources.UnloadUnusedAssets();
			base.gameObject.SetActiveRecursively(false);
			go_mageSellBtn.SetActiveRecursively(false);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			int num = Crazy_Data.CurData().GetAllPlayTime() / 60;
			if (num <= 10)
			{
				hashtable.Add("Playtime", num);
			}
			else
			{
				hashtable.Add("Playtime", num / 10 * 10);
			}
			FlurryPlugin.logEvent("Buy_Mage", hashtable);
		}
	}

	private void OnClose(GameObject go)
	{
		go_mageSellBtn.SetActiveRecursively(true);
		base.gameObject.SetActiveRecursively(false);
	}

	public void SetCountZero(int DeltaTime)
	{
		m_nCountZero = DeltaTime;
		string text = (m_nCountZero / 60).ToString("D02");
		string text2 = (m_nCountZero % 60).ToString("D02");
		text_time.text = text + ":" + text2;
		text_time.UpdateMesh();
	}

	private void Update()
	{
		if (!(Crazy_Data.CurData().GetMageSellTime() == string.Empty))
		{
			fDelta += Time.deltaTime;
			TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Crazy_Data.CurData().GetMageSellTime());
			if (timeSpan.TotalSeconds >= 600.0)
			{
				base.gameObject.SetActiveRecursively(false);
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
}

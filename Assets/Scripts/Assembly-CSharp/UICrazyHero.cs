using System.Collections;
using UnityEngine;

public class UICrazyHero : MonoBehaviour, TUIHandler
{
	private TUI m_tui_down;

	private TUI m_tui_up;

	private bool press;

	private float presslength;

	public GameObject rotate;

	public GameObject state;

	public void Start()
	{
		Crazy_GlobalData.cur_UI6Times++;
		Crazy_GlobalData.cur_ui = 6;
		m_tui_down = TUI.Instance("TUI/Down/TUI");
		m_tui_down.SetHandler(this);
		m_tui_up = TUI.Instance("TUI/Up/TUI");
		m_tui_up.SetHandler(this);
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
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
		else if (control.name == "PlayButton" && eventType == 3)
		{
			OnChooseHero();
		}
		else if (control.name == "BuyButton" && eventType == 3)
		{
			OnBuy();
		}
		else if (control.name == "MoveButton")
		{
			switch (eventType)
			{
			case 1:
				OnBeginMove();
				break;
			case 2:
				OnMove(wparam);
				break;
			case 3:
				OnEndMove();
				break;
			}
		}
		else if (control.name == "GoButton" && eventType == 3)
		{
			Crazy_Global.OnRecommend();
		}
	}

	public void OnBeginMove()
	{
		press = true;
		presslength = 0f;
	}

	public void OnMove(float param)
	{
		if (press)
		{
			presslength += param;
			if (presslength >= 10f)
			{
				rotate.SendMessage("ToLast", SendMessageOptions.DontRequireReceiver);
				press = false;
			}
			else if (presslength <= -10f)
			{
				rotate.SendMessage("ToNext", SendMessageOptions.DontRequireReceiver);
				press = false;
			}
		}
	}

	public void OnEndMove()
	{
		press = false;
	}

	public void OnChooseHero()
	{
		Crazy_Data.CurData().SetPlayerClass(state.GetComponent<UtilUIHeroControl>().cpc);
		Crazy_Data.SaveData();
		OnMap();
	}

	public void OnBuy()
	{
		Crazy_PlayerClass cpc = state.GetComponent<UtilUIHeroControl>().cpc;
		Hashtable hashtable = new Hashtable();
		switch (cpc)
		{
		case Crazy_PlayerClass.Rogue:
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("Buy_Bowmaster", hashtable);
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - 10);
			Crazy_Data.CurData().SetUnlock(Crazy_PlayerClass.Rogue, true);
			Crazy_Data.CurData().SetPlayerClass(Crazy_PlayerClass.Rogue);
			break;
		case Crazy_PlayerClass.Mage:
		{
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - 50);
			Crazy_Data.CurData().SetUnlock(Crazy_PlayerClass.Mage, true);
			Crazy_Data.CurData().SetPlayerClass(Crazy_PlayerClass.Mage);
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
			break;
		}
		default:
			Debug.LogError("Buy Error");
			break;
		}
		state.SendMessage("HideAll", cpc);
		state.SendMessage("updateState", cpc);
		rotate.SendMessage("updateState");
	}

	public void OnMap()
	{
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyMap");
	}

	public void OnIAP()
	{
		Crazy_GlobalData.pre_scene = "CrazyHero";
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyIAP");
	}

	public void OnDestroy()
	{
	}
}

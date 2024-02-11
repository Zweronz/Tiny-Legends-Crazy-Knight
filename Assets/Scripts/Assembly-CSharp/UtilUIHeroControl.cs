using UnityEngine;

public class UtilUIHeroControl : MonoBehaviour
{
	protected GameObject stateboard;

	protected GameObject text;

	public Vector3 itemposition;

	public Vector3 btnposition;

	public Vector3 btnposition2;

	public Vector3 textposition;

	public TUIButtonClick playbtn;

	public TUIButtonClick buybtn;

	public TUIButtonClick gobtn;

	public Crazy_PlayerClass cpc;

	private void Start()
	{
		stateboard = base.transform.Find("StateBoard").gameObject;
		text = base.transform.Find("Hint").gameObject;
	}

	private void ControlBuyBtn(Crazy_PlayerClass cpc)
	{
		if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Rogue)
		{
			buybtn.transform.localPosition = btnposition;
			if (Crazy_Data.CurData().GetCrystal() < 10)
			{
				buybtn.GetComponent<TUIButtonClick>().SetDisabled(true);
			}
			else
			{
				buybtn.GetComponent<TUIButtonClick>().SetDisabled(false);
			}
		}
		else if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Mage)
		{
			buybtn.transform.localPosition = btnposition;
			if (Crazy_Data.CurData().GetCrystal() < 50)
			{
				buybtn.GetComponent<TUIButtonClick>().SetDisabled(true);
			}
			else
			{
				buybtn.GetComponent<TUIButtonClick>().SetDisabled(false);
			}
		}
		else
		{
			buybtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		}
	}

	private void ControlGoBtn(Crazy_PlayerClass cpc)
	{
		if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Paladin)
		{
			gobtn.transform.localPosition = btnposition2;
		}
		else
		{
			gobtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		}
	}

	private void ControlPlayBtn(Crazy_PlayerClass cpc)
	{
		if (Crazy_Data.CurData().GetUnlock(cpc))
		{
			playbtn.transform.localPosition = btnposition;
		}
		else
		{
			playbtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		}
	}

	private void ControlStateBoard(Crazy_PlayerClass cpc)
	{
		if (!Crazy_Data.CurData().GetUnlock(cpc))
		{
			stateboard.transform.localPosition = new Vector3(1000f, 1000f, 0f);
			return;
		}
		stateboard.transform.localPosition = itemposition;
		stateboard.SendMessage("updateClassState", cpc, SendMessageOptions.DontRequireReceiver);
	}

	private void ControlText(Crazy_PlayerClass cpc)
	{
		if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Rogue)
		{
			text.transform.localPosition = textposition;
			TUIMeshText component = text.GetComponent<TUIMeshText>();
			component.text = "Hire Torvass for 10 tCrystals?";
			component.UpdateMesh();
		}
		else if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Warrior)
		{
			text.transform.localPosition = textposition;
			TUIMeshText component2 = text.GetComponent<TUIMeshText>();
			component2.text = "Beat Lv.15 Boss";
			component2.UpdateMesh();
		}
		else if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Fighter)
		{
			text.transform.localPosition = textposition;
			TUIMeshText component3 = text.GetComponent<TUIMeshText>();
			component3.text = "Beat Lv.10 Boss";
			component3.UpdateMesh();
		}
		else if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Paladin)
		{
			text.transform.localPosition = textposition;
			TUIMeshText component4 = text.GetComponent<TUIMeshText>();
			component4.text = "This character must be rescued from another game!";
			component4.UpdateMesh();
		}
		else if (!Crazy_Data.CurData().GetUnlock(cpc) && cpc == Crazy_PlayerClass.Mage)
		{
			text.transform.localPosition = textposition;
			TUIMeshText component5 = text.GetComponent<TUIMeshText>();
			component5.text = "Hire Merrill for 50 tCrystals?";
			component5.UpdateMesh();
		}
	}

	private void HideAll()
	{
		buybtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		buybtn.GetComponent<TUIButtonClick>().Reset();
		playbtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		playbtn.GetComponent<TUIButtonClick>().Reset();
		gobtn.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		gobtn.GetComponent<TUIButtonClick>().Reset();
		stateboard.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		text.transform.localPosition = new Vector3(1000f, 1000f, 0f);
	}

	private void updateState(Crazy_PlayerClass curcpc)
	{
		cpc = curcpc;
		ControlStateBoard(cpc);
		ControlPlayBtn(cpc);
		ControlBuyBtn(cpc);
		ControlGoBtn(cpc);
		ControlText(cpc);
	}

	private void Update()
	{
	}
}

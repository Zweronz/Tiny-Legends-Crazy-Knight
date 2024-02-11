using UnityEngine;

public class UtilUIStateBoardEx : MonoBehaviour
{
	protected Crazy_PlayerClass m_class;

	protected TUIMeshText lv;

	protected UtilUIBloodBoard blood;

	protected TUIMeshText damage;

	protected TUIMeshText mastery;

	protected new TUIMeshText name;

	private void Start()
	{
		lv = base.transform.Find("LV/Text").GetComponent<TUIMeshText>();
		blood = base.transform.Find("BloodBoard").GetComponent("UtilUIBloodBoard") as UtilUIBloodBoard;
		damage = base.transform.Find("State/Damage/TextDamageData").GetComponent<TUIMeshText>();
		mastery = base.transform.Find("State/Mastery/TextMasteryData").GetComponent<TUIMeshText>();
		name = base.transform.Find("State/Name/TextName").GetComponent<TUIMeshText>();
	}

	private void updateName()
	{
		string text = null;
		switch (m_class)
		{
		case Crazy_PlayerClass.Fighter:
			text = "Lina";
			break;
		case Crazy_PlayerClass.Knight:
			text = "Vallen";
			break;
		case Crazy_PlayerClass.Warrior:
			text = "Ganthor";
			break;
		case Crazy_PlayerClass.Rogue:
			text = "Torvass";
			break;
		case Crazy_PlayerClass.Paladin:
			text = "Hilan";
			break;
		case Crazy_PlayerClass.Mage:
			text = "Merrill";
			break;
		}
		name.text = text.ToUpper();
		name.UpdateMesh();
	}

	private void updateMastery()
	{
		string text = null;
		switch (m_class)
		{
		case Crazy_PlayerClass.Fighter:
			text = "Sword MASTER";
			break;
		case Crazy_PlayerClass.Knight:
			text = "Axe MASTER";
			break;
		case Crazy_PlayerClass.Warrior:
			text = "Hammer MASTER";
			break;
		case Crazy_PlayerClass.Rogue:
			text = "Bow MASTER";
			break;
		case Crazy_PlayerClass.Paladin:
			text = "Balanced";
			break;
		case Crazy_PlayerClass.Mage:
			text = "Mage";
			break;
		}
		mastery.text = text.ToUpper();
		mastery.UpdateMesh();
	}

	private void updateLv()
	{
		lv.text = "L" + Crazy_Data.CurData().GetLevel(m_class);
		lv.UpdateMesh();
	}

	private void updateStateData()
	{
		updateMastery();
		updateName();
		int num = Crazy_PlayerClass_Level.GetPlayerLevelinfo(m_class, Crazy_Data.CurData().GetLevel(m_class)).damage;
		damage.text = "DMG " + num;
		damage.UpdateMesh();
	}

	private void updateState()
	{
		updateLv();
		updateStateData();
		blood.UpdateBlood(3);
	}

	public void updateClassState(Crazy_PlayerClass _class)
	{
		m_class = _class;
		Invoke("updateState", 0.01f);
	}

	private void Update()
	{
	}
}

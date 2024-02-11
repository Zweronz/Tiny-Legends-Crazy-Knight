using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPanel : MonoBehaviour
{
	public GameObject plusBtn;

	public GameObject minusBtn;

	public GameObject buyBtn;

	public GameObject bankBtn;

	public GameObject propShow;

	public TUIMeshText cost;

	public TUIMeshText property;

	public new Crazy_PlayTAudio audio;

	private Crazy_PropItem propItem;

	private int m_nPrice;

	private int m_nCost;

	private int m_nProperty;

	private int m_nCount = 1;

	private List<string> digitList = new List<string>();

	private PropPanel m_PropPanel;

	private void Awake()
	{
		for (int i = 0; i < 10; i++)
		{
			digitList.Add("digit_" + i);
		}
	}

	private void OnEnable()
	{
		propItem = Crazy_PropItem.GetSkillItem(m_PropPanel.GetFocusedPropID());
		TUIMeshSprite component = propShow.transform.Find("Icon").GetComponent<TUIMeshSprite>();
		component.frameName = propItem.m_strSmallIcon;
		component.UpdateMesh();
		cost.text = propItem.priceItems[0].nPrice;
		cost.UpdateMesh();
		m_nCount = 1;
		m_nCost = (m_nPrice = int.Parse(propItem.priceItems[0].nPrice));
		m_nProperty = Crazy_Data.CurData().GetCrystal();
		property.text = m_nProperty.ToString();
		property.UpdateMesh();
		m_nProperty -= m_nCost;
		TUIMeshSprite component2 = propShow.transform.Find("Num").GetComponent<TUIMeshSprite>();
		component2.frameName = digitList[m_nCount];
		component2.UpdateMesh();
		plusBtn.GetComponent<MyGUIImageButton>().disabled = false;
		int remainedCount = m_PropPanel.GetRemainedCount();
		if (m_nProperty < m_nPrice || m_PropPanel.GetRemainedCount() == 1)
		{
			plusBtn.GetComponent<MyGUIImageButton>().disabled = true;
		}
		minusBtn.GetComponent<MyGUIImageButton>().disabled = true;
	}

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnHideBuyPanel;
		MyGUIEventListener.Get(plusBtn).EventHandleOnClicked += OnPlusOne;
		MyGUIEventListener.Get(minusBtn).EventHandleOnClicked += OnMinusOne;
		MyGUIEventListener.Get(buyBtn).EventHandleOnClicked += OnBuy;
		MyGUIEventListener.Get(bankBtn).EventHandleOnClicked += OnGoBank;
	}

	public void SetPropPanel(PropPanel panel)
	{
		m_PropPanel = panel;
	}

	private void OnHideBuyPanel(GameObject go)
	{
		base.gameObject.SetActiveRecursively(false);
	}

	private void OnPlusOne(GameObject go)
	{
		audio.Play();
		if (!plusBtn.GetComponent<MyGUIImageButton>().disabled)
		{
			m_nCount++;
			m_nCost += m_nPrice;
			m_nProperty -= m_nPrice;
			TUIMeshSprite component = propShow.transform.Find("Num").GetComponent<TUIMeshSprite>();
			component.frameName = digitList[m_nCount];
			component.UpdateMesh();
			cost.text = m_nCost.ToString();
			cost.UpdateMesh();
			if (m_nProperty < m_nPrice || m_nCount == m_PropPanel.GetRemainedCount())
			{
				plusBtn.GetComponent<MyGUIImageButton>().disabled = true;
			}
			if (m_nCount > 0)
			{
				minusBtn.GetComponent<MyGUIImageButton>().disabled = false;
			}
		}
	}

	private void OnMinusOne(GameObject go)
	{
		audio.Play();
		if (!minusBtn.GetComponent<MyGUIImageButton>().disabled)
		{
			m_nCount--;
			m_nCost -= m_nPrice;
			m_nProperty += m_nPrice;
			TUIMeshSprite component = propShow.transform.Find("Num").GetComponent<TUIMeshSprite>();
			component.frameName = digitList[m_nCount];
			component.UpdateMesh();
			cost.text = m_nCost.ToString();
			cost.UpdateMesh();
			if (m_nProperty > m_nPrice || m_nCount < m_PropPanel.GetRemainedCount())
			{
				plusBtn.GetComponent<MyGUIImageButton>().disabled = false;
			}
			if (m_nCount == 1)
			{
				minusBtn.GetComponent<MyGUIImageButton>().disabled = true;
			}
		}
	}

	private void OnBuy(GameObject go)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("Buy_Item_" + m_PropPanel.GetFocusedPropID(), hashtable);
		base.transform.GetComponent<TAudioController>().PlayAudio("UI_Button_Buy");
		if (m_nCount == 0)
		{
			base.gameObject.SetActiveRecursively(false);
			return;
		}
		Crazy_Data.CurData().SetCrystal(m_nProperty);
		Crazy_Data.SaveData();
		base.gameObject.SetActiveRecursively(false);
		m_PropPanel.UpdatePropProperty(m_nCount);
	}

	private void OnGoBank(GameObject go)
	{
		audio.Play();
		m_PropPanel.OnGoBank(go);
	}

	private void Update()
	{
	}
}

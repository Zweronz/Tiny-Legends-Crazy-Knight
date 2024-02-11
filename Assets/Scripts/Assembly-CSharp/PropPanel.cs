using System.Collections;
using UnityEngine;

public class PropPanel : MonoBehaviour
{
	public GameObject mTUI;

	public GameObject introduce;

	public GameObject buyPanel;

	public GameObject buyBtn;

	public GameObject equipBtn;

	public GameObject unequipBtn;

	public GameObject showHero;

	public GameObject bankBtn;

	public GameObject land;

	public GameObject go_focusedProp;

	public GameObject go_backBtn;

	public GameObject go_collider1;

	public GameObject go_collider2;

	public TUIMeshText uiFocusedNum;

	public TUIMeshSprite uiFocusedIcon;

	public GameObject go_equipedProp;

	public GameObject go_equippingEffect;

	public TUIMeshText uiEquipedNum;

	public TUIMeshSprite uiEquipedIcon;

	public TUIMeshText desc;

	public ShowHero m_showHero;

	public new Crazy_PlayTAudio audio;

	private PropItem currentProp;

	private PropItem currentEquipedProp;

	private int focusedPropID;

	private int equipedPropID;

	private int m_nCount;

	private int m_nPrice;

	private float m_fDelta;

	private void Awake()
	{
		if (!Crazy_Beginner.instance.isProp)
		{
			introduce.SetActiveRecursively(false);
		}
	}

	private void OnEnable()
	{
		if (!Crazy_Beginner.instance.isProp)
		{
			introduce.SetActiveRecursively(false);
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("Enter_item", hashtable);
		buyPanel.SetActiveRecursively(false);
		go_equippingEffect.SetActiveRecursively(false);
		equipedPropID = Crazy_Data.CurData().GetLastProp();
		if (equipedPropID == -1)
		{
			focusedPropID = (equipedPropID = 1);
			equipBtn.SetActiveRecursively(true);
			unequipBtn.SetActiveRecursively(false);
			go_equipedProp.SetActiveRecursively(false);
			UpdateEquipedProp(false);
		}
		else
		{
			focusedPropID = equipedPropID;
			equipBtn.SetActiveRecursively(false);
			unequipBtn.SetActiveRecursively(true);
			go_equipedProp.SetActiveRecursively(true);
			UpdateEquipedProp(true);
		}
		UpdateFocusedProp();
		buyPanel.GetComponent<BuyPanel>().SetPropPanel(this);
	}

	private void Start()
	{
		MyGUIEventListener.Get(buyBtn).EventHandleOnClicked += OnBuy;
		MyGUIEventListener.Get(equipBtn).EventHandleOnClicked += OnEquip;
		MyGUIEventListener.Get(unequipBtn).EventHandleOnClicked += OnUnequip;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnHidePropPanel;
		MyGUIEventListener.Get(go_collider1).EventHandleOnClicked += OnHidePropPanel;
		MyGUIEventListener.Get(go_collider2).EventHandleOnClicked += OnHidePropPanel;
		MyGUIEventListener.Get(go_backBtn).EventHandleOnClicked += OnHidePropPanel;
		MyGUIEventListener.Get(bankBtn).EventHandleOnClicked += OnGoBank;
	}

	public void SetEquipedProp(PropItem _prop)
	{
		currentEquipedProp = _prop;
	}

	public void SetFocusedProp(PropItem _prop)
	{
		currentProp = _prop;
		focusedPropID = _prop.propID;
		m_nCount = Crazy_Data.CurData().GetPropCount(focusedPropID);
		if (focusedPropID == Crazy_Data.CurData().GetLastProp())
		{
			equipBtn.SetActiveRecursively(false);
			unequipBtn.SetActiveRecursively(true);
		}
		else
		{
			equipBtn.SetActiveRecursively(true);
			unequipBtn.SetActiveRecursively(false);
			if (m_nCount == 0)
			{
				equipBtn.GetComponent<MyGUIImageButton>().disabled = true;
			}
		}
		UpdateFocusedProp();
		m_fDelta = 5f;
	}

	private void OnBuy(GameObject go)
	{
		if (!buyBtn.GetComponent<MyGUIImageButton>().disabled)
		{
			buyPanel.SetActiveRecursively(true);
		}
		audio.Play();
	}

	public void UpdatePropProperty(int _count)
	{
		Crazy_Data.CurData().SetPropCount(focusedPropID, _count);
		Crazy_Data.SaveData();
		m_nCount = Crazy_Data.CurData().GetPropCount(focusedPropID);
		uiFocusedNum.text = m_nCount.ToString();
		uiFocusedNum.UpdateMesh();
		currentProp.amount = m_nCount;
		if (currentProp.equiped)
		{
			uiEquipedNum.text = m_nCount.ToString();
			uiEquipedNum.UpdateMesh();
		}
		if (m_nCount == 9 || Crazy_Data.CurData().GetCrystal() < m_nPrice)
		{
			buyBtn.GetComponent<MyGUIImageButton>().disabled = true;
		}
		if (m_nCount > 0)
		{
			equipBtn.GetComponent<MyGUIImageButton>().disabled = false;
		}
	}

	private void UpdateFocusedProp()
	{
		Crazy_PropItem skillItem = Crazy_PropItem.GetSkillItem(focusedPropID);
		int propCount = Crazy_Data.CurData().GetPropCount(focusedPropID);
		uiFocusedNum.text = propCount.ToString();
		uiFocusedNum.UpdateMesh();
		uiFocusedIcon.frameName = skillItem.m_strMiddleIcon;
		uiFocusedIcon.UpdateMesh();
		MyGUIImageButton component = equipBtn.GetComponent<MyGUIImageButton>();
		if (propCount == 0)
		{
			component.disabled = true;
		}
		else
		{
			component.disabled = false;
		}
		m_nPrice = int.Parse(skillItem.priceItems[0].nPrice);
		if (m_nCount == 9 || Crazy_Data.CurData().GetCrystal() < m_nPrice)
		{
			buyBtn.GetComponent<MyGUIImageButton>().disabled = true;
		}
		else
		{
			buyBtn.GetComponent<MyGUIImageButton>().disabled = false;
		}
		desc.text = skillItem.m_strDesc.Replace("\\n", "\n");
		desc.UpdateMesh();
	}

	private void Update()
	{
		m_fDelta += Time.deltaTime;
		if (m_fDelta >= 5f)
		{
			m_fDelta = 0f;
			Crazy_PropItem skillItem = Crazy_PropItem.GetSkillItem(focusedPropID);
			m_showHero.PlayEffect(skillItem.m_strPropName);
		}
	}

	private void UpdateEquipedProp(bool active)
	{
		if (active)
		{
			Crazy_PropItem skillItem = Crazy_PropItem.GetSkillItem(equipedPropID);
			int propCount = Crazy_Data.CurData().GetPropCount(equipedPropID);
			if (propCount == 0)
			{
				go_equipedProp.SetActiveRecursively(false);
				return;
			}
			go_equipedProp.SetActiveRecursively(true);
			uiEquipedNum.text = propCount.ToString();
			uiEquipedNum.UpdateMesh();
			uiEquipedIcon.frameName = skillItem.m_strSmallIcon;
			uiEquipedIcon.UpdateMesh();
		}
		else
		{
			go_equipedProp.SetActiveRecursively(false);
		}
	}

	private void OnEquip(GameObject go)
	{
		if (equipBtn.GetComponent<MyGUIImageButton>().disabled)
		{
			base.transform.GetComponent<TAudioController>().PlayAudio("UI_Button_General");
			return;
		}
		base.transform.GetComponent<TAudioController>().PlayAudio("UI_Button_EqItem");
		if (equipedPropID != -1 && currentEquipedProp != null)
		{
			currentEquipedProp.SetEquiped(false);
		}
		currentEquipedProp = currentProp;
		equipedPropID = focusedPropID;
		go_equippingEffect.SetActiveRecursively(true);
		UpdateEquipedProp(true);
		equipBtn.SetActiveRecursively(false);
		unequipBtn.SetActiveRecursively(true);
		currentProp.equiped = true;
		currentProp.SetEquiped(true);
		if (Crazy_Beginner.instance.isProp)
		{
			Animation[] components = introduce.GetComponents<Animation>();
			Animation[] array = components;
			foreach (Animation animation in array)
			{
				animation.GetComponent<Animation>()["MeshFadeOut"].wrapMode = WrapMode.ClampForever;
				animation.Play("MeshFadeOut");
			}
			Crazy_Beginner.instance.isProp = false;
		}
		Crazy_Data.CurData().SetLastProp(equipedPropID);
		Crazy_Data.SaveData();
	}

	private void OnUnequip(GameObject go)
	{
		audio.Play();
		UpdateEquipedProp(false);
		equipBtn.SetActiveRecursively(true);
		unequipBtn.SetActiveRecursively(false);
		currentProp.SetEquiped(false);
		equipedPropID = -1;
		currentEquipedProp = null;
		Crazy_Data.CurData().SetLastProp(equipedPropID);
		Crazy_Data.SaveData();
	}

	public int GetRemainedCount()
	{
		int num = 9 - Crazy_Data.CurData().GetPropCount(focusedPropID);
		if (num < 0 || num > 9)
		{
			Debug.LogWarning("out of range");
		}
		return num;
	}

	public int GetFocusedPropID()
	{
		return focusedPropID;
	}

	private void OnHidePropPanel(GameObject go)
	{
		base.gameObject.SetActiveRecursively(false);
		showHero.SetActiveRecursively(false);
		land.SendMessage("ResetAllStage", SendMessageOptions.DontRequireReceiver);
	}

	public void OnGoBank(GameObject go)
	{
		audio.Play();
		Crazy_GlobalData.pre_scene = "CrazyMap";
		mTUI.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyIAP");
		Resources.UnloadUnusedAssets();
	}
}

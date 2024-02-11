using System.Collections;
using UnityEngine;

public class EventPropBtn : MonoBehaviour
{
	public GameObject go_price;

	public GameObject go_property;

	public TUIMeshText hint;

	public GameObject go_amount;

	public TUIMeshText money;

	private MyGUIFilledSprite m_mask;

	private Crazy_PropItem propItem;

	private int m_nPrice;

	private int m_nCount;

	private int m_nCurrentPropID;

	private Color hintColor;

	private void Awake()
	{
		if (!Crazy_Data.CurData().IsShowPropPanel())
		{
			base.gameObject.SetActiveRecursively(false);
		}
		m_nCurrentPropID = Crazy_Data.CurData().GetLastProp();
		if (m_nCurrentPropID == -1)
		{
			base.gameObject.SetActiveRecursively(false);
			return;
		}
		propItem = Crazy_PropItem.GetSkillItem(m_nCurrentPropID);
		m_nPrice = int.Parse(propItem.priceItems[0].nPrice);
		m_nCount = Crazy_Data.CurData().GetPropCount(m_nCurrentPropID);
	}

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnUseProp;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnPressed += OnPress;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnReleased += OnRelease;
		money.text = Crazy_Data.CurData().GetCrystal().ToString();
		money.UpdateMesh();
		MyguiButton component = base.transform.GetComponent<MyguiButton>();
		component.normalSprite = propItem.m_strSmallIcon;
		component.pressedSprite = propItem.m_strSmallIconHL;
		TUIMeshSprite component2 = base.transform.Find("SpriteIcon").GetComponent<TUIMeshSprite>();
		component2.frameName = propItem.m_strSmallIcon;
		component2.UpdateMesh();
		TUIMeshText component3 = go_price.transform.Find("Num").GetComponent<TUIMeshText>();
		component3.text = m_nPrice.ToString();
		component3.UpdateMesh();
		TUIMeshText component4 = go_amount.transform.Find("Num").GetComponent<TUIMeshText>();
		component4.text = m_nCount.ToString();
		component4.UpdateMesh();
		go_price.SetActiveRecursively(false);
		go_property.SetActiveRecursively(false);
		m_mask = base.transform.Find("SpriteMask").GetComponent<MyGUIFilledSprite>();
		m_mask.fillAmount = 0f;
		m_mask.mCDTime = propItem.m_fCDTime;
		m_mask.Static = true;
		hintColor = hint.color;
	}

	private void OnPress(GameObject go, Vector2 vec)
	{
		TUIMeshSprite component = go_amount.transform.Find("BK").GetComponent<TUIMeshSprite>();
		component.frameName = "num_slot_hl";
		component.UpdateMesh();
	}

	private void OnRelease(GameObject go, Vector2 vec)
	{
		TUIMeshSprite component = go_amount.transform.Find("BK").GetComponent<TUIMeshSprite>();
		component.frameName = "num_slot";
		component.UpdateMesh();
	}

	private void OnUseProp(GameObject go)
	{
		if (Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.IsDie() || !m_mask.Static)
		{
			return;
		}
		m_mask.fillAmount = 1f;
		m_mask.Static = false;
		if (m_nCount == 0)
		{
			if (Crazy_Data.CurData().GetCrystal() >= m_nPrice)
			{
				Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - m_nPrice);
				money.text = Crazy_Data.CurData().GetCrystal().ToString();
				money.UpdateMesh();
				PropsAction.UseProp(propItem.m_strPropName);
			}
			else
			{
				hint.text = "Not Enough Crystals!";
				hint.color = new Color(1f, 0f, 0f, 1f);
				hint.UpdateMesh();
				StartCoroutine(HintFadeAway());
			}
			return;
		}
		m_nCount--;
		if (m_nCount == 0)
		{
			go_price.SetActiveRecursively(true);
			go_property.SetActiveRecursively(true);
			Crazy_Data.CurData().SetLastProp(-1);
		}
		TUIMeshText component = go_amount.transform.Find("Num").GetComponent<TUIMeshText>();
		component.text = m_nCount.ToString();
		component.UpdateMesh();
		PropsAction.UseProp(propItem.m_strPropName);
		Crazy_Data.CurData().SetPropCount(m_nCurrentPropID, -1);
	}

	private IEnumerator HintFadeAway()
	{
		Color c = hint.color;
		while ((double)c.a > 0.0)
		{
			c.a -= Time.deltaTime * 0.25f;
			hint.color = c;
			hint.UpdateMesh();
			yield return null;
		}
	}
}

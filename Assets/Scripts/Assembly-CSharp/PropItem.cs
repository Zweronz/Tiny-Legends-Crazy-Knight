using UnityEngine;

public class PropItem : MonoBehaviour
{
	public TUIMeshTextClip num;

	public TUIMeshSpriteClip num_slot;

	public TUIMeshSpriteClip bk;

	public TUIMeshSpriteClip payicon;

	public TUIMeshSpriteClip icon;

	public TUIMeshTextClip price;

	public TUIMeshSpriteClip equipedSign;

	private PropPanel panel;

	public int propID;

	private int m_amount = -1;

	public string focusIcon;

	public string desc;

	private bool m_bEquiped;

	public bool m_focused;

	public TUIMeshSprite sprite;

	public string normalSprite;

	public string focusedSprite;

	public int amount
	{
		get
		{
			return m_amount;
		}
		set
		{
			if (m_amount != value)
			{
				m_amount = value;
				num.text = amount.ToString();
				num.UpdateMesh();
			}
		}
	}

	public bool equiped
	{
		get
		{
			return m_bEquiped;
		}
		set
		{
			if (m_bEquiped != value)
			{
				m_bEquiped = value;
			}
		}
	}

	private void Awake()
	{
		if (sprite == null)
		{
			sprite = GetComponentInChildren<TUIMeshSprite>();
		}
	}

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnFocus += OnFocus;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnPressed += OnPress;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnReleased += OnRelease;
		equipedSign.gameObject.SetActiveRecursively(false);
		if (Crazy_Data.CurData().GetLastProp() == -1 && propID == 1)
		{
			OnFocus(base.gameObject);
			panel.SetEquipedProp(this);
		}
		if (Crazy_Data.CurData().GetLastProp() == propID)
		{
			OnFocus(base.gameObject);
			panel.SetEquipedProp(this);
			equipedSign.gameObject.SetActiveRecursively(true);
		}
	}

	private void OnEnable()
	{
		equipedSign.gameObject.SetActiveRecursively(false);
		if (Crazy_Data.CurData().GetLastProp() == -1 && propID == 1)
		{
			OnFocus(base.gameObject);
			panel.SetEquipedProp(this);
		}
		if (Crazy_Data.CurData().GetLastProp() == propID)
		{
			OnFocus(base.gameObject);
			panel.SetEquipedProp(this);
			equipedSign.gameObject.SetActiveRecursively(true);
		}
	}

	public void SetPropPanel(PropPanel _panel)
	{
		panel = _panel;
	}

	public void SetClipRect(TUIRect rect)
	{
		num.clip = rect;
		num_slot.clip = rect;
		bk.clip = rect;
		payicon.clip = rect;
		icon.clip = rect;
		price.clip = rect;
		equipedSign.clip = rect;
	}

	public void SetIcon(string iconName, string payIconName, string priceText)
	{
		icon.frameName = iconName;
		payicon.frameName = payIconName + "_big";
		price.text = priceText;
	}

	public void SetEquiped(bool b)
	{
		equipedSign.gameObject.SetActiveRecursively(b);
	}

	private void OnPress(GameObject go, Vector2 pos)
	{
		sprite.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
	}

	private void OnRelease(GameObject go, Vector2 pos)
	{
		sprite.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void OnFocus(GameObject go)
	{
		panel.SetFocusedProp(this);
		LoadPropItem.UpdateCurrentProp(this);
		m_focused = true;
		sprite.frameName = focusedSprite;
		sprite.UpdateMesh();
		num_slot.frameName = "num_slot_hl";
		num_slot.UpdateMesh();
	}

	public void OnLostFocus()
	{
		m_focused = false;
		sprite.frameName = normalSprite;
		sprite.UpdateMesh();
		num_slot.frameName = "num_slot";
		num_slot.UpdateMesh();
	}
}

using UnityEngine;

public class UtilUIShop : MonoBehaviour
{
	private int m_pageCount;

	private int m_pageIndex = -1;

	private TUIScroll m_scroll;

	public void Awake()
	{
		m_pageCount = 5;
		m_scroll = base.transform.Find("Scroll").gameObject.GetComponent<TUIScroll>();
		m_scroll.rangeYMin = 0f;
		m_scroll.borderYMin = m_scroll.rangeYMin - 120f;
		m_scroll.rangeYMax = (m_pageCount - 1) * 240;
		m_scroll.borderYMax = m_scroll.rangeYMax + 120f;
		m_scroll.pageY = new float[m_pageCount];
		for (int i = 0; i < m_pageCount; i++)
		{
			m_scroll.pageY[i] = (m_pageCount - 1 - i) * 240;
		}
		int num = 35;
		for (int j = 0; j < m_pageCount; j++)
		{
			GameObject gameObject = base.transform.Find("Page").Find(string.Format("PageDot{0:D02}", j + 1)).gameObject;
			gameObject.transform.localPosition = new Vector3(0f, (m_pageCount - 1) * num / 2 - j * num, 0f);
		}
	}

	protected void MoveToWeapon(Crazy_Weapon_Type type)
	{
		int num = 240;
		m_scroll.position.y = (int)type * num;
		m_scroll.SendMessage("ScrollObjectMove", SendMessageOptions.DontRequireReceiver);
	}

	public void Update()
	{
		int num = 0;
		int num2 = (int)m_scroll.position.y;
		for (int i = 0; i < m_pageCount; i++)
		{
			if (num2 < 240 * i + 120 + 1)
			{
				num = i;
				break;
			}
		}
		if (m_pageIndex != num)
		{
			UpdatePage(num);
		}
	}

	private void UpdatePage(int pageIndex)
	{
		m_pageIndex = pageIndex;
		for (int i = 0; i < m_pageCount; i++)
		{
			TUIButtonSelect component = base.transform.Find("Page").Find(string.Format("PageDot{0:D02}", i + 1)).GetComponent<TUIButtonSelect>();
			if (i == m_pageIndex)
			{
				component.SetSelected(true);
			}
			else
			{
				component.SetSelected(false);
			}
		}
	}
}

using System;
using UnityEngine;

public class UtilUIDeath01 : TUIIgnoreTimeScale
{
	[Serializable]
	public class ButtonInfo
	{
		public GameObject button;

		public Vector3 showposition;

		public Vector3 hideposition;

		public void Show()
		{
			button.transform.localPosition = showposition;
		}

		public void Hide()
		{
			button.transform.localPosition = hideposition;
		}
	}

	protected GameObject completeEffect;

	protected GameObject failedEffect;

	public ButtonInfo next;

	public ButtonInfo retry;

	public ButtonInfo exit;

	protected TUIMeshText m_combo;

	protected TUIMeshText m_combotitle;

	protected TUIMeshText m_gold;

	private void Awake()
	{
		completeEffect = base.transform.Find("TitleParent/EffectParent/EffectComplete").gameObject;
		failedEffect = base.transform.Find("TitleParent/EffectParent/EffectFailed").gameObject;
		m_combo = base.transform.Find("ComboBonus").GetComponent<TUIMeshText>();
		m_combotitle = base.transform.Find("ComboTitle").GetComponent<TUIMeshText>();
		m_gold = base.transform.Find("TotalGold").GetComponent<TUIMeshText>();
	}

	public void OnComplete(string framename, int combo, int gold)
	{
		if (framename != null)
		{
			exit.Hide();
			next.Show();
		}
		else
		{
			exit.Show();
			next.Hide();
		}
		completeEffect.transform.localPosition = Vector3.zero;
		completeEffect.GetComponent<Animation>().Play("jiesuan");
		retry.Hide();
		m_combo.text = combo + " Hits";
		m_gold.text = gold.ToString();
		m_combo.UpdateMesh();
		m_gold.UpdateMesh();
	}

	public void OnDeath(int gold)
	{
		failedEffect.transform.localPosition = Vector3.zero;
		failedEffect.GetComponent<Animation>().Play("jiesuan_02");
		next.Hide();
		retry.Show();
		exit.Show();
		m_gold.text = gold.ToString();
		m_combo.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		m_combotitle.transform.localPosition = new Vector3(1000f, 1000f, 0f);
		m_gold.UpdateMesh();
	}
}

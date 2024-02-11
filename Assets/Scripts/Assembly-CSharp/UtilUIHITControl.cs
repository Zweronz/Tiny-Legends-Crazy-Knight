using UnityEngine;

public class UtilUIHITControl : MonoBehaviour
{
	protected Crazy_PlayAnimation effectanimation;

	protected TUIMeshText combotext;

	protected int cur_combo;

	private void Start()
	{
		effectanimation = base.transform.Find("HIT").gameObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
		effectanimation.Hide();
		combotext = base.transform.Find("HIT/Plane01").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		UpdateComboText();
	}

	private void UpdateComboText()
	{
		int combo = Crazy_SceneManager.GetInstance().GetScene().GetPlayerControl()
			.GetCombo();
		if (cur_combo != combo)
		{
			cur_combo = combo;
			if (cur_combo > 1)
			{
				combotext.text = cur_combo + "H";
				combotext.UpdateMesh();
				effectanimation.Stop();
				effectanimation.Play();
			}
		}
	}

	private void Update()
	{
		UpdateComboText();
	}
}

using UnityEngine;

public class UtilUILevelExpControl : MonoBehaviour
{
	protected TUIMeshText exptext;

	protected TUIMeshText lvtext;

	protected TUIMeshSprite expslot;

	protected TUIRect expcliprect;

	protected int cur_exp = -1;

	protected int d_exp;

	protected int lv;

	protected float lv_effect_time = 0.5f;

	protected bool stopup;

	protected float last_effect_time;

	protected Crazy_PlayAnimation effectanimation;

	protected Crazy_ParticleSequenceScript effectcpss;

	private void Start()
	{
		exptext = base.transform.Find("Exp").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		lvtext = base.transform.Find("Lv").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		expslot = base.transform.Find("ExpSlot/Exp").gameObject.GetComponent("TUIMeshSprite") as TUIMeshSprite;
		expcliprect = base.transform.Find("ExpSlot/ExpClipRect").gameObject.GetComponent("TUIRect") as TUIRect;
		effectanimation = base.transform.Find("ExpSlot/EffectAnimation").gameObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
		effectcpss = base.transform.Find("ExpSlot/EffectP").gameObject.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
		effectanimation.Hide();
		InitLv();
		UpdateExpDirect();
		RenderLv();
	}

	private void InitLv()
	{
		lv = Crazy_Data.CurData().GetLevel();
	}

	private void SetExpSlotX(float percent)
	{
		expcliprect.rect = new Rect(expcliprect.rect.x, expcliprect.rect.y, 177.5f * percent, expcliprect.rect.height);
		expcliprect.UpdateRect();
		expslot.UpdateMesh();
	}

	private void OnLvEffect()
	{
		effectanimation.Play();
		effectcpss.EmitParticle();
	}

	private void UpdateExpSlot(int exp, int expup)
	{
		float expSlotX = (float)exp / (float)expup;
		SetExpSlotX(expSlotX);
	}

	private void OnStopUp()
	{
		stopup = true;
		last_effect_time = 0f;
	}

	private void OffStopUp()
	{
		stopup = false;
		RenderLv();
		Crazy_SceneManager.GetInstance().GetScene().SendHintMessage("#LEVEL UP$");
	}

	private void UpdateStop()
	{
		if (stopup)
		{
			last_effect_time += Time.deltaTime;
			if (last_effect_time >= lv_effect_time)
			{
				OffStopUp();
			}
		}
	}

	private void UpdateLv()
	{
		int level = Crazy_Data.CurData().GetLevel();
		if (lv != level)
		{
			cur_exp = Crazy_PlayerClass_Level.GetPlayerLevelinfo(lv).exp;
			UpdateExp(cur_exp, cur_exp);
			lv = level;
			OnLvEffect();
			OnStopUp();
		}
	}

	private void RenderLv()
	{
		lvtext.text = "L " + lv;
		lvtext.UpdateMesh();
	}

	private void UpdateExpDirect()
	{
		Crazy_Player_Level playerLevelinfo = Crazy_PlayerClass_Level.GetPlayerLevelinfo(Crazy_Data.CurData().GetLevel());
		if (playerLevelinfo.exp < 0)
		{
			exptext.gameObject.active = false;
		}
		else if (cur_exp != Crazy_Data.CurData().GetExp())
		{
			cur_exp = Crazy_Data.CurData().GetExp();
			d_exp = cur_exp;
			UpdateExp(d_exp, playerLevelinfo.exp);
		}
	}

	private void UpdateExp(int exp, int expup)
	{
		if (cur_exp < exp)
		{
			cur_exp++;
		}
		else if (cur_exp > exp)
		{
			cur_exp = 0;
		}
		exptext.text = cur_exp + " / " + expup + " EXP";
		exptext.UpdateMesh();
		UpdateExpSlot(cur_exp, expup);
	}

	private void UpdateExp()
	{
		Crazy_Player_Level playerLevelinfo = Crazy_PlayerClass_Level.GetPlayerLevelinfo(Crazy_Data.CurData().GetLevel());
		if (playerLevelinfo.exp < 0)
		{
			exptext.gameObject.active = false;
			return;
		}
		d_exp = Crazy_Data.CurData().GetExp();
		UpdateExp(d_exp, playerLevelinfo.exp);
	}

	private void Update()
	{
		UpdateStop();
		UpdateLv();
		if (!stopup)
		{
			UpdateExpDirect();
		}
	}
}

using UnityEngine;

public class Crazy_HitRecover
{
	private Crazy_HitData crazy_hd;

	private float beatDirEff;

	private float beatSpeedEff;

	private float beatTimeEff;

	private float hitrecovertimeEff;

	private float hitrecoverArmor;

	public Crazy_FightedType getCrazy_FightedType()
	{
		return crazy_hd.crazy_fightedtype;
	}

	public float getBeatTime()
	{
		return crazy_hd.beatTime;
	}

	public float getHitRecoverTime()
	{
		return crazy_hd.hitrecovertime;
	}

	public Vector3 getBeatMove()
	{
		Vector3 vector = default(Vector3);
		return crazy_hd.beatDir * crazy_hd.beatSpeed * Time.deltaTime;
	}

	public Vector3 getBeatMoveDistance()
	{
		Vector3 vector = default(Vector3);
		return crazy_hd.beatDir * crazy_hd.beatSpeed * crazy_hd.beatTime;
	}

	public float getBeatSpeed()
	{
		return crazy_hd.beatSpeed;
	}

	public bool IsSuperArmor()
	{
		return hitrecovertimeEff == 0f;
	}

	public bool IsBreakArmor()
	{
		return crazy_hd.hitrecoverPower >= hitrecoverArmor;
	}

	public bool IsHitRecover()
	{
		return !IsSuperArmor() && IsBreakArmor();
	}

	public void InitializeEff(float beatdireff, float beatspeedeff, float beattimeeff, float hitrecovertimeeff, float hitrecoverarmor)
	{
		beatDirEff = beatdireff;
		beatSpeedEff = beatspeedeff;
		beatTimeEff = beattimeeff;
		hitrecovertimeEff = hitrecovertimeeff;
		hitrecoverArmor = hitrecoverarmor;
	}

	public void ApplyBeat(Crazy_FightedType btype, Vector3 bDir, float bspeed, float btime, float bhrtime)
	{
		crazy_hd.crazy_fightedtype = btype;
		crazy_hd.beatDir = bDir;
		crazy_hd.beatSpeed = bspeed;
		crazy_hd.beatTime = btime;
		crazy_hd.hitrecovertime = bhrtime;
	}

	public void ApplyBeat(Crazy_HitData chd)
	{
		crazy_hd = chd;
	}

	public void ApplyEff(Vector3 effDir, float effSpeed)
	{
		ApplyBeatDirEff(effDir);
		ApplyBeatSpeedEff(effSpeed);
		ApplyBeatTimeEff();
		ApplyHitRecoverTimeEff();
		DirCorrectionY();
	}

	private void ApplyBeatDirEff(Vector3 effDir)
	{
		crazy_hd.beatDir.Normalize();
		effDir.Normalize();
		crazy_hd.beatDir = crazy_hd.beatDir * (1f - beatDirEff) + effDir * beatDirEff;
		crazy_hd.beatDir.Normalize();
	}

	private void ApplyBeatSpeedEff(float effSpeed)
	{
		crazy_hd.beatSpeed += effSpeed * beatSpeedEff;
	}

	private void ApplyBeatTimeEff()
	{
		crazy_hd.beatTime *= beatTimeEff;
	}

	private void ApplyHitRecoverTimeEff()
	{
		crazy_hd.hitrecovertime *= hitrecovertimeEff;
	}

	private void DirCorrectionY()
	{
		crazy_hd.beatDir = new Vector3(crazy_hd.beatDir.x, 0f, crazy_hd.beatDir.z);
		crazy_hd.beatDir.Normalize();
	}
}

public class Crazy_AttackJudgmentInfo
{
	public bool usecallback;

	public float attackjudgmenttime;

	public AttackPoint attackpoint;

	public float attackrange;

	public float attackangle;

	public float attackdamage;

	public Crazy_HitData hitdata;

	public bool attackreset;

	public bool attackpause;

	public float attackpausetime;

	public bool attackshake;

	public float attackshaketime;

	public float attackshakeintervaltime;

	public float attackshakeamplitude;

	public Crazy_AttackJudgmentInfo()
	{
		usecallback = false;
		attackpausetime = 0.2f;
		attackshake = false;
		attackshaketime = 0.1f;
		attackshakeintervaltime = 0.02f;
		attackshakeamplitude = 1f;
	}
}

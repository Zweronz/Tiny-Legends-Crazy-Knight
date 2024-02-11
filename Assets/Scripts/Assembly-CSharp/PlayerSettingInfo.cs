public class PlayerSettingInfo
{
	public Crazy_PlayerClass cpc;

	public int weapon;

	public bool ingame;

	public bool inroom;

	public bool isprepare;

	public PlayerSettingInfo(Crazy_PlayerClass _cpc, int _weapon, bool _ingame, bool _inroom, bool _isprepare)
	{
		cpc = _cpc;
		weapon = _weapon;
		ingame = _ingame;
		inroom = _inroom;
		isprepare = _isprepare;
	}

	public PlayerSettingInfo()
	{
	}
}

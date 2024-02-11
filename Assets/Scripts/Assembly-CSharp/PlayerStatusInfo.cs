public class PlayerStatusInfo
{
	public Crazy_PlayerStatus status;

	public string attackname;

	public PlayerStatusInfo(Crazy_PlayerStatus _status, string _name)
	{
		status = _status;
		attackname = _name;
	}

	public PlayerStatusInfo()
	{
	}
}

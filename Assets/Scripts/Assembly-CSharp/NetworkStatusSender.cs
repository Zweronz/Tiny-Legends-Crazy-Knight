using UnityEngine;

public class NetworkStatusSender : MonoBehaviour
{
	private bool send;

	private Crazy_PlayerStatus lastStatus;

	private Crazy_PlayerStatus curStatus;

	private string attackStatus;

	private Crazy_PlayerControl player;

	private void Start()
	{
		player = base.gameObject.GetComponent<Crazy_PlayerControl>();
		curStatus = player.GetUpStatus();
		lastStatus = curStatus;
		attackStatus = player.GetAttackStatusName();
	}

	private void StartSendStatus()
	{
		send = true;
	}

	private void FixedUpdate()
	{
		if (send)
		{
			SendStatus();
		}
	}

	private void SendStatus()
	{
		curStatus = player.GetUpStatus();
		if (lastStatus != curStatus)
		{
			attackStatus = player.GetAttackStatusName();
			NetworkManager.Instance.SendStatus(new PlayerStatusInfo(curStatus, attackStatus));
			lastStatus = curStatus;
		}
		else if (curStatus == Crazy_PlayerStatus.Attack)
		{
			string attackStatusName = player.GetAttackStatusName();
			if (attackStatusName != attackStatus)
			{
				attackStatus = attackStatusName;
				NetworkManager.Instance.SendStatus(new PlayerStatusInfo(curStatus, attackStatus));
			}
		}
	}
}

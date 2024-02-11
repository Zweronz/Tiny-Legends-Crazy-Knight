using System;
using UnityEngine;

public class Crazy_EnemyControl_RemoteBoss_Net : Crazy_EnemyControl_MidBoss_Net
{
	protected float remote_maxattackrange = 15f;

	protected float remote_minattackrange = 8f;

	protected float remote_attackintervaltime = 5f;

	protected float remote_lastattacktime = 10f;

	protected bool remote_isoncd;

	protected bool remote_escape;

	public GameObject remote_item;

	protected int number_item = 1;

	protected override void InitTexture(int lv)
	{
	}

	public void SetRemoteCoolDown(float cd)
	{
		remote_attackintervaltime = cd;
	}

	public void SetRemoteCount(int count)
	{
		number_item = count;
	}

	protected virtual bool IsinMaxRemoteRange(GameObject tar)
	{
		return (tar.transform.position - base.transform.position).sqrMagnitude < (remote_maxattackrange + GetHitBox()) * (remote_maxattackrange + GetHitBox());
	}

	protected virtual bool IsinMinRemoteRange(GameObject tar)
	{
		return (tar.transform.position - base.transform.position).sqrMagnitude < (remote_minattackrange + GetHitBox()) * (remote_minattackrange + GetHitBox());
	}

	protected override void updateData(float deltatime)
	{
		if (controller)
		{
			curMoveDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
			curForwardDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
			if (IsinAlertRange(reftarget))
			{
				if (!isalert && lastalerttime >= alertrecovertime)
				{
					if (Crazy_EnemyManagement.AddActiveNumber())
					{
						isalert = true;
						lastalerttime = 0f;
					}
				}
				else if (isalert && lastalerttime >= alerttime && curStatus == Crazy_MonsterStatus.Run && Crazy_EnemyManagement.RemoveActiveNumber())
				{
					isalert = false;
					lastalerttime = 0f;
				}
			}
			else if (isalert && Crazy_EnemyManagement.RemoveActiveNumber())
			{
				isalert = false;
				lastalerttime = 0f;
			}
			if (curStatus == Crazy_MonsterStatus.Die || curStatus == Crazy_MonsterStatus.Remote || curStatus == Crazy_MonsterStatus.Attack || curStatus == Crazy_MonsterStatus.PreAttack || curStatus == Crazy_MonsterStatus.EndAttack)
			{
				return;
			}
			bool flag = IsinMaxRemoteRange(reftarget);
			bool flag2 = IsinMinRemoteRange(reftarget);
			bool flag3 = IsinAttackRange(reftarget);
			if (flag && !flag2)
			{
				if (!remote_isoncd)
				{
					switchMonsterStatus(Crazy_MonsterStatus.Remote);
				}
				else if (!remote_escape)
				{
					switchMonsterStatus(Crazy_MonsterStatus.Idle);
				}
				else if (remote_escape)
				{
					Vector2 vector = Crazy_Global.Rotate(new Vector2(0f - curMoveDir.x, 0f - curMoveDir.z), UnityEngine.Random.Range(-30, 30));
					curMoveDir = new Vector3(vector.x, 0f, vector.y);
				}
			}
			else if (flag2 && !flag3)
			{
				Vector2 vector2 = Crazy_Global.Rotate(new Vector2(0f - curMoveDir.x, 0f - curMoveDir.z), UnityEngine.Random.Range(-30, 30));
				curMoveDir = new Vector3(vector2.x, 0f, vector2.y);
				switchMonsterStatus(Crazy_MonsterStatus.Run);
				remote_escape = true;
			}
			else if (flag3)
			{
				switchMonsterStatus(Crazy_MonsterStatus.PreAttack);
				remote_escape = false;
			}
			else if (isalert)
			{
				switchMonsterStatus(Crazy_MonsterStatus.Run);
				remote_escape = false;
			}
			else
			{
				if (isalert)
				{
					return;
				}
				remote_escape = false;
				if (curStatus == Crazy_MonsterStatus.Move || curStatus == Crazy_MonsterStatus.Run)
				{
					float num = UnityEngine.Random.Range(0f, 1f);
					if (num >= 0.95f)
					{
						switchMonsterStatus(Crazy_MonsterStatus.Idle);
					}
				}
				else if (curStatus == Crazy_MonsterStatus.Idle)
				{
					float num2 = UnityEngine.Random.Range(0f, 1f);
					if (num2 >= 0.8f)
					{
						outalertmove = true;
						switchMonsterStatus(Crazy_MonsterStatus.Move);
					}
				}
			}
		}
		else
		{
			updateRemoteCooldown(deltatime);
		}
	}

	protected override void updateTurnRound()
	{
		if (curStatus != Crazy_MonsterStatus.PreAttack && curStatus != Crazy_MonsterStatus.Attack)
		{
			base.transform.forward = curMoveDir;
		}
	}

	protected override void switchMonsterStatus(Crazy_MonsterStatus toStatus)
	{
		if (controller && (toStatus == Crazy_MonsterStatus.Idle || toStatus == Crazy_MonsterStatus.HitRecover || curStatus <= toStatus) && curStatus != toStatus)
		{
			ResetPreAttack(0f);
			switch (toStatus)
			{
			case Crazy_MonsterStatus.PreAttack:
				updateTurnRound();
				break;
			case Crazy_MonsterStatus.Attack:
				OnAttack();
				break;
			case Crazy_MonsterStatus.Remote:
				OnRemote();
				break;
			case Crazy_MonsterStatus.Die:
				Die();
				break;
			}
			PlayAnimation(toStatus);
			curStatus = toStatus;
			OnMonsterStatusSender(new MonsterStatusInfo(networkid.ToString(), curStatus));
		}
	}

	protected void OffRemote()
	{
		switchMonsterStatus(Crazy_MonsterStatus.Idle);
	}

	protected void OnRemote()
	{
		remote_lastattacktime = remote_attackintervaltime;
		attackForward = curForwardDir;
		remote_isoncd = true;
		updateTurnRound();
	}

	protected void Remote()
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(-0.37f, 0.89f, 1.59f);
		for (int i = 0; i < number_item; i++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(remote_item, gameObject.transform.position, base.transform.rotation) as GameObject;
			gameObject2.transform.parent = base.transform.parent;
			gameObject2.SendMessage("SetTarget", target, SendMessageOptions.DontRequireReceiver);
			Vector3 vector = ((i != 0) ? new Vector3(UnityEngine.Random.Range(-7f, 7f), 0f, UnityEngine.Random.Range(-7f, 7f)) : Vector3.zero);
			gameObject2.SendMessage("OnFly", reftarget.transform.position + vector, SendMessageOptions.DontRequireReceiver);
			gameObject2.SendMessage("EmitParticle", SendMessageOptions.DontRequireReceiver);
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	protected override void Update()
	{
		try
		{
			if (controller)
			{
				updateinvincible();
				updateBloodSlot();
				if (Crazy_GlobalData.cur_game_state == Crazy_GameState.Game)
				{
					activetime += Time.deltaTime;
					if (is_pause)
					{
						last_pause_time += Time.deltaTime;
						if (last_pause_time >= pause_time)
						{
							OffPause();
						}
						return;
					}
					_interval_update += Time.deltaTime;
					if (_interval_update >= _Interval_Update)
					{
						updateData(_interval_update);
						_interval_update = 0f;
					}
					switch (curStatus)
					{
					case Crazy_MonsterStatus.Die:
						updateDie(Time.deltaTime);
						break;
					case Crazy_MonsterStatus.Hurt:
						updateHurt(Time.deltaTime);
						break;
					case Crazy_MonsterStatus.HitRecover:
						updateHitRecover(Time.deltaTime);
						break;
					case Crazy_MonsterStatus.Move:
					case Crazy_MonsterStatus.Run:
						updateMove(Time.deltaTime);
						updateTurnRound();
						break;
					case Crazy_MonsterStatus.PreAttack:
					case Crazy_MonsterStatus.Attack:
						updateAttack(Time.deltaTime);
						updateMove(Time.deltaTime);
						break;
					case Crazy_MonsterStatus.EndAttack:
						updateEndAttack(Time.deltaTime);
						break;
					}
					relive();
					reaction(Time.deltaTime);
					updateAlert();
					updateRemoteCooldown(Time.deltaTime);
				}
				else
				{
					base.GetComponent<Animation>().Stop();
				}
				updateDrop();
				return;
			}
			updateDrop();
			updateinvincible();
			updateBloodSlot();
			if (Crazy_GlobalData.cur_game_state == Crazy_GameState.Game)
			{
				activetime += Time.deltaTime;
				if (is_pause)
				{
					last_pause_time += Time.deltaTime;
					if (last_pause_time >= pause_time)
					{
						OffPause();
					}
					return;
				}
				_interval_update += Time.deltaTime;
				if (_interval_update >= _Interval_Update)
				{
					updateData(_interval_update);
					_interval_update = 0f;
				}
				switch (curStatus)
				{
				case Crazy_MonsterStatus.Die:
					updateDie(Time.deltaTime);
					break;
				case Crazy_MonsterStatus.Hurt:
					updateHurt(Time.deltaTime);
					break;
				case Crazy_MonsterStatus.HitRecover:
					updateHitRecover(Time.deltaTime);
					break;
				case Crazy_MonsterStatus.PreAttack:
				case Crazy_MonsterStatus.Attack:
					updateAttack(Time.deltaTime);
					break;
				case Crazy_MonsterStatus.EndAttack:
					updateEndAttack(Time.deltaTime);
					break;
				}
				relive();
				reaction(Time.deltaTime);
				updateAlert();
			}
			else
			{
				base.GetComponent<Animation>().Stop();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void updateRemoteCooldown(float time)
	{
		if (remote_isoncd)
		{
			remote_lastattacktime -= time;
			if (remote_lastattacktime <= 0f)
			{
				remote_isoncd = false;
			}
		}
	}

	protected override void reaction(float time)
	{
		if (curStatus == Crazy_MonsterStatus.Attack || curStatus == Crazy_MonsterStatus.Remote)
		{
			lastreactiontime += time;
			if (lastreactiontime >= reactiontime)
			{
				if (curStatus == Crazy_MonsterStatus.Attack)
				{
					OffAttack();
				}
				else if (curStatus == Crazy_MonsterStatus.Remote)
				{
					OffRemote();
				}
			}
		}
		else
		{
			lastreactiontime = 0f;
		}
	}
}

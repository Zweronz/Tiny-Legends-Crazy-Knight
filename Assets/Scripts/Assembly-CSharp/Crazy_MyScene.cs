using Assets.Scripts.Assembly_CSharp.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Crazy_MyScene : Photon.PunBehaviour
{
	protected Vector3 playerposition = new Vector3(0f, 0.1f, 6f);

	protected float timeToUnload;

	protected Crazy_PlayerControl playerCom;

	protected Camera mainCameraCom;

	private Crazy_CameraCom mainCameraMoveCom;

	private List<Crazy_Weapon> weaponlist;

	private int cur_weaponId;

	private Queue<int> messagequeue = new Queue<int>();

	private bool handlemessage;

	private Queue<Crazy_HintMessage> hintqueue = new Queue<Crazy_HintMessage>();

	public bool playereffect = true;

	public bool monstereffect = true;

	public bool monsterhint;

	protected UtilUIPause UIPause;

	private UtilUIBeginnerHintControl UIBeginnerHint;

	protected bool isDeath;

	protected float lastDeathTime;

	protected bool isComplete;

	protected float lastCompleteTime;

	protected GameObject door;

	protected float total_fight_time;

	protected GameObject countZero;

	protected GameObject waitTotal;

	public Crazy_PlayerControl GetPlayerControl()
	{
		return playerCom;
	}

	public virtual void Init(GameObject go1, GameObject go2)
	{
		countZero = go1.gameObject;
		waitTotal = go2.gameObject;
	}

	public virtual void Awake(UtilUIPause uipause, UtilUIBeginnerHintControl uibeginnerhint)
	{
		weaponlist = Crazy_Weapon.ReadWeaponInfo();
		cur_weaponId = Crazy_Data.CurData().GetWeaponId();
		Crazy_Weapon crazy_Weapon = Crazy_Weapon.FindWeaponByID(weaponlist, cur_weaponId);
		GameObject gameObject = null;
		switch (Crazy_Data.CurData().GetPlayerClass())
		{
		case Crazy_PlayerClass.Fighter:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/fighter/fighter_pfb");
			break;
		case Crazy_PlayerClass.Knight:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/knight/knight_pfb");
			break;
		case Crazy_PlayerClass.Warrior:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/warrior/warrior_pfb");
			break;
		case Crazy_PlayerClass.Rogue:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/rogue/rogue_pfb");
			break;
		case Crazy_PlayerClass.Paladin:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/paladin/paladin_pfb");
			break;
		case Crazy_PlayerClass.Mage:
			gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/mage/mage_pfb");
			break;
		default:
			Debug.LogError("HeroError");
			break;
		}
		gameObject.name = "Player";
		gameObject.transform.parent = GameObject.Find("Scene").transform;
		gameObject.transform.position = playerposition;
		gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		gameObject.layer = LayerMask.NameToLayer("Player");
		playerCom = AddPlayerControl(gameObject);
		playerCom.SetEffect(playereffect);
		PropsAction.Init();
		PropsAction.playerControl = playerCom;
		GameObject gameObject2 = GameUtils.InstantiateAsGameObject<GameObject>(Resources.Load(crazy_Weapon.loadpath));
		gameObject2.name = "Weapon";
		GameObject person = GameObject.Find("Player");
		gameObject2.transform.parent = FindWeaponBone(crazy_Weapon.type, person).transform;
		gameObject2.transform.localPosition = crazy_Weapon.modifyPos;
		gameObject2.transform.localEulerAngles = crazy_Weapon.modifyAngle;
		gameObject2.layer = LayerMask.NameToLayer("Player");
		playerCom.SetCurWeapon(crazy_Weapon, gameObject2);
		Crazy_EffectManagement crazy_EffectManagement = gameObject.AddComponent<Crazy_EffectManagement>();
		crazy_EffectManagement.InitEffect(4, 5, 8, playerCom.GetRootNode());
		GameObject gameObject3 = GameObject.Find("MainCameraParentNode/Main Camera");
		mainCameraMoveCom = gameObject3.GetComponent("Crazy_CameraCom") as Crazy_CameraCom;
		mainCameraCom = gameObject3.GetComponent("Camera") as Camera;
		mainCameraCom.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Slurry")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Items")) | (1 << LayerMask.NameToLayer("Shadow")) | (1 << LayerMask.NameToLayer("Enemy"));
		mainCameraCom.GetComponent<Animation>()["camera1"].layer = 2;
		mainCameraCom.GetComponent<Animation>()["camera2"].layer = 1;
		UIPause = uipause;
		UIBeginnerHint = uibeginnerhint;
		Crazy_Global.PlayBGM("BGM_Battle0" + Random.Range(1, 3));
		Crazy_Statistics.ResetMonsterKillNumber();
		Crazy_GlobalData.cur_UI4Times++;
		Crazy_GlobalData.cur_ui = 4;
		total_fight_time = 0f;
	}

	protected virtual Crazy_PlayerControl AddPlayerControl(GameObject player)
	{
		if (Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage)
		{
			return player.AddComponent<Crazy_PlayerControlMage>() as Crazy_PlayerControl;
		}
		return player.AddComponent<Crazy_PlayerControl>() as Crazy_PlayerControl;
	}

	private GameObject FindWeaponBone(Crazy_Weapon_Type cwt, GameObject person)
	{
		switch (cwt)
		{
		case Crazy_Weapon_Type.Sword:
		case Crazy_Weapon_Type.Hammer:
		case Crazy_Weapon_Type.Axe:
		case Crazy_Weapon_Type.Staff:
			return person.transform.Find("Bone/Pelvis/Spine/Right_Shoulder/Right_Hand/Weapon").gameObject;
		case Crazy_Weapon_Type.Bow:
			return person.transform.Find("Bone/Pelvis/Spine/Left_Shoulder/Left_Hand/L_Weapon").gameObject;
		default:
			return null;
		}
	}

	public void Save()
	{
		Crazy_Data.SaveData();
	}

	public void SendHintMessage(string text)
	{
		Crazy_HintMessage crazy_HintMessage = new Crazy_HintMessage();
		crazy_HintMessage.text = text;
		hintqueue.Enqueue(crazy_HintMessage);
	}

	public void SendHintMessage(Crazy_HintMessage mess)
	{
		hintqueue.Enqueue(mess);
	}

	public Crazy_HintMessage GetHintMessage()
	{
		if (hintqueue.Count != 0)
		{
			return hintqueue.Dequeue();
		}
		return null;
	}

	public Vector3 GetPlayerPositon()
	{
		return playerCom.transform.position;
	}

	private void EquipWeapon()
	{
		cur_weaponId++;
		Crazy_Weapon crazy_Weapon = Crazy_Weapon.FindWeaponByID(weaponlist, cur_weaponId);
		if (crazy_Weapon == null)
		{
			cur_weaponId = 0;
			crazy_Weapon = Crazy_Weapon.FindWeaponByID(weaponlist, cur_weaponId);
		}
		GameObject gameObject = GameObject.Find("Player/Bone/Pelvis/Spine/Right_Shoulder/Right_Hand/Weapon/Weapon");
		GameObject gameObject2 = GameObject.Find("Player/Bone/Pelvis/Spine/Left_Shoulder/Left_Hand/L_Weapon/Weapon");
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
		}
		if (gameObject2 != null)
		{
			Object.Destroy(gameObject2);
		}
		playerCom.DeleteCurWeapon();
		GameObject gameObject3 = GameUtils.InstantiateAsGameObject<GameObject>(Resources.Load(crazy_Weapon.loadpath));
		gameObject3.name = "Weapon";
		GameObject person = GameObject.Find("Player");
		gameObject3.transform.parent = FindWeaponBone(crazy_Weapon.type, person).transform;
		gameObject3.transform.localPosition = crazy_Weapon.modifyPos;
		gameObject3.transform.localEulerAngles = crazy_Weapon.modifyAngle;
		gameObject3.layer = LayerMask.NameToLayer("Player");
		playerCom.SetCurWeapon(crazy_Weapon, gameObject3);
	}

	protected virtual void OnPause()
	{
		Time.timeScale = 0f;
		UIPause.transform.localPosition = new Vector3(0f, 0f, UIPause.transform.localPosition.z);
		OpenClickNew.Show(false);
	}

	protected virtual void OffPause()
	{
		Time.timeScale = 1f;
		UIPause.transform.localPosition = new Vector3(1000f, 1000f, UIPause.transform.localPosition.z);
		OpenClickNew.Hide();
	}

	protected virtual void ToReward()
	{
		Time.timeScale = 1f;
		TUIFade component = GameObject.Find("TUI/TUIControl/Fade").GetComponent<TUIFade>();
		OpenClickNew.Hide();
		FlurryPlugin.logEvent("Victory");
		FlurryPlugin.endTimedEvent("Fight");
		component.FadeOut("CrazyReward");
	}

	protected virtual void BackToMenu()
	{
		Time.timeScale = 1f;
		TUIFade component = GameObject.Find("TUI/TUIControl/Fade").GetComponent<TUIFade>();
		OpenClickNew.Hide();
		Crazy_GameCenter.instance.UpdateGameCenterAchievement();
		Crazy_GameCenter.instance.UpdateGameCenterLeaderBoard(Crazy_Data.CurData().GetKillNumber());
		FlurryPlugin.logEvent("Defeat");
		FlurryPlugin.endTimedEvent("Fight");
		component.FadeOut("CrazyMap");
	}

	public virtual void Update()
	{
		total_fight_time += Time.deltaTime;
		updateCamera();
		timeToUnload += Time.deltaTime;
		if (timeToUnload > 30f)
		{
			timeToUnload = 0f;
			Resources.UnloadUnusedAssets();
		}
		if (isDeath)
		{
			lastDeathTime -= Time.deltaTime;
			if (lastDeathTime <= 0f)
			{
				InnerDeath();
				lastDeathTime = 10000f;
			}
		}
		else if (isComplete)
		{
			lastCompleteTime -= Time.deltaTime;
			if (lastCompleteTime <= 0f)
			{
				InnerComplete();
				lastCompleteTime = 10000f;
			}
		}
		if (playerCom.IsDie())
		{
			OnDeath();
		}
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			UpdateHandleMessage();
		}
	}

	protected virtual void updateCamera()
	{
		if (door == null)
		{
			mainCameraCom.transform.parent.position = Vector3.Lerp(mainCameraCom.transform.parent.position, GetPlayerPositon(), 0.1f);
		}
		else
		{
			mainCameraCom.transform.parent.position = Vector3.Lerp(mainCameraCom.transform.parent.position, door.transform.position, 0.1f);
		}
	}

	public virtual bool IsGameBegin()
	{
		return false;
	}

	public virtual void UpdateGameBegin()
	{
	}

	public void OnGameBegin()
	{
		Crazy_GlobalData.cur_game_state = Crazy_GameState.Game;
		mainCameraCom.GetComponent<Animation>().Play("camera2");
	}

	public void OnDeath()
	{
		Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task08, 0, 0f);
		Crazy_GlobalData.cur_game_state = Crazy_GameState.EndGame;
		if (!isComplete && !isDeath)
		{
			if (!playerCom.IsDie())
			{
				playerCom.OnDeject();
			}
			TUIMeshSprite component = GameObject.FindGameObjectWithTag("LogoTag").GetComponent<TUIMeshSprite>();
			if (playerCom.IsDeject())
			{
				component.frameName = "TimeUpLogo";
			}
			else
			{
				component.frameName = "FailedLogo";
			}
			component.Static = false;
			component.GetComponent<Animation>().Play("FailedLogo");
			Crazy_Global.PlayBGM("BGM_Lose");
			isDeath = true;
			lastDeathTime = 3f;
		}
	}

	public void OnComplete()
	{
		if (playerCom.UseWeaponType() != Crazy_Weapon_Type.Staff)
		{
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task26, (int)playerCom.UseWeaponType(), 0f);
		}
		Crazy_GlobalData.cur_game_state = Crazy_GameState.EndGame;
		if (!isDeath && !isComplete)
		{
			playerCom.OnCelebrate();
			TUIMeshSprite component = GameObject.FindGameObjectWithTag("LogoTag").GetComponent<TUIMeshSprite>();
			component.frameName = "WinLogo";
			component.Static = false;
			component.GetComponent<Animation>().Play("WinLogo");
			Crazy_Global.PlayBGM("BGM_Win");
			isComplete = true;
			lastCompleteTime = 3f;
			if (Crazy_GlobalData.cur_leveltype == Crazy_LevelType.Normal3)
			{
				OnDoorOpen();
				lastCompleteTime = 5f;
			}
			else if (Crazy_GlobalData.cur_leveltype == Crazy_LevelType.Boss && !playerCom.IsHurtted)
			{
				Crazy_TaskManager.GetInstance().updateTask((Crazy_TaskId)(Crazy_GlobalData.cur_level / 5 + 10), 0, 0f);
			}
		}
	}

	private void OnDoorOpen()
	{
		door = Crazy_Global.LoadAssetsPrefab("Prefabs/door/door_pfb");
		door.name = "Door";
		door.transform.parent = GameObject.Find("Scene").transform;
		door.transform.position = new Vector3(0f, 0f, 0f);
	}

	protected void InnerDeath()
	{
		InitRewardDeath();
		OnGameEnd();
	}

	private void InitRewardDeath()
	{
		Crazy_Statistics.isfinish = false;
		Crazy_Statistics.mkillnumber = Crazy_GlobalData.cur_kill_number;
		Crazy_Statistics.mgold = 0;
		Crazy_Statistics.ccombo = GetPlayerControl().GetMaxCombo();
		int num = Crazy_ComboLevel.FindComboLevel(Crazy_Statistics.ccombo);
		Crazy_Modify modify = Crazy_LevelModify.GetModify(Crazy_GlobalData.cur_leveltype, Crazy_GlobalData.cur_level);
		if (modify != null)
		{
			float num2 = Mathf.Clamp((float)(num - modify.combocount) * 0.05f + 0.25f, 0f, 1f);
			Crazy_Statistics.cgold = 0;
		}
	}

	protected void InnerComplete()
	{
		InitRewardComplete();
		OnGameEnd();
	}

	private void InitRewardComplete()
	{
		Crazy_Statistics.isfinish = true;
		Crazy_Statistics.mkillnumber = Crazy_GlobalData.cur_kill_number;
		Crazy_Statistics.mgold = GetPlayerControl().GetGold();
		Crazy_Statistics.ccombo = GetPlayerControl().GetMaxCombo();
		int num = Crazy_ComboLevel.FindComboLevel(Crazy_Statistics.ccombo);
		Crazy_Modify modify = Crazy_LevelModify.GetModify(Crazy_GlobalData.cur_leveltype, Crazy_GlobalData.cur_level);
		if (modify != null)
		{
			float num2 = Mathf.Clamp((float)(num - modify.combocount) * 0.05f + 0.25f, 0f, 1f);
			Crazy_Statistics.cgold = (int)((float)Crazy_Statistics.mgold * num2);
		}
		Crazy_GlobalData.cur_fight_succ++;
	}

	public virtual void OnGameEnd()
	{
		Crazy_GlobalData.cur_fight_num++;
		if (Crazy_GlobalData.cur_fight_shorttime != -1)
		{
			Crazy_GlobalData.cur_fight_shorttime = Mathf.Min(Crazy_GlobalData.cur_fight_shorttime, (int)total_fight_time);
		}
		else
		{
			Crazy_GlobalData.cur_fight_shorttime = (int)total_fight_time;
		}
		Crazy_GlobalData.cur_fight_longtime = Mathf.Max(Crazy_GlobalData.cur_fight_longtime, (int)total_fight_time);
		Crazy_GlobalData.cur_fight_avgtime = (Crazy_GlobalData.cur_fight_avgtime * (Crazy_GlobalData.cur_fight_num - 1) + (int)total_fight_time) / Crazy_GlobalData.cur_fight_num;
		ToReward();
	}

	public virtual void OnReviveDown()
	{
	}

	public void OnRollDown()
	{
		UIBeginnerHint.SendMessage("RollDown", SendMessageOptions.DontRequireReceiver);
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			playerCom.PlayerRoll();
		}
	}

	public void OnAttackDown()
	{
		UIBeginnerHint.SendMessage("AttackDown", SendMessageOptions.DontRequireReceiver);
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			AddQueue(1);
		}
	}

	public void OnSkillDown()
	{
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			AddQueue(2);
		}
	}

	public void OnBackDown()
	{
		Crazy_Data.CurData().AddKillNumber(Crazy_GlobalData.cur_kill_number);
		Crazy_GameCenter.instance.UpdateGameCenterData();
		Crazy_Data.SaveData();
		Crazy_GlobalData.cur_fight_num++;
		if (Crazy_GlobalData.cur_fight_shorttime != -1)
		{
			Crazy_GlobalData.cur_fight_shorttime = Mathf.Min(Crazy_GlobalData.cur_fight_shorttime, (int)total_fight_time);
		}
		else
		{
			Crazy_GlobalData.cur_fight_shorttime = (int)total_fight_time;
		}
		Crazy_GlobalData.cur_fight_longtime = Mathf.Max(Crazy_GlobalData.cur_fight_longtime, (int)total_fight_time);
		Crazy_GlobalData.cur_fight_avgtime = (Crazy_GlobalData.cur_fight_avgtime * (Crazy_GlobalData.cur_fight_num - 1) + (int)total_fight_time) / Crazy_GlobalData.cur_fight_num;
		BackToMenu();
	}

	public void OnContinueDown()
	{
		OffPause();
	}

	public virtual void OnPauseDown()
	{
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			OnPause();
		}
	}

	public void OnMove(Vector2 dir)
	{
		UIBeginnerHint.SendMessage("MoveDown", SendMessageOptions.DontRequireReceiver);
		if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) < 0.01f)
		{
			playerCom.StartContinousMove(Vector2.zero);
		}
		else
		{
			playerCom.StartContinousMove(dir);
		}
	}

	public void OnShotDown()
	{
		UIBeginnerHint.SendMessage("BowDown", SendMessageOptions.DontRequireReceiver);
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			playerCom.OnShot();
		}
	}

	public void OnShotUp()
	{
		if (!playerCom.IsDie() && !playerCom.IsCelebrate() && !playerCom.IsDeject())
		{
			playerCom.OffShot();
		}
	}

	public void OnForward(Vector2 dir)
	{
		if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) < 0.01f)
		{
			playerCom.OnFaceTo(Vector2.zero);
		}
		else
		{
			playerCom.OnFaceTo(dir);
		}
	}

	public void OnInvincible()
	{
		playerCom.PlayInvincible();
	}

	public void OnSpeedUp()
	{
		playerCom.PlaySpeedUp();
	}

	public void OnHealUp()
	{
		playerCom.PlayHealUp();
	}

	public void OnEquipWeapon()
	{
		EquipWeapon();
	}

	public void OnPlayerEffectOnOff()
	{
		playereffect = !playereffect;
		playerCom.SetEffect(playereffect);
	}

	public void OnMonsterEffectOnOff()
	{
		monstereffect = !monstereffect;
		if (Crazy_GlobalData.enemyList == null)
		{
			return;
		}
		Dictionary<int, GameObject>.KeyCollection keys = Crazy_GlobalData.enemyList.Keys;
		foreach (int item in keys)
		{
			GameObject value;
			if (Crazy_GlobalData.enemyList.TryGetValue(item, out value))
			{
				Crazy_EnemyControl crazy_EnemyControl = value.GetComponent("Crazy_EnemyControl") as Crazy_EnemyControl;
				crazy_EnemyControl.SetEffect(monstereffect);
			}
		}
	}

	public void OnMonsterHintOnOff()
	{
		monsterhint = !monsterhint;
		if (Crazy_GlobalData.enemyList == null)
		{
			return;
		}
		Dictionary<int, GameObject>.KeyCollection keys = Crazy_GlobalData.enemyList.Keys;
		foreach (int item in keys)
		{
			GameObject value;
			if (Crazy_GlobalData.enemyList.TryGetValue(item, out value))
			{
				Crazy_EnemyControl crazy_EnemyControl = value.GetComponent("Crazy_EnemyControl") as Crazy_EnemyControl;
				crazy_EnemyControl.SetHint(monsterhint);
			}
		}
	}

	private void AddQueue(int id)
	{
		if (id == 2)
		{
			messagequeue.Clear();
			messagequeue.Enqueue(id);
		}
		else if (messagequeue.ToArray().GetLength(0) < 2)
		{
			messagequeue.Enqueue(id);
		}
	}

	private void MessageQueue()
	{
		if (messagequeue.ToArray().GetLength(0) == 0)
		{
			return;
		}
		switch (messagequeue.Peek())
		{
		case 1:
			if (!playerCom.GetAttackRecover())
			{
				playerCom.PlayerAttack();
				messagequeue.Dequeue();
			}
			break;
		case 2:
			playerCom.PlayerSkill();
			messagequeue.Dequeue();
			break;
		}
	}

	protected void UpdateHandleMessage()
	{
		MessageQueue();
	}
}

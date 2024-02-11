using UnityEngine;

public class UICrazyScene : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	protected Crazy_MyScene m_imp;

	protected UtilUIPause UIPause;

	protected UtilUIBeginnerHintControl UIBeginnerHint;

	protected TUIContainer UIBattle;

	protected UtilUIReviveControl UIRevive;

	private bool isMovie = true;

	protected GameObject countZero;

	protected GameObject waitTotal;

	public virtual void OnDestroy()
	{
		m_imp = null;
		Crazy_SceneManager.GetInstance().Uninitialized();
	}

	public void Awake()
	{
		GameObject gameObject = GameObject.Find("Scene");
		GameObject gameObject2 = null;
		gameObject2 = Object.Instantiate(Resources.Load("UI/CrazyScene/SceneTUIForIPhone5")) as GameObject;
		gameObject2.name = "SceneTUI";
		gameObject2.transform.parent = gameObject.transform;
		UIPause = gameObject2.transform.Find("TUI/TUIControl/Pause").GetComponent<UtilUIPause>();
		UIBeginnerHint = gameObject2.transform.Find("TUI/TUIControl/BeginnerHint").GetComponent<UtilUIBeginnerHintControl>();
		UIBattle = gameObject2.transform.Find("TUI/TUIControl/Battle").GetComponent<TUIContainer>();
		UIRevive = gameObject2.transform.Find("TUI/TUIControl/ReviveControl").GetComponent<UtilUIReviveControl>();
		countZero = gameObject2.transform.Find("TUI/TUIControl/CountZero").gameObject;
		waitTotal = gameObject2.transform.Find("TUI/TUIControl/Mask").gameObject;
		countZero.gameObject.SetActiveRecursively(false);
		waitTotal.gameObject.SetActiveRecursively(false);
		ImpAwake();
	}

	protected virtual void ImpAwake()
	{
		m_imp = new Crazy_MyScene();
		Crazy_SceneManager.GetInstance().Initialized(m_imp);
		m_imp.Awake(UIPause, UIBeginnerHint);
	}

	public void Start()
	{
		m_tui = TUI.Instance("Scene/SceneTUI/TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		Movie();
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			m_imp.Save();
		}
		else if (!isMovie)
		{
			m_imp.OnPauseDown();
		}
	}

	public void Movie()
	{
		isMovie = true;
		m_tui.transform.Find("TUICamera").gameObject.active = false;
		Invoke("EndMovie", 3f);
	}

	public void EndMovie()
	{
		m_tui.transform.Find("TUICamera").gameObject.active = true;
		isMovie = false;
		m_imp.OnGameBegin();
	}

	public void ResetButton()
	{
		TUIControlImpl[] componentsInChildren = UIBattle.GetComponentsInChildren<TUIControlImpl>();
		TUIControlImpl[] array = componentsInChildren;
		foreach (TUIControlImpl tUIControlImpl in array)
		{
			tUIControlImpl.gameObject.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		if (!isMovie)
		{
			for (int i = 0; i < input.Length; i++)
			{
				m_tui.HandleInput(input[i]);
			}
		}
		m_imp.Update();
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "FightButton" && eventType == 3)
		{
			m_imp.OnAttackDown();
		}
		else if (control.name == "SkillButton" && eventType == 3)
		{
			m_imp.OnSkillDown();
		}
		else if (control.name == "PauseButton" && eventType == 3)
		{
			m_imp.OnPauseDown();
		}
		else if (control.name == "MoveButton")
		{
			Vector2 dir = new Vector2(0f - wparam, 0f - lparam);
			m_imp.OnMove(dir);
		}
		else if (control.name == "BackButton" && eventType == 3)
		{
			m_imp.OnBackDown();
		}
		else if (control.name == "ContinueButton" && eventType == 3)
		{
			m_imp.OnContinueDown();
			ResetButton();
		}
		else if (control.name == "RollButton" && eventType == 3)
		{
			m_imp.OnRollDown();
		}
		else if (control.name == "ShotButton")
		{
			switch (eventType)
			{
			case 1:
				m_imp.OnShotDown();
				break;
			case 3:
				m_imp.OnShotUp();
				break;
			}
			Vector2 dir2 = new Vector2(0f - wparam, 0f - lparam);
			m_imp.OnForward(dir2);
		}
		else if (control.name == "ReviveButton" && eventType == 3)
		{
			m_imp.OnReviveDown();
		}
	}

	public void OnGameEnd()
	{
		m_imp.OnComplete();
	}

	public void OnGameFailed()
	{
		m_imp.OnDeath();
	}
}

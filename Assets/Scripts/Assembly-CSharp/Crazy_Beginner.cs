using UnityEngine;

public class Crazy_Beginner
{
	private static Crazy_Beginner s_instance;

	private bool m_isBeginner = true;

	private bool m_isSkill = true;

	private bool m_isMap = true;

	private bool m_isStory = true;

	private bool m_isBow = true;

	private bool m_isRoll = true;

	private bool m_isCoop = true;

	private bool m_isProp = true;

	private bool m_isCheckJailbreak = true;

	public static Crazy_Beginner instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new Crazy_Beginner();
			}
			return s_instance;
		}
	}

	public bool isBeginner
	{
		get
		{
			return m_isBeginner;
		}
		set
		{
			m_isBeginner = value;
			PlayerPrefs.SetInt("BeginnerStage", (!m_isBeginner) ? 1 : 0);
		}
	}

	public bool isSkill
	{
		get
		{
			return m_isSkill;
		}
		set
		{
			m_isSkill = value;
			PlayerPrefs.SetInt("BeginnerSkill", (!m_isSkill) ? 1 : 0);
		}
	}

	public bool isMap
	{
		get
		{
			return m_isMap;
		}
		set
		{
			m_isMap = value;
			PlayerPrefs.SetInt("BeginnerMap", (!m_isMap) ? 1 : 0);
		}
	}

	public bool isStory
	{
		get
		{
			return m_isStory;
		}
		set
		{
			m_isStory = value;
			PlayerPrefs.SetInt("BeginnerStory", (!m_isStory) ? 1 : 0);
		}
	}

	public bool isBow
	{
		get
		{
			return m_isBow;
		}
		set
		{
			m_isBow = value;
			PlayerPrefs.SetInt("BeginnerBow", (!m_isBow) ? 1 : 0);
		}
	}

	public bool isRoll
	{
		get
		{
			return m_isRoll;
		}
		set
		{
			m_isRoll = value;
			PlayerPrefs.SetInt("BeginnerRoll", (!m_isRoll) ? 1 : 0);
		}
	}

	public bool isCoop
	{
		get
		{
			return m_isCoop;
		}
		set
		{
			m_isCoop = value;
			PlayerPrefs.SetInt("BeginnerCoop", (!m_isCoop) ? 1 : 0);
		}
	}

	public bool isProp
	{
		get
		{
			return m_isProp;
		}
		set
		{
			m_isProp = value;
			PlayerPrefs.SetInt("BeginnerProp", (!m_isProp) ? 1 : 0);
		}
	}

	public bool isCheckJailbreak
	{
		get
		{
			return m_isCheckJailbreak;
		}
		set
		{
			m_isCheckJailbreak = value;
			PlayerPrefs.SetInt("CheckJailbreak", (!m_isCheckJailbreak) ? 1 : 0);
		}
	}

	public Crazy_Beginner()
	{
		m_isBeginner = PlayerPrefs.GetInt("BeginnerStage") == 0;
		m_isSkill = PlayerPrefs.GetInt("BeginnerSkill") == 0;
		m_isMap = PlayerPrefs.GetInt("BeginnerMap") == 0;
		m_isStory = PlayerPrefs.GetInt("BeginnerStory") == 0;
		m_isBow = PlayerPrefs.GetInt("BeginnerBow") == 0;
		m_isRoll = PlayerPrefs.GetInt("BeginnerRoll") == 0;
		m_isCoop = PlayerPrefs.GetInt("BeginnerCoop") == 0;
		m_isProp = PlayerPrefs.GetInt("BeginnerProp") == 0;
		m_isCheckJailbreak = PlayerPrefs.GetInt("CheckJailbreak") == 0;
	}

	public void Reset()
	{
		isBeginner = true;
		isSkill = true;
		isMap = true;
		isStory = true;
		isBow = true;
		isRoll = true;
		isCoop = true;
		isProp = true;
	}
}

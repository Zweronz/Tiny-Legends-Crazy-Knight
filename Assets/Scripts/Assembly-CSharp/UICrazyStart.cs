using UnityEngine;

public class UICrazyStart : MonoBehaviour
{
	public TUIFade fade;

	public Transform m_scene_Bk;

	private void Awake()
	{
		if (fade == null)
		{
			fade = base.transform.Find("Fade").GetComponent<TUIFade>();
		}
		if (m_scene_Bk == null)
		{
			m_scene_Bk = base.transform.Find("BK");
		}
	}

	public void Start()
	{
		if ((bool)fade)
		{
			fade.FadeIn();
		}
		if (GameCenterPlugin.IsSupported() && !GameCenterPlugin.IsLogin())
		{
			GameCenterPlugin.Login();
		}
		Crazy_GlobalData.m_curlevel = Crazy_Data.CurData().GetLevel();
		Crazy_GlobalData.m_prelevel = Crazy_Data.CurData().GetLevel();
		Crazy_GlobalData.cur_UI1Times++;
		Crazy_GlobalData.cur_ui = 1;
		if ((bool)m_scene_Bk)
		{
			MyGUIEventListener.Get(m_scene_Bk.gameObject).EventHandleOnClicked += OnNextScene;
		}
		OpenClickNew.Show(true);
	}

	private void OnNextScene(GameObject go)
	{
		m_scene_Bk.gameObject.GetComponent<Collider>().enabled = false;
		if (Crazy_Beginner.instance.isStory)
		{
			fade.FadeOut("CrazyStory");
		}
		else
		{
			fade.FadeOut("CrazyMap");
		}
	}

	public void OnDestroy()
	{
		OpenClickNew.Hide();
	}
}

public class Crazy_SceneManager
{
	private static Crazy_SceneManager s_instance;

	private Crazy_MyScene myScene;

	public static Crazy_SceneManager GetInstance()
	{
		if (s_instance == null)
		{
			s_instance = new Crazy_SceneManager();
		}
		return s_instance;
	}

	public static void DeleteInstance()
	{
		if (s_instance != null)
		{
			s_instance = null;
		}
	}

	public void Initialized(Crazy_MyScene scene)
	{
		myScene = scene;
	}

	public Crazy_MyScene GetScene()
	{
		return myScene;
	}

	public void Uninitialized()
	{
		myScene = null;
	}
}

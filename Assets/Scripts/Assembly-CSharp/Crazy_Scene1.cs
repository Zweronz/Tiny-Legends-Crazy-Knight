using UnityEngine;

public class Crazy_Scene1 : UICrazyScene
{
	public float rangeout;

	protected Crazy_UpdateMonster upmonster;

	public new void Awake()
	{
		base.Awake();
		NewScene();
		NewMonster();
	}

	public void NewMonster()
	{
		GameObject gameObject = new GameObject("Monster");
		gameObject.transform.parent = GameObject.Find("Scene").transform;
		upmonster = gameObject.AddComponent<Crazy_UpdateMonster>() as Crazy_UpdateMonster;
		upmonster.SetRange(rangeout);
		upmonster.SetScene(this);
	}

	public void NewScene()
	{
	}
}

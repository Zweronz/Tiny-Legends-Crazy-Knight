using System;
using UnityEngine;

public class Crazy_SceneNet : UICrazyScene
{
	public float rangeout;

	protected Crazy_UpdateMonster_Net upmonster;

	public new void Awake()
	{
		base.Awake();
		NewMonster();
	}

	protected override void ImpAwake()
	{
		Crazy_MyScene_Net crazy_MyScene_Net = new Crazy_MyScene_Net();
		crazy_MyScene_Net.Awake(UIRevive);
		m_imp = crazy_MyScene_Net;
		Crazy_SceneManager.GetInstance().Initialized(m_imp);
		m_imp.Awake(UIPause, UIBeginnerHint);
		m_imp.Init(countZero, waitTotal);
	}

	public void NewMonster()
	{
		GameObject gameObject = new GameObject("Monster");
		gameObject.transform.parent = GameObject.Find("Scene").transform;
		upmonster = gameObject.AddComponent<Crazy_UpdateMonster_Net>() as Crazy_UpdateMonster_Net;
	}

	public new void Update()
	{
		try
		{
			base.Update();
			UpdateGameBegin();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void UpdateGameBegin()
	{
		if (!Crazy_SceneManager.GetInstance().GetScene().IsGameBegin())
		{
			Crazy_SceneManager.GetInstance().GetScene().UpdateGameBegin();
		}
	}

	public override void OnApplicationPause(bool pause)
	{
	}
}

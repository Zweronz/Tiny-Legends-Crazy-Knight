using System.Collections.Generic;
using UnityEngine;

public class Crazy_EnemyManagement
{
	protected static int enemyActiveNumber;

	protected static int maxEnemyActiveNumber = 8;

	protected static int rangedEnemyNumber;

	protected static int maxRangedEnemyNumber = 4;

	public static void SetMaxActiveNumber(int number)
	{
		maxEnemyActiveNumber = number;
	}

	public static void ResetActiveNumber()
	{
		enemyActiveNumber = 0;
	}

	public static bool AddActiveNumber()
	{
		if (enemyActiveNumber >= maxEnemyActiveNumber)
		{
			return false;
		}
		enemyActiveNumber++;
		return true;
	}

	public static bool RemoveActiveNumber()
	{
		if (enemyActiveNumber <= 0)
		{
			return false;
		}
		enemyActiveNumber--;
		return true;
	}

	public static void SetMaxRangedEnemyNumber(int number)
	{
		maxRangedEnemyNumber = number;
	}

	public static void ResetRangedEnemyNumber()
	{
		rangedEnemyNumber = 0;
	}

	public static bool AddRangedEnemyNumber()
	{
		if (rangedEnemyNumber >= maxRangedEnemyNumber)
		{
			return false;
		}
		rangedEnemyNumber++;
		return true;
	}

	public static bool RemoveRangedEnemyNumber()
	{
		if (rangedEnemyNumber <= 0)
		{
			return false;
		}
		rangedEnemyNumber--;
		return true;
	}

	public static void RenderMonster(Vector3 pos, float distance)
	{
		if (Crazy_GlobalData.enemyList == null)
		{
			return;
		}
		Dictionary<int, GameObject>.KeyCollection keys = Crazy_GlobalData.enemyList.Keys;
		foreach (int item in keys)
		{
			GameObject value;
			if (!Crazy_GlobalData.enemyList.TryGetValue(item, out value))
			{
				continue;
			}
			if ((value.transform.position - pos).sqrMagnitude <= distance * distance)
			{
				Crazy_EnemyControl crazy_EnemyControl = value.GetComponent("Crazy_EnemyControl") as Crazy_EnemyControl;
				Renderer[] meshRenderer = crazy_EnemyControl.GetMeshRenderer();
				for (int i = 0; i < meshRenderer.GetLength(0); i++)
				{
					meshRenderer[i].enabled = true;
				}
			}
			else
			{
				Crazy_EnemyControl crazy_EnemyControl2 = value.GetComponent("Crazy_EnemyControl") as Crazy_EnemyControl;
				Renderer[] meshRenderer2 = crazy_EnemyControl2.GetMeshRenderer();
				for (int j = 0; j < meshRenderer2.GetLength(0); j++)
				{
					meshRenderer2[j].enabled = false;
				}
			}
		}
	}
}

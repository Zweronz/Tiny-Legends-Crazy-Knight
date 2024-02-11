using System;
using UnityEngine;

public class UtilUIIAP : MonoBehaviour
{
	[Serializable]
	public class IAPInfo
	{
		public string id;

		public string pay;

		public Crazy_Price_Type type;

		public string count;

		public string freeCount;

		public string iconname;
	}

	public GameObject go_newbiePack;

	public IAPInfo[] info;

	public void Awake()
	{
		if (info.GetLength(0) >= 7)
		{
			info[6].id = "com.trinitigame.tinysagainvasion.1999centsv22";
		}
	}

	public void Start()
	{
		if (Crazy_Data.CurData().IsGetNewbiePack())
		{
			go_newbiePack.SetActiveRecursively(true);
		}
		else
		{
			go_newbiePack.SetActiveRecursively(false);
		}
		GameObject gameObject = base.transform.Find("ItemPrefab").gameObject;
		int num = 4;
		int num2 = 2;
		for (int i = 0; i < info.GetLength(0); i++)
		{
			if (i != 0)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
				gameObject2.name = info[i].id;
				gameObject2.transform.parent = gameObject.transform.parent;
				gameObject2.transform.localPosition = new Vector3(-92 + i % num * 115 + i / (num * num2) * 290, 47 - i % (num * num2) / num * 100, 0f);
				TUIMeshText component = gameObject2.transform.Find("Money").GetComponent<TUIMeshText>();
				component.text = info[i].pay;
				component.UpdateMesh();
				TUIMeshSprite component2 = gameObject2.transform.Find("Icon").GetComponent<TUIMeshSprite>();
				component2.frameName = info[i].iconname;
				component2.UpdateMesh();
				TUIMeshText component3 = gameObject2.transform.Find("Crystal").GetComponent<TUIMeshText>();
				component3.text = info[i].count;
				component3.UpdateMesh();
				TUIMeshText component4 = gameObject2.transform.Find("Tag/Num").GetComponent<TUIMeshText>();
				component4.text = info[i].freeCount;
				component4.UpdateMesh();
				TUIMeshText component5 = gameObject2.transform.Find("CrystalName").GetComponent<TUIMeshText>();
				switch (info[i].type)
				{
				case Crazy_Price_Type.Gold:
					component5.text = "Gold Pack";
					component5.color = new Color(0.6627451f, 0.47843137f, 0.0627451f, 1f);
					component4.transform.parent.gameObject.SetActiveRecursively(false);
					break;
				case Crazy_Price_Type.Crystal:
					component5.text = "tCrystal Pack";
					component5.color = new Color(0.09019608f, 31f / 51f, 82f / 85f, 1f);
					break;
				}
				component5.UpdateMesh();
			}
		}
	}
}

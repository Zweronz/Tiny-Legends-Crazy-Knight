using UnityEngine;

public class UtilUIMoneyBoard : MonoBehaviour
{
	public void Start()
	{
		UpdateMoney();
	}

	public void Update()
	{
		UpdateMoney();
	}

	public void UpdateMoney()
	{
		TUIMeshText component = base.transform.Find("Gold").Find("Text").GetComponent<TUIMeshText>();
		component.text = Crazy_Data.CurData().GetMoney().ToString();
		component.UpdateMesh();
		TUIMeshText component2 = base.transform.Find("Crystal").Find("Text").GetComponent<TUIMeshText>();
		component2.text = Crazy_Data.CurData().GetCrystal().ToString();
		component2.UpdateMesh();
	}
}

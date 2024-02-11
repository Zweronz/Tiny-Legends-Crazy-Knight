using UnityEngine;

public class UtilUIMonster : MonoBehaviour
{
	protected TUIMeshText current;

	protected TUIMeshText max;

	protected int max_number;

	protected int current_number;

	private void Start()
	{
		max = base.transform.Find("MonsterMaxText").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		current = base.transform.Find("MonsterText").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		current_number = 0;
		Crazy_Modify modify = Crazy_LevelModify.GetModify(Crazy_GlobalData.cur_leveltype, Crazy_GlobalData.cur_level);
		if (modify != null)
		{
			max_number = modify.quantity;
		}
		max.text = "/" + max_number;
		max.UpdateMesh();
		UpdateMonster();
	}

	private void UpdateMonster()
	{
		current_number = Crazy_GlobalData.cur_kill_number;
		if (current_number > max_number)
		{
			current_number = max_number;
		}
		current.text = current_number.ToString();
		current.UpdateMesh();
	}

	private void Update()
	{
		UpdateMonster();
	}
}

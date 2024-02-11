using UnityEngine;

public class GotTapPointsMono : MonoBehaviour
{
	private void GotTapPoints(int tapPoints)
	{
		if (tapPoints > 0)
		{
			Crazy_Data.CurData().AddCrystal(tapPoints);
			Crazy_Data.SaveData();
		}
	}
}

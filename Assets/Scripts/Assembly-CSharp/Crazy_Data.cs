public class Crazy_Data
{
	protected static Crazy_SaveData savedata;

	public static Crazy_SaveData CurData()
	{
		if (savedata == null)
		{
			LoadData();
		}
		return savedata;
	}

	public static void LoadData()
	{
		savedata = Crazy_SaveData.ReadData();
	}

	public static void SaveData()
	{
		Crazy_SaveData.SaveData(savedata);
	}

	public static void DeleteData()
	{
		savedata = null;
		Crazy_SaveData.DeleteData();
	}

	public static void ResetData()
	{
		savedata = Crazy_SaveData.ResetData(savedata);
	}
}

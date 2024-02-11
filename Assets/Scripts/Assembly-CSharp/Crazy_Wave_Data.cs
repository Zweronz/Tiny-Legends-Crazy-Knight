using System.Collections.Generic;

public class Crazy_Wave_Data
{
	public int wave_number;

	public List<Crazy_Wave_Control> controldata = new List<Crazy_Wave_Control>();

	public Crazy_Wave_Data(int number)
	{
		wave_number = number;
	}
}

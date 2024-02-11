using System.Text.RegularExpressions;
using UnityEngine;

public class UtilUIEnterName : MonoBehaviour
{
	private static string showstring = string.Empty;

	public static int namelength = 12;

	private static TouchScreenKeyboard tskb;

	private void Start()
	{
		showstring = Crazy_Data.CurData().GetNetName();
	}

	public static string GetShowString()
	{
		return showstring;
	}

	public static void OpenKeyBoard(string text)
	{
		tskb = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.ASCIICapable, true, false, false, false, "Enter a name(max 12 letters or numbers)");
	}

	private void UpdateKeyboard()
	{
		if (tskb == null)
		{
			return;
		}
		if (!tskb.done)
		{
			if (tskb.text.Length > namelength)
			{
				tskb.text = tskb.text.Substring(0, namelength);
			}
			showstring = tskb.text;
			return;
		}
		if (tskb.text.Length > namelength)
		{
			tskb.text = tskb.text.Substring(0, namelength);
		}
		showstring = tskb.text;
		if (new Regex("^[0-9a-zA-Z]{1,12}$").IsMatch(tskb.text))
		{
			Crazy_Data.CurData().SetNetName(tskb.text);
			tskb = null;
		}
		else
		{
			OpenKeyBoard(tskb.text);
		}
	}

	private void Update()
	{
		UpdateKeyboard();
	}
}

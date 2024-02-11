public class OpenClickNew
{
	private enum Status
	{
		kShowFull = 0,
		kShowTip = 1,
		kHide = 2
	}

	private static Status s_Status;

	public static void Initialize()
	{
		if (Crazy_Const.AMAZON_IAP_CONST)
		{
			ChartBoostAndroid.init("50e69fe817ba47b23b000084", "c3bed3d158ddcda8866bdc7ed1f30f393f4c3f5f");
		}
		else
		{
			ChartBoostAndroid.init("50e6974917ba47a33b000005", "02a2c116b4d0ec5f2fa75b46c7629cd0d534c824");
		}
		ChartBoostAndroid.onStart();
		ChartBoostAndroid.cacheInterstitial(null);
		s_Status = Status.kHide;
	}

	public static void Show(bool show_full)
	{
		if (s_Status == Status.kHide)
		{
			if (show_full)
			{
				ChartBoostAndroid.showInterstitial(null);
			}
			if (show_full)
			{
				s_Status = Status.kShowFull;
			}
			else
			{
				s_Status = Status.kShowTip;
			}
		}
		else if (s_Status == Status.kShowFull)
		{
			if (!show_full)
			{
				if (show_full)
				{
					ChartBoostAndroid.showInterstitial(null);
				}
				s_Status = Status.kShowTip;
			}
		}
		else if (s_Status == Status.kShowTip && show_full)
		{
			if (show_full)
			{
				ChartBoostAndroid.showInterstitial(null);
			}
			s_Status = Status.kShowFull;
		}
	}

	public static void Hide()
	{
		s_Status = Status.kHide;
	}
}

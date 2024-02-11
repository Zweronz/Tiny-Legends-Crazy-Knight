public class Crazy_UtilUISkillBoardContainer : TUIContainer
{
	public UtilUISkillBoard board;

	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			int num = int.Parse(control.name.Replace("Skill", string.Empty));
			board.UpdateSkillInfo(num - 1);
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}

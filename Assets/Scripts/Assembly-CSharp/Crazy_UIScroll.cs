using UnityEngine;

public class Crazy_UIScroll : TUIScroll
{
	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				fingerPosition = input.position;
				touch = true;
				move = false;
				scroll = false;
				lastPosition = fingerPosition;
				moveSpeed = Vector2.zero;
			}
			return false;
		}
		if (input.fingerId != fingerId)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			float num = input.position.x - fingerPosition.x;
			float num2 = input.position.y - fingerPosition.y;
			if (!move && (Mathf.Abs(num) > thresholdX || Mathf.Abs(num2) > thresholdY))
			{
				move = true;
				PostEvent(this, 1, 0f, 0f, null);
				ScrollObjectBegin();
			}
			if (move)
			{
				if (position.x > rangeXMax && num > 0f)
				{
					num *= 0.25f;
				}
				if (position.x < rangeXMin && num < 0f)
				{
					num *= 0.25f;
				}
				if (position.y > rangeYMax && num2 > 0f)
				{
					num2 *= 0.25f;
				}
				if (position.y < rangeYMin && num2 < 0f)
				{
					num2 *= 0.25f;
				}
				position.x += num;
				position.y += num2;
				position.x = Mathf.Clamp(position.x, borderXMin, borderXMax);
				position.y = Mathf.Clamp(position.y, borderYMin, borderYMax);
				fingerPosition = input.position;
				PostEvent(this, 2, position.x, position.y, null);
				ScrollObjectMove();
			}
		}
		else if (input.inputType == TUIInputType.Ended)
		{
			fingerId = -1;
			fingerPosition = Vector2.zero;
			touch = false;
			StartScroll();
			if (move)
			{
				move = false;
				return true;
			}
			return false;
		}
		return false;
	}
}

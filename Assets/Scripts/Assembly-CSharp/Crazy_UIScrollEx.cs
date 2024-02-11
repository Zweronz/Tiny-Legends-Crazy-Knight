using System.Collections.Generic;
using UnityEngine;

public class Crazy_UIScrollEx : TUIScroll
{
	public Crazy_UIGesture m_gesture;

	public bool conflict;

	public List<Crazy_UIScrollEx> conflicts;

	public bool GetConflict()
	{
		bool result = false;
		foreach (Crazy_UIScrollEx conflict in conflicts)
		{
			if (conflict.conflict)
			{
				result = true;
			}
		}
		return result;
	}

	public override bool HandleInput(TUIInput input)
	{
		if (GetConflict())
		{
			fingerId = -1;
			fingerPosition = Vector2.zero;
			touch = false;
			return false;
		}
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
			if (!move)
			{
				if (conflict)
				{
					move = true;
					PostEvent(this, 1, 0f, 0f, null);
					ScrollObjectBegin();
				}
				else if (m_gesture == Crazy_UIGesture.Horizontal && Mathf.Abs(num) > thresholdX && Mathf.Abs(num) > Mathf.Abs(num2))
				{
					conflict = true;
					move = true;
					PostEvent(this, 1, 0f, 0f, null);
					ScrollObjectBegin();
				}
				else if (m_gesture == Crazy_UIGesture.Vertical && Mathf.Abs(num2) > thresholdY && Mathf.Abs(num2) > Mathf.Abs(num))
				{
					conflict = true;
					move = true;
					PostEvent(this, 1, 0f, 0f, null);
					ScrollObjectBegin();
				}
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

	protected override void UpdateScroll(float delta_time)
	{
		bool flag = false;
		if (pageX != null && pageX.Length > 0)
		{
			float num = pagePositionX - position.x;
			if (Mathf.Abs(num) > 5f)
			{
				float num2 = num * 5f;
				position.x += num2 * delta_time;
				float num3 = pagePositionX - position.x;
				if (num * num3 <= 0f)
				{
					position.x = pagePositionX;
				}
				flag = true;
			}
			else
			{
				position.x = pagePositionX;
			}
		}
		else if (Mathf.Abs(moveSpeed.x) > 0.5f)
		{
			float num4 = 1f;
			if (position.x < rangeXMin || moveSpeed.x < 0f)
			{
				num4 = 3f;
			}
			if (position.x > rangeXMax || moveSpeed.x > 0f)
			{
				num4 = 3f;
			}
			num4 = Mathf.Clamp(num4 * delta_time, 0.01f, 0.5f);
			moveSpeed.x -= num4 * moveSpeed.x;
			position.x += moveSpeed.x * delta_time;
			if (position.x <= borderXMin)
			{
				position.x = borderXMin;
				moveSpeed.x = 0f;
			}
			if (position.x >= borderXMax)
			{
				position.x = borderXMax;
				moveSpeed.x = 0f;
			}
			if (Mathf.Abs(moveSpeed.x) < 5f)
			{
				moveSpeed.x = 0f;
			}
			flag = true;
		}
		else if (position.x < rangeXMin)
		{
			float num5 = (rangeXMin - position.x) * 1.5f;
			position.x += num5 * delta_time;
			position.x = Mathf.Clamp(position.x, borderXMin, borderXMax);
			flag = true;
		}
		else if (position.x > rangeXMax)
		{
			float num6 = (rangeXMax - position.x) * 1.5f;
			position.x += num6 * delta_time;
			position.x = Mathf.Clamp(position.x, borderXMin, borderXMax);
			flag = true;
		}
		if (pageY != null && pageY.Length > 0)
		{
			float num7 = pagePositionY - position.y;
			if (Mathf.Abs(num7) > 5f)
			{
				float num8 = num7 * 5f;
				position.y += num8 * delta_time;
				float num9 = pagePositionY - position.y;
				if (num7 * num9 <= 0f)
				{
					position.y = pagePositionY;
				}
				flag = true;
			}
			else
			{
				position.y = pagePositionY;
			}
		}
		else if (Mathf.Abs(moveSpeed.y) > 0.5f)
		{
			float num10 = 1f;
			if (position.y < rangeYMin || moveSpeed.y < 0f)
			{
				num10 = 3f;
			}
			if (position.y > rangeYMax || moveSpeed.y > 0f)
			{
				num10 = 3f;
			}
			num10 = Mathf.Clamp(num10 * delta_time, 0.5f, 1f);
			moveSpeed.y -= num10 * moveSpeed.y;
			position.y += moveSpeed.y * delta_time;
			if (position.y <= borderYMin)
			{
				position.y = borderYMin;
				moveSpeed.y = 0f;
			}
			if (position.y >= borderYMax)
			{
				position.y = borderYMax;
				moveSpeed.y = 0f;
			}
			if (Mathf.Abs(moveSpeed.y) < 5f)
			{
				moveSpeed.y = 0f;
			}
			flag = true;
		}
		else if (position.y < rangeYMin)
		{
			float num11 = (rangeYMin - position.y) * 1.5f;
			position.y += num11 * delta_time;
			position.y = Mathf.Clamp(position.y, borderYMin, borderYMax);
			flag = true;
		}
		else if (position.y > rangeYMax)
		{
			float num12 = (rangeYMax - position.y) * 1.5f;
			position.y += num12 * delta_time;
			position.y = Mathf.Clamp(position.y, borderYMin, borderYMax);
			flag = true;
		}
		if (flag)
		{
			if (Mathf.Abs(eventPosition.x - position.x) >= 0.4f || Mathf.Abs(eventPosition.y - position.y) >= 0.4f)
			{
				eventPosition = position;
				PostEvent(this, 2, position.x, position.y, null);
				ScrollObjectMove();
			}
		}
		else
		{
			conflict = false;
			scroll = false;
			PostEvent(this, 2, position.x, position.y, null);
			ScrollObjectMove();
			PostEvent(this, 3, 0f, 0f, null);
			ScrollObjectEnd();
		}
	}
}

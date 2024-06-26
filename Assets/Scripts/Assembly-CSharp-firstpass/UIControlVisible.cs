using UnityEngine;

public class UIControlVisible : UIControl
{
	public UISprite[] m_Sprite;

	public UIControlVisible()
	{
		m_Sprite = null;
	}

	protected void CreateSprite(int number)
	{
		m_Sprite = new UISprite[number];
		for (int i = 0; i < number; i++)
		{
			m_Sprite[i] = new UISprite();
		}
	}

	protected void SetSpriteTexture(int index, Material material, Rect texture_rect, Vector2 size)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
		m_Sprite[index].Size = size;
	}

	protected void SetSpriteTexture(int index, Material material, Rect texture_rect)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
		m_Sprite[index].Size = new Vector2(texture_rect.width, texture_rect.height);
	}

	protected void SetSpriteTexture(int index, Rect texture_rect)
	{
		m_Sprite[index].TextureRect = texture_rect;
	}

	protected void SetSpriteTexture(int index, Material material)
	{
		m_Sprite[index].Material = material;
	}

	protected void SetSpriteSize(int index, Vector2 size)
	{
		m_Sprite[index].Size = size;
	}

	protected void SetSpriteColor(int index, Color color)
	{
		m_Sprite[index].Color = color;
	}

	protected void SetSpriteAlpha(int index, float alpha)
	{
		m_Sprite[index].Color = new Color(m_Sprite[index].Color.r, m_Sprite[index].Color.g, m_Sprite[index].Color.b, alpha);
	}

	protected float GetSpriteAlpha(int index)
	{
		return m_Sprite[index].Color.a;
	}

	protected void SetSpritePosition(int index, Vector2 position)
	{
		m_Sprite[index].Position = position;
	}

	protected void SetSpriteRotation(int index, float rotation)
	{
		m_Sprite[index].Rotation = rotation;
	}

	protected float GetSpriteRotation(int index)
	{
		return m_Sprite[index].Rotation;
	}

	public void SetScale(float scaleX)
	{
		Vector2 scale = new Vector2(scaleX, scaleX);
		for (int i = 0; i < m_Sprite.Length; i++)
		{
			m_Sprite[i].Scale = scale;
		}
	}

	public void SetScale(float scaleX, float scaleY)
	{
		Vector2 scale = new Vector2(scaleX, scaleY);
		for (int i = 0; i < m_Sprite.Length; i++)
		{
			m_Sprite[i].Scale = scale;
		}
	}

	public Vector2 GetScale(int index)
	{
		return m_Sprite[index].Scale;
	}

	public override void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].SetClip(clip_rect);
			}
		}
	}

	public new void ClearClip()
	{
		base.ClearClip();
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].ClearClip();
			}
		}
	}
}

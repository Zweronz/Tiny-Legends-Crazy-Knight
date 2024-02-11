using System.Collections;
using UnityEngine;

public class HintFadeAway : MonoBehaviour
{
	public TUIMeshText text;

	public float TimeOfFadeIn = 0.5f;

	public float TimeOfFadeOut = 0.3f;

	private bool bFadeIn = true;

	private Color originalColor;

	private void Start()
	{
	}

	private void Update()
	{
		Color color = text.color;
		if (bFadeIn)
		{
			if ((double)color.a < 1.0)
			{
				color.a += Time.deltaTime * 2f;
				text.color = color;
				text.UpdateMesh();
			}
			else
			{
				bFadeIn = false;
			}
		}
		else if ((double)color.a > 0.0)
		{
			color.a -= Time.deltaTime * 3.3f;
			text.color = color;
			text.UpdateMesh();
		}
		else
		{
			bFadeIn = true;
		}
	}

	private IEnumerator AlphaFadeIn()
	{
		Color c = text.color;
		while ((double)c.a < 1.0)
		{
			c.a += Time.deltaTime * 0.25f;
			text.color = c;
			text.UpdateMesh();
			yield return null;
		}
	}

	private IEnumerator AlphaFadeOut()
	{
		Color c = text.color;
		while ((double)c.a > 0.0)
		{
			c.a -= Time.deltaTime * 0.25f;
			text.color = c;
			text.UpdateMesh();
			yield return null;
		}
	}
}

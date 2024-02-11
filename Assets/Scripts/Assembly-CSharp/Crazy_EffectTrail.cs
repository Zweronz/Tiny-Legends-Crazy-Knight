using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Crazy_EffectTrail : MonoBehaviour
{
	private class TronTrailSection
	{
		public Vector3 m_vertexUp;

		public Vector3 m_vertexDown;
	}

	public float m_height = 4f;

	public float m_minDistance = 0.1f;

	public float overLast = 0.2f;

	private float m_overLast;

	private bool m_bCollectPoints;

	private float m_lastTime = 0.5f;

	private float m_time;

	private int m_maxSection = 30;

	private Mesh mesh;

	private Color m_startColor = Color.white;

	private Color m_endColor = Color.white;

	private List<TronTrailSection> m_sections = new List<TronTrailSection>();

	private void Start()
	{
		GameObject gameObject = new GameObject("Trail");
		gameObject.layer = LayerMask.NameToLayer("Player");
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
		meshFilter.mesh = new Mesh();
		mesh = meshFilter.mesh;
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
		meshRenderer.material = Resources.Load("Textures/trail_M") as Material;
	}

	private void Update()
	{
		UpdateMesh();
	}

	private void UpdateMesh()
	{
		if (m_bCollectPoints)
		{
			Vector3 position = base.transform.position;
			Vector3 forward = base.transform.forward;
			TronTrailSection tronTrailSection = new TronTrailSection();
			tronTrailSection.m_vertexDown = position + forward * m_height * 0.2f;
			tronTrailSection.m_vertexUp = position + forward * m_height * 0.8f;
			if (m_sections.Count >= 1)
			{
				TronTrailSection tronTrailSection2 = m_sections[m_sections.Count - 1];
				TronTrailSection tronTrailSection3 = new TronTrailSection();
				tronTrailSection3.m_vertexDown = Vector3.Lerp(tronTrailSection2.m_vertexDown, tronTrailSection.m_vertexDown, 0.5f);
				tronTrailSection3.m_vertexUp = Vector3.Lerp(tronTrailSection2.m_vertexUp, tronTrailSection.m_vertexUp, 0.5f);
				Vector3 vector = tronTrailSection3.m_vertexUp - tronTrailSection3.m_vertexDown;
				tronTrailSection3.m_vertexUp = tronTrailSection3.m_vertexDown + vector.normalized * m_height * 0.6f;
				m_sections.Add(tronTrailSection3);
			}
			m_sections.Add(tronTrailSection);
			m_overLast = overLast;
		}
		else if (m_overLast > 0.001f)
		{
			m_overLast -= Time.deltaTime;
			if (m_overLast < 0.001f)
			{
				mesh.Clear();
				m_sections.Clear();
			}
		}
		if (m_sections.Count >= 2)
		{
			while (m_sections.Count > m_maxSection)
			{
				m_sections.RemoveAt(0);
			}
			mesh.Clear();
			Vector3[] array = new Vector3[m_sections.Count * 2];
			Color[] array2 = new Color[m_sections.Count * 2];
			Vector2[] array3 = new Vector2[m_sections.Count * 2];
			TronTrailSection tronTrailSection4 = m_sections[0];
			for (int i = 0; i < m_sections.Count; i++)
			{
				tronTrailSection4 = m_sections[i];
				float num = i;
				num = Mathf.Clamp01(num / (float)m_sections.Count) + 0.01f;
				array[i * 2] = tronTrailSection4.m_vertexUp;
				array[i * 2 + 1] = tronTrailSection4.m_vertexDown;
				array3[i * 2] = new Vector2(num, 0f);
				array3[i * 2 + 1] = new Vector2(num, 1f);
				Color color = Color.Lerp(m_startColor, m_endColor, m_time / m_lastTime);
				array2[i * 2] = color;
				array2[i * 2 + 1] = color;
			}
			int[] array4 = new int[(m_sections.Count - 1) * 2 * 3];
			for (int j = 0; j < array4.Length / 6; j++)
			{
				array4[j * 6] = j * 2;
				array4[j * 6 + 1] = j * 2 + 1;
				array4[j * 6 + 2] = j * 2 + 2;
				array4[j * 6 + 3] = j * 2 + 2;
				array4[j * 6 + 4] = j * 2 + 1;
				array4[j * 6 + 5] = j * 2 + 3;
			}
			mesh.vertices = array;
			mesh.colors = array2;
			mesh.uv = array3;
			mesh.triangles = array4;
		}
	}

	public void ShowTrail(bool bShow)
	{
		if (bShow)
		{
			mesh.Clear();
			m_sections.Clear();
		}
		m_bCollectPoints = bShow;
	}
}

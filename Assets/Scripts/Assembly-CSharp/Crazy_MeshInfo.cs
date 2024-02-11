using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crazy_MeshInfo : MonoBehaviour
{
	public class TrianglesSeq
	{
		public int[] triangles = new int[3];

		public TrianglesSeq(int i, int j, int k)
		{
			triangles[0] = i;
			triangles[1] = j;
			triangles[2] = k;
		}
	}

	protected Mesh mesh;

	protected List<TrianglesSeq> ts = new List<TrianglesSeq>();

	[CompilerGenerated]
	private static Comparison<TrianglesSeq> _003C_003Ef__am_0024cache2;

	private void CreateGizmos()
	{
		for (int i = 0; i < mesh.vertices.GetLength(0); i++)
		{
			GameObject gameObject = new GameObject("vertices" + i);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = mesh.vertices[i];
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.AddComponent<Crazy_Gizmos>();
		}
	}

	private void Start()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		mesh = component.mesh;
		for (int i = 0; i < mesh.triangles.GetLength(0) / 3; i++)
		{
			TrianglesSeq item = new TrianglesSeq(mesh.triangles[i * 3], mesh.triangles[i * 3 + 1], mesh.triangles[i * 3 + 2]);
			ts.Add(item);
		}
		List<TrianglesSeq> list = ts;
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003CStart_003Em__1;
		}
		list.Sort(_003C_003Ef__am_0024cache2);
		int[] array = new int[mesh.triangles.GetLength(0)];
		for (int j = 0; j < array.GetLength(0) / 3; j++)
		{
			array[j * 3] = ts[j].triangles[0];
			array[j * 3 + 1] = ts[j].triangles[1];
			array[j * 3 + 2] = ts[j].triangles[2];
		}
		mesh.triangles = array;
	}

	[CompilerGenerated]
	private static int _003CStart_003Em__1(TrianglesSeq a, TrianglesSeq b)
	{
		return (a.triangles[0] + a.triangles[1] + a.triangles[2]).CompareTo(b.triangles[0] + b.triangles[1] + b.triangles[2]);
	}
}

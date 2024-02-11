using UnityEngine;

public class Crazy_Shadow : MonoBehaviour
{
	protected GameObject shadowObject;

	public float size = 1.5f;

	protected void InitShadow(float size)
	{
		shadowObject = new GameObject("shadow", typeof(MeshFilter), typeof(MeshRenderer));
		shadowObject.GetComponent<Renderer>().material = Resources.Load("Textures/character_shadow_M") as Material;
		MeshFilter meshFilter = shadowObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0.5f * size, 0.1f, -0.3f * size);
		array[1] = new Vector3(0.5f * size, 0.1f, 0.3f * size);
		array[2] = new Vector3(-0.5f * size, 0.1f, -0.3f * size);
		array[3] = new Vector3(-0.5f * size, 0.1f, 0.3f * size);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(0f, 0f);
		array2[2] = new Vector2(1f, 1f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 2;
		array3[2] = 1;
		array3[3] = 1;
		array3[4] = 2;
		array3[5] = 3;
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.triangles = array3;
		shadowObject.transform.parent = base.transform;
		shadowObject.transform.localPosition = Vector3.zero;
		shadowObject.layer = base.gameObject.layer;
	}

	private void Start()
	{
		InitShadow(size);
	}

	private void Update()
	{
	}
}

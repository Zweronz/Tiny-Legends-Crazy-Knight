using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrinitiSerializeSkinMesh2MeshScript : MonoBehaviour
{
	private class MeshPoints
	{
		public Vector3[] m_vPoints;
	}

	private class MeshInfo
	{
		public Vector2[] m_uv;

		public int[] m_triangles;

		public List<MeshPoints> m_frames;
	}

	private class AnimationInfo
	{
		public int m_iFrameCount;

		public int m_iFrameRate;

		public Dictionary<string, MeshInfo> m_mapMesh;
	}

	public int m_iFrameRate = 30;

	public string[] _anime_name;

	private void Start()
	{
		for (int i = 0; i < _anime_name.GetLength(0); i++)
		{
			SaveAnimation(_anime_name[i]);
		}
	}

	private void SaveAnimation(string aniName)
	{
		base.GetComponent<Animation>()[aniName].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>().Play(aniName);
		float num = 1f / (float)m_iFrameRate;
		int i;
		for (i = 0; (float)i * num <= base.GetComponent<Animation>()[aniName].length; i++)
		{
		}
		AnimationInfo animationInfo = new AnimationInfo();
		animationInfo.m_iFrameCount = i;
		animationInfo.m_iFrameRate = m_iFrameRate;
		animationInfo.m_mapMesh = new Dictionary<string, MeshInfo>();
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
			MeshInfo meshInfo = new MeshInfo();
			animationInfo.m_mapMesh.Add(sharedMesh.name, meshInfo);
			meshInfo.m_uv = sharedMesh.uv;
			meshInfo.m_triangles = sharedMesh.triangles;
			meshInfo.m_frames = new List<MeshPoints>();
		}
		for (int k = 0; k < i; k++)
		{
			base.GetComponent<Animation>()[aniName].enabled = true;
			base.GetComponent<Animation>()[aniName].time = (float)k * num;
			base.GetComponent<Animation>().Sample();
			base.GetComponent<Animation>()[aniName].enabled = false;
			SkinnedMeshRenderer[] array2 = componentsInChildren;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in array2)
			{
				Mesh sharedMesh2 = skinnedMeshRenderer2.sharedMesh;
				Vector3[] array3 = new Vector3[sharedMesh2.vertices.Length];
				for (int m = 0; m < sharedMesh2.vertices.Length; m++)
				{
					Vector3 v = sharedMesh2.vertices[m];
					BoneWeight boneWeight = sharedMesh2.boneWeights[m];
					Transform[] bones = skinnedMeshRenderer2.bones;
					Transform transform = bones[boneWeight.boneIndex0];
					Transform transform2 = bones[boneWeight.boneIndex1];
					Transform transform3 = bones[boneWeight.boneIndex2];
					Transform transform4 = bones[boneWeight.boneIndex3];
					Matrix4x4 matrix4x = transform.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex0];
					Matrix4x4 matrix4x2 = transform2.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex1];
					Matrix4x4 matrix4x3 = transform3.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex2];
					Matrix4x4 matrix4x4 = transform4.localToWorldMatrix * sharedMesh2.bindposes[boneWeight.boneIndex3];
					Vector3 vector = matrix4x.MultiplyPoint(v) * boneWeight.weight0 + matrix4x2.MultiplyPoint(v) * boneWeight.weight1 + matrix4x3.MultiplyPoint(v) * boneWeight.weight2 + matrix4x4.MultiplyPoint(v) * boneWeight.weight3;
					array3[m] = vector;
				}
				MeshPoints meshPoints = new MeshPoints();
				meshPoints.m_vPoints = array3;
				MeshInfo meshInfo2 = animationInfo.m_mapMesh[sharedMesh2.name];
				meshInfo2.m_frames.Add(meshPoints);
			}
		}
		string path = Utils.SavePath() + "/" + aniName + ".bytes";
		FileStream fileStream = File.Open(path, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(animationInfo.m_iFrameCount);
		binaryWriter.Write(animationInfo.m_iFrameRate);
		binaryWriter.Write(animationInfo.m_mapMesh.Count);
		foreach (KeyValuePair<string, MeshInfo> item in animationInfo.m_mapMesh)
		{
			string key = item.Key;
			MeshInfo value = item.Value;
			binaryWriter.Write(key);
			binaryWriter.Write(value.m_triangles.Length);
			for (int n = 0; n < value.m_triangles.Length; n++)
			{
				binaryWriter.Write(value.m_triangles[n]);
			}
			binaryWriter.Write(value.m_uv.Length);
			for (int num2 = 0; num2 < value.m_uv.Length; num2++)
			{
				binaryWriter.Write(value.m_uv[num2].x);
				binaryWriter.Write(value.m_uv[num2].y);
			}
			foreach (MeshPoints frame in value.m_frames)
			{
				for (int num3 = 0; num3 < frame.m_vPoints.Length; num3++)
				{
					Vector3 vector2 = frame.m_vPoints[num3];
					binaryWriter.Write(vector2.x);
					binaryWriter.Write(vector2.y);
					binaryWriter.Write(vector2.z);
				}
			}
		}
		binaryWriter.Close();
		fileStream.Close();
	}
}

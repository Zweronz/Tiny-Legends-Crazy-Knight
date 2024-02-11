using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Crazy_ModelAnimation_zhizhu : Crazy_ModelAnimation
{
	private class TriAndUVInfo
	{
		public Vector2[] m_uv;

		public int[] m_triangles;
	}

	public Material m_material;

	private static Dictionary<string, AnimationInfo> s_mapAnimationInfo;

	private static Dictionary<string, Dictionary<string, List<Mesh>>> s_mapMeshCenter;

	public new void Awake()
	{
		if (s_mapMeshCenter == null)
		{
			s_mapMeshCenter = new Dictionary<string, Dictionary<string, List<Mesh>>>();
		}
		if (s_mapAnimationInfo == null)
		{
			s_mapAnimationInfo = new Dictionary<string, AnimationInfo>();
		}
		for (int i = 0; i < m_strAnimations.Length; i++)
		{
			string text = m_strAnimations[i];
			if (!s_mapMeshCenter.ContainsKey(text))
			{
				ReadAnimation(text);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, List<Mesh>>> item in s_mapMeshCenter)
		{
			string key = item.Key;
			Dictionary<string, List<Mesh>> value = item.Value;
			foreach (KeyValuePair<string, List<Mesh>> item2 in value)
			{
				string key2 = item2.Key;
				GameObject partObj = CreateParts(key2, m_material);
				TrinitiMeshClip trinitiMeshClip = CreatePartAnimation(partObj, key);
				trinitiMeshClip.m_MeshFrames = item2.Value;
			}
		}
		base.Awake();
	}

	private void Start()
	{
	}

	protected override AnimationInfo GetAnimationInfo(string aniName)
	{
		return s_mapAnimationInfo[aniName];
	}

	private void ReadAnimation(string aniName)
	{
		TextAsset textAsset = Resources.Load(m_strResPath + "/" + aniName) as TextAsset;
		MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		int num = binaryReader.ReadInt32();
		int iFrameRate = binaryReader.ReadInt32();
		Dictionary<string, List<Mesh>> dictionary = new Dictionary<string, List<Mesh>>();
		s_mapMeshCenter.Add(aniName, dictionary);
		int num2 = binaryReader.ReadInt32();
		for (int i = 0; i < num2; i++)
		{
			string key = binaryReader.ReadString();
			int num3 = binaryReader.ReadInt32();
			int[] array = new int[num3];
			for (int j = 0; j < num3; j++)
			{
				array[j] = binaryReader.ReadInt32();
			}
			int num4 = binaryReader.ReadInt32();
			Vector2[] array2 = new Vector2[num4];
			for (int k = 0; k < num4; k++)
			{
				array2[k].x = binaryReader.ReadSingle();
				array2[k].y = binaryReader.ReadSingle();
			}
			List<Mesh> list = new List<Mesh>();
			dictionary.Add(key, list);
			for (int l = 0; l < num; l++)
			{
				Mesh mesh = new Mesh();
				list.Add(mesh);
				Vector3[] array3 = new Vector3[num4];
				for (int m = 0; m < num4; m++)
				{
					array3[m].x = binaryReader.ReadSingle();
					array3[m].y = binaryReader.ReadSingle();
					array3[m].z = binaryReader.ReadSingle();
				}
				mesh.vertices = array3;
				mesh.uv = array2;
				mesh.triangles = array;
			}
		}
		binaryReader.Close();
		memoryStream.Close();
		AnimationInfo animationInfo = new AnimationInfo();
		animationInfo.iFrameCount = num;
		animationInfo.iFrameRate = iFrameRate;
		s_mapAnimationInfo.Add(aniName, animationInfo);
		TextAsset textAsset2 = Resources.Load(m_strResPath + "/" + aniName + "_anievt") as TextAsset;
		if (!(null != textAsset2))
		{
			return;
		}
		MemoryStream memoryStream2 = new MemoryStream(textAsset2.bytes);
		BinaryReader binaryReader2 = new BinaryReader(memoryStream2);
		int num5 = binaryReader2.ReadInt32();
		for (int n = 0; n < num5; n++)
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.time = binaryReader2.ReadSingle();
			animationEvent.functionName = binaryReader2.ReadString();
			animationEvent.messageOptions = (SendMessageOptions)binaryReader2.ReadInt32();
			if (binaryReader2.ReadBoolean())
			{
				animationEvent.stringParameter = binaryReader2.ReadString();
			}
			animationInfo.listEvent.Add(animationEvent);
		}
		binaryReader2.Close();
		memoryStream2.Close();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class CommunicationVersion : MonoBehaviour
{
	public OnCommunicationVersion callback;

	public OnCommunicationVersionError callback_error;

	private static CommunicationVersion instance;

	public string CommUrl = string.Empty;

	public bool Comm;

	protected string url = "http://account.trinitigame.com/game/TLCK/TLCK_CommVersion.bytes";

	protected string key = "324516";

	public static CommunicationVersion Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("CommunicationVersion").AddComponent<CommunicationVersion>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Initialize()
	{
		StartCoroutine(Init());
	}

	protected string ReadDataFile(string path)
	{
		string path2 = "Data/" + path;
		TextAsset textAsset = Resources.Load(path2) as TextAsset;
		return textAsset.text;
	}

	protected void WriteFile(string FileName, string WriteString)
	{
		FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.WriteLine(WriteString);
		streamWriter.Flush();
		streamWriter.Close();
		fileStream.Close();
	}

	protected void DeXml(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		CommUrl = GetElement(documentElement, "CommUrl", "url")[0];
		Comm = bool.Parse(GetElement(documentElement, "Comm", "bool")[0]);
	}

	private List<string> GetElement(XmlNode node, string elename, string attname)
	{
		List<string> list = new List<string>();
		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (elename == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				string item = xmlElement.GetAttribute(attname).Trim();
				list.Add(item);
			}
		}
		return list;
	}

	public void OMake()
	{
		string text = ReadDataFile("TLCK_CommVersion");
		text = XXTEAUtils.Encrypt(text, key);
		string fileName = Utils.SavePath() + "/TLCK_CommVersion.bytes";
		WriteFile(fileName, text);
		Debug.Log("TLCK_CommVersion.bytes output is ok.");
	}

	protected IEnumerator Init()
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			Debug.Log(www.error);
			if (callback_error != null)
			{
				callback_error();
			}
			yield break;
		}
		string text = www.text;
		try
		{
			text = XXTEAUtils.Decrypt(text, key);
			if (text == null)
			{
				throw new Exception("XXTEA Failed");
			}
			DeXml(text);
			if (callback != null)
			{
				callback();
			}
		}
		catch (Exception e)
		{
			Debug.Log(e);
			if (callback_error != null)
			{
				callback_error();
			}
		}
	}
}

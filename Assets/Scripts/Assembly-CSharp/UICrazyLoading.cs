using System.Collections;
using UnityEngine;

public class UICrazyLoading : MonoBehaviour
{
	private void Start()
	{
		Crazy_Process cur_process = Crazy_GlobalData.cur_process;
		Crazy_LoadingInfo loadingInfo;
		if (cur_process != null && cur_process.loadingid != -1)
		{
			loadingInfo = Crazy_LoadingInfo.GetLoadingInfo(cur_process.loadingid);
		}
		else
		{
			Crazy_Land landinfo = Crazy_Land.GetLandinfo(Crazy_GlobalData.cur_land_id);
			if (!Crazy_Data.CurData().GetMixMonster())
			{
				loadingInfo = Crazy_LoadingInfo.GetLoadingInfo(landinfo.loading[0]);
			}
			else
			{
				int index = Random.Range(0, landinfo.loading.Count);
				loadingInfo = Crazy_LoadingInfo.GetLoadingInfo(landinfo.loading[index]);
			}
		}
		TUIMeshSprite component = base.transform.Find("LoadingTUI/TUI/TUIControl/LoadingTexture").GetComponent<TUIMeshSprite>();
		TUIMeshText component2 = base.transform.Find("LoadingTUI/TUI/TUIControl/LoadingText").GetComponent<TUIMeshText>();
		component.frameName = loadingInfo.iconname;
		component2.text = loadingInfo.text.Replace("\\n", "\n");
		component.UpdateMesh();
		component2.UpdateMesh();
		Invoke("StartCoroutineLoadLevel", 2f);
	}

	private void StartCoroutineLoadLevel()
	{
		StartCoroutine(LoadLevel());
	}

	private IEnumerator LoadLevel()
	{
		yield return Application.LoadLevelAdditiveAsync(Crazy_GlobalData.next_scene);
		Delete();
	}

	private void Delete()
	{
		Resources.UnloadUnusedAssets();
		Object.Destroy(base.gameObject);
	}
}

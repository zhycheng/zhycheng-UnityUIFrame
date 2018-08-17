using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//完成对scene的管理，对page的管理
public class UIManager
{
    //ArrayList<>
	public static UIManager instance=null;
	public static UIManager Instance()
	{
		if(instance==null)
		{
			instance = new UIManager();
		}
		return instance;
	}

	private UIManager()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void OnSceneUnloaded(Scene scene)
	{
		UnityEngine.Debug.Log("UIManager "+scene.name+" unloaded");
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		UnityEngine.Debug.Log("UIManager " + scene.name + " loaded mode is "+ mode.ToString());
	}


	public void PushPage(string prefabPath)
	{
		Scene cur = SceneManager.GetActiveScene();
		Transform canvasTransform = null;
		GameObject[] goList= cur.GetRootGameObjects();
		for(int i=0;i< goList.Length;i++)
		{
			if(goList[i].name=="Canvas")
			{
				canvasTransform = goList[i].transform;
				break;
			}
		}
        if (canvasTransform == null)
        {
            UnityEngine.Debug.LogError("that scene don't have canvas");
            return;
        }
		GameObject prefabObject = Resources.Load<GameObject>(prefabPath);
		GameObject initPrefab = GameObject.Instantiate(prefabObject, canvasTransform);
	}
	public void ReplaceScene(string name)
	{
		SceneManager.LoadSceneAsync(name);
	}
}

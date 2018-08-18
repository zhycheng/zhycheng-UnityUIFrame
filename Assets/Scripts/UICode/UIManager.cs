using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum UIMEssageType
{
	PushScene,
	PopScene,
	PushPage,
	PopPage,
}

//完成对scene的管理，对page的管理
public class UIManager
{
    List<UIPage> pageList;
    private static UIManager instance=null;
	private Dictionary<string, List<UIPage>> popSceneMessage;
	private Dictionary<string, List<UIPage>> pushSceneMessage;
	private Dictionary<string, List<UIPage>> popPageMessage;
	private Dictionary<string, List<UIPage>> pushPageMessage;
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
        pageList = new List<UIPage>();
		popSceneMessage = new Dictionary<string, List<UIPage>>();
		pushSceneMessage = new Dictionary<string, List<UIPage>>();
		popPageMessage = new Dictionary<string, List<UIPage>>();
		pushPageMessage = new Dictionary<string, List<UIPage>>();
	}

	private void OnSceneUnloaded(Scene scene)
	{
		UnityEngine.Debug.Log("UIManager "+scene.name+" unloaded");
		//在scene unload之前就处理好了
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		UnityEngine.Debug.Log("UIManager " + scene.name + " loaded mode is "+ mode.ToString());
		if(pushSceneMessage.ContainsKey(scene.name))
		{
			List<UIPage> list = pushSceneMessage[scene.name];
			for(int i=0;i< list.Count;i++)
			{
				list[i].OnUIMessage(UIMEssageType.PushScene, scene.name);
			}
		}
	}

	public void RegisterUIMessage(string name, UIMEssageType type,UIPage page)
	{
		Dictionary<string, List<UIPage>> container = null ;
		if(type==UIMEssageType.PushScene)
		{
			container = pushSceneMessage;
		}
		else if(type==UIMEssageType.PopScene)
		{
			container = popSceneMessage;
		}
		else if(type==UIMEssageType.PushPage)
		{
			container = pushPageMessage;
		}
		else if(type==UIMEssageType.PopPage)
		{
			container = popPageMessage;
		}
		if (container.ContainsKey(name))
		{
			List<UIPage> list = container[name];
			bool haveThePage = false;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == page)
				{
					haveThePage = true;
					break;
				}
			}
			if (haveThePage)
			{
				UnityEngine.Debug.LogError("please do not register " + name + " " + type.ToString() + " again");
				return;
			}
			list.Add(page);

		}
		else
		{
			List<UIPage> list = new List<UIPage>();
			list.Add(page);
			container.Add(name, list);
		}
	}
	public void UnRegisterUIMessage(string name, UIMEssageType type,UIPage page)
	{
		Dictionary<string, List<UIPage>> container = null;
		if (type == UIMEssageType.PushScene)
		{
			container = pushSceneMessage;
		}
		else if (type == UIMEssageType.PopScene)
		{
			container = popSceneMessage;
		}
		else if (type == UIMEssageType.PushPage)
		{
			container = pushPageMessage;
		}
		else if (type == UIMEssageType.PopPage)
		{
			container = popPageMessage;
		}
		if(container.ContainsKey(name)==false)
		{
			UnityEngine.Debug.LogError("the "+type.ToString()+" don;t have "+name +"registered");
			return;
		}
		List<UIPage> list = container[name];
		bool haveThePage = false;
		for(int i=0;i<list.Count;i++)
		{
			if(list[i]==page)
			{
				list.RemoveAt(i);
				haveThePage = true;
				break;
			}
		}
		if(haveThePage==false)
		{
			UnityEngine.Debug.LogError(page.GetPageName()+" don't registered "+type.ToString()+" "+ name);
		}
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
		string gameObjectName= initPrefab.GetComponent<UIPage>().GetPageName();
		initPrefab.name = gameObjectName;
		pageList.Add(initPrefab.GetComponent<UIPage>());
		//传递消息
		if(pushPageMessage.ContainsKey(gameObjectName))
		{
			List<UIPage> list = pushPageMessage[gameObjectName];
			for(int i=0;i<list.Count;i++)
			{
				list[i].OnUIMessage(UIMEssageType.PushPage, gameObjectName);
			}
		}

    }
        
    public void PopPage()
    {
        //将最后的一个page删除
        Scene cur = SceneManager.GetActiveScene();
        Transform canvasTransform = null;
        GameObject[] goList = cur.GetRootGameObjects();
        for (int i = 0; i < goList.Length; i++)
        {
            if (goList[i].name == "Canvas")
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
        UIPage up = pageList[pageList.Count - 1];
        string gameObjectName = up.GetPageName();
        Transform childTransform=canvasTransform.Find(gameObjectName);
        GameObject.Destroy(childTransform.gameObject);
        pageList.Remove(up);
		//传递消息
		if (popPageMessage.ContainsKey(gameObjectName))
		{
			List<UIPage> list = popPageMessage[gameObjectName];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnUIMessage(UIMEssageType.PopPage, gameObjectName);
			}
		}
	}

	public void PopPage(string pageName)
	{
		Scene cur = SceneManager.GetActiveScene();
		Transform canvasTransform = null;
		GameObject[] goList = cur.GetRootGameObjects();
		for (int i = 0; i < goList.Length; i++)
		{
			if (goList[i].name == "Canvas")
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
		UIPage up = GetUIPageByName(pageName);
		if(up==null)
		{
			UnityEngine.Debug.LogError("can't find the uipage by the name");
			return;
		}
		string gameObjectName = up.GetPageName();
		Transform childTransform = canvasTransform.Find(gameObjectName);
		GameObject.Destroy(childTransform.gameObject);
		pageList.Remove(up);
		//传递消息
		if (popPageMessage.ContainsKey(gameObjectName))
		{
			List<UIPage> list = popPageMessage[gameObjectName];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnUIMessage(UIMEssageType.PopPage, gameObjectName);
			}
		}
	}

	public void RefreshPageByName(string name, System.Object obj)
	{
		UIPage page = this.GetUIPageByName(name);
		if(page==null)
		{
			UnityEngine.Debug.LogError("can't find the uipage by the name");
			return;
		}
		page.Refresh(obj);
	}

	public UIPage GetUIPageByName(string name)
	{
		for(int i=0;i<pageList.Count;i++)
		{
			if(pageList[i].GetPageName()==name)
			{
				return pageList[i];
			}
		}
		return null;
	}

	public void ReplaceScene(string name)
	{
		//先发送消息
		//当前所有page退出消息
		for(int i=0;i<pageList.Count;i++)
		{
			string pageName = pageList[i].GetPageName();
			if(popPageMessage.ContainsKey(pageName))
			{
				List<UIPage> allpage = popPageMessage[pageName];
				for(int k=0;k<allpage.Count;k++)
				{
					allpage[k].OnUIMessage(UIMEssageType.PopPage, pageName);
				}
			}
		}
		//当前scene的退出消息
		Scene cur = SceneManager.GetActiveScene();
		string sceneName = cur.name;
		if(popSceneMessage.ContainsKey(sceneName))
		{
			List<UIPage> list = popSceneMessage[sceneName];
			for(int i=0;i<list.Count;i++)
			{
				list[i].OnUIMessage(UIMEssageType.PopScene,sceneName);
			}
		}
		pageList.Clear();
		SceneManager.LoadSceneAsync(name);
    }
}

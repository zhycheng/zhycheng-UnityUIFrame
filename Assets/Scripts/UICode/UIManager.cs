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

	private Dictionary<string, List<UIPage>> scenePageInfo;
    private static UIManager instance=null;
	private List<Scene> runningScene;
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
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
		scenePageInfo = new Dictionary<string, List<UIPage>>();
		popSceneMessage = new Dictionary<string, List<UIPage>>();
		pushSceneMessage = new Dictionary<string, List<UIPage>>();
		popPageMessage = new Dictionary<string, List<UIPage>>();
		pushPageMessage = new Dictionary<string, List<UIPage>>();
		runningScene = new List<Scene>();
	}

	private void OnSceneUnloaded(Scene scene)
	{
		UnityEngine.Debug.Log("UIManager "+scene.name+" unloaded");
		//在scene unload之前就处理好了
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		UnityEngine.Debug.Log("UIManager " + scene.name + " loaded mode is "+ mode.ToString());
		SceneManager.SetActiveScene(scene);
		runningScene.Add(scene);
		if (pushSceneMessage.ContainsKey(scene.name))
		{
			List<UIPage> list = pushSceneMessage[scene.name];
			for(int i=0;i< list.Count;i++)
			{
				list[i].OnUIMessage(UIMEssageType.PushScene, scene.name);
			}
		}
	}
	private void OnActiveSceneChanged(Scene current, Scene next)
	{
		string currentName = current.name;

		if (currentName == null)
		{
			// Scene1 has been removed
			currentName = "Replaced";
		}

		UnityEngine.Debug.Log("Scenes: " + currentName + ", " + next.name);
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
		if(scenePageInfo.ContainsKey(cur.name))
		{
			List<UIPage> list = scenePageInfo[cur.name];
			list.Add(initPrefab.GetComponent<UIPage>());
		}
		else
		{
			UnityEngine.Debug.LogError("can't find scene page info");
			return;
		}

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
		if(scenePageInfo.ContainsKey(cur.name))
		{
			List<UIPage> list = scenePageInfo[cur.name];
			if(list.Count>0)
			{
				UIPage up = list[list.Count - 1];
				string gameObjectName = up.GetPageName();
				Transform childTransform = canvasTransform.Find(gameObjectName);
				if(childTransform!=null)
				{
					GameObject.Destroy(childTransform.gameObject);
					list.Remove(up);
					//传递消息
					if (popPageMessage.ContainsKey(gameObjectName))
					{
						List<UIPage> listMsg = popPageMessage[gameObjectName];
						for (int i = 0; i < listMsg.Count; i++)
						{
							listMsg[i].OnUIMessage(UIMEssageType.PopPage, gameObjectName);
						}
					}
				}
				else
				{
					return;
				}
				
			}
			else
			{
				return;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("can't find scene page info poppage");
			return;
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

		UIPage up = GetUIPageByName(cur.name, pageName);
		if(up==null)
		{
			UnityEngine.Debug.LogError("can't find the uipage by the name");
			return;
		}
		string gameObjectName = up.GetPageName();
		Transform childTransform = canvasTransform.Find(gameObjectName);
		if(childTransform!=null)
		{
			GameObject.Destroy(childTransform.gameObject);
			if(scenePageInfo.ContainsKey(cur.name))
			{
				List<UIPage> scenePage = scenePageInfo[cur.name];
				scenePage.Remove(up);
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
			else
			{
				UnityEngine.Debug.LogError("current scene do not contain the scene: "+ cur.name);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("poppage can't find "+ gameObjectName);
		}
	}
	/***
	 * 只能刷新当前scene的page
	 * 
	 * */
	public void RefreshPageByName(string name, System.Object obj)
	{
		Scene currentScene = SceneManager.GetActiveScene();
		UIPage page = this.GetUIPageByName(currentScene.name, name);
		if(page==null)
		{
			UnityEngine.Debug.LogError("can't find the uipage by the name");
			return;
		}
		page.Refresh(obj);
	}

	public UIPage GetUIPageByName(string scene,string name)
	{
		if(scenePageInfo.ContainsKey(scene))
		{
			List<UIPage> list = scenePageInfo[scene];
			for(int i=0;i< list.Count;i++)
			{
				if(list[i].GetPageName()==name)
				{
					return list[i];
				}
			}
		}
		return null;
	}
	
	public void ReplaceScene(string name)
	{
		//先发送消息
		//当前所有page退出消息
		string[] allKeys = new string[scenePageInfo.Count];
		scenePageInfo.Keys.CopyTo(allKeys,0);
		foreach (string key in allKeys)
		{
			if(scenePageInfo.ContainsKey(key))
			{
				List<UIPage> pages = scenePageInfo[key];
				if (pages != null)
				{
					for (int i = 0; i < pages.Count; i++)
					{
						string pageName = pages[i].GetPageName();
						if (popPageMessage.ContainsKey(pageName))
						{
							List<UIPage> allpage = popPageMessage[pageName];
							for (int k = 0; k < allpage.Count; k++)
							{
								allpage[k].OnUIMessage(UIMEssageType.PopPage, pageName);
							}
						}
					}
				}
				//当前scene退出消息
				if(popSceneMessage.ContainsKey(key))
				{
					List<UIPage> list = popSceneMessage[key];
					for (int i = 0; i < list.Count; i++)
					{
						list[i].OnUIMessage(UIMEssageType.PopScene, key);
					}
				}
				scenePageInfo.Remove(key);
			}
		}
		runningScene.Clear();
		if (!scenePageInfo.ContainsKey(name))
		{
			//这个scene对应的page容器还没有建立
			scenePageInfo.Add(name, new List<UIPage>());
		}
		SceneManager.LoadSceneAsync(name);
	}



	public void PushScene(string name)
	{
		if (!scenePageInfo.ContainsKey(name))
		{
			//这个scene对应的page容器还没有建立
			scenePageInfo.Add(name, new List<UIPage>());
		}
		SceneManager.LoadSceneAsync(name,LoadSceneMode.Additive);
	}

	public void PopScene()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene==null)
		{
			return;
		}
		//弹出最上层的scene
		// 1.先把最上面的scene的的page全部都弹出
		if(scenePageInfo.ContainsKey(currentScene.name))
		{
			List<UIPage> scenePages = scenePageInfo[currentScene.name];
			if(scenePages!=null)
			{
				for(int i=0;i< scenePages.Count;i++)
				{
					string pageName = scenePages[i].GetPageName();
					if (popPageMessage.ContainsKey(pageName))
					{
						List<UIPage> allpage = popPageMessage[pageName];
						for (int k = 0; k < allpage.Count; k++)
						{
							allpage[k].OnUIMessage(UIMEssageType.PopPage, pageName);
						}
					}
				}
			}
		}
		//2.删除scene的page容器
		if(scenePageInfo.ContainsKey(currentScene.name))
		{
			scenePageInfo.Remove(currentScene.name);
		}
		//3 当前scene退出的消息
		if (popSceneMessage.ContainsKey(currentScene.name))
		{
			List<UIPage> list = popSceneMessage[currentScene.name];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnUIMessage(UIMEssageType.PopScene, currentScene.name);
			}
		}
		//4.当前scene正式退出
		runningScene.Remove(currentScene);
		if(runningScene.Count>0)
		{
			SceneManager.SetActiveScene(runningScene[runningScene.Count-1]);
		}
		SceneManager.UnloadSceneAsync(currentScene.name);
		//SceneManager.UnloadScene(currentScene.name);
	}
}

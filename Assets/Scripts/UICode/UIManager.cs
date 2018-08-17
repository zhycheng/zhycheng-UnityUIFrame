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
		if (pushSceneMessage.ContainsKey(scene.name))
		{
			List<UIPage> list = pushSceneMessage[scene.name];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnUIMessage(UIMEssageType.PushScene, scene.name);
			}
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		UnityEngine.Debug.Log("UIManager " + scene.name + " loaded mode is "+ mode.ToString());
		if(popSceneMessage.ContainsKey(scene.name))
		{
			List<UIPage> list = popSceneMessage[scene.name];
			for(int i=0;i< list.Count;i++)
			{
				list[i].OnUIMessage(UIMEssageType.PopScene, scene.name);
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
		initPrefab.name = initPrefab.GetComponent<UIPage>().GetPageName();
		pageList.Add(initPrefab.GetComponent<UIPage>());

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
        string gameObjectName = up.gameObject.name;
        Transform childTransform=canvasTransform.Find(gameObjectName);
        GameObject.Destroy(childTransform.gameObject);
        pageList.Remove(up);
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
		string gameObjectName = up.gameObject.name;
		Transform childTransform = canvasTransform.Find(gameObjectName);
		GameObject.Destroy(childTransform.gameObject);
		pageList.Remove(up);
	}

	public void RefreshPageByName(string name,GameObject obj)
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
		SceneManager.LoadSceneAsync(name);
        pageList.Clear();
    }
}

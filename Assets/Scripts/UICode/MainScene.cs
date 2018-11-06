using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : UIPage {
    [SerializeField] Button showBtn;
	[SerializeField] Button popSceneBtn;
	// Use this for initialization
	void Start () {
        showBtn.onClick.AddListener(showPage);
		popSceneBtn.onClick.AddListener(closeScene);
	}
    private void showPage()
    {
		//UnityEngine.Debug.LogError("show page info");
		UIManager.Instance().PushPage("Prefab/ShowInfoPage","come from mainscene");
    }

	private void closeScene()
	{
		//UIManager.Instance().PopScene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDestroy()
    {
        showBtn.onClick.RemoveListener(showPage);
    }

	public override string GetPageName()
	{
		return "MainScene";
	}
}

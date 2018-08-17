using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MianScene : UIPage {
    [SerializeField] Button showBtn;
	// Use this for initialization
	void Start () {
        showBtn.onClick.AddListener(showPage);

    }
    private void showPage()
    {
        //UnityEngine.Debug.LogError("show page info");
        UIManager.Instance().PushPage("Prefab/ShowInfoPage");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoPage : UIPage {
	[SerializeField] Button okBtn;

	// Use this for initialization
	void Start () {
		okBtn.onClick.AddListener(onOkbtnClicked);
	}

	private void onOkbtnClicked()
	{
		UIManager.Instance().PushPage("Prefab/SurePage");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnDestroy()
	{
		okBtn.onClick.RemoveListener(onOkbtnClicked);
	}

	public override string GetPageName()
	{
		return "ShowInfoPage";
	}
}

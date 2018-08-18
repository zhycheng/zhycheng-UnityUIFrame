using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoPage : UIPage {
	[SerializeField] Button okBtn;
	[SerializeField] Button closeBtn;
	// Use this for initialization
	void Start () {
		okBtn.onClick.AddListener(onOkbtnClicked);
		closeBtn.onClick.AddListener(onCloseBtnClosed);
		UIManager.Instance().RegisterUIMessage("SurePage", UIMEssageType.PushPage, this);
		UIManager.Instance().RegisterUIMessage("SurePage", UIMEssageType.PopPage, this);
	}

	private void onCloseBtnClosed()
	{
		UIManager.Instance().PopPage();
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
		closeBtn.onClick.RemoveListener(onCloseBtnClosed);
		UIManager.Instance().UnRegisterUIMessage("SurePage", UIMEssageType.PushPage, this);
		UIManager.Instance().UnRegisterUIMessage("SurePage", UIMEssageType.PopPage, this);
	}

	public override string GetPageName()
	{
		return "ShowInfoPage";
	}
	public override void OnUIMessage(UIMEssageType type, string name)
	{
		UnityEngine.Debug.LogError("Message type is "+type.ToString()+" "+name);
	}
	public override void Refresh(System.Object obj)
	{
		UnityEngine.Debug.LogError("zyc the page ShowInfoPage Refresh.");
	}
}

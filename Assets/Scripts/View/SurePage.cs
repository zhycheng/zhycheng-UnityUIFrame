using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurePage : UIPage {
	[SerializeField] Button closBtn;
	[SerializeField] Button dosomething;

	// Use this for initialization
	void Start () {
		closBtn.onClick.AddListener(onCloseBtnClicked);
		dosomething.onClick.AddListener(onDoSomethingClicked);
	}
	private void onCloseBtnClicked()
	{
		UIManager.Instance().PopPage();
	}
	private void onDoSomethingClicked()
	{
		String ss = "zhycheng";
		UIManager.Instance().RefreshPageByName("ShowInfoPage", ss);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnDestroy()
	{
		closBtn.onClick.RemoveListener(onCloseBtnClicked);
		dosomething.onClick.RemoveListener(onDoSomethingClicked);
	}

	public override string GetPageName()
	{
		return "SurePage";
	}
}

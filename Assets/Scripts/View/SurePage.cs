using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurePage : UIPage {
	[SerializeField] Button closBtn;

	// Use this for initialization
	void Start () {
		closBtn.onClick.AddListener(onCloseBtnClicked);
	}
	private void onCloseBtnClicked()
	{
		UIManager.Instance().PopPage();
	}

	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnDestroy()
	{
		closBtn.onClick.RemoveListener(onCloseBtnClicked);
	}

	public override string GetPageName()
	{
		return "Surepage";
	}
}

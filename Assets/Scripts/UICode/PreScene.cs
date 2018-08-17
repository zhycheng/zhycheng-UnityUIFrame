﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreScene : MonoBehaviour {
    [SerializeField] protected Button loginBtn;
    [SerializeField] protected Button logoutBtn;
    [SerializeField] protected Toggle savePasswordToggle;
    [SerializeField] protected InputField userNameInputField;
    [SerializeField] protected InputField passwordInputField;
	// Use this for initialization
	void Start () {
        loginBtn.onClick.AddListener(onLoginBtnClicked);
        logoutBtn.onClick.AddListener(onLogoutBtnClicked);
        savePasswordToggle.onValueChanged.AddListener(onToogleValueChanged);
    }
    private void onLoginBtnClicked()
    {
        UnityEngine.Debug.LogError("zyc onLoginBtnClicked,name is "+ userNameInputField.text+",password is "+ passwordInputField.text);
    }

    private void onLogoutBtnClicked()
    {
        Application.Quit();
    }

    private void onToogleValueChanged(bool value)
    {
        UnityEngine.Debug.LogError("zyc toggle state is "+value.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDestroy()
    {
        loginBtn.onClick.RemoveListener(onLoginBtnClicked);
        logoutBtn.onClick.RemoveListener(onLogoutBtnClicked);
    }
}
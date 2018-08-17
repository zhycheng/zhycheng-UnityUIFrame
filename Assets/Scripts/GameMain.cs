using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //在这里初始化系统 这里是游戏的入口，初始化各种组件
        //UIManager.Instance().PushPage("Resources/UIPrefab/PreScenePage");
        UIManager.Instance().ReplaceScene("PreScene");
    }

    // Update is called once per frame
    void Update () {
		
	}
}

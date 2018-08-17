using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class UIPage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Refresh(System.Object obj)
    {

    }

	public abstract string GetPageName();

	public virtual void OnUIMessage(UIMEssageType type,string name)
	{
		//throw new NotImplementedException();
	}
}

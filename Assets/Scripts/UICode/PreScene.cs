using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.UI;

public class PreScene : UIPage
{
    [SerializeField] protected Button loginBtn;
    [SerializeField] protected Button logoutBtn;
    [SerializeField] protected Toggle savePasswordToggle;
    [SerializeField] protected InputField userNameInputField;
    [SerializeField] protected InputField passwordInputField;
	[SerializeField] protected Transform ca;
    // Use this for initialization
    void Start()
    {
        loginBtn.onClick.AddListener(onLoginBtnClicked);
        logoutBtn.onClick.AddListener(onLogoutBtnClicked);
        savePasswordToggle.onValueChanged.AddListener(onToogleValueChanged);
		UIManager.Instance().RegisterUIMessage("MainScene", UIMEssageType.PopScene, this);
    }
    private void onLoginBtnClicked()
    {
        //UnityEngine.Debug.LogError("zyc onLoginBtnClicked,name is " + userNameInputField.text + ",password is " + passwordInputField.text);
        UIManager.Instance().ReplaceScene("MainScene");
    }

    private void onLogoutBtnClicked()
    {
        Application.Quit();
    }

	private void OnEnable()
	{
		 
		EasyTouch.instance.alwaysSendSwipe = true;
		EasyTouch.SetUICompatibily(false);
		EasyTouch.On_Swipe += On_Swipe;
	}

	private void OnDisable()
	{
		EasyTouch.On_Swipe -= On_Swipe;
	}


	void Update()
	{
		// 获取当前手势
		Gesture current = EasyTouch.current;
		if (current == null)
			return;
		return;
		// 滑动时实现组件移动
		//Debug.LogError(current.type.ToString());
		if (current.type == EasyTouch.EvtType.On_Swipe)
		{
			//ca.Translate(Vector3.down * current.deltaPosition.x / Screen.width);
			//ca.Rotate(Vector3.down * current.deltaPosition.y, Space.World);
			//float y = (current.deltaPosition.y)  + current.deltaPosition.x;
			//Vector3 vec = new Vector3(0, y, 0);

			//ca.Rotate(vec, Space.World);
			Debug.LogError(" x is "+ current.deltaPosition.x+" y is "+ current.deltaPosition.y);
			Vector2 ab = new Vector2(current.deltaPosition.x, current.deltaPosition.y);
			Vector2 xy = new Vector2(1,0);
			float angle = Vector2.Angle(ab, xy);
			if(ab.y<=0)
			{
				angle = -angle;
			}
			Debug.LogError("angle is "+ angle);
			
			ca.eulerAngles = new Vector3(90, angle, 0);
			//eulerAngles 
		}
	}


	void On_Swipe(Gesture ges)
	{
		Debug.LogError(" x is " + ges.deltaPosition.x + " y is " + ges.deltaPosition.y);
		Vector2 ab = new Vector2(ges.deltaPosition.x, ges.deltaPosition.y);
		if(Mathf.Abs(ges.deltaPosition.x)<0.001 || Mathf.Abs(ges.deltaPosition.y) < 0.001)
		{
			return;
		}
		Vector2 xy = new Vector2(-1, 0);
		float angle = Vector2.Angle(ab, xy);
		if (ab.y <= 0)
		{
			angle = -angle;
		}
		Debug.LogError("angle is " + angle);

		ca.eulerAngles = new Vector3(90, angle, 0);
		
		//eulerAngles 
	}

	private void onToogleValueChanged(bool value)
    {
        UnityEngine.Debug.LogError("zyc toggle state is " + value.ToString());
    }


    private void OnDestroy()
    {
        loginBtn.onClick.RemoveListener(onLoginBtnClicked);
        logoutBtn.onClick.RemoveListener(onLogoutBtnClicked);
        savePasswordToggle.onValueChanged.RemoveListener(onToogleValueChanged);
		UIManager.Instance().UnRegisterUIMessage("MainScene", UIMEssageType.PopScene, this);

	}

	public override string GetPageName()
	{
		return "PreScene";
	}

	public override void OnUIMessage(UIMEssageType type, string name)
	{
		UnityEngine.Debug.LogError(name);
	}
}

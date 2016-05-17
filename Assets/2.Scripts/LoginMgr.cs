using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginMgr : MonoBehaviour {

	#region private property

	[SerializeField]
	private GameObject guiTextLogIn;   // ログインテキスト
	[SerializeField]
	private GameObject guiTextSignUp;  // 新規登録テキスト
	
	UserAuth user_auth;
	// ログイン画面のときtrue, 新規登録画面のときfalse
	private bool isLogin;
	// ボタンが押されると対応する変数がtrueになる
	private bool logInButton;
	private bool signUpMenuButton;
	private bool signUpButton;
	private bool backButton;

	// テキストボックスで入力される文字列を格納
	private string id;
	private string pw;
	private string mail;
	//エラーメッセージ
	[SerializeField]
	private Text error;

	#endregion


	#region event

	void Start () {
		user_auth = GameObject.Find ("UserAuth").GetComponent<UserAuth> ();
		user_auth.logOut();

		isLogin = false;
		guiTextSignUp.SetActive (false);
		guiTextLogIn.SetActive (true);
	}

	void Update(){
		if(user_auth.isError){
			error.text = "入力を受け付けられません";
		}
	}

	#endregion


	#region public method

	//ログイン画面
	public void LogInDisplay(){
		// テキスト切り替え
		guiTextSignUp.SetActive (false);
		guiTextLogIn.SetActive (true);
		user_auth.isError = false;
		error.text = "";
	}

	//ログイン
	public void EnterLogIn(){
		isLogin = true;
		id = GameObject.Find ("ID").GetComponent<InputField> ().text;
		pw = GameObject.Find ("PASS").GetComponent<InputField> ().text;
		user_auth.logIn( id, pw );
	}

		

	//新規登録画面
	public void SignUpDisplay(){
		// テキスト切り替え
		guiTextSignUp.SetActive (true);
		guiTextLogIn.SetActive (false);
		user_auth.isError = false;
		error.text = "";
	}

	//サインアップ
	public void EnterSignUp(){
		isLogin = true;
		id = GameObject.Find ("ID").GetComponent<InputField> ().text;
		pw = GameObject.Find ("PASS").GetComponent<InputField> ().text;
		mail = GameObject.Find ("EMAIL").GetComponent<InputField> ().text;
		user_auth.signUp( id, mail, pw );
	}

	//Title画面に移行
	public void ToTitle(){
		Application.LoadLevel("scTitle");
	}

	#endregion

}
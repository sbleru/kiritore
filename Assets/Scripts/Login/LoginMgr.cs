using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginMgr : MonoBehaviour {

	public GameObject guiTextLogIn;   // ログインテキスト
	public GameObject guiTextSignUp;  // 新規登録テキスト
	// UserAuth
	UserAuth user_auth;

	// ログイン画面のときtrue, 新規登録画面のときfalse
	private bool isLogIn_SignUp;

	// ボタンが押されると対応する変数がtrueになる
	private bool logInButton;
	private bool signUpMenuButton;
	private bool signUpButton;
	private bool backButton;

	// テキストボックスで入力される文字列を格納
	public string id;
	public string pw;
	public string mail;

	//エラーメッセージ
	public Text error;


	void Start () {
		
		user_auth = GameObject.Find ("UserAuth").GetComponent<UserAuth> ();
		user_auth.logOut();

		// ゲームオブジェクトを検索し取得する
		guiTextLogIn  = GameObject.Find ("GUITextLogIn");
		guiTextSignUp = GameObject.Find ("GUITextSignUp");  

		isLogIn_SignUp = false;
		guiTextSignUp.SetActive (false);
		guiTextLogIn.SetActive (true);

	}

	void Update(){
		// currentPlayerを毎フレーム監視し、ログインが完了したら
//		if(isLogIn_SignUp){
//			
//			if( user_auth.currentPlayer() != null )
//				Application.LoadLevel("scTitle");
//		}
	}

	//ログイン画面
	public void LogInDisplay(){
		// テキスト切り替え
		guiTextSignUp.SetActive (false);
		guiTextLogIn.SetActive (true);
		error.text = "";
	}

	//ログイン
	public void EnterLogIn(){
		isLogIn_SignUp = true;
		id = GameObject.Find ("ID").GetComponent<InputField> ().text;
		pw = GameObject.Find ("PASS").GetComponent<InputField> ().text;
		user_auth.logIn( id, pw );
		//エラーメッセージ
		StartCoroutine ("Wait");
		print (user_auth.isError);
		if(user_auth.isError){
			error.text = "IDまたはパスワードが間違っています";
		}
		user_auth.isError = false;
	}

		

	//新規登録画面
	public void SignUpDisplay(){
		// テキスト切り替え
		guiTextSignUp.SetActive (true);
		guiTextLogIn.SetActive (false);
		error.text = "";
	}

	//サインアップ
	public void EnterSignUp(){
		isLogIn_SignUp = true;
		id = GameObject.Find ("ID").GetComponent<InputField> ().text;
		pw = GameObject.Find ("PASS").GetComponent<InputField> ().text;
		mail = GameObject.Find ("EMAIL").GetComponent<InputField> ().text;

		user_auth.signUp( id, mail, pw );
		//エラーメッセージ
		StartCoroutine ("Wait");
		if(user_auth.isError){
			error.text = "入力が間違っています";
		}
		user_auth.isError = false;

		//StartCoroutine ("Wait");
	}

	//Title画面に移行
	public void ToTitle(){
		Application.LoadLevel("scTitle");
	}


	IEnumerator Wait(){
		yield return new WaitForSeconds (1.0f);
	}

//	void OnGUI () {
//
//		// ログイン画面 
//		if( isLogIn ){
//
//			drawLogInMenu();
//
//			// ログインボタンが押されたら
//			if( logInButton )
//				FindObjectOfType<UserAuth>().logIn( id, pw );
//
//			// 新規登録画面に移動するボタンが押されたら
//			if( signUpMenuButton )
//				isLogIn = false;
//		}
//
//		// 新規登録画面
//		else {
//
//			drawSignUpMenu();  
//
//			// 新規登録ボタンが押されたら
//			if( signUpButton )
//				FindObjectOfType<UserAuth>().signUp( id, mail, pw );
//
//			// 戻るボタンが押されたら
//			if( backButton )
//				isLogIn = true;
//		}
//
//		// currentPlayerを毎フレーム監視し、ログインが完了したら
//		if( FindObjectOfType<UserAuth>().currentPlayer() != null )
//			Application.LoadLevel("Stage");
//
//	}
//
//	private void drawLogInMenu()
//	{
//		// テキスト切り替え
//		guiTextSignUp.SetActive (false);
//		guiTextLogIn.SetActive (true);
//
//		// テキストボックスの設置と入力値の取得
//		GUI.skin.textField.fontSize = 20;
//		int txtW = 150, txtH = 40;
//		id = GUI.TextField     (new Rect(Screen.width*1/2, Screen.height*1/3 - txtH*1/2, txtW, txtH), id);
//		pw = GUI.PasswordField (new Rect(Screen.width*1/2, Screen.height*1/2 - txtH*1/2, txtW, txtH), pw, '*');
//
//		// ボタンの設置
//		int btnW = 180, btnH = 50;
//		GUI.skin.button.fontSize = 20;
//		logInButton      = GUI.Button( new Rect(Screen.width*1/4 - btnW*1/2, Screen.height*3/4 - btnH*1/2, btnW, btnH), "Log In" );
//		signUpMenuButton = GUI.Button( new Rect(Screen.width*3/4 - btnW*1/2, Screen.height*3/4 - btnH*1/2, btnW, btnH), "Sign Up" );
//
//	}
//
//	private void drawSignUpMenu()
//	{
//		// テキスト切り替え
//		guiTextLogIn.SetActive (false);
//		guiTextSignUp.SetActive (true);
//
//		// テキストボックスの設置と入力値の取得
//		int txtW = 150, txtH = 35;
//		GUI.skin.textField.fontSize = 20;
//		id = GUI.TextField     (new Rect(Screen.width*1/2, Screen.height*1/4 - txtH*1/2, txtW, txtH), id);
//		pw = GUI.PasswordField (new Rect(Screen.width*1/2, Screen.height*2/5 - txtH*1/2, txtW, txtH), pw, '*');
//		mail = GUI.TextField   (new Rect(Screen.width*1/2, Screen.height*11/20 - txtH*1/2, txtW, txtH), mail);
//
//		// ボタンの設置
//		int btnW = 180, btnH = 50;
//		GUI.skin.button.fontSize = 20;
//		signUpButton = GUI.Button( new Rect(Screen.width*1/4 - btnW*1/2, Screen.height*3/4 - btnH*1/2, btnW, btnH), "Sign Up" );
//		backButton   = GUI.Button( new Rect(Screen.width*3/4 - btnW*1/2, Screen.height*3/4 - btnH*1/2, btnW, btnH), "Back" ); 
//	}

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NCMB;
using System.Collections.Generic;

public class UserAuth : MonoBehaviour {

	private string currentPlayerName;
	//private LoginMgr login_mgr;
	private Text error;
	public bool isError;

	// mobile backendに接続してログイン ------------------------

	public void logIn( string id, string pw ) {

//		try{
//			NCMBUser.LogInAsync(id,pw);
//			currentPlayerName = id;
//		}catch{
//			
//		}

		//android実機ではなぜか機能しない例外処理
		NCMBUser.LogInAsync (id, pw, (NCMBException e) => {
			// 接続成功したら
			if( e == null ){
				currentPlayerName = id;
				Application.LoadLevel("scTitle");
			} else {
				isError = true;
			}

		});
	}

	// mobile backendに接続して新規会員登録 ------------------------

	public void signUp( string id, string mail, string pw ) {

		NCMBUser user = new NCMBUser();
		user.UserName = id;
		user.Email    = mail;
		user.Password = pw;
//		try{
//			user.SignUpAsync();
//			currentPlayerName = id;
//		}catch{
//			
//		}

		user.SignUpAsync((NCMBException e) => { 

			if( e == null ){
				currentPlayerName = id;
				Application.LoadLevel("scTitle");
			} else {
				isError = true;
			}
		});
	}

	// mobile backendに接続してログアウト ------------------------

	public void logOut() {

//		try{
//			NCMBUser.LogOutAsync();
//			currentPlayerName = null;
//		}catch{
//			
//		}

		NCMBUser.LogOutAsync ( (NCMBException e) => {
			if( e == null ){
				currentPlayerName = null;
			}
		});
	}

	// 現在のプレイヤー名を返す --------------------
	public string currentPlayer()
	{
		return currentPlayerName;
	}

	// シングルトン化する ------------------------

	private UserAuth instance = null;
	void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);

			string name = gameObject.name;
			gameObject.name = name + "(Singleton)";

			GameObject duplicater = GameObject.Find (name);
			if (duplicater != null) {
				Destroy (gameObject);
			} else {
				gameObject.name = name;
			}
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		//error = GameObject.Find ("LogInMgr").GetComponent<LoginMgr> ().error.GetComponent<Text>();
		isError = false;
	}
}
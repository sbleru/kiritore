using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NCMB;

public class Manager : MonoBehaviour {

	#region public static

	//オンラインモード
	public static bool isOnline;

	#endregion


	#region private property
	//ハイスコア
	private NCMB.HighScore highScore;
	//ローカルハイスコア
	private string highScoreKey = "highScore";
	int high;

	//ログアウトボタン
	[SerializeField]
	private GameObject logout_button;
	//ログインボタン
	[SerializeField]
	private GameObject login_button;
	//ハイスコアテキスト
	[SerializeField]
	private GameObject high_score;
	//ランキングボード
	[SerializeField]
	private GameObject leader_board;
	private int score;

	// User name
	[SerializeField]
	private Text user_name;

	#endregion


	#region event

	void Start ()
	{
		SoundMgr.Instance.PlayBGM (SoundMgr.Instance.bgm_title); //プレイ時以外のBGM設定

		//staticなクラスのメンバ変数の初期化
		SetValue.initialize ();
		//ローカルのハイスコアを取得する
		high = PlayerPrefs.GetInt (highScoreKey, 0);
		user_name.text = "local";

		isOnline = false;
		logout_button.SetActive (false);
		login_button.SetActive (true);
		leader_board.SetActive (false);

		//ログインしていればユーザネームとログアウトボタンを表示 
		//シングルトン化しているかの確認
		if (FindObjectOfType<UserAuth>() != null) {
			if (FindObjectOfType<UserAuth> ().currentPlayer () != null) {
				isOnline = true;
				user_name.text = FindObjectOfType<UserAuth> ().currentPlayer ();
				//ハイスコア取得
				highScore = new NCMB.HighScore (0, user_name.text);
				highScore.fetch ();

				logout_button.SetActive (true);
				login_button.SetActive (false);
				leader_board.SetActive (true);
			}
		}

	}

	void Update(){
		if (isOnline) {
			high_score.GetComponent<Text> ().text = "HighScore:" + highScore.score;
		} else {
			high_score.GetComponent<Text> ().text = "HighScore:" + high;
		}
	}

	#endregion


	#region public method		

	public void LogOut(){
		FindObjectOfType<UserAuth> ().logOut ();

		//ローカルプレイに切り替える
		user_name.text = "local";
		isOnline = false;
		logout_button.SetActive (false);
		login_button.SetActive (true);
		leader_board.SetActive (false);
		StartCoroutine("WaitFetch");
	}

	#endregion
}
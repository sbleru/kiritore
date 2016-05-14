using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NCMB;

public class Manager : MonoBehaviour
{
	//オンラインモード
	public static bool isOnline;
	//ハイスコア
	private NCMB.HighScore highScore;
	//ローカルハイスコア
	private string highScoreKey = "highScore";
	int high;

	// タイトル
	private GameObject title;
	//ログアウトボタン
	private GameObject logout_button;
	//ログインボタン
	private GameObject login_button;
	//ハイスコアテキスト
	private GameObject High_Score;
	//ランキングボード
	private GameObject leader_board;
	private Text high_score;
	private int score;

	// User name
	public Text user_name;

	// ボタンが押されると対応する変数がtrueになる
//	private bool leaderBoardButton;
//	private bool commentButton;
	private bool logOutButton;

	void Start ()
	{
		isOnline = false;
		logOutButton = false;
		// Titleゲームオブジェクトを検索し取得する
		title = GameObject.Find ("Title");
		logout_button = GameObject.Find ("Logout");
		login_button = GameObject.Find ("Login");
		High_Score = GameObject.Find ("HighScore");
		leader_board = GameObject.Find ("LeaderBoard");
		high_score = High_Score.GetComponent<Text>();

		logout_button.SetActive (false);
		login_button.SetActive (true);
		leader_board.SetActive (false);

		user_name.text = "local";

		//ログインしていればユーザネームとログアウトボタンを表示 
		//try{}catch{}:例外処理
		try{
			if(FindObjectOfType<UserAuth> ().currentPlayer () != null){
				user_name.text = FindObjectOfType<UserAuth> ().currentPlayer ();
				//ハイスコア取得
				highScore = new NCMB.HighScore( 0, user_name.text );
				highScore.fetch();
				

				isOnline = true;
				logout_button.SetActive (true);
				login_button.SetActive(false);
				leader_board.SetActive(true);
			}
		} catch{
		}
		StartCoroutine("WaitFetch");

	}

	IEnumerator WaitFetch(){
		yield return new WaitForSeconds (0.5f);

		if(isOnline){
			high_score.text = "HighScore:" + highScore.score;
		} else {
			high = PlayerPrefs.GetInt (highScoreKey, 0);
			high_score.text = "HighScore:" + high;
		}

	}
		

	public void LogOut(){
		logOutButton = true;
		FindObjectOfType<UserAuth> ().logOut ();

		//ローカルプレイに切り替える
		user_name.text = "local";
		isOnline = false;
		logout_button.SetActive (false);
		login_button.SetActive (true);
		leader_board.SetActive (false);
		StartCoroutine("WaitFetch");
	}
}
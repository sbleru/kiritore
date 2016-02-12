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
		//score = highScore.GetScore ();
		if(isOnline){
			high_score.text = "HighScore:" + highScore.score;
		} else {
			high = PlayerPrefs.GetInt (highScoreKey, 0);
			high_score.text = "HighScore:" + high;
		}

	}

	void Update(){
		// ログアウト完了してたらログインメニューに戻る
//		if(logOutButton){
//			if(FindObjectOfType<UserAuth> ().currentPlayer () == null )
//				Application.LoadLevel("LogIn"); 
//		}
	}

	public void LogOut(){
		logOutButton = true;
		FindObjectOfType<UserAuth> ().logOut ();

		//ローカルプレイに切り替える
		user_name.text = "local";
		isOnline = false;;
		logout_button.SetActive (false);
		login_button.SetActive (true);
		leader_board.SetActive (false);
		StartCoroutine("WaitFetch");
	}
//	void OnGUI() {
//		if( !IsPlaying() ){
//			drawButton();
//
//			// ログアウトボタンが押されたら
//			if( logOutButton )
//				FindObjectOfType<UserAuth> ().logOut ();
//
//			// 画面タップでゲームスタート
//			if ( Event.current.type == EventType.MouseDown) 
//				GameStart ();
//		}
//
//		// ログアウト完了してたらログインメニューに戻る
//		if( FindObjectOfType<UserAuth>().currentPlayer() == null )
//			Application.LoadLevel("Login");   
//	}

//	void GameStart() {
//		// ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
//		title.SetActive (false);
//		Instantiate (player, player.transform.position, player.transform.rotation);
//	}
//
//	public void GameOver() {
//		FindObjectOfType<Score> ().Save ();
//		// ゲームオーバー時に、タイトルを表示する
//		title.SetActive (true);
//	}
//
//	public bool IsPlaying () {
//		// ゲーム中かどうかはタイトルの表示/非表示で判断する
//		return title.activeSelf == false;
//	}
//
//	private void drawButton() {
//		// ボタンの設置
//		int btnW = 140, btnH = 50;
//		GUI.skin.button.fontSize = 18;
//		leaderBoardButton = GUI.Button( new Rect(0*btnW, 0, btnW, btnH), "Leader Board" );
//		commentButton     = GUI.Button( new Rect(1*btnW, 0, btnW, btnH), "Comment" );
//		logOutButton      = GUI.Button( new Rect(2*btnW, 0, btnW, btnH), "Log Out" );
//	}
}
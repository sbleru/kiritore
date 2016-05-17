using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.Collections;

public class ScoreCtrl : MonoBehaviour {

	#region private property

	//スコアをオンラインに保存
	private NCMB.HighScore highScore;
	private bool isNewRecord;

	[SerializeField]
	private Text score_text;		// スコア表示
	[SerializeField]
	private Text highscore_text;	// ハイスコアを表示するGUIText
	[SerializeField]
	private GameObject leader_board;	// リーダーボード

	private int high_score;

	// PlayerPrefsで保存するためのキー
	private string highScoreKey = "highScore";
	private string thistimeScoreKey = "thistimeScore";

	#endregion


	#region event

	// Use this for initialization
	void Start () {
		SoundMgr.Instance.PlayBGM (SoundMgr.Instance.bgm_title); //プレイ時以外のBGM設定

		// ハイスコアを取得する。保存されてなければ0点。
		Initialize ();

		//オンライン
		if(Manager.isOnline){
			string name = FindObjectOfType<UserAuth>().currentPlayer();
			highScore = new NCMB.HighScore( 0, name );
			highScore.fetch();

			StartCoroutine ("WaitFetch");

		} else{  //ローカル
			leader_board.SetActive (false);

			// スコアがハイスコアより大きければ
			if (high_score < SetValue.total_score) {
				high_score = SetValue.total_score;
				Save();
			}
			// スコア・ハイスコアを表示する
			highscore_text.text = "HIGH : " + high_score;
			score_text.text = "SCORE : " + SetValue.total_score;
		}

	}

	#endregion


	#region private method

	IEnumerator WaitFetch(){
		yield return new WaitForSeconds (0.5f);
		// スコアがハイスコアより大きければ
		if (highScore.score < SetValue.total_score) {
			isNewRecord = true; // フラグを立てる
			highScore.score = SetValue.total_score;
			Save ();
		}
		// スコア・ハイスコアを表示する
		highscore_text.text = "HIGH : " + highScore.score;
		score_text.text = "SCORE : " + SetValue.total_score;
	}
		

	private void Initialize(){   
		// フラグを初期化する
		isNewRecord = false;

		// ハイスコアを取得する。保存されてなければ0を取得する。
		high_score = PlayerPrefs.GetInt (highScoreKey, 0);
	}

	// ハイスコアの保存
	private void Save ()
	{
		// ハイスコアを保存する（ただし記録の更新があったときだけ）
		if( isNewRecord )
			highScore.save();

		// ハイスコアを保存する
		//PlayerPrefs.SetInt (highScoreKey, 0);
		PlayerPrefs.SetInt (highScoreKey, high_score);
		PlayerPrefs.SetInt (thistimeScoreKey, SetValue.total_score);
		PlayerPrefs.Save ();

		// ゲーム開始前の状態に戻す
		Initialize ();
	}

	#endregion
}
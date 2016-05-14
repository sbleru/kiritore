using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoardManager : MonoBehaviour {

	private LeaderBoard lBoard;
	private NCMB.HighScore currentHighScore;
	public Text[] top = new Text[5];
	public Text[] nei = new Text[5];

	bool isScoreFetched;
	bool isRankFetched;
	bool isLeaderBoardFetched;

	// ボタンが押されると対応する変数がtrueになる
	private bool backButton;

	void Start ()
	{
		lBoard = new LeaderBoard();

		// テキストを表示するゲームオブジェクトを取得
		for( int i = 0; i < 5; ++i ) {
			top[i] = GameObject.Find ("Top" + i).GetComponent<Text>();
			nei[i] = GameObject.Find ("Neighbor" + i).GetComponent<Text>();
		}

		// フラグ初期化
		isScoreFetched = false;
		isRankFetched  = false;
		isLeaderBoardFetched = false;

		// 現在のハイスコアを取得
		string name = FindObjectOfType<UserAuth>().currentPlayer();
		currentHighScore = new NCMB.HighScore( -1, name );
		currentHighScore.fetch();
	}

	void Update()
	{
		// 現在のハイスコアの取得が完了したら1度だけ実行
		if( currentHighScore.score != -1 && !isScoreFetched ){  
			lBoard.fetchRank( currentHighScore.score );
			isScoreFetched = true;
		}

		// 現在の順位の取得が完了したら1度だけ実行
		if( lBoard.currentRank != 0 && !isRankFetched ){
			lBoard.fetchTopRankers();
			lBoard.fetchNeighbors();
			isRankFetched = true;
		}

		// ランキングの取得が完了したら1度だけ実行
		if( lBoard.topRankers != null && lBoard.neighbors != null && !isLeaderBoardFetched){ 
			// 自分が1位のときと2位のときだけ順位表示を調整
			int offset = 2;
			if(lBoard.currentRank == 1) offset = 0;
			if(lBoard.currentRank == 2) offset = 1;

			// 取得したトップ5ランキングを表示
			for( int i = 0; i < lBoard.topRankers.Count; ++i) {
				top[i].text = i+1 + ". " + lBoard.topRankers[i].print();
			}

			// 取得したライバルランキングを表示
			for( int i = 0; i < lBoard.neighbors.Count; ++i) {
				nei[i].text = lBoard.currentRank - offset + i + ". " + lBoard.neighbors[i].print();
			}
			isLeaderBoardFetched = true;
		}
	}

	public void ToTitle(){
		Application.LoadLevel("scTitle");
	}

	/*
	void OnGUI () {
		drawMenu();
		// 戻るボタンが押されたら
		if( backButton )
			Application.LoadLevel("scTitle");
	}

	private void drawMenu() {
		// ボタンの設置
		int btnW = 170, btnH = 30;
		GUI.skin.button.fontSize = 20;
		backButton = GUI.Button( new Rect(Screen.width*1/2 - btnW*1/2, Screen.height*7/8 - btnH*1/2, btnW, btnH), "Title" );
	}
	*/
}
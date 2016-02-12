using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialCtrl : MonoBehaviour {

	public GameObject _ex1, _ex2, _ex3, _ex4, _ex5, Arrow0, Arrow1, Arrow2, Arrow3, Arrow4;
	public Text explain;
	private int i;

	// Use this for initialization
	void Start () {
		_ex1.SetActive (true);
		_ex2.SetActive (false);
		_ex3.SetActive (false);
		_ex4.SetActive (false);
		_ex5.SetActive (false);
		Arrow0.SetActive (false);
		Arrow1.SetActive (false);
		Arrow2.SetActive (false);
		Arrow3.SetActive (false);
		Arrow4.SetActive (false);
		explain.text = "上画面と一致するように下画面のパネルを指で切り取ってください.";
		i = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick(){
		switch(i){
		case 0:
			_ex1.SetActive (false);
			_ex2.SetActive (true);
			explain.text = "＊パネルの折れ方、位置に注目してみよう！制限時間にも注意！";
			break;
		case 1:
			_ex2.SetActive (false);
			_ex3.SetActive (true);
			Arrow0.SetActive (true);
			explain.text = "切り取りにくい時はZoomボタンを押して拡大してください.もう一度押せば戻ります.";
			break;
		case 2:
			_ex3.SetActive (false);
			_ex4.SetActive (true);
			Arrow0.SetActive (false);
			Arrow1.SetActive (true);
			explain.text = "間違えた時はやり直しボタンを押せば最初からやり直せます.(*時間は戻りません)";
			break;
		case 3:
			_ex4.SetActive (false);
			_ex5.SetActive (true);
			Arrow1.SetActive (false);
			Arrow2.SetActive (true);
			explain.text = "解答できたらAnswerボタンを押してください.";
			break;
		case 4:
			Arrow2.SetActive (false);
			Arrow3.SetActive (true);
			explain.text = "各ステージの持ち点は100点で、誤った箇所の数だけ減点されます.";
			break;
		case 5:
			Arrow3.SetActive (false);
			Arrow4.SetActive (true);
			explain.text = "残り持ち点、ステージ数、残り時間からTotalスコアを計算し、加算します.";
			break;
		case 6:
			Arrow4.SetActive (false);
			explain.text = "残り持ち点が0を下回る場合はゲームオーバーです.";
			break;
		case 7:
			//_ex5.SetActive (false);
			explain.text = "それでは最高得点目指して、\nキリトレ！";
			break;
		case 8:
			Application.LoadLevel ("scTitle");
			break;
		default:
			Application.LoadLevel ("scTitle");
			break;
		}

		i++;
	}

}

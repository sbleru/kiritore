using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeCtrl : MonoBehaviour {
	public Text timeText;
	public string scnext;
	PanelRoot panel_root;
	//public float time = 20;
	private bool isExecuted=false;
	private bool isStop = false;

	// Use this for initialization
	void Start () {
		panel_root = GameObject.Find ("Center").GetComponent<PanelRoot> ();
		//float型からint型へCastし、String型に変換して表示
		timeText.text = ((int)SetValue.time).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(isStop == false){
			//1秒に1ずつ減らしていく
			SetValue.time -= Time.deltaTime;
			//マイナスは表示しない
			if (SetValue.time < 0){
				SetValue.time = 0;
				panel_root.Return_pos ();
				if(isExecuted == false){
					panel_root.CalculateScore ();
					panel_root.NextScene ();
					isExecuted = true;
				}
			} 
		}
		timeText.text = ((int)SetValue.time).ToString ();
	}

	//answerボタンを押したら時間を止める
	public void Stop(){
		isStop = true;
	}
}

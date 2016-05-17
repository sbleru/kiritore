using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeCtrl : MonoBehaviour {

	#region private property

	[SerializeField]
	private Text left_time;
	PanelRoot panel_root;

	private bool isExecuted=false;
	private bool isStop = false;

	#endregion


	#region event

	// Use this for initialization
	void Start () {
		panel_root = GameObject.FindWithTag ("Root").GetComponent<PanelRoot> ();
		//float型からint型へCastし、String型に変換して表示
		left_time.text = ((int)SetValue.time).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(isStop == false){
			//1秒に1ずつ減らしていく
			SetValue.time -= Time.deltaTime;

			//マイナスは表示しない
			if (SetValue.time < 0){
				SetValue.time = 0;
				panel_root.ReturnPos ();

				if(isExecuted == false){
					panel_root.CalculateScore ();
					panel_root.NextScene ();
					isExecuted = true;
				}
			} 
		}

		left_time.text = ((int)SetValue.time).ToString ();
	}

	#endregion


	#region public method

	//answerボタンを押したら時間を止める
	public void StopTime(){
		isStop = true;
	}

	#endregion
}

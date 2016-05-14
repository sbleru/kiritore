using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class CreateTheme : MonoBehaviour {
	const float CAMERA2_Z= 15.0f;
	private TextAsset _question;
	//折り紙を回転させるかのフラグ
	public bool rotate_flag;
	//最後の問題としてのフラグ
	public bool final_flag;
	//ランダム生成のフラグ
	private bool random_flag;
	public GameObject panel;

	private PanelRoot panel_root;
	private float root_z;
	int side_length;
	int i,j;

	string question_texts;
	public bool[,] theme_panel = new bool[11,11];

	// Use this for initialization
	void Start () {
		rotate_flag = false;
		final_flag = false;
		random_flag = false;
		//Resourcesフォルダのテキストを読み込む
		//SetValue.stage_num = 5;
		switch(SetValue.stage_num){
		case 1:
			_question = Resources.Load ("q_1") as TextAsset;
			break;
		case 2:
			_question = Resources.Load ("q_2") as TextAsset;
			break;
		case 3:
			_question = Resources.Load ("q_3") as TextAsset;
			break;
		case 4:
			_question = Resources.Load ("q_4") as TextAsset;
			rotate_flag = true;
			break;
		case 5:
			_question = Resources.Load ("q_5") as TextAsset;
			rotate_flag = true;
			break;
		case 6:
			_question = Resources.Load ("q_6") as TextAsset;
			rotate_flag = true;
			break;
		case 7:
			_question = Resources.Load ("q_7") as TextAsset;
			rotate_flag = true;
			break;
		case 8:
			_question = Resources.Load ("q_8") as TextAsset;
			rotate_flag = true;
			break;
		case 9:
			_question = Resources.Load ("q_9") as TextAsset;
			rotate_flag = true;
			break;
		case 10:
			_question = Resources.Load ("q_10") as TextAsset;
			rotate_flag = true;
			break;
		case 11:
		case 12:
		case 13:
			random_flag = true;
			break;
		default:
			random_flag = true;
			rotate_flag = true;
			break;
		}


		panel_root = GameObject.Find ("Center").GetComponent<PanelRoot> ();
		side_length = panel_root.side_length;

		////折り紙オブジェクトをカメラの範囲に合うように移動
		Camera camera2 = GameObject.Find ("Main Camera Thema").GetComponent<Camera> ();
		this.transform.position = camera2.ScreenToWorldPoint (
			new Vector3(Screen.width * 0.6f, Screen.height * 0.75f, (CAMERA2_Z + side_length)));
		root_z = this.transform.position.z;

		Vector3 root_position = this.transform.position;


		//ランダムにお題を生成する
		if(random_flag){
			GetRandomThemeData ();

		} else { //テキストデータからお題を生成する
			
			//テキストデータを文字列として取り込む
			question_texts = _question.text;
			GetThemeData ();
		}
		
		//お題を作成　第一象限
		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){

				GameObject game_object = (
					Instantiate (panel, new Vector3 (root_position.x + i, root_position.y + j, root_z), Quaternion.identity)
					as GameObject);

				//お題のパネルと解答のパネル
				if(theme_panel[i,j] == false){
					Destroy (game_object);
				}
			}
		}
		//第二象限
		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){

				GameObject game_object = (
					Instantiate (panel, new Vector3 (root_position.x -1 - i, root_position.y + j, root_z), Quaternion.identity)
					as GameObject);

				if(theme_panel[i,j] == false){
					Destroy (game_object);
				}
			}
		}
		//第三象限
		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){

				GameObject game_object = (
					Instantiate (panel, new Vector3 (root_position.x -1 - i, root_position.y -1 - j, root_z), Quaternion.identity)
					as GameObject);

				if(theme_panel[i,j] == false){
					Destroy (game_object);
				}
			}
		}
		//第四象限
		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){

				GameObject game_object = (
					Instantiate (panel, new Vector3 (root_position.x + i, root_position.y -1 - j, root_z), Quaternion.identity)
					as GameObject);

				if(theme_panel[i,j] == false){
					Destroy (game_object);
				}
			}
		}
	}

	IEnumerator WaitWrite(){
		yield return new WaitForSeconds (1.0f);

	}

	//お題のデータを配列に格納
	void GetThemeData(){

		string[] lines = question_texts.Split ('\n');
		int i=0, j=0;

		//lines内の各行に対して、順番に処理していくループ
		foreach(var line in lines){
			if(line == ""){ //行がなければ
				continue;  
			}

			//print (line);
			string[] words = line.Split ();

			//words内の各ワードに対して、順番に処理していくループ
			foreach(var word in words){
				if(word == ""){
					continue;
				}
				if(int.Parse (word) == 1){
					theme_panel [i, j] = true;
				} else {
					theme_panel [i, j] = false;
				}

				j++;
				if (j > side_length-1){
					break;
				}
			}
			j = 0;
			i++;
			if (i > side_length-1){
				break;
			}
		}
	}

	//ランダムにお題生成
	//お題のデータを配列に格納
	void GetRandomThemeData(){
		int i=0, j=0, k=0;
		int judge;
		int count_judge;

		//一度全てtrueまたはfalseに設定
		judge = Random.Range (0, 2);

		if(judge == 0){
			for (i = 0; i < side_length; i++) {
				for(j=0; j<side_length; j++){
					theme_panel [i, j] = false;
				}
			}
			count_judge = Random.Range (12, 15);


			for (k = 0; k < count_judge; k++) {
				judge = Random.Range (0, 4);

				if (judge == 0) {
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//2*2の正方形
					theme_panel [i, j] = true;
					theme_panel [i, j + 1] = true;
					theme_panel [i + 1, j] = true;
					theme_panel [i + 1, j + 1] = true;
				}
				else if(judge == 1){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//鏡面Z型
					theme_panel [i, j] = true;
					theme_panel [i, j + 1] = true;
					theme_panel [i + 1, j+1] = true;
					theme_panel [i + 1, j + 2] = true;
				}
				else if(judge==2){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//Z型
					theme_panel [i+1, j] = true;
					theme_panel [i+1, j + 1] = true;
					theme_panel [i, j+1] = true;
					theme_panel [i, j+2] = true;
				}
				else if(judge==3){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//凸型
					theme_panel [i+1, j+1] = true;
					theme_panel [i, j] = true;
					theme_panel [i, j+1] = true;
					theme_panel [i, j+2] = true;
				}
			}



		} else {
			for (i = 0; i < side_length; i++) {
				for(j=0; j<side_length; j++){
					theme_panel [i, j] = true;
				}
			}
				
			count_judge = Random.Range (12, 15);


			for (k = 0; k < count_judge; k++) {
				judge = Random.Range (0, 4);

				if (judge == 0) {
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//2*2の正方形
					theme_panel [i, j] = false;
					theme_panel [i, j + 1] = false;
					theme_panel [i + 1, j] = false;
					theme_panel [i + 1, j + 1] = false;
				}
				else if(judge == 1){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//鏡面Z型
					theme_panel [i, j] = false;
					theme_panel [i, j + 1] = false;
					theme_panel [i + 1, j+1] = false;
					theme_panel [i + 1, j + 2] = false;
				}
				else if(judge==2){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//Z型
					theme_panel [i+1, j] = false;
					theme_panel [i+1, j + 1] = false;
					theme_panel [i, j+1] = false;
					theme_panel [i, j+2] = false;
				}
				else if(judge==3){
					i = Random.Range (0, side_length);
					j = Random.Range (0, side_length);

					//凸型
					theme_panel [i+1, j+1] = false;
					theme_panel [i, j] = false;
					theme_panel [i, j+1] = false;
					theme_panel [i, j+2] = false;
				}
			}

		}
	}
}

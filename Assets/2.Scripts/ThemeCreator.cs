using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class ThemeCreator : MonoBehaviour {

	#region const

	const float UPPER_CAMERA_Z= 15.0f;

	#endregion


	#region public property

	public bool isRotate   { get; set; }	//折り紙を回転させるかのフラグ
	public bool isFinal { get; set; }		//最後の問題としてのフラグ
	public bool[,] theme_panel = new bool[PanelRoot.ORIGAMI_WIDTH + 2, PanelRoot.ORIGAMI_WIDTH + 2];
		
	#endregion


	#region private property

	private TextAsset _question;
	private bool isRandom;	//ランダム生成のフラグ
	[SerializeField]
	private GameObject panel;
	private float root_z;
	private int origami_width;

	#endregion


	#region event

	// Use this for initialization
	void Start () {
		isRotate = isFinal = isRandom = false;
		origami_width = PanelRoot.ORIGAMI_WIDTH;

		//折り紙オブジェクトをカメラの範囲に合うように移動
		Camera upper_camera = GameObject.Find ("Main Camera Thema").GetComponent<Camera> ();
		this.transform.position = upper_camera.ScreenToWorldPoint (
			new Vector3(Screen.width * 0.6f, Screen.height * 0.75f, (UPPER_CAMERA_Z + origami_width)));
		root_z = this.transform.position.z;
		//お題を作成するパネルの中心位置
		Vector3 center_pos = this.transform.position;

		LoadThemeData ();

		if(isRandom){ //ランダムにお題を生成する
			FetchRandomThemeData ();

		} else { //テキストデータからお題を生成する
			FetchThemeData ();
		}
	
		CreateTheme (center_pos);
	}

	#endregion


	#region private method

	private void LoadThemeData(){
		//Resourcesフォルダのテキストを読み込む
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
			isRotate = true;
			break;
		case 5:
			_question = Resources.Load ("q_5") as TextAsset;
			isRotate = true;
			break;
		case 6:
			_question = Resources.Load ("q_6") as TextAsset;
			isRotate = true;
			break;
		case 7:
			_question = Resources.Load ("q_7") as TextAsset;
			isRotate = true;
			break;
		case 8:
			_question = Resources.Load ("q_8") as TextAsset;
			isRotate = true;
			break;
		case 9:
			_question = Resources.Load ("q_9") as TextAsset;
			isRotate = true;
			break;
		case 10:
			_question = Resources.Load ("q_10") as TextAsset;
			isRotate = true;
			break;
		case 11:
		case 12:
		case 13:
			isRandom = true;
			break;
		default:
			isRandom = true;
			isRotate = true;
			break;
		}
	}

	private void CreateTheme (Vector3 _center_pos){
		int qi, i, j, qx = 1, qy = 1;

		//qiは象限を表す
		for(qi=0; qi<4; qi++){
			Vector3 temp_pos = _center_pos;

			switch(qi){
			case 0:
				qx = 1; qy = 1;
				break;
			case 1:
				qx = -1; qy = 1; _center_pos.x--;
				break;
			case 2:
				qx = -1; qy = -1; _center_pos.x--; _center_pos.y--;
				break;
			case 3:
				qx = 1; qy = -1; _center_pos.y--;
				break;
			default:
				break;
			}
				
			for(i=0; i<origami_width; i++){
				for(j=0; j<origami_width; j++){

					GameObject game_object = (
						Instantiate (panel, new Vector3 (_center_pos.x + (i*qx), _center_pos.y + (j*qy), root_z), Quaternion.identity)
						as GameObject);

					//お題のパネルと解答のパネル
					if(theme_panel[i,j] == false){
						Destroy (game_object);
					}
				}
			}
			_center_pos = temp_pos;
		}
	}


	//お題のデータを配列に格納
	void FetchThemeData(){
		//テキストデータを文字列として取り込む
		string question_texts = _question.text;

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
				if (j > origami_width){
					break;
				}
			}
			j = 0;
			i++;
			if (i > origami_width){
				break;
			}
		}
	}

	//ランダムにお題生成
	//お題のデータを配列に格納
	void FetchRandomThemeData(){
		int i=0, j=0, k=0;
		int judge;
		int count_judge;

		//一度全てtrueまたはfalseに設定
		judge = Random.Range (0, 2);

		if(judge == 0){
			for (i = 0; i < origami_width; i++) {
				for(j=0; j<origami_width; j++){
					theme_panel [i, j] = false;
				}
			}
			count_judge = Random.Range (12, 15);


			for (k = 0; k < count_judge; k++) {
				judge = Random.Range (0, 4);

				if (judge == 0) {
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//2*2の正方形
					theme_panel [i, j] = true;
					theme_panel [i, j + 1] = true;
					theme_panel [i + 1, j] = true;
					theme_panel [i + 1, j + 1] = true;
				}
				else if(judge == 1){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//鏡面Z型
					theme_panel [i, j] = true;
					theme_panel [i, j + 1] = true;
					theme_panel [i + 1, j+1] = true;
					theme_panel [i + 1, j + 2] = true;
				}
				else if(judge==2){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//Z型
					theme_panel [i+1, j] = true;
					theme_panel [i+1, j + 1] = true;
					theme_panel [i, j+1] = true;
					theme_panel [i, j+2] = true;
				}
				else if(judge==3){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//凸型
					theme_panel [i+1, j+1] = true;
					theme_panel [i, j] = true;
					theme_panel [i, j+1] = true;
					theme_panel [i, j+2] = true;
				}
			}
				
		} else {
			for (i = 0; i < origami_width; i++) {
				for(j=0; j<origami_width; j++){
					theme_panel [i, j] = true;
				}
			}
				
			count_judge = Random.Range (12, 15);

			for (k = 0; k < count_judge; k++) {
				judge = Random.Range (0, 4);

				if (judge == 0) {
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//2*2の正方形
					theme_panel [i, j] = false;
					theme_panel [i, j + 1] = false;
					theme_panel [i + 1, j] = false;
					theme_panel [i + 1, j + 1] = false;
				}
				else if(judge == 1){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//鏡面Z型
					theme_panel [i, j] = false;
					theme_panel [i, j + 1] = false;
					theme_panel [i + 1, j+1] = false;
					theme_panel [i + 1, j + 2] = false;
				}
				else if(judge==2){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//Z型
					theme_panel [i+1, j] = false;
					theme_panel [i+1, j + 1] = false;
					theme_panel [i, j+1] = false;
					theme_panel [i, j+2] = false;
				}
				else if(judge==3){
					i = Random.Range (0, origami_width);
					j = Random.Range (0, origami_width);

					//凸型
					theme_panel [i+1, j+1] = false;
					theme_panel [i, j] = false;
					theme_panel [i, j+1] = false;
					theme_panel [i, j+2] = false;
				}
			}

		}
	}

	#endregion
}

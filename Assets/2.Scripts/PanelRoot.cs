using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelRoot : MonoBehaviour {

	#region const, readonly

	const float LOWER_CAMERA_Z = 15.0f;
	const float ZOOM = 2.0f;
	const float ZOOM_SPEED = 10.0f;
	[SerializeField]
	public static readonly int ORIGAMI_WIDTH = 9;

	#endregion


	#region private property

	private ThemeCreator _theme_creator;
	public ThemeCreator theme_creator
	{
		get { 
			_theme_creator = _theme_creator ?? (GameObject.FindWithTag ("Root").GetComponent<ThemeCreator> ());
			return this._theme_creator; 
		}
	}
		
	Camera lower_camera;		//操作画面のカメラ

	// スコア表示
	[SerializeField]
	private Text score_txt;
	[SerializeField]
	private Text minus_point_txt;
	[SerializeField]
	private Text total_score_txt;
	[SerializeField]
	private Text stage_txt;
	[SerializeField]
	private GameObject panel;
	[SerializeField]
	private GameObject origami;

	Transform[] quadrant_panel = new Transform[4];
	Transform[] quadrant = new Transform[4];

	private int score = 100;
	private int demerit_point=0;

	//正解判定時の比較に用いる配列
	private PanelCtrl[,] panels = new PanelCtrl[ORIGAMI_WIDTH, ORIGAMI_WIDTH];

	[SerializeField]
	private Animator panel_animator;
	[SerializeField]
	private Animator text_animator;
	private int anim_type;	//アニメーションの種類を決める
	private bool isAnim;	//アニメーションが行われたかどうか

	// パネルをタッチしたかどうかの判定に用いる
	RaycastHit[] hits;
	private Ray worldPoint;

	private GameObject zoom_pos; //拡大時の中心となるオブジェクト
	private float center_posz;
	private bool isZoom = false;
	private Vector3 initial_pos;
	private bool isAnswered;	 //回答されているか判定

	#endregion


	#region event
	// Use this for initialization
	void Start () {
		SoundMgr.Instance.PlayBGM (SoundMgr.Instance.bgm_play); //プレイ時のBGM設定

		int i,j,k;
		isAnswered = false;
		text_animator.SetBool ("IsAnswered", false);
		stage_txt.text = "Stage" + SetValue.stage_num;

		//折り紙オブジェクトがカメラの範囲に合うように移動するために下画面のカメラを取得
		lower_camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();

		//ズームを戻すときのための初期位置
		origami.transform.position = lower_camera.ScreenToWorldPoint (
			new Vector3(Screen.width * 0.6f, Screen.height * 0.25f, (LOWER_CAMERA_Z + ORIGAMI_WIDTH)));
		initial_pos = origami.transform.position;
		center_posz = origami.transform.position.z;
	
		//第1~4象限のエリアに分ける //折りたたむアニメーションはエリアをアニメーションさせるため
		quadrant [0] = origami.transform.FindChild ("Area1").GetComponent<Transform>();
		quadrant [1] = origami.transform.FindChild ("Area2").GetComponent<Transform>();
		quadrant [2] = origami.transform.FindChild ("Area3").GetComponent<Transform>();
		quadrant [3] = origami.transform.FindChild ("Area4").GetComponent<Transform>();

		//各象限ごとにパネルをひとつずつ格納
		quadrant_panel[0] = quadrant[0].FindChild ("Panel1").GetComponent<Transform>();
		quadrant_panel[1] = quadrant[1].FindChild ("Panel2").GetComponent<Transform>();
		quadrant_panel[2] = quadrant[2].FindChild ("Panel3").GetComponent<Transform>();
		quadrant_panel[3] = quadrant[3].FindChild ("Panel4").GetComponent<Transform>();

		//areaをcenterの子にする
		for(i=0; i<4; i++){
			quadrant_panel [i].parent = quadrant [i];
		}

		CreatePanels ();

		score_txt.text = "Score:\r\n" + score;
		total_score_txt.text = "Total:" + SetValue.total_score;

		//アニメーション設定
		anim_type = 0;
		isAnim = false;
		//折り紙を回転させるフラグが立っていれば回転
		if(theme_creator.isRotate){
			OrigamiRotate ();
			isAnim = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
			// 切り取るパネルを検知
			if (Input.GetMouseButton (0)) {
				worldPoint = Camera.main.ScreenPointToRay (Input.mousePosition);
				hits = Physics.RaycastAll (worldPoint.origin, worldPoint.direction, 100);

				StartCoroutine("PanelDestroy");
			}
	}
		
	#endregion


	#region public method

	//アニメーション制御
	public void OrigamiRotate(){
		anim_type = Random.Range (1, 4);
		switch(anim_type){
		case 1:
			iTween.RotateTo (origami.gameObject, iTween.Hash ("z", -90, "islocal", true, "time",3));
			break;
		case 2:
			iTween.RotateTo (origami.gameObject, iTween.Hash ("z", -180, "islocal", true, "time",3));
			break;
		case 3:
			iTween.RotateTo (origami.gameObject, iTween.Hash ("z", -270, "islocal", true, "time",3));
			break;
		default:
			break;
		}
	}

	//折り紙を元の状態に戻し、次のシーンへ移行
	public void NextScene(){
		//呼び出しを一度だけ許す処理
		if(!isAnswered){
			StartCoroutine ("NextSc");
		}
		isAnswered = true;
	}

	//各象限ごとに拡大縮小 
	public void Zooming(){
		if (isZoom == false) {
			isZoom = true;	
			switch (anim_type) {
			case 0:
				iTween.MoveTo (origami.gameObject,
					lower_camera.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.90f, Screen.height * 0.05f, (ZOOM + ORIGAMI_WIDTH))), 1.0f);
				break;

			case 1:
				iTween.MoveTo (origami.gameObject,
					lower_camera.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.30f, Screen.height * 0.05f, (ZOOM + ORIGAMI_WIDTH))), 1.0f);
				break;

			case 2:
				iTween.MoveTo (origami.gameObject,
					lower_camera.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.30f, Screen.height * 0.40f, (ZOOM + ORIGAMI_WIDTH))), 1.0f);
				break;

			case 3:
				iTween.MoveTo (origami.gameObject,
					lower_camera.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.90f, Screen.height * 0.40f, (ZOOM + ORIGAMI_WIDTH))), 1.0f);
				break;

			default:
				break;
			}

		} else { //ズームされていた場合

			iTween.MoveTo (origami.gameObject, initial_pos, 1.0f);
			isZoom = false;
		}
	}

	//回答の時はカメラを元の位置に戻す
	public void ReturnPos(){
		if(isZoom == true){
			isZoom = false;
			iTween.MoveTo (origami.gameObject, initial_pos, 1.0f);
		}
	}

	//お題と解答の正答率を計算
	public void CalculateScore(){
		int i, j;

		//解答とお題をパネルの有無で照合
		for(i=0; i<ORIGAMI_WIDTH; i++){
			for(j=0;j<ORIGAMI_WIDTH;j++){
				if(theme_creator.theme_panel [i, j] != panels [i, j].isExist){
					demerit_point += 5;
				}
			}
		}
		score -= demerit_point;

		if(score < 0){ //持ち点がマイナスの場合は加算しない + 最終問題とする
			theme_creator.isFinal = true;
		} else {
			//スコア = （残り持ち点＋残り時間）＊　ステージ番号
			SetValue.total_score += (score + (int)SetValue.time) * SetValue.stage_num;
		}

		StartCoroutine ("DisplayScore");
	}

	//タイトル画面へ移行
	public void ToTitle(){
		Application.LoadLevel ("scTitle");
	}

	#endregion


	#region private method

	private void CreatePanels(){
		panels [0, 0] = quadrant [0].FindChild ("Panel1").GetComponent<PanelCtrl>();

		int qi, i, j, qx = 1, qy = 1;

		for(qi = 0; qi < 4; qi++){

			switch(qi){
			case 0:
				qx =  1; qy = 1;
				break;
			case 1:
				qx = -1; qy = 1;
				break;
			case 2:
				qx = -1; qy = -1;
				break;
			case 3:
				qx =  1; qy = -1;
				break;
			default:
				break;
			}

			for(i=0; i<ORIGAMI_WIDTH; i++){
				for(j=0; j<ORIGAMI_WIDTH; j++){
					if (i == 0 && j == 0)
						continue;

					//生成するパネルの位置 元から生成されている各象限ごとのパネルの位置を基に作成
					Vector3 panel_pos = new Vector3 (quadrant_panel [qi].position.x + (i * qx), quadrant_panel [qi].position.y + (j * qy), center_posz);

					GameObject temp_panel = (Instantiate (panel, panel_pos, Quaternion.identity) as GameObject);

					temp_panel.transform.parent = quadrant [qi];
					//拡大時の中心となるオブジェクトであれば
					if(qi == 0){
						if((i == ORIGAMI_WIDTH/2) && (j == ORIGAMI_WIDTH/2)){
							zoom_pos = temp_panel;
						}
						this.panels[i,j] = temp_panel.GetComponent<PanelCtrl>();
					}
				}
			}
		}
	}

	IEnumerator foldanim1(){
		iTween.RotateTo (transform.FindChild ("Area3").gameObject, iTween.Hash ("x", 180, "islocal", true, "time",1.5));
		iTween.RotateTo (transform.FindChild ("Area4").gameObject, iTween.Hash ("x", 180, "islocal", true, "time",1.5));
		yield return new WaitForSeconds(1.5f);
		iTween.RotateTo (transform.FindChild ("Area1").gameObject, iTween.Hash ("y", 180, "islocal", true, "time",2));
		iTween.RotateAdd (transform.FindChild ("Area4").gameObject, iTween.Hash ("y", -180, "islocal", true, "time",2));
	}

	IEnumerator NextSc(){
		SetValue.stage_num++;
		//アニメーションが実行されていれば、実行前の状態に戻す
		if(isAnim){
			iTween.RotateTo (origami.gameObject, iTween.Hash ("z", 0, "islocal", true, "time",3));
		} 

		yield return new WaitForSeconds(5.0f);
		SetValue.time = 30;

		//最後の問題であればスコア画面に移動
		if(theme_creator.isFinal){
			Application.LoadLevel ("scScore");
		} else {
			Application.LoadLevel ("scPlay1");
		}
	}


	IEnumerator DisplayScore(){
		AnimatorStateInfo state = panel_animator.GetCurrentAnimatorStateInfo (0);
		//パネルを展開する場合はスコアを表示するのに時間をかける
		if (state.nameHash == Animator.StringToHash("Base Layer.fold_anim2") ){
			panel_animator.SetBool ("IsPush", true);
			yield return new WaitForSeconds (2.0f);

		}
		minus_point_txt.text = "-" + demerit_point;
		demerit_point = 0;
		text_animator.SetBool ("IsAnswered", true);	//減点を表示するアニメーション
		yield return new WaitForSeconds (1.0f);
		text_animator.SetBool ("IsAnswered", false);
		score_txt.text = "Score:\r\n" + score;
		total_score_txt.text = "Total : " + SetValue.total_score;
	}


	IEnumerator PanelDestroy(){

		foreach (RaycastHit hit in hits) {
			hit.collider.GetComponent<PanelCtrl> ().isExist = false;
			//パネル破壊音
			SoundMgr.Instance.PlayClip (SoundMgr.Instance.slash);
			//パネル破壊
			Destroy (hit.collider.gameObject);
		}
		yield return null;
	}

	#endregion

}
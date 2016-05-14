using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelRoot : MonoBehaviour {
	const float CAMERA1_Z = 15.0f;
	const float ZOOM = 2.0f;
	const float ZOOM_SPEED = 10.0f;

	//CreateThemeクラスの変数を扱うためのもの
	CreateTheme create_theme;

	// スコア表示
	public Text scoreText;
	public Text minuspointText;
	public Text total_scoreText;
	public Text stageText;

	public GameObject panel;

	Transform[] _panel = new Transform[4];
	Transform[] _area = new Transform[4];
	public int side_length;
	public int score = 100;
	private int demerit_point=0;

	//正解判定時の比較に用いる配列
	public PanelCtrl[,] panels;     //サイズは後で指定　blocks[x,y]て感じ
	private int i,j,k;

	//アニメーションの種類を決める
	public int rand;
	//アニメーションフラグ
	public bool animation_flag;

	private Animator _animator;
	public Animator _animator_text;
	RaycastHit[] hits;
	private Ray worldPoint;

	//拡大時の中心となるオブジェクト
	public GameObject zoom_pos;
	//折り紙オブジェクトを
	private Transform center_pos;
	private float root1_z;

	private bool isZoom = false;
	public Vector3 initial_pos;
	Camera camera1;
	//次のシーン
	public string scplay;
	//回答されているか判定
	public bool isAnswered;


	// Use this for initialization
	void Start () {
		isAnswered = false;
		create_theme = GameObject.Find ("RootTheme").GetComponent<CreateTheme> ();
		_animator = this.GetComponent<Animator> ();
		_animator_text.SetBool ("IsAnswered", false);
		stageText.text = "Stage" + SetValue.stage_num;

		//折り紙オブジェクトをカメラの範囲に合うように移動
		camera1 = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		this.transform.position = camera1.ScreenToWorldPoint (
			new Vector3(Screen.width * 0.6f, Screen.height * 0.25f, (CAMERA1_Z+ side_length)));
		initial_pos = this.transform.position;
		root1_z = this.transform.position.z;
	
		_area [0] = transform.FindChild ("Area1").GetComponent<Transform>();
		_area [1] = transform.FindChild ("Area2").GetComponent<Transform>();
		_area [2] = transform.FindChild ("Area3").GetComponent<Transform>();
		_area [3] = transform.FindChild ("Area4").GetComponent<Transform>();

		_panel[0] = _area[0].FindChild ("Panel1").GetComponent<Transform>();
		_panel[1] = _area[1].FindChild ("Panel2").GetComponent<Transform>();
		_panel[2] = _area[2].FindChild ("Panel3").GetComponent<Transform>();
		_panel[3] = _area[3].FindChild ("Panel4").GetComponent<Transform>();

		//areaをcenterの子にする
		for(i=0; i<4; i++){
			_panel [i].parent = _area [i];
		}

		this.panels = 
			new PanelCtrl[Panel.PANEL_NUM_X, Panel.PANEL_NUM_Y];

		//
		panels [0, 0] = _area [0].FindChild ("Panel1").GetComponent<PanelCtrl>();
		
		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){
				if (i == 0 && j == 0)
					continue;

				GameObject game_object = (Instantiate (
					panel, new Vector3 (_panel [0].position.x + i, _panel [0].position.y + j, root1_z), Quaternion.identity)
						as GameObject);
				//拡大時の中心となるオブジェクトであれば
				if(i == side_length/2 && j == side_length/2){
					zoom_pos = game_object;
				}
				game_object.transform.parent = _area [0];
				PanelCtrl _Panel = game_object.GetComponent<PanelCtrl>();
				//
				this.panels[i,j] = _Panel;
			}
		}

		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){
				if (i == 0 && j == 0)
					continue;
				
				(Instantiate (panel, new Vector3 (_panel [1].position.x - i, _panel [1].position.y + j, root1_z), Quaternion.identity)
					as GameObject).transform.parent = _area [1];
			}
		}

		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){
				if (i == 0 && j == 0)
					continue;
				
				(Instantiate (panel, new Vector3 (_panel [2].position.x - i, _panel [2].position.y - j, root1_z), Quaternion.identity)
					as GameObject).transform.parent = _area [2];
			}
		}

		for(i=0; i<side_length; i++){
			for(j=0; j<side_length; j++){
				if (i == 0 && j == 0)
					continue;
				
				(Instantiate (panel, new Vector3 (_panel [3].position.x + i, _panel [3].position.y - j, root1_z), Quaternion.identity)
					as GameObject).transform.parent = _area [3];
			}
		}

		// iTweenで折りたたむアニメーションを実現
		//StartCoroutine("foldanim1");

		scoreText.text = "Score:\r\n" + score;
		total_scoreText.text = "Total:" + SetValue.total_score;

		//アニメーション設定
		rand = 0;
		animation_flag = false;
		//折り紙を回転させるフラグが立っていれば回転
		if(create_theme.rotate_flag){
			OrigamiAnim ();
		}

	}

	IEnumerator foldanim1(){
		iTween.RotateTo (transform.FindChild ("Area3").gameObject, iTween.Hash ("x", 180, "islocal", true, "time",1.5));
		iTween.RotateTo (transform.FindChild ("Area4").gameObject, iTween.Hash ("x", 180, "islocal", true, "time",1.5));
		yield return new WaitForSeconds(1.5f);
		iTween.RotateTo (transform.FindChild ("Area1").gameObject, iTween.Hash ("y", 180, "islocal", true, "time",2));
		iTween.RotateAdd (transform.FindChild ("Area4").gameObject, iTween.Hash ("y", -180, "islocal", true, "time",2));
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
		

	//アニメーション制御
	public void OrigamiAnim(){
		_animator = this.GetComponent<Animator> ();
		rand = Random.Range (1, 4);
		animation_flag = true;
		switch(rand){
		case 1:
			iTween.RotateTo (this.gameObject, iTween.Hash ("z", -90, "islocal", true, "time",3));
			break;
		case 2:
			iTween.RotateTo (this.gameObject, iTween.Hash ("z", -180, "islocal", true, "time",3));
			break;
		case 3:
			iTween.RotateTo (this.gameObject, iTween.Hash ("z", -270, "islocal", true, "time",3));
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

	//
	IEnumerator NextSc(){
		SetValue.stage_num++;
		//アニメーションが実行されていれば、実行前の状態に戻す
		if(animation_flag){
			iTween.RotateTo (this.gameObject, iTween.Hash ("z", 0, "islocal", true, "time",3));
		} 

		yield return new WaitForSeconds(5.0f);
		SetValue.time = 30;

		//最後の問題であればスコア画面に移動
		if(create_theme.final_flag){
			Application.LoadLevel ("scScore");
		} else {
			Application.LoadLevel (scplay);
		}
	}


	//拡大縮小 各象限ごとに
	public void Zooming(){
		if (isZoom == false) {
			isZoom = true;	
			switch (rand) {
			case 0:
				iTween.MoveTo (this.gameObject,
					camera1.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.90f, Screen.height * 0.05f, (ZOOM + side_length))),
					1.0f);
				break;

			case 1:
				iTween.MoveTo (this.gameObject,
					camera1.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.30f, Screen.height * 0.05f, (ZOOM + side_length))),
					1.0f);
				break;

			case 2:
				iTween.MoveTo (this.gameObject,
					camera1.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.30f, Screen.height * 0.40f, (ZOOM + side_length))),
					1.0f);
				break;

			case 3:
				iTween.MoveTo (this.gameObject,
					camera1.ScreenToWorldPoint (
						new Vector3 (Screen.width * 0.90f, Screen.height * 0.40f, (ZOOM + side_length))),
					1.0f);
				break;

			default:
				break;
			}
		} else {
			iTween.MoveTo (this.gameObject, initial_pos, 1.0f);
			isZoom = false;
		}
	}

	//回答の時はカメラを元の位置に戻す
	public void Return_pos(){
		if(isZoom == true){
			isZoom = false;
			iTween.MoveTo (this.gameObject, initial_pos, 1.0f);
		}
	}
		
	//お題と解答の正答率を計算
	public void CalculateScore(){
		int i, j;
	
		//解答とお題を照合
		for(i=0;i<side_length;i++){
			for(j=0;j<side_length;j++){
				if(create_theme.theme_panel [i, j] != panels [i, j]){
					demerit_point += 5;
				}
			}
		}
		score -= demerit_point;

		//持ち点がマイナスの場合は加算しない + 最終問題とする
		if(score < 0){
			create_theme.final_flag = true;
		}
		else {
			//スコア = （残り持ち点＋残り時間）＊　ステージ番号
			SetValue.total_score += (score + (int)SetValue.time) * SetValue.stage_num;
		}

		StartCoroutine ("DisplayScore");
	}

	IEnumerator DisplayScore(){
		AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo (0);
		//パネルを展開する場合はスコアを表示するのに時間をかける
		if (state.nameHash == Animator.StringToHash("Base Layer.fold_anim2") ){
			_animator.SetBool ("IsPush", true);
			yield return new WaitForSeconds (2.0f);
//		} else {
//			yield return new WaitForSeconds (1.0f);
		}
		minuspointText.text = "-" + demerit_point;
		demerit_point = 0;
		//減点を表示するアニメーション
		_animator_text.SetBool ("IsAnswered", true);
		//少し待ってからスコア表示
		yield return new WaitForSeconds (1.0f);
		_animator_text.SetBool ("IsAnswered", false);
		scoreText.text = "Score:\r\n" + score;
		total_scoreText.text = "Total : " + SetValue.total_score;
	}


	public void FoldOrUnfold(){
		//SetValue.time = 100;
		StartCoroutine ("PanelAnim");
	}
	//
	IEnumerator PanelAnim(){
		//アニメーションの状態を取得
		AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo (0);
		if (state.nameHash == Animator.StringToHash("Base Layer.fold_anim2") ){
			//パネルを展開する
			_animator.SetBool ("IsPush", true);
			//紙を開いたら減点
			score -= 10;
			minuspointText.text = "-" + 10;
			//減点を表示するアニメーション
			_animator_text.SetBool ("IsAnswered", true);
			yield return new WaitForSeconds (1.0f);
			_animator_text.SetBool ("IsAnswered", false);
			scoreText.text = "SCORE : " + score;
		} else {
			//パネルを折りたたむ
			_animator.SetBool ("IsPush", false);
		}
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

	//タイトル画面へ移行
	public void ToTitle(){
		Application.LoadLevel ("scTitle");
	}
}
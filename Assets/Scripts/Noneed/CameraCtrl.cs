using UnityEngine;
using System.Collections;

public class CameraCtrl: MonoBehaviour {
	const float ZOOM = -10.0f;
	private Animator _animator;
	public int rand;
	//アニメーションフラグ
	public bool animation_flag;
	SoundMgr sound_mgr;
	public AudioClip clip;


	// Use this for initialization
	void Start () {
		rand = 0;
		animation_flag = false;
		switch(SetValue.stage_num){
		case 1:
		//	rand = Random.Range (1, 4);
			rand = 1;
			_animator = this.GetComponent<Animator> ();
			CameraAnim ();
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
//		float fWheel = Input.GetAxis("Mouse ScrollWheel"); 
//		if(fWheel != 0.0f){
//			transform.Translate(Input.mousePosition.x * 0.1f, Input.mousePosition.y * 0.1f, fWheel);
//		}
//		transform.Translate(-(fWheel/3), (fWheel/4), fWheel);
	}

	public void CameraAnim(){
		animation_flag = true;
 		//ランダムでアニメーションを実行
//		if(rand == 1){
//			_animator.SetBool ("IsRotate0", true);
//		} else if(rand == 2){
//			_animator.SetBool ("IsRotate1", true);
//		} else if(rand == 3){
//			_animator.SetBool ("IsRotate2", true);
//		}

		switch(rand){
		case 1:
		//	iTween.MoveTo (this.gameObject, iTween.Hash ("x", 3, "time", 3));
			iTween.RotateTo (this.gameObject, iTween.Hash ("z", 90, "islocal", false));
			break;
		case 2:
			break;
		case 3:
			break;
		default:
			break;
		}
	}
		
}


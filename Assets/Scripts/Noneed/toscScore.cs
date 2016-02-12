////using UnityEngine;
////using UnityEngine.UI;
////using System.Collections;
////
////public class toscScore : MonoBehaviour {
////
////	public string scplay;
////	public GameObject timeText;
////	TimeCtrl time_ctrl;
////	PanelRoot panel_root;
////
////	//AudioSource touch_sound;
////
////
////	// Use this for initialization
////	void Start () {
////		//touch_sound = GameObject.Find ("TouchSound").GetComponent<AudioSource>();
////		//print ("a");
////		time_ctrl = timeText.GetComponent<TimeCtrl> ();
////		panel_root = GameObject.Find ("Center").GetComponent<PanelRoot> ();
////	}
////	
////	// Update is called once per frame
////	void Update () {
////	
////	}
////
////	public void OnClick(){
////		time_ctrl.Stop ();
////		//touch_sound.Play ();
////		panel_root.CalculateScore ();
////		//print (PanelRoot.score);
////		StartCoroutine ("NextScene");
////	}
////
////	IEnumerator NextScene(){
////		yield return new WaitForSeconds(6.0f);
////		Application.LoadLevel (scplay);
////	}
////}
//
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//
//public class toscScore : MonoBehaviour {
//
//	private Animator camera_anim;
//	public string scplay;
//	public GameObject timeText;
//	TimeCtrl time_ctrl;
//	PanelRoot panel_root;
//
//	//AudioSource touch_sound;
//
//
//	// Use this for initialization
//	void Start () {
//		camera_anim = GameObject.Find ("Main Camera").GetComponent<Animator> ();
//		time_ctrl = timeText.GetComponent<TimeCtrl> ();
//		panel_root = GameObject.Find ("Center").GetComponent<PanelRoot> ();
//	}
//
//	// Update is called once per frame
//	void Update () {
//
//	}
//
//	public void OnClick(){
//		time_ctrl.Stop ();
//
//		//touch_sound.Play ();
//		panel_root.CalculateScore ();
//		//print (PanelRoot.score);
//		if(SetValue.stage_num == 3){
//			SetValue.stage_num++;
//			StartCoroutine ("NextScene2");
//		} else {
//			SetValue.stage_num++;
//			StartCoroutine ("NextScene");
//		}
//	}
//
//	IEnumerator NextScene(){
//		yield return new WaitForSeconds(6.0f);
//		SetValue.time = 30;
//		Application.LoadLevel (scplay);
//	}
//
//	IEnumerator NextScene2(){
//		camera_anim.speed = -1.0f;
//		camera_anim.SetBool ("IsRotate0", false);
//		yield return new WaitForSeconds(6.0f);
//		SetValue.time = 30;
//		Application.LoadLevel (scplay);
//	}
//}

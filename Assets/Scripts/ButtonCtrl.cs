using UnityEngine;
using System.Collections;

public class ButtonCtrl : MonoBehaviour {

	SoundMgr sound_mgr;
	public AudioClip clip;
	public string scplay;

	private PanelRoot panel_root;
	public GameObject timeText;
	TimeCtrl time_ctrl;

	// Use this for initialization
	void Start () {
		sound_mgr = GameObject.Find ("SoundMgr").GetComponent<SoundMgr> ();
		if(timeText){
			time_ctrl = timeText.GetComponent<TimeCtrl> ();
		}
		if(GameObject.Find ("Center")){
			panel_root = GameObject.Find ("Center").GetComponent<PanelRoot> ();
		}
	}

	//ログイン画面へ移行
	public void ToLogIn(){
		Application.LoadLevel ("LogIn");
	}

	public void ToLeaderBoard(){
		Application.LoadLevel ("LeaderBoard");
	}

	public void OnClick(){
		StartCoroutine ("NextScene");
	}

	IEnumerator NextScene(){
		sound_mgr.PlayClip (clip);
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel (scplay);
	}


	public void Answer(){
		sound_mgr.PlayClip (clip);
		time_ctrl.Stop ();
		panel_root.CalculateScore ();
		panel_root.NextScene ();
	}
}

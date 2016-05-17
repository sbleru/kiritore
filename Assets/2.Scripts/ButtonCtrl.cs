using UnityEngine;
using System.Collections;

public class ButtonCtrl : MonoBehaviour {

	#region private property

	private PanelRoot _panel_root;
	private PanelRoot panel_root
	{
		get { 
			_panel_root = _panel_root ?? (GameObject.FindWithTag ("Root").GetComponent<PanelRoot> ());
			return this._panel_root; 
		}
	}

	private TimeCtrl _time_ctrl;
	public TimeCtrl time_ctrl
	{
		get { 
			_time_ctrl = _time_ctrl ?? (GameObject.FindWithTag ("Root").GetComponent<TimeCtrl> ());
			return this._time_ctrl; 
		}
	}

	[SerializeField]
	private string scplay;
	[SerializeField]
	private GameObject timeText;

	#endregion


	#region public method

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

	public void Answer(){
		SoundMgr.Instance.PlayClip (SoundMgr.Instance.touch);
		time_ctrl.StopTime();
		panel_root.CalculateScore ();
		panel_root.NextScene ();
	}

	#endregion


	#region private method

	IEnumerator NextScene(){
		SoundMgr.Instance.PlayClip (SoundMgr.Instance.touch);
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel (scplay);
	}

	#endregion

}

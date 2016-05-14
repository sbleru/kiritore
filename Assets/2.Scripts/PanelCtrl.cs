using UnityEngine;
using System.Collections;

public class Panel{
	public static int PANEL_NUM_X = 10; //ブロックを配置できるX方向の最大数
	public static int PANEL_NUM_Y = 10; //ブロックを配置できるy方向の最大数
}

public class PanelCtrl : MonoBehaviour {
	//
	AudioSource sound;
	public bool isExist;

	void Awake(){
		sound = GetComponent<AudioSource> ();
		isExist = true;
	}

	public void PlaySound(){
		sound.Play ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

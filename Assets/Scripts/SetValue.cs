using UnityEngine;
using System.Collections;

public class SetValue : MonoBehaviour {
	public static int total_score;
	public static int stage_num;
	public static float time;

	// Use this for initialization
	void Start () {
		SetValue.total_score = 0;
		SetValue.stage_num = 1;
		SetValue.time = 30;
	}
}

using UnityEngine;
using System.Collections;

public class PanelCtrl : MonoBehaviour {

	#region public property
	public bool isExist { get; set;}
	#endregion

	#region event
	void Awake(){
		isExist = true;
	}
	#endregion
}

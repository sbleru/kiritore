using NCMB;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LoginTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Login(){
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("TestClass");
		query.WhereEqualTo ("message", "Hello, Tarou!");
		query.FindAsync ((List<NCMBObject> objectList,NCMBException e) => {
			if (objectList.Count != 0) {
				NCMBObject obj = objectList [0];
				Debug.Log ("message : " + obj ["message"]);
			} else {
				NCMBObject testClass = new NCMBObject ("TestClass");
				testClass ["message"] = "Hello, NCMB!";
				testClass.SaveAsync ();
			}
		});
	}
}

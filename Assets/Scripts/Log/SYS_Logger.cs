using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Logger : MonoBehaviour {
	public static SYS_Logger Direct;
	public Text systemMsg;

	public bool logging = true;
	// Start is called before the first frame update

	void Awake() {
		Direct = this;
	}

	public void SetSystemMsg(string msg){
		systemMsg.text = msg;
	}

}

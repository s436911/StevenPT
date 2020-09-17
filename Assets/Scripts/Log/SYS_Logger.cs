using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Logger : MonoBehaviour {
	public static SYS_Logger Direct;
	public bool logging = true;
	// Start is called before the first frame update

	void Awake() {
		Direct = this;
	}
}

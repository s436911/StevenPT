using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_GameEngine : MonoBehaviour
{
	public static SYS_GameEngine Direct;

	void Awake() {
		Direct = this;
	}


	public void SetPause(bool value = true) {
		Time.timeScale = value ? 0 : 1;
	}
}

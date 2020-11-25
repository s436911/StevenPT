using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_GameEngine : MonoBehaviour
{
	public static SYS_GameEngine Direct;
	public GameObject backOff;

	void Awake() {
		Direct = this;
	}


	public void SetPause(bool value = true) {
		if (value) {
			Time.timeScale = 0;
			backOff.SetActive(true);
		} else {
			Time.timeScale = 1;
			backOff.SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour {
	public float speed = 5.0f;
	private bool touchStart = false;
	private Vector2 pointA;
	private Vector2 pointB;

	public RectTransform circle;
	public RectTransform outerCircle;
	public bool draging = false;

	private Vector2 ratio;

	// Update is called once per frame
	void Awake() {
		ratio = new Vector2((float)720 / Screen.width, (float)1280 / Screen.height);
		Debug.Log(ratio);
		Debug.Log(Screen.height + "Screen Width : " + Screen.width);
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (Input.GetMouseButtonDown(0)) {
				pointA = Input.mousePosition * ratio;
				pointB = pointA;
				if (pointA.y < 1180 && pointA.y > 100) {
					circle.anchoredPosition = pointA;
					outerCircle.anchoredPosition = pointA;
					circle.GetComponent<Image>().enabled = true;
					outerCircle.GetComponent<Image>().enabled = true;
				}
			}

			if (Input.GetMouseButton(0)) {
				if (!draging && pointA != pointB) {
					draging = true;
					SYS_ShipController.Direct.BeginMove(1);
				}

				touchStart = true;
				pointB = Input.mousePosition * ratio;

			} else {
				draging = false;
				touchStart = false;
			}

			if (Input.GetMouseButtonUp(0)) {
				SYS_ShipController.Direct.EndMove(1);
			}
		} else {
			draging = false;
			touchStart = false;
		}
	}

	private void FixedUpdate() {
		if (touchStart) {
			Vector2 offset = (pointB - pointA).normalized;
			SYS_ShipController.Direct.UpdateDirection(1,offset);
			circle.anchoredPosition = pointA + offset * 65;

		} else {
			circle.GetComponent<Image>().enabled = false;
			outerCircle.GetComponent<Image>().enabled = false;
		}

	}
}
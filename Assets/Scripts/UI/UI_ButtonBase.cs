using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonBase : MonoBehaviour {
	public int id = 0;
	public Text textMain;
	public Text textSub;
	public RawImage imageIcon;
	public RawImage imageIcon2;
	public GameObject clicker;

	public void Init(int id) {
		this.id = id;
	}

	public void Regist(string value , string valueSub = "") {
		textMain.text = value;
		if (textSub) {
			textSub.text = valueSub;
		}
	}

	public void Regist(Texture2D texture , string value, string valueSub = "") {
		imageIcon.texture = texture;
		if (imageIcon2) {
			imageIcon2.texture = texture;
		}
		Regist(value, valueSub);
	}


	public void Click() {
		clicker.SetActive(true);
	}

	public void DeClick() {
		clicker.SetActive(false);
	}

	public void Reset() {
		if (imageIcon) {
			imageIcon.texture = null;
		}

		textMain.text = "---";
		if (textSub) {
			textSub.text = "--";
		}

		clicker.SetActive(false);
	}
}

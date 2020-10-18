using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonBase : MonoBehaviour {
	public Text textMain;
	public Text textSub;
	public RawImage imageIcon;
	public GameObject clicker;

	public void Regist(string value , string valueSub = "") {
		textMain.text = value;
		if (textSub) {
			textSub.text = valueSub;
		}
	}

	public void Regist(Texture texture , string value, string valueSub = "") {
		imageIcon.texture = texture;
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

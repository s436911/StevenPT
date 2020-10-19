using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SideLog : MonoBehaviour {
	public RectTransform rect;
	public Image back;
	public RawImage icon;
	public Text text;
	public float popTime = 0;
	public float startY = 0;

	void Update() {
		if (Time.unscaledTime - popTime <= SYS_SideLog.Direct.duration) {
			rect.anchoredPosition = new Vector2(0, startY + SYS_SideLog.Direct.animCurve.Evaluate(Time.unscaledTime - popTime));

			float a = SYS_SideLog.Direct.alphaCurve.Evaluate(Time.unscaledTime - popTime);
			back.color = new Color(back.color.r, back.color.g, back.color.b, a);
			icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, a);
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
		} else {
			Destroy(gameObject);
		}
	}

	public void Regist(float startY , Texture2D icon , string text) {
		this.startY = startY;
		this.icon.texture = icon;
		this.text.text = text;
		this.popTime = Time.unscaledTime;
	}
}

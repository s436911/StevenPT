using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_SideLog : MonoBehaviour {
	public static SYS_SideLog Direct;

	public List<Texture2D> resourceIcon = new List<Texture2D>();
	public GameObject logObj;
	public Transform uiPanel;
	public AnimationCurve animCurve;
	public AnimationCurve alphaCurve;
	public float duration = 3;
	public int maxPopNum = 5;

	void Awake() {
		Direct = this;
	}

	public void Reset() {
		foreach (Transform log in uiPanel) {
			Destroy(log.gameObject);
		}
	}

	public void Regist(int typeId, string text) {
		if (uiPanel.childCount < maxPopNum) {
			UI_SideLog objGen = Instantiate(logObj).GetComponent<UI_SideLog>();
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(uiPanel);
			objGen.transform.localScale = Vector2.one;
			objRect.anchoredPosition = objRect.anchoredPosition + new Vector2(0, 0);
			objGen.Regist(uiPanel.childCount * -20 ,resourceIcon[typeId], text);
		}
	}
}

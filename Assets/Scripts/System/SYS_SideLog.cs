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

	public Color colorPosi;
	public Color colorNorm;
	public Color colorNega;

	void Awake() {
		Direct = this;
	}

	public void Reset() {
		foreach (Transform log in uiPanel) {
			Destroy(log.gameObject);
		}
	}

	public void Regist(int typeId, int num, float size = 1, sbyte type = 10) {
		Regist(resourceIcon[typeId], num , size, type);
	}

	public void Regist(Texture2D texture, int num , float size = 1, sbyte type = 10) {
		if (uiPanel.childCount < maxPopNum) {
			if (type == 10) {
				if (num == 0) {
					type = 0;

				} else if (num == 1) {
					type = 1;

				} else if (num == -1) {
					type = -1;
				}
			}

			UI_SideLog objGen = Instantiate(logObj).GetComponent<UI_SideLog>();
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(uiPanel);
			objGen.transform.localScale = Vector2.one;
			objRect.anchoredPosition = objRect.anchoredPosition + new Vector2(0, 0);

			if (type == 0) {
				objGen.Regist(uiPanel.childCount * -60, texture, num.ToString() , size, colorNorm);

			} else if (type == 1) {
				objGen.Regist(uiPanel.childCount * -60, texture, num.ToString(), size, colorPosi);

			} else if (type == -1) {
				objGen.Regist(uiPanel.childCount * -60, texture, num.ToString(), size, colorNega);
			}
		}
	}
}

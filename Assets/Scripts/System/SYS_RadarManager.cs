using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_RadarManager : MonoBehaviour {
	public static SYS_RadarManager Direct;
	public Transform uiPanel;
	public GameObject hintObj;
	public Dictionary<Transform, RectTransform> hintsSide = new Dictionary<Transform, RectTransform>();

	public float detectDisT;

	public float radius = 150;

	public Transform entityPanel;
	public Transform meteorPanel;

	void Awake() {
		Direct = this;
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			float tmpDis;
			SpaceEntity tmpEntity;

			//showEntity
			foreach (Transform trans in entityPanel) {
				tmpDis = Vector2.Distance(trans.position, SYS_ShipController.Direct.transform.position);
				tmpEntity = trans.GetComponent<SpaceEntity>();

				if (tmpDis <= detectDisT * 4) {
					tmpDis = tmpDis / (detectDisT * 4);

					if (hintsSide.ContainsKey(trans)) {
						hintsSide[trans].anchoredPosition = (Vector2)(trans.position - SYS_ShipController.Direct.transform.position).normalized * radius * tmpDis;

					} else {
						RawImage objGen = Instantiate(hintObj).GetComponent<RawImage>();
						objGen.color = new Color(1, 0.85f, 0.3f, 0.5f);
						//objGen.color = new Color(0.5f, 0.9f, 1, 0.5f);

						RectTransform objRect = objGen.GetComponent<RectTransform>();
						objGen.transform.SetParent(uiPanel);
						objRect.anchoredPosition = (Vector2)(trans.position - SYS_ShipController.Direct.transform.position).normalized * radius * tmpDis;
						objRect.sizeDelta = new Vector2(35, 35);
						hintsSide.Add(trans, objRect);
					}

				} else {
					if (hintsSide.ContainsKey(trans)) {
						RectTransform tmpRect = hintsSide[trans];
						hintsSide.Remove(trans);
						Destroy(tmpRect.gameObject);
					}
				}
			}
					   
			//showMeteor
			foreach (Transform trans in meteorPanel) {
				tmpDis = Vector2.Distance(trans.position, SYS_ShipController.Direct.transform.position);
				tmpEntity = trans.GetComponent<SpaceEntity>();

				if (tmpDis <= detectDisT * 4) {
					tmpDis = tmpDis / (detectDisT * 4);

					if (hintsSide.ContainsKey(trans)) {
						hintsSide[trans].anchoredPosition = (Vector2)(trans.position - SYS_ShipController.Direct.transform.position).normalized * radius * tmpDis;

					} else {
						RawImage objGen = Instantiate(hintObj).GetComponent<RawImage>();
						objGen.color = new Color(1, 0.15f, 0, 0.5f);

						RectTransform objRect = objGen.GetComponent<RectTransform>();
						objGen.transform.SetParent(uiPanel);
						objRect.anchoredPosition = (Vector2)(trans.position - SYS_ShipController.Direct.transform.position).normalized * radius * tmpDis;
						objRect.sizeDelta = new Vector2(20, 20);
						hintsSide.Add(trans, objRect);
					}

				} else {
					if (hintsSide.ContainsKey(trans)) {
						RectTransform tmpRect = hintsSide[trans];
						hintsSide.Remove(trans);
						Destroy(tmpRect.gameObject);
					}
				}
			}
		}
	}

	public void TurnActiveStatus() {
		uiPanel.gameObject.SetActive(!uiPanel.gameObject.activeSelf);
		Reset();
	}

	public void Reset() {
		foreach (Transform child in uiPanel) {
			Destroy(child.gameObject);
		}
		hintsSide = new Dictionary<Transform, RectTransform>();
	}
}

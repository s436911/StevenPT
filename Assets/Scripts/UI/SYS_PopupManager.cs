using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_PopupManager : MonoBehaviour
{
	public static SYS_PopupManager Direct;
	public GameObject popObj;
	public RectTransform spawnPos;
	public List<RectTransform> pops = new List<RectTransform>();
	public float offsetRand;
	public float endPosX;
	public float speed;

	public int maxPopNum = 5;
	public float timer;

	void Awake() {
		Direct = this;
	}
	
	public void Regist(string from , string msg) {
		if (pops.Count < maxPopNum) {
			Text objGen = Instantiate(popObj).GetComponent<Text>();
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(transform);
			objRect.anchoredPosition = spawnPos.anchoredPosition + new Vector2(0, Random.Range(-offsetRand, offsetRand));

			objGen.text = from + ":" + msg;
			objRect.localScale = Vector3.one;
			pops.Add(objRect);
		}
	}

	public void Reset() {
		for (int ct = 0; ct < pops.Count; ct++) {
			Destroy(pops[ct].gameObject);
		}

		pops = new List<RectTransform>();
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			List<RectTransform> destroys = new List<RectTransform>();

			foreach (RectTransform pop in pops) {
				if (pop.anchoredPosition.x < endPosX) {
					destroys.Add(pop);
				} else {
					pop.anchoredPosition = pop.anchoredPosition - new Vector2(speed * Time.unscaledDeltaTime, 0);
				}
			}

			for (int ct = 0; ct < destroys.Count; ct++) {
				pops.Remove(destroys[ct]);
				Destroy(destroys[ct].gameObject);
			}

			if (Time.unscaledTime - timer > 10) {
				timer = Time.unscaledTime;
				if (SYS_ResourseManager.Direct.GetResource(2) < 10) {
					Regist("Steven", "我好餓喔..");
				}

				if (SYS_ResourseManager.Direct.GetResource(0) < 15) {
					Regist("CINDY", "好像快沒油了..");
				}
			}
		}
	}
}

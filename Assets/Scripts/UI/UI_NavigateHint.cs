using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NavigateHint : MonoBehaviour {
	public RectTransform rect;
	public Image board;
	public Text textType;
	public Text textDis;
	public Image star;
	public PlanetEntity entity;
	public bool navigating = false;


	void Awake() {
		rect = this.GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update() {
		if (entity.info.sType == StarType.Check) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			if (UI_Navigator.Direct.nextPlanet == entity && distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();
				UpdateHint();

			} else if (distance < UI_Navigator.Direct.detectDis && distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();
				UpdateHint();

			} else {
				CloseHint();
			}
		} else if (entity.info.sType == StarType.End ) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			if (distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();

			}  else {
				CloseHint();
			}
		} 
	}

	public void Regist(PlanetEntity entity) {
		this.entity = entity;
		UpdateHint();
	}

	public void ShowHint() {
		board.gameObject.SetActive(true);
		star.gameObject.SetActive(true);
		textType.gameObject.SetActive(true);
		textDis.gameObject.SetActive(true);
	}

	public void CloseHint() {
		board.gameObject.SetActive(false);
		star.gameObject.SetActive(false);
		textType.gameObject.SetActive(false);
		textDis.gameObject.SetActive(false);
	}

	private void UpdateHint() {
		Color hintColor = Color.white;

		if (entity.info.sType == StarType.Check) {
			if (UI_Navigator.Direct.nextPlanet != entity) {
				if (!entity.explored) {
					hintColor = new Color(0.85F, 0.85F, 0.85F);
					star.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
					textType.text = "???";
					textDis.text = "";

				} else {
					hintColor = new Color(0.45F, 0.8F, 0.85F);
					star.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
					textType.text = "SUP";
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
				}

			} else {
				hintColor = new Color(0.4F, 0.7F, 0.25F);
				star.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
				textType.text = "REC";
				textDis.text = "";
			}

		} else if (entity.info.sType == StarType.End) {
			hintColor = new Color(0.9F, 0.7F, 0.15F);
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 75);
			textType.text = "TGT";
			textDis.text = "";
		}

		textDis.color = hintColor;
		textType.color = hintColor;
		star.color = hintColor;
		board.color = hintColor;
	}

	private void UpdatePos() {
		Vector2 offset = (entity.transform.position - SYS_ShipController.Direct.transform.position);
		offset *= 360;

		float scale = Mathf.Abs(offset.x) / 360;
		offset = offset / scale;

		if (Mathf.Abs(offset.y) > 640) {
			scale = Mathf.Abs(offset.y) / 640;
			offset = offset / scale;
		}

		rect.anchoredPosition = offset;
		rect.eulerAngles = new Vector3(0, 0, Angle(-offset));//※  將Vector3型別轉換四元數型別
	}

	public float Angle(Vector2 p_vector2) {
		if (p_vector2.x < 0) {
			return 360 - (Mathf.Atan2(-p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(-p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
		}
	}

	public void SelfDrive() {
		SYS_SelfDriving.Direct.Regist(entity.transform);
	}
}

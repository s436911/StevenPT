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
	public SpaceEntity entity;
	public bool navigating = false;


	void Awake() {
		rect = this.GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update() {
		if (entity.info.sType == StarType.Check) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			//標記星球
			if (UI_Navigator.Direct.nextPlanet == entity && distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();
				UpdateHint();
			
			//一般星球
			} else if (distance < (UI_Navigator.Direct.detectDisT + SYS_ResourseManager.Direct.detectLv) * SYS_StarmapManager.Direct.avgSpeed && distance > UI_Navigator.Direct.closeDis) {
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
				UpdateHint();

			}  else {
				CloseHint();
			}
		} else if (entity.info.sType == StarType.Activity) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			//一般星球
			if (distance < (UI_Navigator.Direct.detectDisT + SYS_ResourseManager.Direct.detectLv) * SYS_StarmapManager.Direct.avgSpeed && distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();
				UpdateHint();

			} else {
				CloseHint();
			}
		}
	}

	public void Regist(SpaceEntity entity) {
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
					textType.text = "???";

					if (SYS_ShipController.Direct.detector.activeSelf) {
						textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
					} else {
						textDis.text = "";
					}

				} else {
					hintColor = new Color(0.45F, 0.8F, 0.85F);
					textType.text = "SUP";
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
				}

			} else {
				//hintColor = new Color(0.4F, 0.7F, 0.25F);
				hintColor = new Color(0.9F, 0.7F, 0.15F);
				textType.text = "REC";

				if (SYS_ShipController.Direct.detector.activeSelf) {
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
				} else {
					textDis.text = "";
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);

		} else if (entity.info.sType == StarType.End) {
			if (UI_Navigator.Direct.nextPlanet != entity) {
				hintColor = new Color(0.9F, 0.7F, 0.15F);
				textType.gameObject.SetActive(false);

			} else {
				textType.gameObject.SetActive(true);

				hintColor = new Color(0.9F, 0.7F, 0.15F);
				textType.text = "TGT";

				if (SYS_ShipController.Direct.detector.activeSelf) {
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
				} else {
					textDis.text = "";
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);

		} else if (entity.info.sType == StarType.Activity) {
			if (UI_Navigator.Direct.nextPlanet != entity) {
				if (!entity.explored) {
					hintColor = new Color(0.85F, 0.85F, 0.85F);
					textType.text = "???";

					if (SYS_ShipController.Direct.detector.activeSelf) {
						textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
					} else {
						textDis.text = "";
					}

				} else {
					hintColor = new Color(0.45F, 0.8F, 0.85F);
					textType.text = "ACT";
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.maxSpeed).ToString("f0");
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);
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

		if (Mathf.Abs(offset.y) > 560) {//640
			scale = Mathf.Abs(offset.y) / 560;//640
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
		if (SYS_SelfDriving.Direct.GetTGT() == null || SYS_SelfDriving.Direct.GetTGT().tgtTrans != entity.transform) {
			SYS_SelfDriving.Direct.Regist(entity.transform);
		} else {
			SYS_SelfDriving.Direct.Reset();
		}
	}
}

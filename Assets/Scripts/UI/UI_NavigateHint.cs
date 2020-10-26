using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NavigateHint : MonoBehaviour {
	public RectTransform rect;
	public Image board;
	public Image boardBack;
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
		if (entity.info.nvType == NaviType.Check) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			if (distance > UI_Navigator.Direct.closeDis) {
				//標記星球
				if (UI_Navigator.Direct.nextPlanet == entity) {
					ShowHint();
					UpdatePos();
					UpdateHint();

					//上個星球
				} else if (UI_Navigator.Direct.prePlanet == entity) {
					ShowHint();
					UpdatePos();
					UpdateHint();

					//一般星球
				} else if (distance < (UI_Navigator.Direct.detectDisT + SYS_Save.Direct.GetResearch(1)) * SYS_Starmap.Direct.avgSpeed) {
					ShowHint();
					UpdatePos();
					UpdateHint();

				} else {
					CloseHint();
				}
			} else {
				CloseHint();
			}
			
		} else if (entity.info.nvType == NaviType.End ) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			if (distance > UI_Navigator.Direct.closeDis) {
				ShowHint();
				UpdatePos();
				UpdateHint();

			}  else {
				CloseHint();
			}
		} else if (entity.info.nvType == NaviType.Activity) {
			float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);

			//一般星球
			if (distance < (UI_Navigator.Direct.detectDisT + SYS_Save.Direct.GetResearch(1)) * SYS_Starmap.Direct.avgSpeed && distance > UI_Navigator.Direct.closeDis) {
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
		if (entity.info.nvType == NaviType.Check) {
			board.transform.localScale = Vector2.one;

		} else if (entity.info.nvType == NaviType.End) {
			board.transform.localScale = Vector2.one;

		} else if (entity.info.nvType == NaviType.Activity) {
			board.transform.localScale = Vector2.one * 0.85f;
		}

		UpdateHint();
	}

	public void ShowHint() {
		board.gameObject.SetActive(true);
		boardBack.gameObject.SetActive(true);
		star.gameObject.SetActive(true);
		textType.gameObject.SetActive(true);
		textDis.gameObject.SetActive(true);
	}

	public void CloseHint() {
		board.gameObject.SetActive(false);
		boardBack.gameObject.SetActive(false);
		star.gameObject.SetActive(false);
		textType.gameObject.SetActive(false);
		textDis.gameObject.SetActive(false);
	}

	private void UpdateHint() {
		Color hintColor = Color.white;

		if (entity.info.nvType == NaviType.Check) {
			if (UI_Navigator.Direct.nextPlanet == entity) {
				//hintColor = new Color(0.4F, 0.7F, 0.25F);
				hintColor = new Color(0.9F, 0.7F, 0.15F);
				textType.text = "nxt";

				if (SYS_ShipController.Direct.detector.activeSelf) {
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
				} else {
					textDis.text = "";
				}

			} else if (UI_Navigator.Direct.prePlanet == entity) {
				//hintColor = new Color(0.4F, 0.7F, 0.25F);
				hintColor = new Color(0.45F, 0.8F, 0.85F);
				textType.text = "pre";
				textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");

			} else {
				if (!entity.explored) {
					hintColor = new Color(0.85F, 0.85F, 0.85F);
					textType.text = "???";

					if (SYS_ShipController.Direct.detector.activeSelf) {
						textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
					} else {
						textDis.text = "";
					}

				} else {
					hintColor = new Color(0.45F, 0.8F, 0.85F);
					textType.text = "SUP";
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);

		} else if (entity.info.nvType == NaviType.End) {
			if (UI_Navigator.Direct.nextPlanet != entity) {
				hintColor = new Color(0.9F, 0.7F, 0.15F);
				textType.gameObject.SetActive(false);
				textDis.text = "";

			} else {
				textType.gameObject.SetActive(true);
				hintColor = new Color(0.9F, 0.7F, 0.15F);

				if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
					textType.text = "TGT";
				} else {
					textType.text = "NXT";
				}

				if (SYS_ShipController.Direct.detector.activeSelf) {
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
				} else {
					textDis.text = "";
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);

		} else if (entity.info.nvType == NaviType.Activity) {
			if (UI_Navigator.Direct.nextPlanet != entity) {
				if (!entity.explored) {
					hintColor = new Color(0.85F, 0.85F, 0.85F);
					textType.text = "???";

					if (SYS_ShipController.Direct.detector.activeSelf) {
						textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
					} else {
						textDis.text = "";
					}

				} else {
					hintColor = new Color(0.45F, 0.8F, 0.85F);
					textType.text = "ACT";
					textDis.text = (Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed()).ToString("f0");
				}
			}

			float nowSize = UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.GetComponent<RectTransform>().sizeDelta = new Vector2(nowSize, nowSize);
		}

		textDis.color = hintColor;
		textType.color = hintColor;
		star.color = hintColor;
		board.color = hintColor;
		//board.color = new Color(hintColor.r , hintColor.g , hintColor.b , 0.5f);
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

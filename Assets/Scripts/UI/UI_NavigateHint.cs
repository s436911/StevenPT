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
	public RawImage star;
	public SpaceEntity entity;
	public bool navigating = false;
	private bool isStar = true;
	private NaviMode naviMode = NaviMode.None;

	void Awake() {
		rect = this.GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update() {
		if (entity == null) {
			Destroy(gameObject);
			return;
		}

		float distance = Vector2.Distance(Camera.main.transform.position, entity.transform.position);
		if (distance <= UI_Navigator.Direct.closeDis) {
			CloseHint();
			return;
		}

		if (entity.info.nvType == NaviType.Check) {
			//導航星球(標記星球 || 上個星球)
			if (UI_Navigator.Direct.nextPlanet == entity || UI_Navigator.Direct.prePlanet == entity) {
				ShowHint();
				UpdatePos();
				UpdateHint();
				return;

				//一般星球 距離內
			} else if (distance < (UI_Navigator.Direct.detectDisT + SYS_Save.Direct.GetResearch(1)) * SYS_Starmap.Direct.avgSpeed) {
				ShowHint();
				UpdatePos();
				UpdateHint();
				return;
			}

			CloseHint();

		} else if (entity.info.nvType == NaviType.End) {
			if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
				//導航星球(標記星球 || 上個星球)
				if (UI_Navigator.Direct.nextPlanet == entity || UI_Navigator.Direct.prePlanet == entity) {
					ShowHint();
					UpdatePos();
					UpdateHint();
					return;

					//一般星球 距離內
				} else if (distance < (UI_Navigator.Direct.detectDisT + SYS_Save.Direct.GetResearch(1)) * SYS_Starmap.Direct.avgSpeed) {
					ShowHint();
					UpdatePos();
					UpdateHint();
					return;
				}

			} else if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect) {
				//導航星球(標記星球 || 上個星球)
				if (UI_Navigator.Direct.nextPlanet == entity || UI_Navigator.Direct.prePlanet == entity) {
					ShowHint();
					UpdatePos();
					UpdateHint();
					return;
				} 
			}

			CloseHint();

		} else if (entity.info.nvType == NaviType.Activity || entity.info.subType == SubType.Resoreces || entity.info.subType == SubType.Meteor) {
			//一般星球 || 一般資源
			if (distance < (UI_Navigator.Direct.detectDisT + SYS_Save.Direct.GetResearch(1)) * SYS_Starmap.Direct.avgSpeed ) {
				ShowHint();
				UpdatePos();
				UpdateHint();
				return;
			}

			CloseHint();
		}
	}
	
	public void Regist(SpaceEntity value, NaviMode valueNV, Texture valueTexture) {
		entity = value;
		naviMode = valueNV;
		isStar = false;
		star.texture = valueTexture;
		star.GetComponent<Animator>().enabled = false;
		UpdateHint();
	}

	public void Regist(SpaceEntity value ) {
		entity = value;
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
		Color hintColor = new Color(0.85F, 0.85F, 0.85F);
		int hintTGT = 0;

		if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
			if (entity.info.nvType == NaviType.End) {
				hintTGT = 2;
				textType.text = "TGT";

			} else if (UI_Navigator.Direct.nextPlanet == entity) {
				hintTGT = 2;
				textType.text = "NXT";

			} else if (UI_Navigator.Direct.prePlanet == entity) {
				textType.text = "PRE";

			} else if (naviMode == NaviMode.Alert) {
				hintTGT = 2;
				textType.text = "";

			} else if (entity.explored) {
				textType.text = entity.info.nvType == NaviType.Activity ? "ACT" : "SUP";

			} else {
				textType.text = "???";
			}

		} else if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect) {
			if (UI_Navigator.Direct.nextPlanet == entity) {
				textType.text = "NXT";

			} else if (UI_Navigator.Direct.prePlanet == entity) {
				textType.text = "PRE";

			} else if (entity.info.subType == SubType.Resoreces) {
				hintTGT = 1;
				textType.text = "";

			} else if (entity.explored) {
				textType.text = entity.info.nvType == NaviType.Activity ? "ACT" : "SUP";

			} else {
				textType.text = "???";
			}
		}

		if (naviMode == NaviMode.Alert) {
			hintColor = new Color(1, 0, 0);

		} else if (hintTGT > 0) {
			hintColor = new Color(0.9F, 0.7F, 0.15F);

		} else if (entity.explored) {
			hintColor = new Color(0.45F, 0.8F, 0.85F);

		} else {
			hintColor = new Color(0.85F, 0.85F, 0.85F);
		}

		textDis.text = SYS_ShipController.Direct.detector.activeSelf || entity.explored ? GetTimeLeft().ToString("f0") : "";
		textDis.color = hintColor;
		textType.color = hintColor;

		if (isStar) {
			star.GetComponent<RectTransform>().sizeDelta = Vector2.one * UI_Navigator.Direct.hintSizer.Evaluate(Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / 4) * 80;
			star.color = hintColor;
		} 

		if (hintTGT == 0) {
			board.color = new Color(hintColor.r, hintColor.g, hintColor.b, 0.4f);
			boardBack.transform.localScale = Vector2.one * 0.75f;

		} else if (hintTGT == 1) {
			board.color = hintColor;
			boardBack.transform.localScale = Vector2.one * 0.75f;

		} else {
			board.color = hintColor;
			boardBack.transform.localScale = Vector2.one;
		}

	}

	private float GetTimeLeft() {
		return Vector2.Distance(entity.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed();
	}

	private void UpdatePos() {
		Vector2 offset = (entity.transform.position - SYS_ShipController.Direct.transform.position);
		offset *= 360;

		float scale = Mathf.Abs(offset.x) / 360;
		offset = offset / scale;

		if (Mathf.Abs(offset.y) > 540) {//1280 / 2 = 640
			scale = Mathf.Abs(offset.y) / 540;//640
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

public enum NaviMode {
	None,
	Target,
	Alert,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Navigator : MonoBehaviour {
	public static UI_Navigator Direct;
	public PlanetEntity prePlanet;
	public PlanetEntity nextPlanet;
	public AnimationCurve hintSizer;

	public GameObject uiPanel;
	public GameObject nvPfb;
	public float detectDisT = 12;
	public float closeDis = 5;

	public Transform backer;
	public LineRenderer backerLine;
	public TextMesh backerText;

	public StarInfo preRoute;

	void Awake() {
		Direct = this;
		SetBacker(null);
	}


	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			SetBacker(prePlanet);
		}
	}

	public void Init() {
		foreach (PlanetEntity planet in SYS_Space.Direct.planets) {
			if (SYS_Starmap.Direct.route[0] == planet.info) {
				SetNavigate(planet, null);
			}
		}
	}

	//到達星球
	public void Arrive(StarInfo aInfo) {
		if (SYS_Starmap.Direct.route.Contains(aInfo)) {
			int aIndex = SYS_Starmap.Direct.route.IndexOf(aInfo);

			if (SYS_Starmap.Direct.route[aIndex].nvType == NaviType.Check) {
				for (int ct = 0; ct < SYS_Space.Direct.planets.Count; ct++) {
					//找到下一顆
					if (SYS_Starmap.Direct.route[aIndex + 1] == SYS_Space.Direct.planets[ct].info) {
						SetNavigate(SYS_Space.Direct.planets[ct], SYS_Space.Direct.planets[ct - 1]);
						continue;
					}
				}

				if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
					SYS_Mission.Direct.SetMSbar(aIndex + 1);
				}

				if (aIndex == SYS_Starmap.Direct.route.Count - 2 && (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape)) {
					SYS_Audio.Direct.Play(BGMType.End);

				} else {
					SYS_Audio.Direct.Play(BGMType.Space);
				}
			}
		}
	}

	public void SetNavigate(PlanetEntity nextValue, PlanetEntity preValue) {
		prePlanet = preValue;
		nextPlanet = nextValue;

		SetBacker(preValue);
	}

	public void SetBacker(PlanetEntity value) {
		if (value != null) {
			Color hintColor = Color.white;
			Vector2 offset = (value.transform.position - SYS_ShipController.Direct.transform.position);
			float timeLeft = Vector2.Distance(value.transform.position, SYS_ShipController.Direct.transform.position) / SYS_ShipController.Direct.GetMaxSpeed();

			backerLine.gameObject.SetActive(true);
			backerText.gameObject.SetActive(true);
			backerText.text = timeLeft.ToString("f0");
			backerLine.SetPosition(1, new Vector2(0, -Mathf.Clamp(offset.magnitude / SYS_Camera.Direct.GetZoomrate(), 0, 5)));

			offset *= 360;

			float scale = Mathf.Abs(offset.x) / 360;
			offset = offset / scale;

			if (Mathf.Abs(offset.y) > 560) {//640
				scale = Mathf.Abs(offset.y) / 560;//640
				offset = offset / scale;
			}

			backer.eulerAngles = new Vector3(0, 0, Angle(-offset));//※  將Vector3型別轉換四元數型別

			if (timeLeft + 16 < SYS_ResourseManager.Direct.GetResource(0)) {

			} else if (timeLeft + 6 < SYS_ResourseManager.Direct.GetResource(0)) {
				hintColor = new Color(0.9F, 0.7F, 0.16F);

			} else if (timeLeft + 1 < SYS_ResourseManager.Direct.GetResource(0)) {
				hintColor = new Color(0.95F, 0.35F, 0.08F);

			} else {
				backerText.text = "dead";
				hintColor = new Color(1F, 0, 0);
			}

			backerLine.SetColors(hintColor, new Color(hintColor.r , hintColor.g , hintColor.b , 0));
			backerText.color = hintColor;

		} else {
			backerLine.gameObject.SetActive(false);
			backerText.gameObject.SetActive(false);
		}
	}

	public float Angle(Vector2 p_vector2) {
		if (p_vector2.x < 0) {
			return 360 - (Mathf.Atan2(-p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(-p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
		}
	}
	public void Regist(SpaceEntity entity) {
		UI_NavigateHint objGen = Instantiate(nvPfb).GetComponent<UI_NavigateHint>();
		objGen.transform.SetParent(uiPanel.transform);
		objGen.Regist(entity);
	}

	public void Reset() {
		foreach (Transform child in uiPanel.transform) {
			Destroy(child.gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Navigator : MonoBehaviour
{
	public static UI_Navigator Direct;
	public PlanetEntity prePlanet;
	public PlanetEntity nextPlanet;
	public AnimationCurve hintSizer;

	public GameObject uiPanel;
	public GameObject nvPfb;
	public float detectDisT = 12;
	public float closeDis = 5;
	
	public StarInfo preRoute;

	void Awake() {
		Direct = this;
	}

	void Update() {
		/*推薦星球邏輯

		if (SYS_Space.Direct.planets.Count > 0) {
			
			PlanetEntity lowestPlanet = null;
			foreach (PlanetEntity planet in SYS_Space.Direct.planets) {
				if (planet.info.sType == StarType.Check && planet.transform.position.y > SYS_ShipController.Direct.transform.position.y) {
					float shipDis = Vector2.Distance(planet.transform.position, SYS_ShipController.Direct.transform.position);

					if (lowestPlanet != null) {
						if (shipDis < Vector2.Distance(lowestPlanet.transform.position, SYS_ShipController.Direct.transform.position) && shipDis > closeDis) {
							if (shipDis < Vector2.Distance(SYS_Space.Direct.tgtPlanet.transform.position, SYS_ShipController.Direct.transform.position)) {
								lowestPlanet = planet;
							}
						}

					} else if (shipDis > closeDis) {
						if (shipDis < Vector2.Distance(SYS_Space.Direct.tgtPlanet.transform.position, SYS_ShipController.Direct.transform.position)) {
							lowestPlanet = planet;
						}
					}
				}
			nextPlanet = lowestPlanet;
			}
		}
	*/
	}

	public void Init() {
		foreach (PlanetEntity planet in SYS_Space.Direct.planets) {
			if (SYS_Starmap.Direct.route[0] == planet.info) {
				prePlanet = null;
				nextPlanet = planet;
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
						prePlanet = SYS_Space.Direct.planets[ct - 1];
						nextPlanet = SYS_Space.Direct.planets[ct];
						continue;
					}
				}

				SYS_Mission.Direct.SetMSbar(aIndex + 1);

				if (aIndex == SYS_Starmap.Direct.route.Count - 2 && (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape)) {
					SYS_Audio.Direct.Play(BGMType.End);

				} else {
					SYS_Audio.Direct.Play(BGMType.Space);
				}
			}
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

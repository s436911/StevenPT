using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Navigator : MonoBehaviour
{
	public static UI_Navigator Direct;
	public PlanetEntity nextPlanet;
	public AnimationCurve hintSizer;

	public GameObject nvPfb;
	public float detectDisT = 12;
	public float closeDis = 5;

	public StarInfo preRoute;

	void Awake() {
		Direct = this;
	}

	void Update() {
		/*推薦星球邏輯

		if (SYS_SpaceManager.Direct.planets.Count > 0) {
			
			PlanetEntity lowestPlanet = null;
			foreach (PlanetEntity planet in SYS_SpaceManager.Direct.planets) {
				if (planet.info.sType == StarType.Check && planet.transform.position.y > SYS_ShipController.Direct.transform.position.y) {
					float shipDis = Vector2.Distance(planet.transform.position, SYS_ShipController.Direct.transform.position);

					if (lowestPlanet != null) {
						if (shipDis < Vector2.Distance(lowestPlanet.transform.position, SYS_ShipController.Direct.transform.position) && shipDis > closeDis) {
							if (shipDis < Vector2.Distance(SYS_SpaceManager.Direct.tgtPlanet.transform.position, SYS_ShipController.Direct.transform.position)) {
								lowestPlanet = planet;
							}
						}

					} else if (shipDis > closeDis) {
						if (shipDis < Vector2.Distance(SYS_SpaceManager.Direct.tgtPlanet.transform.position, SYS_ShipController.Direct.transform.position)) {
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
		foreach (PlanetEntity planet in SYS_SpaceManager.Direct.planets) {
			if (SYS_StarmapManager.Direct.route[0] == planet.info) {
				nextPlanet = planet;
			}
		}
	}

	//到達星球
	public void Arrive(StarInfo aInfo) {
		if (SYS_StarmapManager.Direct.route.Contains(aInfo)) {
			int aIndex = SYS_StarmapManager.Direct.route.IndexOf(aInfo);

			if (SYS_StarmapManager.Direct.route[aIndex].sType == StarType.Check) {
				foreach (PlanetEntity planet in SYS_SpaceManager.Direct.planets) {
					if (SYS_StarmapManager.Direct.route[aIndex+1] == planet.info) {
						nextPlanet = planet;
					}
				}
			} else {
				nextPlanet = null;
			}
		}
	}

	public void Regist(SpaceEntity entity) {
		UI_NavigateHint objGen = Instantiate(nvPfb).GetComponent<UI_NavigateHint>();
		objGen.transform.SetParent(transform);
		objGen.Regist(entity);
	}

	public void Reset() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEntity : SpaceEntity {
	public GameObject halo;
	public InteractEvent story;
	public TextMesh text;
		
	public void Regist(StarInfo info, Material mat, float size, bool haveHalo, int eventId) {
		Regist(info, mat, size);
		halo.SetActive(haveHalo);
		text.text = info.name;
		story = TMP_InteractEvent.GetPlanetEvent(info.name, eventId);
	}

	public void Regist(StarInfo info, float size, bool haveHalo, int eventId) {
		Regist(info, size);
		halo.SetActive(haveHalo);
		text.text = info.name;
		story = TMP_InteractEvent.GetPlanetEvent(info.name, eventId);
	}

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.transform.GetComponent<SYS_ShipController>() != null) {
			SYS_SelfDriving.Direct.Reset();
			if (info.nvType == NaviType.Check) {
				EnterPlanet();

			} else if (info.nvType == NaviType.End) {
				if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip || SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
					SYS_Mission.Direct.SetMSbar(SYS_Starmap.Direct.route.Count);
				} else {
					EnterPlanet();
				}
			}
		}
	}

	public void EnterPlanet() {
		//SYS_Interactive.Direct.Regist(iEvent);
		SYS_Interactive.Direct.RegistTalker(this, story);
		UI_Navigator.Direct.Arrive(this);
		if (!explored) {
			explored = true;
			SYS_TeamManager.Direct.ModifyMorale(1);
			int randomResource = Random.Range(0, 4);

			if (randomResource == 0) {
				SYS_ResourseManager.Direct.ModifyResource(0, 5);
				SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "好多好多的燃料!");

			} else if (randomResource == 1) {
				SYS_ResourseManager.Direct.ModifyResource(1, 1);
				SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "裝甲升級!");

			} else if (randomResource == 2) {
				SYS_ResourseManager.Direct.ModifyResource(2, 1);
				SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "BUFFET!");

			} else if (randomResource == 3) {
				SYS_ResourseManager.Direct.ModifyResource(3, 1);
				SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "我好餓RRRRR!");
			}
		} else if (SYS_TeamManager.Direct.TriggerEvent(33)) {

		}
	}
}

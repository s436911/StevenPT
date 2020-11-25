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
			if (SYS_ShipController.Direct.TriggerAble()) {

				if (SYS_SelfDriving.Direct.GetTGT() != null && SYS_SelfDriving.Direct.GetTGT().tgtTrans == transform) {
					SYS_SelfDriving.Direct.Reset();
				} 

				SYS_ShipController.Direct.Trigger();
			}
			
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
			int resRate = 1;

			explored = true;
			SYS_TeamManager.Direct.ModifyMorale(5);			

			if (!SYS_Starmap.Direct.route.Contains(info)) {
				foreach (Member mem in SYS_Save.Direct.GetMembers()) {
					if (mem.skill1ID > 0) {
						if (mem.skill1ID == 1) {
							SYS_TeamManager.Direct.ModifyMorale(1);

						} else if (mem.skill1ID == 2) {
							SYS_TeamManager.Direct.ModifyMorale(1);

							if (SYS_TeamManager.Direct.IsHighMorale()) {
								resRate = resRate + 1;
							}

						} else if (mem.skill1ID == 3) {
							if (SYS_Mission.Direct.expNotRoute) {
								SYS_TeamManager.Direct.ModifyMorale(3);
							} else {
								SYS_Mission.Direct.expNotRoute = true;
							}
						}
					}
					if (mem.skill2ID > 0) {
						if (mem.skill2ID == 1) {
							SYS_TeamManager.Direct.ModifyMorale(1);

						} else if (mem.skill2ID == 2) {
							SYS_TeamManager.Direct.ModifyMorale(1);

							if (SYS_TeamManager.Direct.IsHighMorale()) {
								resRate = resRate + 1;
							}

						} else if (mem.skill2ID == 3) {
							if (SYS_Mission.Direct.expNotRoute) {
								SYS_TeamManager.Direct.ModifyMorale(3);
							} else {
								SYS_Mission.Direct.expNotRoute = true;
							}
						}
					}
				}
			}

			int randomResource = Random.Range(0, 3);

			if (randomResource == 0) {
				SYS_ResourseManager.Direct.ModifyResource(0, 5 * resRate);
				SYS_TeamManager.Direct.Talk(2, "好多好多的燃料!");

			} else if (randomResource == 1) {
				SYS_ResourseManager.Direct.ModifyResource(1, 1 * resRate);
				SYS_TeamManager.Direct.Talk(2, "裝甲升級!");

			} else if (randomResource == 2) {
				SYS_ResourseManager.Direct.ModifyResource(2, 1 * resRate);
				SYS_TeamManager.Direct.Talk(2, "BUFFET!");

			} 

		} else if (SYS_TeamManager.Direct.TriggerEvent(33)) {

		}
	}
}

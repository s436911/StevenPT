using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEntity : SpaceEntity {
	public GameObject halo;
	public InteractEvent iEvent;
	public TextMesh text;

	// Start is called before the first frame update
	void Awake() {
		GetComponent<Collider2D>();
	}
	
	public void Regist(StarInfo info, Material mat, float size, bool haveHalo, int eventId) {
		Regist(info, mat, size);
		halo.SetActive(haveHalo);
		text.text = info.sName;
		iEvent = TMP_InteractEvent.GetPlanetEvent(info.sName, eventId);
	}

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.transform.parent.GetComponent<SYS_ShipController>() != null) {
			if (info.sType == StarType.Check) {
				SYS_Interactive.Direct.Regist(iEvent);
				UI_Navigator.Direct.Arrive(info);
				if (!explored) {
					explored = true;
					int randomResource = Random.Range(0, 4);

					if (randomResource == 0) {
						SYS_ResourseManager.Direct.ModifyResource(0, 5);
						SYS_PopupManager.Direct.Regist("LAN", "好多好多的燃料!");

					} else if(randomResource == 1) {
						SYS_ResourseManager.Direct.ModifyResource(1, 1);
						SYS_PopupManager.Direct.Regist("TUDEN", "裝甲升級!");

					} else if (randomResource == 2) {
						SYS_ResourseManager.Direct.ModifyResource(2, 1);
						SYS_PopupManager.Direct.Regist("MIG", "BUFFET!");

					} else if (randomResource == 3) {
						SYS_ResourseManager.Direct.ModifyResource(3, 1);
						SYS_PopupManager.Direct.Regist("STEVEN", "我好餓RRRRR!");
					}
				}

			} else if (info.sType == StarType.End) {
				UI_ScoreManager.Direct.Victory();
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

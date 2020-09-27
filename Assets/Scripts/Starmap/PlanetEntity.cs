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
					} else if(randomResource == 1) {
						SYS_ResourseManager.Direct.ModifyResource(1, 1);
					} else if (randomResource == 2) {
						SYS_ResourseManager.Direct.ModifyResource(2, 1);
					} else if (randomResource == 3) {
						SYS_ResourseManager.Direct.ModifyResource(3, 1);
					}
				}

			} else if (info.sType == StarType.End) {
				SYS_ResourseManager.Direct.ModifyResourceHome(0, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * 2 * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))));
				SYS_ResourseManager.Direct.ModifyResourceHome(2, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * 5 * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))));
				SYS_ResourseManager.Direct.ModifyResourceHome(3, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * 10 * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))));
				SYS_ResourseManager.Direct.ModifyResourceHome(4, Mathf.RoundToInt(1 * (1 + (0.1f * SYS_StarmapManager.Direct.difficult))));
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

﻿using System.Collections;
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
				explored = true;

			} else if (info.sType == StarType.End) {
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

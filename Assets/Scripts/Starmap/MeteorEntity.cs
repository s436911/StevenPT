using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.transform.parent.GetComponent<SYS_ShipController>() != null) {
			SYS_ResourseManager.Direct.ModifyResource(1,-1);

			if (Random.Range(0, 100) < 50) {
				SYS_PopupManager.Direct.Regist("MIG", "好痛!");
			} else {
				SYS_PopupManager.Direct.Regist("MIG", "不能好好開船嗎!");
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

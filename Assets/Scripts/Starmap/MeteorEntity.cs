using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.transform.parent.GetComponent<SYS_ShipController>() != null) {
			SYS_ResourseManager.Direct.ModifyResource(1,-1);
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

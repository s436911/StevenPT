using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null && ship.DamageAble()) {
			SYS_CameraController.Direct.Shake(0.3f);

			if (!ship.reflecter.activeSelf) {
				ship.Damage(1, 2);
			}
			
			//ridgid.velocity = ship.ridgid.velocity * 1.25f;
			ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

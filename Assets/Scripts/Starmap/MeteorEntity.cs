using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {
	public bool starEater = true;

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null && ship.IsDamageAble() ) {
			if (starEater) {
				SYS_Camera.Direct.Shake(0.45f);
				ship.Impact(50 , ridgid.velocity);
				ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized * 4);
				ship.Damage(1);

			} else if (ship.IsHighspeed()) {
				if (Random.Range(0, 100) > SYS_SaveManager.Direct.GetMembersAttribute(3)) {
					SYS_Camera.Direct.Shake(0.3f);

					if (!ship.reflecter.activeSelf) {
						ship.Impact(1, ridgid.velocity);
						ship.Damage(1);
					}

					ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
				} else {
					SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "呼~好險!");
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

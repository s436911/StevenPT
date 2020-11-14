using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {
	public bool ice = false;
	public bool enemy = false;
	public bool nonRespawn = true;
	public bool cloud = false;
	public Transform tgt;

	public void Regist(StarInfo info, float size) {
		this.info = info;
		transform.localScale = new Vector3(size, size, size);
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null) {
			if (ice) {
				ship.ModifyIce(5);
			}

			if (!ship.reflecter.activeSelf) {
				SYS_Camera.Direct.Shake(0.3f);
				if (!SYS_TeamManager.Direct.TriggerEvent(41)) {
					if (Random.Range(0, 100) > SYS_Save.Direct.GetMembersAttribute(3)) {
						ship.Shock(4);

						if (enemy && ship.IsDamageAble()) {
							ship.Damage(1);
						}
					} else if (SYS_TeamManager.Direct.TriggerEvent(42)) {
						ship.Shock(4);

						if (enemy && ship.IsDamageAble()) {
							ship.Damage(1);
						}
					} else {
						SYS_TeamManager.Direct.Talk(2, "呼~好險!");
					}
				}
			}

			ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
		}
	}

	void OnTriggerEnter2D(Collider2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null) {
			if (enemy) {
				SYS_Camera.Direct.Shake(0.3f);
				ship.Damage(1);
			}

			if (cloud) {
				SYS_TeamManager.Direct.TriggerEvent(43);
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

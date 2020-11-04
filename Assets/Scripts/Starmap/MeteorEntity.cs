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

		if (ship != null && ship.IsDamageAble()) {
			if (ice) {
				ship.ModifyIce(5);
			}

			if (Random.Range(0, 100) > SYS_Save.Direct.GetMembersAttribute(3)) {
				SYS_Camera.Direct.Shake(0.3f);

				if (!ship.reflecter.activeSelf) {
					ship.ModifySpeed(-4);
				}

				ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
			} else {
				SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "呼~好險!");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D colli) {
		SYS_ShipController ship = colli.transform.parent.GetComponent<SYS_ShipController>();

		if (ship != null) {
			if (enemy) {
				SYS_Camera.Direct.Shake(0.3f);
				ship.Damage(1);
			}

			if (cloud) {
				if (Random.Range(0,100) < 5) {
					//SYS_TeamManager.Direct.TriggerEvent(13);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {
	public bool boss = false;
	public bool ice = false;
	public bool cloud = false;
	public bool beetle = false;
	public bool nonRespawn = true;
	public float atkDis = 5;
	public bool enemy = false;
	public Transform tgt;

	public void Regist(StarInfo info, float size) {
		this.info = info;
		transform.localScale = new Vector3(size, size, size);
	}

	void FixedUpdate() {
		if (beetle && SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			float distance = ((Vector2)transform.position - (Vector2)SYS_ShipController.Direct.transform.position).magnitude;
			transform.position = new Vector3( tgt.transform.position.x , tgt.transform.position.y , transform.position.z);

			if (distance <= atkDis) {
				beetle = false;
				ridgid.velocity = ((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * 3 - (Vector2)transform.position).normalized * 8;
				return;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null && ship.IsDamageAble() ) {
			if (ice) {
				ship.ModifyIce(5);
			}

			if (enemy) {
				if (boss) {
					SYS_Camera.Direct.Shake(0.45f);
					ship.Impact(50, ridgid.velocity);
					ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized * 4);

				} else {
					SYS_Camera.Direct.Shake(0.3f);
					ship.Impact(1, ridgid.velocity);
					ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized );
				}

				ship.Damage(1);

			} else if (ship.IsHighspeed()) {
				if (Random.Range(0, 100) > SYS_Save.Direct.GetMembersAttribute(3)) {
					SYS_Camera.Direct.Shake(0.3f);

					if (!ship.reflecter.activeSelf) {
						ship.Impact(1, ridgid.velocity);
						ship.Damage(1);
					}

					ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
				} else {
					SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "呼~好險!");
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D  colli) {
		SYS_ShipController ship = colli.transform.parent.GetComponent<SYS_ShipController>();

		if (ship != null ) {
			SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember().name, "看不見RRRRR!");
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

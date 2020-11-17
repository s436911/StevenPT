using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEntity : SpaceEntity {
	public bool boss = false;
	public bool beetle = false;
	public float atkDis = 5;
	public bool enemy = false;
	public Transform tgt;
	public Vector2 movePos;
	public bool whale = false;
	public float reflect = 1;
	public float lerp;
	private float nowAngle = 0;
	private float actTimer;

	public void Regist(StarInfo info, float size) {
		this.info = info;
		transform.localScale = new Vector3(size, size, size);
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (beetle) {
				float distance = ((Vector2)transform.position - (Vector2)SYS_ShipController.Direct.transform.position).magnitude;
				transform.position = new Vector3(tgt.transform.position.x, tgt.transform.position.y, transform.position.z);

				nowAngle = Mathf.Lerp(nowAngle, Common.GetEulerAngle((Vector2)(SYS_ShipController.Direct.transform.position - transform.position)), lerp * Time.fixedDeltaTime);
				transform.eulerAngles = new Vector3(0, 0, nowAngle);

				if (distance <= atkDis) {
					beetle = false;
					ridgid.velocity = ((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * 3 - (Vector2)transform.position).normalized * 8;
					return;
				}

			} else if (whale) {
				nowAngle = Mathf.Lerp(nowAngle, Common.GetEulerAngle(((Vector2)transform.position) - movePos), lerp * Time.fixedDeltaTime);
				transform.eulerAngles = new Vector3(0, 0, nowAngle);

				Vector2 offset = (movePos - (Vector2)transform.position);
				if (offset.magnitude > 3) {
					ridgid.velocity = offset.normalized * 0.5f;
				} else {
					ridgid.velocity = Vector2.zero;
				}
			}

			if (Time.timeSinceLevelLoad - actTimer > 30) {
				if (whale && tgt) {
					movePos = (Vector2)tgt.position + Random.insideUnitCircle * 12;
				}
				actTimer = Time.timeSinceLevelLoad;
			}
		}
	}


	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null) {
			if (enemy && ship.IsDamageAble()) {
				if (boss) {
					SYS_Camera.Direct.Shake(0.45f);
					ship.Shock(4);
					ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized * 8);

				} else if (beetle) {
					SYS_Camera.Direct.Shake(0.3f);
					ship.Shock(4);
				} 

				ship.Damage(1);
			} else {
				if (whale) {
					SYS_Camera.Direct.Shake(0.45f);
					ship.ModifySpeed(-4);
					ship.Impact(ridgid.mass, ridgid.velocity, reflect);
				}
			}
		}
	}
}

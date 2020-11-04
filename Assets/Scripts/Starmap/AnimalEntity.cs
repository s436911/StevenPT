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
	private float actTimer;
	private float nowAngle;

	public void Regist(StarInfo info, float size) {
		this.info = info;
		transform.localScale = new Vector3(size, size, size);
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (beetle) {
				float distance = ((Vector2)transform.position - (Vector2)SYS_ShipController.Direct.transform.position).magnitude;
				transform.position = new Vector3(tgt.transform.position.x, tgt.transform.position.y, transform.position.z);
				UpdateAngle(SYS_ShipController.Direct.transform.position);

				if (distance <= atkDis) {
					beetle = false;
					ridgid.velocity = ((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * 3 - (Vector2)transform.position).normalized * 8;
					return;
				}
			} else if (whale) {
				UpdateAngle(movePos);
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

	private void UpdateAngle(Vector2 pos) {
		Vector2 offset = ((Vector2)transform.position - pos);
		offset = ((Vector2)transform.position - pos);
		offset *= 360;

		nowAngle = Mathf.Lerp(nowAngle, Common.Angle(-offset), lerp * Time.fixedDeltaTime);

		transform.eulerAngles = new Vector3(0, 0, nowAngle);//※  將Vector3型別轉換四元數型別
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();

		if (ship != null && ship.IsDamageAble()) {
			if (enemy) {
				if (boss) {
					SYS_Camera.Direct.Shake(0.45f);
					ship.ModifySpeed(-4);
					ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized * 4);

				} else if (beetle) {
					SYS_Camera.Direct.Shake(0.3f);
					ship.ModifySpeed(-4);
					//ship.ModifyForce((SYS_ShipController.Direct.transform.position - transform.position).normalized);
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

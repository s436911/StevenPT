using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEntity : SpaceEntity {
	public Vector2 sizeRange = new Vector2(1,1.5f);
	public bool ice = false;
	public bool enemy = false;
	public bool nonRespawn = true;
	public bool cloud = false;
	public bool magnet = false;
	public Transform tgt;
	public GameObject spot;

	public void Regist(StarInfo info) {
		this.info = info;
		this.size = Random.Range(sizeRange.x , sizeRange.y);
		transform.localScale = Vector3.one * size;
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (magnet) {
				Vector2 dis = (SYS_ShipController.Direct.transform.position - transform.position);
				ridgid.velocity = dis.normalized * Common.Direct.magnetCurve.Evaluate(dis.magnitude) * (SYS_Weather.Direct.GetWeather() == 2 ? 1.5f : 1);

			} else if (ice) {
				if (SYS_Weather.Direct.GetWeather() == 0) {
					spot.gameObject.SetActive(false);
					enemy = false;
					transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * size, 0.25f * Time.fixedDeltaTime);

				} else {
					spot.gameObject.SetActive(true);
					enemy = true;
					transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * size * 2.5f, 0.25f * Time.fixedDeltaTime);
				}
			}
		}
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
					if (SYS_TeamManager.Direct.TriggerEvent(42)) {
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

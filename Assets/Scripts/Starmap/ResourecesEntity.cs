using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourecesEntity : SpaceEntity {
	public int stack = 0;
	public int resrcType = 0;
	public MeshRenderer meshRenderer2;
	public MeshRenderer meshRenderer3;
	   
	public void Regist(StarInfo info, Material mat, float size, int resrcType) {
		base.Regist(info, mat, size);
		this.resrcType = resrcType;
	}

	public void Regist(StarInfo info, Material mat, float size, int resrcType, int stack) {
		base.Regist(info, mat, size);

		meshRenderer2.material = mat;
		meshRenderer3.material = mat;
		this.stack = stack;
		this.resrcType = resrcType;
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();



		if (ship != null ) {
			if (stack > 0) {
				if (ship.DamageAble() && ship.IsHighspeed()) {
					if (Random.Range(0, 100) > SYS_SaveManager.Direct.GetMembersAttribute(3)) {
						SYS_Camera.Direct.Shake(0.3f);

						if (!ship.reflecter.activeSelf) {
							ship.Damage(1, 2);
						}

						//ridgid.velocity = ship.ridgid.velocity * 1.25f;
						ridgid.velocity = Random.insideUnitCircle.normalized * 2f;
					} else {
						SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "呼~好險!");
					}

					if (stack > 2) {
						if (Random.Range(0, 3) == 0) {
							SYS_SpaceManager.Direct.SplitResourece(transform.position, meshRenderer.material , resrcType);
							stack = Mathf.Clamp(stack - 1, 2, 10);

						} else {
							SYS_SpaceManager.Direct.SplitResourece(transform.position, meshRenderer.material, resrcType);
							SYS_SpaceManager.Direct.SplitResourece(transform.position, meshRenderer.material, resrcType);
							stack = Mathf.Clamp(stack - 2, 2, 10);
						}

						transform.localScale = new Vector3(0.6f + stack * 0.15f, 0.6f + stack * 0.15f, 0.6f + stack * 0.15f);
					} else {
						SYS_SpaceManager.Direct.SplitResourece(transform.position, meshRenderer.material, resrcType);
						SYS_SpaceManager.Direct.SplitResourece(transform.position, meshRenderer.material, resrcType);
						SYS_RadarManager.Direct.Remove(transform);
						Destroy(gameObject);
					}
				}
			} else {
				SYS_ResourseManager.Direct.ModifyResource(resrcType, 1);
				SYS_RadarManager.Direct.Remove(transform);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}
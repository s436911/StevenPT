using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourecesEntity : SpaceEntity {
	public int stack = 0;
	public int resrcSet;
	public MeshRenderer meshRenderer2;
	public MeshRenderer meshRenderer3;
	   
	public void Regist(StarInfo info, Material mat, float size, int resrcSet) {
		base.Regist(info, mat, size);
		this.resrcSet = resrcSet;
	}

	public void Regist(StarInfo info, Material mat, float size, int resrcSet, int stack) {
		base.Regist(info, mat, size);

		meshRenderer2.material = mat;
		meshRenderer3.material = mat;
		this.stack = stack;
		this.resrcSet = resrcSet;
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();


		if (ship != null ) {
			if (stack > 0) {
				if (ship.IsDamageAble() && ship.IsHighspeed()) {
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

					if (stack > 2) {
						if (Random.Range(0, 3) == 0) {
							SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material , resrcSet);
							stack = Mathf.Clamp(stack - 1, 2, 10);

						} else {
							SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcSet);
							SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcSet);
							stack = Mathf.Clamp(stack - 2, 2, 10);
						}

						transform.localScale = new Vector3(0.6f + stack * 0.15f, 0.6f + stack * 0.15f, 0.6f + stack * 0.15f);
					} else {
						SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcSet);
						SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcSet);
						SYS_RadarManager.Direct.Remove(transform);
						Destroy(gameObject);
					}
				}
			} else {
				if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect && SYS_Mission.Direct.nowMission.mainResrc == resrcSet && !SYS_Mission.Direct.IsComplete()) {
					SYS_Mission.Direct.ModifyMSbar(1);
				} else {
					SYS_ResourseManager.Direct.ModifyResource(DB.GetItem(resrcSet).effectID, 1);
				}
				SYS_Save.Direct.ModifyCargobay(DB.NewItem(resrcSet, 1));
				SYS_RadarManager.Direct.Remove(transform);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourecesEntity : SpaceEntity {
	public int stack = 0;
	public int resrcId;
	public MeshRenderer meshRenderer2;
	public MeshRenderer meshRenderer3;
	   
	public void Regist(StarInfo info, Material mat, float size, int resrcId) {
		base.Regist(info, mat, size);
		this.resrcId = resrcId;
	}

	public void Regist(StarInfo info, Material mat, float size, int resrcId, int stack) {
		base.Regist(info, mat, size);

		meshRenderer2.material = mat;
		meshRenderer3.material = mat;
		this.stack = stack;
		this.resrcId = resrcId;
	}

	void OnCollisionEnter2D(Collision2D colli) {
		SYS_ShipController ship = colli.transform.GetComponent<SYS_ShipController>();


		if (ship != null ) {
			if (stack > 0) {
				if (!ship.reflecter.activeSelf) {
					SYS_Camera.Direct.Shake(0.3f);
					if (!SYS_TeamManager.Direct.TriggerEvent(41)) {
						if (SYS_TeamManager.Direct.TriggerEvent(42)) {
							ship.Shock(4);

						} else {
							SYS_TeamManager.Direct.Talk(4, "呼~好險!");
						}
					}
				}

				ridgid.velocity = Random.insideUnitCircle.normalized * 2f;

				if (stack > 2) {
					if (Random.Range(0, 3) == 0) {
						SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcId);
						stack = Mathf.Clamp(stack - 1, 2, 10);

					} else {
						SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcId);
						SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcId);
						stack = Mathf.Clamp(stack - 2, 2, 10);
					}

					transform.localScale = new Vector3(0.6f + stack * 0.15f, 0.6f + stack * 0.15f, 0.6f + stack * 0.15f);
				} else {
					SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcId);
					SYS_Space.Direct.SplitResourece(transform.position, meshRenderer.material, resrcId);
					SYS_RadarManager.Direct.Remove(transform);
					Destroy(gameObject);
				}
			} else {
				if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect && SYS_Mission.Direct.nowMission.mainResrc == resrcId && !SYS_Mission.Direct.IsComplete()) {
					SYS_Mission.Direct.ModifyMSbar(1);
				} 
				SYS_Save.Direct.ModifyCargobay(DB.NewItem(resrcId, 1));
				SYS_RadarManager.Direct.Remove(transform);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}
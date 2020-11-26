using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityEntity : SpaceEntity {
	public InteractEvent story;
	public TextMesh text;
	public GameObject talker;

	// Start is called before the first frame update
	void Awake() {
		GetComponent<Collider2D>();
	}

	public void Regist(StarInfo info, Material mat, float size , int eventId , bool talker = false) {
		base.Regist(info, mat, size);
		text.text = info.name;
		story = TMP_InteractEvent.GetActivityEvent(info.name, eventId);
		if (talker) {
			this.talker.SetActive(true);
		}
	}

	void OnTriggerEnter2D(Collider2D colli) {

		if (colli.transform.GetComponent<SYS_ShipController>() != null) {

			if (SYS_ShipController.Direct.TriggerAble()) {
				if (SYS_SelfDriving.Direct.GetTGT() != null && SYS_SelfDriving.Direct.GetTGT().tgtTrans == transform) {
					SYS_SelfDriving.Direct.Reset();
				}
				SYS_ShipController.Direct.Trigger();
			}

			if (info.nvType == NaviType.Activity) {

				SYS_Interactive.Direct.RegistTalker(this, story);
				explored = true;
			} 
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

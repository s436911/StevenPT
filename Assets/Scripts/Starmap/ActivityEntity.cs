using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityEntity : SpaceEntity {
	public InteractEvent story;
	public TextMesh text; 

	// Start is called before the first frame update
	void Awake() {
		GetComponent<Collider2D>();
	}

	public void Regist(StarInfo info, Material mat, float size , int eventId) {
		base.Regist(info, mat, size);
		text.text = info.name;
		story = TMP_InteractEvent.GetActivityEvent(info.name, eventId);
	}

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.transform.parent.GetComponent<SYS_ShipController>() != null) {
			if (info.nvType == NaviType.Activity) {
				//SYS_Interactive.Direct.Regist(iEvent);
				SYS_Interactive.Direct.RegistTalker(this, story);
				explored = true;

			} 
		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BGRoller : MonoBehaviour {

	public float scale = 0.1F;
	public RawImage rend;
	

	// Update is called once per frame
	void Update() {
		rend.uvRect = new Rect(SYS_ShipController.Direct.transform.position.x * scale, SYS_ShipController.Direct.transform.position.y * scale, 1,1);
	}

}
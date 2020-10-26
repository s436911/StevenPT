using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gacha : MonoBehaviour {
	public float bornTimer;

	public void Regist(Vector2 force) {
		GetComponent<Rigidbody2D>().velocity = force;
		bornTimer = Time.timeSinceLevelLoad;
	}

	void Update() {
		if (Time.timeSinceLevelLoad - bornTimer > 3) {
			Destroy(gameObject);
		} 
	}
}

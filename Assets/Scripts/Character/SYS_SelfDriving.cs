using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_SelfDriving : MonoBehaviour {
	public static SYS_SelfDriving Direct;
	public SelfDriveTGT tgt;
	public float stopDis = 2;
	public bool pause = false;
	public GameObject uiPanel;

	void Awake() {
		Direct = this;
	}

	// Update is called once per frame
	void Update() {
		if (tgt != null && !pause) {
			if (tgt.IsNull()) {
				tgt = null;
			}

			SYS_ShipController.Direct.OnUpdateDirection((tgt.GetPos() - (Vector2)SYS_ShipController.Direct.transform.position));

			/*
			if (Vector2.Distance(tgt.GetPos(),SYS_ShipController.Direct.transform.position) < stopDis) {
				Reset();
			}*/
		}
	}

	public SelfDriveTGT GetTGT() {
		return tgt;
	}

	public void Regist(Vector2 tgtPos) {
		tgt = new SelfDriveTGT(tgtPos);
		SYS_ShipController.Direct.OnBeginMove();
		uiPanel.SetActive(true);
	}

	public void Regist(Transform tgtTrans) {
		tgt = new SelfDriveTGT(tgtTrans);
		SYS_ShipController.Direct.OnBeginMove();
		uiPanel.SetActive(true);
	}

	public void Pause() {
		if (tgt != null) {
			pause = true;
			SYS_ShipController.Direct.OnEndMove(false);
			uiPanel.SetActive(false);
		}
	}

	public void Play() {
		if (tgt != null) {
			pause = false;
			SYS_ShipController.Direct.OnBeginMove();
			uiPanel.SetActive(true);
		}
	}

	public void Reset() {
		tgt = null;
		SYS_ShipController.Direct.OnEndMove();
		pause = false;
		uiPanel.SetActive(false);
	}
}

public class SelfDriveTGT {
	public Vector2 tgtPos;
	public Transform tgtTrans;
	public bool posTGT = false;

	public bool IsNull() {
		return !posTGT && tgtTrans == null;
	}

	public SelfDriveTGT(Vector2 tgtPos) {
		this.tgtPos = tgtPos;
		posTGT = true;
	}

	public SelfDriveTGT(Transform tgtTrans) {
		this.tgtTrans = tgtTrans;
		posTGT = false;
	}

	public Vector2 GetPos() {
		if (posTGT) {
			return tgtPos;
		} else {
			return tgtTrans.position;
		}
	}
}

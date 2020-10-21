using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarObject : MonoBehaviour {
	public StarInfo sInfo;
	public Text sName;
	public Text sPos;
	public RawImage sRawImage;
	public Image halo;
	public Text iactive;
	public float hintTime;
	public bool hinting = false;

	public void Regist(StarInfo info) {
		sInfo = info;
	}

	// Update is called once per frame
	void Update() {
		if (hinting && Time.timeSinceLevelLoad - hintTime > 2) {
			CloseHint();
		}
	}

	public void ShowHint() {
		sName.text = sInfo.name;
		sPos.text = sInfo.sPos.ToString("f0");
		hintTime = Time.timeSinceLevelLoad;
		hinting = true;

		SYS_Starmap.Direct.ClickRoute(sInfo);
	}

	public void CloseHint() {
		sName.text = "";
		sPos.text = "";
		hinting = false; 
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarObject : MonoBehaviour {
	public StarInfo sInfo;
	public Text sName;
	public Text sPos;
	public RawImage sRawImage;
	public float hintTime;
	public bool hinting = false;

	public void Regist(StarInfo info) {
		sInfo = info;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (hinting && Time.timeSinceLevelLoad - hintTime > 5) {
			CloseHint();
		}
	}

	public void ShowHint() {
		sName.text = sInfo.sName;
		sPos.text = sInfo.sPos.ToString();
		hintTime = Time.timeSinceLevelLoad;
		hinting = true;

		SYS_StarmapManager.Direct.ClickRoute(sInfo);
	}

	public void CloseHint() {
		sName.text = "";
		sPos.text = "";
		hinting = false; 
	}
}

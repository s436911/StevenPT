using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonCarrier : MonoBehaviour {
	public int id = 0;
	public List<UI_ButtonBase> buttons = new List<UI_ButtonBase>();

	public void Init(int id) {
		this.id = id;
	}

	public void ClickShop(UI_ButtonBase sub) {
		SYS_Interactive.Direct.ClickShop(this, sub);
	}
}

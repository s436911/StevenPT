using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour {
	public int slot;
	public Item item;
    public RawImage icon;
	public Text text;

	public void Init(int slot) {
		this.slot = slot;
		Clear();
	}

	public void SetItem(Item item = null) {
		if (item != null) {

			this.item = item;
			icon.texture = SYS_ResourseManager.Direct.itemIcon[item.iconID];
			icon.color = new Color(1, 1, 1, 1);

			//1主動使用效果型、2主動使用buff型、3家中使
			if (item.typeID == 1 || item.typeID == 2) {
				text.text = "ACTI";

			} else if (item.typeID == 3) {
				text.text = "HOME";
			}
		} else {
			this.item = item;
			icon.texture = null;
			icon.color = new Color(0, 0, 0, 0);

			text.text = "N/A";
		}
	}

	public void Clear() {
		SetItem();
	}

	public void UseCargo() {
		SYS_ResourseManager.Direct.UseCargo(slot);
	}

	public void UseInventory() {
		SYS_ResourseManager.Direct.UseInventory(slot);
	}

	public void UsePreCargo() {
		SYS_ResourseManager.Direct.UsePreCargo(slot);
	}
}

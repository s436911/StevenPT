using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour {
	public int slot;
    public RawImage icon;
	public Text text;
	public Text textDescript;

	public void Init(int slot) {
		this.slot = slot;
		Clear();
	}

	public void SetItem(Item item) {
		if (!item.isNull) {
			icon.texture = Resources.Load<Texture2D>("Arts/" + item.iconID.ToString("00000"));
			icon.color = new Color(1, 1, 1, 1);

			//1主動使用效果型、2主動使用buff型、3家中使
			if (item.typeID == 1 || item.typeID == 2) {
				text.text = "ACTI";

			} else if (item.typeID == 3) {
				text.text = "HOME";
			}

			if (textDescript != null) {
				textDescript.text = item.text;
			}
		} else {
			icon.texture = null;
			icon.color = new Color(0, 0, 0, 0);

			text.text = "N/A";

			if (textDescript != null) {
				textDescript.text = "N/A";
			}
		}
	}

	public void Clear() {
		SetItem(new Item());
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

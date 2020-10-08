using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ResourseManager : MonoBehaviour {
	public static SYS_ResourseManager Direct;

	public Texture2D[] itemIcon = new Texture2D[6];
	
	public Text[] resourceText = new Text[5];
	public Text[] resourceMaxText = new Text[5];

	public Text[] resourceText_Home = new Text[5];

	public Image fuelBar;
	public Image foodBar;

	public int maxFuel = 50;
	public int maxArmor = 5;
	public int maxFood = 5;
	public int maxMineral = 5;

	public int startFuel = 50;
	public int startArmor = 5;
	public int startFood = 5;
	public int startMineral = 5;

	public int[] resources = new int[5];
	public UI_ItemSlot[] cargo = new UI_ItemSlot[2];
	public UI_ItemSlot[] preCargo = new UI_ItemSlot[2];
	public UI_ItemSlot[] inventory = new UI_ItemSlot[9];

	//-------------------------------home
	public int startFuel_Home = 0;
	public int startFood_Home = 0;
	public int startMineral_Home = 0;
	public int startCoin_Home = 0;

	public int[] resources_Home = new int[5];

	//---------------test
	public int engineNeed = 50;
	public int detectNeed = 50;
	public int shipNeed = 50;
	public int scopeNeed = 50;

	public int engineLv = 1;
	public int detectLv = 1;
	public int shipLv = 1;
	public int scopeLv = 1;

	public Text[] lvText = new Text[4];
	public Text[] needText = new Text[4];

	void Awake() {
		Direct = this;
	}

	void Start() {
		resourceMaxText[0].text = "/" + maxFuel.ToString();
		resourceMaxText[1].text = "/" + maxArmor.ToString();
		resourceMaxText[2].text = "/" + maxFood.ToString();
		resourceMaxText[3].text = "/" + maxMineral.ToString();
		
		//home 
		SetResourceHome(0, 0);
		SetResourceHome(2, 0);
		SetResourceHome(3, 0);
		SetResourceHome(4, 0);
		
		SetLevel(0, 1);
		SetLevel(1, 1);
		SetLevel(2, 1);
		SetLevel(3, 1);
		
		for (int ct = 0; ct < cargo.Length; ct++) {
			cargo[ct].Init(ct);
		}

		for (int ct = 0; ct < inventory.Length; ct++) {
			inventory[ct].Init(ct);
		}

		for (int ct = 0; ct < preCargo.Length; ct++) {
			preCargo[ct].Init(ct);
		}
	}

	public void Init() {
		SetResource(0, startFuel);
		SetResource(1, startArmor);
		SetResource(2, startFood);
		SetResource(3, startMineral);

		AddCargo(preCargo[0].item);
		AddCargo(preCargo[1].item);
	}

	public void AddCargo(Item item) {
		for (int ct = 0; ct < cargo.Length; ct++) {
			if (cargo[ct].item == null) {
				SetCargoSlot(ct, item);
				return;
			}
		}
	}

	public void AddPreCargo(Item item) {
		for (int ct = 0; ct < preCargo.Length; ct++) {
			if (preCargo[ct].item == null) {
				SetPreCargoSlot(ct, item);
				return;
			}
		}
	}

	public void AddInventory(Item item) {
		for (int ct = 0; ct < inventory.Length; ct++) {
			if (inventory[ct].item == null) {
				SetdInventorySlot(ct, item);
				return;
			}
		}
	}

	public void SetdInventorySlot(int slot, Item item = null) {
		inventory[slot].SetItem(item);
	}

	public void SetCargoSlot(int slot , Item item = null) {
		cargo[slot].SetItem(item);
	}

	public void SetPreCargoSlot(int slot, Item item = null) {
		preCargo[slot].SetItem(item);
	}
	/*
	public int GetItemAmount(int typeID, int effectID) {
		int re = 0;
		for (int ct = 0; ct < cargo.Length; ct++) {
			if (cargo[ct].gameObject.activeSelf && cargo[ct].item.typeID == typeID && cargo[ct].item.effectID == effectID) {
				re++;
			}
		}
		return re;
	}

	public bool ContainsItem(int typeID, int effectID) {
		for (int ct = 0; ct < cargo.Length; ct++) {
			if (cargo[ct].gameObject && cargo[ct].item.typeID == typeID && cargo[ct].item.effectID == effectID) {
				return true;
			}
		}
		return false;
	}

	public void UpdateItem() {
		if (ContainsItem(2,1)) {
			UI_Navigator.Direct.showDisHint = true;
		} else {
			UI_Navigator.Direct.showDisHint = false;
		}
	}*/

	public int GetResourceHome(int type) {
		return resources_Home[type];
	}

	public int GetResource(int type) {
		return resources[type];
	}

	public void ModifyResource(int type, int value) {
		SetResource(type, resources[type] + value);
	}

	public void ModifyResourceHome(int type, int value) {
		SetResourceHome(type, resources_Home[type] + value);
	}

	public void SetResource(int type, int value) {
		resources[type] = value;

		if (type == 0) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] > maxFuel) {
				resources[type] = maxFuel;
			}

			fuelBar.fillAmount = (float)resources[type] / (float)maxFuel * 0.2f;

		} else if (type == 1) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] > maxArmor) {
				resources[type] = maxArmor;
			}

		} else if (type == 2) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] > maxFood) {
				resources[type] = maxFood;
			}

			foodBar.fillAmount = (float)resources[type] / (float)maxFood * 0.2f;

		} else if (type == 3) {
			if (resources[type] <= 0) {
				resources[type] = 0;

			} else if (resources[type] > maxMineral) {
				resources[type] = maxMineral;
			}
		} else {
			Debug.LogError("修改錯誤的資源型態");
		}

		resourceText[type].text = resources[type].ToString();
	}

	public void SetResourceHome(int type, int value) {
		resources_Home[type] = value;

		if (type == 0 || type == 2|| type == 3|| type == 4) {
			if (resources_Home[type] <= 0) {
				resources_Home[type] = 0;

			} else if (resources_Home[type] > 99999) {
				resources_Home[type] = 99999;
			}

		} else {
			Debug.LogError("修改錯誤的資源型態");
		}

		resourceText_Home[type].text = resources_Home[type].ToString();
	}
	
	public void UseCargo(int slot) {
		bool used = false;

		if (cargo[slot].item.typeID == 1) {
			if (cargo[slot].item.effectID == 2) {//加倍油料
				UI_ScoreManager.Direct.bonusEnd[0] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist("MIG", "95加滿!");

			} else if (cargo[slot].item.effectID == 3) {//加倍食物
				UI_ScoreManager.Direct.bonusEnd[1] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist("STEVEN", "看起來好好吃!");				

			} else if (cargo[slot].item.effectID == 4) {//加倍礦石
				UI_ScoreManager.Direct.bonusEnd[2] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist("MIG", "大顆鑽石!");

			} else if (cargo[slot].item.effectID == 5) {//加倍硬幣
				UI_ScoreManager.Direct.bonusEnd[3] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist("CINDY", "請給我黃金!");

			} else if (cargo[slot].item.effectID == 6) {//偵測雷達
				SYS_ShipController.Direct.detector.SetActive(true);
				used = true;
				SYS_PopupManager.Direct.Regist("LAN", "喔喔到處都是隕石呢!");

			} else if (cargo[slot].item.effectID == 7) {//反射裝甲
				SYS_ShipController.Direct.reflecter.SetActive(true);
				used = true;
				SYS_PopupManager.Direct.Regist("CINDY", "我要撞飛所有壞隕石!");
			}
		}

		if (used) {
			cargo[slot].Clear();
		}
	}

	public void UsePreCargo(int slot) {
		AddInventory(preCargo[slot].item);
		preCargo[slot].Clear();
	}

	public void UseInventory(int slot) {
		bool used = false;

		if (inventory[slot].item.typeID == 3) {
			
		} else if (preCargo[0].item == null || preCargo[1].item == null) {
			AddPreCargo(inventory[slot].item);
			inventory[slot].Clear();
		}

		if (used) {
			inventory[slot].Clear();
		}
	}

	public string ToString (int type){
		switch (type) {
			case 0:
				return "燃料";
				break;
			case 1:
				return "裝甲";
				break;
			case 2:
				return "食物";
				break;
			case 3:
				return "礦石";
				break;
		}

		return null;
	}

	public void Upgrade(int type) {
		switch (type) {
			case 0:
				if (GetResourceHome(0) >= engineLv * engineNeed && engineLv < 5) {
					ModifyResourceHome(0, -engineLv * engineNeed);
					SetLevel(0, engineLv + 1);
				}
				break;
			case 1:
				if (GetResourceHome(2) >= detectLv * detectNeed && detectLv < 5) {
					ModifyResourceHome(2, -detectLv * detectNeed);
					SetLevel(1, detectLv + 1);
				}
				break;
			case 2:
				if (GetResourceHome(3) >= shipLv * shipNeed && shipLv < 5) {
					ModifyResourceHome(3, -shipLv * shipNeed);
					SetLevel(2, shipLv + 1);
				}
				break;
			case 3:
				if (GetResourceHome(4) >= scopeLv * scopeNeed && scopeLv < 5) {
					ModifyResourceHome(4, -scopeLv * scopeNeed);
					SetLevel(3, scopeLv + 1);
				}
				break;
		}
	}

	public void SetLevel(int type, int value) {
		switch (type) {
			case 0:
				lvText[type].text = value.ToString();
				needText[type].text = (value * engineNeed).ToString();
				engineLv = value;
				break;
			case 1:
				lvText[type].text = value.ToString();
				needText[type].text = (value * detectNeed).ToString();
				detectLv = value;
				break;
			case 2:
				lvText[type].text = value.ToString();
				needText[type].text = (value * shipNeed).ToString();
				shipLv = value;
				break;
			case 3:
				lvText[type].text = value.ToString();
				needText[type].text = (value * scopeNeed).ToString();
				scopeLv = value;
				break;
		}
	}
}

public enum ResourceType {
	Fuel,
	Armor,
	Food,
	Mineral,
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ResourseManager : MonoBehaviour {
	public static SYS_ResourseManager Direct;

	public Texture2D[] itemIcon = new Texture2D[6];
	
	public Text[] resourceText = new Text[5];

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

	public Item[] cargos = { new Item(), new Item() };
	public UI_ItemSlot[] cargo = new UI_ItemSlot[2];
	public UI_ItemSlot[] preCargo = new UI_ItemSlot[2];
	public UI_ItemSlot[] inventory = new UI_ItemSlot[9];

	//-------------------------------home
	public int startFuel_Home = 0;
	public int startFood_Home = 0;
	public int startMineral_Home = 0;
	public int startCoin_Home = 0;
		
	public Text[] lvText = new Text[4];
	public Text[] needText = new Text[4];

	public Image back;
	public bool deleteMode;
	private Color baseColor;
	public Color deleteColor;

	void Awake() {
		Direct = this;

		baseColor = back.color;
	}

	public void Init() {		
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

	public void Restart() {
		SetResource(0, startFuel);
		SetResource(1, startArmor);
		SetResource(2, startFood);
		SetResource(3, startMineral);

		AddCargo(SYS_SaveManager.Direct.GetPrecargo(0));
		AddCargo(SYS_SaveManager.Direct.GetPrecargo(1));
	}

	public void AddCargo(Item item) {
		for (int ct = 0; ct < cargos.Length; ct++) {
			if (cargos[ct].isNull) {
				SetCargo(ct, item);
				return;
			}
		}
	}

	public void SetCargo(int slot, Item item = null) {
		if (item == null) {
			item = new Item();
		}
		cargos[slot] = item;
		SetCargoSlot(slot, item);
	}

	public void SetCargoSlot(int slot , Item item = null) {
		cargo[slot].SetItem(item);
	}

	public int GetResource(int type) {
		return resources[type];
	}

	public void ModifyResource(int type, int value) {
		if (value > 0) {
			SYS_SideLog.Direct.Regist(type, value.ToString());
		}

		SetResource(type, resources[type] + value);
	}

	public void SetResource(int type, int value) {
		bool full = false;
		resources[type] = value;

		if (type == 0) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] >= maxFuel) {
				resources[type] = maxFuel;
				full = true;
			}

			fuelBar.fillAmount = (float)resources[type] / (float)maxFuel * 0.2f;

		} else if (type == 1) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] >= maxArmor) {
				resources[type] = maxArmor;
				full = true;
			}

		} else if (type == 2) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] >= maxFood) {
				resources[type] = maxFood;
				full = true;
			}

			foodBar.fillAmount = (float)resources[type] / (float)maxFood * 0.2f;

		} else if (type == 3) {
			if (resources[type] <= 0) {
				resources[type] = 0;

			} else if (resources[type] >= maxMineral) {
				resources[type] = maxMineral;
				full = true;
			}
		} else {
			Debug.LogError("修改錯誤的資源型態");
		}

		resourceText[type].text = resources[type].ToString();

		if (!full) {
			resourceText[type].color = new Color(0.9f, 0.9f, 0.9f);
		} else {
			resourceText[type].color = new Color(0.9f, 0.7f, 0);
		}
	}
			
	public void UseCargo(int slot) {
		bool used = false;

		if (cargos[slot].typeID == 1) {
			if (cargos[slot].effectID == 2) {//加倍油料
				UI_ScoreManager.Direct.bonusEnd[0] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "95加滿!");

			} else if (cargos[slot].effectID == 3) {//加倍食物
				UI_ScoreManager.Direct.bonusEnd[1] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "看起來好好吃!");				

			} else if (cargos[slot].effectID == 4) {//加倍礦石
				UI_ScoreManager.Direct.bonusEnd[2] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "大顆鑽石!");

			} else if (cargos[slot].effectID == 5) {//加倍硬幣
				UI_ScoreManager.Direct.bonusEnd[3] += 1;
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "請給我黃金!");

			} else if (cargos[slot].effectID == 6) {//偵測雷達
				SYS_ShipController.Direct.detector.SetActive(true);
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "喔喔到處都是隕石呢!");

			} else if (cargos[slot].effectID == 7) {//反射裝甲
				SYS_ShipController.Direct.reflecter.SetActive(true);
				used = true;
				SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "我要撞飛所有壞隕石!");
			}
		}

		if (used) {
			SetCargo(slot);
		}
	}
	
	public void DeteteMode() {
		if (!deleteMode) {
			back.color = deleteColor;
			deleteMode = true;

		} else {
			back.color = baseColor;
			deleteMode = false;
		}
	}

	public void SetPreCargoSlot(int slot, Item item = null) {
		preCargo[slot].SetItem(item);
	}

	public void SetInventorySlot(int slot, Item item = null) {
		inventory[slot].SetItem(item);
	}

	public void UsePreCargo(int slot) {
		if (!SYS_SaveManager.Direct.GetPrecargo(slot).isNull) {
			if (!deleteMode) {
				if (!SYS_SaveManager.Direct.IsInventoryFull()) {
					SYS_SaveManager.Direct.AddInventory(SYS_SaveManager.Direct.GetPrecargo(slot));
					SYS_SaveManager.Direct.SetPrecargo(slot);
				}
			} else {
				SYS_SaveManager.Direct.SetPrecargo(slot);
			}
		}
	}

	public void UseInventory(int slot) {
		if (!SYS_SaveManager.Direct.GetInventory(slot).isNull) {
			if (!deleteMode) {
				if (SYS_SaveManager.Direct.GetInventory(slot).typeID == 3) {
					if (SYS_SaveManager.Direct.GetInventory(slot).effectID == 1) {
						SYS_SaveManager.Direct.AddBullpen(new Member(Random.Range(0, SYS_TeamManager.Direct.headIcons.Count), Random.Range(0, SYS_TeamManager.Direct.bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), Random.Range(1, 2 + SYS_SaveManager.Direct.GetInventory(slot).valueID)));
						SYS_SaveManager.Direct.SetInventory(slot);
					}

				} else if (!SYS_SaveManager.Direct.IsPrecargoFull()) {
					SYS_SaveManager.Direct.AddPrecargo(SYS_SaveManager.Direct.GetInventory(slot));
					SYS_SaveManager.Direct.SetInventory(slot);
				}
			} else {
				SYS_SaveManager.Direct.SetInventory(slot);
			}
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
	
	public void SetLevel(int type, int value) {
		
	}
}

public enum ResourceType {
	Fuel,
	Armor,
	Food,
	Mineral,
}
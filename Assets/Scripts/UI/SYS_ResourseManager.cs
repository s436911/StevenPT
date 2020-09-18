using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ResourseManager : MonoBehaviour {
	public static SYS_ResourseManager Direct;

	public Text fuelText;
	public Text armorText;
	public Text foodText;
	public Text mineralText;

	public int maxFuel = 50;
	public int maxArmor = 5;
	public int maxFood = 5;
	public int maxMineral = 5;

	public int startFuel = 50;
	public int startArmor = 5;
	public int startFood = 5;
	public int startMineral = 5;

	public int[] resources = new int[4];
	public int[] cargo = new int[2];
	
	void Awake() {
		Direct = this;
	}

	void Regist() {

	}

	public void Reset() {
		SetResource(0, startFuel);
		SetResource(1, startArmor);
		SetResource(2, startFood);
		SetResource(3, startMineral);

		cargo[0] = 0;
		cargo[1] = 0;
	}

	public int GetResource(int type) {
		return resources[type];
	}

	public void ModifyResource(int type, int value) {
		SetResource(type, resources[type] + value);
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

			fuelText.text = resources[type].ToString();

		} else if (type == 1) {
			if (resources[type] <= 0) {
				resources[type] = 0;

			} else if (resources[type] > maxArmor) {
				resources[type] = maxArmor;
			}

			armorText.text = resources[type].ToString();

		} else if (type == 2) {
			if (resources[type] <= 0) {
				resources[type] = 0;
				SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

			} else if (resources[type] > maxFood) {
				resources[type] = maxFood;
			}
			foodText.text = resources[type].ToString();

		} else if (type == 3) {
			resources[type] = value;
			if (resources[type] <= 0) {
				resources[type] = 0;

			} else if (resources[type] > maxMineral) {
				resources[type] = maxMineral;
			}
			mineralText.text = resources[type].ToString();
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
}

public enum ResourceType {
	Fuel,
	Armor,
	Food,
	Mineral,
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ResourseManager : MonoBehaviour
{
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
	
	public int[] cargo = new int[2]; 

	private int fuel;
	private int armor;
	private int food;
	private int mineral;

	void Awake() {
		Direct = this;
	}

	void Regist() {
		
	}

	public void Reset() {
		SetFuel(startFuel);
		SetArmor(startArmor);
		SetFood(startFood);
		SetMineral(startMineral);

		cargo[0] = 0;
		cargo[1] = 0;
	}

	public int GetFuel() {
		return fuel;
	}

	public int GetArmor() {
		return armor;
	}

	public int GetFood() {
		return food;
	}

	public int GetMineral() {
		return mineral;
	}

	public void ModifyFuel(int value) {
		SetFuel(fuel + value);
	}

	public void ModifyArmor(int value) {
		SetArmor(armor + value);
	}

	public void ModifyFood(int value) {
		SetFood(food + value);
	}

	public void ModifyMineral(int value) {
		SetMineral(mineral + value);
	}


	public void SetFuel(int value) {
		fuel = value;
		if (fuel <= 0) {
			fuel = 0;
			SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

		} else if (fuel > maxFuel) {
			fuel = startFuel;
		}

		fuelText.text = fuel.ToString();
	}

	public void SetArmor(int value) {
		armor = value;
		if (armor <= 0) {
			armor = 0;

		} else if (armor > maxArmor) {
			armor = startArmor;
		}

		armorText.text = armor.ToString();
	}

	public void SetFood(int value) {
		food = value;
		if (food <= 0) {
			food = 0;
			SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

		} else if (food > maxFood) {
			food = startFood;
		}
		foodText.text = food.ToString();
	}

	public void SetMineral(int value) {
		mineral = value;
		if (mineral <= 0) {
			mineral = 0;

		} else if (mineral > maxMineral) {
			mineral = startMineral;
		}
		mineralText.text = mineral.ToString();
	}
}

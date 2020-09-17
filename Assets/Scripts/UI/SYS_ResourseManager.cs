using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ResourseManager : MonoBehaviour
{
	public static SYS_ResourseManager Direct;

	public Text fuelText;
	public Text armorText;
	public Text tradeText;

	public int startFuel = 50;
	public int startArmor = 5;
	public int startTrade = 5;
	
	public int[] cargo = new int[2]; 

	private int fuel;
	private int armor;
	private int trade;

	void Awake() {
		Direct = this;
	}

	void Regist() {
		
	}

	public void Reset() {
		SetFuel(startFuel);
		SetArmor(startArmor);
		SetTrade(startTrade);

		cargo[0] = 0;
		cargo[1] = 0;
	}

	public int GetFuel() {
		return fuel;
	}

	public int GetArmor() {
		return armor;
	}

	public int GetTrade() {
		return trade;
	}


	public void ModifyFuel(int value) {
		SetFuel(fuel + value);
	}

	public void ModifyArmor(int value) {
		SetArmor(armor + value);
	}

	public void ModifyTrade(int value) {
		SetTrade(trade + value);
	}


	public void SetFuel(int value) {
		fuel = value;
		if (fuel <= 0) {
			fuel = 0;
			SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

		} else if (fuel > startFuel) {
			fuel = startFuel;
		}

		fuelText.text = fuel.ToString();
	}

	public void SetArmor(int value) {
		armor = value;
		if (armor <= 0) {
			armor = 0;

		} else if (armor > startArmor) {
			armor = startArmor;
		}

		armorText.text = armor.ToString();
	}

	public void SetTrade(int value) {
		trade = value;
		if (trade <= 0) {
			trade = 0;

		} else if (trade > startTrade) {
			trade = startTrade;
		}
		tradeText.text = trade.ToString();
	}
}

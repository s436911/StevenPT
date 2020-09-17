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
		SetTrade(startArmor);

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
		fuel += value;
	}

	public void ModifyArmor(int value) {
		armor += value;
	}

	public void ModifyTrade(int value) {
		trade += value;
	}


	public void SetFuel(int value) {
		fuel = value;
		fuelText.text = fuel.ToString();
	}

	public void SetArmor(int value) {
		armor = value;
		armorText.text = armor.ToString();
	}

	public void SetTrade(int value) {
		trade = value;
		tradeText.text = trade.ToString();
	}
}

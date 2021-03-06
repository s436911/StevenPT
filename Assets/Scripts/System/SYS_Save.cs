﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SYS_Save : MonoBehaviour {
	public static SYS_Save Direct;
	[SerializeField]
	private PlayerData gameData;

	public int[] researchNeed = new int[4];

	void Awake() {
		Direct = this;
	}

	public void Init() {
		if (HadFile()) {
			gameData = Load<PlayerData>();			

		} else {
			gameData = new PlayerData();
			SYS_ModeSwitcher.Direct.Stub();
			Save(gameData);
		}
		UpdateUI();
	}

	public void Clear() {
		gameData = new PlayerData();
		SYS_ModeSwitcher.Direct.Stub();
		SYS_Gacha.Direct.Reset();
		Save(gameData);
		UpdateUI();
	}

	public void SaveBTN() {
		Save(gameData);
	}

	public void LoadBTN() {
		gameData = Load<PlayerData>();
		UpdateUI();
	}

	public void UpdateUI() {
		SetResource(0, gameData.resources[0], false);
		SetResource(2, gameData.resources[2], false);
		SetResource(3, gameData.resources[3], false);
		SetResource(4, gameData.resources[4], false);

		for (int ct = 0; ct < gameData.researchs.Length; ct++) {
			SetResearch(ct, gameData.researchs[ct], false);
		}

		for (int ct = 0; ct < gameData.bullpen.Length; ct++) {
			SetBullpen(ct, gameData.bullpen[ct], false);
		}

		for (int ct = 0; ct < gameData.members.Length; ct++) {
			SetMember(ct, gameData.members[ct], false);
		}

		for (int ct = 0; ct < gameData.precargo.Length; ct++) {
			SetPrecargo(ct, gameData.precargo[ct], false);
		}

		for (int ct = 0; ct < gameData.inventory.Length; ct++) {
			SetInventory(ct, gameData.inventory[ct], false);
		}

		SYS_ResourseManager.Direct.UpdateCargobayUI();
		SYS_Gacha.Direct.UpdateGachaUI();
	}

	public bool HadFile(string fileName = "playerSave.dat") {
		string filePath = Application.persistentDataPath + "/" + fileName;
		return File.Exists(filePath);
	}

	public void Save(object gameState, string fileName = "playerSave.dat") {
		string serizliedData = JsonUtility.ToJson(gameState);		
		string filePath = Application.persistentDataPath + "/" + fileName;
		File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(serizliedData));
	}

	public static T Load<T>(string fileName = "playerSave.dat") {
		string filePath = Application.persistentDataPath + "/" + fileName;

		try {
			byte[] serizliedData = File.ReadAllBytes(filePath);
			string serizliedDataStr = Encoding.UTF8.GetString(serizliedData);
			//Debug.LogError(serizliedDataStr);
			return JsonUtility.FromJson<T>(serizliedDataStr);

		} catch (System.IO.FileNotFoundException) {
			return default(T);
		}
	}

	//CargoBay
	public void ModifyCargobay(Item item) {
		if (item.stackAble && gameData.cargobay.Count < SYS_ResourseManager.Direct.cargobay.Count) {
			for (int ct = 0; ct < gameData.cargobay.Count; ct++) {
				//已存在
				if (gameData.cargobay[ct].iconID == item.iconID) {
					gameData.cargobay[ct].stackNum += item.stackNum;
					if (gameData.cargobay[ct].stackNum <= 0) {
						gameData.cargobay.RemoveAt(ct);
					}

					SYS_ResourseManager.Direct.UpdateCargobayUI();
					SYS_Gacha.Direct.UpdateGachaUI();
					SYS_SideLog.Direct.Regist(DB.GetItemTexture(item.iconID), item.stackNum , 1.4F);
					SaveBTN();
					return;
				}
			}
			//不存在
			gameData.cargobay.Add(item);
			SYS_ResourseManager.Direct.UpdateCargobayUI();
			SYS_Gacha.Direct.UpdateGachaUI();
			SYS_SideLog.Direct.Regist(DB.GetItemTexture(item.iconID), item.stackNum, 1.4F);
			SaveBTN();
		}
	}

	public void ResetCargobay() {
		gameData.cargobay = new List<Item>();
		SYS_ResourseManager.Direct.UpdateCargobayUI();
	}

	public List<Item> GetCargobay() {
		return gameData.cargobay;
	}

	public Item GetCargobaySlot(int slotID) {
		if (!gameData.cargobay[slotID].isNull) {
			return new Item(gameData.cargobay[slotID], gameData.cargobay[slotID].stackNum, gameData.cargobay[slotID].valueID);
		}
		return new Item();
	}

	public Item GetCargobayID(int itemID) {
		for (int ct = 0; ct < gameData.cargobay.Count; ct++) {
			//已存在
			if (gameData.cargobay[ct].iconID == itemID) {
				return new Item(gameData.cargobay[ct] , gameData.cargobay[ct].stackNum , gameData.cargobay[ct].valueID) ;
			}
		}
		return new Item();
	}

	//Research
	public int GetResearch(int rdType) {
		return gameData.researchs[rdType];
	}

	public int[] GetResearchs() {
		return gameData.researchs;
	}

	public void SetResearch(int rdType, int value , bool save = true) {
		SYS_ResourseManager.Direct.lvText[rdType].text = value.ToString();
		SYS_ResourseManager.Direct.needText[rdType].text = (value * researchNeed[rdType]).ToString();
		gameData.researchs[rdType] = value;

		if (save) {
			SaveBTN();
		}
	}

	public void ModifyResearch(int rdType , int value) {
		SetResearch(rdType, GetResearch(rdType) + value);
	}

	public void UpgradeResearch(int type) {
		if (type == 0) {
			if (GetResource(type) >= gameData.researchs[type] * researchNeed[type] && gameData.researchs[type] < 5) {
				int cost = gameData.researchs[type] * researchNeed[type];
				SetResearch(type , gameData.researchs[type] + 1);
				ModifyResource(type, -cost);
			}
		} else {
			if (GetResource(type + 1) >= gameData.researchs[type] * researchNeed[type] && gameData.researchs[type] < 5) {
				int cost = gameData.researchs[type] * researchNeed[type];
				SetResearch(type , gameData.researchs[type] + 1);
				ModifyResource(type + 1, -cost);
			}
		}
	}

	//Date
	public string GetDate() {
		return gameData.date;
	}

	public void SetDate(string value, bool save = true) {
		gameData.date = value;

		if (save) {
			SaveBTN();
		}
	}

	//Date
	public int GetGacha(int slotValue) {
		return gameData.gachas[slotValue];
	}

	public void SetGacha(int slotValue, int value, bool save = true) {
		if (value > 0) {
			gameData.gachas[slotValue] = value;
			SYS_Gacha.Direct.UpdateGachaUI();
			if (save) {
				SaveBTN();
			}
		}
	}

	public Item GetPrecargo(int slot) {
		return gameData.precargo[slot];
	}

	public void SetPrecargo(int slot, Item item = null, bool save = true) {
		if (item == null) {
			item = new Item();
		}

		gameData.precargo[slot] = item;
		SYS_ResourseManager.Direct.SetPreCargoSlot(slot, item);

		if (save) {
			SaveBTN();
		}
	}

	public bool IsPrecargoFull() {
		return IsSlotFull(gameData.precargo);
	}

	public void AddPrecargo(Item item) {
		for (int ct = 0; ct < gameData.precargo.Length; ct++) {
			if (gameData.precargo[ct].isNull) {
				SetPrecargo(ct, item);
				return;
			}
		}
	}

	//Inventory
	public Item GetInventory(int slot) {
		return gameData.inventory[slot];
	}

	public void SetInventory(int slot, Item item = null, bool save = true) {
		if (item == null) {
			item = new Item();
		}		

		gameData.inventory[slot] = item;
		SYS_ResourseManager.Direct.SetInventorySlot(slot , item);

		if (save) {
			SaveBTN();
		}
	}

	public bool IsInventoryFull() {
		return IsSlotFull(gameData.inventory);
	}

	public void AddInventory(Item item) {
		for (int ct = 0; ct < gameData.inventory.Length; ct++) {
			if (gameData.inventory[ct].isNull) {
				SetInventory(ct, item);
				return;
			}
		}
	}

	private bool IsSlotFull(Item[] slots) {
		foreach (Item item in slots) {
			if (item.isNull) {
				return false;
			}
		}
		return true;
	}

	//Resource
	public int GetResource(int type) {
		return gameData.resources[type];
	}

	public void SetResource(int type, int value, bool save = true) {
		if (type == 0 || type == 2 || type == 3 || type == 4) {
			gameData.resources[type] = Mathf.Clamp(value, 0, 9999);
			SYS_ResourseManager.Direct.resourceText_Home[type].text = GetResource(type).ToString();

			if (GetResource(type) < gameData.researchs[type != 0 ? type - 1 : type] * researchNeed[type != 0 ? type - 1 : type]) {
				SYS_ResourseManager.Direct.resourceText_Home[type].color = Color.red;
			} else {
				SYS_ResourseManager.Direct.resourceText_Home[type].color = Color.white;
			}


			if (save) {
				SaveBTN();
			}
		} else {
			Debug.LogError("修改錯誤的資源型態");
		}
	}

	public void ModifyResource(int type, int value) {
		SetResource(type, GetResource(type) + value);
	}

	//Bullpen
	public void AddBullpen(Member member) {
		for (int ct = 0; ct < gameData.bullpen.Length; ct++) {
			if (gameData.bullpen[ct].isNull) {
				SetBullpen(ct, member);
				return;
			}
		}
	}

	public bool IsBullpenFull() {
		return IsSlotFull(gameData.bullpen);
	}

	public Member GetBullpen(int slot) {
		return gameData.bullpen[slot];
	}
	
	public void SetBullpen(int slot, Member member = null, bool save = true) {
		if (member == null) {
			member = new Member();
		}

		gameData.bullpen[slot] = member;
		SYS_TeamManager.Direct.SetdBullpenSlot(slot, member);

		if (save) {
			SaveBTN();
		}
	}


	//Members
	public void AddMembers(Member member) {
		for (int ct = 0; ct < gameData.members.Length; ct++) {
			if (gameData.members[ct].isNull) {
				SetMember(ct, member);
				return;
			} 
		}
		SYS_Logger.Direct.SetSystemMsg("隊伍已滿");
	}

	public bool IsMembersFull() {
		return IsSlotFull(gameData.members);
	}

	public Member[] GetMembers() {
		return gameData.members;
	}

	public Member GetMember() {
		return gameData.members[Random.Range(0,4)];
	}

	public int GetMemberNum() {
		return Random.Range(0, 4);
	}

	public int GetMemberNum(Member member) {
		for(int i = 0;i < gameData.members.Length; i++) {
			if (member == gameData.members[i]) {
				return i;
			}
		}
		return 0;
	}

	public Member GetMember(int slot) {
		return gameData.members[slot];
	}

	public void SetMember(int slot, Member member = null, bool save = true) {
		if (member == null) {
			member = new Member();
		}

		gameData.members[slot] = member;
		SYS_TeamManager.Direct.SetMemberSlot(slot, member);

		if (save) {
			SaveBTN();
		}
	}

	public void ModifyMemberAge(int slot, float value , bool save = true) {
		if (gameData.members[slot].isNull) {
			return;
		}

		gameData.members[slot].age += value;
		SYS_TeamManager.Direct.SetMemberSlot(slot, gameData.members[slot]);

		if (save) {
			SaveBTN();
		}
	}

	public bool IsMembersComplete() {
		return !gameData.members[0].isNull && !gameData.members[1].isNull && !gameData.members[2].isNull && !gameData.members[3].isNull;
	}

	/* 0駕駛、1談判、2理智、3運氣*/
	public int GetMembersAttribute(int type) {
		int rt = 0;
		foreach (Member member in gameData.members) {
			if (!member.isNull) {
				rt = rt + member.age < 100 ? member.attribute[type] : (int)Mathf.Ceil(member.attribute[type] * 0.5f) ;
			}
		}
		return (int)(rt * 0.25F);
	}

	private bool IsSlotFull(Member[] slots) {
		foreach (Member member in slots) {
			if (member.isNull) {
				return false;
			}
		}
		return true;
	}
}

[System.Serializable]
public class PlayerData  {
	public string date = "";
	public int[] gachas = new int[3];

	public int[] resources = new int[5];
	public int[] researchs = new int[4];

	public Item[] precargo = new Item[2];
	public Item[] inventory = new Item[9];
	public List<Item> cargobay = new List<Item>();

	public Member[] members = new Member[4];
	public Member[] bullpen = new Member[15];

	public PlayerData() {
		for (int ct = 0; ct < resources.Length; ct++) {
			resources[ct] = 0;
		}

		for (int ct = 0; ct < researchs.Length; ct++) {
			researchs[ct] = 1;
		}

		for (int ct = 0; ct < precargo.Length; ct++) {
			precargo[ct] = new Item();
		}

		for (int ct = 0; ct < inventory.Length; ct++) {
			inventory[ct] = new Item();
		}

		for (int ct = 0; ct < members.Length; ct++) {
			members[ct] = new Member();
		}

		for (int ct = 0; ct < bullpen.Length; ct++) {
			bullpen[ct] = new Member();
		}
	}
}

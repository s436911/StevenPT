using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SYS_SaveManager : MonoBehaviour {
	public static SYS_SaveManager Direct;
	public PlayerData gameData;

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
			SetMembers(ct, gameData.members[ct], false);
		}

		for (int ct = 0; ct < gameData.precargo.Length; ct++) {
			SetPrecargo(ct, gameData.precargo[ct], false);
		}

		for (int ct = 0; ct < gameData.inventory.Length; ct++) {
			SetInventory(ct, gameData.inventory[ct], false);
		}
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

	//Research
	public int GetResearch(int type) {
		return gameData.researchs[type];
	}

	public void SetResearch(int type, int value , bool save = true) {
		SYS_ResourseManager.Direct.lvText[type].text = value.ToString();
		SYS_ResourseManager.Direct.needText[type].text = (value * researchNeed[type]).ToString();
		gameData.researchs[type] = value;

		if (save) {
			SaveBTN();
		}
	}

	public void ModifyResearch(int type, int value) {
		SetResearch(type, GetResearch(type) + value);
	}

	public void UpgradeResearch(int type) {
		if (type == 0) {
			if (GetResource(type) >= gameData.researchs[type] * researchNeed[type] && gameData.researchs[type] < 5) {
				ModifyResource(type, -gameData.researchs[type] * researchNeed[type]);
				SetResearch(type, gameData.researchs[type] + 1);
			}
		} else {
			if (GetResource(type + 1) >= gameData.researchs[type] * researchNeed[type] && gameData.researchs[type] < 5) {
				ModifyResource(type + 1, -gameData.researchs[type] * researchNeed[type]);
				SetResearch(type, gameData.researchs[type] + 1);
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
				SetMembers(ct, member);
				return;
			} 
		}
		SYS_Logger.Direct.SetSystemMsg("隊伍已滿");
	}

	public bool IsMembersFull() {
		return IsSlotFull(gameData.members);
	}

	public Member GetMember() {
		return gameData.members[Random.Range(0,4)];
	}

	public Member GetMember(int slot) {
		return gameData.members[slot];
	}

	public void SetMembers(int slot, Member member = null, bool save = true) {
		if (member == null) {
			member = new Member();
		}

		gameData.members[slot] = member;
		SYS_TeamManager.Direct.SetMemberSlot(slot, member);

		if (save) {
			SaveBTN();
		}
	}

	public bool IsMembersComplete() {
		return !gameData.members[0].isNull && !gameData.members[1].isNull && !gameData.members[2].isNull && !gameData.members[3].isNull;
	}


	public int GetMembersAttribute(int type) {
		int rt = 0;
		foreach (Member member in gameData.members) {
			if (!member.isNull) {
				rt = rt + member.attribute[type];
			}
		}
		return rt;
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
	public int[] resources = new int[5];
	public int[] researchs = new int[4];

	public Item[] precargo = new Item[2];
	public Item[] inventory = new Item[9];

	public Member[] members = new Member[4];
	public Member[] bullpen = new Member[9];

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

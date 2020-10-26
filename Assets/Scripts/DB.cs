using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB : MonoBehaviour {
	private static Dictionary<int, Item> items = new Dictionary<int, Item>();
	
	public static void Init() {
		items.Add(1, new Item(1, 0, 3, true, "石礦"));
		items.Add(2, new Item(2, 0, 3, true, "銀礦"));
		items.Add(3, new Item(3, 0, 3, true, "金礦"));
		items.Add(4, new Item(4, 0, 3, true, "銅礦"));
		items.Add(5, new Item(5, 0, 3, true, "鐵礦"));
		items.Add(6, new Item(6, 0, 3, true, "綠寶石"));
		items.Add(7, new Item(7, 0, 3, true, "藍寶石"));
		items.Add(8, new Item(8, 0, 3, true, "紅寶石"));
		items.Add(9, new Item(9, 0, 3, true, "遠古水晶"));


		items.Add(1001, new Item(1001, 0, 2, true, "花椰菜"));
		items.Add(1002, new Item(1002, 0, 2, true, "詛咒花椰菜"));
		

		items.Add(80001, new Item(80001, 1, 6, false, "這東西似乎可以用來強化探測機!"));
		items.Add(80002, new Item(80002, 1, 3, false, "這似乎可以讓食物變多!"));
		items.Add(80003, new Item(80003, 1, 7, false, "這東西似乎可以用來彈開什麼??"));

		items.Add(80005, new Item(80005, 1, 4, false, "這似乎可以讓礦石變多!"));
		items.Add(80006, new Item(80006, 3, 1, false, "膠囊裡面裡面有個人呢.."));
	}

	public static Item NewItem(int id, int stackNum = 0, int valueID = 0) {
		if (items.ContainsKey(id)) {
			if (items[id].stackAble) {
				return new Item(items[id], stackNum, 0);
			} else {
				return new Item(items[id], 0, valueID);
			}
		}
		return new Item();
	}

	public static Item GetItem(int id) {
		if (items.ContainsKey(id)) {
			return items[id];
		}
		return null;
	}

	public static Texture2D GetItemTexture(int id = 0) {
		if (id > 0 && items.ContainsKey(id)) {
			return Resources.Load<Texture2D>("Icons/" + items[id].iconID.ToString("00000"));
		}
		return Resources.Load<Texture2D>("Icons/00000");
	}

	public static Material GetItemMaterial(int id = 0) {
		if (id > 0 && items.ContainsKey(id)) {
			return Resources.Load<Material>("Mats/" + items[id].iconID.ToString("00000"));
		}
		return null;
	}
}

	[System.Serializable]
public class Item {
	public bool isNull = false;
	public int iconID;
	public int typeID; //0資源、1主動使用效果型、2主動使用buff型、3家中使用
	public int effectID; 
	public string text;
	public bool stackAble;

	//特殊生成值
	public int valueID;

	//儲存值
	public int stackNum;

	public Item() {
		isNull = true;
	}

	public Item(int iconID, int typeID, int effectID, bool stackAble, string text = null) {
		isNull = false;
		this.iconID = iconID;
		this.typeID = typeID;
		this.effectID = effectID;
		this.text = text;
		this.stackAble = stackAble;
	}

	//特殊生成用
	public Item(Item value , int stackNum = 0 , int valueID = 0) {
		isNull = false;
		this.iconID = value.iconID;
		this.typeID = value.typeID;
		this.effectID = value.effectID;
		this.text = value.text;
		this.stackAble = value.stackAble;

		this.stackNum = stackNum;
		this.valueID = valueID;
	}
}

/*
 * [1]
 * 1加速器
 * 2加倍油料
 * 3加倍食物
 * 4加倍礦石
 * 5加倍硬幣
 * 6偵測雷達
 * 7反射裝甲
*/

/*
 * [2]
 
*/

/*
 * [3]
 * 1人員轉蛋
*/


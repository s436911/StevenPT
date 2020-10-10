using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
	public int iconID;
	public int typeID; //1主動使用效果型、2主動使用buff型、3家中使用
	public int effectID; 
	public int valueID;
	public string text;

	public Item(int iconID , int typeID , int effectID , int valueID = 0 , string text = null) {
		this.iconID = iconID;
		this.typeID = typeID;
		this.effectID = effectID;
		this.valueID = valueID;
		this.text = text;
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


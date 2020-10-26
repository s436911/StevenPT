using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Gacha : MonoBehaviour {
	public static SYS_Gacha Direct;
	public Transform entityGroup;
	public GameObject pfbEgg;
	public Vector2 force;

	public List<int> itemIds = new List<int>();
	public List<RawImage> itemIcons = new List<RawImage>();
	public List<Text> itemNeeds = new List<Text>();
	public List<Text> itemHaves = new List<Text>();
	public List<Texture2D> eggTexture = new List<Texture2D>();

	public List<int> randList = new List<int>();

	void Awake() {
		Direct = this;
	}
	
	public void Init() {
		itemIds.Add(0);
		itemIds.Add(0);
		itemIds.Add(0);
		Reset();
		/*
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)] , 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(randList[Random.Range(0, randList.Count)], 10));
		*/
	}

	public void Reset() {
		ReQuest(0);
		ReQuest(1);
		ReQuest(2);
	}

	public void ReQuest(int slot) {
		SetQuest(slot, randList[Random.Range(0,randList.Count)]);
	}

	public void SetQuest(int slot , int itemId = 0) {
		if (itemId != 0) {
			itemIds[slot] = itemId;
		}
	
		itemIcons[slot].texture = DB.GetItemTexture(itemIds[slot]);
		itemNeeds[slot].text = ((slot * 4) + 2).ToString();
		itemHaves[slot].text = SYS_Save.Direct.GetCargobay(itemIds[slot]).stackNum.ToString();
		if (SYS_Save.Direct.GetCargobay(itemIds[slot]).stackNum < ((slot * 4) + 2)) {
			itemHaves[slot].color = Color.red;
		} else {
			itemHaves[slot].color = Color.white;
		}
	}

	public void ReturnQuest(int slot) {
		if (SYS_Save.Direct.GetCargobay(itemIds[slot]).stackNum >= ((slot * 4) + 2)) {
			Gacha(slot);
			SYS_Save.Direct.ModifyCargobay(DB.NewItem(itemIds[slot], -((slot * 4) + 2)));
			SetQuest(slot);
		}
	}

	public void Gacha(int slot) {
		Member newHero = new Member(Random.Range(0, SYS_TeamManager.Direct.headIcons.Count), Random.Range(0, SYS_TeamManager.Direct.bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), Random.Range(slot, slot * 2 + 3));
		SYS_Save.Direct.AddBullpen(newHero);

		RectTransform egg =  Instantiate(pfbEgg).GetComponent<RectTransform>();
		egg.SetParent(entityGroup);
		egg.anchoredPosition = new Vector2(0, 0);
		egg.GetComponent<UI_Gacha>().Regist(new Vector2(Random.Range(-force.x , force.x) ,Random.Range(force.y, force.y * 1.2f)));
		foreach (Transform tran in egg.transform) {
			if (tran.name == "head") {
				tran.GetComponent<RawImage>().texture = SYS_TeamManager.Direct.headIcons[newHero.headID];
			} else if (tran.name == "body") {
				tran.GetComponent<RawImage>().texture = SYS_TeamManager.Direct.bodyIcons[newHero.bodyID];
			} else if (tran.name == "shield") {
				tran.GetComponent<RawImage>().texture = eggTexture[slot];
			}
		}
	}
}

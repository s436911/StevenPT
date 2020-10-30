using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Gacha : MonoBehaviour {
	public static SYS_Gacha Direct;
	public Transform entityGroup;
	public GameObject pfbEgg;
	public Vector2 force;

	public List<RawImage> itemIcons = new List<RawImage>();
	public List<Text> itemNeeds = new List<Text>();
	public List<Text> itemHaves = new List<Text>();
	public List<Texture2D> eggTexture = new List<Texture2D>();

	public List<int> randList = new List<int>();
	public bool changeDate = false;

	void Awake() {
		Direct = this;
	}
	
	public void Init() {

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

		CheckDate();
	}

	public void CheckDate() {
		string date = System.DateTime.UtcNow.ToShortDateString().ToString();
		if (SYS_Save.Direct.GetDate() != date || changeDate) {
			SYS_Save.Direct.SetDate(date);
			ReQuest(0);
			ReQuest(1);
			ReQuest(2);
			Debug.LogWarning("ChangeDate:" + date);

		} else {
			Debug.LogWarning("NonChangeDate:" + date);
		}
	}

	public void Reset() {
		ReQuest(0);
		ReQuest(1);
		ReQuest(2);
	}

	public void ReQuest(int slot) {
		SYS_Save.Direct.SetGacha(slot, randList[Random.Range(0, randList.Count)]);
	}


	public void ReturnQuest(int slot) {
		int itemId = SYS_Save.Direct.GetGacha(slot);

		if (SYS_Save.Direct.GetCargobay(itemId).stackNum >= ((slot * 4) + 2)) {
			Gacha(slot);
			SYS_Save.Direct.ModifyCargobay(DB.NewItem(itemId, -((slot * 4) + 2)));
			//SetQuest(slot);
		}
	}

	public void Gacha(int slot) {
		Member newHero = new Member(Random.Range(0, SYS_TeamManager.Direct.headIcons.Count), Random.Range(0, SYS_TeamManager.Direct.bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), Random.Range(slot, slot * 2 + 3));
		SYS_Save.Direct.AddBullpen(newHero);

		RectTransform egg =  Instantiate(pfbEgg).GetComponent<RectTransform>();
		egg.SetParent(entityGroup);
		egg.anchoredPosition = new Vector2(0, 0);
		egg.GetComponent<UI_Gacha>().Regist(new Vector2(Random.Range(-force.x , force.x) ,Random.Range(force.y, force.y * 1.2f)));
		egg.transform.localScale = Vector2.one;

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

	public void UpdateGachaUI() {
		for (int ct = 0; ct < itemIcons.Count; ct++) {
			int itemId = SYS_Save.Direct.GetGacha(ct);

			itemIcons[ct].texture = DB.GetItemTexture(itemId);
			itemNeeds[ct].text = ((ct * 4) + 2).ToString();
			itemHaves[ct].text = SYS_Save.Direct.GetCargobay(itemId).stackNum.ToString();

			if (SYS_Save.Direct.GetCargobay(itemId).stackNum < ((ct * 4) + 2)) {
				itemHaves[ct].color = Color.red;
			} else {
				itemHaves[ct].color = Color.white;
			}
		}
	}
}

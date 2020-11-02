using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_TeamManager : MonoBehaviour {
	public static SYS_TeamManager Direct;

	public Transform panelMeb;
	public GameObject pfbMeb;

	public float widthItem = 110;
	public float highItem = 110;

	public List<Texture2D> headIcons = new List<Texture2D>();
	public List<Texture2D> bodyIcons = new List<Texture2D>();

	public List<string> manName = new List<string>();
	public List<string> womanName = new List<string>();

	public UI_Member[] bullpen = new UI_Member[15];
	public UI_Member[] members = new UI_Member[4];

	public Image back;
	public bool deleteMode;
	private Color baseColor;
	public Color deleteColor;

	void Awake() {
		Direct = this;
		
		for (int ct = 0; ct < members.Length; ct++) {
			members[ct].Init(ct);
		}

		for (int y = 0; y < 5; y++) {
			for (int x = 0; x < 3; x++) {
				UI_Member objTmp = Instantiate(pfbMeb).GetComponent<UI_Member>();
				objTmp.transform.SetParent(panelMeb);
				objTmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthItem * x, highItem * -y);
				objTmp.transform.localScale = Vector2.one;
				objTmp.Init(y * 3 + x);
				bullpen[y * 3 + x] = objTmp;
				//cargobay.Add(objTmp);
			}
		}

		baseColor = back.color;
	}

	public void Init() {
		
	}

	public void SetdBullpenSlot(int slot, Member member) {
		bullpen[slot].SetMember(member);
	}

	public void SetMemberSlot(int slot, Member member) {
		members[slot].SetMember(member);
	}

	public void UseMember(int slot) {
		if (!SYS_Save.Direct.GetMember(slot).isNull) {
			if (!deleteMode) {
				if (!SYS_Save.Direct.IsBullpenFull()) {
					SYS_Save.Direct.AddBullpen(SYS_Save.Direct.GetMember(slot));
					SYS_Save.Direct.SetMember(slot);
				}
			} else {
				SYS_Save.Direct.SetMember(slot);
			}
		}
	}

	public void UseBullpen(int slot) {
		if (!SYS_Save.Direct.GetBullpen(slot).isNull) {
			if (!deleteMode) {
				if (!SYS_Save.Direct.IsMembersFull()) {
					SYS_Save.Direct.AddMembers(SYS_Save.Direct.GetBullpen(slot));
					SYS_Save.Direct.SetBullpen(slot);
				}
			} else {
				SYS_Save.Direct.SetBullpen(slot);
			}
		}
	}

	public void DeteteMode() {
		if (!deleteMode) {
			back.color = deleteColor;
			deleteMode = true;

		} else {
			back.color = baseColor;
			deleteMode = false;
		}
	}
}

[System.Serializable]
public class Member {
	public bool isNull = false;
	public string name;
	public int rarity;
	public int lv;
	public int headID;
	public int bodyID;
	public int sex;
	public int age;
	public NatureType nature;	

	public int[] attribute = {1,1,1,1};

	public Member() {
		isNull = true;
	}

	public Member(int headID , int bodyID , int sex, NatureType nature , int lv , int rarity = 0, int age = 0) {
		isNull = false;
		if (sex == 0) {
			this.name = SYS_TeamManager.Direct.womanName[Random.Range(0, SYS_TeamManager.Direct.womanName.Count)];
		} else {
			this.name = SYS_TeamManager.Direct.manName[Random.Range(0, SYS_TeamManager.Direct.manName.Count)];
		}
		this.rarity = rarity;
		this.headID = headID;
		this.bodyID = bodyID;
		this.nature = nature;
		this.sex = sex;
		this.age = age == 0 ? Random.Range(15,21) : age;
		this.lv = lv;

		for (int ct = 0; ct < lv + 1;ct++) {
			AddAttribute();
		}
	}

	public void AddAttribute() {
		int tmp = Random.Range(0, 4);
		attribute[tmp] += 1;
	}
}

public enum NatureType {
	Careful,//慎重
	Rash,//馬虎

	Brave,//勇敢
	Timid,//膽小
}
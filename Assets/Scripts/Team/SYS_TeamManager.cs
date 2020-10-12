using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_TeamManager : MonoBehaviour {
	public static SYS_TeamManager Direct;

	public List<Texture2D> headIcons = new List<Texture2D>();
	public List<Texture2D> bodyIcons = new List<Texture2D>();

	public List<string> manName = new List<string>();
	public List<string> womanName = new List<string>();

	public UI_Member[] bullpen = new UI_Member[9];
	public UI_Member[] members = new UI_Member[4];

	public Image back;
	public bool deleteMode;
	private Color baseColor;
	public Color deleteColor;

	void Awake() {
		Direct = this;

		for (int ct = 0; ct < bullpen.Length; ct++) {
			bullpen[ct].Init(ct);
		}

		for (int ct = 0; ct < members.Length; ct++) {
			members[ct].Init(ct);
		}

		baseColor = back.color;
	}

	public void SetdBullpenSlot(int slot, Member member) {
		bullpen[slot].SetMember(member);
	}

	public void SetMemberSlot(int slot, Member member) {
		members[slot].SetMember(member);
	}

	public void UseMember(int slot) {
		if (!SYS_SaveManager.Direct.GetMember(slot).isNull) {
			if (!deleteMode) {
				if (!SYS_SaveManager.Direct.IsBullpenFull()) {
					SYS_SaveManager.Direct.AddBullpen(SYS_SaveManager.Direct.GetMember(slot));
					SYS_SaveManager.Direct.SetMembers(slot);
				}
			} else {
				SYS_SaveManager.Direct.SetMembers(slot);
			}
		}
	}

	public void UseBullpen(int slot) {
		if (!SYS_SaveManager.Direct.GetBullpen(slot).isNull) {
			if (!deleteMode) {
				if (!SYS_SaveManager.Direct.IsMembersFull()) {
					SYS_SaveManager.Direct.AddMembers(SYS_SaveManager.Direct.GetBullpen(slot));
					SYS_SaveManager.Direct.SetBullpen(slot);
				}
			} else {
				SYS_SaveManager.Direct.SetBullpen(slot);
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
	public int headID;
	public int bodyID;
	public int sex;
	public NatureType nature;	

	public int[] attribute = {1,1,1,1};

	public Member() {
		isNull = true;
	}

	public Member(int headID , int bodyID , int sex, NatureType nature , int lv) {
		isNull = false;
		if (sex == 0) {
			this.name = SYS_TeamManager.Direct.womanName[Random.Range(0, SYS_TeamManager.Direct.womanName.Count)];
		} else {
			this.name = SYS_TeamManager.Direct.manName[Random.Range(0, SYS_TeamManager.Direct.manName.Count)];
		}

		this.headID = headID;
		this.bodyID = bodyID;
		this.nature = nature;
		this.sex = sex;
		
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
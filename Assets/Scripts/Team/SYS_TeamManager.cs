using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_TeamManager : MonoBehaviour {
	public static SYS_TeamManager Direct;

	public List<Texture2D> headIcons = new List<Texture2D>();
	public List<Texture2D> bodyIcons = new List<Texture2D>();

	public List<string> manName = new List<string>();
	public List<string> womanName = new List<string>();

	public UI_Member[] bullpen = new UI_Member[9];
	public UI_Member[] members = new UI_Member[4];

	void Awake() {
		Direct = this;

		for (int ct = 0; ct < bullpen.Length; ct++) {
			bullpen[ct].Init(ct);
		}

		for (int ct = 0; ct < members.Length; ct++) {
			members[ct].Init(ct);
		}

		members[0].SetMember(new Member(Random.Range(0, headIcons.Count), Random.Range(0, bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), 1));
		members[1].SetMember(new Member(Random.Range(0, headIcons.Count), Random.Range(0, bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), 1));
		members[2].SetMember(new Member(Random.Range(0, headIcons.Count), Random.Range(0, bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), 1));
		members[3].SetMember(new Member(Random.Range(0, headIcons.Count), Random.Range(0, bodyIcons.Count), Random.Range(0, 2), (NatureType)Random.Range(0, 5), 1));
	}

	public void UseMember(int slot) {
		AddBullpen(members[slot].member);
		members[slot].Clear();
	}

	public void UseBullpen(int slot) {
		if (bullpen[slot].member != null) {
			if (members[0].member == null || members[1].member == null || members[2].member == null || members[3].member == null ) {
				AddMember(bullpen[slot].member);
				bullpen[slot].Clear();
			}
		}
	}

	public void AddMember(Member member) {
		for (int ct = 0; ct < members.Length; ct++) {
			if (members[ct].member == null) {
				SetMemberSlot(ct, member);
				return;
			}
		}
	}

	public void AddBullpen(Member member) {
		for (int ct = 0; ct < bullpen.Length; ct++) {
			if (bullpen[ct].member == null) {
				SetdBullpenSlot(ct, member);
				return;
			}
		}
	}

	public void SetdBullpenSlot(int slot, Member member = null) {
		bullpen[slot].SetMember(member);
	}
	
	public void SetMemberSlot(int slot, Member member = null) {
		members[slot].SetMember(member);
	}

	public bool IsTeamComplete() {
		return members[0].member != null && members[1].member != null && members[2].member != null && members[3].member != null;
	}

	public int GetStr() {
		int rt = 0;
		foreach (UI_Member member in members) {
			if (member.member != null) {
				rt = rt + member.member.v_str;
			}
		}
		return rt;
	}

	public int GetLuk() {
		int rt = 0;
		foreach (UI_Member member in members) {
			if (member.member != null) {
				rt = rt + member.member.v_luk;
			}
		}
		return rt;
	}

	public int GetAGI() {
		int rt = 0;
		foreach (UI_Member member in members) {
			if (member.member != null) {
				rt = rt + member.member.v_agi;
			}
		}
		return rt;
	}

	public int GetInt() {
		int rt = 0;
		foreach (UI_Member member in members) {
			if (member.member != null) {
				rt = rt + member.member.v_int;
			}
		}
		return rt;
	}
}

public class Member {
	public string name;
	public int headID;
	public int bodyID;
	public int sex;
	public NatureType nature;

	public int v_str = 1; //影響戰鬥
	public int v_agi = 1; //影響船隻
	public int v_int = 1; //影響交易
	public int v_luk = 1; //影響機率

	public Member(int headID , int bodyID , int sex, NatureType nature , int lv) {

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
		int tmp = Random.Range(0, 5);
		if (tmp == 0) {
			v_str += 1;
		} else if (tmp == 1) {
			v_agi += 1;
		} else if (tmp == 2) {
			v_int += 1;
		} else {
			v_luk += 1;
		}
	}
}

public enum NatureType {
	Careful,//慎重
	Rash,//馬虎

	Brave,//勇敢
	Timid,//膽小
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Member : MonoBehaviour {
	public int slot;
	public RawImage headIcon;
	public RawImage bodyIcon;
	public Text text;
	public Text t_str;
	public Text t_agi;
	public Text t_int;
	public Text t_luk;
	public Text skill1;

	public void Init(int slot) {
		this.slot = slot;
		Clear();
	}

	public void SetMember(Member member) {
		if (!member.isNull) {
			headIcon.texture = SYS_TeamManager.Direct.headIcons[member.headID];
			bodyIcon.texture = SYS_TeamManager.Direct.bodyIcons[member.bodyID];
			headIcon.color = Color.white;
			bodyIcon.color = Color.white;
			text.text = member.name;

			if (t_str) {
				t_str.text = "str " + member.attribute[0].ToString();
				t_agi.text = "agi " + member.attribute[1].ToString();
				t_int.text = "int " + member.attribute[2].ToString();
				t_luk.text = "luk " + member.attribute[3].ToString();

				skill1.text = "";
			}
		} else {
			headIcon.texture = null;
			bodyIcon.texture = null;
			headIcon.color = Color.clear;
			bodyIcon.color = Color.clear;
			text.text = "N/A";

			if (t_str) {
				t_str.text = "str --";
				t_agi.text = "agi --";
				t_int.text = "int --";
				t_luk.text = "luk --";

				skill1.text = "";
			}
		}
	}

	public void Clear() {
		SetMember(new Member());
	}

	public void UseBullpen() {
		SYS_TeamManager.Direct.UseBullpen(slot);
	}

	public void UseMember() {
		SYS_TeamManager.Direct.UseMember(slot);
	}
}

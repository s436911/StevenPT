using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Member : MonoBehaviour {
	public int slot;
	public RawImage headIcon;
	public RawImage bodyIcon;
	public Text text;
	public Text t_lv;
	public Text t_status;
	public Text t_str;
	public Text t_agi;
	public Text t_int;
	public Text t_luk;
	public Text skill1;

	private static Color[] rarityColor = { new Color(0, 0, 0), new Color(0.6f, 1, 0.45f), new Color(0.5f, 0.75f, 0), new Color(1, 0.6f, 0.3f), new Color(1, 0.35f, 0.25f) };

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
			t_lv.text = member.lv.ToString();

			if (t_str) {
				t_str.text = "駕駛 " + member.attribute[0].ToString();
				t_agi.text = "談判 " + member.attribute[1].ToString();
				t_int.text = "理智 " + member.attribute[2].ToString();
				t_luk.text = "運氣 " + member.attribute[3].ToString();
				t_status.text = member.age + "歲 " + (member.sex == 0 ? "女" : "男");
				skill1.text = "";
			}
		} else {
			headIcon.texture = null;
			bodyIcon.texture = null;
			headIcon.color = Color.clear;
			bodyIcon.color = Color.clear;
			text.text = "N/A";
			t_lv.text = "";

			if (t_str) {
				t_str.text = "駕駛 --";
				t_agi.text = "談判 --";
				t_int.text = "理智 --";
				t_luk.text = "運氣 --";
				t_status.text = "";
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

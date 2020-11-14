using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Member : MonoBehaviour {
	public int slot;
	public Animator anim;
	public RawImage headIcon;
	public RawImage bodyIcon;
	public RawImage emojiIcon;
	public RawImage emoji2Icon;
	public Text text;
	public Text t_lv;
	public Text t_status;
	public Text t_str;
	public Text t_agi;
	public Text t_int;
	public Text t_luk;
	public Text t_nature;
	public Text skill1;

	private float emojiTimer;

	private static Color[] rarityColor = { new Color(0, 0, 0), new Color(0.6f, 1, 0.45f), new Color(0.5f, 0.75f, 0), new Color(1, 0.6f, 0.3f), new Color(1, 0.35f, 0.25f) };

	public void Init(int slot) {
		this.slot = slot;
		Clear();
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (Time.timeSinceLevelLoad - emojiTimer > 5) {
				emojiTimer = Time.timeSinceLevelLoad;
				SetEmoji();
			}
		}
	}

	public void SetMember(Member member) {
		SetEmoji();

		if (!member.isNull) {
			headIcon.texture = SYS_TeamManager.Direct.headIcons[member.headID];
			bodyIcon.texture = SYS_TeamManager.Direct.bodyIcons[member.bodyID];
			headIcon.color = Color.white;
			bodyIcon.color = Color.white;

			if (t_lv) {
				text.text = member.name;
				t_lv.text = member.lv.ToString();
			}

			if (t_str) {
				if (member.age < 100) {
					t_str.text = "駕駛 " + member.attribute[0].ToString();
					t_agi.text = "談判 " + member.attribute[1].ToString();
					t_int.text = "理智 " + member.attribute[2].ToString();
					t_luk.text = "運氣 " + member.attribute[3].ToString();

					t_str.color = Color.white;
					t_agi.color = Color.white;
					t_int.color = Color.white;
					t_luk.color = Color.white;
				} else {
					t_str.text = "駕駛 " + Mathf.Ceil(member.attribute[0] * 0.5f).ToString("f0");
					t_agi.text = "談判 " + Mathf.Ceil(member.attribute[1] * 0.5f).ToString("f0");
					t_int.text = "理智 " + Mathf.Ceil(member.attribute[2] * 0.5f).ToString("f0");
					t_luk.text = "運氣 " + Mathf.Ceil(member.attribute[3] * 0.5f).ToString("f0");

					t_str.color = SYS_TeamManager.Direct.oldColor;
					t_agi.color = SYS_TeamManager.Direct.oldColor;
					t_int.color = SYS_TeamManager.Direct.oldColor;
					t_luk.color = SYS_TeamManager.Direct.oldColor;
				}

				t_status.text = member.age.ToString("F0") + "歲 " + (member.sex == 0 ? "女" : "男");
				t_nature.text = member.GetNatureName();
				skill1.text = "";
			}
		} else {
			headIcon.texture = null;
			bodyIcon.texture = null;
			headIcon.color = Color.clear;
			bodyIcon.color = Color.clear;

			if (t_lv) {
				text.text = "N/A";
				t_lv.text = "";
			}

			if (t_str) {
				t_str.text = "駕駛 --";
				t_agi.text = "談判 --";
				t_int.text = "理智 --";
				t_luk.text = "運氣 --";

				t_str.color = Color.white;
				t_agi.color = Color.white;
				t_int.color = Color.white;
				t_luk.color = Color.white;

				t_status.text = "";
				t_nature.text = "";
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

	public void SetEmoji() {
		SetEmoji(null, Color.clear);
	}

	public void SetEmoji(Texture texture , Color color , EmojiType emoji = EmojiType.None) {
		if (texture) {
			emojiTimer = Time.timeSinceLevelLoad;

			emojiIcon.texture = texture;
			emoji2Icon.texture = texture;
			emojiIcon.color = Color.black;
			emoji2Icon.color = color;

			if (emoji != EmojiType.None) {
				anim.Play(emoji.ToString());
			}
		} else {
			emojiIcon.color = Color.clear;
			emoji2Icon.color = Color.clear;
		}
	}
}

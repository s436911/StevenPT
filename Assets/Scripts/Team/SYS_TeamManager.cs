using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_TeamManager : MonoBehaviour {
	public static SYS_TeamManager Direct;

	public Transform panelBull;
	public Transform panelMeb;
	public Transform panelMeb_game;
	public GameObject pfbBull;
	public GameObject pfbMeb;
	public GameObject pfbMeb_game;

	public float widthItem = 110;
	public float highItem = 110;

	public float widthMeb_game = 110;

	public List<Texture2D> headIcons = new List<Texture2D>();
	public List<Texture2D> bodyIcons = new List<Texture2D>();

	public List<string> manName = new List<string>();
	public List<string> womanName = new List<string>();

	public UI_Member[] bullpen = new UI_Member[15];
	public UI_Member[] members_home = new UI_Member[4];
	public UI_Member[] members_game = new UI_Member[4];

	public Image back;
	public bool deleteMode;
	private Color baseColor;
	public Color deleteColor;
	public Color oldColor;
	public Text moraleText;
	public Image moraleImage;

	public Color fullColor;
	public Color highColor;
	public Color lowColor;
	public Color noneColor;

	public Texture2D moraleIcon;
	public Texture[] emojiTexture;
	public Color[] emojiColor;
	public string[] emojiAnim;

	public int morale = 3;
	public float eventTimer;

	void Awake() {
		Direct = this;
		
		for (int y = 0; y < 5; y++) {
			for (int x = 0; x < 3; x++) {
				UI_Member objTmp = Instantiate(pfbBull).GetComponent<UI_Member>();
				objTmp.transform.SetParent(panelBull);
				objTmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthItem * x, highItem * -y);
				objTmp.transform.localScale = Vector2.one;
				objTmp.Init(y * 3 + x);
				bullpen[y * 3 + x] = objTmp;
			}
		}

		for (int x = 0; x < members_home.Length; x++) {
			UI_Member objTmp = Instantiate(pfbMeb).GetComponent<UI_Member>();
			objTmp.transform.SetParent(panelMeb);
			objTmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, highItem * -x);
			objTmp.transform.localScale = Vector2.one;
			objTmp.Init(x);
			members_home[x] = objTmp;
		}

		for (int x = 0; x < members_game.Length; x++) {
			UI_Member objTmp = Instantiate(pfbMeb_game).GetComponent<UI_Member>();
			objTmp.transform.SetParent(panelMeb_game);
			objTmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthMeb_game * x, 0);
			objTmp.transform.localScale = Vector2.one;
			objTmp.Init(x);
			members_game[x] = objTmp;
		}
		baseColor = back.color;
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {			
			if (Time.timeSinceLevelLoad - eventTimer > 10) {
				eventTimer = Time.timeSinceLevelLoad;
				if (IsNoneMorale() && Random.Range(0, 100) < 16) {
					SYS_ShipController.Direct.OnEndMove();
					Talk(3, SYS_Save.Direct.GetMember(), "不小心踩到煞車...030");

				} else if (IsLowMorale() && Random.Range(0, 100) < 8) {
					SYS_ShipController.Direct.OnEndMove();
					Talk(3, SYS_Save.Direct.GetMember(), "不小心踩到煞車...030");
				}
			}
		}
	}
	public bool IsFullMorale() {
		return morale == 100;
	}

	public bool IsHighMorale() {
		return morale >= 70;
	}

	public bool IsNoneMorale() {
		return morale == 0;
	}

	public bool IsLowMorale() {
		return morale <= 30;
	}

	public void Restart() {
		SetMorale(60);
	}

	public void SetdBullpenSlot(int slot, Member member) {
		bullpen[slot].SetMember(member);
	}

	public void SetMemberSlot(int slot, Member member) {
		members_home[slot].SetMember(member);
		members_game[slot].SetMember(member);
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

	public void Talk(int emoji, string msg) {
		int memNum = SYS_Save.Direct.GetMemberNum();
		members_game[memNum].SetEmoji(emojiTexture[emoji], emojiColor[emoji], (EmojiType)emoji);
		SYS_PopupManager.Direct.Regist(SYS_Save.Direct.GetMember(memNum).name, msg);
	}

	public void Talk(int emoji, Member mem, string msg) {
		int memNum = SYS_Save.Direct.GetMemberNum(mem);
		members_game[memNum].SetEmoji(emojiTexture[emoji], emojiColor[emoji], (EmojiType)emoji);
		SYS_PopupManager.Direct.Regist(mem.name, msg);
	}

	public bool TriggerEvent(int eventID) {
		Member[] members = SYS_Save.Direct.GetMembers();

		switch (eventID) {
			case 1://通用傾向
				for (int i = 0; i < members.Length;i++) {
					if (Trigger(members[i], NatureType.Brave, 5)) {
						ModifyMorale(1);
						Talk(1, members[i], "喔喔衝阿!!");
						return true;
					}
				}
				break;
			case 2://攻擊傾向
				break;
			case 3://探索傾向
				break;
			case 4://交易傾向
				break;
			case 5://一般傾向
				break;
			case 6://放棄負面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Brave, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "討厭逃跑..");
						return true;
					}
				}
				break;
			case 7://放棄正面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Timid, 5)) {
						ModifyMorale(1);
						Talk(1, members[i], "安全第一!!");
						return true;
					}
				}
				break;
			case 11://失控
				break;
			case 12://移動正面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Hasty, 5)) {
						ModifyMorale(1);
						Talk(1, members[i], "Go!Go!");
						return true;
					}
				}
				break;
			case 13://移動負面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Relaxed, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "好累...");
						return true;
					}
				}
				break;
			case 14://靜止正面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Relaxed, 5)) {
						ModifyMorale(1);
						Talk(1, members[i], "zzZ~");
						return true;
					}
				}
				break;
			case 15://靜止負面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Hasty, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "好悶喔...");
						return true;
					}
				}
				break;

			case 21://士氣無視
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Optimistic, 5)) {
						Talk(4, members[i], "下一次會更好!");
						return true;
					}
				}
				break;
			case 22://士氣低落
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Pessimistic, 5)) {
						Talk(3, members[i], "嗚嗚嗚...!");
						return true;
					}
				}
				break;
			case 23://失誤
				break;

			case 31://補滿油正面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Timid, 5)) {
						ModifyMorale(1);
						Talk(1, members[i], "萬全準備!!");
						return true;
					}
				}
				break;
			case 32://補滿油負面
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Relaxed, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "再讓我睡一下拉...");
						return true;
					}
				}
				break;
			case 33://重複登陸
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Hasty, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "我們還要待多久??");
						return true;
					}
				}
				break;
			case 34://出發
				break;

			case 41://撞擊隕石迴避
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Careful, 5)) {
						Talk(2, members[i], "沒事~我閃掉了!");
						return true;
					}
				}
				break;

			case 42://撞擊隕石命中
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Rash, 10)) {
						Talk(4, members[i], "阿!");
						return true;
					}
				}
				break;
			case 43://雲霧
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Timid, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "看不見RRRRR!");
						return true;
					}
					
					if (Trigger(members[i], NatureType.Pessimistic, 5)) {
						ModifyMorale(-1);
						Talk(3, members[i], "會不會撞到東西啊!");
						return true;
					}
				}
				break;
			case 44://受擊低落
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Careful, 10)) {
						Talk(3, members[i], "明明很小心了...");
						return true;
					}

					if (Trigger(members[i], NatureType.Timid, 5)) {
						Talk(3, members[i], "RRRRRR");
						return true;
					}
				}
				break;
			case 45://受擊無視
				for (int i = 0; i < members.Length; i++) {
					if (Trigger(members[i], NatureType.Rash, 5)) {
						Talk(4, members[i], "怎麼了!!??");
						return true;
					}
				}
				break;
		}

		return false;

		/*
		Careful,//細心
		Rash,//粗心

		Brave,//勇敢
		Timid,//膽小

		Optimistic,//樂觀
		Pessimistic,//悲觀

		Hasty,//性急
		Relaxed//悠閒
		*/
	}

	public bool Trigger(Member mem, NatureType nature, float rate) {
		if (mem.nature == nature && Random.Range(0, 100) < rate) {
			return true;
		}
		return false;
	}

	public void ModifyMorale(int value) {
		if (value < 0 ) {
			if (TriggerEvent(21)) {
				value = value + 1;

			} else if (TriggerEvent(22)) {
				value = value - 1;
			}
		}

		SYS_SideLog.Direct.Regist(moraleIcon, value);
		SetMorale(morale + value);
	}

	public void SetMorale(int value) {
		morale = Mathf.Clamp(value, 0, 100);
		moraleText.text = morale.ToString();
		if (IsFullMorale()) {
			moraleText.color = fullColor;
			moraleImage.color = fullColor;

		} else if (IsHighMorale()) {
			moraleText.color = highColor;
			moraleImage.color = highColor;

		} else if (IsLowMorale()) {
			moraleText.color = lowColor;
			moraleImage.color = lowColor;

		} else if (IsNoneMorale()) {
			moraleText.color = noneColor;
			moraleImage.color = noneColor;

		} else {
			moraleText.color = new Color(0.9f, 0.9f, 0.9f);
			moraleImage.color = new Color(0.9f, 0.9f, 0.9f);
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
	public float age;
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

	public string GetNatureName() {
		switch (nature) {
			case NatureType.Careful:
				return "細心";
				break;
			case NatureType.Rash:
				return "粗心";
				break;
			case NatureType.Brave:
				return "勇敢";
				break;
			case NatureType.Timid:
				return "膽小";
				break;
			case NatureType.Optimistic:
				return "樂觀";
				break;
			case NatureType.Pessimistic:
				return "悲觀";
				break;
			case NatureType.Hasty:
				return "性急";
				break;
			case NatureType.Relaxed:
				return "悠閒";
				break;
		}


		return "無";
	}
}

public enum NatureType {
	None,

	Careful,//細心
	Rash,//粗心

	Brave,//勇敢
	Timid,//膽小

	Optimistic,//樂觀
	Pessimistic,//悲觀

	Hasty,//性急
	Relaxed//悠閒
}


public enum EmojiType {
	None,
	Happy,
	Like,
	Angry,
	Sad,
	Doubt,
	Surprise
}

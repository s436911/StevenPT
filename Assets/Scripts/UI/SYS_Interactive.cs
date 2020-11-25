using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Interactive : MonoBehaviour {
	public static SYS_Interactive Direct;
	private InteractEvent nowEvent;
	public GameObject uiPanelAnswer;

	public GameObject panelShop;
	public Transform spawnerShop;
	public GameObject pfbShop;
	public float highShop;
	public List<UI_ButtonCarrier> itemShop = new List<UI_ButtonCarrier>();


	public List<RectTransform> uiTalkers = new List<RectTransform>();

	public SpaceEntity talker;
	public InteractEvent story;
	public float talkerDis = 12;
	public float talkerUIOffset = 120;


	public Text textFrom;
	public Text textMsg;
	public Text textPs;

	public float talkerWidth = 120;

	public RectTransform[] rectBTNs = new RectTransform[3];
	public RawImage[] rawImages = new RawImage[3];
	public RawImage[] rawImageBacks = new RawImage[3];
	public Text[] texts = new Text[3];
	public Text[] textmsgs = new Text[3];

	public Texture2D[] affinityIcon = new Texture2D[4];

	private float intaTimer;

	void Awake() {
		Direct = this;
	}

	public void Regist(InteractEvent value) {
		intaTimer = Time.timeSinceLevelLoad;
		SYS_GameEngine.Direct.SetPause(true);

		nowEvent = value;

		uiPanelAnswer.SetActive(true);
		textFrom.text = nowEvent.from;
		textMsg.text = nowEvent.msg;
		textPs.text = nowEvent.ps;

		if (nowEvent.answers.Count == 1) {
			rectBTNs[0].anchoredPosition = new Vector2(0, 30);

		} else if (nowEvent.answers.Count == 2) {
			rectBTNs[0].anchoredPosition = new Vector2(-125, 30);
			rectBTNs[1].anchoredPosition = new Vector2(125, 30);


		} else if (nowEvent.answers.Count == 3) {
			rectBTNs[0].anchoredPosition = new Vector2(-175, 30);
			rectBTNs[1].anchoredPosition = new Vector2(0, 30);
			rectBTNs[2].anchoredPosition = new Vector2(175, 30);
		}

		for (int id = 0; id < rectBTNs.Length; id++) {
			if (id < nowEvent.answers.Count) {
				rectBTNs[id].gameObject.SetActive(true);

				rawImages[id].texture = affinityIcon[(int)nowEvent.answers[id].affinity];
				rawImageBacks[id].texture = affinityIcon[(int)nowEvent.answers[id].affinity];

				texts[id].text = nowEvent.answers[id].text;

				if (nowEvent.answers[id].costNum != 0 && nowEvent.answers[id].getNum != 0) {
					textmsgs[id].text = (nowEvent.answers[id].successRate >= 100 ? "" : (nowEvent.answers[id].successRate + SYS_Save.Direct.GetMembersAttribute(3)).ToString("f0") + "%機率") + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum + ">" + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].getType) + nowEvent.answers[id].getNum;

				} else if (nowEvent.answers[id].costNum != 0) {
					textmsgs[id].text = (nowEvent.answers[id].successRate >= 100 ? "" : (nowEvent.answers[id].successRate + SYS_Save.Direct.GetMembersAttribute(3)).ToString("f0") + "%機率") + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum;

				} else {
					textmsgs[id].text = "---";
				}

			} else {
				rectBTNs[id].gameObject.SetActive(false);
			}
		}
	}

	public void Reset() {
		Clear();
		RegistTalker();
	}

	public void RegistTalker(SpaceEntity value = null, InteractEvent valueEvent = null) {
		talker = value;
		story = valueEvent;
	}

	public void ClickShop(UI_ButtonCarrier main , UI_ButtonBase sub) {
		Debug.LogWarning(main.id + "/" + sub.id);
		main.gameObject.SetActive(false);
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(main.id , 100));
		SYS_Save.Direct.ModifyCargobay(DB.NewItem(sub.id, -120));

		if (!itemShop[0].gameObject.activeSelf && !itemShop[1].gameObject.activeSelf && !itemShop[2].gameObject.activeSelf) {
			ClearShop();
		}
	}

	public void RegistShop() {
		SYS_GameEngine.Direct.SetPause(true);
		panelShop.SetActive(true);

		for (int x = 0; x < 3; x++) {
			List<int> minus = new List<int>();
			UI_ButtonCarrier objTmp = Instantiate(pfbShop).GetComponent<UI_ButtonCarrier>();
			objTmp.transform.SetParent(spawnerShop);
			objTmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, highShop * -x);
			objTmp.transform.localScale = Vector2.one;
			itemShop.Add(objTmp);

			minus.Add(Random.Range(1, 10));
			Item get = DB.GetItem(minus[minus.Count - 1]);
			objTmp.Init(minus[minus.Count - 1]);

			objTmp.buttons[0].Regist(DB.GetItemTexture(get.iconID), get.text, 100.ToString());

			for (int y = 1; y < 4; y++) {
				minus.Add(Common.Direct.RandomNum(1, 10, minus));
				get = DB.GetItem(minus[minus.Count -1]);
				objTmp.buttons[y].Regist(DB.GetItemTexture(get.iconID), 120.ToString());
				objTmp.buttons[y].Init(minus[minus.Count - 1]);
			}
		}
	}


	public void ClearShop() {
		panelShop.SetActive(false);
		SYS_GameEngine.Direct.SetPause(false);
		itemShop = new List<UI_ButtonCarrier>();
		foreach (Transform log in spawnerShop) {
			Destroy(log.gameObject);
		}
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			SetTalker(talker);
		}
	}

	public void Clear() {
		uiPanelAnswer.SetActive(false);

		foreach (RectTransform rect in rectBTNs) {
			rect.gameObject.SetActive(false);
		}
	}

	public void Answer(int value) {
		SYS_GameEngine.Direct.SetPause(false);
		nowEvent.answers[value].Interact();
		Clear();
	}

	public void SetTalker(SpaceEntity value) {
		if (value != null) {
			Vector2 offset = (value.transform.position - SYS_ShipController.Direct.transform.position);

			//撞到星球
			if (value.info.mainType == MainType.Planet && offset.magnitude <= talkerDis * value.transform.localScale.x) {
				SetTrigger(true, true, true);

			//撞到事件
			} else if (value.info.mainType == MainType.InterAct && offset.magnitude <= talkerDis * 0.5f * value.transform.localScale.x) {
				SetTrigger(true, false, false);

			} else {
				RegistTalker();
			}

		} else {
			SetTalker();
		}
	}

	public void SetTrigger(bool talk, bool fuel, bool shop) {
		List<RectTransform> rects = new List<RectTransform>();
		sbyte num = 0;
		if (talk) {
			rects.Add(uiTalkers[0]);
			num++;
		}

		if (fuel) {
			rects.Add(uiTalkers[1]);
			num++;
		}

		if (shop) {
			rects.Add(uiTalkers[2]);
			num++;
		}

		uiTalkers[0].gameObject.SetActive(talk);
		uiTalkers[1].gameObject.SetActive(fuel);
		uiTalkers[2].gameObject.SetActive(shop);

		for (int i = 0; i < rects.Count; i++) {
			rects[i].anchoredPosition = new Vector2(-(rects.Count - 1) * 0.5f * talkerWidth + talkerWidth * i, 0);
		}

	}

	public void SetTalker() {
		foreach (RectTransform button in uiTalkers) {
			button.gameObject.SetActive(false);
		}
	}

	public void TalkQuest() {
		if (Time.timeSinceLevelLoad - intaTimer < 3) {
			return;
		}
		Regist(story);
		SetTalker();
	}
}


public class InteractEvent {
	public string from;
	public string msg;
	public string ps;
	public List<InteractOption> answers;

	public InteractEvent(string from, string msg, List<InteractOption> answers, string ps = null) {
		this.from = from;
		this.msg = msg;
		this.answers = answers;
		this.ps = ps;
	}
}


public class InteractOption {
	public Affinity affinity;
	public string text;
	public float successRate;

	public int costType;
	public int costNum;

	public int getType;
	public int getNum;

	public int IID;
	public int SID;

	public Item getItem;

	public InteractOption(Affinity affinity, float successRate, int costType, int costNum, int getType, int getNum, string text, Item getItem = null) {
		if (getItem == null) {
			getItem = new Item();
		}

		this.affinity = affinity;
		this.successRate = successRate;
		this.costType = costType;
		this.costNum = costNum;
		this.getType = getType;
		this.getNum = getNum;
		this.getItem = getItem;
		this.text = text;
	}

	public bool InteractAble() {
		return SYS_ResourseManager.Direct.resources[costType] >= costNum;
	}

	public void Interact() {
		if (InteractAble()) {

			SYS_ResourseManager.Direct.ModifyResource(costType, -costNum);

			if (Random.Range(0, 100) < successRate + SYS_Save.Direct.GetMembersAttribute(3)) {
				if (getItem.isNull) {
					SYS_ResourseManager.Direct.ModifyResource(getType, getNum);

				} else {
					SYS_ResourseManager.Direct.AddCargo(getItem);
				}
			}
			
			SYS_TeamManager.Direct.TriggerEvent(costNum > 0 ? 1 : 6);
		}
	}
}

public enum Affinity {
	None,
	Fight,
	Trade,
	Explore
}

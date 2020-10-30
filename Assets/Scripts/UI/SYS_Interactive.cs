using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Interactive : MonoBehaviour {
	public static SYS_Interactive Direct;
	private InteractEvent nowEvent;
	public GameObject uiPanelAnswer;
	public RectTransform uiPanelTalker;
	public SpaceEntity talker;
	public InteractEvent story;
	public float talkerDis = 12;
	public float talkerUIOffset = 120;


	public Text textFrom;
	public Text textMsg;
	public Text textPs;

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
		SYS_SelfDriving.Direct.Reset();
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
					textmsgs[id].text = (nowEvent.answers[id].successRate >= 100 ? "" : (nowEvent.answers[id].successRate + SYS_Save.Direct.GetMembersAttribute(2)).ToString("f0") + "%機率") + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum + ">" + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].getType) + nowEvent.answers[id].getNum;

				} else if (nowEvent.answers[id].costNum != 0) {
					textmsgs[id].text = (nowEvent.answers[id].successRate >= 100 ? "" : (nowEvent.answers[id].successRate + SYS_Save.Direct.GetMembersAttribute(2)).ToString("f0") + "%機率") + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum;

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

			if (value.info.mainType == MainType.Planet && offset.magnitude <= talkerDis * value.transform.localScale.x) {
				uiPanelTalker.gameObject.SetActive(true);
				uiPanelTalker.anchoredPosition = -offset.normalized * talkerUIOffset;

			} else if (value.info.mainType == MainType.InterAct && offset.magnitude <= talkerDis * 0.5f * value.transform.localScale.x) {
				uiPanelTalker.gameObject.SetActive(true);
				uiPanelTalker.anchoredPosition = -offset.normalized * talkerUIOffset;

			} else {
				RegistTalker();
			}

		} else {
			uiPanelTalker.gameObject.SetActive(false);
		}
	}

	public void TalkQuest() {
		if (Time.timeSinceLevelLoad - intaTimer < 3) {
			return;
		}
		Regist(story);
		uiPanelTalker.gameObject.SetActive(false);
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

			if (Random.Range(0, 100) < successRate + SYS_Save.Direct.GetMembersAttribute(2)) {
				if (getItem.isNull) {
					SYS_ResourseManager.Direct.ModifyResource(getType, getNum);

				} else {
					SYS_ResourseManager.Direct.AddCargo(getItem);
				}
			}
		}
	}
}

public enum Affinity {
	None,
	Fight,
	Trade,
	Explore
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Interactive : MonoBehaviour
{
	public static SYS_Interactive Direct;
	private InteractEvent nowEvent;
	public GameObject uiPanel;

	public Text textFrom;
	public Text textMsg;
	public Text textPs;

	public RectTransform[] rectBTNs = new RectTransform[3];
	public RawImage[] rawImages = new RawImage[3];
	public RawImage[] rawImageBacks = new RawImage[3];
	public Text[] texts = new Text[3];
	public Text[] textmsgs = new Text[3];

	public Texture2D[] affinityIcon = new Texture2D[4];

	void Awake() {
		Direct = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Regist(InteractEvent value) {
		nowEvent = value;

		uiPanel.SetActive(true);
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

				if (nowEvent.answers[id].costNum != 0 || nowEvent.answers[id].getNum != 0) {
					textmsgs[id].text = (nowEvent.answers[id].successRate >= 100 ? "" : nowEvent.answers[id].successRate.ToString("f0") + "%") + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum + ">" + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].getType) + nowEvent.answers[id].getNum;

				} else {
					textmsgs[id].text = "---";
				}

			} else {
				rectBTNs[id].gameObject.SetActive(false);
			}
		}
	}

	public void Clear() {
		uiPanel.SetActive(false);

		foreach (RectTransform rect in rectBTNs) {
			rect.gameObject.SetActive(false);
		}
	}

	public void Answer(int value) {
		nowEvent.answers[value].Interact();
		Clear();
	}
}


public class InteractEvent {
	public string from;
	public string msg;
	public string ps;
	public List<InteractOption> answers;
	
	public InteractEvent(string from, string msg , List<InteractOption> answers , string ps = null) {
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

	public InteractOption(Affinity affinity , float successRate, int costType , int costNum  , int getType , int getNum, string text = null) {
		this.affinity = affinity;
		this.successRate = successRate;
		this.costType = costType;
		this.costNum = costNum;
		this.getType = getType;
		this.getNum = getNum;
		this.text = text;
	}

	public bool InteractAble() {
		return SYS_ResourseManager.Direct.resources[costType] >= costNum;
	}
	
	public void Interact() {
		if (InteractAble()) {
			SYS_ResourseManager.Direct.ModifyResource(costType, -costNum);

			if (Random.Range(0, 100) < successRate) {
				SYS_ResourseManager.Direct.ModifyResource(getType, getNum);
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

public static class TMP_InteractEvent {
	public static InteractEvent GetPlanetEvent(string sName , int planetType = 0) {
		List<InteractOption> tempAnswers = new List<InteractOption>();
		string tmpMsg = "";
		int rand = planetType == 0 ? Random.Range(1, 7) : planetType;

		switch (rand) {
			case 1:
				tempAnswers.Add(new InteractOption(Affinity.Trade , 100, 3, 1, 0, 40 , "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這個星球有一間很大的加油站可以用呢!!";
				break;

			case 2:
				tempAnswers.Add(new InteractOption(Affinity.None, 75, 2, 1, 0, 40, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 1, 1, 0, 60, "交戰"));
				tmpMsg = "加油站被野生動物佔據了我們只能偷偷加油，但若擊退他們我們就可以加免費燃料了!!";
				break;

			case 3:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 40, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Explore, 60, 2, 1, 0, 60, "探索"));
				tmpMsg = "加油站旁有座巨大礦坑也許可以找到一點燃料!?";
				break;

			case 4:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 40, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 3, 1, 1, 1, "強化"));
				tmpMsg = "這裡的星球武器商可以補給燃料和強化裝甲!!";
				break;

			case 5:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 2, 1, 0, 40, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這顆星球的加油站竟然只收食物!!";
				break;

			case 6:
				tempAnswers.Add(new InteractOption(Affinity.Explore, 100, 2, 1, 0, 40, "探索"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這個加油站已經廢棄很久了，但是也許可以從廢墟中找到一些燃料!!";
				break;
		}

		return new InteractEvent(sName, tmpMsg, tempAnswers);
	}
}

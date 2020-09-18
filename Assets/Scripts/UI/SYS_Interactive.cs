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

	public Texture2D[] affinityIcon = new Texture2D[4];
	public InteractEvent templateEvent;

	void Awake() {
		Direct = this;

		List<InteractOption> tempAnswers = new List<InteractOption>();
		tempAnswers.Add(new InteractOption(Affinity.Trade, 3, 1, 0, 40));
		tempAnswers.Add(new InteractOption(Affinity.None, 0, 0, 0, 0, "離開"));
		

		if (Random.Range(0, 2) == 0) {
			tempAnswers.Add(new InteractOption(Affinity.Fight, 3, 1, 1, 1));
		} else {

			tempAnswers.Add(new InteractOption(Affinity.Explore, 2, 1, 0, 40));
		}
		
		templateEvent = new InteractEvent("Planet" , "Resupply...", tempAnswers);
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
			rectBTNs[0].anchoredPosition = new Vector2(0, -2);

		} else if (nowEvent.answers.Count == 2) {
			rectBTNs[0].anchoredPosition = new Vector2(-125, -2);
			rectBTNs[1].anchoredPosition = new Vector2(125, -2);


		} else if (nowEvent.answers.Count == 3) {
			rectBTNs[0].anchoredPosition = new Vector2(-175, -2);
			rectBTNs[1].anchoredPosition = new Vector2(0, -2);
			rectBTNs[2].anchoredPosition = new Vector2(175, -2);
		}

		for (int id = 0; id < rectBTNs.Length; id++) {
			if (id < nowEvent.answers.Count) {
				rectBTNs[id].gameObject.SetActive(true);

				rawImages[id].texture = affinityIcon[(int)nowEvent.answers[id].affinity];
				rawImageBacks[id].texture = affinityIcon[(int)nowEvent.answers[id].affinity];

				if (nowEvent.answers[id].text == null) {
					texts[id].text = SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].costType) + nowEvent.answers[id].costNum + ">" + SYS_ResourseManager.Direct.ToString(nowEvent.answers[id].getType) + nowEvent.answers[id].getNum;

				} else {
					texts[id].text = nowEvent.answers[id].text;
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

	public int costType;
	public int costNum;

	public int getType;
	public int getNum;

	public int IID;
	public int SID;

	public InteractOption(Affinity affinity, int costType , int costNum  , int getType , int getNum, string text = null) {
		this.affinity = affinity;
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
		SYS_ResourseManager.Direct.ModifyResource(costType, -costNum);
		SYS_ResourseManager.Direct.ModifyResource(getType, getNum);
	}
}


public enum Affinity {
	None,
	Fight,
	Trade,
	Explore
}
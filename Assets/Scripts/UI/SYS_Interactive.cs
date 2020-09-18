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
		tempAnswers.Add(new InteractOption((Affinity)Random.Range(0, 4), "40 Fuel"));
		tempAnswers.Add(new InteractOption((Affinity)Random.Range(0, 4), "40 Fuel"));
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
		uiPanel.SetActive(true);
		textFrom.text = value.from;
		textMsg.text = value.msg;
		textPs.text = value.ps;

		if (value.answers.Count == 1) {
			rectBTNs[0].anchoredPosition = new Vector2(0, -2);

		} else if (value.answers.Count == 2) {
			rectBTNs[0].anchoredPosition = new Vector2(125, -2);
			rectBTNs[1].anchoredPosition = new Vector2(-125, -2);


		} else if (value.answers.Count == 3) {
			rectBTNs[0].anchoredPosition = new Vector2(175, -2);
			rectBTNs[1].anchoredPosition = new Vector2(0, -2);
			rectBTNs[2].anchoredPosition = new Vector2(-175, -2);
		}

		for (int id = 0; id < rectBTNs.Length; id++) {
			if (id < value.answers.Count) {
				rectBTNs[id].gameObject.SetActive(true);

				rawImages[id].texture = affinityIcon[(int)value.answers[id].affinity];
				rawImageBacks[id].texture = affinityIcon[(int)value.answers[id].affinity];
				texts[id].text = value.answers[id].text;

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
		SYS_ResourseManager.Direct.ModifyResource(3,-1);
		SYS_ResourseManager.Direct.ModifyResource(0,40);
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

	public InteractOption(Affinity affinity, string text) {
		this.affinity = affinity;
		this.text = text;
	}

	public bool UseAble() {
		return true;
	}

	public string ToString() {
		return "40 Fuel";
	}
}


public enum Affinity {
	None,
	Fight,
	Trade,
	Explore
}
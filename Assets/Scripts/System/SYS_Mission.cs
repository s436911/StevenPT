using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Mission : MonoBehaviour {
	public static SYS_Mission Direct;
	public MissionSet nowMission;

	public List<UI_ButtonBase> areas = new List<UI_ButtonBase>();
	public List<UI_ButtonBase> searchs = new List<UI_ButtonBase>();
	public List<UI_ButtonBase> missions = new List<UI_ButtonBase>();
	public List<GameObject> missionUI;

	public RawImage msIcon;
	public Text msText;
	public float msNow;
	public float msTGT;

	public List<Texture2D> missionIcon = new List<Texture2D>();
	public Texture2D tripIcon;

	public Image scopeIcon;
	public Text misTitle;
	public Text scopeCostText;
	public Text scopeLeftText;
	public GameObject msCountDown;
	public Text msCountDownText;
	public Rigidbody2D msStarEater;
	public Texture msStarEaterTexture;

	public float[] scopeCosts = { 0, 1, 2, 4 };
	public Color[] scopeColors = new Color[4];
	public int scopeLeftMax;


	private int nowAreaId = 0; 
	private int scopeLv;
	private int scopeLeft;
	private float msTimeLeft;

	private AreaSet[] areaSets = new AreaSet[3];
	private MissionSet[] searchedSets = new MissionSet[5];
	private List<int> resrcSet = new List<int>();

	private MissionType missionType;
	private int difficult = 0;

	void Awake() {
		Direct = this;
		scopeLeft = scopeLeftMax;
		msStarEater.GetComponent<MeteorEntity>().info = new StarInfo(MainType.Drift, SubType.Meteor, NaviType.None, Affinity.None, Vector2.zero);
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (msTimeLeft > 0) {
				SetCountDown(msTimeLeft - Time.deltaTime);
				msStarEater.velocity = (SYS_ShipController.Direct.transform.position - msStarEater.transform.position).normalized * 2.5f;
				msStarEater.transform.localScale = (1 + Mathf.Clamp01((300 - msTimeLeft) / 300) * 11) * Vector3.one;
			}
		}
	}

	public void Init() {
		UpdateScopeUI();

		areaSets[0] = new AreaSet("新手星系", 4);
		areaSets[1] = new AreaSet("花椰菜星系", 5);
		areaSets[2] = new AreaSet("磁鐵星系", 4);

		resrcSet.Add(0);

		resrcSet.Add(1);
		resrcSet.Add(2);
		resrcSet.Add(3);
		resrcSet.Add(4);
		resrcSet.Add(5);
		resrcSet.Add(6);
		resrcSet.Add(7);
		resrcSet.Add(8);
		resrcSet.Add(9);

		resrcSet.Add(1001);
		resrcSet.Add(1002);
	}

	public void Restart() {
		if (nowMission.missionType == MissionType.Trip) {
			RegistMSbar(tripIcon, new Color(0.9f, 0.7f, 0.15f), SYS_Starmap.Direct.route.Count);

		} else if (nowMission.missionType == MissionType.Collect) {
			RegistMSbar(DB.GetItemTexture(nowMission.mainResrc), Color.white, 10);

		} else if (nowMission.missionType == MissionType.Escape) {
			RegistMSbar(tripIcon, new Color(0.9f, 0.7f, 0.15f), SYS_Starmap.Direct.route.Count);
			SetCountDown(300);
			msStarEater.transform.position = new Vector2(0,-20);
			msStarEater.gameObject.SetActive(true);
			UI_Navigator.Direct.Regist(msStarEater.gameObject.GetComponent<MeteorEntity>() , NaviMode.Alert, msStarEaterTexture);
		}
	}
	public void ResetCountDown() {
		msTimeLeft = 0;
		msCountDown.SetActive(false);
	}

	public void SetCountDown(float ct) {
		if (ct > 0) {
			msTimeLeft = ct;
			UpdateCountDownUI();
			msCountDown.SetActive(true);
		} else {
			UI_ScoreManager.Direct.Lose();
		}
	}

	public void UpdateCountDownUI (){
		System.DateTime shower = new System.DateTime();
		shower = shower.AddSeconds((int)msTimeLeft);
		msCountDownText.text = shower.Minute.ToString() + ":" + shower.Second.ToString();
		if (msTimeLeft > 120) {
			msCountDownText.color = new Color(0.9f, 0.9f, 0.9f);
		} else if (msTimeLeft > 30) {
			msCountDownText.color = new Color(0.9f, 0.7f, 0.15f);
		} else {
			msCountDownText.color = new Color(0.9f, 0.25f, 0.15f);
		}
	}

	public string GetMissionString(MissionType type) {
		if (type == MissionType.Trip) {
			return "探索";
		} else if (type == MissionType.Collect) {
			return "採集";
		} else if (type == MissionType.Convoy) {
			return "護送";
		} else if (type == MissionType.Escape) {
			return "脫逃";
		} else if (type == MissionType.Trade) {
			return "貿易";
		} else if (type == MissionType.Destroy) {
			return "戰鬥";
		} else if (type == MissionType.Treasure) {
			return "巡寶";
		}
		return "n/a";
	}

	public void SetDifficult(InputField value) {
		int tmpDifficult = int.Parse(value.text);

		if (tmpDifficult > 5 + SYS_Save.Direct.GetResearch(2)) {
			tmpDifficult = 5 + SYS_Save.Direct.GetResearch(2);
			value.text = tmpDifficult.ToString();

		} else if (tmpDifficult < 0) {
			tmpDifficult = 0;
			value.text = tmpDifficult.ToString();
		}

		difficult = tmpDifficult;
	}

	public void UseArea(int slotID) {
		nowAreaId = slotID;
		SelectUI(slotID, areas);
		DeSelectUI(searchs);

		for (int ct = 0; ct < searchs.Count; ct++) {
			if (SYS_Save.Direct.gameData.researchs[2] > ct) {
				searchs[ct].gameObject.SetActive(true);
				searchs[ct].Regist((ct * 2).ToString() + "~" + (ct * 2 + 4).ToString(), (ct / 2 + 1).ToString());

			} else {
				searchs[ct].gameObject.SetActive(false);
			}
		}
	}

	public void UseSearch(int slotID) {
		SelectUI(slotID, searchs);
		DeSelectUI(missions);

		for (int ct = 0; ct < searchedSets.Length; ct++) {
			int rand = Random.Range(0, 3);
			int difficult = slotID * 2 + Random.Range(0, 5);
			int mainRsrc = resrcSet[Random.Range(1, resrcSet.Count)];
			int subRsrc = resrcSet[Random.Range(0, resrcSet.Count)];

			if (rand == 0) {
				searchedSets[ct] = new MissionSet(MissionType.Trip, areaSets[nowAreaId].mainStarNum, difficult, 1, mainRsrc, subRsrc);

			} else if (rand == 1) {
				searchedSets[ct] = new MissionSet(MissionType.Collect, areaSets[nowAreaId].mainStarNum - 1, difficult, 1.2f, mainRsrc, subRsrc);

			} else if (rand == 2) {
				searchedSets[ct] = new MissionSet(MissionType.Escape, areaSets[nowAreaId].mainStarNum + 1, difficult, 1, mainRsrc, subRsrc);
			}

			missions[ct].gameObject.SetActive(true);
			missions[ct].Regist(DB.GetItemTexture(searchedSets[ct].mainResrc), GetMissionString(searchedSets[ct].missionType) + " lv " + searchedSets[ct].difficult.ToString());
			missions[ct].DeClick();
		}

		SYS_Save.Direct.ModifyResource(0, -(int)((slotID / 2 + 1) * scopeCosts[scopeLv]));

		if (scopeLv != scopeCosts.Length - 1) {
			scopeLeft = Mathf.Clamp(scopeLeft - 1, 0, scopeLeftMax);

			if (scopeLeft == 0) {
				scopeLv++;
				scopeLeft = scopeLeftMax;
			}

			UpdateScopeUI();
		}
	}

	public void SetScope(int value) {
		scopeLv = value;
		scopeLeft = scopeLeftMax;
		UpdateScopeUI();
	}

	public void SetMission(int slotID) {
		nowMission = searchedSets[slotID];
		misTitle.text = GetMissionString(nowMission.missionType) + " lv " + nowMission.difficult.ToString();
		SYS_Starmap.Direct.GenStarmap(nowMission);

		SelectUI(slotID, missions);

		for (int ct = 0; ct < missionUI.Count; ct++) {
			missionUI[ct].SetActive(true);
		}
	}

	public void UpdateScopeUI() {
		if (scopeLv != 0) {
			scopeCostText.text = (scopeCosts[scopeLv] * 100).ToString() + "%";
		} else {
			scopeCostText.text = "free";
		}

		scopeCostText.color = scopeColors[scopeLv];
		scopeLeftText.color = scopeColors[scopeLv];
		scopeIcon.color = scopeColors[scopeLv];

		if (scopeLv == scopeCosts.Length - 1) {
			scopeLeftText.text = "-";
		} else {
			scopeLeftText.text = scopeLeft.ToString();
		}
	}

	public void Reset() {
		//missionPanel
		misTitle.text = "------";
		for (int ct = 0; ct < areas.Count; ct++) {
			areas[ct].Regist(areaSets[ct].areaName);
		}
		for (int ct = 0; ct < searchs.Count; ct++) {
			searchs[ct].gameObject.SetActive(false);
		}
		for (int ct = 0; ct < missions.Count; ct++) {
			missions[ct].gameObject.SetActive(false);
		}
		for (int ct = 0; ct < missionUI.Count; ct++) {
			missionUI[ct].gameObject.SetActive(false);
		}for (int ct = 0; ct < missionUI.Count; ct++) {
			missionUI[ct].gameObject.SetActive(false);
		}
		DeSelectUI(areas);
		SetScope(Mathf.Clamp(scopeLv -1, 0, 100));
		ResetCountDown();
		RegistMSbar(tripIcon , new Color(0.9f, 0.7f, 0.15f));
		msStarEater.gameObject.SetActive(false);
	}

	public void RegistMSbar(Texture2D texture , Color color, float value = 1) {
		msNow = 0;
		msTGT = value;
		msIcon.texture = texture;
		msIcon.color = color;
		UpdateMSbarUI();
	}

	public bool IsComplete() {
		return msNow == msTGT;
	}

	public void ModifyMSbar(float value) {
		SetMSbar(msNow + value);
	}

	public void SetMSbar(float value) {
		msNow = Mathf.Clamp(value, 0, msTGT);
		UpdateMSbarUI();
		if (msNow == msTGT) {
			UI_ScoreManager.Direct.Victory();
		}
	}

	public void UpdateMSbarUI() {
		float percent = Mathf.Clamp01( msNow / msTGT);		

		if (percent != 1) {
			msText.text = msNow + "/" + msTGT + " (" +(percent * 100).ToString("0") + "%)";
		} else {
			msText.text = "complete";
		}
	}

	public void DeSelectUI(List<UI_ButtonBase> list) {
		for (int ct = 0; ct < list.Count; ct++) {
			list[ct].DeClick();
		}
	}

	public void SelectUI(int slotID , List<UI_ButtonBase> list) {
		for (int ct = 0; ct < list.Count; ct++) {
			if (ct == slotID) {
				list[ct].Click();
			} else {
				list[ct].DeClick();
			}
		}
	}
}

public class AreaSet {
	public string areaName;
	public int mainStarNum = 4;

	public AreaSet(string areaName , int mainStarNum) {
		this.areaName = areaName;
		this.mainStarNum = mainStarNum;
	}
}

public class MissionSet {
	public MissionType missionType;
	public int mainStarNum = 4;
	public int difficult = 0;
	public float iActRate = 1;

	public int mainResrc;
	public int subResrc;

	public MissionSet(MissionType missionType , int mainStarNum, int difficult , float iActRate, int mainResrc, int subResrc ) {
		this.mainStarNum = mainStarNum;
		this.missionType = missionType;
		this.difficult = difficult;
		this.mainResrc = mainResrc;
		this.subResrc = subResrc;
		this.iActRate = iActRate;
	}
}

public enum MissionType {
	Trip,       //	探索任務（家裡出發抵達某顆資源星最後有該獎勵特效或玩法的反饋
	Collect,    //	採集任務（太空出發收集定量資源並送回家的任務
	Convoy,     //	護送任務（太空出發護送人到某顆星球協助排除中間的危險物
	Escape,     //	脫逃任務（太空在計時歸零前(或是敵對飛船擊破玩家)回家的任務、支線報酬豐富
	Trade,      //	貿易任務（家裡出發在星圖中收集足夠資源完成交易可開啟特殊交易選單
	Destroy,    //	戰鬥任務（家裡出發去破壞目標船隻、星球或太空站等
	Treasure,	//	巡寶任務（家裡出發延著情報或蹤跡找到目標
}
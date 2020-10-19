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

	public List<Image> msBars = new List<Image>();
	public RectTransform msPos;
	public RawImage msIcon;
	public Text msText;
	public Text msCompleteText;
	public float msNow;
	public float msTGT;

	public List<Texture2D> missionIcon = new List<Texture2D>();
	public Texture2D tripIcon;
	public List<Texture2D> resourceIcon = new List<Texture2D>();

	public Image scopeIcon;
	public Text misTitle;
	public Text scopeCostText;
	public Text scopeLeftText;

	public float[] scopeCosts = { 0, 1, 2, 4 };
	public Color[] scopeColors = new Color[4];
	public int scopeLeftMax;

	private int nowAreaId = 0; 
	private int scopeLv;
	private int scopeLeft;
	
	private AreaSet[] areaSets = new AreaSet[3];
	private MissionSet[] searchedSets = new MissionSet[5];
	private List<ResourcesSet> resrcSet = new List<ResourcesSet>();

	private MissionType missionType;
	private int difficult = 0;

	void Awake() {
		Direct = this;
		scopeLeft = scopeLeftMax;
	}

	public void Init() {
		UpdateScopeUI();

		areaSets[0] = new AreaSet("新手星系", 4);
		areaSets[1] = new AreaSet("花椰菜星系", 5);
		areaSets[2] = new AreaSet("磁鐵星系", 4);

		resrcSet.Add(null);
		resrcSet.Add(new ResourcesSet(missionIcon[0], SYS_SpaceManager.Direct.matResoureces[0], 0, 3));
		resrcSet.Add(new ResourcesSet(missionIcon[0], SYS_SpaceManager.Direct.matResoureces[1], 1, 3));
		resrcSet.Add(new ResourcesSet(missionIcon[0], SYS_SpaceManager.Direct.matResoureces[2], 2, 3));

		resrcSet.Add(new ResourcesSet(missionIcon[1], SYS_SpaceManager.Direct.matResoureces[5], 5, 2));
		resrcSet.Add(new ResourcesSet(missionIcon[1], SYS_SpaceManager.Direct.matResoureces[6], 6, 2));
	}

	public void Restart() {
		if (nowMission.missionType == MissionType.Trip) {
			RegistMSbar(tripIcon, new Color(0.9f, 0.7f, 0.15f), SYS_StarmapManager.Direct.route.Count);

		} else if (nowMission.missionType == MissionType.Collect) {
			RegistMSbar(resourceIcon[nowMission.mainResrc.resourceId], Color.white, 10);
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

		if (tmpDifficult > 5 + SYS_SaveManager.Direct.GetResearch(2)) {
			tmpDifficult = 5 + SYS_SaveManager.Direct.GetResearch(2);
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
			if (SYS_SaveManager.Direct.gameData.researchs[2] > ct) {
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
			if (Random.Range(0, 2) == 0) {
				searchedSets[ct] = new MissionSet(MissionType.Trip, areaSets[nowAreaId].mainStarNum, slotID * 2 + Random.Range(0, 5), resrcSet[Random.Range(1, resrcSet.Count)], resrcSet[Random.Range(0, resrcSet.Count)]);

			} else {
				searchedSets[ct] = new MissionSet(MissionType.Collect, areaSets[nowAreaId].mainStarNum - 1, slotID * 2 + Random.Range(0, 5), resrcSet[Random.Range(1, resrcSet.Count)], resrcSet[Random.Range(0, resrcSet.Count)]);
			}

			missions[ct].gameObject.SetActive(true);
			missions[ct].Regist(searchedSets[ct].mainResrc.icon, GetMissionString(searchedSets[ct].missionType) + " lv " + searchedSets[ct].difficult.ToString());
			missions[ct].DeClick();
		}

		SYS_SaveManager.Direct.ModifyResource(0, -(int)((slotID / 2 + 1) * scopeCosts[scopeLv]));

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
		SYS_StarmapManager.Direct.GenStarmap(nowMission);

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

		RegistMSbar(tripIcon , new Color(0.9f, 0.7f, 0.15f));
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

		for (int ct = 0; ct < msBars.Count; ct++) {
			if ((ct + 1) * 0.125f <= percent) {
				msBars[ct].color = new Color(0.9f, 0.7f, 0.15f);
			} else {
				msBars[ct].color = new Color(0.8f, 0.8f, 0.8f);
			}
		}

		msPos.anchoredPosition = new Vector2(720 * percent, msPos.anchoredPosition.y);
		msText.text = msNow + "/" + msTGT;

		if (percent != 1) {
			msCompleteText.text = percent * 100 + "%";
		} else {
			msCompleteText.text = "complete";
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

	public ResourcesSet mainResrc;
	public ResourcesSet subResrc;

	public MissionSet(MissionType missionType , int mainStarNum, int difficult , ResourcesSet mainResrc, ResourcesSet subResrc ) {
		this.mainStarNum = mainStarNum;
		this.missionType = missionType;
		this.difficult = difficult;
		this.mainResrc = mainResrc;
		this.subResrc = subResrc;
	}
}

public class ResourcesSet {
	public Texture2D icon;
	public Material mat;
	public int resourceId;
	public int resourceType;

	public ResourcesSet(Texture2D icon , Material mat, int resourceId , int resourceType) {
		this.icon = icon;
		this.mat = mat;
		this.resourceId = resourceId;
		this.resourceType = resourceType;
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
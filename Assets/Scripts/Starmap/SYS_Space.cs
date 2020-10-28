using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Space : MonoBehaviour {
	public static SYS_Space Direct;
	public List<PlanetEntity> planets = new List<PlanetEntity>();
	public List<Material> matPlanets = new List<Material> ();
	public List<Material> matActivities = new List<Material>();
	
	public PlanetEntity tgtPlanet;
	public GameObject pfbPlanet;
	public GameObject pfbActivity;
	public GameObject pfbResourece;
	public GameObject pfbResoureces;
	public Transform entityGroup;

	public int resrcInNum = 4;
	public int resrcOutNum = 8;
	public int resrcNum = 8;

	public int resrcInMaxT = 12;
	public int resrcInMinT = 4;
	public int resrcOutMaxT = 24;
	public int resrcOutMinT = 12;
	public int resrcMaxT = 24;
	public int resrcMinT = 6;

	public int activityNum = 10;
	public int activityMax = 24;
	public int activityMin = 6;

	void Awake() {
		Direct = this;
	}
	
	public void Init() {
		InitPlanet(SYS_Starmap.Direct.stars);
		InitActivity();
		InitResoureces();
	}

	public void SplitResourece(Vector2 pos, Material mat , int resrcId) {
		StarInfo init = new StarInfo(SubType.Resoreces , NaviType.None , Affinity.None , pos);

		ResourecesEntity objGen = Instantiate(pfbResourece).GetComponent<ResourecesEntity>();
		objGen.transform.SetParent(entityGroup);
		objGen.transform.position = new Vector3(init.sPos.x, init.sPos.y, 0);
		objGen.name = init.name;

		objGen.Regist(init, mat, Random.Range(0.9f, 1.1f), resrcId);
	}

	public void InitResoureces() {
		//Side Resoureces
		List<StarInfo> initList = new List<StarInfo>();
		int resrcId;

		//Random內圈 Resoureces
		for (int ct = 0; ct < planets.Count * resrcInNum; ct++) {
			initList.Add(new StarInfo(SubType.Resoreces, NaviType.None, Affinity.None, SYS_Starmap.Direct.stars[Random.Range(0, SYS_Starmap.Direct.stars.Count)].sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(resrcInMinT, resrcInMaxT)));
		}

		foreach (StarInfo starInfo in initList) {
			ResourecesEntity objGen = Instantiate(pfbResoureces).GetComponent<ResourecesEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.name;

			int stack = Random.Range(3, 5);
			resrcId = (SYS_Mission.Direct.nowMission.subResrc != 0 && Random.Range(0, 4) == 0) ? SYS_Mission.Direct.nowMission.subResrc : SYS_Mission.Direct.nowMission.mainResrc;
			objGen.Regist(starInfo, DB.GetItemMaterial(resrcId), 0.6f + stack * 0.15f, resrcId, stack);

			if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect && resrcId == SYS_Mission.Direct.nowMission.mainResrc) {
				UI_Navigator.Direct.Regist(objGen , NaviMode.Target , DB.GetItemTexture(resrcId));
			}
		}

		initList = new List<StarInfo>();
		//Random外圈 Resoureces
		for (int ct = 0; ct < planets.Count * resrcOutNum; ct++) {
			initList.Add(new StarInfo(SubType.Resoreces, NaviType.None, Affinity.None, SYS_Starmap.Direct.stars[Random.Range(0, SYS_Starmap.Direct.stars.Count)].sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(resrcOutMinT, resrcOutMaxT)));
		}

		foreach (StarInfo starInfo in initList) {
			ResourecesEntity objGen = Instantiate(pfbResoureces).GetComponent<ResourecesEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.name;

			int stack = Random.Range(3, 5);
			resrcId = (SYS_Mission.Direct.nowMission.subResrc != 0 && Random.Range(0, 4) == 0) ? SYS_Mission.Direct.nowMission.subResrc : SYS_Mission.Direct.nowMission.mainResrc;
			objGen.Regist(starInfo, DB.GetItemMaterial(resrcId), 0.6f + stack * 0.15f, resrcId, stack);
		}

		initList = new List<StarInfo>();

		//Random Resourece
		for (int ct = 0; ct < planets.Count * resrcNum; ct++) {
			initList.Add(new StarInfo(SubType.Resoreces, NaviType.None, Affinity.None, SYS_Starmap.Direct.stars[Random.Range(0, SYS_Starmap.Direct.stars.Count)].sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(resrcMinT, resrcMaxT)));
		}

		foreach (StarInfo starInfo in initList) {
			ResourecesEntity objGen = Instantiate(pfbResourece).GetComponent<ResourecesEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.name;

			resrcId = (SYS_Mission.Direct.nowMission.subResrc != 0 && Random.Range(0, 3) == 0) ? SYS_Mission.Direct.nowMission.subResrc : SYS_Mission.Direct.nowMission.mainResrc;
			objGen.Regist(starInfo, DB.GetItemMaterial(resrcId), Random.Range(0.9f, 1.1f), resrcId);
		}
		
		//Random goal
		if (SYS_Mission.Direct.nowMission.missionType == MissionType.Trip) {
			initList = new List<StarInfo>();

			for (int ct = 0; ct < Random.Range(8,13); ct++) {
				initList.Add(new StarInfo(SubType.Resoreces, NaviType.None, Affinity.None, SYS_Starmap.Direct.GetInfoTGT().sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(1.5f, 2.5f)));
			}

			foreach (StarInfo starInfo in initList) {
				ResourecesEntity objGen = Instantiate(pfbResourece).GetComponent<ResourecesEntity>();
				objGen.transform.SetParent(entityGroup);
				objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
				objGen.name = starInfo.name;				
				objGen.Regist(starInfo, DB.GetItemMaterial(SYS_Mission.Direct.nowMission.mainResrc), Random.Range(0.9f, 1.1f), SYS_Mission.Direct.nowMission.mainResrc);
			}
		}
	}

	public void InitActivity() {
		List<StarInfo> initList = new List<StarInfo>();
		for (int ct = 0; ct < activityNum * SYS_Mission.Direct.nowMission.iActRate; ct++) {
			initList.Add(new StarInfo(SubType.None, NaviType.Activity, Affinity.None, SYS_Starmap.Direct.stars[Random.Range(0, SYS_Starmap.Direct.stars.Count)].sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(activityMin, activityMax)));
		}

		foreach (StarInfo star in SYS_Starmap.Direct.stars) {
			for (int ct = 0; ct < star.iactNum; ct++) {
				initList.Add(new StarInfo(SubType.None, NaviType.Activity, Affinity.None, star.sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(activityMin, activityMax)));
			}
		}

		foreach (StarInfo starInfo in initList) {
			ActivityEntity objGen = Instantiate(pfbActivity).GetComponent<ActivityEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.name;

			int eventId = Random.Range(0, matActivities.Count);
			objGen.Regist(starInfo, matActivities[eventId], Random.Range(0.8f, 1.2f)  , eventId + 1);

			UI_Navigator.Direct.Regist(objGen);
		}

		//Random goal
		if (SYS_Mission.Direct.nowMission.missionType == MissionType.Escape) {
			initList = new List<StarInfo>();

			for (int ct = 0; ct < 2; ct++) {
				initList.Add(new StarInfo(SubType.None, NaviType.Activity, Affinity.None, SYS_Starmap.Direct.stars[Random.Range(0, 2)].sPos + Random.insideUnitCircle.normalized * SYS_Starmap.Direct.avgSpeed * Random.Range(2, 6)));
			}

			foreach (StarInfo starInfo in initList) {
				ActivityEntity objGen = Instantiate(pfbActivity).GetComponent<ActivityEntity>();
				objGen.transform.SetParent(entityGroup);
				objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
				objGen.name = starInfo.name;

				objGen.Regist(starInfo, matActivities[2], Random.Range(0.8f, 1.2f), 3);

				UI_Navigator.Direct.Regist(objGen);
			}
		}
	}

	public void InitPlanet(List<StarInfo> initList) {
		//init log
		if (SYS_Logger.Direct.logging) {
			foreach (StarInfo starInfo in initList) {
				Debug.LogWarning("[Gen]BayerID : " + starInfo.sID + " , BayerName : " + starInfo.name + " , Location : " + starInfo.sPos);
			}
		}
		
		//gen star
		foreach (StarInfo starInfo in initList) {
			PlanetEntity objGen = Instantiate(pfbPlanet).GetComponent<PlanetEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x , starInfo.sPos.y , 0);
			objGen.name = starInfo.name;

			int eventId = Random.Range(0, matActivities.Count);

			if (starInfo.nvType == NaviType.Check) {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(1, 1.5f), Random.Range(0, 3) == 2 , eventId);
			} else {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(1.5f, 1.75f), Random.Range(0, 3) == 2 , eventId);
				tgtPlanet = objGen;
			}

			UI_Navigator.Direct.Regist(objGen);
			planets.Add(objGen);
		}
	}

	public void Reset(){
		foreach (Transform child in entityGroup) {
			Destroy(child.gameObject);
		}
		planets = new List<PlanetEntity>();
		tgtPlanet = null;
	}
}

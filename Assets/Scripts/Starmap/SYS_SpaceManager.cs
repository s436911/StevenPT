using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_SpaceManager : MonoBehaviour {
	public static SYS_SpaceManager Direct;
	public List<PlanetEntity> planets = new List<PlanetEntity>();
	public List<Material> matPlanets = new List<Material> ();
	public List<Material> matActivities = new List<Material>();
	public List<Material> matResoureces = new List<Material>();

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

	public int activityNum = 12;
	public int activityMax = 24;
	public int activityMin = 6;

	void Awake() {
		Direct = this;
	}
	
	public void Init() {
		InitPlanet(SYS_StarmapManager.Direct.starInfos);
		InitActivity();
		InitResoureces();
	}

	public void SplitResourece(Vector2 pos, Material mat , int resourceId) {
		StarInfo init = new StarInfo(StarType.Resoreces, pos);

		ResourecesEntity objGen = Instantiate(pfbResourece).GetComponent<ResourecesEntity>();
		objGen.transform.SetParent(entityGroup);
		objGen.transform.position = new Vector3(init.sPos.x, init.sPos.y, 0);
		objGen.name = init.sName;

		objGen.Regist(init, mat, Random.Range(0.9f, 1.1f), resourceId);
	}

	public void InitResoureces() {
		//Side Resoureces
		List<StarInfo> initList = new List<StarInfo>();
		ResourcesSet resrcSet;

		for (int ct = 0; ct < planets.Count * resrcInNum; ct++) {
			initList.Add(new StarInfo(StarType.Resoreces, SYS_StarmapManager.Direct.starInfos[Random.Range(0, SYS_StarmapManager.Direct.starInfos.Count)].sPos + Random.insideUnitCircle.normalized * SYS_StarmapManager.Direct.avgSpeed * Random.Range(resrcInMinT, resrcInMaxT)));
		}

		//Random Resoureces
		for (int ct = 0; ct < planets.Count * resrcOutNum; ct++) {
			initList.Add(new StarInfo(StarType.Resoreces, SYS_StarmapManager.Direct.starInfos[Random.Range(0, SYS_StarmapManager.Direct.starInfos.Count)].sPos + Random.insideUnitCircle.normalized * SYS_StarmapManager.Direct.avgSpeed * Random.Range(resrcOutMinT, resrcOutMaxT)));
		}

		foreach (StarInfo starInfo in initList) {
			ResourecesEntity objGen = Instantiate(pfbResoureces).GetComponent<ResourecesEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.sName;

			int stack = Random.Range(3, 5);

			if (SYS_Mission.Direct.nowMission.subResrc != null && Random.Range(0,4) == 0) {
				resrcSet = SYS_Mission.Direct.nowMission.subResrc;
			} else {
				resrcSet = SYS_Mission.Direct.nowMission.mainResrc;
			}
			objGen.Regist(starInfo, resrcSet.mat, 0.6f + stack * 0.15f, resrcSet.resourceId, stack);
		}

		initList = new List<StarInfo>();

		//Random Resourece
		for (int ct = 0; ct < planets.Count * resrcNum; ct++) {
			initList.Add(new StarInfo(StarType.Resoreces, SYS_StarmapManager.Direct.starInfos[Random.Range(0, SYS_StarmapManager.Direct.starInfos.Count)].sPos + Random.insideUnitCircle.normalized * SYS_StarmapManager.Direct.avgSpeed * Random.Range(resrcMinT, resrcMaxT)));
		}

		foreach (StarInfo starInfo in initList) {
			ResourecesEntity objGen = Instantiate(pfbResourece).GetComponent<ResourecesEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.sName;

			int stack = Random.Range(3, 5);

			if (SYS_Mission.Direct.nowMission.subResrc != null && Random.Range(0, 3) == 0) {
				resrcSet = SYS_Mission.Direct.nowMission.subResrc;
			} else {
				resrcSet = SYS_Mission.Direct.nowMission.mainResrc;
			}

			objGen.Regist(starInfo, resrcSet.mat, Random.Range(0.9f, 1.1f), resrcSet.resourceId);
		}
	}

	public void InitActivity() {
		List<StarInfo> initList = new List<StarInfo>();
		for (int ct = 0; ct < activityNum; ct++) {
			initList.Add(new StarInfo(StarType.Activity, SYS_StarmapManager.Direct.starInfos[Random.Range(0, SYS_StarmapManager.Direct.starInfos.Count)].sPos + Random.insideUnitCircle.normalized * SYS_StarmapManager.Direct.avgSpeed * Random.Range(activityMin, activityMax)));
		}

		foreach (StarInfo starInfo in initList) {
			ActivityEntity objGen = Instantiate(pfbActivity).GetComponent<ActivityEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
			objGen.name = starInfo.sName;

			int eventId = Random.Range(0, matActivities.Count);
			objGen.Regist(starInfo, matActivities[eventId], Random.Range(0.8f, 1.2f)  , eventId + 1);

			UI_Navigator.Direct.Regist(objGen);
		}
	}

	public void InitPlanet(List<StarInfo> initList) {
		//init log
		if (SYS_Logger.Direct.logging) {
			foreach (StarInfo starInfo in initList) {
				Debug.LogWarning("[Gen]BayerID : " + starInfo.sID + " , BayerName : " + starInfo.sName + " , Type : " + starInfo.sType.ToString() + " , Location : " + starInfo.sPos);
			}
		}
		
		//gen star
		foreach (StarInfo starInfo in initList) {
			PlanetEntity objGen = Instantiate(pfbPlanet).GetComponent<PlanetEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.position = new Vector3(starInfo.sPos.x , starInfo.sPos.y , 0);
			objGen.name = starInfo.sName;

			int eventId = Random.Range(0, matActivities.Count);

			if (starInfo.sType == StarType.Check) {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(0.75f, 1.5f), Random.Range(0, 3) == 2 , eventId);
			} else {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(1, 1.75f), Random.Range(0, 3) == 2 , eventId);
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

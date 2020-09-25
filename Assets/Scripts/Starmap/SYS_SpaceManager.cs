using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_SpaceManager : MonoBehaviour {
	public static SYS_SpaceManager Direct;
	public List<PlanetEntity> planets = new List<PlanetEntity>();
	public List<Material> matPlanets = new List<Material> ();
	public List<Material> matActivities = new List<Material>();

	public PlanetEntity tgtPlanet;
	public GameObject pfbPlanet;
	public GameObject pfbActivity;
	public Transform entityGroup;

	public int activityNum = 12;
	public int activityMax = 24;
	public int activityMin = 6;

	void Awake() {
		Direct = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Init() {
		InitPlanet(SYS_StarmapManager.Direct.starInfos);
		InitActivity();
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

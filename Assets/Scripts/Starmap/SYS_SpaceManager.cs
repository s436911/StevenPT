using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_SpaceManager : MonoBehaviour {
	public static SYS_SpaceManager Direct;
	public List<PlanetEntity> planets = new List<PlanetEntity>();
	public List<Material> matPlanets = new List<Material> ();
	public List<Material> matMeteors = new List<Material>();

	public PlanetEntity tgtPlanet;
	public GameObject pfbPlanet;
	public GameObject pfbMeteor;
	public Transform entityGroup;
	public int meteorNum = 1000;
	public float scaleRate = 1;

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
		InitMeteor();
	}

	public void InitMeteor() {
		for (int ct = 0; ct < meteorNum; ct++) {
			StarInfo starInfo = new StarInfo(StarType.Meteor);

			MeteorEntity objGen = Instantiate(pfbMeteor).GetComponent<MeteorEntity>();
			objGen.transform.SetParent(entityGroup);
			objGen.transform.localPosition = new Vector3(starInfo.sPos.x * scaleRate, starInfo.sPos.y * scaleRate, 0);
			objGen.Regist(starInfo, matMeteors[Random.Range(0, matMeteors.Count)], Random.Range(0.5f, 1.5F));
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
			objGen.transform.localPosition = new Vector3(starInfo.sPos.x * scaleRate, starInfo.sPos.y * scaleRate, 0);
			objGen.name = starInfo.sName;

			if (starInfo.sType == StarType.Check) {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(0.75f, 1.5f), Random.Range(0, 3) == 2);
			} else {
				objGen.Regist(starInfo, matPlanets[Random.Range(0, matPlanets.Count)], Random.Range(1, 1.75f), Random.Range(0, 3) == 2);
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

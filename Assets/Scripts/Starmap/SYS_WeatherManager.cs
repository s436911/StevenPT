using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_WeatherManager : MonoBehaviour {
	public static SYS_WeatherManager Direct;

	public List<Material> matBackMeteor = new List<Material>();
	public GameObject pfbBackMeteor;

	public List<Material> matFrontMeteor = new List<Material>();
	public GameObject pfbFrontMeteor;

	public Transform backGroup;
	public Transform frontGroup;
	public int backMeteorNum = 20;
	public int frontMeteorNum = 5;

	public float meteorRadiusT;
	public float timer;

	void Awake() {
		Direct = this;
	}

	public void Init() {
		for (int ct = 0; ct < backMeteorNum; ct++) {
			spawnBack((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * meteorRadiusT * 4);
		}

		for (int ct = 0; ct < frontMeteorNum; ct++) {
			spawnFront((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * meteorRadiusT * 4);
		}
	}
	
	public void spawnBack(Vector2 dePos) {
		StarInfo starInfo = new StarInfo(StarType.Meteor, dePos);

		SpaceEntity objGen = Instantiate(pfbBackMeteor).GetComponent<SpaceEntity>();
		objGen.transform.SetParent(backGroup);

		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, backGroup.transform.position.z);
		objGen.Regist(starInfo, matBackMeteor[Random.Range(0, matBackMeteor.Count)], Random.Range(0.3f, 0.9F));
	}

	public void spawnFront(Vector2 dePos) {
		StarInfo starInfo = new StarInfo(StarType.Meteor, dePos);

		MeteorEntity objGen = Instantiate(pfbFrontMeteor).GetComponent<MeteorEntity>();
		objGen.transform.SetParent(frontGroup);

		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, frontGroup.transform.position.z);
		objGen.Regist(starInfo, matFrontMeteor[Random.Range(0, matFrontMeteor.Count)], Random.Range(1f, 1.5F));
	} 

	// Update is called once per frame
	void Update() {
		if (Time.timeSinceLevelLoad - timer > 1) {
			List<Vector2> reVextors = new List<Vector2>();

			//背景清除
			foreach (Transform child in backGroup) {
				if (Vector2.Distance(SYS_ShipController.Direct.transform.position, child.position) > meteorRadiusT * 4) {
					reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
					Destroy(child.gameObject);
				}
			}

			foreach (Vector2 reVextor in reVextors) {
				spawnBack(reVextor);
			}

			reVextors = new List<Vector2>();

			//前景清除
			foreach (Transform child in frontGroup) {
				if (Vector2.Distance(SYS_ShipController.Direct.transform.position, child.position) > meteorRadiusT * 4) {
					reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
					Destroy(child.gameObject);
				}
			}

			foreach (Vector2 reVextor in reVextors) {
				spawnFront(reVextor);
			}
			
			timer = Time.timeSinceLevelLoad;
		}
	}

	public void Reset() {
		foreach (Transform child in backGroup) {
			Destroy(child.gameObject);
		}

		foreach (Transform child in frontGroup) {
			Destroy(child.gameObject);
		}
	}
}

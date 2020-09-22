using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_WeatherManager : MonoBehaviour {
	public static SYS_WeatherManager Direct;
	public List<Material> matMeteors = new List<Material>();

	public GameObject pfbMeteor;
	public Transform entityGroup;
	public int meteorNum = 25;

	public float meteorRadius;
	public float timer;

	void Awake() {
		Direct = this;
	}

	public void Init() {
		for (int ct = 0; ct < meteorNum; ct++) {
			spawnMeteor((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle * meteorRadius);
		}
	}
	
	public void spawnMeteor(Vector2 dePos) {
		StarInfo starInfo = new StarInfo(StarType.Meteor, dePos);

		MeteorEntity objGen = Instantiate(pfbMeteor).GetComponent<MeteorEntity>();
		objGen.transform.SetParent(entityGroup);

		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, 0);
		objGen.Regist(starInfo, matMeteors[Random.Range(0, matMeteors.Count)], Random.Range(0.5f, 1.5F));
	}

	// Update is called once per frame
	void Update() {
		if (Time.timeSinceLevelLoad - timer > 1) {
			List<Vector2> reVextors = new List<Vector2>();

			foreach (Transform child in entityGroup) {
				if (Vector2.Distance(SYS_ShipController.Direct.transform.position , child.position) > meteorRadius) {
					reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
					Destroy(child.gameObject);
				}
			}

			foreach (Vector2 reVextor in reVextors) {
				spawnMeteor(reVextor);
			}

			timer = Time.timeSinceLevelLoad;
		}

		
	}

	public void Reset() {
		foreach (Transform child in entityGroup) {
			Destroy(child.gameObject);
		}
	}
}

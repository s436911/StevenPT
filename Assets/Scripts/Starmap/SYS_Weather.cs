using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Weather : MonoBehaviour {
	public static SYS_Weather Direct;

	public List<Material> matBackMeteor = new List<Material>();
	public GameObject pfbBackMeteor;

	public List<GameObject> pfbMeteor;
	public List<GameObject> pfbCloud;
	public List<GameObject> pfbMeteorIce;

	public GameObject pfbBeetle;
	public GameObject pfbWhale;

	public ParticleSystem ptcIce;

	public Transform backGroup;
	public Transform frontGroup;
	public Transform weatherGroup;

	public int backMeteorNum = 20;
	public int frontMeteorNum = 5;
	public int meteorNumWeather = 3;

	public float meteorRadiusT;
	private float timer1;
	private float timer60;
	private WeatherType weather;

	public enum WeatherType {
		None,
		IceMeteors,
	}

	void Awake() {
		Direct = this;
	}

	public void Restart() {
		timer60 = Time.timeSinceLevelLoad;
		timer1 = Time.timeSinceLevelLoad;

		for (int ct = 0; ct < backMeteorNum; ct++) {
			SpawnBack((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle.normalized * Random.Range(0.5f, meteorRadiusT) * 4);
		}

		for (int ct = 0; ct < frontMeteorNum; ct++) {
			SpawnFront((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle.normalized * Random.Range(0.5f, meteorRadiusT) * 4 , frontGroup);
		}
	}

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (Time.timeSinceLevelLoad - timer60 > 60) {
				if (weather == WeatherType.None && Random.Range(0,4) == 0) {
					SetWeather(WeatherType.IceMeteors);
				} else {
					SetWeather(WeatherType.None);
				}

				timer60 = Time.timeSinceLevelLoad;
			}

			if (Time.timeSinceLevelLoad - timer1 > 1) {
				//背景清除
				List<Vector2> reVextors = new List<Vector2>();
				foreach (Transform child in backGroup) {
					if (Vector2.Distance(SYS_ShipController.Direct.transform.position, child.position) > meteorRadiusT * 4) {
						reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
						Destroy(child.gameObject);
					}
				}

				foreach (Vector2 reVextor in reVextors) {
					SpawnBack(reVextor);
				}

				//前景清除一般層
				reVextors = new List<Vector2>();
				foreach (Transform child in frontGroup) {
					if (Vector2.Distance(SYS_ShipController.Direct.transform.position, child.position) > meteorRadiusT * 4) {
						if (child.GetComponent<MeteorEntity>() && !child.GetComponent<MeteorEntity>().nonRespawn) {
							reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
						}
						Destroy(child.gameObject);
					}
				}

				foreach (Vector2 reVextor in reVextors) {
					SpawnFront(reVextor, frontGroup);
				}

				//前景清除天氣層
				reVextors = new List<Vector2>();
				foreach (Transform child in weatherGroup) {
					if (Vector2.Distance(SYS_ShipController.Direct.transform.position, child.position) > meteorRadiusT * 4) {
						reVextors.Add(2 * SYS_ShipController.Direct.transform.position - child.position);
						Destroy(child.gameObject);
					}
				}

				if (weather != WeatherType.None) {
					foreach (Vector2 reVextor in reVextors) {
						SpawnFront(reVextor, weatherGroup);
					}
				}

				timer1 = Time.timeSinceLevelLoad;
			}
		}
	}

	public void SetWeather(WeatherType value) {
		weather = value;
		if (SYS_Logger.Direct.logging) {
			Debug.LogWarning(weather.ToString());
		}

		if (weather == WeatherType.IceMeteors) {
			ptcIce.Play();

			for (int ct = 0; ct < meteorNumWeather; ct++) {
				SpawnFront((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle.normalized * Random.Range(3, meteorRadiusT) * 4, weatherGroup);
			}

		} else {
			ptcIce.Pause();
		}
	}

	public void SpawnBack(Vector2 dePos) {
		StarInfo starInfo = new StarInfo(MainType.Drift , SubType.Meteor, NaviType.None, Affinity.None, dePos);

		SpaceEntity objGen = Instantiate(pfbBackMeteor).GetComponent<SpaceEntity>();
		objGen.transform.SetParent(backGroup);

		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, backGroup.transform.position.z);
		objGen.Regist(starInfo, matBackMeteor[Random.Range(0, matBackMeteor.Count)], Random.Range(0.3f, 0.9F));
	}

	public void SpawnFront(Vector2 dePos , Transform group) {
		StarInfo starInfo = new StarInfo(MainType.Drift, SubType.Meteor, NaviType.None, Affinity.None, dePos);
		
		if (weather == WeatherType.None) {
			//隕石生成
			if (Random.Range(0, 7) != 0) {//7
				MeteorEntity objGen = Instantiate(pfbMeteor[Random.Range(0, pfbMeteor.Count)]).GetComponent<MeteorEntity>();
				objGen.transform.SetParent(group);
				objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z);
				objGen.Regist(starInfo, Random.Range(1f, 1.5F));

				if (Random.Range(0, 21) == 0) {
					SpawnBeetle(dePos, group, objGen.transform);
				}
			} else {
				//雲生成
				int randID = Random.Range(0, pfbCloud.Count);

				MeteorEntity objGen = Instantiate(pfbCloud[randID]).GetComponent<MeteorEntity>();
				objGen.transform.SetParent(group);
				objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z);
				objGen.Regist(starInfo, Random.Range(1f, 1.5F));

				if (randID == 2 && Random.Range(0, 3) == 0) {
					SpawnWhale(dePos, group, objGen.transform);
				}
			}

		} else {
			MeteorEntity objGen = Instantiate(pfbMeteorIce[Random.Range(0, pfbMeteorIce.Count)]).GetComponent<MeteorEntity>();
			objGen.transform.SetParent(group);
			objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z);
			objGen.Regist(starInfo , Random.Range(1f, 1.5F));
		}
	}

	public void SpawnBeetle(Vector2 dePos, Transform group , Transform tgt = null) {
		StarInfo starInfo = new StarInfo(MainType.Animal, SubType.None, NaviType.None, Affinity.None, dePos);
		AnimalEntity objGen = Instantiate(pfbBeetle).GetComponent<AnimalEntity>();
		objGen.transform.SetParent(group);
		objGen.tgt = tgt;
		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z - 1);
	}


	public void SpawnWhale(Vector2 dePos, Transform group, Transform tgt = null) {
		StarInfo starInfo = new StarInfo(MainType.Animal, SubType.None, NaviType.None, Affinity.None, dePos);
		AnimalEntity objGen = Instantiate(pfbWhale).GetComponent<AnimalEntity>();
		objGen.transform.SetParent(group);
		objGen.tgt = tgt;
		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z - 1);
	}

	public void Reset() {
		foreach (Transform child in backGroup) {
			Destroy(child.gameObject);
		}

		foreach (Transform child in frontGroup) {
			Destroy(child.gameObject);
		}

		SetWeather(WeatherType.None);
	}
}

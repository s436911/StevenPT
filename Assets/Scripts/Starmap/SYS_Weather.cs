using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Weather : MonoBehaviour {
	public static SYS_Weather Direct;

	public List<Material> matBackMeteor = new List<Material>();
	public GameObject pfbBackMeteor;

	public List<GameObject> pfbMeteor;
	public List<GameObject> pfbCloud;

	public GameObject pfbBeetle;
	public GameObject pfbWhale;

	public CanvasGroup uiCanvas;

	public ParticleSystem ptcIce;

	public Transform solarTrans;
	public float solarValue = 0;

	public Transform backGroup;
	public Transform frontGroup;
	public Transform weatherGroup;

	public int backMeteorNum = 20;
	public int frontMeteorNum = 5;
	public int meteorNumWeather = 3;

	public float meteorRadiusT;
	private float timer1;
	private float timer60;
	private int weather;

	/*
	0晴天
	1雪天
	*/

	void Awake() {
		Direct = this;
	}

	public void Restart() {
		timer60 = Time.timeSinceLevelLoad;
		timer1 = Time.timeSinceLevelLoad;
		SetWeather(SYS_Mission.Direct.GetGalaxy().GetWeather());

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
				SetWeather(SYS_Mission.Direct.GetGalaxy().GetWeather());
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

				if (weather != 0) {
					foreach (Vector2 reVextor in reVextors) {
						SpawnFront(reVextor, weatherGroup);
					}
				}

				timer1 = Time.timeSinceLevelLoad;
			}

			if (weather == 2) {
				solarValue = Mathf.Lerp(solarValue, 1, 0.25f * Time.fixedDeltaTime);
				solarTrans.localPosition = Vector3.Lerp(solarTrans.localPosition, Vector3.zero, 0.25f * Time.fixedDeltaTime);
				uiCanvas.alpha = 1 - Random.Range(Mathf.Sin(Time.time) * 0.75f, 1F) * solarValue;

			} else {
				solarValue = Mathf.Lerp(solarValue, 0, 0.25f * Time.fixedDeltaTime);
				solarTrans.localPosition = Vector3.Lerp(solarTrans.localPosition, new Vector3(0, -20, 0), 0.5f * Time.fixedDeltaTime);
			}
		}
	}

	public int GetWeather() {
		return weather;
	}

	public void SetWeather(int value) {
		weather = value;
		if (SYS_Logger.Direct.logging) {
			Debug.LogWarning(weather.ToString());
		}

		ptcIce.Pause();
		uiCanvas.alpha = 1;

		if (weather == 1) {
			ptcIce.Play();

			for (int ct = 0; ct < meteorNumWeather; ct++) {
				SpawnFront((Vector2)SYS_ShipController.Direct.transform.position + Random.insideUnitCircle.normalized * Random.Range(3, meteorRadiusT) * 4, weatherGroup);
			}

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
		int id = 0;

		if (weather == 1 && Random.Range(0, 100) < 33) {
			id = 2;
		} else {
			id = SYS_Mission.Direct.GetGalaxy().GetMeteor();
		}

		MeteorEntity objGen = Instantiate(pfbMeteor[id]).GetComponent<MeteorEntity>();
		objGen.transform.SetParent(group);
		objGen.transform.position = new Vector3(starInfo.sPos.x, starInfo.sPos.y, group.transform.position.z);
		objGen.Regist(starInfo);

		if (id == 0 && Random.Range(0, 21) == 0) {
			SpawnBeetle(dePos, group, objGen.transform);

		} else if (id == 5 && Random.Range(0, 3) == 0) {
			SpawnWhale(dePos, group, objGen.transform);
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

		SetWeather(0);
	}
}

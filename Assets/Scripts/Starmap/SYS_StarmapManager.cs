using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SYS_StarmapManager : MonoBehaviour {
	public static SYS_StarmapManager Direct;
	public GameObject starPfb;
	public int difficult = 0;

	public float avgSpeed = 4;

	public int goalDisMaxT = 50;
	public int goalDisMinT = 40;

	public int planetDisMaxT = 45;
	public int planetDisMinT = 25;

	public int planetMainDisT = 25;
	public int planetOtherDisT = 35;

	public int planetPerHighT = 2;
	public int planetPerLowT = 2;

	public int baseStarNum = 4;
	public int otherStarNum = 5;

	public int planetSideMin = 1;
	public int planetSideMax = 2;

	public LineRenderer line;

	public List<StarInfo> starInfos = new List<StarInfo>();
	public List<StarInfo> route = new List<StarInfo>();

	void Awake() {
		Direct = this;
	}

	public bool IsInMatrix(Vector2 p1, Vector2 p2, Vector2 p) {
		return (p1.x - p.x) * (p2.x - p.x) <= 0 && (p1.y - p.y) * (p2.y - p.y) <= 0;
	}


	private Vector2 GenRandPlanetPos() {
		Vector2 genOffset = Vector2.zero;

		int couter = 0;
		while (genOffset == Vector2.zero) {
			genOffset = CheckPlanetPos(new Vector2(Random.Range(-250,250) , Random.Range(0, 750)) , planetOtherDisT);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + genOffset + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}

		return genOffset;
	}

	private Vector2 GenGoalPos(Vector2 starter, int starNum = 0, int starNumLeft = 0) {
		Vector2 genOffset = Vector2.zero;

		int couter = 0;
		while (genOffset == Vector2.zero) {
			Vector2 routeDirect = Vector2.zero;
			Vector2 randOffset = Vector2.zero;
			/*
			foreach (StarInfo starInfo in starInfos) {
				routeDirect += starInfo.sPos.normalized;
			}*/

			routeDirect = starter.normalized;

			if (Mathf.Abs(routeDirect.x) < 0.7f) {
				randOffset = new Vector2(routeDirect.x > 0 ? -Random.Range(0.5f, 1) : Random.Range(0.5f, 1), Random.Range(0.5f, 1)).normalized;
			} else {
				randOffset = new Vector2(routeDirect.x > 0 ? -Random.Range(0, 0.5f) : Random.Range(0, 0.5f), Random.Range(0, 0.5f)).normalized;
			}
			
			randOffset = randOffset * Random.Range(goalDisMinT, goalDisMaxT + (difficult - SYS_ResourseManager.Direct.engineLv) * 2) * avgSpeed; //向量 * 時間 * 期望速度

			genOffset = CheckPlanetPos(starter + randOffset , planetMainDisT, starter.y, starNum, starNumLeft);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + starNum + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}

		return genOffset;
	}

	private Vector2 GenPlanetPos(Vector2 starter, int starNum = 0, int starNumLeft = 0) {
		Vector2 genOffset = Vector2.zero;

		int couter = 0;
		while (genOffset == Vector2.zero) {
			Vector2 randOffset = Random.insideUnitCircle.normalized * Random.Range(planetDisMinT, planetDisMaxT + (difficult - SYS_ResourseManager.Direct.engineLv) * 2) * avgSpeed; //向量 * 時間 * 期望速度
			if (randOffset.y < 0 && starNum != 0) {
				randOffset.y = randOffset.y * -1;

			}
			genOffset = CheckPlanetPos(starter + randOffset , planetMainDisT, starter.y, starNum , starNumLeft);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + starNum + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}

		return genOffset;
	}
	
	public void Init() {
		//init starinfo
		int planetNum = baseStarNum + 1;

		for (int ct = 0; ct < planetNum; ct++) {
			Vector2 randPos = Vector2.zero;

			if (ct < baseStarNum) {
				if (ct == 0) {
					randPos = GenPlanetPos(Vector2.zero, ct + 1, planetNum - ct - 1);
				} else {
					randPos = GenPlanetPos(starInfos[ct - 1].sPos, ct + 1, planetNum - ct - 1);
				}
				starInfos.Add(new StarInfo(StarType.Check, randPos));

			} else {
				randPos = GenGoalPos(starInfos[ct - 1].sPos, ct + 1, planetNum - ct - 1);
				starInfos.Add(new StarInfo(StarType.End, randPos));

			} 
		}
		
		//支線星球生成
		planetNum = Random.Range(planetSideMin, planetSideMax + 1);

		for (int ct = 0; ct < planetNum; ct++) {
			starInfos.Add(new StarInfo(StarType.Check, GenPlanetPos(starInfos[Random.Range(0, starInfos.Count)].sPos)));
		}

		//亂數星球生成
		planetNum = otherStarNum - planetNum + (Random.Range(0 , 100) < (SYS_ResourseManager.Direct.scopeLv * 10) ? 1 : 0);//

		for (int ct = 0; ct < planetNum; ct++) {
			starInfos.Add(new StarInfo(StarType.Check, GenRandPlanetPos()));
		}


		//init log
		if (SYS_Logger.Direct.logging) {
			foreach (StarInfo starInfo in starInfos) {
				Debug.LogWarning("[Gen]BayerID : " + starInfo.sID + " , BayerName : " + starInfo.sName + " , Type : " + starInfo.sType.ToString() + " , Location : " + starInfo.sPos);
			}
		}

		//gen star
		foreach (StarInfo starInfo in starInfos) {
			StarObject objGen = Instantiate(starPfb).GetComponent<StarObject>();
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(transform);
			objGen.Regist(starInfo);
			objRect.anchoredPosition3D = starInfo.sPos;
			if (starInfo.sType != StarType.End) {
				objGen.sRawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
			} else {
				objGen.sRawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
			}

			objGen.sRawImage.color = starInfo.sColor;
			objRect.localScale = Vector3.one;
		}
	}
	
	public bool IsRouteComplete() {
		return route.Count > 0 && route[route.Count - 1].sType == StarType.End;
	}

	// Update is called once per frame
	public void UpdateLine() {

		if (route.Count > 0) {

			List<Vector3> linePos = new List<Vector3>();

			linePos.Add(Vector3.zero);

			foreach (StarInfo routePos in route) {
				linePos.Add(new Vector3(routePos.sPos.x, routePos.sPos.y, 0));
			}

			Vector3[] tempV3 = linePos.ToArray();

			line.gameObject.SetActive(true);
			line.positionCount = tempV3.Length;
			line.SetPositions(tempV3);

		} else {
			line.gameObject.SetActive(false);
		} 
	}

	public Vector2 CheckPlanetPos(Vector2 checkPos , float checkDis, float startHigh = 0, int starNum = 0, int starNumLeft = 0) {
		if (Mathf.Abs(checkPos.x) > 250 || checkPos.y > 750 || checkPos.y < 0) {
			checkPos = Vector2.zero;

		} else {
			foreach (StarInfo starInfo in starInfos) {
				if (Vector2.Distance(checkPos, starInfo.sPos) < checkDis * avgSpeed) { //時間 * 期望速度
					checkPos = Vector2.zero;
					break;
				}
			}

			if (starNum > 0) {
				if (750 - starNumLeft * planetPerHighT * avgSpeed > startHigh + starNum * planetPerLowT * avgSpeed) {
					if (checkPos.y > 750 - starNumLeft * planetPerHighT * avgSpeed) {
						checkPos = Vector2.zero;

					} else if (checkPos.y < startHigh + starNum * planetPerLowT * avgSpeed) {
						checkPos = Vector2.zero;
					}
				} else {
					if (checkPos.y > 750 - starNumLeft * planetPerHighT * avgSpeed) {
						checkPos = Vector2.zero;

					} 
				}
			}
		}

		return checkPos;
	}

	public void Reset() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		
		starInfos = new List<StarInfo>();
		ResetRoute();
	}
	
	public void AutoRoute() {
		ResetRoute();
		foreach (StarInfo info in starInfos) {
			ClickRoute(info);
			if (info.sType == StarType.End) {
				break;
			}
		}
	}

	public void ResetRoute() {
		route = new List<StarInfo>();
		line.positionCount = 0;
	}

	public void SetDifficult(InputField value) {
		int tmpDifficult = int.Parse(value.text);

		if (tmpDifficult > 5 + SYS_ResourseManager.Direct.shipLv) {
			tmpDifficult = 5 + SYS_ResourseManager.Direct.shipLv;
			value.text = tmpDifficult.ToString();

		} else if (tmpDifficult < 0) {
			tmpDifficult = 0;
			value.text = tmpDifficult.ToString();
		}

		difficult = tmpDifficult;
		Reset();
		Init();
	}

	public void ClickRoute(StarInfo sInfo) {
		if (route.Count > 0) {
			if (route[route.Count - 1].sType != StarType.End) {
				if (route[route.Count - 1] != sInfo) {
					if (!route.Contains(sInfo)) {
						route.Add(sInfo);
						UpdateLine();
					} else {
						SYS_Logger.Direct.SetSystemMsg("無法註冊已存的目標");
					}

				} else {
					route.Remove(sInfo);
					UpdateLine();
				}

			} else {
				if (route[route.Count - 1] == sInfo) {
					route.Remove(sInfo);
					UpdateLine();

				} else {
					SYS_Logger.Direct.SetSystemMsg("無法註冊在終點之後");
				}
			}

		} else {
			route.Add(sInfo);
			UpdateLine();
		}
	}
}

public class StarInfo {
	public StarType sType;
	public string sName;
	public int sID;
	public Vector2 sPos;
	public Color sColor;

	public StarInfo(StarType type, Vector2 sPos) {
		sType = type;
		this.sPos = sPos;

		if (type == StarType.Check) {
			BayerCreater();
			if (Random.Range(0, 100) >= 20) {
				sColor = new Color(0.9f, 0.9f, 0.9f);

			} else {
				sColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
			}

		} else if (type == StarType.End) {
			BayerCreater();
			sColor = new Color(1, 0.85f, 0);
		} 
	}


	private void BayerCreater() {
		//星座
		List<string> bayerMainName = new List<string>();
		bayerMainName.Add("And");
		bayerMainName.Add("Ant");
		bayerMainName.Add("Aps");
		bayerMainName.Add("Aql");
		bayerMainName.Add("Aqr");
		bayerMainName.Add("Ara");
		bayerMainName.Add("Ari");
		bayerMainName.Add("Aur");
		bayerMainName.Add("Boo");
		bayerMainName.Add("Cae");
		bayerMainName.Add("Cam");
		bayerMainName.Add("Cap");
		bayerMainName.Add("Car");
		bayerMainName.Add("Cas");
		bayerMainName.Add("Cen");
		bayerMainName.Add("Cep");
		bayerMainName.Add("Cet");
		bayerMainName.Add("Cha");
		bayerMainName.Add("Cir");
		bayerMainName.Add("CMa");
		bayerMainName.Add("CMi");
		bayerMainName.Add("Cnc");
		bayerMainName.Add("Col");
		bayerMainName.Add("Com");
		bayerMainName.Add("CrA");
		bayerMainName.Add("CrB");
		bayerMainName.Add("Crt");
		bayerMainName.Add("Cru");
		bayerMainName.Add("Crv");
		bayerMainName.Add("CVn");
		bayerMainName.Add("Cyg");
		bayerMainName.Add("Del");
		bayerMainName.Add("Dor");
		bayerMainName.Add("Dra");
		bayerMainName.Add("Equ");
		bayerMainName.Add("Eri");
		bayerMainName.Add("For");
		bayerMainName.Add("Gem");
		bayerMainName.Add("Gru");
		bayerMainName.Add("Her");
		bayerMainName.Add("Hor");
		bayerMainName.Add("Hya");
		bayerMainName.Add("Hyi");
		bayerMainName.Add("Ind");
		bayerMainName.Add("Lac");
		bayerMainName.Add("Leo");
		bayerMainName.Add("Lep");
		bayerMainName.Add("Lib");
		bayerMainName.Add("LMi");
		bayerMainName.Add("Lup");
		bayerMainName.Add("Lyn");
		bayerMainName.Add("Lyr");
		bayerMainName.Add("Men");
		bayerMainName.Add("Mic");
		bayerMainName.Add("Mon");
		bayerMainName.Add("Mus");
		bayerMainName.Add("Nor");
		bayerMainName.Add("Oct");
		bayerMainName.Add("Oph");
		bayerMainName.Add("Ori");
		bayerMainName.Add("Pav");
		bayerMainName.Add("Peg");
		bayerMainName.Add("Per");
		bayerMainName.Add("Phe");
		bayerMainName.Add("Pic");
		bayerMainName.Add("PsA");
		bayerMainName.Add("Psc");
		bayerMainName.Add("Pup");
		bayerMainName.Add("Pyx");
		bayerMainName.Add("Ret");
		bayerMainName.Add("Scl");
		bayerMainName.Add("Sco");
		bayerMainName.Add("Sct");
		bayerMainName.Add("Ser");
		bayerMainName.Add("Sex");
		bayerMainName.Add("Sge");
		bayerMainName.Add("Sgr");
		bayerMainName.Add("Tau");
		bayerMainName.Add("Tel");
		bayerMainName.Add("TrA");
		bayerMainName.Add("Tri");
		bayerMainName.Add("Tuc");
		bayerMainName.Add("UMa");
		bayerMainName.Add("UMi");
		bayerMainName.Add("Vel");
		bayerMainName.Add("Vir");
		bayerMainName.Add("Vol");
		bayerMainName.Add("Vul");

		//星名
		string bayerSubName = "αβγδεζηθικλμνξοπρστυφχψω";
		string planetName = "bcdefghijk";

		int bayerMain = Random.Range(0, bayerMainName.Count);
		int bayerSub = Random.Range(0, bayerSubName.Length);
		int bayerMutiStar = ((Random.Range(0, 100) >= 85) ? 1 : 0) + ((Random.Range(0, 100) >= 98) ? 1 : 0);
		int planetNum = Random.Range(0, planetName.Length);
		int satelliteNum = ((Random.Range(0, 100) >= 90) ? 1 : 0) + ((Random.Range(0, 100) >= 95) ? 1 : 0) + ((Random.Range(0, 100) >= 97) ? 1 : 0) + ((Random.Range(0, 100) >= 99) ? 1 : 0);

		sName = bayerSubName[bayerSub] + (bayerMutiStar > 0 ? bayerMutiStar.ToString() : "") + " " + bayerMainName[bayerMain] + " " + planetName[planetNum] + (bayerMutiStar > 0 ? " " + bayerMutiStar.ToString() : "");
		sID = bayerMain * 100000 + bayerSub * 1000 + bayerMutiStar * 100 + planetNum * 10 + satelliteNum;
	}





	private Vector2 GenPos() {
		return new Vector2(Random.Range(-360, 360), Random.Range(0, 1280));
	}
}

public enum StarType {
	//planet
	Check,
	End,

	//weather
	Meteor,

	//activity
	Activity
}
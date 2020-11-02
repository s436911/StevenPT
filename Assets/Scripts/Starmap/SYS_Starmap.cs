using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SYS_Starmap : MonoBehaviour {
	public static SYS_Starmap Direct;
	public Transform uiPanel;

	public GameObject starPfb;
	public ParticleSystem backNarblua;
	public Vector2 backColor;

	public float avgSpeed = 4;

	public int goalDisMaxT = 50;
	public int goalDisMinT = 40;

	public int planetDisMaxT = 45;
	public int planetDisMinT = 25;

	public int planetMainDisT = 25;
	public int planetOtherDisT = 35;

	public int planetPerHighT = 2;
	public int planetPerLowT = 2;

	public int otherStarNum = 5;

	public int planetSideMin = 1;
	public int planetSideMax = 2;

	public LineRenderer line;
	public LineRenderer preline;

	public List<StarInfo> stars = new List<StarInfo>();
	public List<StarInfo> route = new List<StarInfo>();
	public List<StarInfo> preroute = new List<StarInfo>();
	//星球座標X-250,250 Y0,750

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
			genOffset = CheckPlanetPos(new Vector2(Random.Range(-250, 250), Random.Range(0, 750)), planetOtherDisT);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + genOffset + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}

		return genOffset;
	}

	private Vector2 GenGoalPos(int difficult, Vector2 starter, int starNum = 0, int starNumLeft = 0) {
		Vector2 genOffset = Vector2.zero;

		int couter = 0;
		while (genOffset == Vector2.zero) {
			Vector2 routeDirect = Vector2.zero;
			Vector2 randOffset = Vector2.zero;

			routeDirect = starter.normalized;

			if (Mathf.Abs(routeDirect.x) < 0.7f) {
				randOffset = new Vector2(routeDirect.x > 0 ? -Random.Range(0.5f, 1) : Random.Range(0.5f, 1), Random.Range(0.5f, 1)).normalized;
			} else {
				randOffset = new Vector2(routeDirect.x > 0 ? -Random.Range(0, 0.5f) : Random.Range(0, 0.5f), Random.Range(0, 0.5f)).normalized;
			}

			randOffset = randOffset * Random.Range(goalDisMinT, goalDisMaxT + (difficult - SYS_Save.Direct.GetResearch(0)) * 2) * avgSpeed; //向量 * 時間 * 期望速度
			genOffset = CheckPlanetPos(starter + randOffset, planetMainDisT, starter.y, starNum, starNumLeft);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + starNum + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}
		return genOffset;
	}

	private Vector2 GenPlanetPos(int difficult, Vector2 starter, int starNum = 0, int starNumLeft = 0) {
		Vector2 genOffset = Vector2.zero;

		int couter = 0;
		while (genOffset == Vector2.zero) {
			Vector2 randOffset = Random.insideUnitCircle.normalized * Random.Range(planetDisMinT, planetDisMaxT + (difficult - SYS_Save.Direct.GetResearch(0)) * 2) * avgSpeed; //向量 * 時間 * 期望速度
			if (randOffset.y < 0 && starNum != 0) {
				randOffset.y = randOffset.y * -1;

			}
			genOffset = CheckPlanetPos(starter + randOffset, planetMainDisT, starter.y, starNum, starNumLeft);

			if (couter > 1000) {
				Debug.LogError("GenPlanetPos:" + starNum + " 無限迴圈");
				break;

			} else {
				couter++;
			}
		}
		return genOffset;
	}

	public void GenStarmap(MissionSet misSet) {
		Reset();
		//init starinfo
		int planetNum = misSet.mainStarNum + 1;
		int base1 = Random.Range(0, 2);
		int base2 = Random.Range(2, misSet.mainStarNum);
		int iact1 = Random.Range(0, misSet.mainStarNum);
		int iact2;

		//生成完一般星球後生成目標星球
		for (int ct = 0; ct < planetNum; ct++) {
			Vector2 randPos = Vector2.zero;

			if (ct < misSet.mainStarNum) {
				if (ct == 0) {
					randPos = GenPlanetPos(misSet.difficult, Vector2.zero, ct + 1, planetNum - ct - 1);
				} else {
					randPos = GenPlanetPos(misSet.difficult, stars[ct - 1].sPos, ct + 1, planetNum - ct - 1);
				}
				stars.Add(new StarInfo(MainType.Planet, (ct == base1 || ct == base2) ? SubType.Base : SubType.None, NaviType.Check , (Affinity)Random.Range(0, 4), randPos, ct == iact1 ? 1 : 0));

			} else {
				randPos = GenGoalPos(misSet.difficult, stars[ct - 1].sPos, ct + 1, planetNum - ct - 1);
				stars.Add(new StarInfo(MainType.Planet, SubType.None, NaviType.End , Affinity.None, randPos));
			}
		}

		//支線星球生成
		planetNum = Random.Range(planetSideMin, planetSideMax + 1);
		iact2 = Random.Range(0, planetNum);

		for (int ct = 0; ct < planetNum; ct++) {
			stars.Add(new StarInfo(MainType.Planet, ct == 0 ? SubType.Base : SubType.None, NaviType.Check , (Affinity)Random.Range(0, 4), GenPlanetPos(misSet.difficult, stars[Random.Range(0, stars.Count)].sPos), ct == iact2 ? 1 : 0));
		}

		//亂數星球生成
		planetNum = otherStarNum - planetNum + (Random.Range(0, 100) < (SYS_Save.Direct.GetResearch(3) * 10) ? 1 : 0);//

		for (int ct = 0; ct < planetNum; ct++) {
			stars.Add(new StarInfo(MainType.Planet, ct == 0 ? SubType.Base : SubType.None, NaviType.Check, (Affinity)Random.Range(0, 4), GenRandPlanetPos()));
		}


		//init log
		if (SYS_Logger.Direct.logging) {
			foreach (StarInfo starInfo in stars) {
				Debug.LogWarning("[Gen]BayerID : " + starInfo.sID + " , BayerName : " + starInfo.subType + " , Type : " + starInfo.subType.ToString() + " , Location : " + starInfo.sPos);
			}
		}

		//gen star
		foreach (StarInfo starInfo in stars) {
			StarObject objGen = Instantiate(starPfb).GetComponent<StarObject>();
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(uiPanel);
			objGen.Regist(starInfo);
			objRect.anchoredPosition3D = starInfo.sPos;

			if (starInfo.nvType != NaviType.End) {
				objGen.sRawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
				objGen.halo.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
			} else {
				objGen.sRawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
				objGen.halo.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
			}

			objGen.halo.gameObject.SetActive(starInfo.subType == SubType.Base);
			objGen.halo.color = new Color(starInfo.sColor.r , starInfo.sColor.g , starInfo.sColor.b , objGen.halo.color.a);
			objGen.sRawImage.color = starInfo.sColor;
			objGen.iactive.color = starInfo.sColor;
			objGen.iactive.gameObject.SetActive(starInfo.iactNum > 0);
			objRect.localScale = Vector3.one;
		}

		//backNarblua
		if (Random.Range(0, 3) == 0) {
			backNarblua.startColor = new Color(backColor.x, backColor.y, Random.Range(backColor.x, backColor.y), 0.06f);
		} else if (Random.Range(0, 2) == 0) {
			backNarblua.startColor = new Color(Random.Range(backColor.x, backColor.y), backColor.x, backColor.y, 0.06f);
		} else {
			backNarblua.startColor = new Color(backColor.y, Random.Range(backColor.x, backColor.y), backColor.x, 0.06f);
		}
		backNarblua.gameObject.SetActive(false);
		backNarblua.gameObject.SetActive(true);

		ResetPreRoute();
		AutoRoute(preline, preroute);
	}

	public bool IsRouteComplete() {
		return route.Count > 0 && route[route.Count - 1].nvType == NaviType.End;
	}

	public void UpdateLine(LineRenderer lineValue, List<StarInfo> routeValue) {
		if (routeValue.Count > 0) {
			List<Vector3> linePos = new List<Vector3>();
			linePos.Add(Vector3.zero);

			foreach (StarInfo routePos in routeValue) {
				linePos.Add(new Vector3(routePos.sPos.x, routePos.sPos.y, 0));
			}

			Vector3[] tempV3 = linePos.ToArray();

			lineValue.gameObject.SetActive(true);
			lineValue.positionCount = tempV3.Length;
			lineValue.SetPositions(tempV3);

		} else {
			lineValue.gameObject.SetActive(false);
		}
	}

	public Vector2 CheckPlanetPos(Vector2 checkPos, float checkDis, float startHigh = 0, int starNum = 0, int starNumLeft = 0) {
		if (Mathf.Abs(checkPos.x) > 250 || checkPos.y > 750 || checkPos.y < 0) {
			checkPos = Vector2.zero;

		} else {
			foreach (StarInfo starInfo in stars) {
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
		ResetStars();
		ResetRoute();
		ResetPreRoute();
	}

	public void ResetStars() {
		foreach (Transform child in uiPanel) {
			Destroy(child.gameObject);
		}

		stars = new List<StarInfo>();
	}

	public void AutoRoute() {
		ResetRoute();
		AutoRoute(line, route);
	}

	public void AutoRoute(LineRenderer lineValue, List<StarInfo> routeValue) {
		foreach (StarInfo info in stars) {
			ClickRoute(info , lineValue, routeValue);
			if (info.nvType == NaviType.End) {
				break;
			}
		}
	}

	public void ResetRoute() {
		route = new List<StarInfo>();
		line.positionCount = 0;
	}

	public void ResetPreRoute() {
		preroute = new List<StarInfo>();
		preline.positionCount = 0;
	}

	public void ClickRoute(StarInfo sInfo) {
		ClickRoute(sInfo, line, route);
	}

	public void ClickRoute(StarInfo sInfo , LineRenderer lineValue, List<StarInfo> routeValue) {
		if (routeValue.Count > 0) {
			if (routeValue[routeValue.Count - 1].nvType != NaviType.End) {
				if (routeValue[routeValue.Count - 1] != sInfo) {
					if (!routeValue.Contains(sInfo)) {
						routeValue.Add(sInfo);
						UpdateLine(lineValue , routeValue);
					} else {
						SYS_Logger.Direct.SetSystemMsg("無法註冊已存的目標");
					}

				} else {
					routeValue.Remove(sInfo);
					UpdateLine(lineValue, routeValue);
				}

			} else {
				if (routeValue[routeValue.Count - 1] == sInfo) {
					routeValue.Remove(sInfo);
					UpdateLine(lineValue, routeValue);

				} else {
					SYS_Logger.Direct.SetSystemMsg("無法註冊在終點之後");
				}
			}

		} else {
			routeValue.Add(sInfo);
			UpdateLine(lineValue, routeValue);
		}
	}

	public StarInfo GetInfoTGT() {
		foreach (StarInfo star in stars) {
			if (star.nvType == NaviType.End) {
				return star;
			}
		}
		return null;
	}
}

public class StarInfo {
	public MainType mainType;
	public SubType subType;
	public NaviType nvType;
	public Affinity affinity;
	public string name;
	public int sID;
	public int iactNum;
	public Vector2 sPos;
	public Color sColor;
	 
	public StarInfo(MainType mainType , SubType subType, NaviType nvType , Affinity affinity, Vector2 sPos , int iactNum = 0) {
		this.mainType = mainType;
		this.subType = subType;
		this.nvType = nvType;
		this.sPos = sPos;
		this.affinity = affinity;
		this.iactNum = iactNum;
		
		if (nvType == NaviType.Check) {
			BayerCreater();
			if (affinity == Affinity.None) {
				sColor = new Color(0.9f, 0.9f, 0.9f);

			} else if(affinity == Affinity.Fight) {
				sColor = new Color(0.9f, 0.5f, 0.4f);

			} else if (affinity == Affinity.Trade) {
				sColor = new Color(0.9f, 0.8f, 0.4f);

			} else if (affinity == Affinity.Explore) {
				sColor = new Color(0.7f, 0.9f, 0.4f);

			}

		} else if (nvType == NaviType.End) {
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

		name = bayerSubName[bayerSub] + (bayerMutiStar > 0 ? bayerMutiStar.ToString() : "") + " " + bayerMainName[bayerMain] + " " + planetName[planetNum] + (bayerMutiStar > 0 ? " " + bayerMutiStar.ToString() : "");
		sID = bayerMain * 100000 + bayerSub * 1000 + bayerMutiStar * 100 + planetNum * 10 + satelliteNum;
	}

	private Vector2 GenPos() {
		return new Vector2(Random.Range(-360, 360), Random.Range(0, 1280));
	}
}

public enum MainType {
	None,

	Planet,
	InterAct,
	Drift,
	Animal,
}

public enum SubType {
	None,

	//Planet
	Base,

	//Meteor
	Meteor,
	Resoreces
}

public enum NaviType {
	None,

	//Planet
	Check,
	End,

	//Interactive
	Activity,
}
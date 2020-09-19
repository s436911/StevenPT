using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SYS_StarmapManager : MonoBehaviour {
	public static SYS_StarmapManager Direct;
	public GameObject starPfb;
	public int difficult = 0;
	public int starDis = 100;

	public int baseStarMin = 8;
	public int baseStarMax = 9;

	public List<StarInfo> starInfos = new List<StarInfo>();

	void Awake() {
		Direct = this;
	}

	public void Init() {
		//init starinfo
		for (int ct = 0; ct < Random.Range(baseStarMin - difficult, baseStarMax - difficult); ct++) {
			starInfos.Add(new StarInfo(StarType.Check , ct < 3));
		}
		starInfos.Add(new StarInfo(StarType.End));

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
			if (starInfo.sType == StarType.End) {
				objGen.sRawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);				
			}
			objGen.sRawImage.color = starInfo.sColor;
			objRect.localScale = Vector3.one;
		}
	}

	// Start is called before the first frame update
	void Start() {
		Init();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public Vector2 CheckEntityPos(Vector2 checkPos) {
		foreach (StarInfo starInfo in Direct.starInfos) {
			if (Vector2.Distance(checkPos, starInfo.sPos) < starDis) {
				checkPos = Vector2.zero;
				continue;
			}
		}

		return checkPos;
	}

	public void Reset() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}

		starInfos = new List<StarInfo>();
	}

	public void SetDifficult(InputField value) {
		int tmpDifficult = int.Parse(value.text);

		if (tmpDifficult > 3) {
			tmpDifficult = 3;
			value.text = "3";

		} else if (tmpDifficult < 0) {
			tmpDifficult = 0;
			value.text = "0";
		}

		difficult = tmpDifficult;
		Reset();
		Init();
	}
}

public class StarInfo {
	public StarType sType;
	public string sName;
	public int sID;
	public Vector2 sPos;
	public Color sColor;

	public StarInfo(StarType type , bool coreStar = false ) {
		sType = type;

		if (type == StarType.Check) {
			BayerCreater();
			sPos = GenCheckStarPos(coreStar);
			if (Random.Range(0, 100) >= 20) {
				sColor = new Color(0.9f, 0.9f, 0.9f);

			} else {
				sColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
			}

		} else if (type == StarType.End) {
			BayerCreater();
			sPos = GenTgtStarPos();
			sColor = new Color(1, 0.85f, 0);
		} else {
			sPos = GenPos();
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
	
	//Trip Manager map x600 y900	
	private Vector2 GenTgtStarPos() {
		Vector2 genPos = Vector2.zero;

		while (genPos == Vector2.zero) {
			genPos = SYS_StarmapManager.Direct.CheckEntityPos(new Vector2(Random.Range(-300, 300), Random.Range(850, 900)));
		}

		return genPos;
	}

	private Vector2 GenCheckStarPos(bool coreStar = false) {
		Vector2 genPos = Vector2.zero;

		if (coreStar) {
			while (genPos == Vector2.zero) {
				genPos = SYS_StarmapManager.Direct.CheckEntityPos(new Vector2(Random.Range(-210, 210), Random.Range(180, 720)));				
			}

		} else {
			while (genPos == Vector2.zero) {
				genPos = SYS_StarmapManager.Direct.CheckEntityPos(new Vector2(Random.Range(-300, 300), Random.Range(0, 850)));
			}
		}

		return genPos;
	}

	private Vector2 GenPos() {
		return new Vector2(Random.Range(-360, 360), Random.Range(0, 1280));
	}
}

public enum StarType {
	Check,
	End,
	Meteor
}
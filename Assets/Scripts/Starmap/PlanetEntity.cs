using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEntity : SpaceEntity {
	public GameObject halo;
	public Collider2D colli;
	public InteractEvent iEvent;
	public TextMesh text;
	public bool explored = false;

	// Start is called before the first frame update
	void Awake() {
		GetComponent<Collider2D>();

	}

	// Update is called once per frame
	void Start()    {

		iEvent = TMP_InteractEvent.GetPlanetEvent(info.sName);
	}

	public void Regist(StarInfo info, Material mat, float size, bool haveHalo) {
		Regist(info, mat, size);
		halo.SetActive(haveHalo);
		text.text = info.sName;
	}

	void OnTriggerEnter2D(Collider2D colli) {
		if (info.sType == StarType.Check) {
			SYS_Interactive.Direct.Regist(iEvent);
			explored = true;

		} else if (info.sType == StarType.End) {
			SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);

		}
	}

	void OnTriggerExit2D(Collider2D colli) {

	}
}

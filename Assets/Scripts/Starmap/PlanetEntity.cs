using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEntity : SpaceEntity {
	public GameObject halo;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Regist(StarInfo info, Material mat, float size, bool haveHalo) {
		Regist(info, mat, size);
		halo.SetActive(haveHalo);
	}
}

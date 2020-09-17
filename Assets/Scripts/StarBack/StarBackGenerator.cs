using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBackGenerator : MonoBehaviour
{
	public GameObject starPfb;

	// Start is called before the first frame update
	void Start()
    {
		for (int ct = 0; ct < Random.Range(50, 60); ct++) {
			GameObject objGen = Instantiate(starPfb);
			RectTransform objRect = objGen.GetComponent<RectTransform>();
			objGen.transform.SetParent(transform);
			objRect.anchoredPosition3D = new Vector2(Random.Range(-360, 360), Random.Range(-640, 640));

			float objSize = Random.Range(6, 18);
			objRect.sizeDelta = new Vector2(objSize, objSize);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}

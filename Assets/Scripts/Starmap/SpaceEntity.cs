using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEntity : MonoBehaviour
{
	public MeshRenderer meshRenderer;
	public StarInfo info;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Regist(StarInfo info, Material mat, float size) {
		this.info = info;
		meshRenderer.material = mat;
		transform.localScale = new Vector3(size, size, size);
	}
}

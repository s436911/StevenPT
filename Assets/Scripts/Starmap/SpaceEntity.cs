using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEntity : MonoBehaviour {
	public Rigidbody2D ridgid;
	public MeshRenderer meshRenderer;
	public StarInfo info;
	public bool explored = false;

	public virtual void Awake() {
		ridgid = GetComponent<Rigidbody2D>();
	}

	public virtual void Regist(StarInfo info, Material mat, float size) {
		this.info = info;
		meshRenderer.material = mat;
		transform.localScale = new Vector3(size, size, size);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEntity : MonoBehaviour {
	public Rigidbody2D ridgid;
	public MeshRenderer meshRenderer;
	public StarInfo info ;
	public bool explored = false;
	public float size;

	public virtual void Awake() {
		ridgid = GetComponent<Rigidbody2D>();
	}

	public virtual void Regist(StarInfo info, Material mat, float size) {
		this.info = info;
		this.size = size;
		meshRenderer.material = mat;
		transform.localScale = Vector3.one * size;
	}

	public virtual void Regist(StarInfo info, float size) {
		this.info = info;
		this.size = size;
		transform.localScale = Vector3.one * size;
	}
}

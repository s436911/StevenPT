using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_CameraController : MonoBehaviour
{
	public Transform objFollowed;
	public Vector2 followOffset;
	public float lerpTime = 1;
	
	// Update is called once per frame
	void Update() {
		Vector2 nowPos = transform.position;
		Vector2 followedPos = objFollowed.position;
		Vector2 offset = Vector2.Lerp(transform.position, objFollowed.position, lerpTime) + followOffset;

		transform.position = new Vector3(offset.x, offset.y, transform.position.z);
	}
}

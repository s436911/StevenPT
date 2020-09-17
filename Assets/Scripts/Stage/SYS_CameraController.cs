using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_CameraController : MonoBehaviour
{
	public Transform objFollowed;
	public Vector2 followOffset;
	public Vector2 followDistance;
	public float lerpTime = 1;


    void Start()
    {
        
    }

	// Update is called once per frame
	void Update() {
		Vector2 nowPos = transform.position;
		Vector2 followedPos = objFollowed.position;
		Vector2 followOffset = Vector2.Lerp(transform.position, objFollowed.position, lerpTime);

		transform.position = new Vector3(followOffset.x, followOffset.y, transform.position.z);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Camera : MonoBehaviour {
	public static SYS_Camera Direct;
	public Transform objFollowed;
	public Vector2 followOffset;
	public float followDis;
	public AnimationCurve speedCurve;
	public AnimationCurve shakeCurve;
	public float lerpTime = 1;
	public float lerpTimeZ = 0.25F;
	public float shakeValue = 0.05f;

	private float cameraShaking = 0; // 攝影機晃動時間
	private float shakeStart = 0;
	private float shakeTime = 0;

	void Awake() {
		Direct = this;
	}

	// Update is called once per frame
	void Update() {
		Vector2 offset = Vector2.Lerp(transform.position, (Vector2)objFollowed.position + followOffset, lerpTime);

		float tgtPosZ = (objFollowed.position.z + followDis) * GetZoomrate();
		float offsetZ = Mathf.Lerp(transform.position.z, tgtPosZ, lerpTimeZ);

		//簡易震動區
		if (shakeStart != 0) {
			if (shakeStart + shakeTime > Time.timeSinceLevelLoad) {
				offset = offset + new Vector2(Random.Range(-shakeValue, shakeValue), Random.Range(-shakeValue, shakeValue)) * shakeCurve.Evaluate((Time.timeSinceLevelLoad - shakeStart) / shakeTime);

			} else {
				shakeStart = 0;
				shakeTime = 0;
			}
		}

		transform.position = new Vector3(offset.x, offset.y, offsetZ);
	}

	public void Shake(float shakeTime) {
		shakeStart = Time.timeSinceLevelLoad;
		this.shakeTime = shakeTime;
	}

	public float GetZoomrate() {
		return speedCurve.Evaluate(SYS_ShipController.Direct.speed / 4);
	}
}

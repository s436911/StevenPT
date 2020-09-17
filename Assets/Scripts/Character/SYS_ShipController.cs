using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_ShipController : MonoBehaviour {
	public static SYS_ShipController Direct;
	public static bool reverse = false;

	public bool handling = false;
	public float speed = 0;
	public float maxSpeed = 2;
	public float accelerate = 2;
	public float smoothing = 0.5f;

	public Rigidbody2D ridgid;
	public Vector2 direction = Vector2.up;
	private Coroutine cououtine;
	private float fuelTimer;

	void Awake() {
		Direct = this;
		ridgid = GetComponent<Rigidbody2D>();
	}

	public void Reset() {
		transform.localPosition = Vector2.zero;
	}

	void Update() {
		if (direction != Vector2.zero) {
			Quaternion qua = Quaternion.LookRotation(direction, Vector3.forward);//※  將Vector3型別轉換四元數型別
			transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * smoothing);
		}

		if (handling) {
			if (speed < maxSpeed) {
				speed = speed + maxSpeed * Time.deltaTime;
				if (speed > maxSpeed) {
					speed = maxSpeed;
				}
			}

			if (Time.timeSinceLevelLoad - fuelTimer > 1) {
				fuelTimer = Time.timeSinceLevelLoad;
				SYS_ResourseManager.Direct.ModifyFuel(-1);
			}
		}
	}

	private IEnumerator Move() {

		while (true) {
			if (!reverse) {
				ridgid.velocity = this.direction * speed;
			} else {
				ridgid.velocity = this.direction * -speed;
			}
			yield return null;
		}
	}
		

	public void BeginMove() {
		SYS_SelfDriving.Direct.Reset();
		OnBeginMove();
	}

	public void OnBeginMove() {
		this.cououtine = StartCoroutine(this.Move());
		handling = true;
	}

	

	public void EndMove() {
		SYS_SelfDriving.Direct.ClearTGT ();
		OnEndMove();
	}

	public void OnEndMove() {
		ridgid.velocity = Vector2.zero;
		StopCoroutine(this.cououtine);
		this.speed = 0;
		handling = false;
	}



	public void OnUpdateDirection(Vector2 direction) {
		this.direction = direction.normalized;
	}

	public void UpdateDirection(Vector2 direction) {
		SYS_SelfDriving.Direct.Reset();
		OnUpdateDirection(direction);
	}

	public void UpdateDirection(Vector3 direction) {
		SYS_SelfDriving.Direct.Reset();
		OnUpdateDirection(direction);
	}
}

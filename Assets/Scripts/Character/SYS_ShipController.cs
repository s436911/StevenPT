using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_ShipController : MonoBehaviour {
	public static SYS_ShipController Direct;
	public static bool reverse = false;

	public bool handling = false;
	public float speed = 0;
	public float maxSpeed = 2;
	public float accelerate = 0.5f;
	public float decelerate = 1;
	public float smoothing = 0.5f;

	public GameObject character;
	public Rigidbody2D ridgid;
	public Vector2 direction = Vector2.up;
	public Vector2 force;

	public GameObject reflecter;
	public GameObject detector;
	public GameObject shield;
	public GameObject dash;
	public GameObject drill;

	private Coroutine cououtine;
	private float fuelTimer;
	private float foodTimer;
	private float damageTimer;
	private float shieldTimer;

	void Awake() {
		Direct = this;
		ridgid = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		Reset();
	}

	public void Restart() {
		character.SetActive(true);
		if (SYS_Mission.Direct.nowMission.missionType == MissionType.Collect) {
			reflecter.SetActive(true);
		}
	}

	public void Reset() {
		transform.localPosition = Vector2.zero;
		fuelTimer = 0;
		foodTimer = 0;
		character.SetActive(false);
		detector.SetActive(false);
		reflecter.SetActive(false);
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {
			if (direction != Vector2.zero) {
				Quaternion qua = Quaternion.LookRotation(direction, Vector3.forward);//※  將Vector3型別轉換四元數型別
				transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * smoothing);
			}

			if (handling) {
				if (speed < maxSpeed) {
					ModifySpeed(accelerate + SYS_SaveManager.Direct.GetMembersAttribute(1) * 0.1f);
				} else {
					ModifySpeed(decelerate);
				}

				if (Time.timeSinceLevelLoad - fuelTimer > 1) {
					fuelTimer = Time.timeSinceLevelLoad;
					SYS_ResourseManager.Direct.ModifyResource(0,-1);
				}
			}
			
			dash.SetActive(speed > 3);
			if (force != Vector2.zero) {
				force = force.normalized * Mathf.Clamp(force.magnitude - decelerate * Time.deltaTime, 0, 9999);
			}

			if (!reverse) {
				ridgid.velocity = this.direction * speed + force;
			} else {
				ridgid.velocity = this.direction * -speed + force;
			}

			if (Time.timeSinceLevelLoad - foodTimer > 10) {
				foodTimer = Time.timeSinceLevelLoad;
				SYS_ResourseManager.Direct.ModifyResource(2,-1);
			}
		}
	}
	public void ModifyForce(Vector2 value) {
		force = force + value;
	}
	public void ModifySpeed(float value) {
		speed = Mathf.Clamp( speed + value * Time.deltaTime , 0 , maxSpeed);
	}

	public float  GetMaxSpeed() {
		return maxSpeed + SYS_SaveManager.Direct.GetMembersAttribute(0) * 0.05f;
	}


	private IEnumerator Move() {

		while (true) {
			if (!reverse) {
				ridgid.velocity = this.direction * speed + force;
			} else {
				ridgid.velocity = this.direction * -speed + force;
			}

			//Debug.LogError(speed + "/" + force + "/" + ridgid.velocity);
			yield return null;
		}
	}
		

	public void BeginMove() {
		SYS_SelfDriving.Direct.Pause();
		OnBeginMove();
	}

	public void OnBeginMove() {
		//this.cououtine = StartCoroutine(this.Move());
		handling = true;
	}

	public void EndMove() {
		OnEndMove();
		SYS_SelfDriving.Direct.Play();
	}

	public void OnEndMove(bool stop = true) {
		ridgid.velocity = Vector2.zero;
		try{
			StopCoroutine(this.cououtine);
		} catch {
			Debug.LogWarning("有空再修");
		}

		if (stop && SYS_SelfDriving.Direct.tgt == null) {
			speed = 0;
			dash.SetActive(false);
		}
		
		handling = false;
	}

	public void OnUpdateDirection(Vector2 direction) {
		this.direction = direction.normalized;
	}

	public void UpdateDirection(Vector2 direction) {
		OnUpdateDirection(direction);
	}

	public void UpdateDirection(Vector3 direction) {
		OnUpdateDirection(direction);
	}

	public bool IsHighspeed() {
		return ridgid.velocity.magnitude >= 3;
	}

	public bool IsDamageAble() {
		return (Time.timeSinceLevelLoad - damageTimer) > 0.5f;
	}


	public void Damage(int damage) {
		damageTimer = Time.timeSinceLevelLoad;
		SYS_ResourseManager.Direct.ModifyResource(1, -damage);

		if (Random.Range(0, 100) < 50) {
			SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "好痛!");
		} else {
			SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "不能好好開船嗎!");
		}
	}

	public void Impact(float mass , Vector2 velocity) {
		force = (velocity * mass + force) / (mass + 1);
		speed = speed / (mass + 1);
	}
}

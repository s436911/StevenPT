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
	public float smoothing = 0.5f;

	public GameObject character;
	public Rigidbody2D ridgid;
	public Vector2 direction = Vector2.up;

	public GameObject reflecter;
	public GameObject detector;

	private Coroutine cououtine;
	private float fuelTimer;
	private float foodTimer;
	private float damageTimer;

	void Awake() {
		Direct = this;
		ridgid = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		Reset();
	}

	public void Init() {
		character.SetActive(true);
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
					speed = Mathf.Clamp(speed + accelerate * SYS_SaveManager.Direct.GetMembersAttribute(1) * 0.1f * Time.deltaTime, 0, maxSpeed);
				}

				if (Time.timeSinceLevelLoad - fuelTimer > 1) {
					fuelTimer = Time.timeSinceLevelLoad;
					SYS_ResourseManager.Direct.ModifyResource(0,-1);
				}
			}

			if (Time.timeSinceLevelLoad - foodTimer > 10) {
				foodTimer = Time.timeSinceLevelLoad;
				SYS_ResourseManager.Direct.ModifyResource(2,-1);
			}
		}
	}

	public float  GetMaxSpeed() {
		return maxSpeed + SYS_SaveManager.Direct.GetMembersAttribute(0) * 0.05f;
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
		SYS_SelfDriving.Direct.Pause();
		OnBeginMove();
	}

	public void OnBeginMove() {
		this.cououtine = StartCoroutine(this.Move());
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

	public bool DamageAble() {
		return (Time.timeSinceLevelLoad - damageTimer) > 0.5f;
	}

	public void Damage(int damage , int speedDown = 0) {
		damageTimer = Time.timeSinceLevelLoad;
		SYS_ResourseManager.Direct.ModifyResource(1, -damage);
		speed = Mathf.Clamp(speed - speedDown, 0, maxSpeed);

		if (Random.Range(0, 100) < 50) {
			SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "好痛!");
		} else {
			SYS_PopupManager.Direct.Regist(SYS_SaveManager.Direct.GetMember().name, "不能好好開船嗎!");
		}
	}
}

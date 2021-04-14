using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SYS_ShipController : MonoBehaviour {
	public static SYS_ShipController Direct;
	public static bool reverse = false;
	public RectTransform fakeSticker;

	public float speed = 0;
	public float maxSpeed = 2;
	public float accelerate = 0.5f;
	public float decelerate = 1;
	public float lerp = 0.5f;

	public Transform charactership;
	public GameObject character;
	public GameObject characterJet;
	public Rigidbody2D ridgid;
	public Vector2 force;

	public GameObject reflecter;
	public GameObject detector;
	public GameObject shield;
	public GameObject dash;
	public GameObject drill;

	public AnimationCurve iceSize;
	public AnimationCurve iceAlpha;
	public AnimationCurve iceAlphaMask;
	public GameObject icePanel;
	public Image iceImage;
	public Image iceMask;
	private float nowAngle = 0;
	private float iceValue;

	private Coroutine cououtine;
	private float fuelTimer;
	private float foodTimer;
	private float triggerTimer;
	private float damageTimer;
	private float shieldTimer;

	public float moveCounter = 0;

	public bool[] moveOrder = {false , false }; //0自動駕駛、1手動駕駛
	public Vector2 direction = Vector2.up;

	void Awake() {
		Direct = this;
		ridgid = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		Reset();
	}

	public void Restart() {
		character.SetActive(true);
		characterJet.SetActive(true);
	}

	public void ModifyIce(float value) {
		iceValue = Mathf.Clamp(iceValue + value, 0, 15);
		iceImage.gameObject.SetActive(iceValue > 0);
		iceMask.gameObject.SetActive(iceValue > 0);
		iceImage.color = new Color(iceImage.color.r, iceImage.color.g, iceImage.color.b, iceAlpha.Evaluate(iceValue));
		iceMask.color = new Color(iceMask.color.r, iceMask.color.g, iceMask.color.b, iceAlphaMask.Evaluate(iceValue));
		iceImage.transform.localScale = Vector2.one * iceSize.Evaluate(iceValue);
		
	}

	public void Reset() {
		transform.localPosition = Vector2.zero;
		fuelTimer = 0;
		foodTimer = 0;
		SetSpeed(0);
		force = Vector2.zero;
		character.SetActive(false);
		characterJet.SetActive(false);
		detector.SetActive(false);
		reflecter.SetActive(false);
		iceValue = 0;
	}

	public float whirl;
	public float whirlLerp;

	void FixedUpdate() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {

			if (direction != Vector2.zero) {
				float a = nowAngle;
				nowAngle = Mathf.LerpAngle(nowAngle, Common.GetEulerAngle(direction), lerp * Time.fixedDeltaTime);
				float b = nowAngle;

				transform.eulerAngles = new Vector3(0 , 0, nowAngle);
				charactership.localEulerAngles = new Vector3(charactership.localEulerAngles.x, Mathf.LerpAngle(charactership.localEulerAngles.y, (b - a) * whirl, whirlLerp * Time.fixedDeltaTime), charactership.localEulerAngles.z);


				ModifyMoveCounter(Time.fixedDeltaTime);
			} else {

				ModifyMoveCounter(-Time.fixedDeltaTime);
				charactership.localEulerAngles = new Vector3(charactership.localEulerAngles.x, Mathf.LerpAngle(charactership.localEulerAngles.y, 0, whirlLerp * Time.fixedDeltaTime), charactership.localEulerAngles.z);
			}

			if (SYS_Interactive.Direct.fuelTimer == 0) {
				if (moveOrder[1] || moveOrder[0]) {
					if (speed < GetMaxSpeed()) {
						ModifySpeed((accelerate * (1 + SYS_Save.Direct.GetMembersAttribute(0) * 0.01f)) * Time.fixedDeltaTime);
					} else {
						ModifySpeed(decelerate * Time.fixedDeltaTime);
					}

					if (Time.timeSinceLevelLoad - fuelTimer > 1) {
						fuelTimer = Time.timeSinceLevelLoad;
						SYS_ResourseManager.Direct.ModifyResource(0, -1, false);
					}
				}
			}

			dash.SetActive(speed > 3);
			if (force != Vector2.zero) {
				force = force.normalized * Mathf.Clamp(force.magnitude - (decelerate + speed) * Time.fixedDeltaTime, 0, 9999);
			}

			//ridgid.velocity = this.direction * speed * (!reverse ? 1 : -1) + force;
			ridgid.velocity = direction * speed * (!reverse ? 1 : -1) + force;

			if (iceValue >= 0) {
				ModifyIce(-Time.fixedDeltaTime);
			}
		}
	}

	public void ModifyMoveCounter(float value) {
		if ((value > 0 && moveCounter < 0) || (value < 0 && moveCounter > 0)) {
			moveCounter = 0;
		}

		if (moveCounter > 0) {
			if (moveCounter < 10 && moveCounter + value >= 10) {
				SYS_TeamManager.Direct.TriggerEvent(12);
			} 
		} else if(moveCounter < 0) {
			if (moveCounter > -10 && moveCounter + value <= -10) {
				SYS_TeamManager.Direct.TriggerEvent(13);
			} 
		}
		moveCounter = moveCounter + value;
	}

	void Update() {
		if (SYS_ModeSwitcher.Direct.gameMode == GameMode.Space) {

			if (Time.timeSinceLevelLoad - foodTimer > 10) {
				foodTimer = Time.timeSinceLevelLoad;
				SYS_ResourseManager.Direct.ModifyResource(2, -1 , false);
			}
		}
	}
	public void ModifyForce(Vector2 value) {
		force = force + value;
	}
	public void ModifySpeed(float value) {
		SetSpeed(speed + value);
	}

	public void SetSpeed(float value) {
		speed = Mathf.Clamp(value, 0, GetMaxSpeed());
	}

	public float GetMaxSpeed() {
		return maxSpeed * (1 + SYS_Save.Direct.GetMembersAttribute(0) * 0.01f) * Mathf.Clamp01((15 - iceValue) / 15);
	}

	public void BeginMove(int value) {
		moveOrder[value] = true;
	}

	public void EndMove(int value) {
		moveOrder[value] = false;

		if (!moveOrder[0] && !moveOrder[1]) {
			ridgid.velocity = Vector2.zero; //速度不變
			direction = Vector2.zero;
			SetSpeed(0);
			dash.SetActive(false);
		}
	}
	
	public void UpdateDirection(int value , Vector2 direction) {
		if (direction != Vector2.zero) {
			this.direction = direction.normalized;
		}
	}
	
	public bool IsDamageAble() {
		return (Time.timeSinceLevelLoad - damageTimer) > 0.5f;
	}


	public void Damage(int damage) {
		damageTimer = Time.timeSinceLevelLoad;
		SYS_ResourseManager.Direct.ModifyResource(1, -damage);
		if (Random.Range(0, 100) < 50) {
			SYS_TeamManager.Direct.Talk(3, "不能好好開船嗎!");
		} else {
			SYS_TeamManager.Direct.Talk(3, "好痛!");
		}
	}

	public void Impact(float mass , Vector2 velocity , float reflect) {
		//	force = (velocity * mass + ridgid.velocity * 1) / (mass + 1) ; 
		force = ridgid.velocity * -reflect;
	}

	public void Shock(float value) {
		if (Random.Range(0, 100) < 50) {
			if (!SYS_TeamManager.Direct.TriggerEvent(45)) {
				SYS_TeamManager.Direct.ModifyMorale(-1);

				if (Random.Range(0, 100) < 50) {
					SYS_TeamManager.Direct.Talk(3, "嗚嗚頭好暈...");
				} else {
					SYS_TeamManager.Direct.Talk(3, "哇!");
				}
			}

		} else if(SYS_TeamManager.Direct.TriggerEvent(44)) {
			SYS_TeamManager.Direct.ModifyMorale(-1);
		}
		ModifySpeed(-value);
	}

	public bool TriggerAble() {
		return true;
		//return Time.timeSinceLevelLoad - triggerTimer > 3;
	}

	public void Trigger() {
		triggerTimer = Time.timeSinceLevelLoad;
	}
}

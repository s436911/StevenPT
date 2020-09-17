using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_StickHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

	[System.Serializable]
	public class VirtualJoystickEvent : UnityEvent<Vector3> {
	}

	public Transform content;

	public UnityEvent beginControl;
	public VirtualJoystickEvent controlling;
	public UnityEvent endControl;
	public bool sceneControl;
	public bool scrollRectControl;

	private Vector2 posDown;
	private Vector2 direction;
	private Vector2 directionAbs;

	public void OnBeginDrag(PointerEventData eventData) {
		try {
			this.beginControl.Invoke();
		} catch {
			this.endControl.Invoke();
		}
	}

	public void OnDrag(PointerEventData eventData) {
		try {
			if (this.content) {

				this.controlling.Invoke(this.content.localPosition.normalized);
			}
		} catch {
			this.endControl.Invoke();
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		this.endControl.Invoke();
	}

	void Update() {
		try {
			if (sceneControl) {
				if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > Screen.height * 0.22f) {
					posDown = Input.mousePosition;
					this.beginControl.Invoke();
				} else if (Input.GetKeyUp(KeyCode.Mouse0)) {
					if (posDown != Vector2.zero) {
						posDown = Vector2.zero;
						this.endControl.Invoke();
					}
				}
			}

			if (posDown != Vector2.zero) {
				if (this.content) {

					direction = (Vector2)Input.mousePosition - posDown;
					directionAbs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

					if (directionAbs.x > directionAbs.y) {
						direction /= directionAbs.x;
					} else if (directionAbs.x < directionAbs.y) {
						direction /= directionAbs.y;
					} else {
						direction = Vector2.zero;
					}

					this.controlling.Invoke(direction);
				}
			}
		} catch {
			this.endControl.Invoke();
		}
	}
}

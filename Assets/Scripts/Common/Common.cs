using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common : MonoBehaviour {
	public static Common Direct;
	public AnimationCurve magnetCurve;

	void Awake() {
		Direct = this;
	}

	public static float GetEulerAngle(Vector2 direction) {
		if (direction.x < 0) {
			return 360 - (Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
		}
	}

	public int RandomNum(int min, int max , List<int> minus) {
		List<int> tmp = new List<int>();
		for (int i = min; i < max; i++) {
			if (!minus.Contains(i)) {
				tmp.Add(i);
			}
		}

		return tmp[Random.Range(0, tmp.Count)];
	}
}

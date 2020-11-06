using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common {
	public static float GetEulerAngle(Vector2 direction) {
		if (direction.x < 0) {
			return 360 - (Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
		}
	}
}

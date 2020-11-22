using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Home : MonoBehaviour, IBeginDragHandler , IEndDragHandler {
	public ScrollRect scrollRect;
	public RectTransform contenter;
	public UI_Player player;
	public bool draging = false;
	public bool onPos = true;

	public float controlSpeed = 360;
	public float lerpTime = 1;

    // Update is called once per frame
    void Update()
    {
		float nowSpeed = Mathf.Abs(scrollRect.velocity.y);
		if (!draging && ((nowSpeed < controlSpeed && nowSpeed > 0)|| (nowSpeed == 0 && !onPos)) ) {

			if (contenter.anchoredPosition.y > -1080 && contenter.anchoredPosition.y < -360) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, -720), 0.5f);
				if (contenter.anchoredPosition.y < -720) {
					contenter.anchoredPosition = new Vector2(0, -720);
					scrollRect.velocity = Vector2.zero;
					onPos = true;
					SYS_Logger.Direct.SetSystemMsg("選擇任務後點Play");
				}


			} else if (contenter.anchoredPosition.y > 360 && contenter.anchoredPosition.y < 1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, 720), lerpTime);
				if (contenter.anchoredPosition.y > 720) {
					contenter.anchoredPosition = new Vector2(0, 720);
					scrollRect.velocity = Vector2.zero;
					onPos = true;
					SYS_Logger.Direct.SetSystemMsg("花費資源可升級飛船或太空站");
				}

			} else if (contenter.anchoredPosition.y > 1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, 1440), lerpTime);
				if (contenter.anchoredPosition.y > 1440) {
					contenter.anchoredPosition = new Vector2(0, 1440);
					scrollRect.velocity = Vector2.zero;
					onPos = true;
					SYS_Logger.Direct.SetSystemMsg("點擊push扭出全新駕駛員");
				}

			} else if (contenter.anchoredPosition.y < -1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, -1440), lerpTime);
				if (contenter.anchoredPosition.y < -1440) {
					contenter.anchoredPosition = new Vector2(0, -1440);
					scrollRect.velocity = Vector2.zero;
					onPos = true;
					SYS_Logger.Direct.SetSystemMsg("可自訂船隻和船員");
				}

			} else {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, Vector2.zero, lerpTime);
				if (Mathf.Abs(scrollRect.velocity.y) < 252) {
					contenter.anchoredPosition = new Vector2(0, 0);
					scrollRect.velocity = Vector2.zero;
					onPos = true;
					SYS_Logger.Direct.SetSystemMsg("向右滑動至觀測站出任務\n向左滑動至太空站強化設備");
				}

			}
		} 

		if (Mathf.Abs(scrollRect.velocity.y) <= 0) {
			player.Stand();
		} else {
			player.Move();
		}
	}

	public void OnBeginDrag(PointerEventData data) {
		draging = true;
		onPos = false;
	}

	public void OnEndDrag(PointerEventData data) {
		draging = false;
	}

	public void ToPage(int page) {
		contenter.anchoredPosition = new Vector2(0, page * 720);
		scrollRect.velocity = Vector2.zero;
		onPos = true;
	}
}

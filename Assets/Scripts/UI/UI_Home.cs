﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Home : MonoBehaviour, IBeginDragHandler , IEndDragHandler {
	public ScrollRect scrollRect;
	public RectTransform contenter;
	public UI_Player player;
	public bool draging = false;

    // Start is called before the first frame update
    void Start() {

	}

    // Update is called once per frame
    void Update()
    {

		if (!draging && Mathf.Abs(scrollRect.velocity.y) < 360 && Mathf.Abs(scrollRect.velocity.y) > 0) {

			if (contenter.anchoredPosition.y > -1080 && contenter.anchoredPosition.y < -360) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, -720), 0.5f);
				if (contenter.anchoredPosition.y < -720) {
					contenter.anchoredPosition = new Vector2(0, -720);
					scrollRect.velocity = Vector2.zero;
					SYS_Logger.Direct.SetSystemMsg("設定完導航路徑後點擊望遠鏡出發");
				}


			} else if (contenter.anchoredPosition.y > 360 && contenter.anchoredPosition.y < 1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, 720), 0.5f);
				if (contenter.anchoredPosition.y > 720) {
					contenter.anchoredPosition = new Vector2(0, 720);
					scrollRect.velocity = Vector2.zero;
					SYS_Logger.Direct.SetSystemMsg("可自訂船隻和船員");
				}

			} else if (contenter.anchoredPosition.y > 1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, 1440), 0.5f);
				if (contenter.anchoredPosition.y > 1440) {
					contenter.anchoredPosition = new Vector2(0, 1440);
					scrollRect.velocity = Vector2.zero;
					SYS_Logger.Direct.SetSystemMsg("花費資源可升級飛船或太空站");
				}

			} else if (contenter.anchoredPosition.y < -1080) {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, new Vector2(0, -1440), 0.5f);
				if (contenter.anchoredPosition.y < -1440) {
					contenter.anchoredPosition = new Vector2(0, -1440);
					scrollRect.velocity = Vector2.zero;
					SYS_Logger.Direct.SetSystemMsg("施工中");
				}

			} else {
				contenter.anchoredPosition = Vector2.Lerp(contenter.anchoredPosition, Vector2.zero, 1);
				if (Mathf.Abs(scrollRect.velocity.y) < 252) {
					contenter.anchoredPosition = new Vector2(0, 0);
					scrollRect.velocity = Vector2.zero;
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
	}

	public void OnEndDrag(PointerEventData data) {
		draging = false;
	}
}

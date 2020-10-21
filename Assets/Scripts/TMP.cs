using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TMP_InteractEvent {
	public static InteractEvent GetPlanetEvent(string sName, int planetType = 0) {
		List<InteractOption> tempAnswers = new List<InteractOption>();
		string tmpMsg = "";
		int rand = planetType == 0 ? Random.Range(1, 7) : planetType;

		switch (rand) {
			case 1:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 30, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這個星球有一間很大的加油站可以用呢!!";
				break;

			case 2:
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 2, 1, 0, 30, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 1, 1, 0, 60, "交戰"));
				tmpMsg = "加油站被野生動物佔據了我們只能偷偷加油，但若擊退他們我們就可以加免費燃料了!!";
				break;

			case 3:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 30, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Explore, 50, 2, 1, 0, 60, "探索"));
				tmpMsg = "加油站旁有座巨大礦坑也許可以找到更多燃料!?";
				break;

			case 4:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 30, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 3, 1, 1, 1, "強化"));
				tmpMsg = "這裡的星球武器商可以補給燃料和強化裝甲!!";
				break;

			case 5:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 2, 1, 0, 30, "加油"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這顆星球的加油站竟然只收食物!!";
				break;

			case 6:
				tempAnswers.Add(new InteractOption(Affinity.Explore, 100, 2, 1, 0, 30, "探索"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這個加油站已經廢棄很久了，但是也許可以從廢墟中找到一些燃料!!";
				break;
		}

		return new InteractEvent(sName, tmpMsg, tempAnswers);
	}

	public static InteractEvent GetActivityEvent(string sName, int activityType = 0) {
		List<InteractOption> tempAnswers = new List<InteractOption>();
		string tmpMsg = "";
		int rand = activityType == 0 ? Random.Range(1, 5) : activityType;

		switch (rand) {
			case 1:
				if (Random.Range(0, 2) == 0) {
					tempAnswers.Add(new InteractOption(Affinity.Explore, 100, 2, 2, 0, 0, "探索", Random.Range(0, 2) == 0 ? DB.GetItem(80001): DB.GetItem(80002)));
				} else {
					tempAnswers.Add(new InteractOption(Affinity.Explore, 50, 2, 1, 3, 2, "探索"));
				}

				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tmpMsg = "這裡有一艘被廢棄的太空船，也許可以從中找到一些東西!!!!";
				break;

			case 2:
				tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 2, 1, 3, 1, "交易"));
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				if (Random.Range(0, 2) == 0) {
					tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 2, 0, 0, "交易", Random.Range(0, 2) == 0 ? DB.GetItem(80001) : DB.GetItem(80003)));
				} else {
					tempAnswers.Add(new InteractOption(Affinity.Trade, 100, 3, 1, 0, 30, "交易"));
				}
				tmpMsg = "有艘商船想和我們進行交易，也許可以看看!!";
				break;

			case 3:
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));
				tempAnswers.Add(new InteractOption(Affinity.Explore, 50, 2, 2, 0, 0, "探索", DB.NewItem(80001, 0, (int)(SYS_Mission.Direct.nowMission.difficult * 0.5f))));
				tmpMsg = "是一個太空膠囊，等等裡面好像有個人影!?";
				break;

			case 4:
				tempAnswers.Add(new InteractOption(Affinity.None, 100, 0, 0, 0, 0, "離開"));

				if (Random.Range(0, 2) == 0) {
					tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 1, 1, 3, 2, "掠奪"));
				} else {
					tempAnswers.Add(new InteractOption(Affinity.Fight, 100, 1, 1, 0, 0, "掠奪", DB.GetItem(80005)));
				}

				tmpMsg = "是一架採集機器人，背後背著大量的礦物!!";
				break;
		}

		return new InteractEvent(sName, tmpMsg, tempAnswers);
	}
}

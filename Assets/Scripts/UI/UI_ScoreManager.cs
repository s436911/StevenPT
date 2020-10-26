using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreManager : MonoBehaviour
{
	public static UI_ScoreManager Direct;
	public GameObject uiPanel;

	public Text[] leftText = new Text[4];
	public Text[] rateText = new Text[4];
	public Text[] diffText = new Text[4];
	public Text[] getText = new Text[4];
	public UI_ItemSlot[] cargo = new UI_ItemSlot[2];

	public int[] getRate = new int[4];

	public Text fomulaText;
	public Text ScoreText;

	public float[] bonusEnd = new float[4];

	void Awake() {
		Direct = this;
		Reset();
	}
	
	public void Victory() {
		int difficult = SYS_Mission.Direct.nowMission.difficult;

		SYS_SelfDriving.Direct.Reset();
		SYS_GameEngine.Direct.SetPause(true);
		uiPanel.SetActive(true);

		fomulaText.text = Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * difficult))) + "+" +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * difficult))) + "+" +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * difficult)));

		int score = Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * difficult)));

		int maxScore = Mathf.RoundToInt(SYS_ResourseManager.Direct.maxFuel * getRate[0] * (1 + (0.2f * difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.maxFood * getRate[1] * (1 + (0.2f * difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.maxMineral * getRate[2] * (1 + (0.2f * difficult)));

		int getCoin = 1;

		if (score > maxScore * 0.6f) {
			ScoreText.text = "s";
			getCoin = 4;

		} else if (score > maxScore * 0.4f) {
			ScoreText.text = "a";
			getCoin = 3;

		} else if (score > maxScore * 0.2f) {
			ScoreText.text = "b";
			getCoin = 2;

		} else {
			ScoreText.text = "c";
		}

		leftText[0].text = SYS_ResourseManager.Direct.GetResource(0).ToString();
		leftText[1].text = SYS_ResourseManager.Direct.GetResource(2).ToString();
		leftText[2].text = SYS_ResourseManager.Direct.GetResource(3).ToString();
		leftText[3].text = getCoin.ToString();

		rateText[0].text = "x" + getRate[0].ToString();
		rateText[1].text = "x" + getRate[1].ToString();
		rateText[2].text = "x" + getRate[2].ToString();
		rateText[3].text = "x" + getRate[3].ToString();

		diffText[0].text = "x" + (1 + (0.2f * difficult)).ToString("f1") + (bonusEnd[0] > 1 ? " x" + bonusEnd[0].ToString("f1") : "");
		diffText[1].text = "x" + (1 + (0.2f * difficult)).ToString("f1") + (bonusEnd[1] > 1 ? " x" + bonusEnd[1].ToString("f1") : "");
		diffText[2].text = "x" + (1 + (0.2f * difficult)).ToString("f1") + (bonusEnd[2] > 1 ? " x" + bonusEnd[2].ToString("f1") : "");
		diffText[3].text = "x" + (1 + (0.2f * difficult)).ToString("f1") + (bonusEnd[3] > 1 ? " x" + bonusEnd[3].ToString("f1") : "");

		getText[0].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * difficult)) * (bonusEnd[0] > 1 ? bonusEnd[0] : 1)).ToString();
		getText[1].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * difficult)) * (bonusEnd[1] > 1 ? bonusEnd[1] : 1)).ToString();
		getText[2].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * difficult)) * (bonusEnd[2] > 1 ? bonusEnd[2] : 1)).ToString();
		getText[3].text = "+" + Mathf.RoundToInt(getCoin                                   * getRate[3] * (1 + (0.2f * difficult)) * (bonusEnd[3] > 1 ? bonusEnd[3] : 1)).ToString();

		SYS_Save.Direct.ModifyResource(0, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * difficult)) * (bonusEnd[0] > 1 ? bonusEnd[0] : 1)));
		SYS_Save.Direct.ModifyResource(2, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * difficult)) * (bonusEnd[1] > 1 ? bonusEnd[1] : 1)));
		SYS_Save.Direct.ModifyResource(3, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * difficult)) * (bonusEnd[2] > 1 ? bonusEnd[2] : 1)));
		SYS_Save.Direct.ModifyResource(4, Mathf.RoundToInt(getCoin                                   * getRate[3] * (1 + (0.2f * difficult)) * (bonusEnd[3] > 1 ? bonusEnd[3] : 1)));
		
		//顯示結算
		cargo[0].SetItem(SYS_ResourseManager.Direct.cargos[0]);
		cargo[1].SetItem(SYS_ResourseManager.Direct.cargos[1]);

		//加進道具攔
		SYS_Save.Direct.AddInventory(SYS_ResourseManager.Direct.cargos[0]);
		SYS_Save.Direct.AddInventory(SYS_ResourseManager.Direct.cargos[1]);

		//清除副本道具
		SYS_ResourseManager.Direct.SetCargo(0);
		SYS_ResourseManager.Direct.SetCargo(1);
		
		//清除攜帶副本道具
		SYS_Save.Direct.SetPrecargo(0);
		SYS_Save.Direct.SetPrecargo(1);

		SYS_Gacha.Direct.Reset();
	}

	public void Confirm() {
		SYS_GameEngine.Direct.SetPause(false);
		uiPanel.SetActive(false);
		SYS_ModeSwitcher.Direct.SetMode(GameMode.Home);
	}

	public void Reset() {
		bonusEnd[0] = 1;
		bonusEnd[1] = 1;
		bonusEnd[2] = 1;
		bonusEnd[3] = 1;
	}
}

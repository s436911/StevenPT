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
		SYS_SelfDriving.Direct.Reset();
		SYS_GameEngine.Direct.SetPause(true);
		uiPanel.SetActive(true);

		fomulaText.text = Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) + "+" +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) + "+" +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)));

		int score = Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)));

		int maxScore = Mathf.RoundToInt(SYS_ResourseManager.Direct.maxFuel * getRate[0] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.maxFood * getRate[1] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult))) +
			Mathf.RoundToInt(SYS_ResourseManager.Direct.maxMineral * getRate[2] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)));

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

		diffText[0].text = "x" + (1 + (0.2f * SYS_StarmapManager.Direct.difficult)).ToString("f1") + (bonusEnd[0] > 1 ? " x" + bonusEnd[0].ToString("f1") : "");
		diffText[1].text = "x" + (1 + (0.2f * SYS_StarmapManager.Direct.difficult)).ToString("f1") + (bonusEnd[1] > 1 ? " x" + bonusEnd[1].ToString("f1") : "");
		diffText[2].text = "x" + (1 + (0.2f * SYS_StarmapManager.Direct.difficult)).ToString("f1") + (bonusEnd[2] > 1 ? " x" + bonusEnd[2].ToString("f1") : "");
		diffText[3].text = "x" + (1 + (0.2f * SYS_StarmapManager.Direct.difficult)).ToString("f1") + (bonusEnd[3] > 1 ? " x" + bonusEnd[3].ToString("f1") : "");

		getText[0].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[0] > 1 ? bonusEnd[0] : 1)).ToString();
		getText[1].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[1] > 1 ? bonusEnd[1] : 1)).ToString();
		getText[2].text = "+" + Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[2] > 1 ? bonusEnd[2] : 1)).ToString();
		getText[3].text = "+" + Mathf.RoundToInt(getCoin                                   * getRate[3] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[3] > 1 ? bonusEnd[3] : 1)).ToString();

		SYS_ResourseManager.Direct.ModifyResourceHome(0, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(0) * getRate[0] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[0] > 1 ? bonusEnd[0] : 1)));
		SYS_ResourseManager.Direct.ModifyResourceHome(2, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(2) * getRate[1] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[1] > 1 ? bonusEnd[1] : 1)));
		SYS_ResourseManager.Direct.ModifyResourceHome(3, Mathf.RoundToInt(SYS_ResourseManager.Direct.GetResource(3) * getRate[2] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[2] > 1 ? bonusEnd[2] : 1)));
		SYS_ResourseManager.Direct.ModifyResourceHome(4, Mathf.RoundToInt(getCoin                                   * getRate[3] * (1 + (0.2f * SYS_StarmapManager.Direct.difficult)) * (bonusEnd[3] > 1 ? bonusEnd[3] : 1)));

		cargo[0].SetItem(SYS_ResourseManager.Direct.cargo[0].item);
		cargo[1].SetItem(SYS_ResourseManager.Direct.cargo[1].item);

		SYS_ResourseManager.Direct.AddInventory(SYS_ResourseManager.Direct.cargo[0].item);
		SYS_ResourseManager.Direct.AddInventory(SYS_ResourseManager.Direct.cargo[1].item);

		SYS_ResourseManager.Direct.SetCargoSlot(0);
		SYS_ResourseManager.Direct.SetCargoSlot(1);
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

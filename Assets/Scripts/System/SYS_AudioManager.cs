using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SYS_AudioManager : MonoBehaviour {
	public static SYS_AudioManager Direct;

	public int nowPass = 0;
	public AudioSource[] passes = new AudioSource[2];
	public AudioClip passNext;
	public AudioClip nowClip;

	public AudioMixer passMixer;

	public AudioClip homeBGM;
	public AudioClip launchBGM;
	public AudioClip endBGM;

	public AudioClip[] affinityBGM = new AudioClip[3];
	public AudioClip[] spaceBGM;

	public float muteSpeed = 60;
	public float unmuteSpeed = 60;

	void Awake() {
		Direct = this;
	}

	void Update() {
		if (passNext != null) {//有要播放的BGM
			string passStr = nowPass == 0 ? "main" : "sub";
			string otherPassStr = nowPass == 0 ? "sub" : "main";
			int nextPass = nowPass == 0 ? 1 : 0;

			float passVolume = 0;
			float otherPassVolume = 0;

			passMixer.GetFloat(passStr, out passVolume);
			passMixer.GetFloat(otherPassStr, out otherPassVolume);

			if (passVolume > -80) {//還沒靜音
				passMixer.SetFloat(passStr, Mathf.Clamp(passVolume - muteSpeed * Time.deltaTime, -80, 0));

			}

			if (passVolume < 0) {//還沒靜音
				passMixer.SetFloat(otherPassStr, Mathf.Clamp(otherPassVolume + unmuteSpeed * Time.deltaTime, -80, 0));

			}

			if (passVolume <= -80 && otherPassVolume >= 0) {				
				passNext = null;
				nowPass = nextPass;
			}
		}
	}

	private void SetNext(AudioClip next) {
		if (nowClip != next) {
			int nextPass = nowPass == 0 ? 1 : 0;
			passNext = next;
			nowClip = passNext;
			passes[nextPass].clip = passNext;
			passes[nextPass].Play();
		}
	}

	public void Play(BGMType playType) {
		switch (playType) {
			case BGMType.Home:
				SetNext(homeBGM);
				break;

			case BGMType.Launch:
				SetNext(launchBGM);
				break;

			case BGMType.Space:
				SetNext(spaceBGM[0]);
				break;

			case BGMType.End:
				SetNext(endBGM);
				break;

			case BGMType.Fight:
				SetNext(affinityBGM[0]);
				break;

			case BGMType.Trade:
				SetNext(affinityBGM[1]);
				break;

			case BGMType.Explore:
				SetNext(affinityBGM[2]);
				break;
		}
	}
}

public enum BGMType {
	Home,
	Launch,
	Space,
	End,
	Fight,
	Trade,
	Explore
}

using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private float volume = 0;
	private int currentMusicIndex;
	private int targetMusicIndex;

	public float fadeSpeed = 0.01f;

	public AudioClip[] audioClips;

	void Update () {
		var currentClip = this.gameObject.GetComponent<AudioSource>().clip;
		if (currentClip == null) { return; }
		if (currentMusicIndex != targetMusicIndex) {
			if (volume <= 0) {
				if (targetMusicIndex != -1) {
					currentMusicIndex = targetMusicIndex;
					setTrack(targetMusicIndex);
					volume = 1;
				}
			} else {
				volume -= fadeSpeed;
			}
		} else {
			if (volume < 1) {
				volume += fadeSpeed;
			} else {
				volume = 1;
			}
		}
		this.gameObject.GetComponent<AudioSource>().volume = volume;
	}

	public void switchMusicByIndex (int index) {
		Debug.Log("Switching music to track number " + index);
		targetMusicIndex = index;
		var currentClip = this.gameObject.GetComponent<AudioSource>().clip;
		if (currentClip == null) {
			currentMusicIndex = index;
			volume = 1;
			setTrack(index);
		}
	}

	public void switchMusicByName (string name) {
		var c = 0;
		foreach (var ac in audioClips) {
			if (ac.name == name) {
				switchMusicByIndex(c);
				return;
			}
			c += 1;
		}
	}

	private void setTrack(int index) {
		var al = this.gameObject.GetComponent<AudioSource>();
		al.clip = audioClips[index];
		al.Play ();
	}

	public void stopMusic () {
		this.gameObject.GetComponent<AudioSource>().Stop ();
	}

	public void fadeOut () {
		targetMusicIndex = -1;
	}
}

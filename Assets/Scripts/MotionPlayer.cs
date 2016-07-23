﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HandController))]
public class MotionPlayer : MonoBehaviour {

	private GameObject srcObject;
	public int numPoints;

	private List<Vector3> objectPositions;
	private List<Quaternion> objectRotations;
	private List<bool> triggerDowns;
	private List<bool> triggerUps;
	private HandController handController;

	private int max = 0;
	private int cursor = 0;
	private bool isRecording = false;
	private bool isPlayback = true;
	private bool lastTriggerDown = false;
	private bool lastTriggerUp = false;

	// Use this for initialization
	void Start() {
		handController = GetComponent<HandController>();

		objectPositions = new List<Vector3>(numPoints);
		objectRotations = new List<Quaternion>(numPoints);
		triggerDowns = new List<bool>(numPoints);
		triggerUps = new List<bool>(numPoints);

		cursor = 0;
		max = 0;
		isRecording = false;
		isPlayback = true;
		lastTriggerDown = false;
		lastTriggerUp = false;
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (IsRecording()) {
			RecordMotion();
		} else {
			PlaybackMotion();
		}
	}

	void RecordMotion() {
		if (srcObject == null) {
			Debug.Log("src Object doesn't exist!");
			return;
		}
		objectPositions.Add(srcObject.transform.position);
		objectRotations.Add(srcObject.transform.rotation);
		triggerDowns.Add(lastTriggerDown);
		triggerUps.Add(lastTriggerUp);

		max += 1;
		lastTriggerDown = false;
		lastTriggerUp = false;
	}

	public void RecordTriggerDown() {
		lastTriggerDown = true;
	}

	public void RecordTriggerUp() {
		lastTriggerUp = true;
	}

	void PlaybackMotion() {
		if (cursor < max && cursor < max) {
			transform.position = objectPositions[cursor];
			transform.rotation = objectRotations[cursor];

			if (handController != null && triggerDowns[cursor] == true) {
				Debug.Log("playback trigger down");
				handController.pickUp();
			}

			if (handController != null && triggerUps[cursor] == true) {
				Debug.Log("playback trigger up");
				handController.letGo();
			}

			cursor += 1;
		}
	}

	public void Record(GameObject src) {
		Debug.Log("Now Record");
		srcObject = src;
		isRecording = true;
		isPlayback = false;
		objectPositions.Clear();
		objectRotations.Clear();
		triggerDowns.Clear();
		triggerUps.Clear();
		cursor = 0;
		max = 0;
	}

	public void Playback() {
		Debug.Log("Now Playback");
		isRecording = false;
		isPlayback = true;
		cursor = 0;
	}

	public bool IsRecording() {
		return isRecording;
	}

}

﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public float startBuffer;
	private PlayerController playerController;

	void Start () {
		playerController = (PlayerController)GameObject.Find ("Player").GetComponent (typeof(PlayerController));
	}

	public float gameTime() {
		return Time.time - startBuffer;
	}

	// Update is called once per frame
	void Update () {
		GameObject.Find("Score").GetComponent<ScoreController>().getNote(gameTime());
		GameObject.Find("SwipeDebug").guiText.text = gameTime().ToString();
		if (!playerController.isAlive ()) {
			loseLevel();
		}
	}

	public void HandleInput(NoteType direction) {
		GameObject.Find("Score").GetComponent<ScoreController>().processNote(gameTime(), direction);
	}

	public void titleScreen() {
		Application.LoadLevel("TitleScreen");
	}

	public void loseLevel() {
		Application.LoadLevel("LoseLevel");
	}

	public void winLevel() {
		Application.LoadLevel("WinLevel");
	}
}

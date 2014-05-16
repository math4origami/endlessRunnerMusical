using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int lives;
	private int originalNumLives;
	private int currentNumLives;
	public int CurrentNumLives {
		get {return currentNumLives;}
	}
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;

	// Use this for initialization
	void Start () {
		originalNumLives = lives;
		resetPlayer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void resetPlayer() {
		currentNumLives = originalNumLives;

		updateHearts();
	}

	private void updateHearts() {
		heart1.guiTexture.enabled = false;
		heart2.guiTexture.enabled = false;
		heart3.guiTexture.enabled = false;
		for (int i=0; i<currentNumLives; ++i) {
			switch (i) {
				case 0: {
					heart1.guiTexture.enabled = true;
					break;
				}
				case 1: {
					heart2.guiTexture.enabled = true;
					break;
				}
				case 2: {
					heart3.guiTexture.enabled = true;
					break;
				}
			}
		}
	}

	public void deductLife() {
		--currentNumLives;

		if (currentNumLives <= 0) {
			currentNumLives = 0;
		}

		updateHearts();
	}

	public bool isAlive() {
		return (currentNumLives > 0);
	}
}

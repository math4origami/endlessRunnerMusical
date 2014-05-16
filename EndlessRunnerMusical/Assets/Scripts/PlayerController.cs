using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int lives;
	private int originalNumLives;
	private int currentNumLives;
	public int CurrentNumLives {
		get {return currentNumLives;}
	}
	static public int score;
	static public int resultScore;
	public int ResultScore {
		get { return resultScore;}
	}
	static public int highScore = 0;
	static public int lastHighScore = 0;
	public int LastHighScore {
		get { return lastHighScore;}
	}
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public GameObject scoreText;
	public GameObject resultScoreText;
	public GameObject highScoreText;
	public GameObject lastHighScoreText;
	

	// Use this for initialization
	void Start () {
		originalNumLives = lives;
		resetPlayer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void resetPlayer() {
		lastHighScore = highScore;
		if (score > highScore) {
			highScore = score;
		}

		currentNumLives = originalNumLives;
		score = 0;
		updateScoreText ();

		updateHearts();
	}

	private void updateHearts() {
		if (!heart1 && !heart2 && !heart3) {
			return;
		}
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

	private void updateScoreText() {
		if (scoreText) {
			scoreText.guiText.text = score.ToString ();
		}

		if (highScoreText) {
			highScoreText.guiText.text = highScore.ToString ();
		}

		if (resultScoreText) {
			resultScoreText.guiText.text = resultScore.ToString ();
		}

		if (lastHighScoreText) {
			lastHighScoreText.guiText.text = lastHighScore.ToString ();
		}
	}

	public void deductLife() {
		--currentNumLives;

		if (currentNumLives <= 0) {
			currentNumLives = 0;
			resultScore = score;
		}

		updateHearts();
		GetComponent<PlayerBehaviorScript>().HandleHurt();
	}
	
	public void updateScore(int delta) {
		score += delta;

		updateScoreText ();
	}

	public bool isAlive() {
		return (currentNumLives > 0);
	}
}

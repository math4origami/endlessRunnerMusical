using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int lives;
	private int originalNumLives;
	private int currentNumLives;

	// Use this for initialization
	void Start () {
		originalNumLives = lives;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void resetPlayer() {
		currentNumLives = originalNumLives;
	}

	public void deductLife() {
		--currentNumLives;

		if (currentNumLives <= 0) {
			currentNumLives = 0;
		}
	}

	public bool isAlive() {
		return (currentNumLives > 0);
	}
}

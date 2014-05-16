using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public float startBuffer;

	private float lastSwipe = -10.0f;

	void Start () {

	}

	public float gameTime() {
		return Time.time - startBuffer;
	}

	// Update is called once per frame
	void Update () {
		float gameTime = this.gameTime();
		if (gameTime > lastSwipe) {
			lastSwipe = gameTime + Random.Range(0.5f, 1.0f);

			ScoreController score = GameObject.Find("Score").GetComponent<ScoreController>();
			score.processNote(gameTime);
		}
	}
}

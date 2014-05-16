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
		GameObject.Find("Score").GetComponent<ScoreController>().getNote(gameTime());
		GameObject.Find("SwipeDebug").guiText.text = gameTime().ToString();
	}

	public void HandleInput(NoteType direction) {
		GameObject.Find("Score").GetComponent<ScoreController>().processNote(gameTime(), direction);
	}
}

using UnityEngine;
using System.Collections;

public class NoteView : MonoBehaviour {

	private ScoreNote scoreNote = null;

	public GameObject left_idle;
	public GameObject left_death;
	public GameObject right_idle;
	public GameObject right_death;
	public GameObject up_idle;
	public GameObject up_death;
	public GameObject down_idle;
	public GameObject down_death;
	public GameObject tap_idle;
	public GameObject tap_death;

	public void initWithScoreNote(ScoreNote note) {
		scoreNote = note;
	}

	// Use this for initialization
	void Start () {
		Update();
	}

	public static float calcX(ScoreNote scoreNote) {
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		float gameTime = gc.gameTime();

		return (scoreNote.scriptNote.beatInSeconds() - gameTime) * 
			GameObject.Find("NoteController").GetComponent<NoteController>().unitsPerSecond;
	}

	float calcX() {
		return NoteView.calcX(scoreNote);
	}

	// Update is called once per frame
	void Update () {
		if (scoreNote == null) {
			return;
		}
		move();
		checkStatus();
		checkDestroy();
	}
	
	void move() {
		float x = calcX();
		if (scoreNote.result == ScoreNoteResult.FAIL) {
		} else if (scoreNote.result == ScoreNoteResult.PASS) {
		}
		transform.position = new Vector3(x, 0.0f, 0.0f);
	}

	void checkStatus() {
		GameObject idle;
		GameObject death;

		switch (scoreNote.scriptNote.type) {
		case NoteType.LEFT:
			idle = left_idle;
			death = left_death;
			break;
		case NoteType.RIGHT:
			idle = right_idle;
			death = right_death;
			break;
		case NoteType.UP:
			idle = up_idle;
			death = up_death;
			break;
		case NoteType.DOWN:
			idle = down_idle;
			death = down_death;
			break;
		case NoteType.TAP:
		default:
			idle = tap_idle;
			death = tap_death;
			break;
		}

		if (scoreNote.result == ScoreNoteResult.INCOMPLETE) {
			idle.SetActive(true);
			death.SetActive(false);
		} else if (scoreNote.result == ScoreNoteResult.PASS) {
			idle.SetActive(false);
			death.SetActive(true);
		} else {
			idle.SetActive(false);
			death.SetActive(false);
		}
	}

	void checkDestroy() {
		if (calcX() < -GameObject.Find("NoteController").GetComponent<NoteController>().spawnDistance()) {
			Destroy(gameObject);
		}
	}
}

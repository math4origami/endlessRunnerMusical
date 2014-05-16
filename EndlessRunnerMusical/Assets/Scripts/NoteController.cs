using UnityEngine;
using System.Collections;

public class NoteController : MonoBehaviour {
	private int nextNote = 0;

	public GameObject noteObject;
	public float unitsPerSecond;

	public float spawnDistance() {
		return GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize * 2;
	}

	// Update is called once per frame
	void Update () {
		ScoreController score = GameObject.Find("Score").GetComponent<ScoreController>();
		while (nextNote < score.notes.Count) {
			ScoreNote note = score.notes[nextNote];

			float x = NoteView.calcX(note);
			if (x < spawnDistance()) {
				GameObject newNote = Instantiate(noteObject) as GameObject;
				newNote.GetComponent<NoteView>().initWithScoreNote(note);

				nextNote++;
			} else {
				return;
			}
		}
	}
}

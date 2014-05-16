using UnityEngine;
using System.Collections;

public class NoteController : MonoBehaviour {
	private int nextNote = 0;

	public float spawnDistance;
	public float noteRate;
	public GameObject noteObject;

	// Update is called once per frame
	void Update () {
		ScoreController score = GameObject.Find("Score").GetComponent<ScoreController>();
		while (nextNote < score.notes.Count) {
			ScoreNote note = score.notes[nextNote];
			GameObject newNote = Instantiate(noteObject) as GameObject;
			newNote.GetComponent<NoteView>().initWithScoreNote(note);

			nextNote++;
		}
	}
}

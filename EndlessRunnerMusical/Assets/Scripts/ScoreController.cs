using UnityEngine;
using System.Collections.Generic;

public enum ScoreNoteResult {
	INCOMPLETE,
	FAIL,
	PASS
}

public class ScoreNote {
	public ScriptNote scriptNote;
	public ScoreNoteResult result;

	public ScoreNote(ScriptNote note_) {
		scriptNote = note_;
		result = ScoreNoteResult.INCOMPLETE;
	}

	public override string ToString ()
	{
		return scriptNote + " - " + result;
	}
}

public class ScoreController : MonoBehaviour {
	public List<ScoreNote> notes;
	public float registerThreshold;
	public float passThreshold;
	public GameObject playerControllerObj;

	private int currentNote = 0;

	public void initScoreWithScript(List<ScriptNote> scriptNotes) {
		notes = new List<ScoreNote>();
		foreach (ScriptNote scriptNote in scriptNotes) {
			notes.Add(new ScoreNote(scriptNote));
			Debug.Log(notes[notes.Count-1]);
		}
	}

	public void processNote(float seconds, NoteType direction) {
		ScoreNote note = getNote(seconds);
		if (note != null) {
			if (note.scriptNote.type != NoteType.TAP && direction == NoteType.TAP) {
				return;
			}
			if (note.scriptNote.interval(seconds) < passThreshold &&
			    note.scriptNote.type == direction) {
				note.result = ScoreNoteResult.PASS;
			} else {
				note.result = ScoreNoteResult.FAIL;

				PlayerController pc = (PlayerController)playerControllerObj.GetComponent(typeof(PlayerController));
				pc.deductLife();
			}
			Debug.Log(seconds + " " + note);
		} else {
			Debug.Log(seconds + " - ");
		}
	}

	public ScoreNote getNote(float seconds) {
		return getNote(seconds, false);
	}

	public ScoreNote getNote(float seconds, bool ignoreThreshold) {
		for (; currentNote < notes.Count; currentNote++) {
			ScoreNote note = notes[currentNote];
			if (note.result != ScoreNoteResult.INCOMPLETE) {
				continue;
			}

			float interval = note.scriptNote.beatInSeconds() - seconds;
			if (interval > registerThreshold && !ignoreThreshold) {
				return null;
			} else if (interval < -passThreshold) {
				note.result = ScoreNoteResult.FAIL;

				PlayerController pc = (PlayerController)playerControllerObj.GetComponent(typeof(PlayerController));
				pc.deductLife();

				Debug.Log(seconds + " " + note);
			} else {
				return note;
			}
		}
		//gameover? or reset.
		return null;
	}
}

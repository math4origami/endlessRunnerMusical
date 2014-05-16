﻿using UnityEngine;
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
	public float scriptBuffer;

	private int currentNote = 0;
	private float bufferedSeconds = 0;

	void Start() {
		notes = new List<ScoreNote>();
	}

	void Update() {
		if (bufferedSeconds < GameObject.Find("GameController").GetComponent<GameController>().gameTime() + scriptBuffer) {
			List<ScriptNote> bufferedNotes =  GameObject.Find("Script").GetComponent<ScriptController>().GetNotes(bufferedSeconds + 0.01f, bufferedSeconds + scriptBuffer);

			foreach (ScriptNote note in bufferedNotes) {
				notes.Add(new ScoreNote(note));
				Debug.Log(notes[notes.Count-1] + "\n");
			}

			bufferedSeconds = notes[notes.Count-1].scriptNote.beatInSeconds();
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
//			Debug.Log(seconds + " " + note);
		} else {
//			Debug.Log(seconds + " - ");
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

//				Debug.Log(seconds + " " + note);
			} else {
				return note;
			}
		}
		//gameover? or reset.
		return null;
	}
}

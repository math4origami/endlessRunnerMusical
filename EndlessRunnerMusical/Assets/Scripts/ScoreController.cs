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

	private int currentNote = 0;

	public void initScoreWithScript(List<ScriptNote> scriptNotes) {
		notes = new List<ScoreNote>();
		foreach (ScriptNote scriptNote in scriptNotes) {
			notes.Add(new ScoreNote(scriptNote));
		}
	}

	public void processNote(float seconds) {
		ScoreNote note = getNote(seconds);
		if (note != null) {
			if (note.scriptNote.interval(seconds) < passThreshold) {
				note.result = ScoreNoteResult.PASS;
			} else {
				note.result = ScoreNoteResult.FAIL;
			}
			Debug.Log(seconds + " " + note);
		} else {
			Debug.Log(seconds + " - ");
		}
	}

	public ScoreNote getNote(float seconds) {
		for (; currentNote < notes.Count; currentNote++) {
			ScoreNote note = notes[currentNote];
			if (note.result != ScoreNoteResult.INCOMPLETE) {
				continue;
			}

			float interval = note.scriptNote.beatInSeconds() - seconds;
			if (interval > registerThreshold) {
				return null;
			} else if (interval < -registerThreshold) {
				note.result = ScoreNoteResult.FAIL;
			} else {
				return note;
			}
		}
		
		return null;
	}
}

using UnityEngine;
using System.Collections.Generic;

public enum NoteType {
	TAP,
	UP,
	DOWN,
	LEFT,
	RIGHT
}

public class ScriptNote {
	public float beat;
	public NoteType type;
	public ScriptNote(int beat_, NoteType type_) {
		beat = beat_;
		type = type_;
	}

	public float beatInSeconds() {
		float bpm = GameObject.Find("Script").GetComponent<ScriptController>().bpm;
		return beat / bpm * ScriptController.SECONDS_PER_MINUTE;
	}

	public float interval(float seconds) {
		return Mathf.Abs(seconds - beatInSeconds());
	}

	public override string ToString() {
		return string.Format("{0,4}: {1}", beat, type);
	}
}

public class ScriptController : MonoBehaviour {

	public const float SECONDS_PER_MINUTE = 60.0f;
	public List<ScriptNote> notes;

	public float bpm;

	void Start() {
		notes = new List<ScriptNote>();
		notes.Add(new ScriptNote(4, NoteType.UP));
		notes.Add(new ScriptNote(8, NoteType.DOWN));
		notes.Add(new ScriptNote(12, NoteType.LEFT));
		notes.Add(new ScriptNote(16, NoteType.RIGHT));
		notes.Add(new ScriptNote(20, NoteType.UP));
		notes.Add(new ScriptNote(24, NoteType.DOWN));
		notes.Add(new ScriptNote(28, NoteType.LEFT));
		notes.Add(new ScriptNote(32, NoteType.RIGHT));
		notes.Add(new ScriptNote(36, NoteType.TAP));
		notes.Add(new ScriptNote(38, NoteType.TAP));
		notes.Add(new ScriptNote(40, NoteType.TAP));
		notes.Add(new ScriptNote(42, NoteType.TAP));

		GameObject.Find("Score").GetComponent<ScoreController>().initScoreWithScript(notes);
	}
}

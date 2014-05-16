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

	public ScriptNote(float beat_, NoteType type_) {
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

	private int beatsPerMeasure = 4;

	public const float SECONDS_PER_MINUTE = 60.0f;
	public List<ScriptNote> notes;

	public float bpm;
	public float noteTypeScaling;

	void Start() {
		notes = new List<ScriptNote>();
		notes.Add(new ScriptNote(8, NoteType.TAP));
		notes.Add(new ScriptNote(12, NoteType.TAP));
		for (int i=16; i<24; i+=2) {
			notes.Add(new ScriptNote(i, NoteType.TAP));
		}
		for (int i=24; i<28; i++) {
			notes.Add(new ScriptNote(i, NoteType.TAP));
		}

		GetComponent<AudioSource>().PlayDelayed(-GameObject.Find("GameController").GetComponent<GameController>().gameTime());
	}

	public List<ScriptNote> GetNotes(float from, float to) {
		GenerateNotes(to);

		List<ScriptNote> subset = new List<ScriptNote>();
		foreach (ScriptNote sn in notes) {
			if (sn.beatInSeconds() > from && sn.beatInSeconds() < to) {
				subset.Add(sn);
			}
		}

		return subset;
	}

	public void GenerateNotes(float to) {
		float lastSecond = 0;
		if (notes.Count >= 1) {
			lastSecond = notes[notes.Count-1].beatInSeconds();
		}
		for (; 
		     lastSecond < to; 
		     lastSecond = notes[notes.Count-1].beatInSeconds()) {
			float lastBeat = notes[notes.Count-1].beat + 1;
			float nextMeasure = Mathf.Ceil(lastBeat / beatsPerMeasure) * beatsPerMeasure;

			notes.Add(GenerateScriptNote(nextMeasure));
			notes.Add(GenerateScriptNote(nextMeasure+2));
		}
	}

	ScriptNote GenerateScriptNote(float beat) {
		return new ScriptNote(beat, GenerateNoteType(beat / bpm * ScriptController.SECONDS_PER_MINUTE));
	}

	NoteType GenerateNoteType(float time) {
		int different = Mathf.Clamp((int)(time / noteTypeScaling), 1, 5);
		Debug.Log(time + " ---- " + different);
		return (NoteType)(Random.Range(0, different));
	}
}

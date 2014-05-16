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
	public float noteIntervalScaling;

	void Start() {
		notes = new List<ScriptNote>();
		notes.Add(new ScriptNote(8, NoteType.TAP));
		notes.Add(new ScriptNote(12, NoteType.TAP));
		notes.Add(new ScriptNote(16, NoteType.UP));
		notes.Add(new ScriptNote(18, NoteType.DOWN));
		notes.Add(new ScriptNote(20, NoteType.UP));
		notes.Add(new ScriptNote(22, NoteType.DOWN));
		notes.Add(new ScriptNote(24, NoteType.TAP));
		notes.Add(new ScriptNote(25, NoteType.TAP));
		notes.Add(new ScriptNote(26, NoteType.TAP));
		notes.Add(new ScriptNote(27, NoteType.TAP));

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

			foreach (int i in GenerateScriptIntervals(nextMeasure / bpm * ScriptController.SECONDS_PER_MINUTE)) {
				notes.Add(GenerateScriptNote(nextMeasure + i));
			}
		}
	}

	List<float> GenerateScriptIntervals(float time) {
		int different = Mathf.Clamp ((int)(time / noteIntervalScaling), 2, 4);
		List<float> result = new List<float>();
		switch (Random.Range(0, different)) {
		case 0:
			result.Add(4);
			break;
		case 1:
			result.Add(0);
			break;
		case 2:
			result.Add(0);
			result.Add(2);
			break;
		case 3:
			result.Add(0);
			result.Add(1.5f);
			result.Add(2);
			break;
		}
		return result;
	}

	ScriptNote GenerateScriptNote(float beat) {
		return new ScriptNote(beat, GenerateNoteType(beat / bpm * ScriptController.SECONDS_PER_MINUTE));
	}

	NoteType GenerateNoteType(float time) {
		int different = Mathf.Clamp((int)(time / noteTypeScaling), 1, 5);
		return (NoteType)(Random.Range(0, different));
	}
}

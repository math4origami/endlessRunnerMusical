using UnityEngine;
using System.Collections;

public class NoteView : MonoBehaviour {

	private ScoreNote scoreNote;

	public GameObject note1;
	public GameObject note2;
	public GameObject note3;
	public GameObject note4;
	public GameObject note5;

	public GameObject[] notes;

	public void initWithScoreNote(ScoreNote note) {
		scoreNote = note;
		note1.SetActive(false);
		note2.SetActive(false);
		switch (note.scriptNote.type) {
		case NoteType.UP:
			note1.SetActive(true);
			break;
		default:
			note2.SetActive(true);
			break;
		}
//		note3.SetActive(false);
//		note4.SetActive(false);
//		note5.SetActive(false);

	}

	// Use this for initialization
	void Start () {

	}

	float calcX() {
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		float gameTime = gc.gameTime();

		return (scoreNote.scriptNote.beatInSeconds() - gameTime) * gc.PIXELS_PER_SECOND;
	}
	
	// Update is called once per frame
	void Update () {
		float x = calcX();
		transform.position = new Vector3(x, 0.0f, 0.0f);
	}
}

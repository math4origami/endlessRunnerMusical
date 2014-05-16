using UnityEngine;
using System.Collections;

public class NoteView : MonoBehaviour {

	private ScoreNote scoreNote;

//	public GameObject note1;
//	public GameObject note2;
//	public GameObject note3;
//	public GameObject note4;
//	public GameObject note5;

//	public GameObject[] notes;

	public void initWithScoreNote(ScoreNote note) {

		scoreNote = note;
//		note1.SetActive(false);
//		note2.SetActive(false);
//		switch (note.scriptNote.type) {
//		case NoteType.UP:
//			note1.SetActive(true);
//			break;
//		default:
//			note2.SetActive(true);
//			break;
//		}
//		note3.SetActive(false);
//		note4.SetActive(false);
//		note5.SetActive(false);

//		activeClip.getFramesCount() 
//		activeClip.gotoAndPlay((uint)frame);

	}

	// Use this for initialization
	void Start () {
		float gameTime = GameObject.Find("GameController").GetComponent<GameController>().gameTime();
//		GetComponentInChildren<Animator>().playbackTime = gameTime - Mathf.Floor(gameTime);
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
		setStart();
		move();
		checkStatus();
		checkDestroy();
	}
	
//	bool firstDone = false;
//	uint firstFrame;
//	bool done = false;
	void setStart() {
//		if (!done) {
//			try {
//				float gameTime = GameObject.Find("GameController").GetComponent<GameController>().gameTime();
//				float frames = (gameTime * (float)activeClip.settings.targetFPS);
//				while (frames < 0) {
//					frames += activeClip.getFramesCount();
//				}
//				uint frame = (uint) (frames % activeClip.getFramesCount());
//				if (!firstDone) {
//					firstDone = true;
//					firstFrame = frame;
//				} else if (frame == firstFrame + 1) {
//					activeClip.gotoAndPlay(frame + 1);
//					Debug.Log(frame);
//					done = true;
//				}
//			} catch (System.Exception e) {
//				
//			}
//		}
	}

	void move() {
		float x = calcX();
		float y = 0.0f;
		if (scoreNote.result == ScoreNoteResult.FAIL) {
//			y = -1.0f;
		} else if (scoreNote.result == ScoreNoteResult.PASS) {
//			y = 1.0f;
		}
		transform.position = new Vector3(x, y, 0.0f);
	}

	void checkStatus() {

	}

	void checkDestroy() {
		if (calcX() < -GameObject.Find("NoteController").GetComponent<NoteController>().spawnDistance()) {
			Destroy(gameObject);
		}
	}
}

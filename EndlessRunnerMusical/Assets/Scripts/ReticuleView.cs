using UnityEngine;
using System.Collections;

public class ReticuleView : MonoBehaviour {

	public float baseAlpha;
	public float maxAlpha;
	public float alphaDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		ScoreController score = GameObject.Find("Score").GetComponent<ScoreController>();

		ScoreNote note = score.getNote(gc.gameTime(), true);
		float alpha = baseAlpha;
		Color color = GetComponent<SpriteRenderer>().color;

		if (note != null) {
			float interval = note.scriptNote.interval(gc.gameTime());
			
			if (interval < alphaDistance) {
				alpha += (maxAlpha - baseAlpha) * (alphaDistance - interval) / alphaDistance;
			}
		}

		color.a = alpha;
		GetComponent<SpriteRenderer>().color = color;
	}
}
